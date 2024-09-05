using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonReleased
    {
        void OnButtonReleased(in BtnStatus btnStatus);
    }
}