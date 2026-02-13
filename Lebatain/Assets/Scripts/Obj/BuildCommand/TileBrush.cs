using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
public class TileBrush : BuildCommand
{
    public override void Build(Vector2Int pos)
    {
        TileBase tile = context.GetTile(pos);
        tile.color = (ColorType)context.SelectedColorIndex;
        tile.SetColor(context.SelectedColorIndex);
    }

    public override void Init()
    {
        buildUnitType = UnitType.Tile;
    }
}
