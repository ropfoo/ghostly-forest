using System.Runtime;
using Godot;

public partial class LumberJack : Moveable
{
	private Map map;
	private Timer attackTimer;
	private Area2D target;
	public enum TaskType
	{
		LookAround,
		Attack
	}

	public TaskType Task = TaskType.LookAround;

	public override void _Ready()
	{
		map = GetNode<Map>("../Map");
		Sprite = GetNode<AnimatedSprite2D>("LumberJackSprite");
		attackTimer = GetNode<Timer>("AttackTimer");

		attackTimer.Timeout += Attack;
		attackTimer.Start();
	}

	public override void _Process(double delta)
	{
		if (Destination == Vector2.Zero && Task == TaskType.LookAround)
		{
			FindManaTree();
		}

		if (Destination != Vector2.Zero)
		{
			HasArrived = MoveToDestination(delta, acceptableDistance: 20);
		}
	}

	private void FindManaTree()
	{
		foreach (var child in map.GetChildren())
		{
			if (child is ManaTree manaTree)
			{
				if (manaTree.Health > 0.2 && IsInVision(manaTree.Position))
				{
					GD.Print(manaTree.Health, manaTree.Position, Position);
					Destination = manaTree.Position;
					break;
				}
			}
		}

	}

	private void OnTimerTimeout()
	{
		// Heal(0.1f);
	}

	private void Attack()
	{
		if (Task != TaskType.Attack) return;
		if (target.HasMethod("TakeDamage") && IsNearDestination())
		{
			GD.Print("ATTACK");

			var isStillAlive = (bool)target.Call("TakeDamage", 0.2f);
			if (!isStillAlive)
			{
				Task = TaskType.LookAround;
				Destination = Vector2.Zero;

			}
		}
	}

	private void LookAround()
	{
		if (Task != TaskType.LookAround) return;

	}

	private void _on_area_entered(Area2D area)
	{
		if (area.HasMethod("TakeDamage") && IsNearDestination())
		{
			Task = TaskType.Attack;
			target = area;
			Attack();
		}
	}
}


