using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform[] path;


    // Start is called before the first frame update
    void Start()
    {
        DoPathMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoPathMove()
    {
        Vector3[] path1 = new Vector3[6];
        path1[0] = path[0].position; ;//起始点
        path1[1] = path[1].position;//中间点
        path1[2] = path[2].position;
        path1[3] = path[3].position;
        path1[4] = path[4].position;
        path1[5] = path[5].position;

        var tweenPath = transform.DOPath(path1, 2f, PathType.CubicBezier).SetLoops(-1);
        tweenPath.onWaypointChange = (p) =>
        {
            //transform.LookAt(path1[p + 1]);
        };
        tweenPath.onComplete = () =>
        {
            
        };
    }
}
