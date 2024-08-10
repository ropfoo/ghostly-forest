class_name  Ghost extends Area2D

signal hit

@export var speed: int = 100

var destination: Vector2 = Vector2.ZERO
var is_selected: bool = false
var is_mouse_enter: bool = false

# reference to child nodes
@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D
@onready var point_light_2d: PointLight2D = $PointLight2D

func _ready():
	pass

func _process(delta):
	if destination.length() > 0:
		move_to_destination(delta)

func _input(_event):
	if Input.is_action_just_released("click"):
		is_selected = is_mouse_enter

	if Input.is_action_just_pressed("right_click"):
		if is_selected:
			destination = get_global_mouse_position()

func _mouse_enter():
	is_mouse_enter = true

func _mouse_exit():
	is_mouse_enter = false

func move_to_destination(delta):
	var acceptable_distance = 5
	if position.distance_to(destination) > acceptable_distance:
		var target: Vector2 = (destination - position).normalized()
		var velocity: Vector2 = target * speed
		position += velocity * delta

func _on_area_entered(area):
	if area.has_method("accept_ghost") && is_near_destination():
		print("hi tree")
		if area.accept_ghost(self):
			hide()
			is_selected = false
			destination = Vector2.ZERO

# check if ghost is near its destination
func is_near_destination() -> bool:
	return destination.distance_to(position) <= 64 # <- buffer
