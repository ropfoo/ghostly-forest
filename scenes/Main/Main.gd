extends Node2D

@onready var selection_rect = $SelectionRect  # Your ColorRect for selection
var selection_start = Vector2()  # Start point of the rectangle
var is_selecting = false

func _ready():
	selection_rect.visible = false

func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				for unit in get_children():
					if unit is Ghost:
						unit.deselect()
				# Start drawing the selection rectangle
				is_selecting = true
				selection_start = get_global_mouse_position()
				selection_rect.visible = true
				selection_rect.position = selection_start
				selection_rect.size = Vector2.ZERO
			else:
				# Stop drawing and select units
				is_selecting = false
				select_units_in_rectangle()
				selection_rect.visible = false

	if is_selecting and event is InputEventMouseMotion:
		# Update the size and position of the selection rectangle
		var selection_end = get_global_mouse_position()
		var rect_position = Vector2(
			min(selection_start.x, selection_end.x),
			min(selection_start.y, selection_end.y)
		)
		var rect_size = Vector2(
			abs(selection_start.x - selection_end.x),
			abs(selection_start.y - selection_end.y)
		)
		selection_rect.position = rect_position
		selection_rect.size = rect_size

func select_units_in_rectangle():
	var selected_units = []
	var selection_box = Rect2(selection_rect.position, selection_rect.size)

	# Iterate through all children to check if they are inside the selection rectangle
	for unit in get_children():
		if unit is Ghost:
			var unit_position = unit.global_position
			if selection_box.has_point(unit_position):
				selected_units.append(unit)
				unit.select()
				unit.modulate = Color(1, 1, 1)  # Highlight selected units

	print("Selected Units: ", selected_units.size())
