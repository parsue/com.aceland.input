using AceLand.Input.PlayerLoopSystems;
using AceLand.Input.ProjectSetting;
using AceLand.Input.State;
using AceLand.Library.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AceLand.Input
{
    public static class InputHelper
    {
        public static AceLandInputSettings Settings
        {
            get => _settings ?? Resources.Load<AceLandInputSettings>(nameof(AceLandInputSettings));
            internal set => _settings = value;
        }
        
        private static AceLandInputSettings _settings;

        public static Vector2 WinMousePosition => InputManager.WinMousePosition;
        public static Vector2 WinMouseDelta => InputManager.WinMouseDelta;
        public static bool IsOverUI => Helper.IsOverUIElement(WinMousePosition);

        public static BtnStatus GetBtnStatus(string name) =>
            InputManager.GetButtonStatus(name);

        public static InputAction GetAction(string name) =>
            Settings.actionAsset.FindAction(name);
        
        public static void SetBtnStatus(string name, BtnState state) =>
            InputManager.SetBtnStatus(name, state);

        public static void LockCursor(bool lockCursor, CursorLockMode unlockMode = CursorLockMode.Confined) =>
            InputManager.LockCursor(lockCursor, unlockMode);

        public static void ShowCursor(bool show) =>
            InputManager.ShowCursor(show);
    }
}
