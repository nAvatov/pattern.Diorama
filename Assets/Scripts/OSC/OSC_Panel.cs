using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSC_Panel : MonoBehaviour
{
    public InputField InnerPort;
    public InputField OutIP;
    public InputField OutPort;


    void Start()
    {
        if (OSC_settingsController.Instance == null){
            new OSC_settingsController(true);
        }
        InnerPort.text = OSC_settingsController.Instance.InPort.ToString();
        OutIP.text = OSC_settingsController.Instance.OutIP.ToString();
        OutPort.text = OSC_settingsController.Instance.OutPort.ToString();
    }

    public void Submit(){
        try{
            OSC_settingsController.ChangeSettings(InnerPort.text, OutIP.text, OutPort.text);
        } catch (System.Exception e){
            Debug.LogError(e);
        }
    }
}
