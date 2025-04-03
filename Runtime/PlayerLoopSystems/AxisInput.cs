using System;
using AceLand.EventDriven.Bus;
using AceLand.Input.Events;
using UnityEngine;

namespace AceLand.Input.PlayerLoopSystems
{
    internal sealed class AxisInput : InputBaseSystem<float>
    {
        public override void AddProvideMethod(string inputName, Func<float, float> providedMethod)
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
                if (Equals(data.RawData, data.LastRawData)) continue;
                
                EventBus.Event<IAxisInput>()
                    .WithSender(this)
                    .WithData(data)
                    .Raise();
            }
        }
    }
}