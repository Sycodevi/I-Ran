using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //config
    [SerializeField] float PlatformSpeed = 1f;
    public GameObject player;
    bool moveright = true;

    //Cache
    Rigidbody2D myRigidBody;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }
    private void Update()
    {
        PlatformMove();

        if(moveright)
            transform.position = new Vector2(transform.position.x + PlatformSpeed * Time.deltaTime, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - PlatformSpeed * Time.deltaTime, transform.position.y);
    }
    private void PlatformMove()
    {
        if(transform.position.x > 97)
        {
            moveright = false;
        }
        if(transform.position.x < 87)
        {
            moveright = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("Player Stepped on platform");
            collision.collider.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("Player has stepped off the platform");
            collision.collider.transform.SetParent(null);
        }
    }
}
