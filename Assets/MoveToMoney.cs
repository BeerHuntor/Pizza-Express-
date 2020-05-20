using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMoney : MonoBehaviour
{
    private GameObject customerSpawner;
    private void Start()
    {
        customerSpawner = GameObject.Find("EnemySpawn3");
    }
    // Update is called once per frame
    void Update()
    {
        if (customerSpawner != null)
        {
            Debug.LogWarning("Attempted to deploy particle");
            transform.Translate(Vector3.MoveTowards(transform.position, customerSpawner.transform.position, 2000f));
        } else
        {
            Debug.LogError("Could not find moneyUI!");
            return;
        }
    }
}
