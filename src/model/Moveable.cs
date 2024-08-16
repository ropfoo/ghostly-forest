using Godot;

public partial class Moveable : Area2D
{
    [Export]
    public int Speed { get; set; } = 100;
    public Vector2 Destination = Vector2.Zero;
    public AnimatedSprite2D Sprite;
    public bool HasArrived;

    public bool MoveToDestination(double delta, double acceptableDistance = 5)
    {
        if (Position.DistanceTo(Destination) < acceptableDistance) return true;

        Vector2 target = (Destination - Position).Normalized();
        Vector2 velocity = target * Speed;
        Sprite.FlipH = velocity.X < 0;
        Position += velocity * (float)delta;
        return false;
    }
}