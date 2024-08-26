using Godot;

public partial class LumberJack : Moveable
{
	private Map map;
	private Timer attackTimer;
	private Timer idleTimer;
	private Area2D target;
	public enum TaskType
	{
		Walk,
		PickRandomDestination,
		MoveToTree,
		ChopTree,
		Attack
	}

	public TaskType Task = TaskType.Walk;

	public override void _Ready()
	{
		map = GetNode<Map>("../Map");
		Sprite = GetNode<AnimatedSprite2D>("LumberJackSprite");

		attackTimer = GetNode<Timer>("AttackTimer");
		attackTimer.Timeout += Attack;

		idleTimer = GetNode<Timer>("IdleTimer");
		idleTimer.Timeout += LookAround;
		idleTimer.Start();

		Vision = new Vector2(150, 150);
	}

	public override void _Process(double delta)
	{
		// check if there is a nearby mana tree
		if (Task == TaskType.Walk)
		{
			var manaTreePosition = FindManaTree();
			if (manaTreePosition.HasValue)
			{
				GD.Print("found a tree AT ", manaTreePosition.Value);
				Destination = manaTreePosition.Value;
				Task = TaskType.MoveToTree;
			}
		}

		if (Destination != Vector2.Zero)
		{
			HasArrived = MoveToDestination(delta, acceptableDistance: 5);
			// Task = TaskType.Attack;
		}
	}

	private void LookAround()
	{
		if (Task == TaskType.Walk)
		{
			MoveToRandomPlaceInVision();
		}
	}

	private Vector2? FindManaTree()
	{
		GD.Print("searching tree....");
		foreach (var child in map.GetChildren())
		{
			if (child is ManaTree manaTree)
			{
				if (manaTree.Health > 0 && IsInVision(manaTree.Position))
				{
					GD.Print(manaTree.Health, manaTree.Position, Position);
					return manaTree.Position;
				}
			}
		}
		return null;
	}

	private void Attack()
	{
		if (Task != TaskType.Attack) return;
		if (target.HasMethod("TakeDamage") && IsNearDestination())
		{
			GD.Print("ATTACK");
			var isTargetStillAlive = (bool)target.Call("TakeDamage", 0.2f);
			if (!isTargetStillAlive)
			{
				Task = TaskType.Walk;
				Destination = Vector2.Zero;
				attackTimer.Stop();
			}
		}
	}

	private void _on_area_entered(Area2D area)
	{
		if (area.HasMethod("TakeDamage") && IsNearDestination())
		{
			Task = TaskType.Attack;
			target = area;
			attackTimer.Start();
		}
	}
}


