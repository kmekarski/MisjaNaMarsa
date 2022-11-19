using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

using SFML.Graphics;
using SFML.System;
using System.Security.Cryptography.X509Certificates;

namespace MisjaNaMarsa
{
    public class QuestionManager
    {
        public List<Question> questions = new List<Question>();
        public List<Question> activeQuestions = new List<Question>();


        public static string CONSOLE_FONT_PATH = "./fonts/arial.ttf";

        public Font consoleFont;

        public int timeBetweenQuestions;

        public bool added = false;


        public void LoadFont()
        {
            consoleFont = new Font(CONSOLE_FONT_PATH);
        }

        public void LoadQuestions()
        {
            using (StreamReader sr = new StreamReader("./questions.txt"))
            {

                string line;
                line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {

                    string[] bits = line.Split(' ');

                    int noOfAnswers;

                    Dictionary<string, Tuple<string, int>> answers = new Dictionary<string, Tuple<string, int>>();
                    Dictionary<int, Tuple<string, int>> type3Correct = new Dictionary<int, Tuple<string, int>>();
                    Tuple<string, int> type3Wrong = new Tuple<string,int>("",0);

                    switch (bits[0])
                    {
                        case "prosty":
                            noOfAnswers = 1;
                            answers.Add(bits[4], new Tuple<string, int>(bits[5], Convert.ToInt16(bits[6])));
                            questions.Add(new Question(bits[0], bits[1], Convert.ToDouble(bits[2]), Convert.ToDouble(bits[3]), answers, type3Correct, type3Wrong));
                            break;

                        case "opt":
                            noOfAnswers = (bits.Length - 4) / 3;
                            for (int i = 0; i < noOfAnswers; i++)
                                answers.Add(bits[1 + 3 * (i+1)], new Tuple<string, int>(bits[2 + 3 * (i+1)], Convert.ToInt16(bits[3 + 3 * (i+1)])));
                            questions.Add(new Question(bits[0], bits[1], Convert.ToDouble(bits[2]), Convert.ToDouble(bits[3]), answers, type3Correct, type3Wrong));
                            break;

                        case "zamk":
                            noOfAnswers = bits.Length - 13;
                            for (int i = 0; i < noOfAnswers; i++)
                                answers.Add(bits[4 + i], new Tuple<string, int>("",0));
                            type3Correct.Add(Convert.ToInt16(bits[6 + noOfAnswers]), new Tuple<string, int>(bits[8 + noOfAnswers], Convert.ToInt16(bits[9 + noOfAnswers])));
                            type3Wrong = new Tuple<string, int>(bits[11 + noOfAnswers], Convert.ToInt16(bits[12 + noOfAnswers]));
                            questions.Add(new Question(bits[0], bits[1], Convert.ToDouble(bits[2]), Convert.ToDouble(bits[3]), answers, type3Correct, type3Wrong));
                            break;

                    }
                }
            }
        }
        public void DrawQuestionsData(GameLoop gameLoop, Color fontColor)
        {
            Text text = new Text("questions: " + questions.Count.ToString(), consoleFont, 14);
            text.Position = new Vector2f(4f, 64f);
            text.Color = fontColor;
            gameLoop.Window.Draw(text);

            for (int i = 0; i < questions.Count; i++)
            {
                text = new Text(questions[i].ToString(), consoleFont, 14);
                text.Position = new Vector2f(4f, 78 + 112 * i);
                text.Color = fontColor;
                gameLoop.Window.Draw(text);
            }

            text = new Text("active questions: "+ activeQuestions.Count.ToString(), consoleFont, 14);
            text.Position = new Vector2f(4f, 78 + 112 * questions.Count);
            text.Color = fontColor;
            gameLoop.Window.Draw(text);

            for (int i = 0; i < activeQuestions.Count; i++)
            {
                text = new Text(Convert.ToString(i), consoleFont, 14);
                text.Position = new Vector2f(4f, 92 + 112 * questions.Count + 112 * i);
                text.Color = fontColor;
                gameLoop.Window.Draw(text);
            }
        }

        void UpdateActiveQuestions(GameTime gameTime)
        {
            if (Math.Floor(gameTime.TotalTimeElapsed) % timeBetweenQuestions == 0 && !added)
            {
                foreach(Question q in questions)
                {
                    if(q.startTime == Math.Floor(gameTime.TotalTimeElapsed) / 60)
                    {
                        activeQuestions.Add(q);
                        added = true;
                        break;
                    }
                }
            }
            if (Math.Floor(gameTime.TotalTimeElapsed) % timeBetweenQuestions == 0)
            {
                foreach (Question q in activeQuestions)
                {
                    if (q.endTime == Math.Floor(gameTime.TotalTimeElapsed) / 60)
                    {
                        activeQuestions.Remove(q);
                        break;
                    }
                }
            }
            if (Math.Floor(gameTime.TotalTimeElapsed) % timeBetweenQuestions != 0)
            {
                added = false;
            }
        }



        public void Update(GameTime gameTime)
        {
            UpdateActiveQuestions(gameTime);
        }
    }
}
