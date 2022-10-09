using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("EnemySpawner변수")]
    [SerializeField]
    private GameObject[] PoolingEnemyPrefabs;
    [SerializeField]
    private Queue<Enemy> PoolingEnemyQueue = new Queue<Enemy>();
    [SerializeField]
    private float MaxTime, MinTime;
    [SerializeField]
    private float MaxY, MinY;

    [SerializeField]
    private Coroutine EnemySpawnCorutine;

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }

    private void Start()
    {
        Instance.StartEnemySpawn(5);
    }

    //초기화
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            PoolingEnemyQueue.Enqueue(CreateNewEnemy());
        }
    }

    //초기화된 적 미리 소환
    private Enemy CreateNewEnemy()
    {
        var newEnemy = Instantiate(PoolingEnemyPrefabs[0]).GetComponent<Enemy>();
        newEnemy.gameObject.SetActive(false);
        newEnemy.transform.SetParent(transform);
        return newEnemy;
    }

    //적 활성화
    private Enemy GetEnemy()
    {
        Enemy enemy = null;

        //pool에 적이 있을 시 소환
        if(Instance.PoolingEnemyQueue.Count > 0)
        {
            enemy = Instance.PoolingEnemyQueue.Dequeue();
            enemy.transform.SetParent(null);
            enemy.gameObject.SetActive(true);
        }

        //없을 시 새로 생성
        else
        {
            enemy = Instance.CreateNewEnemy();
            enemy.gameObject.SetActive(true);
            enemy.transform.SetParent(null);
        }



        return enemy;
    }

    //적 오브젝트 풀로 리턴
    public static void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(Instance.transform);
        Instance.PoolingEnemyQueue.Enqueue(enemy);
    }

    public void StartEnemySpawn(int SpawnCnt)
    {
        EnemySpawnCorutine = StartCoroutine(EnemySpawn(SpawnCnt));
    }

    public void StopEnemySpawn()
    {
        StopCoroutine(EnemySpawnCorutine);
    }

    private IEnumerator EnemySpawn(int SpawnCnt)
    {
        yield return null;

        while(SpawnCnt > 0)
        {
            Enemy enemy = Instance.GetEnemy();
            enemy.transform.position = new Vector2(this.transform.position.x, Random.Range(MinY, MaxY));

            SpawnCnt--;
            yield return new WaitForSeconds(Random.Range(MinTime, MaxTime));
        }
    }
}
