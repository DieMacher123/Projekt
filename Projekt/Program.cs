using System;
using System.IO;
using System.Security.Policy;

class DungeonGenerator
{
    static Random random = new Random(); // macht Zufallszahlen
    static bool dungeonFertig = false; // merkt ob der Dungeon fertig ist

    static void Main()
    {
        Willkommen(); // zeigt eine Begrüßung

        int höhe = EingabeZahl("Höhe (10–25): ", 10, 25); // fragt die Höhe ab (min, max)
        int breite = EingabeZahl("Breite (10–50): ", 10, 50); // fragt die Breite ab (min, max)

        char[,] dungeon = GenerateDungeon(höhe, breite); // erstellt das Dungeon

        Console.WriteLine("\nDungeon wurde erfolgreich generiert und ausgegeben!\n");

        PrintDungeonColored(dungeon); // zeigt das Dungeon farbig
        dungeonFertig = true; // Dungeon ist jetzt fertig

        if (dungeonFertig)
        {
            string antwort = EingabeJaNein("\nDungeon speichern? (j/n): "); // fragt ob gespeichert wird

            if (antwort == "j")
            {
                Console.Write("Dateiname ohne Endung: ");
                string dateiName = Console.ReadLine(); // setzt den namen

                // prüft ob der Name länger als 20 Zeichen lang ist
                while (dateiName.Length > 20)
                {
                    Console.WriteLine("Der Name darf nur 20 Zeichen haben");
                    Console.Write("Bitte neuen Namen eingeben: ");
                    dateiName = Console.ReadLine(); // setzt neuen namen
                }

                string speicherText = DungeonToString(dungeon); // macht den Dungeon zu Text
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // holt den Desktop Pfad

                File.WriteAllText(Path.Combine(desktopPath, dateiName + ".txt"), speicherText); // speichert die Datei

                Console.WriteLine("Dungeon wurde gespeichert!");
            }

        }

        Console.ReadLine();
    }

    static string EingabeJaNein(string text)
    {
        Console.Write(text); // zeigt die Frage
        string eingabe = Console.ReadLine().Trim().ToLower(); // speichert die Antwort

        while (eingabe != "j" && eingabe != "n") // prüft ob die Antwort gültig ist
        {
            Console.WriteLine("Bitte nur j oder n eingeben.");
            Console.Write(text);
            eingabe = Console.ReadLine().Trim().ToLower();
        }

        return eingabe; // gibt die Antwort zurück
    }

    static string DungeonToString(char[,] dungeon)
    {
        string dungeonText = ""; // speichert den Text

        for (int y = 0; y < dungeon.GetLength(0); y++) // geht jede Zeile durch
        {
            for (int x = 0; x < dungeon.GetLength(1); x++) // geht jede Spalte durch
            {
                dungeonText = dungeonText + dungeon[y, x]; // fügt das Zeichen hinzu
            }
            dungeonText = dungeonText + "\n"; // macht eine neue Zeile
        }

        return dungeonText; // gibt den Text zurück
    }

