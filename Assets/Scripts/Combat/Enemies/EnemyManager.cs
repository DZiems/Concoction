using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one EnemyManager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
