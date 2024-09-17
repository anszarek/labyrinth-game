using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class EmotionRecognition : MonoBehaviour {

    private TcpClient client;
    private NetworkStream stream;
    private StreamReader reader;
    private Thread receiveThread;
    private string emotion = "Neutral";

    public Text predictedEmotion;
    public GameObject happy;
    public GameObject sadAndFear;
    public GameObject surprise;

    void Start() {
        predictedEmotion.text = "";
        ConnectToServer();
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ConnectToServer() {
        try {
            client = new TcpClient("localhost", 65432);
            stream = client.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            Debug.Log("Connected to Python Emotion Recognition Server");
        }
        catch (Exception e) {
            Debug.LogError("Could not connect to server: " + e.Message);
        }
    }

    void ReceiveData() {
        while (client.Connected) {
            try {
                string newEmotion = reader.ReadLine();
                //Debug.Log("Emotion received from Python (raw): " + newEmotion);
                if (!string.IsNullOrEmpty(newEmotion)) {
                    lock (this) {
                        emotion = newEmotion.Trim();
                    }
                }
            }
            catch (Exception e) {
                Debug.LogError("Error receiving data: " + e.Message);
                break;
            }
        }
    }

    void Update() {
        lock (this) {
            if (!string.IsNullOrEmpty(emotion)) {
                ProcessEmotion(emotion);
            }
        }
    }

    void ProcessEmotion(string emotion) {

        //Debug.Log("Processed emotion: " + emotion);

        if (emotion == "Happy") {
            predictedEmotion.text = "Happy";
            happy.SetActive(true);
            sadAndFear.SetActive(false);
            surprise.SetActive(false);
        }
        else if (emotion == "Sad") {
            predictedEmotion.text = "Sad";
            sadAndFear.SetActive(true);
            happy.SetActive(false);
            surprise.SetActive(false);
        }
        else if (emotion == "Fear") {
            predictedEmotion.text = "Fear";
            sadAndFear.SetActive(true);
            happy.SetActive(false);
            surprise.SetActive(false);
        }
        else if (emotion == "Surprise") {
            predictedEmotion.text = "Surprise";
            surprise.SetActive(true);
            sadAndFear.SetActive(false);
            happy.SetActive(false);
        }
        else if (emotion == "Angry") {
            predictedEmotion.text = "Angry";
            sadAndFear.SetActive(false);
            happy.SetActive(false);
            surprise.SetActive(false);
        }
        else if (emotion == "Disgust") {
            predictedEmotion.text = "Disgust";
            sadAndFear.SetActive(false);
            happy.SetActive(false);
            surprise.SetActive(false);
        }
        else if (emotion == "Neutral") {
            predictedEmotion.text = "Neutral";
            sadAndFear.SetActive(false);
            happy.SetActive(false);
            surprise.SetActive(false);
        }
        else {
            predictedEmotion.text = "";
            sadAndFear.SetActive(false);
            happy.SetActive(false);
            surprise.SetActive(false);
        }
    }

    void OnApplicationQuit() {
        if (receiveThread != null && receiveThread.IsAlive) {
            receiveThread.Abort();
        }
        reader.Close();
        stream.Close();
        client.Close();
    }
}
