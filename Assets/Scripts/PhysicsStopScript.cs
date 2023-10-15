using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsStopScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(waitThenStopGravity());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator waitThenStopGravity()
    {
        yield return new WaitForSeconds(2F);
        Destroy(rb);
    }
}
