using UnityEngine.InputSystem.EnhancedTouch;

namespace AceLand.Input.State
{
    public class TouchBtnStatus : BtnStatus
    {
        public Finger Finger { get; set; }

        public TouchBtnStatus()
        {
            Reset();
            Name = "";
        }
    }
}
