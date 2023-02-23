using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override IEnumerator Attack()
    {
        return base.Attack();
    }

    protected override void Die()
    {
        Player.Instance.Range.TargetEnemy.Remove(this.gameObject);

        for (int i = 0; i < 3; i++)
        {
            Coin coin = Instantiate(CoinObj, this.transform.position, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f))).GetComponent<Coin>();

            coin.CoinValue = this.CoinValue / 3;
        }

        GameManager.Instance.GemUnit += 10;

        int cnt = TextObjs.Count;
        for (int i = 0; i < cnt; i++)
        {
            TextObjs.RemoveAt(0);
        }

        for (int i = 0; i < 3; i++)
        {
            BattleSceneManager.Instance.quantityOfMaterials[i]++;
        }

        ParticleSystemRenderer Particle = Instantiate(ParticlePrefab, this.transform.position, Quaternion.identity).GetComponent<ParticleSystemRenderer>();
        Particle.material = ParticleMaterial;

        Destroy(gameObject);
    }
}
