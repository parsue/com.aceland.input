using AceLand.Input.PlayerLoopSystems;
using UnityEngine;

namespace AceLand.Input
{
    internal static class InputBootstrapper
    {
        private static InputManager _inputManager;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialization()
        {
            _inputManager = new InputManager();
            _inputManager.Initialize();
        }
    }
}
