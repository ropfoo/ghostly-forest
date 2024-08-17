using Godot;

public partial class ManaTree : Area2D
{
	private Timer timer;
	private ProgressBar healthBar;
	private AnimatedSprite2D sprite;
	private PointLight2D ghostLight;

	private bool isSelected = false;
	private bool isMouseEnter = false;

	private GhostContainer ghostContainer;
	public float Health = 0.1f;

	private Main main;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		healthBar = GetNode<ProgressBar>("ProgressBar");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		ghostLight = GetNode<PointLight2D>("GhostLight");
		main = GetNode<Main>("../../../Main");

		// Flip the sprite horizontally randomly
		sprite.FlipH = GD.Randi() % 2 == 1;

		ghostLight.Visible = false;
		ghostContainer = new GhostContainer(ghostLight);

		healthBar.Value = Health;
		PlayAnimation();

		timer.Timeout += OnTimerTimeout;
		timer.Start();
	}

	public override void _Process(double delta)
	{
		// Process code here, if needed
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustReleased("click"))
		{
			isSelected = isMouseEnter;
		}

		if (Input.IsActionJustReleased("right_click") && isSelected)
		{
			ghostContainer.ReleaseGhost(GetGlobalMousePosition());
		}
	}

	public override void _MouseEnter()
	{
		isMouseEnter = true;
	}

	public override void _MouseExit()
	{
		isMouseEnter = false;
	}

	private void OnTimerTimeout()
	{
		Heal(0.1f);
	}

	public bool AcceptGhost(Ghost incomingGhost)
	{
		return ghostContainer.SetGhost(incomingGhost);
	}

	public bool TakeDamage(float damage)
	{
		GD.Print("take some damage: ", damage);
		if (Health <= 0)
		{
			QueueFree();
			return false;
		}
		Health -= damage;
		healthBar.Value = Health;
		PlayAnimation();
		return true;
	}

	private void Heal(float amount)
	{
		if (ghostContainer.Ghost == null) return;

		main.AddToManaCount(1);
		Health += amount;
		healthBar.Value = Health;
		PlayAnimation();
	}

	private void PlayAnimation()
	{
		if (Health <= 0.1f)
		{
			sprite.Animation = "no_health";
			return;
		}

		if (Health >= 0.9f)
		{
			sprite.Animation = "full_health";
			return;
		}
	}
}
