using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Engine.Discord
{
    public class DiscordComponent : Microsoft.Xna.Framework.GameComponent
    {
        private Discord _discord;

        public DiscordComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            if (IsDiscordRunning())
                InitializeDiscord();

            UpdateUserActivity("Playing Campaign", "Difficulty - IWTD");
            base.Initialize();
        }

        private void InitializeDiscord()
        {
            CreateFlags flags = CreateFlags.NoRequireDiscord;
            _discord = new Discord(1141624841322106921, (UInt32)flags);
        }

        private bool IsDiscordRunning()
        {
            Process[] processes = Process.GetProcessesByName("Discord");
            return processes.Length != 0;
        }

        private bool IsDiscordInitialized()
        {
            return _discord != null;
        }

        public void UpdateUserActivity(string topText = "Top text", string bottomText = "Bottom text")
        {
            if (IsDiscordRunning() && IsDiscordInitialized())
            {
                ActivityManager activityManager = _discord.GetActivityManager();

                Activity activity = new Activity
                {
                    Details = topText,
                    State = bottomText
                };
                activity.Type = ActivityType.Playing;
                activity.Assets.LargeImage = "codyface";
                activity.Assets.SmallImage = "123";
                activity.Assets.LargeText = "Yes this is cody face";
                activity.Party.Size = new PartySize() { CurrentSize = 1, MaxSize = 2 };

                activityManager.UpdateActivity(activity, (res) => { });
            }
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (IsDiscordRunning())
            {
                if (IsDiscordInitialized())
                    _discord.RunCallbacks();
                else
                    InitializeDiscord();
            }

            base.Update(gameTime);
        }
    }
}
