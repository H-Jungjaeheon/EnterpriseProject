using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MaterialRating
{
    IntermediateMaterial,
    FinestMaterial,
    InferiorMaterial
}

public class BattleSceneManager : Singleton<BattleSceneManager>
{
    public int[] quantityOfMaterials;
}
