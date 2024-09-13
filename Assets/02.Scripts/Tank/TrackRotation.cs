using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackRotation : MonoBehaviour
{
    private float _scrollSpeed = 1.0f;
    private MeshRenderer _renderer;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        var offset = Time.time * _scrollSpeed * Input.GetAxisRaw("Vertical");
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
        _renderer.material.SetTextureOffset("_BumpMap",new Vector2(0f, offset));

    }
}
