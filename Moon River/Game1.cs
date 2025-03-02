using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Moon_River
{
    enum GameState
    {
        Menu,
        Explore,
        Dialogue,
        Building
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState gameState;

        // bg
        private Texture2D moonriverBG;
        private Vector2 screenPos;
        private Vector2 worldPos;

        // player
        private Texture2D[] playerAnim;
        private Player player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            worldPos = new Vector2();
            gameState = GameState.Menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // bg
            moonriverBG = this.Content.Load<Texture2D>("moonriverbg");
            screenPos = new Vector2(-moonriverBG.Width / 3 + 50, -moonriverBG.Height / 3 - 200);

            // player
            playerAnim = new Texture2D[3];
            for (int i = 0; i < 3; i++)
            {
                playerAnim[i] = this.Content.Load<Texture2D>($"player{i + 1}");
            }
            Rectangle playerLoc = new Rectangle(
                _graphics.PreferredBackBufferWidth / 2 - 25, 
                _graphics.PreferredBackBufferHeight / 2 - 25,
                50, 
                50);
            player = new Player(playerLoc, playerAnim);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState kb = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.Menu:
                    // transition
                    if (kb.IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Explore;
                    }
                    break;

                case GameState.Explore:
                    // transition for dialogue & building
                    
                    // happenings
                    player.Walking = false;

                    if (kb.IsKeyDown(Keys.Up) && worldPos.Y > screenPos.X - _graphics.PreferredBackBufferHeight/2) // idk why this works but it does so DONT TOUCH IT
                    {
                        worldPos.Y -= 2;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Down) && worldPos.Y < moonriverBG.Height/2 - 200)
                    {
                        worldPos.Y += 2;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Right) && worldPos.X < moonriverBG.Width/2 - 260)
                    {
                        worldPos.X += 2;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Left) && worldPos.X > screenPos.X + 20)
                    {
                        worldPos.X -= 2;
                        player.Walking = true;
                    }

                    player.Update(gameTime); 
                    break;

                case GameState.Dialogue:
                    break;

                case GameState.Building:
                    break;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Vector2 worldToScreen = screenPos - worldPos;

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.Menu:
                    break;

                case GameState.Explore:
                    // map
                    _spriteBatch.Draw(
                        moonriverBG,
                        new Rectangle((int)worldToScreen.X, (int)worldToScreen.Y, moonriverBG.Width, moonriverBG.Height),
                        Color.White);
                    // player
                    player.Draw(_spriteBatch); 
                    break;

                case GameState.Dialogue:
                    break;

                case GameState.Building:
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
