using Godot;

public partial class Atlas : Resource
{
    public Vector2I GrassAtlas { get; private set; } = new Vector2I(0, 0);
    public Vector2I Grass2Atlas { get; private set; } = new Vector2I(1, 0);
    public Vector2I LavaAtlas { get; private set; } = new Vector2I(0, 1);
    public Vector2I WaterAtlas { get; private set; } = new Vector2I(0, 2);
    public Vector2I TreeAtlas { get; private set; } = new Vector2I(13, 3);
}
