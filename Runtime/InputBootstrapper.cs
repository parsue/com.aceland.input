using AceLand.Input.ProjectSetting;
using UnityEngine;

namespace AceLand.Input
{
    internal static class InputBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialization()
        {
            InputHelper.Settings = Resources.Load<AceLandInputSettings>(nameof(AceLandInputSettings));
            InputHelper.InputManager = new();
            InputHelper.InputManager.Initialize();
        }
    }
}