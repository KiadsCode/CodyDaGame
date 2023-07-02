using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SparrowV2;
using WindowsGame1.Engine;

namespace WindowsGame1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, SpriteSheet> SpriteSheets;
        public static Dictionary<string, SoundEffect> SoundEffects;
        public static Dictionary<string, SpriteFont> SpriteFonts;
        public static Dictionary<string, Effect> Shaders;
        public static Player Player;
        public static UserInterface UserInterface;
        public static EnemyContainer EnemyContainer;
        public static Camera GamePlayCamera = new Camera();
        public const int GlobalAnimationCoolDown = 2;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Random _random = new Random();
        private string[] _titles = 
        {
            "FireTeam Osiris. Light is green",
            "Betrayed",
            "Used zero budget"
        };

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Textures = new Dictionary<string, Texture2D>();
            SpriteSheets = new Dictionary<string, SpriteSheet>();
            SoundEffects = new Dictionary<string, SoundEffect>();
            SpriteFonts = new Dictionary<string, SpriteFont>();
            Shaders = new Dictionary<string, Effect>();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
#if !PROTOTYPE
            InitiateTitle();
#endif
            base.Initialize();
        }

        private void InitiateTitle()
        {
            int selectedTitle = _random.Next(0, _titles.Length);
            Window.Title = string.Format("Ñody Game: {0}", _titles[selectedTitle]);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Textures.Add("uiHealth", Content.Load<Texture2D>(@"images\\uiHealth"));
            Textures.Add("uiAim", Content.Load<Texture2D>(@"images\\uiAim"));
            Textures.Add("playerHUD", Content.Load<Texture2D>(@"images\\playerHUD"));
            Textures.Add("missionTextUI", Content.Load<Texture2D>(@"images\\missionTextUI"));
            Textures.Add("fade", Content.Load<Texture2D>(@"images\\fade"));
            Textures.Add(Player.AssetsName, Content.Load<Texture2D>(@"images\\codyGamePlay"));
            Textures.Add("pistolShot", Content.Load<Texture2D>(@"images\\PistolShoot"));
            Textures.Add("menuBG", Content.Load<Texture2D>(@"images\\menuBG"));
            Textures.Add("1x1", Content.Load<Texture2D>(@"images\\1x1"));
            Textures.Add("codyMenu", Content.Load<Texture2D>(@"images\\codyMenu"));
            Textures.Add("enemy", Content.Load<Texture2D>(@"images\\enemy"));
            Textures.Add("arShot", Content.Load<Texture2D>(@"images\\AssaultRifleShoot"));
            Textures.Add("explosion", Content.Load<Texture2D>(@"images\\explosion"));

            SpriteFonts.Add("hudfont", Content.Load<SpriteFont>(@"fonts\\hudFont"));
            SpriteFonts.Add("vcr", Content.Load<SpriteFont>(@"fonts\\vcr"));

            SoundEffects.Add("magnum", Content.Load<SoundEffect>(@"sounds\\magnumShot"));
            SoundEffects.Add("weaponNoAmmo", Content.Load<SoundEffect>(@"sounds\\weaponNoAmmo"));
            SoundEffects.Add("arShotC", Content.Load<SoundEffect>(@"sounds\\arShotC"));
            SoundEffects.Add("killsoundA", Content.Load<SoundEffect>(@"sounds\\killsoundA"));
            SoundEffects.Add("killsoundB", Content.Load<SoundEffect>(@"sounds\\killsoundB"));

            SpriteSheets.Add(Player.AssetsName, SpriteSheetImporter.Import(Content, "images\\xml\\cody"));
            SpriteSheets.Add("arShot", SpriteSheetImporter.Import(Content, "images\\xml\\AssaultRifleShoot"));
            SpriteSheets.Add("pistolShot", SpriteSheetImporter.Import(Content, "images\\xml\\PistolShoot"));
            SpriteSheets.Add("codyMenu", SpriteSheetImporter.Import(Content, "images\\xml\\CodyMenu"));

            Shaders.Add("mono", Content.Load<Effect>(@"shaders\\monochrome"));
            Shaders.Add("invert", Content.Load<Effect>(@"shaders\\invert"));
            Shaders.Add("blur", Content.Load<Effect>(@"shaders\\blur"));

            ComponentsInitialize();

            for (int i = 1; i < 15; i++)
                for (int j = 0; j < 5; j++)
                    EnemyContainer.Add(new Vector2(i * 50, j * 52));
        }

        private void ComponentsInitialize()
        {
            Player = new Player(this);
            Components.Add(Player);

            EnemyContainer = new EnemyContainer(this);
            Components.Add(EnemyContainer);

            UserInterface = new UserInterface(this);
            Components.Add(UserInterface);

            foreach (IGameComponent item in Components)
                item.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
