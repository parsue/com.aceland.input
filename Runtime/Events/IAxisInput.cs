using AceLand.Input.State;

namespace AceLand.Input.Events
{
    public interface IAxisInput
    {
        void OnAxisInput(InputData<float> data);
    }
}