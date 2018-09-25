using ArdJoystick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestesBotoes : MonoBehaviour
{
    public GameObject botaoA;

    public float forca = 350f;

    private void Start()
    {
    }

    private bool down;
    private bool press;
    private bool up;

    private void Update()
    {
        down = ArdController.GetKeyDown(ArdKeyCode.BUTTON_A);
        press = ArdController.GetKey(ArdKeyCode.BUTTON_A);
        up = ArdController.GetKeyUp(ArdKeyCode.BUTTON_A);
        if (down)
        {
            Rigidbody rb = botaoA.transform.GetChild(0).GetComponent<Rigidbody>();

            if (rb.velocity == Vector3.zero)
            {
                rb.AddForce(forca * Vector3.up);
            }
        }

        if (press)
        {
            Rigidbody rb = botaoA.transform.GetChild(1).GetComponent<Rigidbody>();

            if (rb.velocity == Vector3.zero)
            {
                rb.AddForce(forca * Vector3.up);
            }
        }

        if (up)
        {
            Rigidbody rb = botaoA.transform.GetChild(2).GetComponent<Rigidbody>();

            if (rb.velocity == Vector3.zero)
            {
                rb.AddForce(forca * Vector3.up);
            }
        }
    }
}