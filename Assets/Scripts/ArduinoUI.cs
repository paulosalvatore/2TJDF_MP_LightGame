using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class ArduinoUI : MonoBehaviour
{
    [Header("Arduino")]
    public int baudRate = 9600;
    internal SerialPort arduino;
    private Coroutine readingPort;

    [Header("Ports List")]
    public Dropdown portsListDropdown;
    private string[] availablePorts;

	void Start()
    {
        UpdatePortsList();
	}

	public void UpdatePortsList()
    {
        availablePorts = SerialPort.GetPortNames();

        portsListDropdown.ClearOptions();

        var portsList = new List<Dropdown.OptionData>();

        if (availablePorts.Length > 0)
        {
            portsListDropdown.enabled = true;

            foreach (var portName in availablePorts)
            {
                portsList.Add(new Dropdown.OptionData(portName));
            }
        }
        else
        {
            portsListDropdown.enabled = false;

            portsList.Add(new Dropdown.OptionData("No ports found."));
        }

        portsListDropdown.AddOptions(portsList);
	}

    public void ConnectSelectedPort()
    {
        string selectedPortName = portsListDropdown.options[portsListDropdown.value].text;

        try
        {
            arduino = new SerialPort(selectedPortName, baudRate)
            {
                ReadTimeout = 10
            };

            arduino.Open();

            readingPort = StartCoroutine(ReadPort());
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    IEnumerator ReadPort()
    {
        while (true)
        {
            try
            {
                string readLine = arduino.ReadLine();

                Debug.Log(readLine);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
