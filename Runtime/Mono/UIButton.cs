using System;
using AceLand.Input.State;
using AceLand.Library.Attribute;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AceLand.Input.Mono
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("AceLand/Input/UI/Button")]
    [ExecuteInEditMode]
    public class UIButton : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Button")]
        [SerializeField] private protected bool interactable = true;
        [SerializeField] private protected UIButtonType type = UIButtonType.UIAction;
        [SerializeField] private protected string actionKey;
        [SerializeField, ConditionalShow("type", UIButtonType.PhysicsButton)]
        private protected bool isExitAsRelease;

        [Header("Transition")]
        [SerializeField] private protected UIButtonTransition transition = UIButtonTransition.Color;
        [SerializeField, ConditionalShow("transition", UIButtonTransition.Color)]
        private protected Color activeColor = new(0.85f, 0.85f, 0.85f, 1f);
        [SerializeField, ConditionalShow("transition", UIButtonTransition.Color)]
        private protected Color inactiveColor = Color.grey;
        [SerializeField, ConditionalShow("transition", UIButtonTransition.Sprite)]
        private protected Sprite activeSprite;
        [SerializeField, ConditionalShow("transition", UIButtonTransition.Sprite)]
        private protected Sprite inactiveSprite;

        private protected Image Image;
        private protected bool IsPressed;
        private protected bool IsEnter;

        public bool Interactive
        {
            get => interactable;
            set
            {
                interactable = value;
                SetImage();
            }
        }

        protected virtual  void OnValidate()
        {
            Image ??= GetComponentInChildren<Image>();
            if (!Image) return;
            
            SetImage();
        }

        protected virtual void Awake()
        {
            Image = GetComponentInChildren<Image>();
        }

        protected virtual void OnEnable()
        {
            SetImage();
        }

        protected virtual void OnDisable()
        {
            IsPressed = false;
            SetImage();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!enabled || !interactable) return;
            IsEnter = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!enabled || !interactable) return;
            
            IsEnter = false;
            if (!IsPressed) return;
            
            switch (type)
            {
                case UIButtonType.None:
                    break;
                case UIButtonType.UIAction:
                    break;
                case UIButtonType.PhysicsButton:
                    IsPressed = false;
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
            IsPressed = true;
            OnPress();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!enabled || !interactable) return;
            if (!IsEnter || !IsPressed) return;
            
            OnRelease();
            IsPressed = false;
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
            if (!Image) return;
            
            switch (transition)
            {
                case UIButtonTransition.None:
                    break;
                case UIButtonTransition.Color:
                    Image.color = enabled && interactable ? activeColor : inactiveColor;
                    break;
                case UIButtonTransition.Sprite:
                    Image.sprite = enabled && interactable ? activeSprite : inactiveSprite;
                    break;
                case UIButtonTransition.SpriteBundle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
