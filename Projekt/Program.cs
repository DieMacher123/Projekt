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
        // Ungerade Länge und höhe des Dungeons erzwingen
        if (height % 2 == 0) height++;
        if (width % 2 == 0) width++;

        char[,] dungeon = new char[height, width];

        // Alles mit Wänden füllen
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                dungeon[y, x] = '#';

        // Maze erzeugen
        LabyrinthWege(dungeon, 1, 1);

        // Start zufällig
        int sx, sy;
        do
        {
            sx = rnd.Next(1, width - 1);
            sy = rnd.Next(1, height - 1);
        }
        while (dungeon[sy, sx] != '.');

        dungeon[sy, sx] = 'S';

        // Ende zufällig
        int ex, ey;
        do
        {
            ex = rnd.Next(1, width - 1);
            ey = rnd.Next(1, height - 1);
        }
        while (dungeon[ey, ex] != '.' || (ex == sx && ey == sy));

        dungeon[ey, ex] = 'E';

        // Truhen setzen
        for (int i = 0; i < 5; i++) { 
            SetRandomChest(dungeon, rnd);
        }
        return dungeon;
    }


    // Labyrinth Wege erstellen
    private static void LabyrinthWege(char[,] dungeon, int y, int x)
    {
        dungeon[y, x] = '.';

        // mögliche rintungen (dirs)
        int[][] dirs = new int[][]
        {
            new int[]{ 0, 2 },
            new int[]{ 0,-2 },
            new int[]{ 2, 0 },
            new int[]{-2, 0 }
        };

        // Richtungen zufällig generieren
        for (int i = 0; i < dirs.Length; i++)
        {
            int r = rnd.Next(dirs.Length);
            int[] temp = dirs[i];
            dirs[i] = dirs[r];
            dirs[r] = temp;
        }

        // In jede Richtung gehen
        for (int i = 0; i < dirs.Length; i++)
        {
            // Aktuelle Richtung aus dem Array holen (dy, dx)
            int[] d = dirs[i];

            // Neue Zielposition generieren (2 Felder weiter)
            int nx = x + d[1]; // neue x Position
            int ny = y + d[0]; // neue y Position

            // Prüfen ob die neue Position gültig ist ->
            // Im Spielfeld? Ja/Nein
            // Ist da eine Wand? Ja/Nein
            if (ny > 0 && ny < dungeon.GetLength(0) - 1 &&
                nx > 0 && nx < dungeon.GetLength(1) - 1 &&
                dungeon[ny, nx] == '#')
            {
                // Die Wand zwischen aktueller Position und neuer Position entfernen
                // Beispiel: von (5,5) nach (5,7) -> Wand ist bei (5,6)
                dungeon[y + d[0] / 2, x + d[1] / 2] = '.';

                LabyrinthWege(dungeon, ny, nx);
            }
        }

    }


    // Truhe setzen
    private static void SetRandomChest(char[,] dungeon, Random rnd)
    {
        // Größe des dungeons
        int w = dungeon.GetLength(1);
        int h = dungeon.GetLength(0);

        while (true)
        {
            int x = rnd.Next(1, w - 1);
            int y = rnd.Next(1, h - 1);

            if (dungeon[y, x] == '.')
            {
                dungeon[y, x] = 'T';
                return;
            }
        }
    }


    // Ausgabe Farbig ausgeben
    public static void PrintDungeonColored(char[,] dungeon)
    {
        for (int y = 0; y < dungeon.GetLength(0); y++)
        {
            for (int x = 0; x < dungeon.GetLength(1); x++)
            {
                char c = dungeon[y, x];

                if (c == '.') // Weg
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else if (c == '#') // Wand
                    Console.ForegroundColor = ConsoleColor.White;
                else if (c == 'S') // Start
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (c == 'E') // Ende
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (c == 'T') // Truhe
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
