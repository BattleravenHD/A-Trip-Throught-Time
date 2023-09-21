using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public dialogeController dialoge;
    public float waitTime = 1f;
    public int callNum;

    public Vector3 TPSpot;
    public GameObject flasharea;
    public Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (TPSpot == Vector3.zero)
            {
                if (dialoge != null)
                {
                    collision.GetComponent<Player_Main>().enabled = false;
                    StartCoroutine(disableUntill(collision.GetComponent<Player_Main>()));
                    GetComponent<BoxCollider2D>().enabled = false;
                    dialoge.StartDialoge(callNum);
                }
            } else
            {
                collision.transform.position = TPSpot;
                if (flasharea != null)
                    flasharea.SetActive(true);
            }
        }
    }

    IEnumerator disableUntill(Player_Main player)
    {
        yield return new WaitForSeconds(waitTime);
        player.enabled = true;
        if (enemyScript != null)
        {
            enemyScript.enabled = true;
        }
    }
}
