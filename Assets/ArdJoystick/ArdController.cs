﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

namespace ArdJoystick
{
	public class ArdController : MonoBehaviour
	{
		public Dictionary<ArdKeyCode, ArdButton> buttons = new Dictionary<ArdKeyCode, ArdButton>();

		[Header("Arduino")]
		public string port = "COM4";
		public int baudRate = 9600;
		private SerialPort serialPort;

		private void Awake()
		{
			try
			{
				serialPort = new SerialPort(port, baudRate)
				{
					ReadTimeout = 10
				};
				serialPort.Open();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}

			foreach (ArdKeyCode keyCode in Enum.GetValues(typeof(ArdKeyCode)))
			{
				ArdButton button = new ArdButton(keyCode);

				buttons.Add(keyCode, button);
			}

			StartCoroutine(ProcessData());
		}

		private void LateUpdate()
		{
			// Keys Process
			foreach (KeyValuePair<ArdKeyCode, ArdButton> button in buttons)
			{
				button.Value.keyDown = false;
				button.Value.keyUp = false;
			}
		}

		public bool GetKeyDown(ArdKeyCode keyCode)
		{
			return buttons[keyCode].keyDown;
		}

		public bool GetKey(ArdKeyCode keyCode)
		{
			return buttons[keyCode].keyPress;
		}

		public bool GetKeyUp(ArdKeyCode keyCode)
		{
			return buttons[keyCode].keyUp;
		}

		private IEnumerator ProcessData()
		{
			while (true)
			{
				yield return new WaitForEndOfFrame();

				try
				{
					//string result = serialPort.ReadLine();

					// Tests
					string result = SimulateReadLine();

					string[] resultData = result.Split(';');

					ArdKeyCode[] keyCodes = (ArdKeyCode[])Enum.GetValues(typeof(ArdKeyCode));
					for (int i = 0; i < keyCodes.Length; i++)
					{
						ArdKeyCode keyCode = keyCodes[i];
						ArdButton button = buttons[keyCode];
						int data = int.Parse(resultData[i]);

						button.ProcessData(data);
					}
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}

		private void OnApplicationQuit()
		{
			try
			{
				serialPort.Close();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		#region TESTS

		[Header("Tests")]
		public float forca = 350f;

		public GameObject[] buttonsRbs;

		private string SimulateReadLine()
		{
			string result = "";
			result += (Input.GetKey(KeyCode.Z) ? "1" : "0") + ";";
			result += (Input.GetKey(KeyCode.X) ? "1" : "0") + ";";
			result += (Input.GetKey(KeyCode.C) ? "1" : "0") + ";";
			result += (Input.GetKey(KeyCode.V) ? "1" : "0");
			return result;
		}

		private void Update()
		{
			// Test for pausing the game
			if (GetKeyDown(ArdKeyCode.BUTTON_Y))
			{
				Debug.Log("Pausar");
				Time.timeScale = Time.timeScale != 0 ? 0 : 1;
			}

			ArdKeyCode[] keyCodes = (ArdKeyCode[])Enum.GetValues(typeof(ArdKeyCode));
			for (int i = 0; i < keyCodes.Length; i++)
			{
				ArdKeyCode keyCode = keyCodes[i];
				ArdButton button = buttons[keyCode];
				GameObject buttonRb = buttonsRbs[i];

				Rigidbody rbDown = buttonRb.transform.GetChild(0).GetComponent<Rigidbody>();
				Rigidbody rbPress = buttonRb.transform.GetChild(1).GetComponent<Rigidbody>();
				Rigidbody rbUp = buttonRb.transform.GetChild(2).GetComponent<Rigidbody>();

				if (GetKeyDown(keyCode)
					&& rbDown.velocity == Vector3.zero)
				{
					rbDown.AddForce(forca * Vector3.up);
				}
				else if (GetKeyUp(keyCode)
					&& rbUp.velocity == Vector3.zero)
				{
					rbUp.AddForce(forca * Vector3.up);
				}

				if (GetKey(keyCode)
					&& rbPress.velocity == Vector3.zero)
				{
					rbPress.AddForce(forca * Vector3.up);
				}
			}
		}

		#endregion TESTS
	}
}
