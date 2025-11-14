using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IQuitButtonPressed : IEvent
    {
        void OnQuitButtonPressed(object sender, BtnStatus btnStatus);
    }
}