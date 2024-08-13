using Godot;
using System;
using System.Collections.Generic;

public partial class TreeSpawner : Node
{
    private PackedScene _treeScene = (PackedScene)GD.Load("res://scenes/Tree/Tree.tscn");
    private TileMap _tileMap;
    private Node _node;

    public TreeSpawner(Node spawnNode, TileMap newTileMap)
    {
        _tileMap = newTileMap;
        _node = spawnNode;
    }

    public void CreateTree(Vector2I tilePosition)
    {
        var treeInstance = (ManaTree)_treeScene.Instantiate();
        treeInstance.Position = _tileMap.MapToLocal(tilePosition);
        treeInstance.VisibilityLayer = 2;
        _node.AddChild(treeInstance);
    }

    public void SpawnTreesOnTiles(Godot.Collections.Array<Vector2I> tiles)
    {
        var createdTrees = new List<Vector2I>();
        var radius = new Vector2(4, 5);  // Minimum distance between trees
        var random = new Random();

        foreach (var tile in tiles)
        {
            var shouldSpawn = random.Next(2) == 1;
            if (!shouldSpawn)
                continue;

            var canCreate = true;

            // Check if the current tile is far enough from all existing trees
            foreach (var tree in createdTrees)
            {
                if (Math.Abs(tree.X - tile.X) < radius.X && Math.Abs(tree.Y - tile.Y) < radius.Y)
                {
                    canCreate = false;
                    break;  // No need to check further
                }
            }

            // If the current tile is valid, create the tree and add it to the list
            if (canCreate)
            {
                CreateTree(tile);
                createdTrees.Add(tile);
            }
        }
    }
}
