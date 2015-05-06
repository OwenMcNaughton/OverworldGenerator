using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OverworldGenerator
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int width = 500; int height = 500;
        int points = 1000;

        MapGenerator mapgen;

        Texture2D map;

        int imageCount = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i != 1; i++)
            {
                mapgen = new MapGenerator(width, height, points, 
                    MapGenerator.VoroType.RELAXED, MapGenerator.IslandType.RADIAL); /*
                map = mapgen.GetImage(GraphicsDevice);
                
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                map.SaveAsPng(ms, map.Width, map.Height);

                Bitmap bm = new Bitmap(ms);
                bm.Save("C:\\Users\\Owen\\Desktop\\" + imageCount++ + "bigimg.png");*/
                
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            spriteBatch.Begin();

            mapgen.Draw(gameTime, spriteBatch);
            //spriteBatch.Draw(map, new Vector2(0, 0), Microsoft.Xna.Framework.Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
