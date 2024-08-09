using Godot;

public partial class Ghost : Area2D
{
	[Export]
	public int Speed { get; set; } = 100;

	AnimatedSprite2D animatedSprite2D;
	PointLight2D pointLight2D;
	Godot.Vector2 destination;
	bool isSelected = false;
	bool isMouseEnter = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		pointLight2D = GetNode<PointLight2D>("PointLight2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (destination.Length() > 0)
		{
			moveToDestination(delta);
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (Input.IsActionJustReleased("click"))
		{
			isSelected = isMouseEnter;

		}
		if (Input.IsActionJustPressed("right_click"))
		{
			if (isSelected)
			{
				destination = GetGlobalMousePosition();
			}
		}
	}

	public override void _MouseEnter()
	{
		base._MouseEnter();
		isMouseEnter = true;
	}

	public override void _MouseExit()
	{
		base._MouseExit();
		isMouseEnter = false;
	}

	private void moveToDestination(double delta)
	{
		var acceptableDistance = 5;
		if (Position.DistanceTo(destination) > acceptableDistance)
		{
			var target = (destination - Position).Normalized();
			var velocity = target * Speed;
			// animatedSprite2D.FlipH = velocity.X < 0;
			Position += velocity * (float)delta;
		}
	}
}
