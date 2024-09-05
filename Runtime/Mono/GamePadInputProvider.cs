using AceLand.Input.PlayerLoopSystems;
using AceLand.Input.State;
using AceLand.Library.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AceLand.Input.Mono
{
    public class GamePadInputProvider : MonoBehaviour
    {
        [Header("Action Name")]
        [SerializeField] private string leftStickAction = "LeftStick";
        [SerializeField] private string rightStickAction = "RightStick";
        
        [Header("Settings")]
        [SerializeField, Range(0.01f, 5)] private float smoothDumpOnMove = 0.2f;
        [SerializeField, Range(0.01f, 5)] private float smoothDumpOnStop = 0.05f;
        [SerializeField] private MoveDirectionType moveDirectionType = MoveDirectionType.Free;
        
        private readonly InputSettings _inputSettings = InputSystem.settings;
        
        private Vector2 _leftStickVelocity, _rightStickVelocity;
        
        private void Awake()
        {
            InputProvider.Importer<Vector2>()
                .WithActionName(leftStickAction)
                .WithMethod(LeftStickHandler)
                .Import();
            InputProvider.Importer<Vector2>()
                .WithActionName(rightStickAction)
                .WithMethod(RightStickHandler)
                .Import();
        }

        private void OnDestroy()
        {
            InputProvider.Remove<Vector2>(leftStickAction);
            InputProvider.Remove<Vector2>(rightStickAction);
        }

        private Vector2 LeftStickHandler(Vector2 input)
        {
            return SmoothAxis(ref input, ref _leftStickVelocity);
        }

        private Vector2 RightStickHandler(Vector2 input)
        {
            return SmoothAxis(ref input, ref _rightStickVelocity);
            
        }

        private Vector2 SmoothAxis(ref Vector2 rawAxis, ref Vector2 velocity)
        {
            if (InputManager.OverrideUserInput)
                return Vector2.zero;

            var finalAxis = Vector2.zero;
            var absInputAxisRaw = Vector2.zero;
            var inputAxisRawDirection = Vector2.zero;
            var targetAxis = Vector2.zero;
            
            absInputAxisRaw.x = Mathf.Abs(rawAxis.x);
            absInputAxisRaw.y = Mathf.Abs(rawAxis.y);
            inputAxisRawDirection.x = Mathf.Sign(rawAxis.x);
            inputAxisRawDirection.y = Mathf.Sign(rawAxis.y);
            targetAxis.x = (absInputAxisRaw.x < _inputSettings.defaultDeadzoneMin) switch
            {
                true => 0,
                false => absInputAxisRaw.x.Remap(
                    _inputSettings.defaultDeadzoneMin, _inputSettings.defaultDeadzoneMax,
                    0, 1) * inputAxisRawDirection.x,
            };
            targetAxis.y = (absInputAxisRaw.y < _inputSettings.defaultDeadzoneMin) switch
            {
                true => 0,
                false => absInputAxisRaw.y.Remap(
                    _inputSettings.defaultDeadzoneMin, _inputSettings.defaultDeadzoneMax,
                    0, 1) * inputAxisRawDirection.y,
            };
            finalAxis.x = (targetAxis.x == 0) switch
            {
                false when targetAxis.x != 0 || Mathf.Approximately(absInputAxisRaw.x, inputAxisRawDirection.x) => 
                    Mathf.SmoothDamp(finalAxis.x, targetAxis.x, ref velocity.x, smoothDumpOnMove),
                _ => Mathf.SmoothDamp(finalAxis.x, targetAxis.x, ref velocity.x, smoothDumpOnStop)
            };
            finalAxis.y = (targetAxis.y == 0) switch
            {
                false when targetAxis.y != 0 || Mathf.Approximately(absInputAxisRaw.y, inputAxisRawDirection.y) => 
                    Mathf.SmoothDamp(finalAxis.y, targetAxis.y, ref velocity.y, smoothDumpOnMove),
                _ => Mathf.SmoothDamp(finalAxis.y, targetAxis.y, ref velocity.y, smoothDumpOnStop)
            };

            return finalAxis;
        }
    }
}