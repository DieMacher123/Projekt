using System;

class DungeonGenerator
{
    static Random rnd = new Random();

    static void Main()
    {

        Console.WriteLine("Willkommen, bitte breite und länge eingeben: ");

        // Eingabe der höhe und breite
        int höhe = EingabeZahl("Höhe: ");
        int breite = EingabeZahl("Breite: ");

        // 2D Array für Dungeon erstellen
        char[,] dungeon = GenerateDungeon(höhe, breite);

        // Ausgeben
        PrintDungeonColored(dungeon);

        Console.ReadLine();
    }


    // Dungeon erzeugen
    public static char[,] GenerateDungeon(int height, int width)
    {
        // Ungerade Maße erzwingen
        if (height % 2 == 0) height++;
        if (width % 2 == 0) width++;

        char[,] map = new char[height, width];

        // Alles mit Wänden füllen
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                map[y, x] = '#';

        // Maze erzeugen
        LabyrinthWege(map, 1, 1);

        // Start zufällig
        int sx, sy;
        do
        {
            sx = rnd.Next(1, width - 1);
            sy = rnd.Next(1, height - 1);
        }
        while (map[sy, sx] != '.');

        map[sy, sx] = 'S';

        // Ende zufällig
        int ex, ey;
        do
        {
            ex = rnd.Next(1, width - 1);
            ey = rnd.Next(1, height - 1);
        }
        while (map[ey, ex] != '.' || (ex == sx && ey == sy));

        map[ey, ex] = 'E';

        // Truhen setzen
        for (int i = 0; i < 5; i++)
            SetRandomChest(map, rnd);

        return map;
    }


    // Labyrinth Wege erstellen
    private static void LabyrinthWege(char[,] map, int y, int x)
    {
        map[y, x] = '.';

        int[][] dirs = new int[][]
        {
            new int[]{ 0, 2 },
            new int[]{ 0,-2 },
            new int[]{ 2, 0 },
            new int[]{-2, 0 }
        };

        // Richtungen mischen
        for (int i = 0; i < dirs.Length; i++)
        {
            int r = rnd.Next(dirs.Length);
            int[] temp = dirs[i];
            dirs[i] = dirs[r];
            dirs[r] = temp;
        }

        // In jede Richtung graben
        for (int i = 0; i < dirs.Length; i++)
        {
            int[] d = dirs[i];

            int nx = x + d[1];
            int ny = y + d[0];

            if (ny > 0 && ny < map.GetLength(0) - 1 &&
                nx > 0 && nx < map.GetLength(1) - 1 &&
                map[ny, nx] == '#')
            {
                // Wand zwischen den Zellen öffnen
                map[y + d[0] / 2, x + d[1] / 2] = '.';

                LabyrinthWege(map, ny, nx);
            }
        }
    }


    // Truhe setzen
    private static void SetRandomChest(char[,] map, Random rnd)
    {
        int w = map.GetLength(1);
        int h = map.GetLength(0);

        while (true)
        {
            int x = rnd.Next(1, w - 1);
            int y = rnd.Next(1, h - 1);

            if (map[y, x] == '.')
            {
                map[y, x] = 'T';
                return;
            }
        }
    }


    // Ausgabe Farbig ausgeben
    public static void PrintDungeonColored(char[,] map)
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                char c = map[y, x];

                if (c == '.')
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else if (c == '#')
                    Console.ForegroundColor = ConsoleColor.White;
                else if (c == 'S')
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (c == 'E')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (c == 'T')
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;

                Console.Write(c);
            }
            Console.WriteLine();
        }

        Console.ResetColor();
    }


    // Überprüfung der gültigen eingaben
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
