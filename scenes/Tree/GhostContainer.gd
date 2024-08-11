class_name GhostContainer

var ghost: Ghost
var ghost_light: PointLight2D

func _init(light: PointLight2D):
	ghost_light = light

func set_ghost(incoming_ghost: Ghost) -> bool:
	if !ghost:
		ghost_light.enabled = true
		ghost = incoming_ghost
		return true
	return false

func release_ghost(position: Vector2):
	if ghost:
		ghost.show()
		ghost.destination = position
		ghost.is_selected = true
	ghost_light.enabled = false
	ghost = null
