using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;


namespace MisjaNaMarsa
{
    public class MarsGame : GameLoop
    {

        Dictionary<string, int> stats = new Dictionary<string, int>();

        public Font font;

        uint width, height;
        string windowTitle, consoleFont;
        int startingStatsValue;


        RectangleShape background = new RectangleShape();
        RectangleShape errorsTable = new RectangleShape();
        RectangleShape resourcesTable = new RectangleShape();

        bool errorsActive = false;
        bool resourcesActive = false;
        bool toggled = false;

        Text[] statTexts = new Text[3];

        Text[] resourceTexts = new Text[9];

        QuestionManager qm = new QuestionManager();

        List<Button> errors = new List<Button>();

        TextField answerField;

        Vector2i mousePosWindow = new Vector2i();

        bool added = false;

        Button errorsBtn;
        Button resourcesBtn;

        public MarsGame()
        {
        }

        void ToggleErrors()
        {
            if (errorsActive)
                errorsActive = false;
            else
                errorsActive = true;
        }

        void ToggleResources()
        {
            if (resourcesActive)
                resourcesActive = false;
            else
                resourcesActive = true;
        }

        private void WindowClosed(object? sender, EventArgs e)
        {
            Window.Close();
        }

        public override void Draw(GameTime gameTime)
        {
            this.Window.Draw(background);

            //DebugUtility.DrawPerformenceData(this, Color.White);
            //qm.DrawQuestionsData(this, Color.White);

            DrawStats();
            DrawButtons();
            DrawWindows();
            DrawResources();
            DrawTextField();
        }

        private void DrawButtons()
        {
            errorsBtn.Draw(this);
            resourcesBtn.Draw(this);
        }

        public void DrawWindows()
        {
            if (errorsActive)
            {
                this.Window.Draw(errorsTable);
                foreach(Button btn in errors)
                {
                    btn.Draw(this);
                }
            }

            if(resourcesActive)
            {
                this.Window.Draw(resourcesTable);
            }
        }

        public void DrawStats()
        {
            foreach (Text text in statTexts)
            {
                this.Window.Draw(text);
            }
        }

        public void DrawResources()
        {
            if (resourcesActive)
            {
                foreach (Text text in resourceTexts)
                {
                    this.Window.Draw(text);
                }
            }
        }

        public void DrawTextField()
        {
            if (answerField.visible)
            {
                answerField.Draw(this);
            }
        }
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                if (resourcesActive)
                    resourcesActive = false;
                else if (errorsActive && !answerField.visible)
                    errorsActive = false;

