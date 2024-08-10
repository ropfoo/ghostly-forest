class_name TreeSpawner

var tree_scene: PackedScene = preload("res://scenes/Tree/Tree.tscn")
var tile_map: TileMap
var node: Node

func _init(spawn_node: Node, new_tile_map: TileMap):
	tile_map = new_tile_map
	node = spawn_node

func create_tree(tile_position: Vector2):
	var tree_instance: Area2D = tree_scene.instantiate()
	tree_instance.position = tile_map.map_to_local(tile_position)
	tree_instance.visibility_layer = 2
	node.add_child(tree_instance)

func spawn_trees_on_tiles(tiles: Array[Vector2i]):
	var random = RandomNumberGenerator.new()
	
	var created_trees: Array[Vector2] = []
	var radius = Vector2(3, 5)  # Minimum distance between trees

	for tile: Vector2 in tiles:
		var can_create = true

		# Check if the current tile is far enough from all existing trees
		for tree in created_trees:
			if abs(tree.x - tile.x) < radius.x and abs(tree.y - tile.y) < radius.y:
				can_create = false
				break  # No need to check further

		# If the current tile is valid, create the tree and add it to the list
		if can_create:
			create_tree(tile)
			created_trees.append(tile)