using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class tObjectSpawner<T> : MonoBehaviour where T : tPoolable<T>
{
    [SerializeField] private GameObject _objPrefab;

    private tObjectPool<T> _objectPool;

    void Start()
    {
        _objectPool = new tObjectPool<T>(FactoryMethod, TurnOn, TurnOff, 5, true);
    }
    private T FactoryMethod()
    {
        GameObject obj = Instantiate(_objPrefab) as GameObject;
        T objScript = obj.GetComponent<T>();
        // attach a reference of the objectPool to the pool object
        objScript.AssignObjectPool(_objectPool);

        obj.gameObject.SetActive(false);

        return obj.GetComponent<T>();
    }

    private void TurnOn(T obj)
    {
        obj.gameObject.SetActive(true);
    }
    private void TurnOff(T obj)
    {
        obj.gameObject.SetActive(false);
    }

}

