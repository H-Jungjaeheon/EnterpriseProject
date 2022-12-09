using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IngredientObj : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�ش� ������Ʈ�� ��������Ʈ ������ ������Ʈ")]
    private SpriteRenderer sR;

    [SerializeField]
    [Tooltip("��� ���ҽ���")]
    private Sprite[] ingredientSprite;

    [SerializeField]
    [Tooltip("���� ������Ʈ")]
    private GameObject potObj;

    [SerializeField]
    [Tooltip("�ش� ������Ʈ�� ������ٵ�(2D) ������Ʈ")]
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
            rigid.AddForce(Vector2.left, ForceMode2D.Impulse); //�������� ��, ��, ��
            rigid.gravityScale = 1;

            yield return new WaitForSeconds(0.4f);

            StartCoroutine(AlphaMinus());
        }

        yield return null;
    }

    /// <summary>
    /// ��� ���� �ִϸ��̼� ���� �� �������̵� ���̵� �� ȿ�� �Լ�
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
    /// ��� ���� �ִϸ��̼� ���� �� �������̵� ���̵� �ƿ� ȿ�� �Լ�
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
