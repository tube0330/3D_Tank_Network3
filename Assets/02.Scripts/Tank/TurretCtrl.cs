using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);

        if (Physics.Raycast(ray, out hit, 100f, 1 << 8))
        {
            Vector3 relative = tr.InverseTransformPoint(hit.point);
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f);
        }
    }
}
