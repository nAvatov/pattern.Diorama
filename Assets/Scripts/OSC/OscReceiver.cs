using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Plugins.OSC
{
    public class OscReceiver : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_InputField _myPort;
        [SerializeField] private TMPro.TMP_InputField _outerPort;
        [SerializeField] private TMPro.TMP_InputField _outerIp;
        
        [SerializeField] private List<CanvasGroup> _controlCanvasGroups;
        private OSC_settingsController _settingsController;
        
        private bool _wasCanceled;

        private void Start()
        {
            CreateOscListeners();
            GetDataFromConfig();
        }

        private void GetDataFromConfig()
        {
            _myPort.text = _settingsController.InPort.ToString();
            _outerPort.text = _settingsController.OutPort.ToString();
            _outerIp.text = _settingsController.OutIP;
        }

        public void SaveOscSettings()
        {
            OSC_settingsController.ChangeSettings(_myPort.text, _outerIp.text, _outerPort.text);
        }

        private void SetCurrentDefaultOscSettings(OSC_settingsController settingsController)
        {
            _myPort.text = settingsController.InPort.ToString();
            _outerPort.text = settingsController.OutPort.ToString();
            _outerIp.text = settingsController.OutIP;
        }

        private void CreateOscListeners()
        {
            _settingsController = new OSC_settingsController(
                new OSC_MessageController("/blockControl",
                    (message) =>
                    {
                        _controlCanvasGroups.ForEach(control =>
                        {
                            if (Mathf.Approximately(control.alpha, 1f))
                            {
                                control.blocksRaycasts = false;
                                control.interactable = false;
                            }
                        });
                    }),
                new OSC_MessageController("/unblockControl",
                    (message) =>
                    {
                        Debug.Log("Unblock command");
                        _controlCanvasGroups.ForEach(control =>
                        {
                            if (Mathf.Approximately(control.alpha, 1f))
                            {
                                control.blocksRaycasts = true;
                                control.interactable = true;
                            }
                        });
                    })
            );
            
            SetCurrentDefaultOscSettings(_settingsController);
        }

        private void OnDestroy()
        {
            _myPort.onSubmit.RemoveAllListeners();
            _outerPort.onSubmit.RemoveAllListeners();
            _outerIp.onSubmit.RemoveAllListeners();
        }
    }
}
