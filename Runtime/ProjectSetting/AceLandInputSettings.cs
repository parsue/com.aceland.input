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
        public PlayerLoopType managerLoopType = PlayerLoopType.TimeUpdate;
        public PlayerLoopType inputLoopType = PlayerLoopType.Initialization;
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
        public ReleaseHandlingType releaseType = ReleaseHandlingType.ReleasedOnly;
        [ConditionalShow("handleButtonInput")]
        public BuildLevel quitKeyLevel = BuildLevel.DevelopmentBuild;
        public string quitKey = "PlayerQuit";
        [ConditionalShow("handleButtonInput")]
        public BuildLevel reloadKeyLevel = BuildLevel.DevelopmentBuild;
        public string reloadKey = "PlayerReload";

        [Header("Axis Input")]
        public bool handleAxisInput = true;
        [ConditionalShow("handleAxisInput")]
        public string axisActionMapName = "AxisInput";
        [ConditionalShow("handleAxisInput")]
        [Range(-1, 1)] public float axisButtonThreshold = -0.6f;
        [ConditionalShow("handleAxisInput")]
        [Min(0)] public float axisButtonActionTime = 0.8f;
        
        [Header("Axis2 Input")]
        public bool handleAxis2Input = true;
        [ConditionalShow("handleAxis2Input")]
        public string axis2ActionMapName = "Axis2Input";
    }
}
