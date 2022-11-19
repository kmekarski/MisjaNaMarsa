using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

namespace MisjaNaMarsa
{
    public static class DebugUtility
    {
        public static string CONSOLE_FONT_PATH = "./fonts/arial.ttf";

        public static Font consoleFont;
        public static void LoadFont()
        {
            consoleFont = new Font(CONSOLE_FONT_PATH);
        }

        public static void DrawPerformenceData(GameLoop gameLoop, Color fontColor)
        {
            if (consoleFont == null)
                return;

            string totalTimeElapsed = gameLoop.GameTime.TotalTimeElapsed.ToString("0.000");
            string deltaTime = gameLoop.GameTime.DeltaTime.ToString("0.00000");
            float fps = 1f / gameLoop.GameTime.DeltaTime;
            string fpsStr = fps.ToString("0.00");

            Text textA = new Text("total time elapsed: " +totalTimeElapsed, consoleFont, 14);
            textA.Position = new Vector2f(4f, 8f);
            textA.Color = fontColor;

            Text textB = new Text("delta time: " + deltaTime, consoleFont, 14);
            textB.Position = new Vector2f(4f, 22f);
            textB.Color = fontColor;


            Text textC = new Text("fps: " + fpsStr, consoleFont, 14);
            textC.Position = new Vector2f(4f, 36f);
            textC.Color = fontColor;



            gameLoop.Window.Draw(textA);
            gameLoop.Window.Draw(textB);
            gameLoop.Window.Draw(textC);
        }
    }
}
