using Godot;

public partial class LumberJack : Moveable
{
	private Map map;

	public override void _Ready()
	{
		map = GetNode<Map>("../Map");
		Sprite = GetNode<AnimatedSprite2D>("LumberJackSprite");
	}

	public override void _Process(double delta)
	{
		if (Destination == Vector2.Zero)
		{
			FindManaTree();
		}

		if (Destination != Vector2.Zero)
		{
			HasArrived = MoveToDestination(delta, acceptableDistance: 20);
		}
	}

	private void FindManaTree()
	{
		foreach (var child in map.GetChildren())
		{
			if (child is ManaTree manaTree)
			{
				if (manaTree.Health > 0.2)
				{
					GD.Print(manaTree.Health, manaTree.Position, Position);
					Destination = manaTree.Position;
					break;
				}
			}
		}

	}
}
