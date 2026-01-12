using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Willkommen, bitte breite und länge eingeben: ");

            // Eingabe der höhe und breite
            int höhe = EingabeZahl("Höhe: ");
            int breite = EingabeZahl("Breite: ");

            // Erstellung des Dungeons
            var dungeon = GenerateDungeon(höhe, breite); 

            // Dungeon Ausgeben
            PrintDungeon(dungeon);

            // Alles ausgeben
            Console.ReadLine();
        }

        static Random rnd = new Random();

        public static char[,] GenerateDungeon(int height, int width)
        {
            // Nur ungerade Maße funktionieren perfekt für Maze-Carving
            if (height % 2 == 0) height++;
            if (width % 2 == 0) width++;

            char[,] map = new char[height, width];

            // 1. Alles mit Wänden füllen
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map[y, x] = '#';

            // 2. Maze-Carving starten
            LabyrinthWege(map, 1, 1);

            // 3. Start & Ende setzen
            map[1, 1] = 'S';
            map[height - 2, width - 2] = 'E';

            // 4. Schätze setzen
            for (int i = 0; i < 3; i++)
            {
                int x, y;
                do
                {
                    x = rnd.Next(1, width - 1);
                    y = rnd.Next(1, height - 1);
                }
                while (map[y, x] != '.');

                map[y, x] = 'T';
            }

            return map;
        }

        // Erstellung des Labyrinths
        private static void LabyrinthWege(char[,] map, int y, int x)
        {
            map[y, x] = '.';

            // zufällige Richtungen
            int[][] dirs = new int[][]
            {
            new []{ 0, 2 },
            new []{ 0,-2 },
            new []{ 2, 0 },
            new []{-2, 0 }
            };

            // random
            for (int i = 0; i < dirs.Length; i++)
            {
                int r = rnd.Next(dirs.Length);
                (dirs[i], dirs[r]) = (dirs[r], dirs[i]);
            }

            // in jede Richtung gehen
            for (int i = 0; i < dirs.Length; i++)
            {
                int[] d = dirs[i];

                int nx = x + d[1];
                int ny = y + d[0];

                if (ny > 0 && ny < map.GetLength(0) - 1 &&
                    nx > 0 && nx < map.GetLength(1) - 1 &&
                    map[ny, nx] == '#')
                {
                    // Wand zwischen den Zeichen öffnen
                    map[y + d[0] / 2, x + d[1] / 2] = '.';
                    LabyrinthWege(map, ny, nx);
                }
            }

        }

        // Methode zum Ausgeben
        public static void PrintDungeon(char[,] map)
        {
            int h = map.GetLength(0);
            int w = map.GetLength(1);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    char c = map[y, x];

                    if (c == '.') // Weg
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (c == '#') // Wand
                        Console.ForegroundColor = ConsoleColor.White;
                    else if (c == 'S') // Start
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (c == 'E') // Ende
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write(c);
                }

                Console.WriteLine();
            }

            Console.ResetColor();
        }


        static int EingabeZahl(string eingabe)
        {
            int zahl;   
            Console.Write(eingabe);
            while (!int.TryParse(Console.ReadLine(), out zahl))
            {
                Console.Write("Ungültige Eingabe, bitte erneut eingeben: ");
            }
            return zahl;
        }

    }
}
