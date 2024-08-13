using Godot;

public enum UnitType
{
    Ghost
}

public interface IUnit
{
    Vector2 GetPosition();

    void Select();

    void Deselect();

    void MoveToDestination(double delta);

    bool IsNearDestination();

    UnitType GetUnitType();
}
