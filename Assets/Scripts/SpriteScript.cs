using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScript : MonoBehaviour
{
    private float speed = 3f;
    private float movementBeforeDestroy = 3f;
    private float positionZ;

    private void Start()
    {
        positionZ = transform.position.z;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < positionZ + movementBeforeDestroy)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
}
