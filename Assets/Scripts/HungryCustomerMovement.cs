using UnityEngine;

public class HungryCustomerMovement : MonoBehaviour
{

    private Transform player;
    private float defaultSpeed;
    private float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        
        movementSpeed = 0.8f;
        defaultSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameIsRunning)
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
            GameManager.instance.CounterHealth--;
            UIManager.instance.UpdateCounterHealth(GameManager.instance.CounterHealth);
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
