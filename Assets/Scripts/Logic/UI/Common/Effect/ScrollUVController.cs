using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YooAsset;

public class ScrollUVController : MonoBehaviour
{
    /// <summary>
    /// 当前速度
    /// </summary>
    private float curSpeed;

    /// <summary>
    /// 目标速度
    /// </summary>
    public float targetSpeed;


    Material _material;

    float _uvX = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        _uvX += Time.deltaTime * curSpeed;
        _material.SetFloat("_UVX", _uvX);
    }

    // Update is called once per frame

    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        this.curSpeed = speed;
    }

    public float GetSpeed()
    {
        return curSpeed;
    }

    /// <summary>
    /// 重置UV
    /// </summary>
    public void ResetUVX()
    {
        _uvX = 0f;
    }

    /// <summary>
    /// 设置材质
    /// </summary>
    /// <param name="tex"></param>
    public void SetTexture(Texture2D tex)
    {
        _material.SetTexture("_MainTexture", tex);
    }
}
