using AceLand.Input.State;
using UnityEngine;

namespace AceLand.Input.Events
{
    public interface IAxis2Input
    {
        void OnAxis2Input(InputData<Vector2> data);
    }
}