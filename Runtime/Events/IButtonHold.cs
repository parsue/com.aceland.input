using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonHold : IEvent
    {
        void OnButtonHold(object sender, BtnStatus btnStatus);
    }
}