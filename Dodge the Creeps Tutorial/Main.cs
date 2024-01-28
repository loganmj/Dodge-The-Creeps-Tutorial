using Godot;
using System;
using System.Diagnostics;
using System.Linq;

namespace DodgeTheCreeps
{
    /// <summary>
    /// Describes the main scene of the game.
    /// </summary>
    public partial class Main : Node2D
    {
        #region Fields

        private int _score;

        /// <summary>
        /// We use 'System.Random' as an alternative to GDScript's random methods.
        /// </summary>
        private readonly Random _random = new();

        #endregion

        #region Properties

        [Export]
        public PackedScene Mob;

        #endregion

        #region Methods

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        /// <summary>
        /// We'll use this later because C# doesn't support GDScript's randi().
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private float RandRange(float min, float max)
        {
            return (float)_random.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Cleans up the game resources when the game is over.
        /// </summary>
        public void GameOver()
        {
            GetNode<Timer>("MobTimer").Stop();
            GetNode<Timer>("ScoreTimer").Stop();

            // Update the HUD
            GetNode<HUD>("HUD").ShowGameOver();

            // Remove all mobs
            Node[] mobs = GetChildren().Where(x => x is Mob).ToArray();
            foreach (Node mob in mobs)
            {
                RemoveChild(mob);
            }
        }

        /// <summary>
        /// Sets up the game resources when the game begins.
        /// </summary>
        public void NewGame()
        {
            _score = 0;

            Player player = GetNode<Player>("Player");
            player.Start(new Vector2(240, 450));

            // Update the HUD
            var hud = GetNode<HUD>("HUD");
            hud.UpdateScore(_score);
            hud.ShowMessage("Get Ready!");

            GetNode<Timer>("StartTimer").Start();
        }

        /// <summary>
        /// Starts the game in response to the start timer.
        /// </summary>
        public void OnStartTimerTimeout()
        {
            GetNode<Timer>("MobTimer").Start();
            GetNode<Timer>("ScoreTimer").Start();
        }

        /// <summary>
        /// Incrementes the score counter when the score timer times out.
        /// </summary>
        public void OnScoreTimerTimeout()
        {
            _score++;

            // Update the HUD
            GetNode<HUD>("HUD").UpdateScore(_score);
        }

        /// <summary>
        /// Spawns a mob at a random starting location.
        /// </summary>
        public void OnMobTimerTimeout()
        {
            // DEBUG
            Debug.WriteLine($"Mob timer expired!");

            // Choose a random location along the mob spawn path
            PathFollow2D mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
            mobSpawnLocation.ProgressRatio = _random.Next();

            // Create a Mob instance and add it to the scene.
            Mob mobInstance = (Mob)Mob.Instantiate();
            AddChild(mobInstance);
            mobInstance.Visible = true;

            // DEBUG
            Debug.WriteLine($"Created new Mob instance:\n" +
                $"- Object ID: {mobInstance}\n" +
                $"- Path: {mobInstance.GetPath()}\n");

            // Set the mob's direction perpendicular to the path direction.
            float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

            // Set the mob's position to a random location.
            mobInstance.Position = mobSpawnLocation.Position;

            // Add some randomness to the direction.
            direction += RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
            mobInstance.Rotation = direction;

            // Choose the velocity.
            mobInstance.LinearVelocity = new Vector2(RandRange(150f, 250f), 0).Rotated(direction);

            // DEBUG
            Debug.WriteLine($"Setting up the mob's spawn data:\n" +
                $"- Location: {mobInstance.Position}\n" +
                $"- Rotation: {mobInstance.Rotation}\n" +
                $"- Velocity: {mobInstance.LinearVelocity}\n");
        }

        #endregion
    }
}
