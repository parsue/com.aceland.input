using AceLand.Input.ProjectSetting;
using AceLand.Library.Editor.Providers;
using UnityEditor;
using UnityEngine.UIElements;

namespace AceLand.Input.Editor.ProjectSettingsProvider
{
    public class InputSettingsProvider : AceLandSettingsProvider
    {
        public const string SETTINGS_NAME = "Project/AceLand Input";
        
        [InitializeOnLoadMethod]
        public static void CreateSettings() => AceLandInputSettings.GetSerializedSettings();
        
        private InputSettingsProvider(string path, SettingsScope scope = SettingsScope.User) 
            : base(path, scope) { }
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            Settings = AceLandInputSettings.GetSerializedSettings();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new InputSettingsProvider(SETTINGS_NAME, SettingsScope.Project);
            return provider;
        }
    }
}