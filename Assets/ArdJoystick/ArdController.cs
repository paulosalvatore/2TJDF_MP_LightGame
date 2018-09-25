using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

namespace ArdJoystick
{
    public class ArdController : MonoBehaviour
    {
        public static Dictionary<ArdKeyCode, ArdButton> buttons =
            new Dictionary<ArdKeyCode, ArdButton>();

        [Header("Arduino")]
        public string port = "COM3";
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
            catch (Exception) { }

            foreach (ArdKeyCode keyCode in Enum.GetValues(typeof(ArdKeyCode)))
            {
                ArdButton button = new ArdButton(keyCode);

                buttons.Add(keyCode, button);
            }

            StartCoroutine(KeysProcess());

            StartCoroutine(ProcessData());
        }

        private IEnumerator KeysProcess()
        {
            while (true)
            {
                foreach (KeyValuePair<ArdKeyCode, ArdButton> button in buttons)
                {
                    if (button.Value.processed)
                    {
                        button.Value.keyDown = false;
                        button.Value.keyUp = false;
                    }
                    else
                    {
                        button.Value.processed = true;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator ProcessData()
        {
            while (true)
            {
                try
                {
                    //string result = serialPort.ReadLine();
                    //string result = FakeArduino.ReadLine();
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
                catch (Exception) { }

                yield return new WaitForSeconds(0.1f);
            }
        }

        public static bool GetKeyDown(ArdKeyCode keyCode)
        {
            return buttons[keyCode].keyDown;
        }

        public static bool GetKey(ArdKeyCode keyCode)
        {
            return buttons[keyCode].keyPress;
        }

        public static bool GetKeyUp(ArdKeyCode keyCode)
        {
            return buttons[keyCode].keyUp;
        }

        // Simulate Arduino's ReadLine Method
        public string SimulateReadLine()
        {
            string resultData = "";
            resultData += (Input.GetKey(KeyCode.A) ? "1" : "0") + ";";
            resultData += (Input.GetKey(KeyCode.S) ? "1" : "0") + ";";
            resultData += (Input.GetKey(KeyCode.D) ? "1" : "0") + ";";
            resultData += (Input.GetKey(KeyCode.F) ? "1" : "0");

            return resultData;
        }
    }
}