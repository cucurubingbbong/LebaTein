using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GhostManager : MonoBehaviour, IBuildPreview
{
    /// <summary>
    /// 0 : true , 1 : false
    /// </summary>
    [SerializeField] Material[] ghostMatArr = new Material[2];

    private Material selectedMaterial;

    [SerializeField] GameObject currentGhostObj = null;
    private BuildCommand currentCommand = null;

    private MeshRenderer ghostMesh;

    public bool isGhost {get; private set;}

    public void GetGhost(BuildCommand command)
    {
        isGhost = true;
        currentCommand = command;
        currentGhostObj = Instantiate(currentCommand.ghost, new Vector3(-1 , -10 , -1) , Quaternion.identity);
        ghostMesh = currentGhostObj.AddComponent<MeshRenderer>();
    }

    public void Ghost(bool isBuild , Vector2Int pos)
    {
        selectedMaterial = isBuild ? ghostMatArr[0] : ghostMatArr[1];
        currentGhostObj.transform.position = new Vector3(pos.x , 1 , pos.y);
        ghostMesh.sharedMaterial = selectedMaterial;
    }

    public void GhostCancel()
    {
        isGhost = false;
        currentCommand = null;
        Destroy(currentGhostObj);
        ghostMesh = null;
    }



}
