using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BezierMissile : MonoBehaviour
{

    Vector2[] point = new Vector2[4];
    Animator anim;
    bool hit = false;

    [SerializeField] [Range(0, 1)] private float t = 0;
    [SerializeField] public float spd = 5;
    [SerializeField] public float posA = 0.55f;
    [SerializeField] public float posB = 0.45f;

    public GameObject master;
    public GameObject enemy;

    public bool IsEnemy = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Attack");

            other.GetComponent<Enemy>().StartTakeDamage(20, false);
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        point[0] = master.transform.position; // P0
        point[1] = PointSetting(master.transform.position); // P1
        point[2] = PointSetting(enemy.transform.position); // P2
        point[3] = enemy.transform.position; // P3
    }

    void FixedUpdate()
    {
        if (t > 1) return;
        if (hit) return;
        t += Time.deltaTime * spd;
        DrawTrajectory();

        if(enemy.activeSelf == false && IsEnemy == true || !enemy)
        {
            IsEnemy = false;

            this.gameObject.GetComponent<SpriteRenderer>().DOFade(0, 1.5f);
            Destroy(this.gameObject, 1.5f);
        }

    }

    Vector2 PointSetting(Vector2 origin)
    {
        float x, y;

        x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad)
            + origin.x;
        y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad)
            + origin.y;
        return new Vector2(x, y);
    }

    void DrawTrajectory()
    {
        transform.position = new Vector2(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)
        );
    }


    private float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
    }


}
