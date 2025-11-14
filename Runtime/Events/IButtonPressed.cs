using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonPressed : IEvent
    {
        void OnButtonPressed(object sender, BtnStatus btnStatus);
    }
}