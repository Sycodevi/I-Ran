using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public float typingspeed;
    public TextMeshProUGUI textdisplay;
    public string[] sentences;
    private int index;

    public GameObject continuebutton;


    private void Start()
    {
        StartCoroutine(Type());
    }
    private void Update()
    {
        if(textdisplay.text == sentences[index])
        {
            continuebutton.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textdisplay.text += letter;
            yield return new WaitForSeconds(typingspeed);
        }
    }
    public void nextsentence()
    {
        if(index < sentences.Length - 1)
        {
            index++;
            textdisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textdisplay.text = "";
        }
    }
}
