using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main : MonoBehaviour
{
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask enemyLayer;
    public float Health = 100;
    public float Speed = 5;
    public float JumpForce = 10;
    public float AttackSpeed = 5;
    public float NormalAttackDamage;
    public float HeavyAttackDamage;
    public Combo[] Combos;
    public Animator anim;

    private Rigidbody2D rigid;
    private BoxCollider2D coll;
    private bool isGrounded;
    private string ComboQue = "";
    private float timeBetweenStrikes;
    private float CurrentAnimTime;
    private GameObject PlayerModel;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        PlayerModel = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        isGrounded = Isgrounded();
        if (Health > 0)
        {
            Movement();
            Comabt();
        }
    }

    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
        anim.SetBool("Running", false);
    }

    void Comabt()
    {
        if (timeBetweenStrikes > 0.1f)
        {
            ComboQue = "";
        }
        else
        {
            timeBetweenStrikes += 0.1f * Time.deltaTime;
        }
        if (CurrentAnimTime <= 0)
        {
            anim.SetBool("Punch", false);
            if (Input.GetMouseButtonDown(0))
            {
                //Light Attack
                ComboQue += "0";
                if (ComboQue.Length > 1)
                {
                    bool comboing = false;
                    //Combo Check
                    foreach (Combo com in Combos)
                    {
                        if (ComboQue == com.ComboValue)
                        {
                            //Play combo
                            comboing = true;
                            timeBetweenStrikes = -com.AnimationTime;
                            RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerModel.transform.right, 1, enemyLayer);
                            if (hit.collider != null && hit.collider.GetComponent<Health>() != null)
                            {
                                hit.collider.GetComponent<Health>().Damage(com.DamageAmount, com.Knockback);
                            }
                        }
                    }
                    if (!comboing)
                    {
                        anim.SetBool("Punch", true);
                        CurrentAnimTime++;
                        //Normal Attack
                        timeBetweenStrikes = 0;
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerModel.transform.right, 1, enemyLayer);
                        if (hit.collider != null && hit.collider.GetComponent<Health>() != null)
                        {

                            hit.collider.GetComponent<Health>().Damage(NormalAttackDamage, Vector2.zero);
                        }
                    }
                }
                else
                {
                    CurrentAnimTime++;
                    anim.SetBool("Punch", true);
                    //Normal Attack
                    timeBetweenStrikes = 0;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerModel.transform.right, 1, enemyLayer);
                    if (hit.collider != null && hit.collider.GetComponent<Health>() != null)
                    {
                        hit.collider.GetComponent<Health>().Damage(NormalAttackDamage, Vector2.zero);
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                //Heavy Attack
                ComboQue += "1";
                timeBetweenStrikes = 0;
                if (ComboQue.Length > 1)
                {
                    bool comboing = false;
                    //Combo Check
                    foreach (Combo com in Combos)
                    {
                        if (ComboQue == com.ComboValue)
                        {
                            //Play combo
                            comboing = true;
                            timeBetweenStrikes = -com.AnimationTime;
                            RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerModel.transform.right, 1, enemyLayer);
                            if (hit.collider != null && hit.collider.GetComponent<Health>() != null)
                            {
                                hit.collider.GetComponent<Health>().Damage(com.DamageAmount, com.Knockback);
                            }
                        }
                    }
                    if (!comboing)
                    {
                        anim.SetBool("Punch", true);
                        CurrentAnimTime++;
                        //Heavy attack
                        timeBetweenStrikes = 0;
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerModel.transform.right, 1, enemyLayer);
                        if (hit.collider != null && hit.collider.GetComponent<Health>() != null)
                        {
                            hit.collider.GetComponent<Health>().Damage(HeavyAttackDamage, Vector2.zero);
                        }
                    }
                }
                else
                {
                    anim.SetBool("Punch", true);
                    CurrentAnimTime++;
                    //Heavy attack
                    timeBetweenStrikes = 0;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerModel.transform.right, 1, enemyLayer);
                    if (hit.collider != null && hit.collider.GetComponent<Health>() != null)
                    {
                        hit.collider.GetComponent<Health>().Damage(HeavyAttackDamage, Vector2.zero);
                    }
                }
            }
        }
        else
            CurrentAnimTime -= 2f * Time.deltaTime;
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rigid.velocity = new Vector2(Speed, rigid.velocity.y);
            if (Input.GetKey(KeyCode.A))
                rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rigid.velocity = new Vector2(-Speed, rigid.velocity.y);
            if (Input.GetKey(KeyCode.D))
                rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        else
            rigid.velocity = new Vector2(0, rigid.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded))
        {
            if (!isGrounded)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, JumpForce);
            }
            else
            {
                rigid.velocity = new Vector2(rigid.velocity.x, JumpForce);
            }
        }
        if (Input.GetKey(KeyCode.S) && !isGrounded)
        {
            rigid.AddForce(Vector2.down * JumpForce / 2);
        }
        else if (Input.GetKey(KeyCode.S) && isGrounded)
        {

        }
        else
        {

        }
        if (Mathf.Abs(rigid.velocity.x) > 0)
            anim.SetBool("Running", true);
        else
            anim.SetBool("Running", false);
        
        if (rigid.velocity.x > 0 && PlayerModel.transform.rotation.eulerAngles.y != 0)
        {
            PlayerModel.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rigid.velocity.x < 0 && PlayerModel.transform.rotation.eulerAngles.y != 180)
            PlayerModel.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void Damage(float damageNum, Vector2 knockback)
    {
        Health -= damageNum;
        rigid.AddForce(knockback);
    }

    bool Isgrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        return raycastHit.collider != null;
    }
}


[System.Serializable]
public class Combo
{
    //define all of the values for the class
    public string Name;
    public string ComboValue;
    public int DamageAmount;
    public int AnimationNum;
    public float AnimationTime;
    public Vector2 Knockback;
}