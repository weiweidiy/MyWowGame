using Chronos;
using DG.Tweening;
using Framework.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotweenManager : MonoSingleton<DotweenManager>
{
    Timeline m_TimeLine;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        m_TimeLine = GetComponent<Timeline>();
    }


    private void Update()
    {
        Debug.Assert(m_TimeLine != null, "û���ҵ�timeline " + gameObject);
        float deltaTime = m_TimeLine ? m_TimeLine.deltaTime : Time.deltaTime;
        deltaTime = deltaTime * Time.timeScale;
        DOTween.ManualUpdate(deltaTime, m_TimeLine.unscaledTime);
    }

    public void DOTweenDelay(float delayedTimer, int loopTimes, Action action)
    {
        float timer = 0;
        //DOTwwen.To()�в�����ǰ���������ǹ̶�д�����������ǵ��������ֵ�����ĸ��ǽ���������õ�ʱ��
        Tween t = DOTween.To(() => timer, x => timer = x, 1, delayedTimer).SetUpdate(UpdateType.Manual)
                      .OnStepComplete(() =>
                      {
                          action?.Invoke();
                      })
                      .SetLoops(loopTimes);
    }

    public void DOTweenDelayUnityTime(float delayedTimer, int loopTimes, Action action)
    {
        float timer = 0;
        //DOTwwen.To()�в�����ǰ���������ǹ̶�д�����������ǵ��������ֵ�����ĸ��ǽ���������õ�ʱ��
        Tween t = DOTween.To(() => timer, x => timer = x, 1, delayedTimer)
                      .OnStepComplete(() =>
                      {
                          action?.Invoke();
                      })
                      .SetLoops(loopTimes);
    }

}
