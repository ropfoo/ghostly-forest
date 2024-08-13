using Godot;

public partial class GhostContainer : Node
{
    public Ghost Ghost { get; private set; }
    private PointLight2D ghostLight;

    public GhostContainer(PointLight2D light)
    {
        ghostLight = light;
    }

    public bool SetGhost(Ghost incomingGhost)
    {
        if (Ghost == null)
        {
            ghostLight.Visible = true;
            Ghost = incomingGhost;
            return true;
        }
        return false;
    }

    public void ReleaseGhost(Vector2 position)
    {
        if (Ghost != null)
        {
            Ghost.Show();
            Ghost.Destination = position;
            Ghost.IsSelected = true;
            ghostLight.Visible = false;
            Ghost = null;
        }
    }
}
