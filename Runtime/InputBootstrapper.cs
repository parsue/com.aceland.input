using AceLand.Input.PlayerLoopSystems;
using AceLand.Input.ProjectSetting;
using UnityEngine;

namespace AceLand.Input
{
    internal static class InputBootstrapper
    {
        private static InputManager _inputManager;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialization()
        {
            InputHelper.Settings = Resources.Load<AceLandInputSettings>(nameof(AceLandInputSettings));
            _inputManager = new();
            _inputManager.Initialize();
        }
    }
}
