using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRange : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> TargetEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy") && TargetEnemy.Contains(other.gameObject) == false)
        {
            TargetEnemy.Add(other.gameObject);
        }
    }

    public void RemoveTarget(GameObject Target)
    {
        TargetEnemy.Remove(Target);
    }
}
