using Godot;
using System;

namespace DodgeTheCreeps
{
    /// <summary>
    /// Defines code for the HUD.
    /// </summary>
    public partial class HUD : CanvasLayer
    {
        [Signal]
        public delegate void StartGameEventHandler();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        /// <summary>
        /// Displays a message temporarily
        /// </summary>
        /// <param name="text"></param>
        public void ShowMessage(string text)
        {
            var messageLabel = GetNode<Label>("MessageLabel");
            messageLabel.Text = text;
            messageLabel.Show();

            GetNode<Timer>("MessageTimer").Start();
        }

        /// <summary>
        /// Displays a "Game Over" message
        /// </summary>
        async public void ShowGameOver()
        {
            ShowMessage("Game Over");

            var messageTimer = GetNode<Timer>("MessageTimer");
            await ToSignal(messageTimer, "timeout");

            var messageLabel = GetNode<Label>("MessageLabel");
            messageLabel.Text = "Dodge the\nCreeps!";
            messageLabel.Show();

            GetNode<Button>("StartButton").Show();
        }

        /// <summary>
        /// Updates the score
        /// </summary>
        /// <param name="score"></param>
        public void UpdateScore(int score)
        {
            GetNode<Label>("ScoreLabel").Text = score.ToString();
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void OnStartButtonPressed()
        {
            // DEBUG
            Console.WriteLine($"Start button pressed!");

            GetNode<Button>("StartButton").Hide();
            EmitSignal("StartGame");
        }

        /// <summary>
        /// Hides the message label
        /// </summary>
        public void OnMessageTimerTimeout()
        {
            GetNode<Label>("MessageLabel").Hide();
        }
    }
}
