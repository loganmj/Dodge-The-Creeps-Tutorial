using Godot;

namespace DodgeTheCreeps
{
    /// <summary>
    /// Defines code for the player.
    /// </summary>
    public partial class Player : Area2D
    {
        #region Fields

        // The size of the game window
        private Vector2 _screenSize;

        #endregion

        #region Properties

        // How fast the player will move (pixels/sec)
        [Export]
        public int Speed = 400;

        #endregion

        #region Delegates

        [Signal]
        public delegate void HitEventHandler();

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles a collision with an enemy.
        /// </summary>
        /// <param name="body"></param>
        public void OnPlayerBodyEntered(PhysicsBody2D body)
        {
            Hide(); // Player disappears after being hit.
            EmitSignal("Hit");
            GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the player sprite when starting a new game.
        /// </summary>
        /// <param name="pos"></param>
        public void Start(Vector2 pos)
        {
            Position = pos;
            Show();
            GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
        }

        /// <summary>
        /// Called when the node enters the scene tree for the first time.
        /// </summary>
        public override void _Ready()
        {
            _screenSize = GetViewportRect().Size;
            // Hide();
        }

        /// <summary>
        /// Called every frame. 'delta' is the elapsed time since the previous frame.
        /// </summary>
        /// <param name="delta"></param>
        public override void _Process(double delta)
        {
            // The player's movement vector
            Vector2 velocity = new();

            if (Input.IsActionPressed("ui_right"))
            {
                velocity.X += 1;
            }

            if (Input.IsActionPressed("ui_left"))
            {
                velocity.X -= 1;
            }

            if (Input.IsActionPressed("ui_down"))
            {
                velocity.Y += 1;
            }

            if (Input.IsActionPressed("ui_up"))
            {
                velocity.Y -= 1;
            }

            // TODO: Play the appropriate animation
            AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

            if (velocity.Length() > 0)
            {
                velocity = velocity.Normalized() * (float)(Speed * delta);
                animatedSprite.Play();
            }
            else
            {
                animatedSprite.Stop();
            }

            if (velocity.X != 0)
            {
                animatedSprite.Animation = "right";
                // See the note below about boolean assignment
                animatedSprite.FlipH = velocity.X < 0;
                animatedSprite.FlipV = false;
            }
            else if (velocity.Y != 0)
            {
                animatedSprite.Animation = "up";
                animatedSprite.FlipV = velocity.Y > 0;
            }

            Position += velocity;
            Position = new Vector2(
                x: Mathf.Clamp(Position.X, 0, _screenSize.X),
                y: Mathf.Clamp(Position.Y, 0, _screenSize.Y)
            );
        }

        #endregion
    }
}
