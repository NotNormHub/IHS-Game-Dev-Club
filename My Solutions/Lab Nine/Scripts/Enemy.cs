using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	private const float Speed = 80f;
	private NavigationAgent2D _nav;

	private Player _player;
	private AnimatedSprite2D _sprite;
	private bool _isHurt;
	private int hp = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_player = GetNode<Player>("%Player");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _PhysicsProcess(double delta)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		_nav.TargetPosition = _player.Position;
		if (hp < 1)
			return;
		
		Vector2 velocity;

		if (_isHurt)
			velocity = Vector2.Zero;
		else
			velocity = ToLocal(_nav.GetNextPathPosition()).Normalized() * Speed;
		_nav.Velocity = velocity;
		
		if (GetSlideCollisionCount() == 0)
			return;
		
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			GodotObject k = GetSlideCollision(i).GetCollider();
			if (GetSlideCollision(i).GetCollider().Equals(_player))
			{
				_player.Attack();
				return;
			}
		}
	}

	private void VelocityComputed(Vector2 safeVelocity)
	{
		if (hp < 1)
			return;
		Velocity = safeVelocity;
		MoveAndSlide();
	}

	public void Attack()
	{
		if (_isHurt)
			return;
		
		_isHurt = true;
		hp--;
		_nav.Velocity = Vector2.Zero;
		if (hp < 1)
		{
			_sprite.Animation = "Dying";
			_nav.MaxSpeed = 0;
		}
		else
		{
			_sprite.Animation = "Hurt";
			Tween tween = CreateTween();
			tween.TweenCallback(Callable.From(EndHurt)).SetDelay(1);
			tween.Play();
		}
	}

	public void EndHurt()
	{
		_isHurt = false;
		_sprite.Animation = "Taunt";
	}

	public bool isDead()
	{
		return hp < 1;
	}
}
