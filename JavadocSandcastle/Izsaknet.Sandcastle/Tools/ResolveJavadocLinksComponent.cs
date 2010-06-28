using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;


using Microsoft.Ddue.Tools;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using System.IO;

namespace Izsaknet.Sandcastle.Tools
{
	public class ResolveJavadocLinksComponent : ResolveReferenceLinksComponent2
	{
		private const string UnsupportedTypeValue = "Node <{0}> have unsupported value '{1}' in attribute {2}.";

		public ResolveJavadocLinksComponent(BuildAssembler assembler,
			XPathNavigator configuration)
			: base(assembler, configuration)
		{
		}

		protected override void ProcessExternalTargetNode(XPathNavigator targets_node)
		{
			string resolver = targets_node.GetAttribute("resolver", String.Empty);

			if (String.IsNullOrEmpty(resolver))
				WriteMessage(MessageLevel.Error, "External targets element must have attribute resolver specifying which external resolver to use.");

			string configFile = targets_node.GetAttribute("config", String.Empty);
			if (String.IsNullOrEmpty(configFile))
				WriteMessage(MessageLevel.Error, "External targets element must have attribute config specifying which file contains Unity configuration.");

			configFile = Environment.ExpandEnvironmentVariables(configFile);
			configFile = Path.GetFullPath(configFile);
			if (!File.Exists(configFile))
				WriteMessage(
					MessageLevel.Error,
					String.Format("Configuration file '{0}' does not exists.",configFile));

			this.ExternalResover = CreateResolver(resolver, configFile);
		}

		protected override Target GetTarget(string targetId)
		{
			Target target = base.GetTarget(targetId);

			if (target == null)
			{
				// Assume the target is from Java and create dummy Target object.

				TargetIdConverter parser = new TargetIdConverter(targetId);
				target = parser.ToTarget();
			}

			return target;
		}

		private IExternalReferenceResolver CreateResolver(string name, string configFile)
		{
			IExternalReferenceResolver resolver = null;

			try
			{
				ExeConfigurationFileMap map = new ExeConfigurationFileMap();
				map.ExeConfigFilename = configFile;
				var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				var unityConfig = (UnityConfigurationSection)config.GetSection("unity");

				if (unityConfig == null)
				{
					WriteMessage(MessageLevel.Error, "Missing <unity> configuration section in app.config file.");
					return null;
				}

				UnityContainer unity = new UnityContainer();
				unityConfig.Containers.Default.Configure(unity);

				resolver = unity.Resolve<IExternalReferenceResolver>(name);
				WriteMessage(MessageLevel.Info, String.Format("Created resolver '{0}'.", resolver.ResolverName));
			}
			catch (ResolutionFailedException ex)
			{
				WriteMessage(MessageLevel.Error, ex.Message);
			}
			catch (ConfigurationException ex)
			{
				WriteMessage(MessageLevel.Error, ex.Message);
			}

			return resolver;
		}
	}
}
