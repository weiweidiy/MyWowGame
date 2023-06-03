using Chronos;
using Logic.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timeline))]
public class InitTimeLine : MonoBehaviour
{
    [SerializeField] GameTimeScaleManager.TimeScaleModule module;

    ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Awake()
    {
        var timeLine = GetComponent<Timeline>();

        if (timeLine != null)
        {
            timeLine.mode = TimelineMode.Global;
            timeLine.globalClockKey = module.ToString();
            timeLine.rewindable = false;
            
        }

        particleSystem = GetComponent<ParticleSystem>();
        //particleSystem?.Play();
    }

    private void Start()
    {
        if (particleSystem != null)
            particleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
