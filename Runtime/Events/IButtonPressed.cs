using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonPressed
    {
        void OnButtonPressed(in BtnStatus btnStatus);
    }
}