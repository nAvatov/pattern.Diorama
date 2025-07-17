using UnityEngine;

namespace Plugins.OSC
{
    public class OscReceiver : MonoBehaviour
    {
        private void CreateOscListeners()
        {
            var oscSettingsController = new OSC_settingsController(
                new OSC_MessageController("/H1",
                    (message) =>
                    {
                        Debug.Log("H1");
                        
                    }),
                new OSC_MessageController("/V1",
                    (message) =>
                    {
                        Debug.Log("V1");
                       
                    }),

                new OSC_MessageController("/H2",
                    (message) =>
                    {
                        Debug.Log("H2");
                        
                    }),
                new OSC_MessageController("/V2",
                    (message) =>
                    {
                        Debug.Log("V2");
                        
                    }),

                new OSC_MessageController("/H3",
                    (message) =>
                    {
                        Debug.Log("H3");
                        
                    }),
                new OSC_MessageController("/V3",
                    (message) =>
                    {
                        Debug.Log("V3");
                    })
            );
        }
    }
}
