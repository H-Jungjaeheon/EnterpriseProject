using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum NowDirection
{
    Left,
    Right
}

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

    private int directionIndex; //실패 애니메이션의 튕기는 방향을 정할 랜덤값

    private float powerIndex; //실패 애니메이션의 날아가는 정도를 정할 랜덤값

    private Color ingredientColor = new Color(1, 1, 1, 0); //투명도 초기화용 색

    private Vector3 initialPos = new Vector3(0, 900, 0); //오브젝트 위치 초기화

    private WaitForSeconds animDelay = new WaitForSeconds(0.4f); //애니메이션 딜레이

    private IEnumerator spinCoroutine;

    public void StartIngredientAnim(bool isFail)
    {
        StartCoroutine(IngredientAnim(isFail));
    }

    private IEnumerator IngredientAnim(bool isFail)
    {
        sR.sprite = ingredientSprite[Random.Range(0, 4)];

        StartCoroutine(AlphaPlus());

        if (isFail == false)
        {
            sR.transform.DOLocalMoveY(450, 0.4f).SetEase(Ease.InBack);

            yield return animDelay;

            potObj.transform.DOScale(new Vector3(128f, 148f, 1), 0.1f);
            yield return new WaitForSeconds(0.13f);
            potObj.transform.DOScale(new Vector3(110, 130, 1), 0.1f);
        }
        else
        {
            directionIndex = Random.Range(0, 2);
            powerIndex = Random.Range(1, 5);

            sR.sortingOrder = 103;

            sR.transform.DOLocalMoveY(600, 0.4f).SetEase(Ease.InBack);

            yield return animDelay;

            if ((NowDirection)directionIndex == NowDirection.Left)
            {
                rigid.AddForce(Vector2.left, ForceMode2D.Impulse);
            }
            else
            {
                rigid.AddForce(Vector2.right, ForceMode2D.Impulse);
            }

            rigid.AddForce(Vector2.up * powerIndex, ForceMode2D.Impulse);

            spinCoroutine = SpinRotation();
            StartCoroutine(spinCoroutine);

            rigid.gravityScale = 1;

            yield return animDelay;
        }
        StartCoroutine(AlphaMinus());
    }

    IEnumerator SpinRotation()
    {
        Vector3 spinSpeed = new Vector3(0, 0, 50);

        while (true)
        {
            transform.rotation = Quaternion.Euler(spinSpeed * Time.deltaTime);
            yield return null;
        }
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
            nowAlpha -= Time.deltaTime * 2;
            ingredientColor.a = nowAlpha;
            sR.color = ingredientColor;
            yield return null;
        }

        StopCoroutine(spinCoroutine);
        sR.sortingOrder = 101;
        rigid.gravityScale = 0;
        rigid.velocity = Vector2.zero;
        sR.transform.DOLocalMove(initialPos, 0);
    }
}
