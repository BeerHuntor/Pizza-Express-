using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    private GameObject player;
    private GameManager _gameManager;
    

    private float movementSpeed = 0.8f;
    public float zMoveLimit = -20.1f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if the game is running, before issueing any movement code.
        if (_gameManager.GameIsRunning)
        {

            //Pizza Movement from its spawn position. 
            if (!transform.IsChildOf(player.transform)) 
            {
                transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
                if (transform.position.z < zMoveLimit)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, zMoveLimit);
                    transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
                }

            }
        }
    }

    void OnTriggerEnter(Collider other) {
        // Destroys pizza when it reaches the bin.
        if(other.gameObject.name == "Bin") {
            Destroy(gameObject);
            
        }
    }

}
