using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Izsaknet.Sandcastle.Tests
{
	public static class UnityBootstrapper
	{
		static UnityBootstrapper()
		{
			var unityConfig = (UnityConfigurationSection)
				ConfigurationManager.GetSection("unity");

			UnityContainer unity = new UnityContainer();
			unityConfig.Containers.Default.Configure(unity);

			Unity = unity;
		}

		public static IUnityContainer Unity { get; set; }
	}
}
