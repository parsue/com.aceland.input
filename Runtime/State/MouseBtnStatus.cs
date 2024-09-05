using UnityEngine;

namespace AceLand.Input.State
{
    public class MouseBtnStatus : BtnStatus
    {
        public Vector2 OnPressPosition { get; set; }
        public Vector2 OnReleasePosition { get; set; }

        public MouseBtnStatus()
        {
            Reset();
            Name = "";
        }

        protected sealed override void Reset()
        {
            base.Reset();
            OnPressPosition = Vector2.zero;
            OnReleasePosition = Vector2.zero;
        }

    }
}
