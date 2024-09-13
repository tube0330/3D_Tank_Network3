using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ApacheCtrl : MonoBehaviour
{
    public float moveSpeed = 0f;
    public float rotSpeed = 0f;
    [SerializeField] Transform tr;
    private float verticalSpeed = 0f;
    void Start()
    {
        tr = transform;
    }
    void FixedUpdate()
    {
        #region ����ġ A,D �¿�ȸ��
        if (Input.GetKey(KeyCode.A))
            rotSpeed += -0.02f;
        else if (Input.GetKey(KeyCode.D))
            rotSpeed += 0.02f;
        else
        {
            if (rotSpeed > 0f) rotSpeed += -0.02f;
            else if (rotSpeed < 0f) rotSpeed += 0.02f;
        }
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        #endregion
        #region ����ġ �յ� �̵� 
        if (Input.GetKey(KeyCode.W))
            moveSpeed += 0.02f;
        else if (Input.GetKey(KeyCode.S))
            moveSpeed += -0.02f;
        else
        {
            if (moveSpeed > 0f) moveSpeed += -0.02f;
            else if (moveSpeed < 0f) moveSpeed += 0.02f;
        }

        tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);

        #endregion
        #region ����ġ ���Ʒ� �̵� 
        if (Input.GetKey(KeyCode.C))
            verticalSpeed += 0.04f;
        else if (Input.GetKey(KeyCode.Z))
            verticalSpeed += -0.04f;
        else
        {
            if (verticalSpeed > 0f) verticalSpeed += -0.04f;
            else if (verticalSpeed < 0f) verticalSpeed += 0.04f;
        }
        tr.Translate(Vector3.up * verticalSpeed * Time.deltaTime, Space.Self);
        #endregion

    }
}
