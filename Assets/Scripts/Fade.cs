using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public bool IsFaded;
    public float fadeSpeed = 0.01f;
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFaded == false && rend.color.a < 1)
        {
            Color temp = rend.color;
            temp.a = rend.color.a + fadeSpeed;
            rend.color = temp;
        }else if (IsFaded == true && rend.color.a > 0)
        {
            Color temp = rend.color;
            temp.a = rend.color.a - fadeSpeed;
            rend.color = temp;
        }
    }

    public void fade()
    {
        IsFaded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            IsFaded = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            IsFaded = true;
    }
}
