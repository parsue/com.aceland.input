using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonHold
    {
        void OnButtonHold(in BtnStatus btnStatus);
    }
}