using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�ΰ� �̹���")]
    private Image logoImage;

    [SerializeField]
    [Tooltip("�ΰ� �ؽ�Ʈ")]
    private Text logoText;

    private Vector3 logoMoveSpeed;

    [SerializeField]
    [Tooltip("�ΰ� �ؽ�Ʈ �÷�")]
    private Color color;

    private void Start()
    {
        StartCoroutine(TextAnim());
    }

    void Update()
    {
        ImageAnim();
        TouchDistinction();
    }

    private void ImageAnim()
    {
        logoMoveSpeed.y = 1000 + Mathf.Sin(Time.time) * 50;
        logoImage.transform.localPosition = logoMoveSpeed;
    }

    IEnumerator TextAnim()
    {
        while (true)
        {
            while (color.a < 1)
            {
                color.a += Time.deltaTime;
                logoText.color = color;
                yield return null;
            }
            while (color.a > 0)
            {
                color.a -= Time.deltaTime;
                logoText.color = color;
                yield return null;
            }
        }
    }

    private void TouchDistinction()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
    }
}
