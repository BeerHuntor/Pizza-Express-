
using UnityEngine;

public class HungryCustomerMovement : MonoBehaviour
{
    private Transform player;
    private float defaultSpeed;
    [SerializeField] float movementSpeed;
    private GameManager _gameManager;
    private DeliverySystem _deliverySystem;

    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = 0.8f;
        defaultSpeed = movementSpeed;
        player = GameObject.Find("Player").transform;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _deliverySystem = GameObject.Find("GameManager").GetComponent<DeliverySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.GameIsRunning)
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
            _gameManager.CounterHealth--;
            _gameManager.UpdateCounterHealth(_gameManager.CounterHealth);
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

    public void IncreaseMovementSpeed(float speed)
    {
        movementSpeed += speed;
        
    }
}
