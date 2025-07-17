using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OscSenderComponent : MonoBehaviour
{
    [SerializeField] private string _address;
    [SerializeField] private int _intValue;
    [SerializeField] private string _stringValue;
    
    public void SendOscMessage() {
        Debug.Log("Sending message to " + _address + " with value " +_intValue);
        OSC_settingsController.SendOscMessage(_address, _intValue);
    }

    public void SendOscMessageWithString() {
        Debug.Log("Sending message to " + _address + " with value " + _stringValue);
        OSC_settingsController.SendOscMessageWithString(_address, _stringValue);
    }
}
