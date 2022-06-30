using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    //config
    [Header("Config")]
    [SerializeField] float movespeed = 1.2f;
    [SerializeField] int dealsDamage = 50;
    public int maxHealth = 150;
    public int Health;
    public Transform AttackPos;
    public float AttackRange;
    public LayerMask WhatisEnemies;
    public Player player;
    public HealthbarBehavior healthbar;
    public GameObject bloodEffect;

    //cache
    Rigidbody2D myRigidBody;
    SpriteRenderer mySprite;
    Animator animatorController;
    Transform playerTransform;
    BoxCollider2D boxcollider;
    CapsuleCollider2D capsulecollider;
    BoxCollider2D boxcolliderchild;
    CircleCollider2D circlecollider;

    //state
    bool isTriggered = false;
    bool isTriggerActivated = false;
    bool isAttacking = false;
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponentInChildren<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<Animator>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        boxcollider = GetComponent<BoxCollider2D>();
        boxcolliderchild = transform.Find("Body").GetComponent<BoxCollider2D>();
        capsulecollider = GetComponent<CapsuleCollider2D>();
        circlecollider = transform.Find("radar").GetComponent<CircleCollider2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
        Health = maxHealth;
        healthbar.sethealth(Health, maxHealth);
    }
   

    // Update is called once per frame
    void Update()
    {
        if (player.isAlive)
        {
            if (isAlive)
            {
                Die();
                if (isTriggered && !isTriggerActivated)
                {
                    InvokeRepeating("Move", 0f, .1f);
                    isTriggerActivated = true;
                }

                Jump();
                animControl();
                Flip();
            }
        }
        else if (!player.isAlive)
        {
            CancelInvoke("Move");
            animatorController.SetBool("isMoving", false);
        }

    }
    private void Move()
    {
        if (!isAttacking)
        {
            if (playerTransform.position.x > transform.position.x)
            {
                myRigidBody.velocity = new Vector2(movespeed, myRigidBody.velocity.y);
            }
            else if (playerTransform.position.x < transform.position.x)
            {
                myRigidBody.velocity = new Vector2(-movespeed, myRigidBody.velocity.y);
            }
        }
    }
    private void Flip()
    {
        if (myRigidBody.velocity.x >= 1)
        {
            mySprite.flipX = false;
            AttackPos.transform.localPosition = new Vector2(0.5f, 0);
        }
        else if (myRigidBody.velocity.x <= -1)
        {
            mySprite.flipX = true;
            AttackPos.transform.localPosition = new Vector2(-0.6f, 0);
        }
    }

    private void Jump()
    {
        if (capsulecollider.IsTouchingLayers(LayerMask.GetMask("Ground"))
            && !boxcollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidBody.velocity = new Vector2(movespeed, 8f); //If body collides and vision does not collide then jump
        }
    }
    private void animControl()
    {
        if (boxcolliderchild.IsTouchingLayers(LayerMask.GetMask("Ground"))
            && myRigidBody.velocity.magnitude > 0.5f)
        {
            animatorController.SetBool("isMoving", true);
        }
        else
        {
            animatorController.SetBool("isMoving", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (circlecollider.IsTouching(collision))
        {
            if (collision.gameObject.tag == "Player")
                isTriggered = true;
        }
        if (boxcollider.IsTouching(collision))
        {
            if (collision.gameObject.tag == "Player")
            {
                animatorController.SetBool("Attack", true);
                isAttacking = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animatorController.SetBool("Attack", false);
            isAttacking = false;
        }
    }
    private void Die()
    {
        if(Health <= 0)
        {
            CancelInvoke("Move");
            boxcollider.enabled = false;
            circlecollider.enabled = false;
            capsulecollider.enabled = false;
            isAlive = false;
            animatorController.Play("Enemy Die");
            transform.Find("Canvas").gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        healthbar.sethealth(Health, maxHealth);
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
    }
    private void DealDamage()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, WhatisEnemies); Debug.Log("DealDamage Initiated");
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Player>().TakeDamage(dealsDamage); Debug.Log("Dealt Damage to Enemy: " + enemiesToDamage[i]);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, AttackRange);
    }

}