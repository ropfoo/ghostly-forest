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

    public bool IsInVision(Vector2 obstacle) =>
            Math.Abs(Position.X - obstacle.X) < Vision.X &&
            Math.Abs(Position.Y - obstacle.Y) < Vision.Y;

    public bool IsNearDestination() => Destination.DistanceTo(Position) <= 64;

    public void MoveToRandomPlaceInVision()
    {
        Random random = new Random();
        int randomX = random.Next((int)(Position.X - Vision.X), (int)(Position.X + Vision.X));
        int randomY = random.Next((int)(Position.Y - Vision.Y), (int)(Position.Y + Vision.Y));
        Destination = new Vector2(randomX, randomY);
    }
}