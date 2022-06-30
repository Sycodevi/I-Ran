using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torch : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void Update()
    {
        Debug.Log("Distane from Torch: " + player.transform.localPosition);
        if(player.transform.localPosition.x >= -13f)
        {
            player.transform.Find("PlayerLight").gameObject.SetActive(false);
        }
        else if(player.transform.localPosition.x <= -13f)
        {
            player.transform.Find("PlayerLight").gameObject.SetActive(true);
        }
    }

}
