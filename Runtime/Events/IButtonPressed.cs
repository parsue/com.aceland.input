using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IButtonPressed : IEvent<BtnStatus>
    {
        void OnButtonPressed(BtnStatus btnStatus);
    }
}