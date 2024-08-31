using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BluetoothController : MonoBehaviour
{
    public bool isScanningDevices = false;
    public bool isScanningServices = false;
    public bool isScanningCharacteristics = false;
    public bool isSubscribed = false;
    public Text deviceScanButtonText;
    public Text deviceScanStatusText;
    public GameObject deviceScanResultProto;
    public Button serviceScanButton;
    public Text serviceScanStatusText;
    public Dropdown serviceDropdown;
    public Button characteristicScanButton;
    public Text characteristicScanStatusText;
    public Dropdown characteristicDropdown;
    public Button subscribeButton;
    public Text subcribeText;
    public Button writeButton;
    public InputField writeInput;
    public Text errorText;

    public string targetDeviceName = "Polar H9 D983B628";

    Transform scanResultRoot;
    public string selectedDeviceId;
    public string selectedServiceId;
    Dictionary<string, string> characteristicNames = new Dictionary<string, string>();
    public string selectedCharacteristicId;
    Dictionary<string, Dictionary<string, string>> devices = new Dictionary<string, Dictionary<string, string>>();
    string lastError;

    // Start is called before the first frame update
    void Start()
    {
        scanResultRoot = deviceScanResultProto.transform.parent;
        deviceScanResultProto.transform.SetParent(null);
        StartStopDeviceScan();
    }

    // Update is called once per frame
    void Update()
    {
        BleApi.ScanStatus status;
        if (isScanningDevices)
        {
            BleApi.DeviceUpdate res = new BleApi.DeviceUpdate();
            do
            {
                status = BleApi.PollDevice(ref res, false);
                if (status == BleApi.ScanStatus.AVAILABLE)
                {
                    if (!devices.ContainsKey(res.id))
                        devices[res.id] = new Dictionary<string, string>() {
                            { "name", "" },
                            { "isConnectable", "False" }
                        };
                    if (res.nameUpdated)
                        devices[res.id]["name"] = res.name;
                    if (res.isConnectableUpdated)
                        devices[res.id]["isConnectable"] = res.isConnectable.ToString();
                    // consider only devices which have a name and which are connectable
                    if (devices[res.id]["name"] != "" && devices[res.id]["isConnectable"] == "True")
                    {
                        // add new device to list
                        GameObject g = Instantiate(deviceScanResultProto, scanResultRoot);
                        g.name = res.id;
                        g.transform.localScale = new Vector3(0.99f, 0.99f, 0.99f);
                        g.transform.GetChild(0).GetComponent<Text>().text = devices[res.id]["name"];
                        g.transform.GetChild(1).GetComponent<Text>().text = res.id;

                        // Automatically select the device with the target name
                        if (devices[res.id]["name"] == targetDeviceName) {
                            SelectDevice(g); // Automatically select the target device
                        }
                    }
                }
                else if (status == BleApi.ScanStatus.FINISHED)
                {
                    isScanningDevices = false;
                    deviceScanButtonText.text = "Scan devices";
                    deviceScanStatusText.text = "finished";
                }
            } while (status == BleApi.ScanStatus.AVAILABLE);
        }
        if (isScanningServices)
        {
            BleApi.Service res = new BleApi.Service();
            do
            {
                status = BleApi.PollService(out res, false);
                if (status == BleApi.ScanStatus.AVAILABLE)
                {
                    serviceDropdown.AddOptions(new List<string> { res.uuid });
                    //////////////////////////// first option gets selected
                    //////////////////////////if (serviceDropdown.options.Count == 1)
                    //////////////////////////    SelectService(serviceDropdown.gameObject);
                    // Automatically select the target service and start characteristic scan
                    if (res.uuid.Contains("0000180d")) { //== "{0000180d-0000-1000-8000-00805f9b34fb}"
                        selectedServiceId = res.uuid;
                        characteristicScanButton.interactable = true;
                        StartCharacteristicScan();
                    }
                }
                else if (status == BleApi.ScanStatus.FINISHED)
                {
                    isScanningServices = false;
                    serviceScanButton.interactable = true;
                    serviceScanStatusText.text = "finished";
                }
            } while (status == BleApi.ScanStatus.AVAILABLE);
        }
        if (isScanningCharacteristics)
        {
            BleApi.Characteristic res = new BleApi.Characteristic();
            do
            {
                status = BleApi.PollCharacteristic(out res, false);
                if (status == BleApi.ScanStatus.AVAILABLE)
                {
                    string name = res.userDescription != "no description available" ? res.userDescription : res.uuid;
                    characteristicNames[name] = res.uuid;
                    characteristicDropdown.AddOptions(new List<string> { name });
                    //////////////////////// first option gets selected
                    //////////////////////if (characteristicDropdown.options.Count == 1)
                    //////////////////////    SelectCharacteristic(characteristicDropdown.gameObject);
                    // Automatically select the target characteristic and subscribe
                    if (res.uuid.Contains("00002a37") ) {  //== "00002a37-0000-1000-8000-00805f9b34fb"
                        selectedCharacteristicId = res.uuid;
                        subscribeButton.interactable = true;
                        Subscribe();
                    }
                }
                else if (status == BleApi.ScanStatus.FINISHED)
                {
                    isScanningCharacteristics = false;
                    characteristicScanButton.interactable = true;
                    characteristicScanStatusText.text = "finished";
                }
            } while (status == BleApi.ScanStatus.AVAILABLE);
        }
        if (isSubscribed)
        {
            BleApi.BLEData res = new BleApi.BLEData();
            while (BleApi.PollData(out res, false))
            {
                //subcribeText.text = BitConverter.ToString(res.buf, 0, res.size);
                // subcribeText.text = Encoding.ASCII.GetString(res.buf, 0, res.size);
                // Ensure there are enough bytes in the data buffer
                if (res.size >= 4) // Assuming the heart rate is the second byte
                {
                    // Extract the heart rate from the second byte of the data buffer
                    int heartRate = res.buf[1]; // Get the second byte (index 1) as heart rate
                    subcribeText.text = heartRate.ToString(); // Display heart rate in decimal
                }
            }


        }
        {
            // log potential errors
            //BleApi.ErrorMessage res = new BleApi.ErrorMessage();
            //BleApi.GetError(out res);
            //if (lastError != res.msg)
            //{
            //    Debug.LogError(res.msg);
            //    errorText.text = res.msg;
            //    lastError = res.msg;
            //}
        }
    }



    private void OnApplicationQuit()
    {
        BleApi.Quit();
    }

    public void StartStopDeviceScan()
    {
        if (!isScanningDevices)
        {
            // start new scan
            for (int i = scanResultRoot.childCount - 1; i >= 0; i--)
                Destroy(scanResultRoot.GetChild(i).gameObject);
            BleApi.StartDeviceScan();
            isScanningDevices = true;
            deviceScanButtonText.text = "Stop scan";
            deviceScanStatusText.text = "scanning";
        }
        else
        {
            // stop scan
            isScanningDevices = false;
            BleApi.StopDeviceScan();
            deviceScanButtonText.text = "Start scan";
            deviceScanStatusText.text = "stopped";
        }
    }

    public void SelectDevice(GameObject data)
    {
        for (int i = 0; i < scanResultRoot.transform.childCount; i++)
        {
            var child = scanResultRoot.transform.GetChild(i).gameObject;
            child.transform.GetChild(0).GetComponent<Text>().color = child == data ? UnityEngine.Color.red :
                deviceScanResultProto.transform.GetChild(0).GetComponent<Text>().color;
        }
        selectedDeviceId = data.name;
        serviceScanButton.interactable = true;

        // Automatically start service scan
        StartServiceScan();
    }

    public void StartServiceScan()
    {
        if (!isScanningServices)
        {
            // start new scan
            serviceDropdown.ClearOptions();
            BleApi.ScanServices(selectedDeviceId);
            isScanningServices = true;
            serviceScanStatusText.text = "scanning";
            serviceScanButton.interactable = false;
        }
    }

    public void SelectService(GameObject data)
    {
        selectedServiceId = serviceDropdown.options[serviceDropdown.value].text;
        characteristicScanButton.interactable = true;
    }
    public void StartCharacteristicScan()
    {
        if (!isScanningCharacteristics)
        {
            // start new scan
            characteristicDropdown.ClearOptions();
            BleApi.ScanCharacteristics(selectedDeviceId, selectedServiceId);
            isScanningCharacteristics = true;
            characteristicScanStatusText.text = "scanning";
            characteristicScanButton.interactable = false;
        }
    }

    public void SelectCharacteristic(GameObject data)
    {
        string name = characteristicDropdown.options[characteristicDropdown.value].text;
        selectedCharacteristicId = characteristicNames[name];
        subscribeButton.interactable = true;
        writeButton.interactable = true;
    }

    public void Subscribe()
    {
        // no error code available in non-blocking mode
        BleApi.SubscribeCharacteristic(selectedDeviceId, selectedServiceId, selectedCharacteristicId, false);
        isSubscribed = true;
    }

    public void Write()
    {
        byte[] payload = Encoding.ASCII.GetBytes(writeInput.text);
        BleApi.BLEData data = new BleApi.BLEData();
        data.buf = new byte[512];
        data.size = (short)payload.Length;
        data.deviceId = selectedDeviceId;
        data.serviceUuid = selectedServiceId;
        data.characteristicUuid = selectedCharacteristicId;
        for (int i = 0; i < payload.Length; i++)
            data.buf[i] = payload[i];
        // no error code available in non-blocking mode
        BleApi.SendData(in data, false);
    }
}
