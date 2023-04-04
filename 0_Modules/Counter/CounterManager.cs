using System;
using System.Collections.Generic;

/// <summary>
/// 计数器管理器
/// </summary>
public class CounterManager : BaseClass<CounterManager>
{
    MemoryPool<Counter> counterPool = new MemoryPool<Counter>();

    List<Counter> counters = new List<Counter>();

    public Counter GetCounter(CounterDef def,Action ZeroCallback, ReleaseCallback releaseCallback=null) 
    {
        Counter counter = GetCounter(def);
        counter.Def = def;
        counter.ZeroCallback = ZeroCallback;
        counter.releaseCallback = releaseCallback;
        counter.ReSet();
        return counter;
    }

    Counter GetCounter(CounterDef def) 
    {
        Counter counter = null;

        foreach (var item in counters)
        {
            if (item.Def == def)
                return item;
        }

        counter = CreaterCounter();
        counters.Add(counter);
        return counter;
    }

    public void KillCounter(CounterDef def) 
    {
        Counter counter = GetCounter(def);
        counters.Remove(counter);
        RecycleCounter(counter);
    }

    Counter CreaterCounter() 
    {
        Counter counter = counterPool.Alloc();
        if (counter != null)
        {
            return counter;
        }
        return new Counter();
    }

    void RecycleCounter(Counter counter) 
    {
        counterPool.Free(counter);
    }

}
