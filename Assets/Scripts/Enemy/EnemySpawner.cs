using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("EnemySpawnerº¯¼ö")]
    [SerializeField]
    private GameObject[] EnemyPrefabs;

    [SerializeField]
    private float MaxY, MinY;

    [SerializeField]
    private Coroutine EnemySpawnCorutine;

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


            SpawnCnt--;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.gameObject.transform.position, new Vector2(this.transform.position.x, MaxY));
    }
}
