using System;
using System.IO;

class DungeonGenerator
{
    static Random random = new Random(); // macht Zufallszahlen
    static bool dungeonFertig = false; // merkt ob der Dungeon fertig ist

    static void Main()
    {
        Start();
   
    }

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("1. Spiel Starten");
        Console.WriteLine("2. Dokumentation");
        Console.WriteLine("3. Grüner Nico");
        int Eingabe = EingabeZahl("Eingabe: ", 1, 3);
        switch (Eingabe)
        {
            case 1:
                Console.WriteLine("Nico Grüner");
                startDungeon();
                break;
            case 2:
                Console.WriteLine("Nico Grüner");
                Dokumentation();
                break;
            case 3:
                Console.WriteLine("Nico Grüner");
                Start();
                break;
            default:
                break;

        }
    }

    public static void Dokumentation()
    {
        Console.Clear();
        Console.WriteLine("--------------- DUNGEON DOKUMENTATION ---------------\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("In diesem Dungeon können folgende Elemente erscheinen:\n");

        // Wand
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("# ");
        Console.ResetColor();
        Console.WriteLine("= Wand – blockiert den Weg.");

        // Weg
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(". ");
        Console.ResetColor();
        Console.WriteLine("= Weg – begehbares Feld.");

        // Start
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("S ");
        Console.ResetColor();
        Console.WriteLine("= Startpunkt – hier beginnt der Spieler.");

        // Ende
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("E ");
        Console.ResetColor();
        Console.WriteLine("= Ausgang – Ziel des Dungeons.");

        // Truhe
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("T ");
        Console.ResetColor();
        Console.WriteLine("= Truhe – enthält Belohnungen.");

        // Falle
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("F ");
        Console.ResetColor();
        Console.WriteLine("= Falle – verursacht Schaden oder Nachteile.");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Hinweis:");
        Console.ResetColor();
        Console.WriteLine("Räume und Wege werden zufällig generiert. Truhen und Fallen haben eine Chance von 5% pro Wegfeld.\n");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Viel Spaß beim Erkunden des Dungeons!");
        Console.ResetColor();

        Console.WriteLine("\n-----------------------------------------------------\n");

        Console.WriteLine("1. Spiel Starten");
        Console.WriteLine("2. Main Menu");

        int Eingabe = EingabeZahl("Eingabe: ", 1, 2);
        switch (Eingabe)
        {
            case 1:
                Console.WriteLine("Nico Grüner");
                startDungeon();
                break;
            case 2:
                Console.WriteLine("Nico Grüner");
                Start();
                break;
            default:
                break;

        }

        Console.ReadLine();
        
    }

    public static void startDungeon()
    {
        Console.Clear();
        Willkommen(); // zeigt eine Begrüßung

        int höhe = EingabeZahl("Höhe (10–25): ", 10, 25);   // fragt die Höhe ab (min, max)
        int breite = EingabeZahl("Breite (10–50): ", 10, 50); // fragt die Breite ab (min, max)

        char[,] dungeon = GenerateDungeon(höhe, breite); // erstellt das Dungeon

        Console.WriteLine("\nDungeon wurde erfolgreich generiert und ausgegeben!\n");

        PrintDungeonColored(dungeon); // zeigt das Dungeon farbig
        dungeonFertig = true;         // Dungeon ist jetzt fertig

        if (dungeonFertig)
        {
            string antwort = EingabeJaNein("\nDungeon speichern? (j/n): "); // fragt ob gespeichert wird

            if (antwort == "j")
            {
                Console.Write("Dateiname ohne Endung: ");
                string dateiName = Console.ReadLine(); // setzt den Namen

                // prüft ob der Name länger als 20 Zeichen lang ist
                while (dateiName.Length > 20)
                {
                    Console.WriteLine("Der Name darf nur 20 Zeichen haben");
                    Console.Write("Bitte neuen Namen eingeben: ");
                    dateiName = Console.ReadLine(); // setzt neuen Namen
                }

                string speicherText = DungeonToString(dungeon); // macht den Dungeon zu Text
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // holt den Desktop Pfad

                File.WriteAllText(Path.Combine(desktopPath, dateiName + ".txt"), speicherText); // speichert die Datei

                Console.WriteLine("Dungeon wurde gespeichert!");
            } else if (antwort == "n")
            {
                Console.WriteLine("Dungeon wurde nicht gespeichert!\n");
                Console.WriteLine("1. Main Menu");
                Console.WriteLine("2. Beenden");
                int Eingabe = EingabeZahl("Eingabe: ", 1, 2);
                switch (Eingabe)
                {
                    case 1:
                        Console.WriteLine("Nico Grüner");
                        Start();
                        break;
                    case 2:
                        Console.WriteLine("Nico Grüner");
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        Console.ReadLine();
    }

    public static void Willkommen()
    {
        Console.WriteLine("---------------------------------------------------------");
        Console.WriteLine("           Willkommen zu diesem ZUFALLS-Dungeon          ");
        Console.WriteLine("---------------------------------------------------------\n");
        Console.WriteLine("Breite und Länge sind erforderlich.");
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

    public static char[,] GenerateDungeon(int height, int width)
    {
        if (height % 2 == 0) height++; // macht die Höhe ungerade
        if (width % 2 == 0) width++; // macht die Breite ungerade

        char[,] dungeon = new char[height, width]; // erstellt das Feld

        for (int y = 0; y < height; y++) // füllt alles mit Wänden
            for (int x = 0; x < width; x++)
                dungeon[y, x] = '#';

        // Anzahl der Räume abhängig von der Dungeon-Größe
        int basis = (width * height) / 50;        // Grundwert aus Fläche
        int raumAnzahl = Math.Min(20, basis);     // maximal 20
       
        // bisschen extra werte
        raumAnzahl += random.Next(-2, 3);

        // sicherstellen, dass es mindestens 3 Räume gibt
        raumAnzahl = Math.Max(3, raumAnzahl);

        for (int r = 0; r < raumAnzahl; r++)
        {
            // zufällige Größe
            int raumBreite = random.Next(2, 4);
            int raumHöhe = random.Next(2, 4);

            // zufällige Position
            int rx = random.Next(1, width - raumBreite - 1);
            int ry = random.Next(1, height - raumHöhe - 1);

            // Raum einzeichnen
            for (int y = ry; y < ry + raumHöhe; y++)
            {
                for (int x = rx; x < rx + raumBreite; x++)
                {
                    dungeon[y, x] = '.'; // macht den Raum frei
                }
            }
        }

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
        int abstand;

        do
        {
            ex = random.Next(1, width - 1); // zufällige x Position
            ey = random.Next(1, height - 1); // zufällige y Position

            // berechnet den Abstand zwischen Start und Ende
            abstand = Math.Abs(ex - sx) + Math.Abs(ey - sy);

        }
        while (dungeon[ey, ex] != '.' || abstand < 8); // prüft ob Weg und Abstand groß genug

        dungeon[ey, ex] = 'E'; // setzt das Ende

        // macht auf jedem Wegfeld eine 5 Prozent Chance für Truhe oder Falle
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (dungeon[y, x] == '.') // nur auf Wegen
                {
                    int chance = random.Next(100); // Zahl von 0 bis 99

                    if (chance < 5) // 5 Prozent Chance dass etwas passiert
                    {
                        int art = random.Next(2); // gibt 0 oder 1

                        if (art == 0)
                        {
                            dungeon[y, x] = 'T'; // Truhe
                        }
                        else
                        {
                            dungeon[y, x] = 'F'; // Falle
                        }
                    }
                }
            }
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
                else if (c == 'F') Console.ForegroundColor = ConsoleColor.DarkRed; // Falle
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