using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 提前定义一些泛型相关的方法, 否则热更可能找不到对应的类型
/// </summary>

public class AOTGenericReferences : MonoBehaviour
{
    public void RefMethods()
    {
        object _;
        _ = new List<object>();
        _ = new List<int>();
        _ = new List<float>();

        _ = new Dictionary<int, int>();
        _ = new Dictionary<int, float>();
        _ = new Dictionary<int, object>();
        
        _ = default(ValueTuple<int, object>);
        _ = default(ValueTuple<int, int>);
        _ = default(ValueTuple<float, object>);
        _ = default(ValueTuple<float, float>);
        _ = default(ValueTuple<object, object>);

        UnityAction<int> _UAInt = pArg0 => { };
        UnityAction<Byte> _UAB = pArg0 => { };
        UnityAction<float> _UAfloat = pArg0 => { };
        UnityAction<object> _UAObject = pArg0 => { };
        
        UnityAction<int, object> _UAIObject = (pArg0, p2) => { };
        UnityAction<int, int> _UAIIt = (pArg0, p2) => { };
        
        var builder = new AsyncVoidMethodBuilder();
        IAsyncStateMachine asm = default;
        builder.Start(ref asm);
    }
}
