using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed;

    void Update()
    {
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime); //rotates 50 degrees per second around z axis
    }
}
