using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TankDamage : MonoBehaviourPun
{
    [SerializeField] private MeshRenderer[] m_Renderer;
    [SerializeField] private GameObject expEffect;

    private int initHp = 100;
    private int curHp = 0;

    private readonly string playerTag = "Player";
    string apacheTag = "APACHE";

    public Canvas hudCanvas;
    public Image HpBar;

    void Start()
    {
        m_Renderer = GetComponentsInChildren<MeshRenderer>();
        expEffect = Resources.Load<GameObject>("Explosion");

        curHp = initHp;
        HpBar.color = Color.green;
    }

    [PunRPC]
    void OnDamageRPC(string tag)
    {
        if (curHp > 0 && tag == playerTag)
        {
            //Debug.Log(playerTag);
            curHp -= 25;
            HpBar.fillAmount = (float)curHp / (float)initHp;
            if (HpBar.fillAmount <= 0.4f)
                HpBar.color = Color.red;
            else if (HpBar.fillAmount <= 0.6f)
                HpBar.color = Color.yellow;

            if (curHp <= 0)
            {
                StartCoroutine(ExplosionTank());
            }
        }

        else if (curHp > 0 && tag == apacheTag)
        {
            Debug.Log(curHp);
            curHp -= 1;
            HpBar.fillAmount = (float)curHp / (float)initHp;
            if (HpBar.fillAmount <= 0.4f)
                HpBar.color = Color.red;
            else if (HpBar.fillAmount <= 0.6f)
                HpBar.color = Color.yellow;

            if (curHp <= 0)
                StartCoroutine(ExplosionTank());
        }
    }

    public void OnDamage(string tag)
    {
        if (photonView.IsMine)
            photonView.RPC("OnDamageRPC", RpcTarget.All, tag);
    }

    IEnumerator ExplosionTank()
    {
        var effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2.0f);
        SetTankvisible(false);
        hudCanvas.enabled = false;

        yield return new WaitForSeconds(5.0f);

        curHp = initHp;
        SetTankvisible(true);
        HpBar.fillAmount = 1.0f;
        HpBar.color = Color.green;
        hudCanvas.enabled = true;
    }

    void SetTankvisible(bool isvisible)
    {
        foreach (var tank in m_Renderer)
            tank.enabled = isvisible;
    }
}