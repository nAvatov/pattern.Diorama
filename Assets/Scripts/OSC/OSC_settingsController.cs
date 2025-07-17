using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OSC_settingsController
{
    public static OSC_settingsController Instance { get; private set; }
    public int InPort;
    public string OutIP;
    public int OutPort;

    private OSC osc;

    public OSC_settingsController(bool trigger){
        Application.runInBackground = true;
        if (trigger){
            Debug.Log("Starting read json file");
            using (StreamReader r =
                new StreamReader(
                    Path.Combine(Application.streamingAssetsPath, "osc_settings.json")))
            {
                string json = r.ReadToEnd();
                OSC_settingsController temp = JsonUtility.FromJson<OSC_settingsController>(json);
                Instance = temp;
            }
        }

        ChangeOSC();
    }

    /// <summary>
    /// Конструктор с реакциями на получения OSC сообщения
    /// </summary>
    /// <param name="message_controllers">Контроллер OSC сообщений</param>
    /// <returns>Экземпляр класса с предзаписанными реакциями на обращения</returns>
    public OSC_settingsController(params OSC_MessageController[] message_controllers): this (true){
        if (Instance.osc != null){
            for (int i = 0; i < message_controllers.Length; i++){
                int a = i;
                Instance.osc.SetAddressHandler(message_controllers[a].Address, (message) => {
                    message_controllers[a].Reaction(message);
                });
            }
        } else {
            Debug.LogError("Somehow instance osc is null");
        }
    }

    // public static OSC_settingsController GetFromJson (string json_input){
    //     return JsonUtility.FromJson<OSC_settingsController>(json_input);
    // }

    private static void ChangeOSC(){
        if (Instance.osc == null){
            GameObject osc_go = new GameObject();
            Instance.osc = osc_go.AddComponent<OSC>();
            GameObject.DontDestroyOnLoad(osc_go);
        } else {
            Debug.Log("Change osc");
            // Instance.osc = new OSC();
            Instance.osc.ImplementSettings(Instance);
        }
        // if (Instance.osc == null) Debug.LogError("WTF!?!!?");
    }

    /// <summary>
    /// Change settings in osc object and json file
    /// Изменить настройки в osc объекте и json файле
    /// </summary>
    /// <param name="inner_port">Внутренний порт</param>
    /// <param name="outer_ip">Внешний IP</param>
    /// <param name="outer_port">Внешний порт</param>
    public static void ChangeSettings(string inner_port, string outer_ip, string outer_port){
        Instance.InPort = Int32.Parse(inner_port);
        Instance.OutIP = outer_ip;
        Instance.OutPort = Int32.Parse(outer_port);
        SaveJson();

        ChangeOSC();
    }

    private static void SaveJson(){
        using (StreamWriter w =
            new StreamWriter(
                Path.Combine(Application.streamingAssetsPath, "osc_settings.json"), false))
        {
            string json = JsonUtility.ToJson(Instance);
            w.WriteLine(json);
        }
    }

    /// <summary>
    /// Отправить сообщение
    /// </summary>
    /// <param name="address">Адрес сообщения</param>
    /// <param name="value">Отправляемое значени</param>
    public static void SendOscMessage(string address, int value)
    {
        if (Instance.osc != null){
            OscMessage message = new OscMessage();
            message.address = address;
            message.values.Add(value);
            Debug.Log("Sending message to " + Instance.osc.outIP + " on port " + Instance.osc.outPort);
            Debug.Log("message address is " + message.address + " message value is " + message.values);
            Instance.osc.Send(message);
        } else {
            Debug.LogError("osc instance is null");
        }
    }

    public static void SendOscMessageWithString(string address, string value)
    {
        if (Instance.osc != null){
            OscMessage message = new OscMessage();
            message.address = address;
            message.values.Add(value);
            Debug.Log("Sending message to " + Instance.osc.outIP + " on port " + Instance.osc.outPort);
            Debug.Log("message address is " + message.address + " message value is " + message.values);
            Instance.osc.Send(message);
        } else {
            Debug.LogError("osc instance is null");
        }
    }

    public static void SendOscMessageWithArray(string address, int[] arr)
    {
        if (Instance.osc != null){
            OscMessage message = new OscMessage();
            message.address = address;

            foreach(int code in arr) {
                message.values.Add(code);
            }
            
            Debug.Log("Sending message to " + Instance.osc.outIP + " on port " + Instance.osc.outPort);
            Debug.Log("message address is " + message.address + " message value is " + message.values);
            Instance.osc.Send(message);
        } else {
            Debug.LogError("osc instance is null");
        }
    }
}

public delegate void OSC_MessageReceive(OscMessage message);
public class OSC_MessageController{
    public string Address {get; private set;}
    public OSC_MessageReceive Reaction{get; private set;}

    public OSC_MessageController(string address, params OSC_MessageReceive[] reactions){
        for(int i = 0; i < reactions.Length; i++ ){
            Reaction += reactions[i];
        }
        Address = address;
    }
}

public static class OSC_Extention{
    /// <summary>
    /// Changing osc settings by settingsController
    /// </summary>
    /// <param name="osc"></param>
    /// <param name="settings"></param>
    public static void ImplementSettings(this OSC osc, OSC_settingsController settings){
        if (OSC_settingsController.Instance != null){
            // osc.inPort = Int32.Parse(settings.InPort);
            Debug.Log("InPort is :" + settings.InPort + " || OutPort is " + settings.OutPort + " || OutIp is " + settings.OutIP);
            osc.inPort = settings.InPort;
            osc.outPort = settings.OutPort;
            osc.outIP = settings.OutIP;
            // Debug.Log("InPort is :" + osc.inPort + " || OutPort is " + osc.outPort + " || OutIp is " + osc.outIP);
            if (osc.OscPacketIO != null && osc.OscPacketIO.IsOpen())
            {
                Debug.Log("Close OscPacketIO");
                osc.OscPacketIO.Close();
            }
            if (osc.OscPacketIO != null && !osc.OscPacketIO.IsOpen())
            {
                Debug.Log("Open OscPacketIO");
                osc.OscPacketIO = new UDPPacketIO(osc.outIP, osc.outPort, osc.inPort);
                osc.OscPacketIO.Open();
            }
        }
    }
}
