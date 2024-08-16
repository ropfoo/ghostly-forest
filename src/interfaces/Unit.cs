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

    bool IsNearDestination();

    UnitType GetUnitType();
}
