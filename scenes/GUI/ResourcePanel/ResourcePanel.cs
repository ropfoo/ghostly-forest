using Godot;

public partial class ResourcePanel : Node
{
	private Label manaCountLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		manaCountLabel = GetNode<Label>("Layout/Grid/ManaCount");
	}

	public void SetManaCount(double newManaCount)
	{
		manaCountLabel.Text = newManaCount.ToString();
	}
}
