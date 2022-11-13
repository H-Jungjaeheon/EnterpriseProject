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

    [Tooltip("변경할 스킬 장착 창")]
    public GameObject changeObj;

    [SerializeField]
    [Tooltip("스킬 장착 창")]
    private GameObject equipObj;

    [SerializeField]
    [Tooltip("화살표 오브젝트")]
    private GameObject arrowObj;

    [SerializeField]
    [Tooltip("스킬 데이터")]
    private SkillData[] SkillDatas;

    [SerializeField]
    [Tooltip("스킬 이름")]
    private string[] names;

    [SerializeField]
    [Tooltip("스킬 등급")]
    private string[] descriptions;

    [SerializeField]
    [Tooltip("스킬 설명")]
    private string[] ratings;

    [SerializeField]
    [Tooltip("스킬 쿨타임")]
    private float[] coolTimes;

    [SerializeField]
    [Tooltip("스킬 아이콘 스프라이트")]
    private Sprite[] iconSprite;

    [SerializeField]
    [Tooltip("스킬 요소들 텍스트")]
    private Text[] elementTexts;

    [SerializeField]
    [Tooltip("현재 선택 스킬 아이콘")]
    private Image nowSkillIcon;

    [SerializeField]
    [Tooltip("스킬 매니저 스크립트")]
    private SkillManager smComponent;

    public void SkillIconClick(int skillIndex)
    {
        nowSkillIndex = skillIndex;

        nowSkillIcon.sprite = iconSprite[skillIndex];

        elementTexts[(int)SkillElements.Name].text = names[skillIndex];

        elementTexts[(int)SkillElements.Description].text = descriptions[skillIndex];

        elementTexts[(int)SkillElements.Rating].text = ratings[skillIndex];

        elementTexts[(int)SkillElements.CoolTime].text = $"{coolTimes[skillIndex]} 초";

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
