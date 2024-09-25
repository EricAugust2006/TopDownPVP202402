using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthController : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] TextMeshProUGUI txtHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawHealth();
    }

    public void TakeDamage(int damage) 
    { 
        health -= damage;
        if (health <= 0) 
        { 
            Destroy(gameObject);
        }
    }

    public void DrawHealth() 
    {
        string txt = "";
        for (int i = 0; i < health; i++)
        {
            txt += "♥";
        }
        txtHealth.text = txt;
    }

}
