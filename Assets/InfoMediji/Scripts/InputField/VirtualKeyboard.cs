using System;
using System.Collections;
using UnityEngine;

namespace InfoMediji.InputField
{
    public class VirtualKeyboard : MonoBehaviour
    {
        public enum KeyboardCase
        {
            Upper,
            Lower
        }

        public MyInputField inputField;
        public KeyboardCase currentCase = KeyboardCase.Upper;

        [SerializeField]
        private float keystrokeDelay = 0.5f;

        private KeyCode _lastKeyCode;

        private Coroutine _keyPressedCoroutine;

        public void KeyDown(KeyCode keyCode)
        {
            Action action = null;

            switch (keyCode)
            {
                case KeyCode.LeftArrow:
                    action = () => inputField.VirtualCaretShift(MyInputField.ShiftDirection.Left);
                    break;
                case KeyCode.RightArrow:
                    action = () => inputField.VirtualCaretShift(MyInputField.ShiftDirection.Right);
                    break;
                case KeyCode.Backspace:
                    action = () => inputField.VirtualBackspace();
                    break;
                case var code when code >= KeyCode.A && code <= KeyCode.Z:
                    var c = ApplyCurrentCase(keyCode.ToString()[0]);
                    action = () => inputField.VirtualKeyPress(c);
                    break;
            }

            if (_keyPressedCoroutine != null)
            {
                StopCoroutine(_keyPressedCoroutine);
            }

            _keyPressedCoroutine = StartCoroutine(KeyPressed(action));

            _lastKeyCode = keyCode;
        }

        public void KeyUp(KeyCode keyCode)
        {
            if (keyCode == _lastKeyCode)
            {
                StopCoroutine(_keyPressedCoroutine);
            }
        }

        private char ApplyCurrentCase(char c)
        {
            switch (currentCase)
            {
                case KeyboardCase.Upper:
                    c = char.ToUpperInvariant(c);
                    break;
                case KeyboardCase.Lower:
                    c = char.ToLowerInvariant(c);
                    break;
            }

            return c;
        }

        private IEnumerator KeyPressed(Action action)
        {
            if (action is null)
            {
                yield break;
            }

            action.Invoke();

            while (true)
            {
                yield return new WaitForSeconds(keystrokeDelay);
                action.Invoke();
            }
        }
    }
}