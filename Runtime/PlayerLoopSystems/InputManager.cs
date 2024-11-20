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
            SetCursor();

            if (!Settings.ActionAsset) return;
            if (Settings.ActionAsset.actionMaps.Count == 0) return;
            
            if (Settings.HandleButtonInput)
            {
                _buttonInput = new ButtonInput();
                _buttonInput.Init();
                SetButtonInput();
            }

            if (Settings.HandleAxisInput)
            {
                AxisInputSystem = new AxisInput();
                AxisInputSystem.Init();
                SetAxisInput();
            }

            if (Settings.HandleAxis2Input)
            {
                Axis2InputSystem = new Axis2Input();
                Axis2InputSystem.Init();
                SetAxis2Input();
            }

            Start();
        }
        
        private void Start()
        {
            OnStart();
            _playerLoopSystem = this.CreatePlayerLoopSystem();
            _playerLoopSystem.InjectSystem(Settings.ManagerLoopState);
            Promise.AddApplicationQuitListener(Stop);
        }

        private void Stop()
        {
            OnStop();
            _playerLoopSystem.RemoveSystem(Settings.ManagerLoopState);
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
            Cursor.visible = Settings.ShowWinCursor;
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
            Cursor.visible = Settings.ShowWinCursor;
            
            if (Settings.IsLockCursor)
                Cursor.lockState = Settings.LockMode;
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
            if (!Settings.HandleButtonInput) return;
            
            _actionButtonInput = Settings.ActionAsset.actionMaps
                .FirstOrDefault(a => a.name == Settings.ButtonActionMapName);
            if (_actionButtonInput is null) return;
            
            _buttonInput.SetActions(_actionButtonInput);
        }

        private void SetAxisInput()
        {
            if (!Settings.HandleAxisInput) return;
            
            _actionAxisInput = Settings.ActionAsset.actionMaps
                .FirstOrDefault(a => a.name == Settings.AxisActionMapName);
            if (_actionAxisInput is null) return;
            
            AxisInputSystem.SetActions(_actionAxisInput);
        }

        private void SetAxis2Input()
        {
            if (!Settings.HandleAxis2Input) return;
            
            _actionAxis2Input = Settings.ActionAsset.actionMaps
                .FirstOrDefault(a => a.name == Settings.Axis2ActionMapName);
            if (_actionAxis2Input is null) return;
            
            Axis2InputSystem.SetActions(_actionAxis2Input);
        }
    }
}
