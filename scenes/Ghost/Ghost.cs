using Godot;

public partial class Ghost : Moveable, IUnit
{
	public bool IsSelected = false;
	private bool isMouseEnter = false;

	// References to child nodes
	private PointLight2D pointLight2D;

	public override void _Ready()
	{
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		pointLight2D = GetNode<PointLight2D>("PointLight2D");
	}

	public override void _Process(double delta)
	{
		if (Destination.Length() > 0)
		{
			MoveToDestination(delta);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustReleased("click"))
		{
			IsSelected = isMouseEnter;
		}

		if (Input.IsActionJustPressed("right_click") && IsSelected)
		{
			Destination = GetGlobalMousePosition();
		}
	}

	public Vector2 GetPosition()
	{
		return GlobalPosition;
	}

	public void Select()
	{
		IsSelected = true;
		Sprite.Scale = new Vector2(2, 2);
	}

	public void Deselect()
	{
		IsSelected = false;
		Sprite.Scale = new Vector2(1, 1);
	}

	public override void _MouseEnter()
	{
		isMouseEnter = true;
	}

	public override void _MouseExit()
	{
		isMouseEnter = false;
	}


	public bool IsNearDestination()
	{
		return Destination.DistanceTo(Position) <= 64;
	}

	public UnitType GetUnitType()
	{
		return UnitType.Ghost;
	}

	public void CheckIntersect(IUnit unit)
	{
		if (unit.GetUnitType() == UnitType.Ghost)
		{
			Position = new Vector2(Position.X + 100, Position.Y + 100);
		}
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.HasMethod("AcceptGhost") && IsNearDestination())
		{
			if ((bool)area.Call("AcceptGhost", this))
			{
				Hide();
				IsSelected = false;
				Destination = Vector2.Zero;
			}
		}

		if (area is Ghost)
		{
			Destination = Position;
		}

	}
}



