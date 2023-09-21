using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogeController : MonoBehaviour
{
    public GameObject DialogeBox;
    public Text DialogeText;
    public Dialoge[] Dialoges;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartDialoge(int Call)
    {
        foreach(Dialoge dio in Dialoges)
        {
            if (dio.CallNumber == Call)
            {
                DialogeBox.SetActive(true);
                StartCoroutine(conversation(dio));
            }
        }
    }

    IEnumerator conversation(Dialoge dio)
    {
        foreach(string sen in dio.Text)
        {
            StartCoroutine(sentance(sen));
            yield return new WaitForSeconds(3.5f);
        }
        DialogeBox.SetActive(false);
    }

    IEnumerator sentance(string sen)
    {
        DialogeText.text = "";
        foreach(char letter in sen.ToCharArray())
        {
            DialogeText.text += letter;
            yield return new WaitForSeconds(0.01f);
        } 
    }
}

[System.Serializable]
public class Dialoge
{
    //define all of the values for the class
    public string Name;
    public string[] Text;
    public int CallNumber;
}
