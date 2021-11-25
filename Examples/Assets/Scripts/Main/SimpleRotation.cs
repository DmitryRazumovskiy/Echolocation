using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float speed;
    public Transform axisObject;

    private void FixedUpdate()
    {
        transform.RotateAround(axisObject.position, Vector3.up, speed);
    }
}
