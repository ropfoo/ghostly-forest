class_name ManaTree extends Area2D

@onready var timer = $Timer
@onready var health_bar = $ProgressBar
@onready var sprite = $AnimatedSprite2D
@onready var ghost_light = $GhostLight

var is_selected = false
var is_mouse_enter = false

var ghost_container: GhostContainer
var health: float = 0.1

func _ready():
	playAnimaiton()
	health_bar.value = health

	sprite = $AnimatedSprite2D
	var should_flip: bool = randi() % 2
	sprite.flip_h = should_flip

	ghost_light.enabled = false
	ghost_container = GhostContainer.new(ghost_light)

	timer.timeout.connect(_on_timer_timeout)
	timer.start()
	pass # Replace with function body.

func _process(_delta):
	pass

func _input(_event):
	if Input.is_action_just_released("click"):
		is_selected = is_mouse_enter

	if Input.is_action_just_released("right_click") && is_selected:
			ghost_container.release_ghost(get_global_mouse_position())

func _mouse_enter():
	is_mouse_enter = true

func _mouse_exit():
	is_mouse_enter = false

func _on_timer_timeout():
	heal(0.1)

func accept_ghost(incoming_ghost: Ghost) -> bool:
	return ghost_container.set_ghost(incoming_ghost)

func heal(amount: float):
	if !ghost_container.ghost: return
	health += amount
	health_bar.value = health
	playAnimaiton()

func playAnimaiton():
	if health <= 0.1:
		sprite.animation="no_health"
		return
	if health >= 0.9:
		sprite.animation = "full_health"
		return
