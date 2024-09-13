using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ray를 쏘아서 마우스 포지션 따라서 
public class TurretCtrl : MonoBehaviour
{
    private Transform tr;
    private float rotSpeed = 5f;
    RaycastHit hit;
    void Start()
    {
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray  = Camera.main.ScreenPointToRay(Input.mousePosition);
                   //카메라에서 마우스 포지션 방향으로 광선을 발사 
        Debug.DrawRay(ray.origin, ray.direction *100f,Color.green);
       
        if(Physics.Raycast(ray, out hit,100f,1<<8))
        {  //레이가 터레인에 맞았다면 

            Vector3 relative = tr.InverseTransformPoint(hit.point);
            //맞았던 월드 위치를 탱크에 맞는 로컬좌표로 바꿈
                                                              // Mathf.Deg2Rad;일반각도를 라디언 각도로 바꿈 
            float angle = Mathf.Atan2(relative.x,relative.z) * Mathf.Rad2Deg; //라디언각도를 일반각도로 바꿈
                          //역탄젠트 함수인 Atan2로  두점간 각도를 계산
            tr.Rotate(0f, angle * Time.deltaTime *rotSpeed, 0f);
        }
    }
}
