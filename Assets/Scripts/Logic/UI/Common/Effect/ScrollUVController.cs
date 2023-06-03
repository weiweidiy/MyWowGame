using Chronos;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YooAsset;

public class ScrollUVController : MonoBehaviour
{
    /// <summary>
    /// ��ǰ�ٶ�
    /// </summary>
    private float curSpeed;

    /// <summary>
    /// Ŀ���ٶ�
    /// </summary>
    public float targetSpeed;


    Material _material;

    float _uvX = 0f;

    // Start is called before the first frame update
    Timeline m_TimeLine;

    void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
        m_TimeLine = GetComponent<Timeline>();
    }

    private void Update()
    {
        Debug.Assert(m_TimeLine != null, "û���ҵ�timeline" +gameObject);
        float deltaTime = m_TimeLine ? m_TimeLine.deltaTime : Time.deltaTime;
        deltaTime *= Time.timeScale;
        _uvX += deltaTime * curSpeed;
        _material.SetFloat("_UVX", _uvX);
    }

    // Update is called once per frame

    /// <summary>
    /// �����ƶ��ٶ�
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
    /// ����UV
    /// </summary>
    public void ResetUVX()
    {
        _uvX = 0f;
    }

    /// <summary>
    /// ���ò���
    /// </summary>
    /// <param name="tex"></param>
    public void SetTexture(Texture2D tex)
    {
        _material.SetTexture("_MainTexture", tex);
    }
}
