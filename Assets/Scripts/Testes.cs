using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testes : MonoBehaviour
{
    private void Start()
    {
        Arduino.KeyDownCallback += CustomCallback;
    }

    private void CustomCallback()
    {
        Debug.Log("Processar Custom Callback");
    }
}