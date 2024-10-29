using System.Linq;
using AceLand.Input.Inputs;
using AceLand.Input.ProjectSetting;
using AceLand.Input.State;
using AceLand.Library.Disposable;
using AceLand.PlayerLoopHack;
using AceLand.TaskUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

namespace AceLand.Input.PlayerLoopSystems
{
    internal class InputManager : DisposableObject, IPlayerLoopSystem
    {
        private static AceLandInputSettings Settings => InputHelper.Settings;
        private static PlayerLoopSystem _playerLoopSystem;

        public static Vector2 WinMousePosition => Mouse.current.position.ReadValue();
        public static Vector2 WinMouseDelta => WinMousePosition - _lastMousePosition;
        public static bool OverrideUserInput;

        public static BtnStatus GetButtonStatus(string name) =>
            _buttonInput.GetStatus(name);
        
        public static void SetBtnStatus(string name, BtnState state) => 
            _buttonInput.ForceBtnState(name, state);

        private static InputActionMap _actionButtonInput;
        private static InputActionMap _actionAxisInput;
        private static InputActionMap _actionAxis2Input;
        
        private static ButtonInput _buttonInput;
        internal static AxisInput AxisInputSystem;
        internal static Axis2Input Axis2InputSystem;

        private static Vector2 _lastMousePosition = Vector2.zero;

        internal void Initialize()
        {
            _buttonInput = new ButtonInput();
            AxisInputSystem = new AxisInput();
            Axis2InputSystem = new Axis2Input();
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
            _playerLoopSystem.InsertSystem(Settings.managerLoopType);
            TaskHelper.AddApplicationQuitListener(Stop);
        }

        private void Stop()
        {
            OnStop();
            _playerLoopSystem.RemoveSystem(Settings.managerLoopType);
        }
        
        public void SystemUpdate()
        {
            UpdateMousePosition();
        }

        private void OnStart()
        {
            _actionButtonInput.Enable();
            _actionAxisInput.Enable();
            _actionAxis2Input.Enable();
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

        private static void SetCursor()
        {
            Cursor.visible = Settings.showWinCursor;
            Cursor.lockState = Settings.lockMode;
        }

        public static void ShowCursor(bool show)
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

        public static void LockCursor(bool lockCursor, CursorLockMode unlockMode = CursorLockMode.Confined)
        {
            Cursor.lockState = lockCursor switch
            {
                true => CursorLockMode.Locked,
                false => unlockMode,
            };
        }

        private void SetButtonInput()
        {
            if (!Settings.handleButtonInput) return;
            _actionButtonInput = Settings.actionAsset.actionMaps
                .First(a => a.name == Settings.buttonActionMapName);
            _buttonInput.SetActions(_actionButtonInput);
        }

        private void SetAxisInput()
        {
            if (!Settings.handleAxisInput) return;
            _actionAxisInput = Settings.actionAsset.actionMaps
                .First(a => a.name == Settings.axisActionMapName);
            AxisInputSystem.SetActions(_actionAxisInput);
        }

        private void SetAxis2Input()
        {
            if (!Settings.handleAxis2Input) return;
            _actionAxis2Input = Settings.actionAsset.actionMaps
                .First(a => a.name == Settings.axis2ActionMapName);
            Axis2InputSystem.SetActions(_actionAxis2Input);
        }
    }
}
