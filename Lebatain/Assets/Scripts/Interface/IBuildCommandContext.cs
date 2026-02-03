using UnityEngine;

public interface IBuildCommandContext
{
    int SelectedColorIndex { get; }
    TileBase GetTile(Vector2Int pos);
}
