using System.Collections;
using AceLand.Input.Events;
using AceLand.Input.ProjectSetting;
using AceLand.Input.State;
using UnityEngine;
using UnityEngine.UI;

namespace AceLand.Input.Mono
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("AceLand/Input/UI/Axis Button")]
    [ExecuteInEditMode]
    public class UIAxisButton : UIButton,
        IAxisInput
    {
        private static AceLandInputSettings Settings => InputHelper.Settings;
        
        [Header("Axis Button")]
        [SerializeField] private string axisKey;
        [SerializeField] private Color selectColor = Color.white;

        private bool _init;
        private Coroutine _inputCoroutine;

        protected override void OnDisable()
        {
            base.OnDisable();
            _init = false;
            if (_inputCoroutine == null) return;
            StopCoroutine(_inputCoroutine);
            _inputCoroutine = null;
        }

        public void OnAxisInput(InputData<float> data)
        {
            if (!enabled || !interactable) return;
            if (data.Name != axisKey) return;

            InputHandle(data.RawData);
        }

        private void InputHandle(float clampedValue)
        {
            if (clampedValue < Settings.axisButtonThreshold)
            {
                if (!_init) _init = true;
                if (_inputCoroutine == null) return;
                StopCoroutine(_inputCoroutine);
                _inputCoroutine = null;
                Image.color = activeColor;
                return;
            }

            if (!_init) return;
            
            _inputCoroutine ??= StartCoroutine(InputControl());
        }

        private IEnumerator InputControl()
        {
            var targetTime = Time.realtimeSinceStartup + Settings.axisButtonActionTime;
            while (Time.realtimeSinceStartup < targetTime)
            {
                var amount = (targetTime - Time.realtimeSinceStartup) / Settings.axisButtonActionTime;
                var color = Color.Lerp(selectColor, activeColor, amount);
                Image.color = color;
                yield return null;
            }

            Image.color = selectColor;
            _inputCoroutine = null;
            OnPress();
        }
    }
}
