using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region movement_variables
    public float movespeed;
    float x_input;
    float y_input;
    float boost_timer;
    float originalms;
    #endregion

    #region attack_variables
    public float damage;
    public float attackspeed;
    float attackTimer;
    public float hitboxTiming;
    public float endAnimationTiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region health_variables
    public float maxHealth;
    float currHealth;
    public Slider hpSlider;
    #endregion

    #region physics_components
    Rigidbody2D playerRB;
    #endregion

    #region animation_components
    Animator anim;
    #endregion

    #region Unity_functions
    //Called once on creation
    private void Awake(){
    	playerRB = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        attackTimer = 0;

        currHealth = maxHealth;

        hpSlider.value = currHealth / maxHealth;

        boost_timer = 0;

        originalms = movespeed;
    }

    //Called every frame
    private void Update(){
        if (isAttacking)
        {
            return;
        }
        //access our input values
        x_input = Input.GetAxisRaw("Horizontal");
    	y_input = Input.GetAxisRaw("Vertical");

    	Move();

        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0) {
            Attack();
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Interact();
        }
        if (boost_timer < 0) {
            boost_timer = 0;
            movespeed = originalms;
            Debug.Log("Out of boost");

        }
        else if (boost_timer > 0)
        {
            boost_timer -= Time.deltaTime;
        }

    }
    #endregion

    #region movement_functions
    //Moves the player based on WASD inputs and "movespeed"
    private void Move() {
        anim.SetBool("Moving", true);

    	//if player is pressing D
    	if (x_input > 0) {
    		playerRB.velocity = Vector2.right * movespeed;
            currDirection = Vector2.right;
    	} 
    	//if player is pressing A
    	else if (x_input < 0) {
    		playerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        }
    	//if player is pressing W
    	else if (y_input > 0) {
    		playerRB.velocity = Vector2.up * movespeed;
            currDirection = Vector2.up;
        }
    	//if player is pressing S
    	else if (y_input < 0) {
    		playerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        }
        else {
            playerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }

        //set animator directional values
        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);

    }

    public void Speed(float value, float time)
    {
        movespeed = originalms + value;
        boost_timer = time;
        Debug.Log("Speed is now " + movespeed.ToString());
    }
    #endregion

    #region attack_functions
    //attacks in the direction that the player if facing
    private void Attack()
    {
        Debug.Log("Attacking now");
        Debug.Log(currDirection);
        //handles all attack animations and calculates hitboxes
        StartCoroutine(AttackRoutine());
        attackTimer = attackspeed;

    }

    //handle animations and hitboxes for the attack mechanism
    IEnumerator AttackRoutine()
    {
        //Pause movement and freeze player during attack
        isAttacking = true;
        playerRB.velocity = Vector2.zero;

        //start animation
        anim.SetTrigger("Attack");

        //start sound effect
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        //Brief pause before we calculate the hitbox
        yield return new WaitForSeconds(hitboxTiming);

        Debug.Log("Cast hitbox now");

        //Create hitbox
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, Vector2.one, 0f, Vector2.zero, 0);
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Tons of damage");
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(endAnimationTiming);
        isAttacking = false;
    }
    #endregion

    #region health_functions
    //take damage based on value parameter, which is passed in by caller
    public void TakeDamage(float value)
    {
        //call sound effect
        FindObjectOfType<AudioManager>().Play("PlayerHurt");

        //decrement health
        currHealth -= value;
        Debug.Log("Health is now " + currHealth.ToString());

        //Change UI
        hpSlider.value = currHealth / maxHealth;

        //check if dead
        if (currHealth <= 0) {
            //die
            Die();
        }
    }
    //heals player hp based on value parameter, passed in by caller
    public void Heal(float value)
    {
        //increment health
        currHealth += value;
        currHealth += Mathf.Min(currHealth, maxHealth);
        Debug.Log("Health is now " + currHealth.ToString());
        //change ui
        hpSlider.value = currHealth / maxHealth;
    }

    private void Die()
    {
        //call sound effect
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        //destroy gameobject
        Destroy(this.gameObject);
        //trigger anything we need to end the game, find game manager and lose game
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }
    #endregion

    #region interact_functions
    void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Chest"))
            {
                hit.transform.GetComponent<Chest>().Interact();
            }
        }
    }
    #endregion
}
