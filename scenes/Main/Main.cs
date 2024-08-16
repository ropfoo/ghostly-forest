using Godot;
using System.Collections.Generic;

public partial class Main : Node2D
{
	List<IUnit> selectedUnits = new List<IUnit>();

	private double mana;

	private ColorRect selectionRect;
	private ResourcePanel resourcePanel;

	private Vector2 selectionStart;
	private bool isSelecting = false;

	public override void _Ready()
	{
		selectionRect = GetNode<ColorRect>("SelectionRect");
		resourcePanel = GetNode<ResourcePanel>("GUI/ResourcePanel");
		resourcePanel.SetManaCount(10);
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
					DeselectUnits();

					// Start drawing the selection rectangle
					isSelecting = true;
					selectionStart = GetGlobalMousePosition();
					selectionRect.Visible = true;
					selectionRect.Position = selectionStart;
					selectionRect.Size = Vector2.Zero;
					SelectUnitOnClick();
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

	private void SelectUnitOnClick()
	{
		foreach (var child in GetChildren())
		{
			if (child is IUnit unit)
			{
				if (unit.GetPosition().DistanceTo(GetGlobalMousePosition()) < 20)
				{
					selectedUnits.Add(unit);
					unit.Select();
					GD.Print($"Selected Units: {selectedUnits.Count}");
				}
			}
		}
	}

	private void SelectUnitsInRectangle()
	{
		Rect2 selectionBox = new Rect2(selectionRect.Position, selectionRect.Size);

		// Iterate through all children to check if they are inside the selection rectangle
		foreach (var child in GetChildren())
		{
			if (child is IUnit unit)
			{
				Vector2 unitPosition = unit.GetPosition();
				if (selectionBox.HasPoint(unitPosition))
				{
					selectedUnits.Add(unit);
					unit.Select();
				}
			}
		}

		GD.Print($"Selected Units: {selectedUnits.Count}");
	}

	private void DeselectUnits()
	{
		foreach (var unit in selectedUnits)
		{
			unit.Deselect();
		}
		selectedUnits = new List<IUnit>();
	}

	public void AddToManaCount(double newMana)
	{
		mana += newMana;
		resourcePanel.SetManaCount(mana);
		GD.Print(mana);
	}
}

