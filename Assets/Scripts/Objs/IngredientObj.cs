using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IngredientObj : MonoBehaviour
{
    [SerializeField]
    [Tooltip("해당 오브젝트의 스프라이트 렌더러 컴포넌트")]
    private SpriteRenderer sR;

    [SerializeField]
    [Tooltip("재료 리소스들")]
    private Sprite[] ingredientSprite;

    [SerializeField]
    [Tooltip("냄비 오브젝트")]
    private GameObject potObj;

    [SerializeField]
    [Tooltip("해당 오브젝트의 리지드바디(2D) 컴포넌트")]
    private Rigidbody2D rigid;

    Color ingredientColor = new Color(1, 1, 1, 0);

    public void StartIngredientAnim(bool isFail)
    {
        StartCoroutine(IngredientAnim(isFail));
    }

    private IEnumerator IngredientAnim(bool isFail)
    {
        sR.sprite = ingredientSprite[Random.Range(0, 4)];
        sR.transform.DOLocalMove(new Vector2(0, 900), 0);

        StartCoroutine(AlphaPlus());

        if (isFail == false)
        {
            sR.transform.DOLocalMoveY(550, 0.4f).SetEase(Ease.InBack);

            yield return new WaitForSeconds(0.4f);

            ingredientColor.a = 0;
            sR.color = ingredientColor;

            potObj.transform.DOScale(new Vector3(1.08f, 1.08f, 1), 0.1f);
            yield return new WaitForSeconds(0.13f);
            potObj.transform.DOScale(new Vector3(1, 1, 1), 0.1f);
        }
        else
        {
            sR.transform.DOLocalMoveY(600, 0.4f).SetEase(Ease.InBack);

            yield return new WaitForSeconds(0.4f);

            rigid.AddForce(Vector2.up * 3.5f, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.left, ForceMode2D.Impulse); //랜덤으로 왼, 오, 힘
            rigid.gravityScale = 1;

            yield return new WaitForSeconds(0.4f);

            StartCoroutine(AlphaMinus());
        }

        yield return null;
    }

    /// <summary>
    /// 재료 투입 애니메이션 시작 시 스프라이드 페이드 인 효과 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator AlphaPlus()
    {
        float nowAlpha = 0;

        while (nowAlpha < 1)
        {
            nowAlpha += Time.deltaTime * 6;
            ingredientColor.a = nowAlpha;
            sR.color = ingredientColor;
            yield return null;
        }
    }

    /// <summary>
    /// 재료 투입 애니메이션 종료 시 스프라이드 페이드 아웃 효과 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator AlphaMinus()
    {
        float nowAlpha = 1;

        while (nowAlpha > 0)
        {
            nowAlpha -= Time.deltaTime;
            ingredientColor.a = nowAlpha;
            sR.color = ingredientColor;
            yield return null;
        }
    }
}
