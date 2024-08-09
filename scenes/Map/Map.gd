extends Node2D

@export var noise_texture_2d: NoiseTexture2D
@export var source_id: int = 2

var tile_map: TileMap
var noise: Noise
var atlas: Atlas
var tree_scene: PackedScene = preload("res://scenes/Tree/Tree.tscn")

func _ready():
	atlas = Atlas.new()
	noise = noise_texture_2d.noise
	tile_map = get_node("TileMap")
	generate_world(30, 30)

func _process(delta):
	pass

func _input(event):
	if Input.is_action_just_pressed("click"):
		# Uncomment the following lines if you want to handle click events
		# var mouse_pos = get_global_mouse_position()
		# GD.print(mouse_pos.x + " " + mouse_pos.y)
		# var tile_mouse_pos = tile_map.local_to_map(mouse_pos)
		# GD.print("tile: " + tile_mouse_pos.x + " " + tile_mouse_pos.y)
		# var type = tile_map.get_cell_atlas_coords(0, tile_mouse_pos)
		# GD.print("type: " + type.x + " " + type.y)

		# Uncomment to set a tree at clicked position
		# set_tile_cell(Vector2(tile_mouse_pos.x, tile_mouse_pos.y), atlas.tree_atlas, 0)
		# create_tree(Vector2(tile_mouse_pos.x, tile_mouse_pos.y))
		pass

func generate_world(width, height):
	var tree_count = 0

	for x in range(width, 0, -1):
		for y in range(height, 0, -1):
			var noise_val = noise.get_noise_2d(x, y)
			if noise_val >= 0.0:
				if noise_val >= 0.3 and noise_val <= 0.4:
					set_tile_cell(Vector2(x, y), atlas.lava_atlas)
					continue
				elif noise_val >= 0.4 and noise_val <= 0.5:
					set_tile_cell(Vector2(x, y), atlas.grass2_atlas)
					continue
				else:
					set_tile_cell(Vector2(x, y), atlas.grass_atlas)
			else:
				set_tile_cell(Vector2(x, y), atlas.water_atlas)

	var tiles = tile_map.get_used_cells_by_id(0, source_id, atlas.grass2_atlas)
	var length = 0
	var random = RandomNumberGenerator.new()

	for tile in tiles:
		if random.randi_range(0, 1) == 1:
			create_tree(tile)
		length += 1
		print(tile)

	print(length)
	print("Done generating")

func set_tile_cell(coords, atlas_coords, layer = 0):
	tile_map.set_cell(layer, coords, source_id, atlas_coords)

func create_tree(tile_position):
	var tree_instance = tree_scene.instantiate()
	tree_instance.position = tile_map.map_to_local(tile_position)
	tree_instance.visibility_layer = 2
	add_child(tree_instance)
