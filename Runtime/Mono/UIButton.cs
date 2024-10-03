using System;
using AceLand.Input.State;
using AceLand.Library.Attribute;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AceLand.Input.Mono
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("AMVR/Input/UI/UI Button")]
    public class UIButton : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UIButton")]
        [SerializeField] private bool interactable = true;
        [SerializeField] private UIButtonType type = UIButtonType.UIAction;
        [SerializeField] private string actionKey;
        [SerializeField, ConditionalShow("type", UIButtonType.PhysicsButton)]
        private bool isExitAsRelease = false;

        [Header("Transition")]
        [SerializeField] private UIButtonTransition transition = UIButtonTransition.Color;
        [SerializeField, ConditionalShow("transition", UIButtonTransition.Color)]
        private Color activeColor = new(0.85f, 0.85f, 0.85f, 1f);
        [SerializeField, ConditionalShow("transition", UIButtonTransition.Color)]
        private Color inactiveColor = Color.grey;
        [SerializeField, ConditionalShow("transition", UIButtonTransition.Sprite)]
        private Sprite activeSprite;
        [SerializeField, ConditionalShow("transition", UIButtonTransition.Sprite)]
        private Sprite inactiveSprite;

        private Image _image;
        private bool _isPressed;
        private bool _isEnter;

        public bool Interactive
        {
            get => interactable;
            set
            {
                interactable = value;
                SetImage();
            }
        }

        private void OnValidate()
        {
            _image ??= GetComponentInChildren<Image>();
            if (!_image) return;
            
            SetImage();
        }

        protected virtual void Awake()
        {
            _image = GetComponentInChildren<Image>();
        }

        protected virtual void OnEnable()
        {
            SetImage();
        }

        protected virtual void OnDisable()
        {
            _isPressed = false;
            SetImage();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!enabled || !interactable) return;
            _isEnter = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!enabled || !interactable) return;
            
            _isEnter = false;
            if (!_isPressed) return;
            
            switch (type)
            {
                case UIButtonType.None:
                    break;
                case UIButtonType.UIAction:
                    break;
                case UIButtonType.PhysicsButton:
                    _isPressed = false;
                    if (isExitAsRelease) OnRelease();
                    else OnIdle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!enabled || !interactable) return;
            _isPressed = true;
            OnPress();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!enabled || !interactable) return;
            if (!_isEnter || !_isPressed) return;
            
            OnRelease();
            _isPressed = false;
        }

        private void OnIdle()
        {
            InputHelper.SetBtnStatus(actionKey, BtnState.Idle);
        }

        protected virtual void OnPress()
        {
            InputHelper.SetBtnStatus(actionKey, BtnState.Pressed);
        }

        protected virtual void OnRelease()
        {
            InputHelper.SetBtnStatus(actionKey, BtnState.Released);
        }
        
        protected virtual void SetImage()
        {
            switch (transition)
            {
                case UIButtonTransition.None:
                    break;
                case UIButtonTransition.Color:
                    _image.color = enabled && interactable ? activeColor : inactiveColor;
                    break;
                case UIButtonTransition.Sprite:
                    _image.sprite = enabled && interactable ? activeSprite : inactiveSprite;
                    break;
                case UIButtonTransition.SpriteBundle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
