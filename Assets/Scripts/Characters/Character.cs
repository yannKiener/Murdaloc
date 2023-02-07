using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Character 
{
    void kill();
    string GetName();
    void move();

}



public abstract class AbstractCharacter : MonoBehaviour, Character 
{
    protected int maxLife;
    protected int currentLife;
    protected int maxMana;
    protected int currentMana;
    protected string charName;

    public void Initialize(string name)
    {
        maxLife = 100;
        currentLife = maxLife;
        maxMana = 100;
        currentMana = maxMana;
        this.charName = name;
    }

    public void kill()
    {
        GameObject.Destroy(this.gameObject);
    }

    public string GetName()
    {
        return charName;
    }

    public void move()
    {

    }

}



[System.Serializable]
public class Player : AbstractCharacter
{

    private float MAXSPEED = 10f;	
    private float JUMPFORCE = 5f;
    private bool jumping = false;
    private bool wantToJump = false;

    void Update()
    {

        MovePlayer(GetComponent<Rigidbody2D>()); 
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        /* on arrete le saut selon un tag
        if(collision.transform.tag == "Ground"){
            jumping = false;
        }*/
        jumping = false;

    }

    private void MovePlayer(Rigidbody2D player)
    {
        float xSpeed = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown("space"))
        {
            wantToJump = true;
        }
        if (Input.GetKeyUp("space"))
        {
            wantToJump = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        { //Si c'est maintenu. On pourrait changer les sauts aussi pour ca.
            xSpeed = xSpeed * 2;
        }


        float ySpeed = player.velocity.y;

        if (wantToJump && !jumping)
        {
            ySpeed = JUMPFORCE;
            jumping = true;
        }

        player.velocity = new Vector2(xSpeed * MAXSPEED, ySpeed);

        //Limit player to camera At ALL TIMES

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

}



[System.Serializable]
public class Hostile : AbstractCharacter
{
}

[System.Serializable]
public class Friendly : AbstractCharacter
{

}
