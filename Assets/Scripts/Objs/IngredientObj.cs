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

    private int directionIndex; //���� �ִϸ��̼��� ƨ��� ������ ���� ������

    private float powerIndex; //���� �ִϸ��̼��� ���ư��� ������ ���� ������

    private Color ingredientColor = new Color(1, 1, 1, 0); //���� �ʱ�ȭ�� ��

    private Vector3 initialPos = new Vector3(0, 900, 0); //������Ʈ ��ġ �ʱ�ȭ

    private WaitForSeconds animDelay = new WaitForSeconds(0.4f); //�ִϸ��̼� ������

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
