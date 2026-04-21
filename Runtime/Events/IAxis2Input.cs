using AceLand.EventDriven.Bus;
using AceLand.Input.State;
using UnityEngine;

namespace AceLand.Input.Events
{
    public interface IAxis2Input : IEvent<InputData<Vector2>>
    {
        void OnAxis2Input(object sender, InputData<Vector2> data);
    }
}