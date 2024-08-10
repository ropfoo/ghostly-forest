extends Area2D

@onready var timer = $Timer
@onready var health_bar = $ProgressBar
@onready var sprite = $AnimatedSprite2D

var is_selected = false
var is_mouse_enter = false

var ghost: Ghost
var health: float = 0.1

func _ready():
	playAnimaiton()
	health_bar.value = health

	timer.timeout.connect(_on_timer_timeout)
	timer.start()
	pass # Replace with function body.

func _process(_delta):
	pass

func _input(_event):
	if Input.is_action_just_released("click"):
		is_selected = is_mouse_enter

	if Input.is_action_just_released("right_click") && is_selected:
			release_ghost()

func _mouse_enter():
	is_mouse_enter = true

func _mouse_exit():
	is_mouse_enter = false

func _on_timer_timeout():
	heal(0.1)

func accept_ghost(incoming_ghost: Ghost) -> bool:
	if !ghost:
		ghost = incoming_ghost
		return true
	return false

func release_ghost():
	if ghost:
		ghost.show()
	ghost = null

func heal(amount: float):
	if !ghost: return
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
