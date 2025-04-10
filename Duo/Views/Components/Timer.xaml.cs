using System;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Duo.Views.Components
{
    /// <summary>
    /// Timer component for displaying elapsed time in various app features.
    /// </summary>
    public sealed partial class Timer : UserControl
    {
        // Core timer components
        private DispatcherTimer timer;     // UI updates
        private Stopwatch stopwatch;       // Time tracking
        private bool isRunning;            // Current state

        /// <summary>
        /// Fires on each timer update with current elapsed time.
        /// </summary>
        public event EventHandler<TimeSpan> TimerTick;

        /// <summary>
        /// Initializes timer control with default settings.
        /// </summary>
        public Timer()
        {
            this.InitializeComponent();

            stopwatch = new Stopwatch();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            isRunning = false;
        }

        /// <summary>
        /// Updates display and notifies subscribers on each tick.
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            UpdateTime();
            TimerTick?.Invoke(this, stopwatch.Elapsed);
        }

        /// <summary>
        /// Starts the timer if not already running.
        /// </summary>
        public void Start()
        {
            if (!isRunning)
            {
                stopwatch.Start();  // Start timing
                timer.Start();      // Start UI updates
                isRunning = true;
            }
        }

        /// <summary>
        /// Pauses the timer if currently running.
        /// </summary>
        public void Stop()
        {
            if (isRunning)
            {
                stopwatch.Stop();  // Pause timing
                timer.Stop();      // Stop UI updates
                isRunning = false;
            }
        }

        /// <summary>
        /// Resets timer to zero.
        /// </summary>
        public void Reset()
        {
            Stop();
            stopwatch.Reset();
            UpdateTime();
        }

        /// <summary>
        /// Gets current elapsed time.
        /// </summary>
        public TimeSpan GetElapsedTime()
        {
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// Updates display with formatted time (MM:SS).
        /// </summary>
        private void UpdateTime()
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            string formattedTime = string.Format("{0:00}:{1:00}",
                elapsed.Minutes,
                elapsed.Seconds);
            TimerDisplay.Text = formattedTime;
        }
    }
}