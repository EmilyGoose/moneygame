using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurgerFactoryScript : MonoBehaviour
{
    // Burger spawn amount
    public int burgersToSpawn = 0;
    // Prefab of burger
    public GameObject burgerPrefab;
    
    private float burgerInterval;
    private List<GameObject> borgerList;
    
    // Start is called before the first frame update
    void Start()
    {
        borgerList = new List<GameObject>();
        InvokeRepeating(nameof(spawnBurger), 1f, 1f);
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
            // Todo position of burger spawner
            borgerList.Add(Instantiate(burgerPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        }
    }
}
