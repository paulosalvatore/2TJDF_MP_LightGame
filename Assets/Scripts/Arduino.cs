using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Arduino : MonoBehaviour
{
    internal SerialPort arduino;

    public static Action KeyDownCallback;

    private void Start()
    {
        StartCoroutine(ProcessarFakeData());
    }

    private void ConectarArduino()
    {
        string[] portas = SerialPort.GetPortNames();

        foreach (string porta in portas)
        {
            Debug.Log(porta);
        }

        string portaConectar = "COM3";
        int baudRate = 9600;

        try
        {
            arduino = new SerialPort(portaConectar, baudRate)
            {
                ReadTimeout = 500
            };

            arduino.Open();

            if (arduino.IsOpen)
            {
                Debug.Log("Conexão realizada com sucesso.");
            }
            else
            {
                Debug.Log("Conexão falhou.");
            }

            StartCoroutine(LerArduino());
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    private IEnumerator LerArduino()
    {
        while (true)
        {
            try
            {
                arduino.BaseStream.Flush();
                string leitura = arduino.ReadLine();
                Debug.Log("Leitura: " + leitura);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Leitura falhou: " + e);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ProcessarFakeData()
    {
        bool botaoPressionado = false;
        bool botaoPressionadoAnterior = false;

        while (true)
        {
            string fakeData = FakeArduino.ReadLine();
            Debug.Log(fakeData);

            string[] separatedData = fakeData.Split(';');

            botaoPressionado = (separatedData[0] == "1");

            // Checagem aqui
            if (botaoPressionado && !botaoPressionadoAnterior)
            {
                // KeyDown
                //Action handler = KeyDownCallback;

                if (KeyDownCallback != null)
                    KeyDownCallback();
            }
            else if (botaoPressionado && botaoPressionadoAnterior)
            {
                // KeyPress
                Debug.Log("KeyPress");
            }
            else if (!botaoPressionado && botaoPressionadoAnterior)
            {
                // KeyUp
                Debug.Log("KeyUp");
            }

            botaoPressionadoAnterior = botaoPressionado;

            yield return new WaitForSeconds(2f);
        }
    }
}