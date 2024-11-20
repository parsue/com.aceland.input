using System;
using System.Collections.Generic;
using AceLand.Input.Inputs;
using AceLand.Input.State;
using AceLand.PlayerLoopHack;
using AceLand.TaskUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

namespace AceLand.Input.PlayerLoopSystems
{
    internal abstract class InputBaseSystem<T> : InputBaseSystem
        where T : struct
    {
        private protected readonly Dictionary<string, InputData<T>> InputData = new();
        private protected readonly List<string> DataKeys = new();

        public virtual void AddProvideMethod(string inputName, Func<T, T> providedMethod)
        {
            // noop
        }

        public virtual void RemoveProvideMethod(string inputName)
        {
            if (!InputData.TryGetValue(inputName, out var data)) return;
            data.ProvidedMethod = value => value;
        }
        
        protected override void InputHandle()
        {
            // noop
        }

        public void SetActions(InputActionMap actionMap)
        {
            foreach (var action in actionMap.actions)
            {
                var btnName = action.name;
                AddInput(btnName);
                action.performed += ctx => OnDataPerformed(btnName, ctx.ReadValue<T>());
                action.canceled += _ => OnDataCanceled(btnName);
            }
        }

        protected virtual void AddInput(string name)
        {
            if (InputData.ContainsKey(name)) return;

            var data = new InputData<T>()
            {
                Name = name,
                ProvidedMethod = value => value,
            };
            InputData.Add(name, data);
            if (!DataKeys.Contains(name)) DataKeys.Add(name);
        }

        private void OnDataPerformed(string name, T input)
        {
            SetData(name, input);
        }

        private void OnDataCanceled(string name)
        {
            SetData(name, default);
        }

        private void SetData(string name, T value)
        {
            var data = InputData[name];
            data.LastRawData = data.RawData;
            data.LastProvidedData = data.ProvidedData;
            data.RawData = value;
            data.ProvidedData = data.ProvidedMethod.Invoke(data.RawData);
            data.Time = Time.realtimeSinceStartup;
            InputData[name] = data;
        }
    }

    internal abstract class InputBaseSystem : InputBase, IPlayerLoopSystem
    {
        private static PlayerLoopSystem _playerLoopSystem;

        protected override void OnInit()
        {
            if (!Settings.HandleAxisInput) return;
            
            OnStart();
            Promise.AddApplicationQuitListener(OnStop);
        }

        private void OnStart()
        {
            _playerLoopSystem = this.CreatePlayerLoopSystem();
            _playerLoopSystem.InjectSystem(PlayerLoopState.Initialization);
        }

        private void OnStop()
        {
            _playerLoopSystem.RemoveSystem(PlayerLoopState.Initialization);
        }

        public void SystemUpdate()
        {
            InputHandle();
        }

        protected virtual void InputHandle()
        {
            // noop
        }
    }
}