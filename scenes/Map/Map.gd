extends Node2D

@export var noise_texture_2d: NoiseTexture2D
@export var source_id: int = 2

var tile_map: TileMap
var noise: Noise
var atlas: Atlas
var tree_spawner: TreeSpawner

func _ready():
	atlas = Atlas.new()
	noise = noise_texture_2d.noise
	tile_map = get_node("TileMap")
	tree_spawner = TreeSpawner.new(self, tile_map)
	generate_world(30, 30)

func _process(delta):
	pass

func _input(event):
	if Input.is_action_just_pressed("click"):
		pass

func generate_world(width, height):
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

	var tree_tiles = tile_map.get_used_cells_by_id(0, source_id, atlas.grass_atlas)
	tree_spawner.spawn_trees_on_tiles(tree_tiles)

	print("Done generating")

func set_tile_cell(coords, atlas_coords, layer = 0):
	tile_map.set_cell(layer, coords, source_id, atlas_coords)


