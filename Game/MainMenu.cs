using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MisjaNaMarsa
{

    public class MainMenu : GameLoop
    {

        Button startBtn;

        MarsGame game = new MarsGame();

        uint width, height;
        string windowTitle, consoleFont;

        RectangleShape mainMenu = new RectangleShape();

        Vector2i mousePosWindow = new Vector2i();

        public MainMenu()
        {
        }

        public override void Draw(GameTime gameTime)
        {
            this.Window.Draw(mainMenu);
            startBtn.Draw(this);
        }

        public override void Initialize()
        {
            Window = new RenderWindow(new VideoMode(1920, 1080), "Operacja Amundsen", Styles.Fullscreen);

            mainMenu.Size = new Vector2f(Convert.ToInt16(1920), Convert.ToInt16(1080));
            mainMenu.Texture = new Texture("./MainMenu.png", new IntRect(0, 0, Convert.ToInt16(1920), Convert.ToInt16(1080)));

            startBtn = new Button(770, 585, 385, 85, "./fonts/BlackOpsOne-Regular.ttf", "", Color.Transparent, Color.Transparent, Color.Transparent);
        }

        public override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            this.mousePosWindow = Mouse.GetPosition(this.Window);
            startBtn.Update(mousePosWindow);
            if (startBtn.IsPressed())
            {

                this.Window.Close();
                game.Run();
            }

        }
    }
}
