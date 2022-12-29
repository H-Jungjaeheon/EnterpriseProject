using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RiggingType
{
    Shoes,
    Bowl,
    Spoon
}

[System.Serializable]
public class Rigging
{
    [Tooltip("장비 ID")]
    public int id;

    [Tooltip("장비 종류")]
    public RiggingType type;

    [Tooltip("장비 잠금해제 여부")]
    public bool isUnlock;

    [Tooltip("장비 아이콘")]
    public Sprite icon;

    [Tooltip("장비 이름")]
    public string name;

    [Tooltip("장비 설명")]
    public string explanation;

    [Tooltip("장비 등급")]
    public int rank;

    [Tooltip("장비 레벨")]
    public int level;

    [Tooltip("최대 장비 경험치")]
    public int maxExp;

    [Tooltip("현재 장비 경험치")]
    public int nowExp;
}

public class RiggingManager : MonoBehaviour
{
    [Tooltip("장비 데이터들")]
    public Rigging[] riggingDatas;

    [SerializeField]
    [Tooltip("장비 장착 버튼들")]
    private Button[] equipButtons;

    [SerializeField]
    [Tooltip("장비 장착 버튼 이미지들")]
    private Image[] buttonImage;

    /// <summary>
    /// 장비 장착 화면 활성화 시 신발 종류의 장비들로 버튼 세팅
    /// </summary>
    private void OnEnable()
    {
        ButtonSetting(0);
    }

    /// <summary>
    /// 장비 장착 버튼들 장비 종류 변경(버튼)
    /// </summary>
    /// <param name="typeIndex"> 장비 종류 인덱스(RiggingType) </param>
    public void ButtonSetting(int typeIndex)
    {
        RiggingType riggingType = (RiggingType)typeIndex; //현재 장비 타입
        int buttonIndex = 0; //현재 버튼 인덱스

        for (int nowIndex = riggingDatas.Length - 1; nowIndex >= 0; nowIndex--)
        {
            if (riggingDatas[nowIndex].type == riggingType && riggingDatas[nowIndex].isUnlock)
            {
                //equipButtons[buttonIndex].onClick.AddListener 
                buttonImage[buttonIndex].sprite = riggingDatas[nowIndex].icon;

                buttonIndex++;
            }
        }
    }
}
