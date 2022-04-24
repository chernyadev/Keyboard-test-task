#The Keyboard Task

##Initial Task Description:

We have prepared a project template with several buttons and one input field.
Please check scene called "Scene" in the root of the Assets.

Try pressing A, B or C buttons. This will add a character to the end of the input field.
Then it reactivates the input field, and this will select and highlight all the text.
This is the wrong behavior.

The challenge is to create a new class that will implement the InputField.
The new extended class (MyInputField) should make the InputField work with the UI buttons in the same way as manual input does.

* Focus should remain on the input field when the user interface buttons are pressed.
* Caret must remain in last position.
* Pressing UI button when the text is selected should replace it with corresponding character.
  Selection should disappear, the caret should move to the position after this symbol.
* Backspace, Left and Right buttons should be implemented. Left and Right buttons should move the cursor over the input field.
* Virtual keyboard must be consistent and should not interfere with manual input from a real keyboard,
* The bonus is to implement multiple keystrokes on the button hold, as on a real keyboard.

* Try not to change the base InputField (Original) class, or make as few changes as possible.

In this project you will also find InputFieldOriginal class, which is just a copy of regular InputField class from the Internet.
You can use either this class to check what is happening inside, or use the usual from the Unity package.

## Implementation:

* [`MyInputField`](Assets/InfoMediji/Scripts/InputField/MyInputField.cs) - class is inherited from the unmodified [`InputFieldOriginal`](Assets/InfoMediji/Scripts/InputField/InputFieldOriginal.cs) class.
  Interaction with the input field is done via emulation of real keyboard events by calling of the `KeyPressed` method.
* [`VirtualKeyboard`](Assets/InfoMediji/Scripts/InputField/VirtualKeyboard.cs) - class is modified and it's main interface are `KeyDown` and `KeyUp` methods.
  The `KeyCode` enum is used as the parameter for these methods. Supported options:
  * `KeyCode.A`-`KeyCode.Z` - corresponding letter will be printed. 
    Provided key code is converted to char and modified to UPPER o lower case depending on the value of `currentCase` field.
  * `KeyCode.LeftArrow`, `KeyCode.RightArrow` - caret will be shifted to the corresponding direction.
  * `KeyCode.Backspace` - backspace press will be emulated.
  
  Functionality of multiple keystrokes on button hold is implemented. Delay between the keystrokes is controlled by the `keystrokeDelay` field. 
  Initially provided methods: `KeyPress`, `KeyLeft`, `KeyRight`, `KeyDelete` are also implemented. All these methods emulate button _click_, they do not support holding.
* [`VirtualKeyboardButton`](Assets/InfoMediji/Scripts/InputField/VirtualKeyboardButton.cs) - component which could be added to the `Button` object to automate pointer events handling and support key holding feature.
  You must specify reference to the `keyboard` object and select emulated `keyCode` with respect to the logic of the `VirtualKeyboard.KeyDown` method described above.
  In runtime standard `EventTrigger` will be used to handle `PointerDown` and `PointerUp` events. `VirtualKeyboard.KeyDown` and `VirtualKeyboard.KeyUp` methods will be called respectfully.
  
## Examples

Example scenes could be found in the [Scenes](Assets/InfoMediji/Scenes) directory.
* [VirtualKeyboard](Assets/InfoMediji/Scenes/VirtualKeyboard.unity) - scene utilizing new `KeyDown` and `KeyUp` methods. Methods are called by `VirtualKeyboardButton` component added to each button in the scene.
* [VirtualKeyboard_NoMultipleKeystroke](Assets/InfoMediji/Scenes/VirtualKeyboard_NoMultipleKeystroke.unity) - scene utilizing initially provided `KeyPress`, `KeyLeft`, `KeyRight`, `KeyDelete` methods. Methods are called via `OnClick` callbacks defined in the editor. Does not support button hold feature.
