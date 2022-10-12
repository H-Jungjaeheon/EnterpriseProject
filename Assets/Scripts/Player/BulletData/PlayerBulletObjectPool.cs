using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletObjectPool : MonoBehaviour
{
    public static PlayerBulletObjectPool Instance;

    [SerializeField]
    private GameObject PoolingBulletPrefabs;
    [SerializeField]
    private Queue<PlayerBullet> PoolingBulletQueue = new Queue<PlayerBullet>();

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }

    void Update()
    {
        Debug.Log(PoolingBulletQueue.Count);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            PoolingBulletQueue.Enqueue(CreateNewBullet());
        }
    }

    private PlayerBullet CreateNewBullet()
    {
        var newBullet = Instantiate(PoolingBulletPrefabs).GetComponent<PlayerBullet>();
        newBullet.transform.position = transform.position;

        newBullet.gameObject.SetActive(false);
        newBullet.transform.SetParent(transform);
        return newBullet;
    }

    public PlayerBullet GetBullet(GameObject Target)
    {
        PlayerBullet bullet = null;

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

    public void ReturnBullet(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
        bullet.transform.position = this.transform.position;

        Instance.PoolingBulletQueue.Enqueue(bullet);
    }
}
