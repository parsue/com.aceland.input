using AceLand.Input.State;
using AceLand.Library.Attribute;
using AceLand.Library.BuildLeveling;
using AceLand.Library.ProjectSetting;
using AceLand.PlayerLoopHack;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AceLand.Input.ProjectSetting
{
    public class AceLandInputSettings : ProjectSettings<AceLandInputSettings>
    {
        [Header("Settings")]
        [SerializeField] private InputActionAsset actionAsset;
        [SerializeField] private char keyNameSeparateChar = '_';
        
        [Header("Cursor")]
        [SerializeField] private bool showWinCursor = true;
        [Tooltip("Invert Build Level")]
        [SerializeField] private BuildLevel cursorLockLevel; 
        [ConditionalShow("cursorLockLevel", true, BuildLevel.None)] 
        [SerializeField]private CursorLockMode lockMode;

        [Header("Button Input")]
        [SerializeField] private bool handleButtonInput = true;
        [ConditionalShow("handleButtonInput")]
        [SerializeField] private string buttonActionMapName = "ButtonInput";
        [ConditionalShow("handleButtonInput")]
        [SerializeField] private ReleaseHandlingType releaseType = ReleaseHandlingType.ReleasedOnly;
        [ConditionalShow("handleButtonInput")]
        [SerializeField] private BuildLevel quitKeyLevel = BuildLevel.Development;
        [ConditionalShow("handleButtonInput", "quitKeyLevel", true, BuildLevel.None)]
        [SerializeField] private string quitKey = "PlayerQuit";
        [ConditionalShow("handleButtonInput")]
        [SerializeField] private BuildLevel reloadKeyLevel = BuildLevel.Development;
        [ConditionalShow("handleButtonInput", "reloadKeyLevel", true, BuildLevel.None)]
        [SerializeField] private string reloadKey = "PlayerReload";
        
        [Header("Axis Input")]
        [SerializeField] private bool handleAxisInput = true;
        [ConditionalShow("handleAxisInput")]
        [SerializeField] private string axisActionMapName = "AxisInput";
        [ConditionalShow("handleAxisInput")]
        [SerializeField, Range(-1, 1)] private float axisButtonThreshold = -0.6f;
        [ConditionalShow("handleAxisInput")]
        [SerializeField, Min(0)] private float axisButtonActionTime = 0.8f;
        
        [Header("Axis2 Input")]
        [SerializeField] private bool handleAxis2Input = true;
        [ConditionalShow("handleAxis2Input")]
        [SerializeField] private string axis2ActionMapName = "Axis2Input";

        public InputActionAsset ActionAsset => actionAsset;
        public char KeyNameSeparateChar => keyNameSeparateChar;

        public bool ShowWinCursor => showWinCursor;
        public BuildLevel CursorLockLevel => cursorLockLevel;
        public bool IsLockCursor => cursorLockLevel.IsAcceptedLevelInvert();
        public CursorLockMode LockMode => lockMode;

        public bool HandleButtonInput => handleButtonInput;
        public string ButtonActionMapName => buttonActionMapName;
        public ReleaseHandlingType ReleaseType => releaseType;
        public BuildLevel QuitKeyLevel => quitKeyLevel;
        public string QuitKey => quitKey;
        public BuildLevel ReloadKeyLevel => reloadKeyLevel;
        public string ReloadKey => reloadKey; 

        public bool HandleAxisInput => handleAxisInput;
        public string AxisActionMapName => axisActionMapName;
        public float AxisButtonThreshold => axisButtonThreshold;
        public float AxisButtonActionTime => axisButtonActionTime;

        public bool HandleAxis2Input => handleAxis2Input;
        public string Axis2ActionMapName => axis2ActionMapName;
    }
}
