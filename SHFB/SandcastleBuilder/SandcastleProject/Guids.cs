using System;

namespace Izsaknet.Sandcastle.VisualStudio
{
    static class GuidList
    {
        public const string SandcastleProjectPkgString = "59cf444c-3b9d-44f3-abf6-a412beed0684";
        public const string SandcastleProjectCmdSetString = "0cbb1931-5e92-4ccd-b401-bd15b9b3569f";
        public const string SandcastleProjectFactoryString = "DBF5A742-155F-495C-A065-F521896F407B";
        public const string GeneralPropertyPageString = "2C9A97ED-651D-4CAB-8257-922F57A02AE3";

        public static readonly Guid SandcastleProjectCmdSetGuid = new Guid(SandcastleProjectCmdSetString);
        public static readonly Guid SandcastleProjectFactoryGuid = new Guid(SandcastleProjectFactoryString);
    };
}
