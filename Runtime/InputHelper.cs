using AceLand.Input.PlayerLoopSystems;
using AceLand.Input.ProjectSetting;
using AceLand.Input.State;
using AceLand.Library.Utils;
using UnityEngine;

namespace AceLand.Input
{
    public static class InputHelper
    {
        public static AceLandInputSettings Settings { get; internal set; }

        public static Vector2 WinMousePosition => InputManager.WinMousePosition;
        public static Vector2 WinMouseDelta => InputManager.WinMouseDelta;
        public static bool IsOverUI => Helper.IsOverUIElement(WinMousePosition);

        public static void SetBtnStatus(string name, BtnState state) =>
            InputManager.SetBtnStatus(name, state);

        public static void LockCursor(bool lockCursor, CursorLockMode unlockMode = CursorLockMode.Confined) =>
            InputManager.LockCursor(lockCursor, unlockMode);

        public static void ShowCursor(bool show) =>
            InputManager.ShowCursor(show);
    }
}
