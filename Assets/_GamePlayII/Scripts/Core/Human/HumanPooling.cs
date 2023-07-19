using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HumanPooling : MonoBehaviour
{
    private static HumanPooling instance;
    public static HumanPooling Instance { get { return instance; } }

    [Header("Pool Manager")]
    public List<ObjectPool> ObjectPools = new List<ObjectPool>();

    [System.Serializable]
    public class ObjectPool
    {
        [HideInInspector] public Transform parent;
        public Human objectPrefab;
        public eNameJob nameObject;

        [HideInInspector]
        public List<Human> listObject = new List<Human>();
    }

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
    }

    private void Start()
    {
        GenerateObjectPool();
    }

    public void GenerateObjectPool()
    {
        int count = ObjectPools.Count;

        for (int i = 0; i < count; i++)
        {
            int amount = 10;
            Human prefab = ObjectPools[i].objectPrefab;
            GameObject newParent = new GameObject();
            newParent.gameObject.name = ObjectPools[i].nameObject.ToString();
            newParent.transform.SetParent(this.transform);
            ObjectPools[i].parent = newParent.transform;
            Transform parent = ObjectPools[i].parent;

            for (int j = 0; j < amount; j++)
            {
                Human human = Instantiate(prefab, parent) as Human;
                human.gameObject.SetActive(false);
                ObjectPools[i].listObject.Add(human);
            }
        }
    }

    public Human GetObject(eNameJob name)
    {
        int count = ObjectPools.Count;
        ObjectPool objectPool = null;

        for (int i = 0; i < count; i++)
        {
            if (ObjectPools[i].nameObject == name)
            {
                objectPool = ObjectPools[i];
            }
        }

        if (objectPool == null) return null;

        int childCount = objectPool.listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Human childObject = objectPool.listObject[i];
            GameObject _go = childObject.gameObject;
            if (_go.activeInHierarchy == false)
            {
                return childObject;
            }
        }
        Human human = Instantiate(objectPool.objectPrefab, objectPool.parent) as Human;
        human.gameObject.SetActive(false);
        objectPool.listObject.Add(human);
        return human;
    }

    public void RefreshItem(eNameJob name)
    {
        for (int i = 0; i < ObjectPools.Count; i++)
        {
            if (ObjectPools[i].nameObject == name)
            {
                for (int k = 0; k < ObjectPools[i].listObject.Count; k++)
                {
                    Human _go = ObjectPools[i].listObject[k];
                    _go.transform.SetParent(ObjectPools[i].parent);
                    _go.gameObject.SetActive(false);
                }
            }
        }
    }

    public void RefreshAll()
    {
        int count = 1 + eNameJob.GetValues(typeof(eNameJob)).Cast<int>().Max();
        for (int i = 0; i < count; i++)
        {
            eNameJob _nameObject = (eNameJob)i;
            RefreshItem(_nameObject);
        }
    }

}