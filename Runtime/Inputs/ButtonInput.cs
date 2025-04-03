using AceLand.EventDriven.Bus;
using AceLand.Input.Events;
using AceLand.Input.PlayerLoopSystems;
using AceLand.Input.State;
using AceLand.Library.BuildLeveling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace AceLand.Input.Inputs
{
    internal class ButtonInput : BtnInputBase<BtnStatus>
    {
        protected override void ResetState()
        {
            foreach (var name in BtnKeys)
            {
                var currentTime = Time.realtimeSinceStartup;
                var btnStatus = Btns[name];
                if (btnStatus.State is BtnState.Holding)
                {
                    btnStatus.State = BtnState.Idle;
                }
                else if (btnStatus.State is not BtnState.ReleasedAsButton and not BtnState.Released &&
                    !btnStatus.IsHolding &&
                    currentTime - btnStatus.PressTime > InputSettings.defaultHoldTime)
                {
                    OnHold(name);
                }

                btnStatus.State = btnStatus.State switch
                {
                    BtnState.Idle => BtnState.Idle,
                    BtnState.Pressed => BtnState.Idle,
                    BtnState.Released => BtnState.Idle,
                    BtnState.ReleasedAsButton => BtnState.Idle,
                    BtnState.Holding => BtnState.Holding,
                    _ => throw new System.ArgumentOutOfRangeException(nameof(BtnState), btnStatus.State, string.Empty),
                };
            }
        }

        public void SetActions(InputActionMap actionMap)
        {
            foreach (var action in actionMap.actions)
            {
                var btnName = action.name;
                
                if (btnName == Settings.QuitKey && Settings.QuitKeyLevel.IsAcceptedLevel())
                    action.performed += _ => OnPlayerQuit();
                
                if (btnName == Settings.ReloadKey && Settings.ReloadKeyLevel.IsAcceptedLevel())
                    action.performed += _ => OnPlayerReload();
                
                AddBtn(btnName);
                action.performed += _ => OnPress(btnName);
                action.canceled += _ => OnRelease(btnName);
            }
        }

        private void OnPlayerReload()
        {
            Debug.Log("Player Reload Scene");
            EventBus.Event<IReloadButtonPressed>()
                .WithSender(this)
                .Raise();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnPlayerQuit()
        {
            Debug.Log("Player Quit");
            EventBus.Event<IQuitButtonPressed>()
                .WithSender(this)
                .Raise();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        protected override void OnPress(string name)
        {
            if (!Btns.ContainsKey(name)) return;
            
            base.OnPress(name);
            var btnStatus = Btns[name];
            if (btnStatus.State is not BtnState.Pressed) return;
            if (InputManager.OverrideUserInput) return;

            EventBus.Event<IButtonPressed>()
                .WithSender(this)
                .WithData(btnStatus)
                .Raise();
        }

        protected override void OnHold(string name)
        {
            if (!Btns.ContainsKey(name)) return;
            
            base.OnHold(name);
            var btnStatus = Btns[name];
            if (btnStatus.State is not BtnState.Holding) return;
            if (InputManager.OverrideUserInput) return;
            
            EventBus.Event<IButtonHold>()
                .WithSender(this)
                .WithData(btnStatus)
                .Raise();
        }

        protected override void OnRelease(string name)
        {
            if (!Btns.ContainsKey(name)) return;
            
            base.OnRelease(name);
            var btnStatus = Btns[name];
            if (InputManager.OverrideUserInput) return;
            
            if (btnStatus.State is BtnState.Released)
            {
                EventBus.Event<IButtonReleased>()
                    .WithSender(this)
                    .WithData(btnStatus)
                    .Raise();
                return;
            }
            
            EventBus.Event<IButtonReleasedAsButton>()
                .WithSender(this)
                .WithData(btnStatus)
                .Raise();
        }
    }
}
