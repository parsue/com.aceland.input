using AceLand.Input.PlayerLoopSystems;
using AceLand.Input.ProjectSetting;

namespace AceLand.Input
{
    public static class InputHelper
    {
        public static AceLandInputSettings Settings { get; internal set; }
        public static InputManager InputManager { get; internal set; }
    }
}