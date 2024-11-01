using AceLand.EventDriven.EventInterface;
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
                
                if (btnName == Settings.quitKey && Settings.quitKeyLevel.IsAcceptedLevel())
                    action.performed += _ => OnPlayerQuit();
                
                if (btnName == Settings.reloadKey && Settings.reloadKeyLevel.IsAcceptedLevel())
                    action.performed += _ => OnPlayerReload();
                
                AddBtn(btnName);
                action.performed += _ => OnPress(btnName);
                action.canceled += _ => OnRelease(btnName);
            }
        }

        private void OnPlayerReload()
        {
            foreach (var listener in InterfaceMapping.FindObjects<IReloadButtonPressed>())
                listener?.OnReloadButtonPressed();

            Debug.Log("Player Reload Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnPlayerQuit()
        {
            foreach (var listener in InterfaceMapping.FindObjects<IQuitButtonPressed>())
                listener?.OnQuitButtonPressed();
            
            Debug.Log("Player Quit");
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

            foreach (var handler in InterfaceMapping.FindObjects<IButtonPressed>())
            {
                handler?.OnButtonPressed(btnStatus);
            }
        }

        protected override void OnHold(string name)
        {
            if (!Btns.ContainsKey(name)) return;
            
            base.OnHold(name);
            var btnStatus = Btns[name];
            if (btnStatus.State is not BtnState.Holding) return;
            if (InputManager.OverrideUserInput) return;
            
            foreach (var handler in InterfaceMapping.FindObjects<IButtonHold>())
                handler?.OnButtonHold(btnStatus);
        }

        protected override void OnRelease(string name)
        {
            if (!Btns.ContainsKey(name)) return;
            
            base.OnRelease(name);
            var btnStatus = Btns[name];
            if (InputManager.OverrideUserInput) return;
            
            if (btnStatus.State is BtnState.Released)
            {
                foreach (var handler in InterfaceMapping.FindObjects<IButtonReleased>())
                    handler?.OnButtonReleased(btnStatus);
                
                return;
            }
            
            foreach (var handler in InterfaceMapping.FindObjects<IButtonReleasedAsButton>())
                handler?.OnButtonReleasedAsButton(btnStatus);
        }
    }
}
