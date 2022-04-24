using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InfoMediji.InputField
{
    [RequireComponent(typeof(Button))]
    public class VirtualKeyboardButton : MonoBehaviour
    {
        [SerializeField]
        private VirtualKeyboard keyboard;
        [SerializeField]
        private KeyCode keyCode;

        private EventTrigger _eventTrigger;

        private void Awake()
        {
            _eventTrigger = gameObject.AddComponent<EventTrigger>();

            var down = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            down.callback.AddListener(_ => keyboard.KeyDown(keyCode));

            var up = new EventTrigger.Entry {eventID = EventTriggerType.PointerUp};
            up.callback.AddListener(_ => keyboard.KeyUp(keyCode));

            _eventTrigger.triggers.Add(down);
            _eventTrigger.triggers.Add(up);
        }
    }
}