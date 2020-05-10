using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyCustomerMovement : MonoBehaviour
{
    private GameObject[] happyCustomerLocations;
    private GameObject loc1;
    private GameObject loc2;
    private GameObject loc3;
    private GameObject loc4;
    private GameObject loc5;
    private GameObject loc6;
    private GameObject loc7;
    private GameObject loc8;

    private GameManager _gameManager;


    private float movementSpeed = 5f;
    private int locationIndex;
    // Start is called before the first frame update
    void Start() {
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
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }
    // Update is called once per frame
    void Update()
    {
        if (_gameManager.gameIsRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, happyCustomerLocations[locationIndex].transform.position, movementSpeed * Time.deltaTime);
            transform.LookAt(happyCustomerLocations[locationIndex].transform.position);
        }
    }

    //Deletes happy people once they reach there destination
    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("HappyDestination")) {
            Destroy(gameObject);
        }
        //Turns off the collider of the happy customer if collides with another customer. 
        if (other.CompareTag("Customer"))
        {
            StartCoroutine(SelectCollider());
        }
    }
    //Activates / deactivates collider after 1 second.
    IEnumerator SelectCollider()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
