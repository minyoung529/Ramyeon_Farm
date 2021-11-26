using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Vector2 offset = Vector2.zero;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SetMainCameraColor();
    }

    private void Update()
    {
        offset.x += Time.deltaTime;
        meshRenderer.material.SetTextureOffset("_MainTex", offset);
    }

    private void SetMainCameraColor()
    {
        Camera.main.backgroundColor = RandomRGB(0, 200);
        meshRenderer.material.color = RandomRGB(100);
    }

    private Color32 RandomRGB(int startColor = 0, int endColor = 255)
    {
        return new Color32((byte)Random.Range(startColor, endColor), (byte)Random.Range(startColor, endColor), (byte)Random.Range(startColor, endColor), 255);
    }
}
