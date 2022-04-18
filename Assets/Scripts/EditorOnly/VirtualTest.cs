#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualTest : MonoBehaviour
{
    private void Start()
    {
        //BoardTest();
        //MatchTest();
        SpaceFillTest();
    }

    private static void BoardTest()
    {
        Game game = new Game(new Vector2Int(3, 3));
        Tile.TileType red = new Tile.TileType('r', editorTileColor: "red");
        Tile.TileType green = new Tile.TileType('g', editorTileColor: "lime");
        Tile.TileType blue = new Tile.TileType('b', editorTileColor: "aqua");
        Grid grid = game.grid;

        grid.GetCellAt(new Vector2Int(0, 0)).tile = new Tile(game, blue);
        grid.GetCellAt(new Vector2Int(1, 0)).tile = new Tile(game, green);
        grid.GetCellAt(new Vector2Int(2, 0)).tile = new Tile(game, red);

        grid.GetCellAt(new Vector2Int(0, 1)).tile = new Tile(game, red);
        grid.GetCellAt(new Vector2Int(1, 1)).tile = new Tile(game, red);
        grid.GetCellAt(new Vector2Int(2, 1)).tile = new Tile(game, red);

        grid.GetCellAt(new Vector2Int(0, 2)).tile = new Tile(game, blue);
        grid.GetCellAt(new Vector2Int(1, 2)).tile = new Tile(game, green);
        grid.GetCellAt(new Vector2Int(2, 2)).tile = new Tile(game, red);
        Debug.Log(grid);
    }

    private static void MatchTest()
    {
        Game game = new Game(new Vector2Int(3, 3));
        Tile.TileType red = new Tile.TileType('r', editorTileColor: "red");
        Tile.TileType green = new Tile.TileType('g', editorTileColor: "lime");
        Tile.TileType blue = new Tile.TileType('b', editorTileColor: "aqua");
        Grid grid = game.grid;

        grid.GetCellAt(new Vector2Int(0, 0)).tile = new Tile(game, blue);
        grid.GetCellAt(new Vector2Int(1, 0)).tile = new Tile(game, green);
        grid.GetCellAt(new Vector2Int(2, 0)).tile = new Tile(game, red);

        grid.GetCellAt(new Vector2Int(0, 1)).tile = new Tile(game, red);
        grid.GetCellAt(new Vector2Int(1, 1)).tile = new Tile(game, red);
        grid.GetCellAt(new Vector2Int(2, 1)).tile = new Tile(game, red);

        grid.GetCellAt(new Vector2Int(0, 2)).tile = new Tile(game, blue);
        grid.GetCellAt(new Vector2Int(1, 2)).tile = new Tile(game, green);
        grid.GetCellAt(new Vector2Int(2, 2)).tile = new Tile(game, red);
        Debug.Log(grid);

        game.Start();

        Debug.Log("----------------------------------------");
        Match[] results = MatchDetector.Find(grid);
        Debug.Log(results.Length + " match(s) found");
        foreach (Match r in results)
        {
            Debug.Log(r);
        }
    }

    private static void SpaceFillTest()
    {
        Game game = new Game(new Vector2Int(3, 3));
        Tile.TileType red = new Tile.TileType('r', editorTileColor: "red");
        Tile.TileType green = new Tile.TileType('g', editorTileColor: "lime");
        Tile.TileType blue = new Tile.TileType('b', editorTileColor: "aqua");
        Grid grid = game.grid;

        grid.GetCellAt(new Vector2Int(0, 0)).tile = new Tile(game, blue);
        grid.GetCellAt(new Vector2Int(1, 0)).tile = new Tile(game, green);
        grid.GetCellAt(new Vector2Int(2, 0)).tile = null;

        grid.GetCellAt(new Vector2Int(0, 1)).tile = null;
        grid.GetCellAt(new Vector2Int(1, 1)).tile = null;
        grid.GetCellAt(new Vector2Int(2, 1)).tile = null;

        grid.GetCellAt(new Vector2Int(0, 2)).tile = new Tile(game, blue);
        grid.GetCellAt(new Vector2Int(1, 2)).tile = null;
        grid.GetCellAt(new Vector2Int(2, 2)).tile = new Tile(game, green);
        Debug.Log(grid);

        game.Start();

        Debug.Log("----------------------------------------");
        DroppedTile[] droppedTiles;
        SpaceFiller.Fill(ref grid, new FillStrategyEmpty(), out droppedTiles);
        if (droppedTiles.Length > 0)
        {
            Debug.Log(droppedTiles.Length + " tiles dropped");
            foreach (DroppedTile dt in droppedTiles)
            {
                Debug.Log(dt);
            }
            Debug.Log(grid);
        }
        else
        {
            Debug.Log("No change after space fill");
            Debug.Log(grid);
        }
    }
}

#endif