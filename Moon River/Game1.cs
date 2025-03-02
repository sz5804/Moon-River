using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;

namespace Moon_River
{
    enum GameState
    {
        Menu,
        Explore,
        Dialogue,
        Building
    }
    enum Symbols
    {
        Mushroom = 0,
        Star = 1,
        Heart = 2,
        Flower = 3,
        Pen = 4, 
        Moon = 5
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState gameState;
        private KeyboardState prevkb;
        private SpriteFont coolfont;

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
        private Texture2D[] buildingBG;
        private int currentBuilding;
        private Texture2D placeholder;

        // NPC
        private NPC[] NPCs;
        private NPC[] NPCsPT2;
        private Texture2D[] npcTextures;
        private Texture2D[] npcFullTextures;
        private int currentNPC;

        // quest tracking
        private Symbols progress = Symbols.Mushroom;
        private bool scene2 = false;
        private bool complete = false;
        private Texture2D questMarker;
        private NPC end;

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

            // menu
            titleScreen = this.Content.Load<Texture2D>("MoonRiver Title Screen");

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

            buildingBG = new Texture2D[6];
            buildingBG[0] = this.Content.Load<Texture2D>("Mushroom House");
            buildingBG[1] = this.Content.Load<Texture2D>("Star House");
            buildingBG[2] = this.Content.Load<Texture2D>("Heart Home");
            buildingBG[3] = this.Content.Load<Texture2D>("Flower House");
            buildingBG[4] = this.Content.Load<Texture2D>("Pen House");
            buildingBG[5] = this.Content.Load<Texture2D>("Moon House");

            // NPCs
            NPCs = new NPC[6];
            Texture2D mushroomNPC = this.Content.Load<Texture2D>("Mom");
            NPCs[0] = new NPC(new Rectangle(200, 230, 80, 80), mushroomNPC, "../../../mushroom.txt");
            Texture2D starNPC = this.Content.Load<Texture2D>("Star");
            NPCs[1] = new NPC(new Rectangle(400, -50, 170, 170), starNPC, "../../../star.txt"); 
            Texture2D heartNPC = this.Content.Load<Texture2D>("Heart");
            NPCs[2] = new NPC(new Rectangle(400, -50, 170, 170), heartNPC, "../../../heart.txt");
            Texture2D flowerNPC = this.Content.Load<Texture2D>("Flower");
            NPCs[3] = new NPC(new Rectangle(400, -50, 170, 170), flowerNPC, "../../../flower.txt");
            Texture2D penNPC = this.Content.Load<Texture2D>("Pen");
            NPCs[4] = new NPC(new Rectangle(400, -50, 170, 170), penNPC, "../../../pen.txt");
            Texture2D moonNPC = this.Content.Load<Texture2D>("Moon");
            NPCs[5] = new NPC(new Rectangle(400, -50, 170, 170), moonNPC, "../../../moon.txt");

            NPCsPT2 = new NPC[6];
            NPCsPT2[0] = new NPC(new Rectangle(200, 230, 80, 80), mushroomNPC, "../../../mushroom2.txt");
            NPCsPT2[1] = new NPC(new Rectangle(400, -50, 170, 170), starNPC, "../../../star2.txt");
            NPCsPT2[2] = new NPC(new Rectangle(400, -50, 170, 170), heartNPC, "../../../heart2.txt");
            NPCsPT2[3] = new NPC(new Rectangle(400, -50, 170, 170), flowerNPC, "../../../flower2.txt");
            NPCsPT2[4] = new NPC(new Rectangle(400, -50, 170, 170), penNPC, "../../../pen2.txt");
            NPCsPT2[5] = new NPC(new Rectangle(400, -50, 170, 170), moonNPC, "../../../moon2.txt");

            npcTextures = new Texture2D[6];
            npcTextures[0] = mushroomNPC;
            npcTextures[1] = starNPC;
            npcTextures[2] = heartNPC;
            npcTextures[3] = flowerNPC;
            npcTextures[4] = penNPC;
            npcTextures[5] = moonNPC;

            // dialogue closeup textures
            coolfont = this.Content.Load<SpriteFont>("font");

            npcFullTextures = new Texture2D[6];
            npcFullTextures[0] = this.Content.Load<Texture2D>("Mom Full");
            npcFullTextures[1] = this.Content.Load<Texture2D>("Star Full");
            npcFullTextures[2] = this.Content.Load<Texture2D>("Heart Full");
            npcFullTextures[3] = this.Content.Load<Texture2D>("Flower Full");
            npcFullTextures[4] = this.Content.Load<Texture2D>("Pen Full");
            npcFullTextures[5] = this.Content.Load<Texture2D>("Moon Full");

            // quest marker
            questMarker = this.Content.Load<Texture2D>("Quest Marker");
            end = new NPC(new Rectangle(200, 230, 80, 80), mushroomNPC, "../../../mushroom3.txt");
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
                            currentBuilding = i;
                            buildings[currentBuilding].Occupied = true;

