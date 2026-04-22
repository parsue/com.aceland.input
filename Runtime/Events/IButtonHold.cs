using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonHold : IEvent<BtnStatus>
    {
        void OnButtonHold(BtnStatus btnStatus);
    }
}