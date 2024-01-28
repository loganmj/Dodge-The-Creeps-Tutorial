using Godot;
using System;
using System.Diagnostics;

namespace DodgeTheCreeps
{
    /// <summary>
    /// Defines code for an enemy mob.
    /// </summary>
    public partial class Mob : RigidBody2D
    {
        #region Fields

        private String[] _mobTypes = { "walk", "swim", "fly" };
        private readonly Random _random = new();

        #endregion

        #region Properties

        /// <summary>
        /// Minimum speed range
        /// </summary>
        [Export]
        public int MinSpeed = 150;

        /// <summary>
        /// Maximum speed range
        /// </summary>
        [Export]
        public int MaxSpeed = 250;

        #endregion

        #region Event Handlers

        /// <summary>
        /// De-spawns the mob when it leaves the screen.
        /// </summary>
        public void OnVisibilityScreenExited()
        {
            // DEBUG
            Debug.WriteLine($"Mob {this} has exited the scene!");

            QueueFree();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the node enters the scene tree for the first time.
        /// </summary>
        public override void _Ready()
        {
            // DEBUG
            Debug.WriteLine($"Mob {this} has entered the scene!");
            var thisNode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            
            thisNode.Animation = _mobTypes[_random.Next(0, _mobTypes.Length)];
            thisNode.Play();
        }

        /// <summary>
        /// Called every frame. 'delta' is the elapsed time since the previous frame.
        /// </summary>
        /// <param name="delta"></param>
        public override void _Process(double delta)
        {
        }

        /// <summary>
        /// Releases the mob object
        /// </summary>
        public void OnStartGame()
        {
            QueueFree();
        }

        #endregion
    }
}
