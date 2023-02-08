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
