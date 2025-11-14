using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IReloadButtonPressed : IEvent
    {
        void OnReloadButtonPressed(object sender, BtnStatus btnStatus);
    }
}