using System;
using UnityEngine;

public class TacoTrigger : MonoBehaviour
{
    public bool hit = false;

    private void OnTriggerEnter(Collider other)
    {
        hit = true;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * (Time.deltaTime * 100));
    }
}