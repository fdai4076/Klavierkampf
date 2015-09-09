using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    class Button
    {
        Texture2D texture;
        Texture2D texture2;
        Texture2D finalTexture;
        Vector2 position;
        Rectangle rectangle;

        public Vector2 size;

        public Button(Texture2D newTexture, Texture2D newTexture2, GraphicsDevice graphics)
        {
            texture = newTexture;
            texture2 = newTexture2;
            finalTexture = newTexture;


            size = new Vector2(texture.Width, texture.Height);
        }

        bool down;


        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {

                if (finalTexture != texture2)
                {
                    finalTexture = texture2;
                }

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;

                }

            }

            else if (finalTexture == texture2)
            {
                finalTexture = texture;
                isClicked = false;
            }
        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(finalTexture, position, Color.White);
        }

    }
}
