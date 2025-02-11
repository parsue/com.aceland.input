using System;
using AceLand.EventDriven.EventInterface;
using AceLand.Input.Events;
using UnityEngine;

namespace AceLand.Input.PlayerLoopSystems
{
    internal sealed class Axis2Input : InputBaseSystem<Vector2>
    {
        public override void AddProvideMethod(string inputName, Func<Vector2, Vector2> providedMethod)
        {
            if (!InputData.TryGetValue(inputName, out var data)) return;
            Debug.Log($"Add Input Provider {providedMethod.Method.Name} to {inputName}");
            data.ProvidedMethod = providedMethod;
            InputData[inputName] = data;
        }
        
        protected override void InputHandle()
        {
            foreach (var key in DataKeys)
            {
                var data = InputData[key];
                var implementations  = InterfaceBinding.ListBindings<IAxis2Input>();
                foreach (var impl in implementations )
                    impl?.OnAxis2Input(data);
            }
        }
    }
}