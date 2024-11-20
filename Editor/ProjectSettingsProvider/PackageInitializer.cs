using AceLand.Input.ProjectSetting;
using UnityEditor;

namespace AceLand.Input.Editor.ProjectSettingsProvider
{
    [InitializeOnLoad]
    public static class PackageInitializer
    {
        static PackageInitializer()
        {
            AceLandInputSettings.GetSerializedSettings();
        }
    }
}