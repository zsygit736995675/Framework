using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 协程
/// </summary>
public class Coroutiner : SingletonObject<Coroutiner>
{

    static public Coroutine Start(IEnumerator result)
    {
        return  Ins.StartCoroutine(result);
    }

    static public void Stop(IEnumerator result)
    {
         Ins.StopCoroutine(result);
    }

    static public void Stop(Coroutine routine)
    {
        Ins.StopCoroutine(routine);
    }

    static public void StopAll()
    {
        Ins.StopAllCoroutines();
    }


}
