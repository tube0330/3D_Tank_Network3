using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ApacheDamage : MonoBehaviourPun
{
    [SerializeField] MeshRenderer[] mren;
    [SerializeField] GameObject explosionEff;
    readonly string tankTag = "Player";

    void Start()
    {
        mren = GetComponentsInChildren<MeshRenderer>();
        explosionEff = Resources.Load("Explosion") as GameObject;
    }

    [PunRPC]
    void OnDamageRPC(string tag)
    {
        if (tag == tankTag)
        {
            Debug.Log(tankTag);
            var eff = Instantiate(explosionEff, transform.position, Quaternion.identity);
            Destroy(eff, 1f);

            SetApacheVisible(false);
        }
    }

    public void OnDamage(string tag)
    {
        if (photonView.IsMine)
            photonView.RPC("OnDamageRPC", RpcTarget.All, tag);
    }

    void SetApacheVisible(bool isvisible)
    {
        foreach (var mren in mren)
            mren.enabled = isvisible;
    }
}
