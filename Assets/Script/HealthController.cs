using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class HealthController : NetworkBehaviour
{
    public NetworkVariable<int> health = new NetworkVariable<int>(5);
    public NetworkVariable<bool> isMorto = new NetworkVariable<bool>(false);
    
    [SerializeField] TextMeshProUGUI txtHealth;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClient)
        {
            DrawHealth(); // Apenas o cliente desenha a saúde
        }
    }

    public void TakeDamage(int damage) 
    {
        TakeDamageServerRpc(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    void TakeDamageServerRpc(int damage) 
    {
        if (!IsServer) return;

        if (!isMorto.Value) 
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
