using AceLand.Input.State;
using AceLand.Library.Attribute;
using AceLand.Library.ProjectSetting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AceLand.Input.ProjectSetting
{
    public class AceLandInputSettings : ProjectSettings<AceLandInputSettings>
    {
        [Header("Input Actions")]
        public InputActionAsset actionAsset;
        public char keyNameSeparateChar = '_';
        
        [Header("Cursor")]
        public bool showWinCursor = true;
        public CursorLockMode lockMode = CursorLockMode.None;
        public bool showGameCursor;
        public bool showOnClickFX;

        [Header("Button Input")]
        public bool handleButtonInput = true;
        [ConditionalShow("handleButtonInput")]
        public string buttonActionMapName = "ButtonInput";
        [ConditionalShow("handleButtonInput")]
        public ReleaseHandlingType releaseType;
        [ConditionalShow("handleButtonInput")]
        public bool enableQuitKey;
        [ConditionalShow("handleButtonInput", "enableQuitKey")]
        public string quitKey = "Quit";
        [ConditionalShow("handleButtonInput", "enableQuitKey")]
        public bool disableQuitKeyInEditor;
        [ConditionalShow("handleButtonInput", "enableQuitKey")]
        public bool disableQuitKeyInDevBuild;
        [ConditionalShow("handleButtonInput", "enableQuitKey")]
        public bool disableQuitKeyInRuntime;
        [ConditionalShow("handleButtonInput")]
        public bool enableReloadKey;
        [ConditionalShow("handleButtonInput", "enableReloadKey")]
        public string reloadKey = "Reload";
        [ConditionalShow("handleButtonInput", "enableReloadKey")]
        public bool disableReloadKeyInEditor;
        [ConditionalShow("handleButtonInput", "enableReloadKey")]
        public bool disableReloadKeyInDevBuild;
        [ConditionalShow("handleButtonInput", "enableReloadKey")]
        public bool disableReloadKeyInRuntime;

        [Header("Axis Input")]
        public bool handleAxisInput;
        [ConditionalShow("handleAxisInput")]
        public string axisActionMapName = "AxisInput";
        
        [Header("Axis2 Input")]
        public bool handleAxis2Input;
        [ConditionalShow("handleAxis2Input")]
        public string axis2ActionMapName = "Axis2Input";
    }
}