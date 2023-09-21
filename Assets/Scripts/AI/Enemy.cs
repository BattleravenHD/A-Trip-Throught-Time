using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public Vector2 visionRadius;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask playerLayer;
    public Animator anim;

    public bool Alerted;
    public bool Dazed = false;

    public Vector3[] patrolPathLocations;

    private Vector2 lastKnownPlayerLocation;
    private GameObject player;
    private int currentLocation = 0;
    private GameObject Model;
    private Rigidbody2D rigid;
    private bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Model = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vision();
        if (!Alerted)
        {
            anim.SetBool("isRunning", true);
            MovementPathing();
        }
        else
            Combat();  
    }

    void Combat()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > 1 && !attacking)
        {
            anim.SetBool("isRunning", true);
            Vector3 dir = (transform.position - player.transform.position).normalized;
            rigid.velocity = new Vector3(-dir.x * Speed, rigid.velocity.y);
            if (dir.x > 0)
                Model.transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                Model.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (!attacking)
        {
            rigid.velocity = Vector3.zero;
            attacking = true;
            anim.SetBool("isRunning", false);
            //Attack
        }
        else
        {
            Debug.Log("swing");
            StartCoroutine(wait());
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    void Vision()
    {
        if (Alerted)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, dir, visionRadius.y, playerLayer);
            if (raycast.collider != null)
            {
                lastKnownPlayerLocation = raycast.collider.transform.position;
            }
            else
            {

            }
        }
        else
        {
            if (transform.position.x + Model.transform.right.x < transform.position.x)
            {
                if (player.transform.position.x > transform.position.x)
                {
                    if ((Vector2.Angle(transform.position - player.transform.position, player.transform.position) - 180) * -1 < visionRadius.x)
                    {
                        Vector2 dir = (player.transform.position - transform.position).normalized;
                        RaycastHit2D raycast = Physics2D.Raycast(transform.position, dir, visionRadius.y, playerLayer);
                        if (raycast.collider != null)
                            if (raycast.collider.tag == "Player")
                                Alerted = true;
                    }
                }
            }
            else
            {
                if (player.transform.position.x < transform.position.x)
                {
                    if (Vector2.Angle(transform.position - player.transform.position, player.transform.position) < visionRadius.x)
                    {
                        Vector2 dir = (player.transform.position - transform.position).normalized;
                        RaycastHit2D raycast = Physics2D.Raycast(transform.position, dir, visionRadius.y, playerLayer);
                        if (raycast.collider != null)
                            if (raycast.collider.tag == "Player")
                                Alerted = true;
                    }
                }
            }
        }
    }

    void MovementPathing()
    {
        if (patrolPathLocations.Length != 0)
        {
            if (Vector3.Distance(transform.position, patrolPathLocations[currentLocation]) > 1)
            {
                Vector3 dir = (transform.position - patrolPathLocations[currentLocation]).normalized;
                if (dir.x > 0)
                    Model.transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    Model.transform.rotation = Quaternion.Euler(0, 180, 0);
                rigid.velocity = new Vector3(-dir.x * Speed, rigid.velocity.y);
                if (patrolPathLocations[currentLocation].y - transform.position.y > 1 && patrolPathLocations[currentLocation].x - transform.position.x < 1)
                {
                    if (Isgrounded())
                    {
                        rigid.velocity = rigid.velocity = new Vector3(rigid.velocity.x, -dir.y * Speed * 1.5f);
                    }
                }
            }
            else
            {
                currentLocation = (currentLocation + 1) % patrolPathLocations.Length;
            }
        }
    }

    bool Isgrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        return raycastHit.collider != null;
    }

    /*private IEnumerator fadeIn()
    {
        HealthBar.parent.gameObject.SetActive(true);
        SpriteRenderer red = HealthBar.parent.GetComponent<SpriteRenderer>();
        SpriteRenderer green = HealthBar.GetChild(0).GetComponent<SpriteRenderer>();

        green.color = new Color(green.color.r, green.color.g, green.color.b, 1 / 3f);
        red.color = new Color(red.color.r, red.color.g, red.color.b, 1 / 3f);
        yield return new WaitForSeconds(0.1f);
        green.color = new Color(green.color.r, green.color.g, green.color.b, 2 / 3f);
        red.color = new Color(red.color.r, red.color.g, red.color.b, 2 / 3f);
        yield return new WaitForSeconds(0.1f);
        green.color = new Color(green.color.r, green.color.g, green.color.b, 1f);
        red.color = new Color(red.color.r, red.color.g, red.color.b, 1f);
    }*/
}
