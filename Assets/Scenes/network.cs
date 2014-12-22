//Class for Serial and Network, connects easily to muscle propelled force feedback interface
//In Edit->Project Settings-> Player: set Api Compability level to .Net 2.0
//In the editor check that you enabled "useSerial" by ticking it.
//Connect an arduino to your serial port (make sure the arduino has code in it)
/*
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO.Ports;

public class network : MonoBehaviour {

	static TcpClient client = null;
	public bool useTCP =true;
	public bool useSerial= true;
	private static string server = "127.0.0.1";
	private static int port = 25000;
	private static SerialPort serialport;
	public string serialPortName="/dev/tty.usbmodem1421"; //usually something like COM4 in Windows
	public string serialMessage = null;
	static NetworkStream stream;
	public bool detected = false;

	// Initializes the Serial Port class and TCP Stream, if the useSerial and useTCP are respectively enabled
	void Start() {

		if (useTCP) {
			client = new TcpClient (server, port);
			stream = client.GetStream ();
		}
		if (useSerial) {
			String[] portNames = System.IO.Ports.SerialPort.GetPortNames();
			foreach (String portName in portNames)
			{
				Debug.Log(portName);
			}
			serialport = new SerialPort(serialPortName, 9600, Parity.None, 8, StopBits.One);
		 	//serialport = new SerialPort(serialPortName,9600);
			try
			{
				serialport.Open();
				detected = true;
			}
			catch (Exception ex)
			{
				Debug.Log("ERROR OPENING PORT: " + ex.Message.ToString());
			}
		}
	}

	// Send a message via Serial Port (attach an arduino to your USB port)
	public void SendSerialMessage(string message){
		if (!useSerial)
			return;
		Debug.Log("sending message through serial port");
		serialport.Write(message);
	}

	// Send a message via TCP
	public void SendNetworkMessage(string message){
		if (!useTCP)
			return;
		Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
		Debug.Log("sending message");
		stream.Write(data, 0, data.Length);
	}
	
	// Update is called once per frame & Contains a Test case for the Serial and TCP sends
	void Update () {
		if (Input.GetKeyDown (KeyCode.T))
		{
			Debug.Log("MPFF: Keyboard T detected, sending Test messages.");
			string msg = "1";
			if (serialMessage != null) 
				msg = serialMessage;

			//Test Connection by hitting the key "T" on yoru keyboard, which will send "1" to the arduino if useSerial is enabled. 
			if (useSerial) {
				SendSerialMessage(msg);
			} else Debug.Log("MPFF: useSerial disabled, not sending serial message");

			//Will also send TCP message "1" if useTCP is enabled.
			if (useTCP) {
				SendSerialMessage(msg);
			} else Debug.Log("MPFF: useTCP disabled, not sending network message");
		}
	}

	//Awake function
	void Awake() {
		DontDestroyOnLoad(this);
	}

}
*/