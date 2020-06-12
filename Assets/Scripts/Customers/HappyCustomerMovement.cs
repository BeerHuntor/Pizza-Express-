using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyCustomerMovement : MonoBehaviour
{
    public HappyCustomer customer;
    public GameObject fedSpritePrefab;

    private GameObject[] happyCustomerLocations;
    private GameObject loc1;
    private GameObject loc2;
    private GameObject loc3;
    private GameObject loc4;
    private GameObject loc5;
    private GameObject loc6;
    private GameObject loc7;
    private GameObject loc8;




    //private float movementSpeed = 5f;
    private int locationIndex;
    // Start is called before the first frame update
    private void Awake()
    {
        customer = new HappyCustomer();
    }
    void Start()
    {
        loc1 = GameObject.Find("Loc1");
        loc2 = GameObject.Find("Loc2");
        loc3 = GameObject.Find("Loc3");
        loc4 = GameObject.Find("Loc4");
        loc5 = GameObject.Find("Loc5");
        loc6 = GameObject.Find("Loc6");
        loc7 = GameObject.Find("Loc7");
        loc8 = GameObject.Find("Loc8");


        happyCustomerLocations = new GameObject[8] { loc1, loc2, loc3, loc4, loc5, loc6, loc7, loc8 };
        locationIndex = Random.Range(0, happyCustomerLocations.Length);
        //v0.5 EDITED HERE (Code for spawning the sprite gameobject)
        Instantiate(fedSpritePrefab, transform.position, fedSpritePrefab.transform.rotation);

    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameIsRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, happyCustomerLocations[locationIndex].transform.position, customer.MovementSpeed * Time.deltaTime);
            transform.LookAt(happyCustomerLocations[locationIndex].transform.position);

        }
    }

    //Deletes happy people once they reach there destination
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HappyDestination"))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Customer"))
        {
            StartCoroutine(SelectCollider());
        }

    }

    //Activates / deactivates collider after 1 second.
    IEnumerator SelectCollider()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
