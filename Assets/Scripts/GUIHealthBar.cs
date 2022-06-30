using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIHealthBar : MonoBehaviour
{
    public Slider slider;
    public Image image;
    public TextMeshProUGUI TMP;
    public Color low;
    public Color high;
    public Vector3 offset;

    float perc;

    private void Start()
    {
        slider.maxValue = GameObject.Find("Player").GetComponent<Player>().maxHealth;
        slider.value = slider.maxValue;
        slider.fillRect.gameObject.GetComponent<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
        TMP.color = Color.Lerp(low, high, slider.normalizedValue);
        image.color = Color.Lerp(low, high, slider.normalizedValue);
    }
    private void healthtext(float health, float maxhealth)
    {
        perc = ((health / maxhealth) * 100); Debug.Log(perc);
        if(perc <= 0)
        {
            perc = 0;
        }
        TMP.text = perc + "%";
    }
    public void sethealth(float health, float maxhealth)
    {
        healthtext(health, maxhealth);
        slider.value = health;
        slider.maxValue = maxhealth;
        slider.fillRect.gameObject.GetComponent<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
        TMP.color = Color.Lerp(low, high, slider.normalizedValue);
        image.color = Color.Lerp(low, high, slider.normalizedValue);
    }
}
