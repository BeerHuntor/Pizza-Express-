
using UnityEngine;

public class HungryCustomerMovement : MonoBehaviour
{
    private Transform player;
    public float movementSpeed;

    //TODO - ANIMATION OF THE CHARACTER MODEL THROUGHOUT. 

    private GameManager _gameManager;

        // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.gameIsRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
            transform.LookAt(player);
        }
    }

    //checks for contact with the counter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Counter"))
        {
            _gameManager.counterHealth--;
            _gameManager.UpdateCounterHealth(_gameManager.counterHealth);
        }
    }
}
