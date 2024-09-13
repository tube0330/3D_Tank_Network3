using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// A: ���� ȸ�� D ���������� ȸ��  W ����  S ���� 
public class TankMove : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;
    private float h = 0f, v = 0f;
    private Transform tr;
    float moveSpeed = 20f;
    public float rotSpeed = 90f;
    void Start()
    {
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        
        tr.Translate(Vector3.forward * v * Time.deltaTime *moveSpeed);
        tr.Rotate(Vector3.up * h * Time.deltaTime *rotSpeed);

    }
}
