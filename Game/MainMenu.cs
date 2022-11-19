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
        Button newGameButton;

        uint width, height;
        string windowTitle, consoleFont;
        int startingStatsValue;

        Vector2i mousePosWindow = new Vector2i();

        public MainMenu()
        {
            newGameButton = new Button(100, 100, 100, 100, "./fonts/arial.ttf", "new game", Color.White, Color.Cyan, Color.Magenta);
        }

        public override void Draw(GameTime gameTime)
        {
            this.newGameButton.Draw(this);
        }

        public override void Initialize()
        {
            using (StreamReader sr = new StreamReader("./config.txt"))
            {
                string line = sr.ReadLine();
                width = Convert.ToUInt16(sr.ReadLine());
                line = sr.ReadLine();
                height = Convert.ToUInt16(sr.ReadLine());
                line = sr.ReadLine();
                windowTitle = sr.ReadLine();
                line = sr.ReadLine();
                consoleFont = "./fonts/" + sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                line = sr.ReadLine();
                startingStatsValue = Int16.Parse(sr.ReadLine());
            }
            Window = new RenderWindow(new VideoMode(width, height), windowTitle, Styles.Fullscreen);
        }

        public override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            this.mousePosWindow = Mouse.GetPosition(this.Window);

            if (newGameButton.IsPressed())
            {
                MarsGame game = new MarsGame();

                game.Run();
            }

        }
    }
}
