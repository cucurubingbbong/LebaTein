using UnityEngine;

public interface IBuildPreview
{
    void GetGhost(BuildCommand command);
    void Ghost(bool isBuild, Vector2Int pos);
    void GhostCancel();
}
