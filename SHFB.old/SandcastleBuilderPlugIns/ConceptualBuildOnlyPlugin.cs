using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SandcastleBuilder.Utils.PlugIn;
using SandcastleBuilder.Utils.BuildEngine;

namespace SandcastleBuilder.PlugIns
{
	public class ConceptualBuildOnlyPlugin : IPlugIn
	{
		private ExecutionPointCollection executionPoints;

		#region IPlugIn Members

		public string Name
		{
			get { return "Conceptual Build Only"; }
		}

		public Version Version
		{
			get { return new Version(1, 0); }
		}

		public string Copyright
		{
			get { return "2009 (c) Jozef Izso"; }
		}

		public string Description
		{
			get { return "Skips all build steps and runs only conceptual build."; }
		}

		public bool RunsInPartialBuild
		{
			get { return true; }
		}

		public ExecutionPointCollection ExecutionPoints
		{
			get
			{
				if (executionPoints == null)
				{
					executionPoints = new ExecutionPointCollection();

					var steps = Enum.GetValues(typeof(BuildStep));

					foreach(var s in steps)
					{
						BuildStep step = (BuildStep)s;

						if (step == BuildStep.BuildConceptualTopics ||
							step == BuildStep.None ||
							step == BuildStep.Initializing ||
							step == BuildStep.Canceled ||
							step == BuildStep.Failed ||
							step == BuildStep.FindingTools ||
							step == BuildStep.Completed)
							continue;

						executionPoints.Add(new ExecutionPoint(step, ExecutionBehaviors.InsteadOf));
					}
				}

				return executionPoints;
			}
		}

		public string ConfigurePlugIn(SandcastleBuilder.Utils.SandcastleProject project, string currentConfig)
		{
			return currentConfig;
		}

		public void Initialize(SandcastleBuilder.Utils.BuildEngine.BuildProcess buildProcess, System.Xml.XPath.XPathNavigator configuration)
		{
			
		}

		public void Execute(ExecutionContext context)
		{
			
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			
		}

		#endregion
	}
}
