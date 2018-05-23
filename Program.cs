using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using System.IO;

namespace LearningAStarB
{
    class Program
    {
        static void Main(string[] args)
        {
            Window window = new Window(800, 800, "Learning A Star");
            window.SetDefaultOrthographicSize(10);

            int[] cells;
            int line = 0;
            using (StreamReader sr = File.OpenText("TextFile1.txt"))
            {
                string s;
                s = sr.ReadLine();
                s.Split(',');
                cells = new int[s[0] * s[1]];
                while ((s = sr.ReadLine()) != null)
                {
                    string[] str = s.Split(',');
                    for (int j = 0; j < str.Length; j++)
                    {
                        int k = int.Parse(str[j]);
                        cells[line++] = k;
                    }
                }
            }


            Map map = new Map(10, 10, cells);
            Agent agent = new Agent(1, 1);
            Enemy enemy = new Enemy(1, 8);
            float counter = 0.25f;
            while (window.opened)
            {
                counter -= window.deltaTime;
                List<Node> enemyPath = map.GetPath(enemy.X, enemy.Y, agent.X, agent.Y);
                if (enemyPath.Count > 0)
                    enemy.SetPath(enemyPath);
                if (window.mouseLeft)
                {
                    List<Node> path = map.GetPath(agent.X, agent.Y, (int)window.mouseX, (int)window.mouseY);
                    if (path.Count > 0)
                        agent.SetPath(path);

                }
                if (window.mouseRight && counter <= 0)
                {
                    map.ChangeNode((int)window.mouseX, (int)window.mouseY);
                    counter = 0.25f;
                }

                agent.Update(window.deltaTime);
                enemy.Update(window.deltaTime);
                map.Draw();
                agent.Draw();
                enemy.Draw();

                window.Update();
            }
        }
    }
}
