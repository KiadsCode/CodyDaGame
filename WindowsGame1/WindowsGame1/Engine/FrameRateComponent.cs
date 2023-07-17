using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Engine
{
    public class FrameRateComponent : GameComponent
    {
        public const int MaximumSamples = 100;

        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }
        private Queue<float> _sampleBuffer = new Queue<float>();

        public FrameRateComponent(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            Enabled = false;
            base.Initialize();
        }

        public override string ToString()
        {
            return string.Format("FrameRate: {0}", AverageFramesPerSecond);
        }

        public override void Update(GameTime gameTime)
        {
            CurrentFramesPerSecond = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MaximumSamples)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }
    }
}