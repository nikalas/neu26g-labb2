using System;
using System.Collections.Generic;
using System.Text;

namespace neu26g_labb2.Models
{
    internal class LevelData
    {
        private readonly List<LevelElement> elements = new();

        public IReadOnlyList<LevelElement> Elements => elements.AsReadOnly();


        public void Load(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            int y = 0;
            foreach (string line in lines)
            {
                int x = 0;
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case '#':
                            elements.Add(new Wall(x, y));
                            break;
                        case 'r':
                            elements.Add(new Rat(x, y));
                            break;
                        case 's':
                            elements.Add(new Snake(x, y));
                            break;
                        case '@':
                            elements.Add(new Player(x, y));
                            break;
                        default:
                            break;
                    }
                    x++;
                }
                y++;
            }
        }
    }
}
