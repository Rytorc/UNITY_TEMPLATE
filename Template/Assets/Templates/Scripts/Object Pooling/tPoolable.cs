using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class tPoolable<T> : MonoBehaviour where T : MonoBehaviour
{
    private tObjectPool<T> _objectPool;

    public void AssignObjectPool(tObjectPool<T> objectPool)
    {
        _objectPool = objectPool;
    }
}
