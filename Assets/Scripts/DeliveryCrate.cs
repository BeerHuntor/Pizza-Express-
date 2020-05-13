using UnityEngine;

public class DeliveryCrate : MonoBehaviour
{
    [SerializeField] float speed;

    private DeliverySystem _deliverySystem;
    private SpawnManager _spawnManager;
    private Rigidbody rigidbody;

    private float xMargin; 
    
    // Start is called before the first frame update
    void Start()
    {
        _deliverySystem = GameObject.Find("GameManager").GetComponent<DeliverySystem>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        rigidbody = gameObject.GetComponent<Rigidbody>();

        rigidbody.mass = 1000;

        xMargin = 14f;
        speed = 1f;

    }

    // Update is called once per frame

    void LateUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x > -xMargin)
        {
            gameObject.transform.position = new Vector3(-xMargin, transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_deliverySystem.GetCrateActive()) //Checks to see if the player already has a delivery active. If not they can get another delivery. 
            {
                Destroy(gameObject);
                _deliverySystem.GetDelivery();
                _deliverySystem.SetCrateActive(true);
                _spawnManager.CrateSpawned = false;
            }
        }

    }

}