    public static void Willkommen()
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("      Willkommen zu diesem Dungeon        ");
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Breite und Länge sind erforderlich.");
    }

    public static char[,] GenerateDungeon(int height, int width)
    {
        if (height % 2 == 0) height++; // macht die Höhe ungerade
        if (width % 2 == 0) width++; // macht die Breite ungerade

        char[,] dungeon = new char[height, width]; // erstellt das Feld

        for (int y = 0; y < height; y++) // füllt alles mit Wänden
            for (int x = 0; x < width; x++)
                dungeon[y, x] = '#';

        LabyrinthWege(dungeon, 1, 1); // startet das Labyrinth

        int sx, sy; // Start Position
        do
        {
            sx = random.Next(1, width - 1); // zufällige X Position
            sy = random.Next(1, height - 1); // zufällige Y Position
        }
        while (dungeon[sy, sx] != '.'); // nur auf einem Weg erlaubt

        dungeon[sy, sx] = 'S'; // setzt den Start

        int ex, ey; // Ende Position
        do
        {
            ex = random.Next(1, width - 1);
            ey = random.Next(1, height - 1);
        }
        while (dungeon[ey, ex] != '.' || (ex == sx && ey == sy)); // darf nicht Start sein

        dungeon[ey, ex] = 'E'; // setzt das Ende

        for (int i = 0; i < 5; i++) // setzt fünf Truhen
        {
            SetRandomChest(dungeon, random);
        }

        return dungeon; // gibt den Dungeon zurück
    }

    private static void LabyrinthWege(char[,] dungeon, int y, int x)
    {
        dungeon[y, x] = '.'; // macht einen Weg

        int[][] dirs = new int[][] // mögliche Richtungen
        {
            new int[]{ 0, 2 },
            new int[]{ 0,-2 },
            new int[]{ 2, 0 },
            new int[]{-2, 0 }
        };

        for (int i = 0; i < dirs.Length; i++) // mischt die Richtungen
        {
            int r = random.Next(dirs.Length);
            int[] temp = dirs[i];
            dirs[i] = dirs[r];
            dirs[r] = temp;
        }

        for (int i = 0; i < dirs.Length; i++) // geht jede Richtung durch
        {
            int[] d = dirs[i]; // nimmt eine Richtung

            int nx = x + d[1]; // neues x
            int ny = y + d[0]; // neues y

            if (ny > 0 && ny < dungeon.GetLength(0) - 1 &&
                nx > 0 && nx < dungeon.GetLength(1) - 1 &&
                dungeon[ny, nx] == '#') // prüft ob dort eine Wand ist
            {
                dungeon[y + d[0] / 2, x + d[1] / 2] = '.'; // macht die Wand weg

                LabyrinthWege(dungeon, ny, nx); // geht weiter
            }
        }
    }

    private static void SetRandomChest(char[,] dungeon, Random rnd)
    {
        int w = dungeon.GetLength(1); // Breite
        int h = dungeon.GetLength(0); // Höhe

        while (true) // sucht einen Platz
        {
            int x = rnd.Next(1, w - 1); // zufällige x Position
            int y = rnd.Next(1, h - 1); // zufällige y Position

            if (dungeon[y, x] == '.') // nur auf Wegen erlaubt
            {
                dungeon[y, x] = 'T'; // setzt die Truhe
                return; // fertig
            }
        }
    }

    public static void PrintDungeonColored(char[,] dungeon)
    {
        for (int y = 0; y < dungeon.GetLength(0); y++) // geht jede Zeile durch
        {
            for (int x = 0; x < dungeon.GetLength(1); x++) // geht jede Spalte durch
            {
                char c = dungeon[y, x]; // holt das Zeichen

                if (c == '.') Console.ForegroundColor = ConsoleColor.DarkGray; // Weg
                else if (c == '#') Console.ForegroundColor = ConsoleColor.White; // Wand
                else if (c == 'S') Console.ForegroundColor = ConsoleColor.Green; // Start
                else if (c == 'E') Console.ForegroundColor = ConsoleColor.Red; // Ende
                else if (c == 'T') Console.ForegroundColor = ConsoleColor.Yellow; // Truhe
                else Console.ForegroundColor = ConsoleColor.Gray; // alles andere

                Console.Write(c); // zeigt das Zeichen
            }
            Console.WriteLine(); // neue Zeile
        }

        Console.ResetColor(); // setzt die Farbe zurück
    }

    static int EingabeZahl(string text, int min, int max)
    {
        Console.Write(text); // zeigt die Frage
        string input = Console.ReadLine(); // liest die Eingabe
        int zahl;

        while (!int.TryParse(input, out zahl) || zahl < min || zahl > max) // prüft die Zahl
        {
            Console.WriteLine($"Bitte eine Zahl zwischen {min} und {max} eingeben.");
            Console.Write(text);
            input = Console.ReadLine();
        }

        return zahl; // gibt die gültige Zahl zurück
    }
}
