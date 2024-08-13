using Godot;
using System;

public partial class Ghost : Area2D
{
	[Export]
	public int Speed { get; set; } = 100;

	public Vector2 Destination = Vector2.Zero;
	public bool IsSelected = false;
	private bool isMouseEnter = false;

	// References to child nodes
	private AnimatedSprite2D animatedSprite2D;
	private PointLight2D pointLight2D;

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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

	public void Select()
	{
		IsSelected = true;
		animatedSprite2D.Scale = new Vector2(2, 2);
	}

	public void Deselect()
	{
		IsSelected = false;
		animatedSprite2D.Scale = new Vector2(1, 1);
	}

	public override void _MouseEnter()
	{
		isMouseEnter = true;
	}

	public override void _MouseExit()
	{
		isMouseEnter = false;
	}

	private void MoveToDestination(double delta)
	{
		var acceptableDistance = 5;
		if (Position.DistanceTo(Destination) > acceptableDistance)
		{
			Vector2 target = (Destination - Position).Normalized();
			Vector2 velocity = target * Speed;
			animatedSprite2D.FlipH = velocity.X < 0;
			Position += velocity * (float)delta;
		}
	}

	private bool IsNearDestination()
	{
		return Destination.DistanceTo(Position) <= 64;
	}

	private void _on_area_entered(Area2D area)
	{
		GD.Print("hi area");
		if (area.HasMethod("AcceptGhost") && IsNearDestination())
		{
			GD.Print("hi tree");
			if ((bool)area.Call("AcceptGhost", this))
			{
				Hide();
				IsSelected = false;
				Destination = Vector2.Zero;
			}
		}
	}
}



