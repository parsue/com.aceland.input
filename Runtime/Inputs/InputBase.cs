using AceLand.Input.ProjectSetting;
using UnityEngine.InputSystem;
using InputSettings = UnityEngine.InputSystem.InputSettings;

namespace AceLand.Input.Inputs
{
    internal abstract class InputBase
    {
        private protected static AceLandInputSettings Settings => AceInput.Settings;
        private protected readonly InputSettings InputSettings = InputSystem.settings;

        internal void Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {
            // noop
        }

        protected virtual void ResetInputStateCycle()
        {
            // noop
        }

        protected virtual void ResetState()
        {
            // noop
        }
    }
}
