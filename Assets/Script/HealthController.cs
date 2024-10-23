using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class HealthController : NetworkBehaviour
{
    public NetworkVariable<int> health = new NetworkVariable<int>(5);
    public NetworkVariable<bool> isMorto = new NetworkVariable<bool>(false);

    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI txtHealth;

    float respawnTime = 5;

    float respawnCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClient) 
        {
            DrawHealth();
        }

        if (!IsOwner) return;

        if (isMorto.Value)
        {
            RespawnCounter();
            return;
        }
        else
        {
            animator.SetBool("Death", false);
        }
    }

    public void RecoverHealth(int qtd) 
    { 
        RecoverHealthServerRpc(qtd);
    }

    public void TakeDamage(int damage) 
    {
        TakeDamageServerRpc(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    void RecoverHealthServerRpc(int qtd) 
    {
        if (!IsServer) return;

        isMorto.Value = false;
        health.Value = qtd;

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

    void RespawnCounter() 
    {
        animator.SetBool("Death", true);

        respawnCounter += Time.deltaTime;

        txtHealth.text = respawnCounter.ToString("0.00");

        if (respawnCounter > respawnTime)
        {
            respawnCounter = 0;
            RecoverHealth(5);
        }
    }

    public void DrawHealth() 
    {
        string txt = "";
        if (isMorto.Value && IsOwner) 
        {
            txt = respawnCounter.ToString("0.00");
        }
        else 
        { 
            for (int i = 0; i < health.Value; i++)
            {
                txt += "♥";
            }
        }
        txtHealth.text = txt;
    }

}
