using Godot;

public partial class Map : Node2D
{
	[Export]
	public NoiseTexture2D NoiseTexture2D;

	[Export]
	public int SourceId = 2;

	public TileMap TileMap;
	private Noise noise;
	private Atlas atlas;
	private TreeSpawner treeSpawner;

	public override void _Ready()
	{
		atlas = new Atlas();
		noise = NoiseTexture2D.Noise;
		TileMap = GetNode<TileMap>("TileMap");
		treeSpawner = new TreeSpawner(this, TileMap);
		GenerateWorld(30, 30);
	}

	public override void _Process(double delta)
	{
		// Optional: Add any per-frame logic here
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("click"))
		{
			// Handle input event here if needed
		}
	}

	private void GenerateWorld(int width, int height)
	{
		for (int x = width; x > 0; x--)
		{
			for (int y = height; y > 0; y--)
			{
				float noiseVal = noise.GetNoise2D(x, y);
				if (noiseVal >= 0.0)
				{
					if (noiseVal >= 0.3 && noiseVal <= 0.4)
					{
						SetTileCell(new Vector2I(x, y), atlas.LavaAtlas);
						continue;
					}
					else if (noiseVal >= 0.4 && noiseVal <= 0.5)
					{
						SetTileCell(new Vector2I(x, y), atlas.Grass2Atlas);
						continue;
					}
					else
					{
						SetTileCell(new Vector2I(x, y), atlas.GrassAtlas);
					}
				}
				else
				{
					SetTileCell(new Vector2I(x, y), atlas.WaterAtlas);
				}
			}
		}

		var treeTiles = TileMap.GetUsedCellsById(0, SourceId, atlas.GrassAtlas);
		treeSpawner.SpawnTreesOnTiles(treeTiles);

		GD.Print("Done generating");
	}

	private void SetTileCell(Vector2I coords, Vector2I atlasCoords, int layer = 0)
	{
		TileMap.SetCell(layer, coords, SourceId, atlasCoords);
	}
}
