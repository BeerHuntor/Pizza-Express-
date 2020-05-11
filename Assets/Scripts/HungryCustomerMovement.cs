
using UnityEngine;

public class HungryCustomerMovement : MonoBehaviour
{
    private Transform player;
    private float movementSpeed;
    private float defaultSpeed;
    private int speed;
    private GameManager _gameManager;
    private DeliverySystem _deliverySystem;

    // Start is called before the first frame update
    void Start()
    {

        defaultSpeed = movementSpeed;
        player = GameObject.Find("Player").transform;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _deliverySystem = GameObject.Find("GameManager").GetComponent<DeliverySystem>();

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
    public void SetDefaultSpeed()
    {
        movementSpeed = defaultSpeed;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public void SetMovementSpeed(float speed)
    {
        movementSpeed += speed;
    }
}
