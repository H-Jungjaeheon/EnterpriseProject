using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillElements
{
    Name,
    Description,
    Rating,
    CoolTime,
    ElementCount
}

public class SkillEquipManager : MonoBehaviour
{
    public int nowSkillIndex;
    public SkillData CurSkillData;

    [Tooltip("������ ��ų ���� â")]
    public GameObject changeObj;

    [SerializeField]
    [Tooltip("��ų ���� â")]
    private GameObject equipObj;

    [SerializeField]
    [Tooltip("ȭ��ǥ ������Ʈ")]
    private GameObject arrowObj;

    [SerializeField]
    [Tooltip("��ų ������")]
    private SkillData[] SkillDatas;

    [SerializeField]
    [Tooltip("��ų �̸�")]
    private string[] names;

    [SerializeField]
    [Tooltip("��ų ���")]
    private string[] descriptions;

    [SerializeField]
    [Tooltip("��ų ����")]
    private string[] ratings;

    [SerializeField]
    [Tooltip("��ų ��Ÿ��")]
    private float[] coolTimes;

    [SerializeField]
    [Tooltip("��ų ������ ��������Ʈ")]
    private Sprite[] iconSprite;

    [SerializeField]
    [Tooltip("��ų ��ҵ� �ؽ�Ʈ")]
    private Text[] elementTexts;

    [SerializeField]
    [Tooltip("���� ���� ��ų ������")]
    private Image nowSkillIcon;

    [SerializeField]
    [Tooltip("��ų �Ŵ��� ��ũ��Ʈ")]
    private SkillManager smComponent;

    public void SkillIconClick(int skillIndex)
    {
        nowSkillIndex = skillIndex;

        nowSkillIcon.sprite = iconSprite[skillIndex];

        elementTexts[(int)SkillElements.Name].text = names[skillIndex];

        elementTexts[(int)SkillElements.Description].text = descriptions[skillIndex];

        elementTexts[(int)SkillElements.Rating].text = ratings[skillIndex];

        elementTexts[(int)SkillElements.CoolTime].text = $"{coolTimes[skillIndex]} ��";

        CurSkillData = SkillDatas[skillIndex];

        equipObj.SetActive(true);
    }

    public void EquipButtonClick()
    {
        equipObj.SetActive(false);
        changeObj.SetActive(true);

        smComponent.isSkillEquiping = true;

        StartCoroutine(ArrowAnim());
    }

    IEnumerator ArrowAnim()
    {
        Vector2 moveSpeed = new Vector2(0, 0);

        while (true)
        {
            moveSpeed.y = Mathf.Sin(Time.time * 3);
            arrowObj.transform.position = moveSpeed / 6;
            if (arrowObj.activeSelf == false)
            {
                break;
            }
            yield return null;
        }
    }

    public void CloseSkillEquipPanel()
    {
        CurSkillData = null;
        equipObj.SetActive(false);
    }
}
