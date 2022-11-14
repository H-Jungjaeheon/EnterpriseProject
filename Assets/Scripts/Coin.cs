using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    public int CoinValue;

    public Vector3 GoToPos;

    // Start is called before the first frame update
    void Start()
    {
        float Ran = Random.Range(0.3f, 0.5f);

        this.transform.localScale = new Vector2(Ran, Ran);

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return null;

        float RanTime = Random.Range(0.5f, 0.7f);

        this.transform.DOMove(this.transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), 0.0f), RanTime);
        yield return new WaitForSeconds(RanTime + 0.3f);

        this.transform.DOMove(GoToPos, RanTime);
        yield return new WaitForSeconds(RanTime + 0.1f);

        this.gameObject.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.MoneyUnit += CoinValue;

        yield break;
    }
}
