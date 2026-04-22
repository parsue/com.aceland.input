using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonReleasedAsButton : IEvent<BtnStatus>
    {
        void OnButtonReleasedAsButton(BtnStatus btnStatus);
    }
}