using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class HealthController : NetworkBehaviour
{
    public NetworkVariable<int> health;
    public NetworkVariable<bool> isMorto;
    
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
        TakeDamageServerRpc(damage);
    }

    [ServerRpc]
    void TakeDamageServerRpc(int damage) 
    {
        if (isMorto.Value == false) 
        { 
            health.Value -= damage;
            if (health.Value <= 0)
            {
                isMorto.Value = true;
            }
        }
    }

    public void DrawHealth() 
    {
        string txt = "";
        for (int i = 0; i < health.Value; i++)
        {
            txt += "♥";
        }
        txtHealth.text = txt;
    }

}
