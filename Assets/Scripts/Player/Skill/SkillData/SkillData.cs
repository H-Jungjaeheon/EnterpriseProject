using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable/SkillData")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public Sprite SkillImage;
    public float SkillCol;
    public float SkillDuration;
}
