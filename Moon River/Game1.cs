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
        private KeyboardState prevkb;

        // title screen
        private Texture2D titleScreen;

        // bg
        private Texture2D moonriverBG;
        private Vector2 screenPos;
        private Vector2 worldPos;

        // player
        private Texture2D[] playerAnim;
        private Player player;

        // buildings
        private Building[] buildings;
        private Building currentBuilding;
        private Texture2D placeholder;

        // NPC
        private NPC[] NPCs;
        private NPC currentNPC;

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
            titleScreen = this.Content.Load<Texture2D>("TitleScreen");
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

            // buildings
            buildings = new Building[6]; 
            placeholder = this.Content.Load<Texture2D>("blacksquare");
            buildings[0] = new Building(new Rectangle(320,170,80,80), placeholder); // mushroom
            buildings[1] = new Building(new Rectangle(-460, -700, 80, 80), placeholder); // star
            buildings[2] = new Building(new Rectangle(1180, -160, 80, 80), placeholder); // heart
            buildings[3] = new Building(new Rectangle(90, 1440, 80, 80), placeholder); // flower
            buildings[4] = new Building(new Rectangle(700, 820, 80, 80), placeholder); // pen
            buildings[5] = new Building(new Rectangle(1530, 1020, 80, 80), placeholder); // moon

            // NPCs
            NPCs = new NPC[1]; 
            NPCs[0] = new NPC(new Rectangle(0,0,50,50), placeholder, "");
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
                    if (kb.IsKeyDown(Keys.Enter) && prevkb.IsKeyUp(Keys.Enter))
                    {
                        gameState = GameState.Explore;
                    }
                    break;

                case GameState.Explore:
                    // transition for dialogue & building
                    for (int i = 0; i < buildings.Length; i++)
                    {
                        if (buildings[i].CanEnterBuilding(player, worldPos) && kb.IsKeyDown(Keys.Enter) && prevkb.IsKeyUp(Keys.Enter))
                        {
                            currentBuilding = buildings[i];
                            currentBuilding.Occupied = true;
                            gameState = GameState.Building;
                        }
                    }
                    for (int i = 0; i < NPCs.Length; i++)
                    {
                        if (NPCs[i].CanTalk(player, worldPos) && kb.IsKeyDown(Keys.Enter) && prevkb.IsKeyUp(Keys.Enter))
                        {
                            currentNPC = NPCs[i];
                            gameState = GameState.Dialogue;
                        }
                    }

                    // happenings
                    player.Walking = false;

                    if (kb.IsKeyDown(Keys.Up) && worldPos.Y > screenPos.X - _graphics.PreferredBackBufferHeight/2) // idk why this works but it does so DONT TOUCH IT
                    {
                        worldPos.Y -= 20;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Down) && worldPos.Y < moonriverBG.Height/2 - 200)
                    {
                        worldPos.Y += 20;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Right) && worldPos.X < moonriverBG.Width/2 - 260)
                    {
                        worldPos.X += 20;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Left) && worldPos.X > screenPos.X + 20)
                    {
                        worldPos.X -= 20;
                        player.Walking = true;
                    }

                    player.Update(gameTime);
                    break;

                case GameState.Dialogue:
                    if ((kb.IsKeyDown(Keys.Q) && prevkb.IsKeyUp(Keys.Q)) || currentNPC.CurrentLine > currentNPC.Script.Count)
                    {
                        // return to building if in building
                        if (currentBuilding != null && currentBuilding.Occupied == true)
                        {
                            gameState = GameState.Building;
                        }
                        else
                        {
                            gameState = GameState.Explore;
                        }
                    }
                    break;

                case GameState.Building:
                    if (kb.IsKeyDown(Keys.Q) && prevkb.IsKeyUp(Keys.Q))
                    {
                        currentBuilding.Occupied = false;
                        gameState = GameState.Explore;
                    }
                    break;
            }
            prevkb = kb;
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
                    //title screen
                    _spriteBatch.Draw(
                        titleScreen,
                        new Rectangle(0, 0,titleScreen.Width, titleScreen.Height),
                        Color.White);
                    break;

                case GameState.Explore:
                    // building
                    for (int i = 0; i < buildings.Length; i++)
                    {
                        _spriteBatch.Draw(
                            placeholder,
                            buildings[i].Reposition(worldPos),
                            Color.White);
                    }
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
