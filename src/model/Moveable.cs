using System;
using Godot;

public partial class Moveable : Area2D
{
    [Export]
    public int Speed { get; set; } = 100;
    public Vector2 Destination = Vector2.Zero;
    public AnimatedSprite2D Sprite;
    public bool HasArrived;
    public Vector2 Vision = new Vector2(300, 300);

    public bool MoveToDestination(double delta, double acceptableDistance = 5)
    {
        if (Position.DistanceTo(Destination) < acceptableDistance) return true;

        Vector2 target = (Destination - Position).Normalized();
        Vector2 velocity = target * Speed;
        Sprite.FlipH = velocity.X < 0;
        Position += velocity * (float)delta;
        return false;
    }

    public bool IsInVision(Vector2 obstacle)
    {
        return
            Math.Abs(Position.X - obstacle.X) < Vision.X &&
            Math.Abs(Position.Y - obstacle.Y) < Vision.Y;
    }


    public bool IsNearDestination()
    {
        return Destination.DistanceTo(Position) <= 64;
    }
}