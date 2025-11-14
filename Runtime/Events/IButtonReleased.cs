using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonReleased : IEvent
    {
        void OnButtonReleased(object sender, BtnStatus btnStatus);
    }
}