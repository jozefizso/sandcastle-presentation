using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Build.BuildEngine;

using NUnit.Framework;

namespace SandcastleBuilder.Utils.Tests.MSBuild
{
    [TestFixture]
    public class MSBuildProject35Tests
    {
        public const string Platform = "Platform";

        [Test]
        public void Project_GetConditionedPropertyValues_ReturnsPropertyValues()
        {
            Project project = new Project(Engine.GlobalEngine);
            project.Load(@"Data\Project1.proj");

            var property = project.GetConditionedPropertyValues(Platform);

            Assert.IsNotNull(property);
            Assert.AreEqual(2, property.Length);
        }

        [Test]
        public void Project_EvaluatedProperties_ReturnsBuildPropertyGroup()
        {
            Project project = new Project(Engine.GlobalEngine);
            project.Load(@"Data\Project1.proj");
            
            BuildPropertyGroup properties = project.EvaluatedProperties;
            var guidProperty = properties["ProjectGuid"];
            var assemblyProperty = properties["AssemblyName"];
            var debugTypeProperty = properties["DebugType"];

            Assert.IsNotNull(properties);

            Assert.IsNotNull(guidProperty);
            Assert.AreEqual("{F5362C80-44E3-47E4-AAFD-49050B22B550}", guidProperty.FinalValue);

            Assert.IsNotNull(assemblyProperty);
            Assert.AreEqual("SandcastleBuilderGUI", assemblyProperty.FinalValue);

            Assert.IsNotNull(debugTypeProperty);
            Assert.AreEqual("full", debugTypeProperty.FinalValue);
        }

        [Test]
        public void Project_ReleaseConfiguation_ReturnsCorrectDebugType()
        {
            Project project = new Project(Engine.GlobalEngine);
            project.Load(@"Data\Project1.proj");
            project.GlobalProperties.SetProperty("Configuration", "Release");

            BuildPropertyGroup properties = project.EvaluatedProperties;
            var debugTypeProperty = properties["DebugType"];

            Assert.IsNotNull(debugTypeProperty);
            Assert.AreEqual("pdbonly", debugTypeProperty.FinalValue);
        }

        [Test]
        public void Project_ReleaseConfiguationAfterPropertyEvaluation_ReturnsIncorrectDebugType()
        {
            Project project = new Project(Engine.GlobalEngine);
            project.Load(@"Data\Project1.proj");

            BuildPropertyGroup properties = project.EvaluatedProperties;
            project.GlobalProperties.SetProperty("Configuration", "Release");

            var debugTypeProperty = properties["DebugType"];

            Assert.IsNotNull(debugTypeProperty);
            Assert.AreNotEqual("pdbonly", debugTypeProperty.FinalValue);
        }

        [Test]
        public void Project_GetEvaluatedItemsByName_ReturnsBuildItems()
        {
            Project project = new Project(Engine.GlobalEngine);
            project.Load(@"Data\Project1.proj");

            var references = project.GetEvaluatedItemsByName("Reference");
            var comReferences = project.GetEvaluatedItemsByName("COMReference");
            var projectReferences = project.GetEvaluatedItemsByName("ProjectReference");

            Assert.AreEqual(11, references.Count);
            Assert.AreEqual(0, comReferences.Count);
            Assert.AreEqual(1, projectReferences.Count);

            var item = references[0];
            var meta = item.CustomMetadataNames;
            Assert.AreEqual(2, meta.Count);

            var item2 = references[3];
            var meta2 = item2.CustomMetadataNames;
            Assert.AreEqual(1, meta2.Count);
        }
    }
}
