using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SandcastleBuilder.Utils.Strings
{
	/// <summary>
	/// String resources for <see cref="SandcastleProject"/>.
	/// </summary>
	public static class ProjectSR
	{

		public static BuilderException MissingSchemaVersionElement()
		{
			return CreateException(
				"PRJ0001",
				"Invalid or missing schema version element <SHFBSchemaVersion>.");
		}

		public static BuilderException UnsupportedSchemaVersion(Version projectVersion, Version supportedVersion)
		{
			return CreateException(
				"PRJ0002",
				"The selected file is for a more recent version of the help file builder (v{0}). "+
					"Please upgrade your copy to load the file. This project supports only schema v{1}.",
				projectVersion, 
				supportedVersion);
		}

		private static BuilderException CreateException(string prjNumber, string message)
		{
			return new BuilderException(prjNumber, message);
		}

		private static BuilderException CreateException(string prjNumber, string format, params object[] args)
		{
			string msg = String.Format(format, args);
			return CreateException(prjNumber, msg);
		}
	}
}
