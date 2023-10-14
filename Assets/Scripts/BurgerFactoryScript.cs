using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BurgerFactoryScript : MonoBehaviour
{
    // Burger spawn amount
    public int burgersToSpawn = 0;
    // Burger spawn interval
    public float spawnInterval = 0F;
    // Prefab of burger
    public GameObject burgerPrefab;
    
    private float burgerInterval;
    private List<GameObject> borgerList;
    
    // Start is called before the first frame update
    void Start()
    {
        borgerList = new List<GameObject>();
        InvokeRepeating(nameof(spawnBurger), 1f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (burgersToSpawn == 0)
        {
            foreach (var borger in borgerList)
            {
                Destroy(borger);
            }
            borgerList.Clear();
        }
    }

    void spawnBurger()
    {
        if (borgerList.Count < burgersToSpawn)
        {
            // Create new burger
            GameObject newBurger = Instantiate(burgerPrefab, gameObject.transform.position, Quaternion.identity);
            newBurger.transform.parent = gameObject.transform;
            // Make it spawn with a bit of outwards force so it doesn't just drop
            newBurger.GetComponent<Rigidbody>().AddForce(randomVelocityVector());
            // Keep track of it in the list
            borgerList.Add(newBurger);
        }
    }

    Vector3 randomVelocityVector()
    {
        // Get random angle rotated around vertical axis
        Quaternion velocityQuackOnion = Quaternion.AngleAxis(Random.Range(0F, 360F), Vector3.up);
        // Get Vector3 representing the angle
        Vector3 velocityVector = velocityQuackOnion * Vector3.forward;
        // Multiply to make the effect stronger since we start with a unit vector
        int forceMultiplier =  (int)Random.Range(0, 50F);
        velocityVector.Scale(new Vector3(forceMultiplier, forceMultiplier, forceMultiplier));
        
        return velocityVector;
    }
}
