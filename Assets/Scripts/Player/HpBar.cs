using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField]
    private Image HpFrontImage;

    private void Update()
    {
        HpFrontImage.fillAmount = (float)Player.Instance.Hp / 100.0f;
    }
}
