using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
	private ColorRect selectionRect;
	private Vector2 selectionStart;
	private bool isSelecting = false;

	public override void _Ready()
	{
		selectionRect = GetNode<ColorRect>("SelectionRect");
		selectionRect.Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left)
			{
				if (mouseEvent.Pressed)
				{
					// Deselect all units
					foreach (var child in GetChildren())
					{
						if (child is Ghost ghost)
						{
							ghost.Deselect();
						}
					}

					// Start drawing the selection rectangle
					isSelecting = true;
					selectionStart = GetGlobalMousePosition();
					selectionRect.Visible = true;
					selectionRect.Position = selectionStart;
					selectionRect.Size = Vector2.Zero;
				}
				else
				{
					// Stop drawing and select units
					isSelecting = false;
					SelectUnitsInRectangle();
					selectionRect.Visible = false;
				}
			}
		}

		if (isSelecting && @event is InputEventMouseMotion)
		{
			// Update the size and position of the selection rectangle
			Vector2 selectionEnd = GetGlobalMousePosition();
			Vector2 rectPosition = new Vector2(
				Mathf.Min(selectionStart.X, selectionEnd.X),
				Mathf.Min(selectionStart.Y, selectionEnd.Y)
			);
			Vector2 rectSize = new Vector2(
				Mathf.Abs(selectionStart.X - selectionEnd.X),
				Mathf.Abs(selectionStart.Y - selectionEnd.Y)
			);

			selectionRect.Position = rectPosition;
			selectionRect.Size = rectSize;
		}
	}

	private void SelectUnitsInRectangle()
	{
		List<Ghost> selectedUnits = new List<Ghost>();
		Rect2 selectionBox = new Rect2(selectionRect.Position, selectionRect.Size);

		// Iterate through all children to check if they are inside the selection rectangle
		foreach (var child in GetChildren())
		{
			if (child is Ghost ghost)
			{
				Vector2 unitPosition = ghost.GlobalPosition;
				if (selectionBox.HasPoint(unitPosition))
				{
					selectedUnits.Add(ghost);
					ghost.Select();
					ghost.Modulate = Colors.White;  // Highlight selected units
				}
			}
		}

		GD.Print($"Selected Units: {selectedUnits.Count}");
	}
}
