﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SandcastleBuilder.Gui.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Eric@EWoodruff.us")]
        public string AuthorEMailAddress {
            get {
                return ((string)(this["AuthorEMailAddress"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection MruList {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["MruList"]));
            }
            set {
                this["MruList"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SettingsUpgraded {
            get {
                return ((bool)(this["SettingsUpgraded"]));
            }
            set {
                this["SettingsUpgraded"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.EWoodruff.us")]
        public string EWoodruffURL {
            get {
                return ((string)(this["EWoodruffURL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://SHFB.CodePlex.com")]
        public string ProjectURL {
            get {
                return ((string)(this["ProjectURL"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool VerboseLogging {
            get {
                return ((bool)(this["VerboseLogging"]));
            }
            set {
                this["VerboseLogging"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string HTMLHelp2ViewerPath {
            get {
                return ((string)(this["HTMLHelp2ViewerPath"]));
            }
            set {
                this["HTMLHelp2ViewerPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("12345")]
        public int ASPNETDevServerPort {
            get {
                return ((int)(this["ASPNETDevServerPort"]));
            }
            set {
                this["ASPNETDevServerPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::SandcastleBuilder.Utils.Design.ContentFileEditorCollection ContentFileEditors {
            get {
                return ((global::SandcastleBuilder.Utils.Design.ContentFileEditorCollection)(this["ContentFileEditors"]));
            }
            set {
                this["ContentFileEditors"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<WINDOWPLACEMENT xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <length>0</length>
  <flags>0</flags>
  <showCmd>0</showCmd>
  <ptMinPosition_x>0</ptMinPosition_x>
  <ptMinPosition_y>0</ptMinPosition_y>
  <ptMaxPosition_x>0</ptMaxPosition_x>
  <ptMaxPosition_y>0</ptMaxPosition_y>
  <rcNormalPosition_left>0</rcNormalPosition_left>
  <rcNormalPosition_top>0</rcNormalPosition_top>
  <rcNormalPosition_right>0</rcNormalPosition_right>
  <rcNormalPosition_bottom>0</rcNormalPosition_bottom>
</WINDOWPLACEMENT>")]
        public global::SandcastleBuilder.Gui.WINDOWPLACEMENT WindowPlacement {
            get {
                return ((global::SandcastleBuilder.Gui.WINDOWPLACEMENT)(this["WindowPlacement"]));
            }
            set {
                this["WindowPlacement"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool OpenHelpAfterBuild {
            get {
                return ((bool)(this["OpenHelpAfterBuild"]));
            }
            set {
                this["OpenHelpAfterBuild"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Debug")]
        public string LastConfig {
            get {
                return ((string)(this["LastConfig"]));
            }
            set {
                this["LastConfig"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("AnyCPU")]
        public string LastPlatform {
            get {
                return ((string)(this["LastPlatform"]));
            }
            set {
                this["LastPlatform"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ContentEditorDockState {
            get {
                return ((string)(this["ContentEditorDockState"]));
            }
            set {
                this["ContentEditorDockState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Window")]
        public global::System.Drawing.Color BuildOutputBackground {
            get {
                return ((global::System.Drawing.Color)(this["BuildOutputBackground"]));
            }
            set {
                this["BuildOutputBackground"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WindowText")]
        public global::System.Drawing.Color BuildOutputForeground {
            get {
                return ((global::System.Drawing.Color)(this["BuildOutputForeground"]));
            }
            set {
                this["BuildOutputForeground"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Courier New, 7.8pt")]
        public global::System.Drawing.Font BuildOutputFont {
            get {
                return ((global::System.Drawing.Font)(this["BuildOutputFont"]));
            }
            set {
                this["BuildOutputFont"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Courier New, 10.2pt")]
        public global::System.Drawing.Font TextEditorFont {
            get {
                return ((global::System.Drawing.Font)(this["TextEditorFont"]));
            }
            set {
                this["TextEditorFont"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SaveOpenDocuments")]
        public global::SandcastleBuilder.Gui.BeforeBuildAction BeforeBuild {
            get {
                return ((global::SandcastleBuilder.Gui.BeforeBuildAction)(this["BeforeBuild"]));
            }
            set {
                this["BeforeBuild"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ShowLineNumbers {
            get {
                return ((bool)(this["ShowLineNumbers"]));
            }
            set {
                this["ShowLineNumbers"] = value;
            }
        }
    }
}
