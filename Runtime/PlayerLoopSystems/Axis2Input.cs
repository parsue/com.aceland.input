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
                foreach (var handler in InterfaceMapping.FindObjects<IAxis2Input>())
                    handler?.OnAxis2Input(data);
            }
        }
    }
}