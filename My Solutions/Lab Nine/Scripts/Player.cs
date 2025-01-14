using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;

	private AnimatedSprite2D _sprite;
	private PackedScene _fireball;
	private Marker2D _marker;
	public bool _isAttacking;
	private bool _isHurt;
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_fireball = ResourceLoader.Load<PackedScene>("Scenes/Fireball.tscn");
		_marker = GetNode<Marker2D>("Marker2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		if (Input.IsActionJustPressed("click") && !_isAttacking && !_isHurt)
		{
			_isAttacking = true;
			Fireball fireball = (Fireball)_fireball.Instantiate();
			AddChild(fireball);
			fireball.Position = _marker.Position;
			fireball._player = this;
			fireball.ZIndex = 100;

			Tween tween = fireball.CreateTween();
			Vector2 v = GetLocalMousePosition();
			tween.TweenInterval(1);
			tween.TweenProperty(fireball, "position", GetLocalMousePosition().Normalized() * 1500, .5).AsRelative();
			tween.TweenCallback(Callable.From(() => _isAttacking = false));
			tween.TweenCallback(Callable.From(fireball.QueueFree));
			tween.Play();
		}

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();
		if (_isAttacking)
		{
			velocity.X = 0;
			velocity.Y = 0;
			if (!_isHurt)
				_sprite.Animation = "Attacking";
		}
		else if (direction != Vector2.Zero)
		{
			if (!_isHurt)
				_sprite.Animation = "Walking";

			if (direction.X > 0)
				_sprite.Scale = _sprite.Scale with { X = 1 };
			else if (direction.X < 0)
				_sprite.Scale = _sprite.Scale with { X = -1 };
			
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(from: Velocity.Y, 0, Speed);
			if (!_isHurt)
				_sprite.Animation = "Idle";
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
	
	public void Attack()
	{
		if (!_isHurt)
			_isHurt = true;
		_sprite.Animation = "Hurt";
		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(() => _isHurt = false)).SetDelay(1);
		tween.Play();
	}
	
}
