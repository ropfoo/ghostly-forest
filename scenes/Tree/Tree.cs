using Godot;
using System;

public partial class Tree : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
	{
		base._InputEvent(viewport, @event, shapeIdx);
		if (Input.IsActionJustPressed("click"))
		{
			GD.Print("clicked on tree");
		}
	}
	// public override void _(InputEvent @event)
	// {
	// 	base._Input(@event);
	// 	if (Input.IsActionJustPressed("click"))
	// 	{
	// 		GD.Print("clicked on tree");
	// 	}

	// }
}
