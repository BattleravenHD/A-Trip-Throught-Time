using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCityOffset : MonoBehaviour
{
    [SerializeField] private float parrallaxEffectMultiplier;
    [SerializeField] private Transform camTransform;
    private Vector3 lastcamLocation;

    void Start()
    {
        lastcamLocation = camTransform.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltamovement = camTransform.position - lastcamLocation;
        Vector3 newlocation = transform.position + new Vector3(deltamovement.x * parrallaxEffectMultiplier, 0, 0);
        transform.position = newlocation;
        lastcamLocation = camTransform.transform.position;
    }
}
