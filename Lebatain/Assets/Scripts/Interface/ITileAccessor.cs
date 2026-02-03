using UnityEngine;

public interface ITileAccessor
{
    TileBase GetTile(Vector2Int gridPos);
}
