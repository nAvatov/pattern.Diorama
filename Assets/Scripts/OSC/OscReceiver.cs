using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.OSC
{
    public class OscReceiver : MonoBehaviour
    {
        [SerializeField] private List<CanvasGroup> _controlCanvasGroups;

        private void Start()
        {
            CreateOscListeners();
        }

        private void CreateOscListeners()
        {
            var oscSettingsController = new OSC_settingsController(
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
        }
    }
}
