using Godot;
using System;
using System.Numerics;
using System.Runtime.Serialization;

public partial class RandomMap : Node2D
{

	[Export]
	public NoiseTexture2D noiseTexture2D;

	public TileMap tileMap;

	private Noise noise;

	public override void _Ready()
	{
		noise = noiseTexture2D.Noise;
		tileMap = GetNode<TileMap>("TileMap");

		Godot.Vector2 screenSize = GetViewportRect().Size;
		var width = (int)screenSize.X / 2;
		var height = (int)screenSize.Y / 2;
		generateWorld(width, height);


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void generateWorld(int width, int height)
	{
		int sourceId = 0;

		Vector2I lavaAtlas = new Vector2I(0, 0);
		Vector2I grassAtlas = new Vector2I(1, 0);
		Vector2I waterAtlas = new Vector2I(2, 0);

		for (int x = width; x > 0; x--)
		{
			for (int y = height; y > 0; y--)
			{
				float noiseVal = noise.GetNoise2D(x, y);
				if (noiseVal >= 0.0)
				{
					// paint lava
					if (noiseVal >= 0.3)
					{
						tileMap.SetCell(0, new Vector2I(x, y), sourceId, lavaAtlas);
						continue;
					}

					// paint grass
					tileMap.SetCell(0, new Vector2I(x, y), sourceId, grassAtlas);
					continue;
				}
				else
				{
					// paint water
					tileMap.SetCell(0, new Vector2I(x, y), sourceId, waterAtlas);
				}
			}
		}

		GD.Print("Done generating");
	}
}
