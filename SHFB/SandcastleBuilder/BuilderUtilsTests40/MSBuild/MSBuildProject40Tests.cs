using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Build.Evaluation;

using NUnit.Framework;

using BuildPropertyGroup = Microsoft.Build.BuildEngine.BuildPropertyGroup;
using System.IO;

namespace SandcastleBuilder.Utils.Tests.MSBuild
{
    [TestFixture]
    public class MSBuildProject40Tests
    {
        public const string Platform = "Platform";

        private Project GetProject1()
        {
            var project = new Project(
                @"Data\Project1.proj",
                null,
                null,
                ProjectCollection.GlobalProjectCollection);

            return project;
        }
        private Project GetProject2()
        {
            var project = new Project(
                @"Data\Project2.proj",
                null,
                null,
                ProjectCollection.GlobalProjectCollection);

            return project;
        }

        [Test]
        public void Project_ConditionedProperties_ReturnsPropertyValues()
        {
            Project project = GetProject1();

            // equivalent to MSBuild 3.5: Project.GetConditionedPropertyValues(string);
            var property = project.ConditionedProperties[Platform];

            Assert.IsNotNull(property);
            Assert.AreEqual(2, property.Count);
        }

        [Test(Description = "Tests API equivalent to Project.EvaluatedProperties from MSBuild 3.5.")]
        public void Project_EvaluatedProperties_ReturnsBuildPropertyGroup()
        {
            Project project = GetProject1();

            var properties = project.AllEvaluatedProperties;
            var guidProperty = project.GetPropertyValue("ProjectGuid");
            var assemblyProperty = project.GetPropertyValue("AssemblyName");

            Assert.IsNotNull(properties);
            Assert.LessOrEqual(261, properties.Count);

            Assert.IsNotNull(guidProperty);
            Assert.AreEqual("{F5362C80-44E3-47E4-AAFD-49050B22B550}", guidProperty);

            Assert.IsNotNull(assemblyProperty);
            Assert.AreEqual("SandcastleBuilderGUI", assemblyProperty);
        }

        /// <summary>
        /// Old code with BuildPropertyGroup returned from Project.EvaluatedProperties
        /// should be replaced with call to ReevaluateIfNecessary()
        /// and properties' values should be loaded directly from new Project object
        /// instead of the BuildPropertyGroup.
        /// </summary>
        [Test(Description = "Tests API equivalent to Project.EvaluatedProperties from MSBuild 3.5.")]
        public void Project_ReleaseConfiguation_ReturnsCorrectDebugType()
        {
            Project project = GetProject1();
            project.SetGlobalProperty("Configuration", "Release");
            project.ReevaluateIfNecessary();

            var debugType = project.GetPropertyValue("DebugType");

            Assert.IsNotNull(debugType);
            Assert.AreEqual("pdbonly", debugType);
        }

        [Test]
        public void Project_GetEvaluatedItemsByName_ReturnsBuildItems()
        {
            Project project = GetProject1();
            
            var references = project.GetItems("Reference");
            var comReferences = project.GetItems("COMReference");
            var projectReferences = project.GetItems("ProjectReference");

            Assert.AreEqual(11, references.Count);
            Assert.AreEqual(0, comReferences.Count);
            Assert.AreEqual(1, projectReferences.Count);

            var item = references.First();
            var meta = item.DirectMetadata;
            Assert.AreEqual(2, item.DirectMetadataCount);

            var item2 = references.Take(4).Last();
            var meta2 = item2.DirectMetadata;
            Assert.AreEqual(1, item2.DirectMetadataCount);

            var item3 = projectReferences.First();
            var meta3 = item3.DirectMetadata;
            Assert.AreEqual(2, item3.DirectMetadataCount);
        }

        [Test]
        public void Project_AddItem_ReturnedListContainsObjectAsFirstElement()
        {
            Project project = new Project();

            var metadata = new Dictionary<string, string> {
                { "HintPath", "Assembly123.dll" }
            };

            var newItemList = project.AddItem(
                "Reference",
                "Assembly123",
                metadata);

            Assert.IsNotNull(newItemList[0]);

            var newItem = newItemList[0];
            Assert.AreEqual("Reference", newItem.ItemType);
            Assert.AreEqual("Assembly123", newItem.EvaluatedInclude);
            Assert.IsTrue(newItem.HasMetadata("HintPath"));
        }

        [Test]
        public void Project_SetProperty_Condition_SetsConditionOnNewProperty()
        {
            Project project = new Project();
            var prop = project.SetProperty("MojaVlastnost", "Hodnota123");
            prop.Xml.Condition = " '$(MojaPodmienka)' == '123' ";

            using (var writer = new StringWriter())
            {
                project.Save(writer);

                var xml = writer.ToString();

                StringAssert.Contains(
                    @"<MojaVlastnost Condition="" '$(MojaPodmienka)' == '123' "">" +
                    @"Hodnota123</MojaVlastnost>",
                    xml);
            }
        }
    }
}
