using AceLand.Input.ProjectSetting;
using AceLand.Library.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace AceLand.Input.Editor.ProjectSettingsProvider
{
    public class InputSettingsProvider : SettingsProvider
    {
        public const string SETTINGS_NAME = "Project/AceLand Input";
        private SerializedObject _settings;
        
        [InitializeOnLoadMethod]
        public static void CreateSettings() => AceLandInputSettings.GetSerializedSettings();
        
        private InputSettingsProvider(string path, SettingsScope scope = SettingsScope.User) 
            : base(path, scope) { }
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = AceLandInputSettings.GetSerializedSettings();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new InputSettingsProvider(SETTINGS_NAME, SettingsScope.Project);
            return provider;
        }

        public override void OnGUI(string searchContext)
        {
            EditorHelper.DrawAllProperties(_settings);
        }
    }
}