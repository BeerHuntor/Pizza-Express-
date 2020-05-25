using UnityEngine;

public class DeliveryCrate : MonoBehaviour
{
    [SerializeField] float speed;

    private Rigidbody boxRb;

    private float xMargin; 
    
    // Start is called before the first frame update
    void Start()
    {
        boxRb = gameObject.GetComponent<Rigidbody>();

        boxRb.mass = 1000;

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
            if (!DeliverySystem.instance.DeliveryActive && !DeliverySystem.instance.DoubleSlicesActive) //Checks to see if the player already has a delivery active. If not they can get another delivery. 
            {
                DeliverySystem.instance.StartCrateStatus();
                DeliverySystem.instance.GetDelivery();
                SpawnManager.instance.CrateSpawned = false;
                Destroy(gameObject);
            }
        }

    }

}
