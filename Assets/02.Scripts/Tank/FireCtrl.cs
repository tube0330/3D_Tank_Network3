using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FireCtrl : MonoBehaviourPun
{
    public GameObject bullet =null;
    public Transform firePos = null;
    public LeaserBeamT BeamT = null;
    public GameObject expEffect;
    private readonly string playerTag = "Player";
    readonly string apacheTag = "APACHE";

    void Start()
    {
      bullet =  Resources.Load<GameObject>("Bullet");
      firePos = transform.GetChild(4).GetChild(1).GetChild(0).GetChild(0).transform;
       BeamT = GetComponentInChildren<LeaserBeamT>();
       expEffect = Resources.Load<GameObject>("Explosion");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && photonView.IsMine)
        {
            Fire();
            photonView.RPC("Fire", RpcTarget.Others);
        }
    }
    
    [PunRPC]
    void Fire()
    {
        //Instantiate(bullet,firePos.position,firePos.rotation);
        RaycastHit hit;
        Ray ray = new Ray(firePos.position, firePos.forward);
        if(Physics.Raycast(ray,out hit,100f,1<<8|1<<10 ))
        {
            BeamT.FireRay();
            ShowEffect(hit);

            if(hit.collider.CompareTag(playerTag))
            {
                string tag = hit.collider.tag;
                hit.collider.transform.parent.SendMessage("OnDamage", tag, SendMessageOptions.DontRequireReceiver);
            }

            else if(hit.collider.CompareTag(apacheTag))
            {
                hit.collider.transform.parent.SendMessage("OnDamage", playerTag, SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            BeamT.FireRay();
            Vector3 hitpos = ray.GetPoint(200f);
            Vector3 _normal = firePos.position - hitpos.normalized;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            GameObject eff = Instantiate(expEffect, hitpos, rot);
            Destroy(eff, 1.5f);
        }
    }
    void ShowEffect(RaycastHit hitTank)
    {
        Vector3 hitpos = hitTank.point;
        Vector3 _normal = firePos.position - hitpos.normalized;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject eff = Instantiate(expEffect,hitpos,rot);
        Destroy(eff,1.5f );
    }
}
