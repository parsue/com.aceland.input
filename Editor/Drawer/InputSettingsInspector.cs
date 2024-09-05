using AceLand.Input.ProjectSetting;
using AceLand.Library.Editor;
using UnityEditor;

namespace AceLand.Input.Editor.Drawer
{
    [CustomEditor(typeof(AceLandInputSettings))]
    public class InputSettingsInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorHelper.DrawAllPropertiesAsDisabled(serializedObject);
        }
    }
}