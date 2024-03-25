using System;
using UnityEngine;

public class WorldSpaceOffset : MonoBehaviour
{
    private Renderer _renderer;

    private static readonly int WorldPos = Shader.PropertyToID("WorldPos");

    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        _renderer.GetPropertyBlock(_propertyBlock);
        Vector3 lastWorldPos = _propertyBlock.GetVector(WorldPos);

        if (lastWorldPos != transform.position)
        {
            _propertyBlock.SetVector(WorldPos, transform.position);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
