using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public Material[] colorMaterialArr = new Material[7];

    private void Awake()
    {
        Instance = this;
    }

    public Material GetMaerterial(int index)
    {
        return colorMaterialArr[index];
    }
}