                else if(!answerField.visible)
                    window.Close();
            }
            answerField.handleInput(e);
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
                qm.timeBetweenQuestions = Int16.Parse(sr.ReadLine());
                line = sr.ReadLine();
                startingStatsValue = Int16.Parse(sr.ReadLine());
            }
            Window = new RenderWindow(new VideoMode(width, height), windowTitle, Styles.Fullscreen);

            Window.Closed += WindowClosed;

            Window.KeyPressed += Window_KeyPressed;
            Window.DispatchEvents();

            background.Size = new Vector2f(Convert.ToInt16(width), Convert.ToInt16(height));
            background.Texture = new Texture("./Dashboard.png", new IntRect(0, 0, Convert.ToInt16(width), Convert.ToInt16(height)));

            errorsTable.Size = new Vector2f((4835/4501) * Convert.ToInt16(height)* 2 / 3, Convert.ToInt16(height) * 2 / 3);
            errorsTable.Texture = new Texture("./ErrorsTable.png", new IntRect(0, 0, 4835, 4501));
            errorsTable.Position = new Vector2f(Convert.ToInt16(width) / 10, Convert.ToInt16(height) / 10);

            resourcesTable.Size = new Vector2f(1132, Convert.ToInt16(height) * 2 / 3);
            resourcesTable.Texture = new Texture("./ResourcesTable.png", new IntRect(0, 0, 4835, 3076));
            resourcesTable.Position = new Vector2f(Convert.ToInt16(width) / 10, Convert.ToInt16(height) / 10);

            this.stats["sprawnosc"] = startingStatsValue;
            this.stats["morale"] = startingStatsValue;
            this.stats["zapasy"] = startingStatsValue;

            this.stats["paliwo"] = 0;
            this.stats["poszycie"] = 0;
            this.stats["energia"] = 0;
            this.stats["chwytaki"] = 0;
            this.stats["gasienica"] = 0;
            this.stats["antena"] = 0;
            this.stats["termostat"] = 0;
            this.stats["komora"] = 0;
            this.stats["zegarek"] = 0;



            this.font = new Font(consoleFont);


            statTexts[0] = new Text();
            statTexts[1] = new Text();
            statTexts[2] = new Text();

            resourceTexts[0] = new Text();
            resourceTexts[1] = new Text();
            resourceTexts[2] = new Text();
            resourceTexts[3] = new Text();
            resourceTexts[4] = new Text();
            resourceTexts[5] = new Text();
            resourceTexts[6] = new Text();
            resourceTexts[7] = new Text();
            resourceTexts[8] = new Text();

            errorsBtn = new Button(1365, 878, 264, 195, consoleFont, "", Color.Transparent, Color.Transparent, Color.Transparent);
            resourcesBtn = new Button(1623, 878, 264, 195, consoleFont, "", Color.Transparent, Color.Transparent, Color.Transparent);


            foreach (Text text in statTexts)
            {
                text.Font = new Font(consoleFont);
                text.Color = new Color(134, 222, 242);
                text.CharacterSize = 62;
            }

            foreach (Text text in resourceTexts)
            {
                text.Font = new Font(consoleFont);
                text.Color = new Color(134, 222, 242);
                text.CharacterSize = 62;
            }

            answerField = new TextField(1000, 250, 20, consoleFont);

        }
        

        public override void LoadContent()
        {
            qm.LoadQuestions();
            DebugUtility.LoadFont();
            qm.LoadFont();
        }
        public void UpdateButtons(GameTime gameTime)
        {
            errorsBtn.Update(mousePosWindow);
            resourcesBtn.Update(mousePosWindow);

            foreach(Button btn in errors)
            {
                btn.Update(mousePosWindow);
                if(btn.IsPressed())
                {
                    answerField.visible = true;
                    answerField.q = btn.q;
                }
            }

            if(errorsBtn.IsPressed() && !toggled)
            {
                ToggleErrors();
                resourcesActive = false;
                toggled = true;
            }
            if(errorsActive && errorsBtn.IsPressedOutside() && !toggled && !resourcesBtn.IsPressed())
            {
                if (!errorsTable.GetGlobalBounds().Contains(mousePosWindow.X, mousePosWindow.Y))
                {
                    ToggleErrors();
                    toggled = true;
                }
            }

            if(resourcesBtn.IsPressed() && !toggled)
            {

                ToggleResources();
                errorsActive = false;
                toggled = true;
            }

            if (resourcesActive && resourcesBtn.IsPressedOutside() && !toggled && !errorsBtn.IsPressed())
            {
                if (!resourcesTable.GetGlobalBounds().Contains(mousePosWindow.X, mousePosWindow.Y))
                {
                    ToggleResources();
                    toggled = true;
                }
            }

            if (!errorsBtn.IsPressed() && !resourcesBtn.IsPressed())
            {
                toggled = false;
            }
        }

        public void UpdateBars()
        {
            for(int i=0; i<3; i++)
            {
                if(stats.ElementAt(i).Value<=0)
                {
                    Window.Close();
                }
            }
            
            statTexts[0].DisplayedString = stats["sprawnosc"].ToString();
            statTexts[1].DisplayedString = stats["morale"].ToString();
            statTexts[2].DisplayedString = stats["zapasy"].ToString();

            statTexts[0].Position = new Vector2f(389-statTexts[0].GetGlobalBounds().Width/2, 520-statTexts[0].GetGlobalBounds().Height / 2);
            statTexts[1].Position = new Vector2f(955-statTexts[1].GetGlobalBounds().Width/2, 520-statTexts[1].GetGlobalBounds().Height / 2);
            statTexts[2].Position = new Vector2f(1521-statTexts[2].GetGlobalBounds().Width/2, 520-statTexts[2].GetGlobalBounds().Height / 2);

            resourceTexts[0].DisplayedString = stats["paliwo"].ToString();
            resourceTexts[1].DisplayedString = stats["poszycie"].ToString();
            resourceTexts[2].DisplayedString = stats["energia"].ToString();
            resourceTexts[3].DisplayedString = stats["chwytaki"].ToString();
            resourceTexts[4].DisplayedString = stats["gasienica"].ToString();
            resourceTexts[5].DisplayedString = stats["antena"].ToString();
            resourceTexts[6].DisplayedString = stats["termostat"].ToString();
            resourceTexts[7].DisplayedString = stats["komora"].ToString();
            resourceTexts[8].DisplayedString = stats["zegarek"].ToString();

            for(int i=0; i<9; i++)
            {
                resourceTexts[i].Position = new Vector2f(1110-resourceTexts[i].GetGlobalBounds().Width / 2, (float)(Math.Round(211+(58.8*i)) - resourceTexts[i].GetGlobalBounds().Height / 2));
            }
        }

        void UpdateErrors(GameTime gameTime)
        {
            if(Math.Floor(gameTime.TotalTimeElapsed) % 2 == 0 && !added)
            {
                errors.Clear();
                foreach(Question q in qm.activeQuestions)
                {
                    errors.Add(new Button(errorsTable.Position.X + 95 * Convert.ToInt16(width) / 4835, errorsTable.Position.Y + 645 * Convert.ToInt16(height) / 4501 + 52 * (errors.Count),
                                          645, 50, consoleFont, q));
                }
                added = true;
            }

            if (Math.Floor(gameTime.TotalTimeElapsed) % 2 == 1)
            {
                added = false;
            }
        }
        void CheckAnswer()
        {
            if (!answerField.answer)
                return;
            if (answerField.q.type == "zamk")
            {
                int correct = 0;
                bool mistake = false;
                string[] answers = answerField.result.Split(' ');
                
                    for (int j = 0; j < answers.Length; j++)
                    {
                        if (answerField.q.answers.ContainsKey(answers[j]))
                        {
                            correct++;
                        }
                        else
                        {
                            mistake = true;
                        }
                    }
                
                    if(mistake)
                {
                    stats[answerField.q.type3Wrong.Item1] += answerField.q.type3Wrong.Item2;
                }
                else
                {
                    int points = correct / answerField.q.type3Correct.ElementAt(0).Key;
                    stats[answerField.q.type3Correct.ElementAt(0).Value.Item1] += points*answerField.q.type3Correct.ElementAt(0).Value.Item2;
                }
                
            }
            else
            {
                for (int i = 0; i < answerField.q.answers.Count; i++)
                {
                    if (answerField.q.answers.ContainsKey(answerField.result))
                    {
                        stats[answerField.q.answers[answerField.result].Item1] += answerField.q.answers[answerField.result].Item2;
                        break;
                    }
                }
                
            }
            answerField.answer = false;
            errors.Remove(errors.Find(x => x.q == answerField.q));
            qm.activeQuestions.Remove(answerField.q);
        }

        public override void Update(GameTime gameTime)
        {
            this.mousePosWindow = Mouse.GetPosition(this.Window);
            qm.Update(gameTime);
            CheckAnswer();
            UpdateErrors(gameTime);
            UpdateButtons(gameTime);
            UpdateBars();
        }
    }
}