                            // set screenPos & worldPos back for building bg
                            screenPos = new Vector2(-buildingBG[currentBuilding].Width / 3 + 150, -buildingBG[currentBuilding].Height / 3 - 200);
                            worldPos = new Vector2();

                            // bigger player 
                            player.Location = new Rectangle(
                                _graphics.PreferredBackBufferWidth / 2 - 25,
                                _graphics.PreferredBackBufferHeight / 2 - 25,
                                100,
                                100);
                            gameState = GameState.Building;
                        }
                    }
                    for (int i = 0; i < NPCs.Length; i++)
                    {
                        if (NPCs[i].CanTalk(player, worldPos) && kb.IsKeyDown(Keys.Enter) && prevkb.IsKeyUp(Keys.Enter))
                        {
                            currentNPC = i;
                            NPCs[currentNPC].Played = false;
                            gameState = GameState.Dialogue;
                        }
                    }

                    // happenings
                    player.Walking = false;

                    if (kb.IsKeyDown(Keys.Up) && worldPos.Y > screenPos.X - _graphics.PreferredBackBufferHeight / 2) // idk why this works but it does so DONT TOUCH IT
                    {
                        worldPos.Y -= 20;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Down) && worldPos.Y < moonriverBG.Height / 2 - 200)
                    {
                        worldPos.Y += 20;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Right) && worldPos.X < moonriverBG.Width / 2 - 260)
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
                    if ((kb.IsKeyDown(Keys.Q) && prevkb.IsKeyUp(Keys.Q)) || NPCs[currentNPC].Played == true)
                    {
                        // return to building if in building
                        if (buildings[currentBuilding].Occupied == true)
                        {
                            // set screenPos & worldPos back for building bg
                            screenPos = new Vector2(
                                -buildingBG[currentBuilding].Width / 3 + 150, 
                                -buildingBG[currentBuilding].Height / 3 - 200);
                            worldPos = new Vector2();
                            gameState = GameState.Building;
                        }
                        else
                        {
                            // set screenPos & worldPos back for explore bg
                            screenPos = new Vector2(-moonriverBG.Width / 3 + 50, -moonriverBG.Height / 3 - 200);
                            worldPos = new Vector2(
                                buildings[currentBuilding].X - _graphics.PreferredBackBufferWidth / 2 + 25,
                                buildings[currentBuilding].Y - _graphics.PreferredBackBufferHeight / 2 + 50); 
                            gameState = GameState.Explore;
                        }
                        // QUEST PROGRESS TRACKING
                        switch (progress)
                        {
                            case Symbols.Mushroom:
                                if (!scene2 && currentNPC == (int)Symbols.Mushroom && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Moon;
                                else if (scene2 && currentNPC == (int)Symbols.Mushroom && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Flower;
                                else
                                {
                                    complete = true;
                                }
                                break;

                            case Symbols.Moon:
                                if (!scene2 && currentNPC == (int)Symbols.Moon && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Heart;
                                else if (scene2 && currentNPC == (int)Symbols.Moon && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Mushroom;
                                break;

                            case Symbols.Heart:
                                if (!scene2 && currentNPC == (int)Symbols.Heart && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Star;
                                else if (scene2 && currentNPC == (int)Symbols.Heart && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Moon;
                                break;

                            case Symbols.Star:
                                if (!scene2 && currentNPC == (int)Symbols.Star && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Pen;
                                else if (scene2 && currentNPC == (int)Symbols.Star && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Heart;
                                break;

                            case Symbols.Pen:
                                if (!scene2 && currentNPC == (int)Symbols.Pen && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Flower;
                                else if (scene2 && currentNPC == (int)Symbols.Pen && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Star;
                                break;

                            case Symbols.Flower:
                                if (!scene2 && currentNPC == (int)Symbols.Flower && NPCs[currentNPC].Played == true)
                                {
                                    progress = Symbols.Moon;
                                    scene2 = true;
                                    progress = Symbols.Mushroom;
                                }
                                else if (scene2 && currentNPC == (int)Symbols.Flower && NPCs[currentNPC].Played == true)
                                    progress = Symbols.Pen;
                                break;
                        }
                    }
                    NPCs[currentNPC].Update(gameTime);
                    break;

                case GameState.Building:
                    // exit transition (back to explore)
                    if (kb.IsKeyDown(Keys.Q) && prevkb.IsKeyUp(Keys.Q))
                    {
                        buildings[currentBuilding].Occupied = false;
                        // set screenPos & worldPos back for explore bg
                        screenPos = new Vector2(-moonriverBG.Width / 3 + 50, -moonriverBG.Height / 3 - 200);
                        worldPos = new Vector2(
                            buildings[currentBuilding].X - _graphics.PreferredBackBufferWidth/2 + 25, 
                            buildings[currentBuilding].Y - _graphics.PreferredBackBufferHeight/2 + 50);
                        player.Location = new Rectangle(
                                _graphics.PreferredBackBufferWidth / 2 - 25,
                                _graphics.PreferredBackBufferHeight / 2 - 25,
                                50,
                                50);
                        gameState = GameState.Explore;
                    }
                    // dialogue transition
                    if (NPCs[currentBuilding].CanTalk(player, worldPos) && kb.IsKeyDown(Keys.Enter) && prevkb.IsKeyUp(Keys.Enter))
                    {
                        currentNPC = currentBuilding;
                        NPCs[currentNPC].Played = false;
                        gameState = GameState.Dialogue;
                    }

                    // walk
                    player.Walking = false;

                    if (kb.IsKeyDown(Keys.Up) && worldPos.Y > screenPos.X - _graphics.PreferredBackBufferHeight / 2) // idk why this works but it does so DONT TOUCH IT
                    {
                        worldPos.Y -= 20;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Down) && worldPos.Y < buildingBG[currentBuilding].Height / 2 - 500)
                    {
                        worldPos.Y += 20;
                        player.Walking = true;
                    }
                    if (kb.IsKeyDown(Keys.Right) && worldPos.X < buildingBG[currentBuilding].Width / 2 - 500)
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
            }
            prevkb = kb;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Vector2 worldToScreen = screenPos - worldPos;

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.Menu:
                    //title screen
                    _spriteBatch.Draw(
                        titleScreen,
                        new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
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
                        new Rectangle(
                            (int)worldToScreen.X, 
                            (int)worldToScreen.Y, 
                            moonriverBG.Width, 
                            moonriverBG.Height),
                        Color.White);

                    // npcs
                    if (!scene2)
                    {
                        _spriteBatch.Draw(
                         npcTextures[(int)Symbols.Mushroom],
                         NPCs[(int)Symbols.Mushroom].Reposition(worldPos),
                         Color.White);
                    }
                    else if (complete)
                    {
                        _spriteBatch.Draw(
                        npcTextures[(int)Symbols.Mushroom],
                        end.Reposition(worldPos),
                        Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(
                        npcTextures[(int)Symbols.Mushroom],
                        NPCsPT2[(int)Symbols.Mushroom].Reposition(worldPos),
                        Color.White);
                    }
                    

                    if ((int)progress == currentNPC)
                    {
                        Rectangle NPCLoc = NPCs[currentNPC].Reposition(worldPos);
                        _spriteBatch.Draw(
                            questMarker,
                            new Rectangle(NPCLoc.X + 50, NPCLoc.Y - 20, 30, 30),
                            Color.White);
                    }

                    // player
                    player.Draw(_spriteBatch);
                    break;

                case GameState.Dialogue:
                    _spriteBatch.Draw(
                                npcFullTextures[currentNPC],
                                new Rectangle(200, 20, 500, 500),
                                Color.White);
                    NPCs[currentNPC].DrawString(_spriteBatch, coolfont);

                    /*switch (progress)
                    {
                        case Symbols.Mushroom:
                            break;
                        case Symbols.Star:
                            break;
                        case Symbols.Heart:
                            break;
                        case Symbols.Flower:
                            break;
                        case Symbols.Pen:
                            break;
                        case Symbols.Moon:
                            break;
                    }*/

                    break;

                case GameState.Building:
                    // map
                    _spriteBatch.Draw(
                        buildingBG[currentBuilding],
                        new Rectangle(
                            (int)worldToScreen.X,
                            (int)worldToScreen.Y,
                            buildingBG[currentBuilding].Width,
                            buildingBG[currentBuilding].Height),
                        Color.White);

                    // npc
                    if (!scene2)
                    {
                        _spriteBatch.Draw(
                        npcTextures[currentBuilding],
                        NPCs[currentBuilding].Reposition(worldPos),
                        Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(
                        npcTextures[currentBuilding],
                        NPCsPT2[currentBuilding].Reposition(worldPos),
                        Color.White);
                    }
                    

                    // player
                    player.Draw(_spriteBatch);

                    // quest marker
                    if ((int)progress == currentNPC)
                    {
                        Rectangle NPCLoc = NPCs[currentBuilding].Reposition(worldPos);
                        _spriteBatch.Draw(
                            questMarker,
                            new Rectangle(NPCLoc.X + 50, NPCLoc.Y - 20, 20, 20),
                            Color.White);
                    }
                    /*switch (currentBuilding)
                    {
                        case (int) Symbols.Mushroom:
                            break;
                        case (int) Symbols.Star:
                            break;
                        case (int) Symbols.Heart:
                            break;
                        case (int) Symbols.Flower:
                            break;
                        case (int) Symbols.Pen:
                            break;
                        case (int) Symbols.Moon:
                            break;
                    }*/

                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
