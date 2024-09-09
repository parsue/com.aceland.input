using System.Linq;
using AceLand.Input.Inputs;
using AceLand.Input.ProjectSetting;
using AceLand.Library.Disposable;
using AceLand.Library.Utils;
using AceLand.PlayerLoopHack;
using AceLand.TasksUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace AceLand.Input.PlayerLoopSystems
{
    public class InputManager : DisposableObject, IPlayerLoopSystem
    {
        private static AceLandInputSettings Settings => InputHelper.Settings;
        private static PlayerLoopSystem _playerLoopSystem;

        public static Vector2 WinMousePosition => Mouse.current.position.ReadValue();
        public static Vector2 WinMouseDelta => WinMousePosition - _lastMousePosition;
        public static bool IsOverUI => Helper.IsOverUIElement(WinMousePosition);
        public static bool OverrideUserInput;

        private static InputActionMap _actionButtonInput;
        private static InputActionMap _actionAxisInput;
        private static InputActionMap _actionAxis2Input;
        
        private static ButtonInput _buttonInput;
        internal static AxisInput AxisInputSystem;
        internal static Axis2Input Axis2InputSystem;

        private static Vector2 _lastMousePosition = Vector2.zero;

        internal void Initialize()
        {
            if (Settings.actionAsset == null) return;

            _buttonInput = new();
            AxisInputSystem = new();
            Axis2InputSystem = new();
            _buttonInput.Init();
            AxisInputSystem.Init();
            Axis2InputSystem.Init();
            SetCursor();
            SetButtonInput();
            SetAxisInput();
            SetAxis2Input();

            Start();
        }
        
        private void Start()
        {
            OnStart();
            _playerLoopSystem = this.CreatePlayerLoopSystem();
            _playerLoopSystem.InsertSystem<TimeUpdate>();
            TaskHandler.AddApplicationQuitListener(Stop);
        }

        private void Stop()
        {
            OnStop();
            _playerLoopSystem.RemoveSystem<TimeUpdate>();
        }
        
        public void SystemUpdate()
        {
            UpdateMousePosition();
        }

        private void OnStart()
        {
            _actionButtonInput?.Enable();
            _actionAxisInput?.Enable();
            _actionAxis2Input?.Enable();
        }

        private void OnStop()
        {
            Cursor.visible = Settings.showWinCursor;
            _actionButtonInput?.Disable();
            _actionAxisInput?.Disable();
            _actionAxis2Input?.Disable();
        }

        private void UpdateMousePosition()
        {
            _lastMousePosition = WinMousePosition;
        }

        private void SetCursor()
        {
            Cursor.visible = Settings.showWinCursor;
            Cursor.lockState = Settings.lockMode;
        }

        public void ShowCursor(bool show)
        {
            if (show)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Debug.Log("Show Cursors");
            }
            else
            {
                SetCursor();
                Debug.Log("Set Default Cursors");
            }
        }

        private void SetAxisInput()
        {
            if (!Settings.handleAxisInput) return;
            _actionAxisInput = Settings.actionAsset.actionMaps.First(a => a.name == Settings.axisActionMapName);
            AxisInputSystem.SetActions(_actionAxisInput);
        }

        private void SetAxis2Input()
        {
            if (!Settings.handleAxis2Input) return;
            _actionAxis2Input = Settings.actionAsset.actionMaps.First(a => a.name == Settings.axis2ActionMapName);
            Axis2InputSystem.SetActions(_actionAxis2Input);
        }

        private void SetButtonInput()
        {
            if (!Settings.handleButtonInput) return;
            _actionButtonInput = Settings.actionAsset.actionMaps.First(a => a.name == Settings.buttonActionMapName);
            _buttonInput.SetActions(_actionButtonInput);
        }

        public static void LockCursor(bool lockCursor, CursorLockMode unlockMode = CursorLockMode.Confined)
        {
            Cursor.lockState = lockCursor switch
            {
                true => CursorLockMode.Locked,
                false => unlockMode,
            };
        }
    }
}