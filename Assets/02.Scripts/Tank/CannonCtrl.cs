using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    private Transform tr;
    public float rotSpeed = 5000f;
    public float upperAngle = -30f; //제한 각도
    public float downAngle = 0f; //제한 각도 
    public float currentRotate = 0f; //현재 회전 각도 
    void Start()
    {
        tr = transform;
    }
    void Update()
    {   //포신 제한
        float Wheel = -Input.GetAxisRaw("Mouse ScrollWheel");
        float angle =  Time.deltaTime * rotSpeed * Wheel;
        if(Wheel <= -0.01f) //포신을 올릴때 
        {
            currentRotate += angle;
            if(currentRotate > upperAngle)
            {
                tr.Rotate(angle, 0f, 0f);
            }
            else
            {
                currentRotate = upperAngle;
            }
        }
        else //포신을 내릴때
        {
            currentRotate += angle;
            if (currentRotate < downAngle)
            {
                tr.Rotate(angle, 0f, 0f);
            }
            else
            {
                currentRotate = downAngle;
            }
        }
       
    }
}
