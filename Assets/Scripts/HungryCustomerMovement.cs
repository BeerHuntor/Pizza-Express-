
using TMPro.EditorUtilities;
using UnityEngine;

public class HungryCustomerMovement : MonoBehaviour
{
    private GameManager _gameManager;
    private UIManager _uiManager;

    private Transform player;
    private float defaultSpeed;
    private float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        player = GameObject.Find("Player").transform;
        
        movementSpeed = 0.8f;
        defaultSpeed = movementSpeed;
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
            _uiManager.UpdateCounterHealth(_gameManager.CounterHealth);
        }
    }
    public void SetDefaultSpeed()
    {
        movementSpeed = defaultSpeed;
    }

    public void IncreaseMovementSpeed(float speed)
    {
        movementSpeed += speed;
        
    }
}
