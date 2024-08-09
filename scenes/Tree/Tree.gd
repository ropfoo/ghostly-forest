extends Area2D

var has_ghost = false
var is_selected = false
var is_mouse_enter = false

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass

func _input(_event):
	if Input.is_action_just_released("click"):
		is_selected = is_mouse_enter

	if Input.is_action_just_released("release") && is_selected:
		 	# spawn ghost
			pass

func _mouse_enter():
	is_mouse_enter = true

func _mouse_exit():
	is_mouse_enter = false

func accept_ghost() -> bool:
	print("let me check if i can accept one")
	if !has_ghost:
		print("yep")
		has_ghost = true
		return true
	print("nope")	
	return false
