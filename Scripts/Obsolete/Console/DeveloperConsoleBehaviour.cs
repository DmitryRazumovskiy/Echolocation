using Utilities.DeveloperConsole.Commands;
using UnityEngine;
using TMPro;
using static UnityEngine.InputSystem.InputAction;

namespace Utilities.DeveloperConsole
{
    public class DeveloperConsoleBehaviour : MonoBehaviour
    {
        [SerializeField] private string prefix = string.Empty;
        [SerializeField] private ConsoleComnand[] comnands = new ConsoleComnand[0];

        [Header("Интерфейс")]
        [SerializeField] private GameObject uiCanvas = null;
        [SerializeField] private TMP_InputField inputField = null;

        private float pausedTimeScale;

        private static DeveloperConsoleBehaviour instance;

        private static DeveloperConsole developerConsole;

        private DeveloperConsole DeveloperConsole
        {
            get
            {
                if (developerConsole != null)
                {
                    return developerConsole;
                }
                return developerConsole = new DeveloperConsole(prefix, comnands);
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void Toggle(CallbackContext context)
        {
            if (!context.action.triggered)
            {
                return;
            }

            if (uiCanvas.activeSelf)
            {
                Time.timeScale = pausedTimeScale;
                uiCanvas.SetActive(false);
            }
            else
            {
                pausedTimeScale = Time.timeScale;
                Time.timeScale = 0;
                uiCanvas.SetActive(true);
                inputField.ActivateInputField();
            }
        }

        public void ProcessCommand(string inputValue)
        {
            DeveloperConsole.ProccessCommand(inputValue);

            inputField.text = string.Empty;
        }

    }
}
