using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothMovment : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;
    public float smoothTime = 0.3F;
    public dialogeController dialogeProcessor;

    public AudioSource External;
    public AudioSource Internal;
    public AudioSource Forest;
    private Vector3 velocity = Vector3.zero;
    private bool active = false;

    private void FixedUpdate()
    {
        if (active)
        {
            // Define a target position above and behind the target transform
            Vector3 targetPosition = target.TransformPoint(offset);

            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
    
    public void callDiolage(int callNum)
    {
        dialogeProcessor.StartDialoge(callNum);
    }

    public void fadeTitle()
    {
        transform.GetChild(0).GetComponent<Fade>().fade();
    }

    public void On()
    {
        active = true;
        target.GetComponent<Player_Main>().enabled = true;
        External.Stop();
        Internal.volume = 0.2f;
    }
}
