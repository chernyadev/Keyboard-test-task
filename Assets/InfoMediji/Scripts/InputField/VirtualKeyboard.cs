﻿using UnityEngine;

namespace InfoMediji.InputField
{
    public class VirtualKeyboard : MonoBehaviour {

        public MyInputField InputField;

        public void KeyPress(string c)
        {
            InputField.text += c;
            InputField.ActivateInputField();
        }

        public void KeyLeft()
        {

        }

        public void KeyRight()
        {

        }

        public void KeyDelete()
        {

        }
    }
}
