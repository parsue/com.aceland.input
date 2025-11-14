using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IAxisInput : IEvent
    {
        void OnAxisInput(object sender, InputData<float> data);
    }
}