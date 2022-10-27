using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParterBulletObjectPool : MonoBehaviour
{
    public static ParterBulletObjectPool Instance;

    [SerializeField]
    private GameObject PoolingBulletPrefabs;
    [SerializeField]
    private Queue<ParterBullet> PoolingBulletQueue = new Queue<ParterBullet>();

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }

    void Update()
    {
        //Debug.Log(PoolingBulletQueue.Count);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            PoolingBulletQueue.Enqueue(CreateNewBullet());
        }
    }

    private ParterBullet CreateNewBullet()
    {
        var newBullet = Instantiate(PoolingBulletPrefabs).GetComponent<ParterBullet>();
        newBullet.transform.position = transform.position;

        newBullet.gameObject.SetActive(false);
        newBullet.transform.SetParent(transform);
        return newBullet;
    }

    public ParterBullet GetBullet(GameObject Target)
    {
        ParterBullet bullet = null;

        if (Instance.PoolingBulletQueue.Count > 0)
        {
            bullet = Instance.PoolingBulletQueue.Dequeue();
            bullet.transform.SetParent(null);
            bullet.gameObject.SetActive(true);
            
        }

        else
        {
            bullet = Instance.CreateNewBullet();
            bullet.gameObject.SetActive(true);
            bullet.transform.SetParent(null);
        }

        bullet.transform.position = transform.position;
        bullet.TargetSetting(Target);
        return bullet;
    }

    public void ReturnBullet(ParterBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
        bullet.transform.position = this.transform.position;

        Instance.PoolingBulletQueue.Enqueue(bullet);
    }
}
