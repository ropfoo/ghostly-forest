extends Area2D

signal hit

@export var speed: int = 100

var destination: Vector2 = Vector2.ZERO
var is_selected: bool = false
var is_mouse_enter: bool = false

# Reference to the child nodes
@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D
@onready var point_light_2d: PointLight2D = $PointLight2D

# Called when the node enters the scene tree for the first time.
func _ready():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
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
		# animated_sprite_2d.flip_h = velocity.x < 0
		position += velocity * delta

func _on_area_entered(area):
	print("hi tree")
	if area.has_method("accept_ghost"):
		if area.accept_ghost():
			# queue_free()
			$AnimatedSprite2D.animation = "idle"
		print("cool")			
