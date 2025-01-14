using Godot;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	
	const string SAVE_DIR = "user://saves/";
	const string SAVE_FILE = "save.json";
	const string KEY = "password";

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void SavePressed()
	{
		DirAccess.MakeDirAbsolute(SAVE_DIR);
		using var file = FileAccess.OpenEncryptedWithPass(SAVE_DIR + SAVE_FILE, FileAccess.ModeFlags.Write, KEY);
		var data = new Godot.Collections.Dictionary<string, Variant>
		{
			{"x", Position.X},
			{"y", Position.Y}
		};

		string json = Json.Stringify(data, "\t");
		file.StoreLine(json);
		file.Close();
	}

	public void LoadPressed()
	{
		using var saveGame = FileAccess.OpenEncryptedWithPass(SAVE_DIR + SAVE_FILE, FileAccess.ModeFlags.Read, KEY);
		var json = new Json();
		var nodeData = new Godot.Collections.Dictionary<string, Variant>();
		string jsonString = saveGame.GetAsText();
		json.Parse(jsonString);
		nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
		Set(Node2D.PropertyName.Position, new Vector2((float)nodeData["x"], (float)nodeData["y"]));
	}
}
