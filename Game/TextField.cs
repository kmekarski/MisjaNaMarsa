using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace MisjaNaMarsa
{
    public class TextField
    {
        public TextField(int x, int y, int maxChars, string fontPath)
        {

            size = maxChars;

            background = new RectangleShape();
            background.Size = new Vector2f(364, 233);
            background.Texture = new Texture("./AnswerDialog.png", new IntRect(0, 0, 1623, 1165));
            background.Position = new Vector2f(x,y);

            field = new RectangleShape(new Vector2f(15 * size, 30));


            field.OutlineThickness = 2;
            field.FillColor = new Color(134, 222, 242);
            field.OutlineColor = new Color(127, 127, 127);
            field.Position = new Vector2f(background.Position.X+(background.Size.X-15*size)/2, background.Position.Y + background.Size.Y/2-15);


            hasFocus = true;

            this.text = new Text();

            this.text.Font = new Font(fontPath);
            this.text.DisplayedString = "";
            this.text.Color = new Color(34, 55, 60);
            this.text.CharacterSize = 25;
            this.text.Position = new Vector2f(field.Position.X+5, field.Position.Y);

            this.result = "";
            this.answer = false;
        }

        int size;
        public Text text;
        public string result;
        RectangleShape field;
        RectangleShape background;
        public bool hasFocus;
        public bool visible;
        public bool answer;

        public Question q;

        public string Text
        {
            get { return text.DisplayedString; }
        }

        public void handleInput(KeyEventArgs e)
        {
            if (!hasFocus)
                return;
            switch(e.Code)
            {
                case SFML.Window.Keyboard.Key.Escape:
                    {
                        visible = false;
                        text.DisplayedString = "";
                        break;
                    }
                case SFML.Window.Keyboard.Key.BackSpace:
                    {
                        if (text.DisplayedString.Length > 0)
                        {
                            text.DisplayedString = text.DisplayedString.Substring(0, text.DisplayedString.Length - 1);
                        }
                        return;
                        break;
                    }
                case SFML.Window.Keyboard.Key.Return:
                    {
                        result = text.DisplayedString;
                        result = result.ToLower();
                        text.DisplayedString = "";
                        visible = false;
                        answer = true;
                        return;
                        break;
                    }
                case SFML.Window.Keyboard.Key.Space:
                    {
                        text.DisplayedString += " ";
                        return;
                        break;
                    }
                default:
                    {
                        if(e.Code >= SFML.Window.Keyboard.Key.A && e.Code <= SFML.Window.Keyboard.Key.Z)
                        text.DisplayedString += e.Code;

                        if (e.Code >= SFML.Window.Keyboard.Key.Num0 && e.Code <= SFML.Window.Keyboard.Key.Num9)
                            text.DisplayedString += e.Code.ToString().Substring(3);
                        return;
                    }

            }
        }
        public void Draw(GameLoop gameLoop)
        {
            gameLoop.Window.Draw(this.background);
            gameLoop.Window.Draw(this.field);
            gameLoop.Window.Draw(this.text);
        }
    }
}
