using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InfoMediji.InputField
{
    public class MyInputField : InputFieldOriginal
    {
        public enum ShiftDirection
        {
            Left,
            Right
        }

        private readonly Queue<Action> _nextUpdateActions = new Queue<Action>();

        private int _cachedSelectionAnchorPosition;
        private int _cachedSelectionFocusPosition;

        public override void OnDeselect(BaseEventData eventData)
        {
            CacheSelection();
            base.OnDeselect(eventData);
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (_nextUpdateActions.Count > 0)
            {
                _nextUpdateActions.Dequeue().Invoke();
            }
        }

        public void VirtualKeyPress(char c)
        {
            ActivateInputField();
            EnqueueInputAction(new Event {character = c});
        }

        public void VirtualCaretShift(ShiftDirection direction)
        {
            ActivateInputField();
            EnqueueInputAction(new Event {keyCode = ShiftDirectionToKeyCode(direction)});
        }

        public void VirtualBackspace()
        {
            ActivateInputField();
            EnqueueInputAction(new Event {keyCode = KeyCode.Backspace});
        }

        private void EnqueueInputAction(Event e)
        {
            _nextUpdateActions.Enqueue(() =>
            {
                RecoverSelection();
                KeyPressed(e);
                CacheSelection();
                UpdateLabel();
            });
        }

        private void CacheSelection()
        {
            _cachedSelectionAnchorPosition = selectionAnchorPosition;
            _cachedSelectionFocusPosition = selectionFocusPosition;
        }

        private void RecoverSelection()
        {
            selectionAnchorPosition = _cachedSelectionAnchorPosition;
            selectionFocusPosition = _cachedSelectionFocusPosition;
        }

        private static KeyCode ShiftDirectionToKeyCode(ShiftDirection direction)
        {
            switch (direction)
            {
                case ShiftDirection.Left:
                    return KeyCode.LeftArrow;
                case ShiftDirection.Right:
                    return KeyCode.RightArrow;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}