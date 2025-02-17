using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    bool hasControl = true;
    public Animator animator;

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            //reload scene
            SceneManager.LoadSceneAsync("Game");
        }

        if (Input.GetKey(KeyCode.M))
        {
            //spawn dragon
            GetComponent<Inventory>().GiveAllItems();
        }
    }

    void FixedUpdate()
    {
        if(hasControl)
        {
            Vector2 move = transform.position;
            move.x += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime;
            move.y += Input.GetAxisRaw("Vertical") * moveSpeed * Time.fixedDeltaTime;

            if(move.Equals(transform.position)) {
                animator.SetBool("Moving", false);
            } else {
                animator.SetBool("Moving", true);
            }

            transform.position = move;
        }
        //Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
        //controller.Move(move);
    }

    public bool GetPlayerControl() => hasControl;

    public void TogglePlayerControl()
    {
        if (hasControl)
        {
            hasControl = false;
        }
        else
        {
            hasControl = true;
        }
    }

    public void SetPlayerControl(bool controlState)
    {
        hasControl = controlState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Final Item")
        {
            Destroy(collision.gameObject);
            //Play sparkle collecting effect
            //Fade to black
            GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("FinalCinematic");
        }

        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);

            GetComponent<Health>().GiveDamage(-1);
        }
    }
}
