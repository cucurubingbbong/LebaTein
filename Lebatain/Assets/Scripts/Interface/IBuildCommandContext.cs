using UnityEngine;

public interface IBuildCommandContext
{
    int SelectedColorIndex { get; }
    TileBase GetTile(Vector2Int pos);
    UnitBase GetUnit(Vector2Int pos);

    void SetUnit(UnitBase unit , Vector2Int pos);
}
