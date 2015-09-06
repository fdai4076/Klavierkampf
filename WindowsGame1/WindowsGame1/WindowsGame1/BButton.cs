using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    class BButton
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        Color colour = new Color(255, 255, 255, 255);

        public Vector2 size;

        public BButton(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;

            size = new Vector2(texture.Width, texture.Height);
        }

        int bewegung = 0;

        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (bewegung <= 30)
                {
                    position.X += 3;
                    bewegung += 3;
                }

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                    position.X -= bewegung;
                    bewegung = 0;
                }

            }

            else
            {
                if (bewegung > 0 && bewegung <= 30)
                {
                    position.X -= 3;
                    bewegung -= 3;
                }
            }


        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, colour);
        }

    }
}
