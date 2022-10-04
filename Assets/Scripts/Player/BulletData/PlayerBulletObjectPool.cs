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
    }

    //초기화
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            PoolingBulletQueue.Enqueue(CreateNewEnemy());
        }
    }

    //초기화된 적 미리 소환
    private PlayerBullet CreateNewEnemy()
    {
        var newBullet = Instantiate(PoolingBulletPrefabs).GetComponent<PlayerBullet>();
        newBullet.gameObject.SetActive(false);
        newBullet.transform.SetParent(transform);
        return newBullet;
    }

    //적 활성화
    private PlayerBullet GetEnemy()
    {
        if (Instance.PoolingBulletQueue.Count > 0)
        {
            var bullet = Instance.PoolingBulletQueue.Dequeue();
            bullet.transform.SetParent(null);
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        else
        {
            var newBullet = Instance.CreateNewEnemy();
            newBullet.gameObject.SetActive(true);
            newBullet.transform.SetParent(null);
            return newBullet;
        }
    }

    //적 오브젝트 풀로 리턴
    public static void ReturnEnemy(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
        Instance.PoolingBulletQueue.Enqueue(bullet);
    }
}
