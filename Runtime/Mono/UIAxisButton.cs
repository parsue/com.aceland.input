using System.Collections;
using AceLand.EventDriven.Bus;
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
    public class UIAxisButton : UIButton
    {
        private static AceLandInputSettings Settings => AceInput.Settings;
        
        [Header("Axis Button")]
        [SerializeField] private string axisKey;
        [SerializeField] private Color selectColor = Color.white;

        private bool _init;
        private Coroutine _inputCoroutine;

        protected override void OnEnable()
        {
            base.OnEnable();
            EventBus.Event<IAxisInput>()
                .WithListener<InputData<float>>(OnAxisInput)
                .Listen();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _init = false;
            EventBus.Event<IAxisInput>()
                .Unlisten<InputData<float>>(OnAxisInput);
            if (_inputCoroutine == null) return;
            StopCoroutine(_inputCoroutine);
            _inputCoroutine = null;
        }

        private void OnAxisInput(object sender, InputData<float> data)
        {
            if (!enabled || !interactable) return;
            if (data.Name != axisKey) return;

            InputHandle(data.RawData);
        }

        private void InputHandle(float clampedValue)
        {
            if (clampedValue < Settings.AxisButtonThreshold)
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
            var targetTime = Time.realtimeSinceStartup + Settings.AxisButtonActionTime;
            while (Time.realtimeSinceStartup < targetTime)
            {
                var amount = (targetTime - Time.realtimeSinceStartup) / Settings.AxisButtonActionTime;
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
