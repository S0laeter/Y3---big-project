using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string poolTag;
        public GameObject poolObject;
        public int poolAmount;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public static ObjectPool instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        //instantiate all object pools
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolAmount; i++)
            {
                GameObject obj = Instantiate(pool.poolObject);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.poolTag, objectPool);
        }

    }

    public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("wtf r u trying to spawn??");
            return null;
        }

        //take it out from the queue
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        //adjust position and rotation, and enable it
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        //add the thing into queue again
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    //use GameObject bloodObj = ObjectPool.instance.SpawnObject(whateverthefuck); to spawn stuffs and can edit it after




}