using DG.Tweening;
using Logic.Fight.Skill.State;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Common
{

    public class FlashColorEffect : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] spRenderers;

        [SerializeField] IEventNotifier eventNotifier;

        /// <summary>
        /// 闪色因子
        /// </summary>
        [SerializeField] float factor;

        /// <summary>
        /// 闪色周期
        /// </summary>
        [SerializeField] float duration;

        private void Awake()
        {
            //Debug.Assert(material != null, " material == null");

            //Debug.Assert(eventNotifier != null, " eventNotifier == null");
            if (spRenderers == null)
                spRenderers = GetComponentsInChildren<SpriteRenderer>();

            Debug.Assert(spRenderers != null, " spRenderers == null");

            if (eventNotifier == null)
                eventNotifier = GetComponent<IEventNotifier>();
            
            Debug.Assert(eventNotifier != null, " eventNotifier == null");

            eventNotifier.onEventRaise += EventNotifier_onEventRaise;
        }

        private void EventNotifier_onEventRaise()
        {
            foreach(var sp in spRenderers)
            {
                var mat = sp.material;
                mat.SetFloat("_Factor", factor);

                DOTween.To(() => mat.GetFloat("_Factor"), r => mat.SetFloat("_Factor", r), 0, duration);
            }    
        }



        private void OnDestroy()
        {
            eventNotifier.onEventRaise -= EventNotifier_onEventRaise;
        }
    }

}
