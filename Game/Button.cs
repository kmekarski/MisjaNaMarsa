using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MisjaNaMarsa
{
    public enum buttonState
    {
        BTN_IDLE = 0,
        BTN_HOVER = 1,
        BTN_PRESSED = 2
    }
    public class Button

    {
        string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        RectangleShape shape = new RectangleShape();
        Font font;
        public Text text = new Text();

        Text code = new Text();
        Text name = new Text();
        Text start = new Text();
        Text stop = new Text();

        Color idleColor;
        Color hoverColor;
        Color pressedColor;

        public Question q;

       

        buttonState state;
        public Button(float x, float y, float width, float height, string fontPath, string text, Color idleColor, Color hoverColor, Color pressedColor)
        {

            this.state = buttonState.BTN_IDLE;

            this.shape.Position = new Vector2f(x, y);
            this.shape.Size = new Vector2f(width, height);


            this.text.Font = new Font(fontPath);
            this.text.DisplayedString = text;
            this.text.Color = Color.White;
            this.text.CharacterSize = 22;
            this.text.Position = new Vector2f(x+this.shape.GetGlobalBounds().Width/2f - this.text.GetGlobalBounds().Width / 2f, y + this.shape.GetGlobalBounds().Height/2f  - this.text.GetGlobalBounds().Height);

            this.idleColor = idleColor;
            this.hoverColor = hoverColor;
            this.pressedColor = pressedColor;

            this.shape.FillColor = this.idleColor;


        }
        public Button(float x, float y, float width, float height, string fontPath, Question q)
        {
            for(int i=1; i<q.name.Length-1; i++)
            {
                if (Char.IsUpper(q.name[i+1]))
                {
                    q.name.Insert(i, " ");
                }
            }
            this.state = buttonState.BTN_IDLE;

            this.shape.Position = new Vector2f(x, y);
            this.shape.Size = new Vector2f(width, height);


            this.code.Font = new Font(fontPath);
            this.code.Color = new Color(134, 222, 242);
            this.code.CharacterSize = 18;
            this.code.DisplayedString = q.qCode;
            this.code.Position = new Vector2f(x + this.shape.GetGlobalBounds().Width / 2f - this.code.GetGlobalBounds().Width / 2f - 300,
                y + this.shape.GetGlobalBounds().Height / 2f - this.code.GetGlobalBounds().Height);

            this.name.Font = new Font(fontPath);
            this.name.Color = new Color(134, 222, 242);
            this.name.CharacterSize = 18;
            this.name.DisplayedString = AddSpacesToSentence(q.name);
            this.name.Position = new Vector2f(x + this.shape.GetGlobalBounds().Width / 2f - this.name.GetGlobalBounds().Width / 2f - 65,
                y + this.shape.GetGlobalBounds().Height / 2f - 12);

            this.start.Font = new Font(fontPath);
            this.start.Color = new Color(134, 222, 242);
            this.start.CharacterSize = 18;
            this.start.DisplayedString = q.startTime.ToString();
            this.start.DisplayedString = Math.Floor(q.startTime).ToString() + ":";
            if ((q.startTime - Math.Floor(q.startTime)) * 60 < 10)
            {
                this.start.DisplayedString += "0";
                    }
            this.start.DisplayedString += Math.Round((q.startTime - Math.Floor(q.startTime))*60).ToString();
            this.start.Position = new Vector2f(x + this.shape.GetGlobalBounds().Width / 2f - this.start.GetGlobalBounds().Width / 2f +150,
                y + this.shape.GetGlobalBounds().Height / 2f - this.start.GetGlobalBounds().Height);

            this.stop.Font = new Font(fontPath);
            this.stop.Color = new Color(134, 222, 242);
            this.stop.CharacterSize = 18;
            this.stop.DisplayedString = Math.Floor(q.endTime).ToString() + ":";
            if ((q.endTime - Math.Floor(q.endTime))*60 < 10)
            {
                this.stop.DisplayedString += "0";
            }
            this.stop.DisplayedString += Math.Round((q.endTime - Math.Floor(q.endTime)) * 60).ToString();
            this.stop.Position = new Vector2f(x + this.shape.GetGlobalBounds().Width / 2f - this.stop.GetGlobalBounds().Width / 2f +307,
                y + this.shape.GetGlobalBounds().Height / 2f - this.stop.GetGlobalBounds().Height);

            this.idleColor = Color.Transparent;
            this.hoverColor = new Color(255, 255, 255, 50);
            this.pressedColor = new Color(255, 255, 255, 128);

            this.shape.FillColor = this.idleColor;

            this.q = q;
        }

        public Button(float v1, double v2, int v3, int v4, string consoleFont, Question q)
        {
            this.q = q;
        }

        public void Update(Vector2i mousePos)
        {
            this.state = buttonState.BTN_IDLE;

            if (this.shape.GetGlobalBounds().Contains(mousePos.X, mousePos.Y))
            {
                this.state = buttonState.BTN_HOVER;
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    this.state = buttonState.BTN_PRESSED;
                }
            }

            switch (this.state)
            {
                case buttonState.BTN_IDLE:
                    this.shape.FillColor = this.idleColor;
                    break;
                case buttonState.BTN_HOVER:
                    this.shape.FillColor = this.hoverColor;
                    break;
                case buttonState.BTN_PRESSED:
                    this.shape.FillColor = this.pressedColor;
                    break;
                default:
                    this.shape.FillColor = Color.Red;
                    break;
            }
        }
        public void Draw(GameLoop gameLoop)
        {
            gameLoop.Window.Draw(this.shape);
            gameLoop.Window.Draw(this.text);

            gameLoop.Window.Draw(this.code);
            gameLoop.Window.Draw(this.name);
            gameLoop.Window.Draw(this.start);
            gameLoop.Window.Draw(this.stop);
        }
        public bool IsPressed()
            {
            if(this.state == buttonState.BTN_PRESSED)
                 return true;
                 return false;
            }
        public bool IsPressedOutside()
        {
            if (this.state != buttonState.BTN_PRESSED && Mouse.IsButtonPressed(Mouse.Button.Left))
                return true;
            return false;
        }


    }
}
