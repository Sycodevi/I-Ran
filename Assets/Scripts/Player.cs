using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 3.6f;
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] int dealsDamage = 15;
    [SerializeField] public int maxHealth = 200;
    public float AttackRange;
    public int Health;
    public LayerMask WhatisEnemies;
    

    //State
    public bool isAlive = true;
    bool inAir = false;
    public bool JumpPressed;

    //Cached References
    SpriteRenderer mySprite;
    Animator animatorController;
    Rigidbody2D myRigidBody;
    Transform myOrbTransform;
    BoxCollider2D boxcollider;
    CapsuleCollider2D capsulecollider;
    CapsuleCollider2D capsulecolliderofenemy;
    GameObject buttonCanvas;
    GameObject deathCanvas;
    public Transform AttackPosPlayer;
    public GUIHealthBar healthbar;

    public Joystick joystick;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        mySprite = transform.Find("Body").GetComponent<SpriteRenderer>();
        animatorController = GetComponent<Animator>();
        myOrbTransform = transform.Find("Orb").GetComponent<Transform>();
        AttackPosPlayer = GameObject.Find("Player/AttackPosPlayer").GetComponent<Transform>();
        boxcollider = GetComponent<BoxCollider2D>();
        capsulecollider = GetComponent<CapsuleCollider2D>();
        capsulecolliderofenemy = FindObjectOfType<Enemy>().GetComponent<CapsuleCollider2D>();
        buttonCanvas = GameObject.Find("Button Canvas");
        deathCanvas = GameObject.Find("Death Canvas");
        Health = maxHealth;
        healthbar.sethealth(Health, maxHealth);
}

    void Update()
    {
        if (isAlive)
        {
            Die();
            Run();
            Flip();
            PlayerinAirCondition();
        }
    }

    private void PlayerinAirCondition()
    {
        if (inAir)
        {
            PlayerinAir();
        }
    }

    //Triggers Jump anim if is in air or tilemap.
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Foreground")
        {
            inAir = false;
            Debug.Log("Touching Foreground");
            animatorController.SetBool("isJumping", false);
            animatorController.SetBool("isFalling", false);
        }
        if (collision.gameObject.name == "Platform")
        {
            Debug.Log(GetComponent<Rigidbody2D>().velocity);
            inAir = false;
            animatorController.SetBool("isJumping", false);
            animatorController.SetBool("isFalling", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Foreground")
        {
            Debug.Log("Not Touching Foreground");
            inAir = true;
        }
        if (collision.gameObject.name == "Platform")
        {
            inAir = true;
        }
    }

    private void PlayerinAir()
    {
        if (myRigidBody.velocity.y > 1)
        {
            animatorController.SetBool("isJumping", true);
            animatorController.SetBool("isFalling", false);
        }
        else if (myRigidBody.velocity.y < -2.8)
        {
            animatorController.SetBool("isFalling", true);
            animatorController.SetBool("isJumping", false);
        }
    }

    private void Run()
    {
       
        float controlThrow = joystick.Horizontal; //values is between -1 and 1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        animatorController.SetBool("isMoving", hasHorizontalSpeed); //if has velocity on x-axis, trigger run animation.

        if (hasHorizontalSpeed) 
        { 
            animatorController.ResetTrigger("Execute");
        }
    }
    public void Jump()
    {
        //can also use OnCollisionEnter2D
        if (!boxcollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Debug.Log("Is not touching"); return;
        }
        Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, jumpSpeed);
        myRigidBody.velocity = jumpVelocityToAdd;
    }
    public void Attack()
    {
        animatorController.SetTrigger("Execute");
    }

    private void Flip()
    {
        if (joystick.Horizontal > 0)
        {
            mySprite.flipX = false;
            myOrbTransform.transform.localPosition = new Vector2(-0.5f, 1.1f);
            AttackPosPlayer.transform.localPosition = new Vector2(.8f, 0.6f);
        }
        else if (joystick.Horizontal < 0)
        {
            mySprite.flipX = true;
            myOrbTransform.transform.localPosition = new Vector2(0.5f, 1.1f);
            AttackPosPlayer.transform.localPosition = new Vector2(-1f, 0.6f);
        }
    }
    private void DealDamage()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(AttackPosPlayer.position, AttackRange, WhatisEnemies); Debug.Log("DealDamage Initiated");
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if(enemiesToDamage[i].name.Contains("Normal"))
            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(dealsDamage); Debug.Log("Dealt Damage to Enemy: " + enemiesToDamage[i]);

            if(enemiesToDamage[i].name.Contains("Ghost"))
            enemiesToDamage[i].GetComponent<EnemyGhost>().TakeDamage(dealsDamage); Debug.Log("Dealt Damage to Enemy Ghost: " + enemiesToDamage[i]);
        }
    }
    private void Die()
    {
        if (Health <= 0)
        {
            capsulecollider.enabled = false;
            isAlive = false;
            animatorController.Play("King Die");
            buttonCanvas.SetActive(false);
            deathCanvas.SetActive(true);
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosPlayer.position, AttackRange);
    }
    

}
