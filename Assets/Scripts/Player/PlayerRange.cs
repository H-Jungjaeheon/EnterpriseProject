using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRange : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> TargetEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            TargetEnemy.Add(other.gameObject);
            Debug.Log(other.gameObject.name);
        }
    }

    public void RemoveTarget(GameObject Target)
    {
        TargetEnemy.Remove(Target);
    }
}
