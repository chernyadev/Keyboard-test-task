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

        [Obsolete("Please use generic KeyDown and KeyUp instead")]
        public void KeyPress(string c)
        {
            var initialCase = currentCase;
            currentCase = char.IsUpper(c[0]) ? KeyboardCase.Upper : KeyboardCase.Lower;
            
            var code = (KeyCode) Enum.Parse(typeof(KeyCode), char.ToUpperInvariant(c[0]).ToString());
            KeyDown(code);
            KeyUp(code);

            currentCase = initialCase;
        }

        [Obsolete("Please use generic KeyDown and KeyUp instead")]
        public void KeyLeft()
        {
            var code = KeyCode.LeftArrow;
            KeyDown(code);
            KeyUp(code);
        }

        [Obsolete("Please use generic KeyDown and KeyUp instead")]
        public void KeyRight()
        {
            var code = KeyCode.RightArrow;
            KeyDown(code);
            KeyUp(code);
        }

        [Obsolete("Please use generic KeyDown and KeyUp instead")]
        public void KeyDelete()
        {
            var code = KeyCode.Backspace;
            KeyDown(code);
            KeyUp(code);
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

            while (true)
            {
                action.Invoke();
                yield return new WaitForSeconds(keystrokeDelay);
            }
        }
    }
}
