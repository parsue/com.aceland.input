using AceLand.EventDriven.Bus;
using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IAxisInput : IEvent<InputData<float>>
    {
        void OnAxisInput(InputData<float> data);
    }
}