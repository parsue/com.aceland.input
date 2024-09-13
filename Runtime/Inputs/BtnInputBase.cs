using System.Collections.Generic;
using AceLand.Input.PlayerLoopSystems;
using AceLand.Input.State;
using UnityEngine;

namespace AceLand.Input.Inputs
{
    internal abstract class BtnInputBase<T> : InputBase
        where T : BtnStatus, new()
    {
        private protected readonly Dictionary<string, T> Btns = new();
        private protected readonly List<string> BtnKeys = new();
        private protected bool ReleaseFromHolding = false;

        protected override void OnInit()
        {
            Btns.Clear();
        }

        protected override void ResetInputStateCycle()
        {
            if (Btns.Count == 0) return;
            ResetState();
        }

        protected virtual void AddBtn(string name)
        {
            if (Btns.ContainsKey(name)) return;
            var btnStatus = new T { Name = name };
            Btns.Add(name, btnStatus);
            if (!BtnKeys.Contains(name)) BtnKeys.Add(name);
        }

        protected virtual void RemoveBtn(string name)
        {
            if (!Btns.ContainsKey(name)) return;
            Btns.Remove(name);
            if (BtnKeys.Contains(name)) BtnKeys.Remove(name);
        }

        protected virtual bool IsAnyState(string name)
        {
            return !InputManager.OverrideUserInput && Btns.TryGetValue(name, out var btnStatus) && btnStatus.State is not BtnState.Idle;
        }

        protected virtual T GetBtnStatus(string name)
        {
            return InputManager.OverrideUserInput || !Btns.TryGetValue(name, out var btnStatus) ? new T { Name = name } : btnStatus;
        }

        protected virtual BtnState GetBtnState(string name)
        {
            return InputManager.OverrideUserInput || !Btns.TryGetValue(name, out var btnStatus) ? BtnState.Idle : btnStatus.State;
        }

        internal virtual void ForceBtnState(string name, BtnState state)
        {
            switch (state)
            {
                case BtnState.Idle:
                    OnForceIdle(name);
                    break;
                case BtnState.Pressed:
                    OnPress(name);
                    break;
                case BtnState.Holding:
                    OnHold(name);
                    break;
                case BtnState.Released:
                    OnRelease(name);
                    break;
                case BtnState.ReleasedAsButton:
                    OnPressAsButton(name);
                    break;
            }
        }

        protected virtual void OnForceIdle(string name)
        {
            if (!Btns.TryGetValue(name, out var btnStatus)) return;
            
            btnStatus.ReleaseTime = Time.realtimeSinceStartup;
            btnStatus.PressTime = float.MaxValue;
            btnStatus.IsHolding = false;
            btnStatus.State = BtnState.Idle;
        }

        protected virtual void OnPress(string name)
        {
            if (!Btns.TryGetValue(name, out var btnStatus)) return;
            
            btnStatus.PressTime = Time.realtimeSinceStartup;
            btnStatus.State = BtnState.Pressed;
            if (ReleaseFromHolding || btnStatus.PressTime - btnStatus.ReleaseTime >= InputSettings.defaultHoldTime)
                btnStatus.TapCounter = 0;
            btnStatus.TapCounter++;
            btnStatus.ReleaseTime = 0f;
        }

        protected virtual void OnHold(string name)
        {
            if (!Btns.TryGetValue(name, out var btnStatus)) return;
            
            btnStatus.State = BtnState.Holding;
            btnStatus.IsHolding = true;
        }

        protected virtual void OnRelease(string name)
        {
            if (!Btns.TryGetValue(name, out var btnStatus)) return;
            
            ReleaseFromHolding = btnStatus.IsHolding;
            btnStatus.ReleaseTime = Time.realtimeSinceStartup;
            btnStatus.State = btnStatus.ReleaseTime - btnStatus.PressTime < InputSettings.defaultHoldTime
                ? Settings.releaseType is ReleaseHandlingType.ReleasedReplaceReleasedAsButton
                    ? BtnState.Released 
                    : BtnState.ReleasedAsButton
                : BtnState.Released;
            btnStatus.PressTime = float.MaxValue;
            btnStatus.IsHolding = false;
            if (btnStatus.State is BtnState.ReleasedAsButton) OnPressAsButton(name);
        }

        protected virtual void OnPressAsButton(string name)
        {
            // noop
        }
    }
}
