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

    private int rotIndex;

    public void GetGhost(BuildCommand command)
    {
        isGhost = true;
        currentCommand = command;
        currentGhostObj = Instantiate(currentCommand.ghost, new Vector3(-1 , -10 , -1) , Quaternion.identity);
        ghostMesh = currentGhostObj.gameObject.GetComponent<MeshRenderer>();
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
        Destroy(currentGhostObj);
        currentCommand = null;
        ghostMesh = null;
    }

    public void GhostRotate(int direction)
    {
        int step = (direction == 0) ? -1 : 1;
        rotIndex = (rotIndex + step) & 3; 
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        if (currentGhostObj == null) return;
        currentGhostObj.transform.rotation = Quaternion.Euler(0f, rotIndex * 90f, 0f);
    }

    public GameObject GetGhostObj()
    {
        return currentGhostObj;
    }
}
