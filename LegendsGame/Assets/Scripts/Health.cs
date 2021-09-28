using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    bool isInvulnerable = false;
    float invulnerableTimer, maxInvulnerableTimer = 2;

    public int health;
    public int numberOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [SerializeField] private GameObject gameOverScreen;
    private bool isGameOver = false;
    
    void Start()
    {
        invulnerableTimer = maxInvulnerableTimer;
//        gameOverScreen.SetActive(false);
    }

    //TODO (Aiden) potentially refactor slightly... probably not great to do this for loop every frame
    // Move to take damage method?
    void Update()
    {
        if(isInvulnerable)
        {
            invulnerableTimer -= Time.deltaTime;
        }
        if(invulnerableTimer < 0)
        {
            isInvulnerable = false;
        }

        if(health > numberOfHearts) {
            health = numberOfHearts;
        }

        //Test if lose heart when damaged
        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    TakeDamage(1);
        //}

        for (int i = 0; i < hearts.Length; i++) {

            if(i < health) {
                hearts[i].sprite = fullHeart;
            }
            else {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numberOfHearts) {
                hearts[i].enabled = true;
            }
            else {
                hearts[i].enabled = false;
            }
        }

        //Eventually merge the below 3 if statements
        if(health <= 0 && gameObject.tag == "Dragon")
        {
            playDeathEffect();
        }

        //Check if health is 0
        if (health <= 0 && gameObject.tag == "Player") {
            isGameOver = true;
        }

        if (isGameOver) {
            gameOverScreen.SetActive(true);
        }
    }

    //Take damage
    //Include SFX and visual effect to know the player is damaged and invulnerable
    //Take knockback????
    void TakeDamage(int damage) {
        if(!isInvulnerable)
        {
            health -= damage;
            if (health < 1)
            {
                //Debug.Log("Game Over");
            }
            isInvulnerable = true;
            invulnerableTimer = maxInvulnerableTimer;
        }
    }

    //Method exists for encapsulation reasons
    public void GiveDamage(int damage)
    {
        TakeDamage(damage);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {   
        if (gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "Dragon")
            {
                Dragon dragon = collision.transform.root.GetComponentInChildren<Dragon>();
                TakeDamage(dragon.touchDamage);
            }
        }
        //else if(gameObject.tag == "Bullet")
        //{
        //    if(collision.gameObject.tag == "Dragon")
        //    {
        //        Health health = collision.transform.root.GetComponentInChildren<Health>();
        //        health.GiveDamage(1);
        //        Destroy(gameObject);//Destrosy the bullet
        //    }
        //}
    }

    void playDeathEffect()
    {
        Debug.Log($"Playing death effect for {gameObject.tag}");
        switch(gameObject.tag)
        {
            case "Player":
                break;
            case "Dragon":
                //create dragon death effect
                GetComponent<Dragon>().dragonController.HandleDragonDeath();
                Destroy(transform.root.gameObject);
                break;
        }
    }
}