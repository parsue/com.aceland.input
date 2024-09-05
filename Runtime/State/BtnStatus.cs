namespace AceLand.Input.State
{
    public class BtnStatus
    {
        public string Name { get; set; }
        public BtnState State { get; set; }
        public float PressTime { get; set; }
        public float ReleaseTime { get; set; }
        public int TapCounter { get; set; }
        public bool IsHolding { get; set; }
        public bool IsReleasedAsButton { get; set; }
        public bool IsDoubleTapped => State is BtnState.ReleasedAsButton && TapCounter == 2;
        public bool IsMultiTapped(int tapCount) => State is BtnState.ReleasedAsButton && TapCounter == tapCount;
        
        public BtnStatus()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            State = BtnState.Idle;
            PressTime = float.MaxValue;
            ReleaseTime = 0f;
            TapCounter = 0;
            IsHolding = false;
            IsReleasedAsButton = false;
        }

    }
}
