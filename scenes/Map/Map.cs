using Godot;
using System;
using System.Numerics;
using System.Runtime.Serialization;

public partial class Map : Node2D
{

	[Export]
	public NoiseTexture2D noiseTexture2D;

	[Export]
	public int sourceId = 2;

	public TileMap tileMap;

	private Noise noise;

	private Atlas atlas;

	private PackedScene treeScene = GD.Load<PackedScene>("res://scenes/Tree/Tree.tscn");
	public override void _Ready()
	{
		atlas = new Atlas();
		noise = noiseTexture2D.Noise;
		tileMap = GetNode<TileMap>("TileMap");
		generateWorld(30, 30);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (Input.IsActionJustPressed("click"))
		{
			// var mousePos = GetGlobalMousePosition();
			// GD.Print(mousePos.X + " " + mousePos.Y);
			// var tileMousePos = tileMap.LocalToMap(mousePos);
			// GD.Print("tile: " + tileMousePos.X + " " + tileMousePos.Y);
			// var type = tileMap.GetCellAtlasCoords(0, tileMousePos);
			// GD.Print("type: " + type.X + " " + type.Y);

			// setTileCell(new Vector2I(tileMousePos.X, tileMousePos.Y), atlas.treeAtlas, 0);
			// createTree((int)mousePos.X, (int)mousePos.Y);
		}
	}

	private void generateWorld(int width, int height)
	{
		int treeCount = 0;

		for (int x = width; x > 0; x--)
		{
			for (int y = height; y > 0; y--)
			{
				float noiseVal = noise.GetNoise2D(x, y);
				if (noiseVal >= 0.0)
				{
					// paint lava
					if (noiseVal >= 0.3 && noiseVal <= 0.4)
					{
						setTileCell(new Vector2I(x, y), atlas.lavaAtlas);
						continue;
					}

					// paint grass2 (for trees)
					if (noiseVal >= 0.4 && noiseVal <= 0.5)
					{
						setTileCell(new Vector2I(x, y), atlas.grass2Atlas);
						continue;
					}

					// paint grass
					setTileCell(new Vector2I(x, y), atlas.grassAtlas);
					continue;
				}
				else
				{
					// paint water
					setTileCell(new Vector2I(x, y), atlas.waterAtlas);
				}
			}
		}

		var tiles = tileMap.GetUsedCellsById(0, sourceId, atlas.grass2Atlas);
		int length = 0;
		var random = new Random();
		foreach (var tile in tiles)
		{
			var randomBool = random.Next(2) == 1;
			if (randomBool)
			{
				CreateTree(tile);
			}
			length++;
			GD.Print(tile);
		}
		GD.Print(length);

		GD.Print("Done generating");
	}

	private void setTileCell(Vector2I coords, Vector2I atlasCoords, int layer = 0)
	{
		tileMap.SetCell(layer, coords, sourceId, atlasCoords);
	}

	private void CreateTree(Vector2I tilePosition)
	{
		var treeInstance = (Node2D)treeScene.Instantiate();
		treeInstance.Position = tileMap.MapToLocal(tilePosition);
		treeInstance.VisibilityLayer = 2;
		// add tree instance to the scene
		AddChild(treeInstance);
	}
}
