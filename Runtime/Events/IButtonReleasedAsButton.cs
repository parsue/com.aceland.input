using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonReleasedAsButton : IEvent
    {
        void OnButtonReleasedAsButton(object sender, BtnStatus btnStatus);
    }
}