using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolManager<T> where T : MonoBehaviour 
{
    private T prefab { get; }
    public bool autoExpand { get; set; }
    private Transform container { get; }
    private List<T> pool;

    public PoolManager(T prefab, int count )
    {
        this.prefab = prefab;
        this.container = null;
        
        this.CreatePool(count);
    }

    public PoolManager(T prefab, int count, Transform container)
    {
        this.prefab = prefab;
        this.container = container;
        
        this.CreatePool(count);
    }

    private void CreatePool(int count)
    {
        this.pool = new List<T>();

        for (int i = 0; i < count; i++)
            this.CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createObject = Object.Instantiate(this.prefab, this.container);
        createObject.gameObject.SetActive(isActiveByDefault);
        this.pool.Add(createObject);
        return createObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
            
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (this.HasFreeElement(out var element))
            return element;

        if (this.autoExpand)
            return this.CreateObject(true);
        
        throw new Exception($"You haven't free elements in pool of type {typeof(T)}");
    }
}
