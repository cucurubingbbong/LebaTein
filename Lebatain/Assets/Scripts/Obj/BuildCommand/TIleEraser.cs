using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
public class TileEraser : BuildCommand
{
    public override void Build(Vector2Int pos)
    {
        TileBase tile = context.GetTile(pos);
        tile.color = (ColorType)6;
        tile.SetColor(6);
    }

    public override void Init()
    {
        buildUnitType = UnitType.Tile;
    }
}
