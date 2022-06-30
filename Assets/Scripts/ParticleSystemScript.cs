using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemScript : MonoBehaviour
{
    ParticleSystem particleLife;

    private void Start()
    {
        particleLife = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(particleLife.startLifetime);
      Destroy(gameObject, 2.5f);
    }
}
