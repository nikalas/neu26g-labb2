using System.Collections.ObjectModel;

namespace neu26g_labb2.Models
{
    public class LevelData
    {
        private readonly List<LevelElement> elements = new();
        private readonly ReadOnlyCollection<LevelElement> readonlyElements;

        public LevelData()
        {
            readonlyElements = elements.AsReadOnly();
        }

        public IReadOnlyList<LevelElement> Elements => readonlyElements;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public void Load(string filename)
        {
            elements.Clear();

            string[] lines = File.ReadAllLines(filename);

            Height = lines.Length;
            Width = lines.Length == 0 ? 0 : lines.Max(line => line.Length);

            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];

                for (int x = 0; x < line.Length; x++)
                {
                    switch (line[x])
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
                    }
                }
            }
        }

        public void Add(LevelElement element)
        {
            elements.Add(element);
        }

        public bool Remove(LevelElement element)
        {
            return elements.Remove(element);
        }

        public LevelElement? ElementAt(int x, int y)
        {
            return elements.FirstOrDefault(element =>
                element.X == x &&
                element.Y == y &&
                !(element is Actor actor && actor.IsDead));
        }
    }
}
