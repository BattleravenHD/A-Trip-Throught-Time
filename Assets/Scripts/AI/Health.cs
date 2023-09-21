using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth = 100;
    public bool canKnockback;
    public bool isKnocked = false;
    public bool HasDeathAnimation;
    public dialogeController dio;
    public int callnum;

    private Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        if (canKnockback)
            rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth <= 0)
            if (!HasDeathAnimation)
                Destroy(gameObject); 
            else
            {
                //GetComponent<Animator>().SetBool("Death", true);
                Destroy(gameObject, 5);
                TryGetComponent<Enemy>(out Enemy en);
                Destroy(en);
                if (dio != null)
                {
                    dio.StartDialoge(callnum);
                }
            }
    }

    public void Damage(float damage, Vector2 knockback)
    {
        CurrentHealth -= damage;
        if (canKnockback)
            rigid.velocity = knockback * -transform.forward;
    }
}
