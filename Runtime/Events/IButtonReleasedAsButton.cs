using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonReleasedAsButton
    {
        void OnButtonReleasedAsButton(in BtnStatus btnStatus);
    }
}