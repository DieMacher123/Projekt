using System;
using System.IO;
using System.Threading;

internal class DungeonGenerator
{
    // Zufall, Status und Objektchance für Truhen/Fallen
    private static Random random = new Random();
    private static bool dungeonFertig = false;
    private static int objektChance = 5;

    // Einstiegspunkt des Programms -> zeigt das Hauptmenü
    private static void Main()
    {
        Start();
    }

    // Hauptmenü des Spiels
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("1. Spiel Starten\n2. Dokumentation\n3. Beenden");

        int Eingabe = EingabeZahl("Eingabe: ", 1, 3);
        switch (Eingabe)
        {
            case 1: StartDungeon(); break;
            case 2: Dokumentation(); break;
            case 3: Beenden(); break;
        }
    }

    // Beenden des Programms mit kleiner Animation
    public static void Beenden()
    {
        Console.Clear();
        Console.Write("Das Programm wird beenden");
        for (int i = 0; i < 3; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }
        Environment.Exit(0);
    }

    // Hilfsmethoden für farbige Meldungen
    private static void InformationsMeldung(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private static void Fehlermeldung(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    // Hilfsmethode für Dokumentationszeilen (Symbol + Farbe + Erklärung)
    private static void DokuInfo(char symbol, ConsoleColor farbe, string text)
    {
        Console.ForegroundColor = farbe;
        Console.Write(symbol + " ");
        Console.ResetColor();
        Console.WriteLine("= " + text);
    }

    // Dokumentation des Dungeons (Erklärung aller Symbole)
    public static void Dokumentation()
    {
        Console.Clear();
        Console.WriteLine("--------------- DUNGEON DOKUMENTATION ---------------\n");
        Console.WriteLine("In diesem Dungeon können folgende Elemente erscheinen:\n");

        DokuInfo('#', ConsoleColor.White, "Wand – blockiert den Weg.");
        DokuInfo('.', ConsoleColor.DarkGray, "Weg – begehbares Feld.");
        DokuInfo('S', ConsoleColor.Green, "Startpunkt – hier startet es.");
        DokuInfo('E', ConsoleColor.Red, "Ausgang – Ziel des Dungeons.");
        DokuInfo('T', ConsoleColor.Yellow, "Truhe – enthält Belohnungen.");
        DokuInfo('F', ConsoleColor.DarkRed, "Falle – verursacht Schaden oder Nachteile.");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Hinweis:");
        Console.ResetColor();
        Console.WriteLine("Räume und Wege werden zufällig generiert. Truhen und Fallen haben eine Chance von 5% pro Wegfeld.\n");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Viel Spaß beim Erkunden des Dungeons!");
        Console.ResetColor();
        Console.WriteLine("\n-----------------------------------------------------\n");

        Console.WriteLine("1. Spiel Starten\n2. Hauptmenü");

        int Eingabe = EingabeZahl("Eingabe: ", 1, 2); // min 1, max 2
        switch (Eingabe)
        {
            case 1: StartDungeon(); break;
            case 2: Start(); break;
        }

        Console.ReadLine();
    }

    // Hauptfunktion zum Erstellen, Anzeigen und Speichern eines Dungeons
    public static void StartDungeon()
    {
        Console.Clear();
        Willkommen();

        // Eingaben für Dungeon-Größe und Objektchance
        int höhe = EingabeZahl("Höhe (10–25): ", 10, 25);
        int breite = EingabeZahl("Breite (10–50): ", 10, 50);
        objektChance = EingabeZahl("Object chance (1-100): ", 1, 100);

        // Dungeon generieren und farbig ausgeben
        char[,] dungeon = GenerateDungeon(höhe, breite);
        Console.WriteLine("");
        FarbigeAusgabe(dungeon);
        dungeonFertig = true;

        InformationsMeldung($"\nDungeon wurde erfolgreich generiert!\nHöhe: {höhe}\nBreite: {breite}\nObjekt chance: {objektChance}%");

        // Speichern des Dungeons
        if (dungeonFertig)
        {
            string antwort = EingabeJaNein("\nDungeon speichern? (Ja/Nein): ");

            if (antwort.ToLower() == "ja")
            {
                // Dateiname einlesen und prüfen
                Console.Write("Datei name: ");
                char[] ungültig = Path.GetInvalidFileNameChars();
                string dateiName = Console.ReadLine();

                while (dateiName.IndexOfAny(ungültig) >= 0 || dateiName.Length > 200)
                {
                    if (dateiName.Length > 200) Fehlermeldung("Der Name darf nur 20 Zeichen haben!");
                    else Fehlermeldung("Keine Sonderzeichen!");

                    Console.Write("Bitte neuen Datei Namen eingeben: ");
                    dateiName = Console.ReadLine();
                }

                // Pfad vorbereiten
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string pfad = Path.Combine(desktopPath, dateiName + ".txt");

                string gespeichertesDungeon = "";

                // Datei existiert bereits -> nachfragen
                if (File.Exists(pfad))
                {
                    InformationsMeldung("Duplikat gefunden!");
                    string überschreiben = EingabeJaNein("trotzdem Speichern? (Ja/Nein): ");

                    if (überschreiben.ToLower() == "ja")
                    {
                        gespeichertesDungeon = ArrayToText(dungeon);
                        File.WriteAllText(pfad, gespeichertesDungeon);
                        InformationsMeldung("Dungeon wurde gespeichert!");
                    }
                    else
                    {
                        InformationsMeldung("Dungeon wurde nicht gespeichert!");
                    }
                }
                else
                {
                    // Datei existiert nicht -> normal speichern
                    gespeichertesDungeon = ArrayToText(dungeon);
                    File.WriteAllText(pfad, gespeichertesDungeon);
                    InformationsMeldung("Dungeon wurde gespeichert!");
                }
            }
            else
            {
                InformationsMeldung("Dungeon wurde nicht gespeichert!");
            }

            // Nach dem Speichern zurück ins Menü oder beenden
            Console.WriteLine("\n1. Hauptmenü\n2. Beenden");
            int Eingabe = EingabeZahl("Eingabe: ", 1, 2);

            switch (Eingabe)
            {
                case 1: Start(); break;
                case 2: Beenden(); break;
            }
        }

        Console.ReadLine();
    }

    // Begrüßungstext beim Start des Dungeons
    public static void Willkommen()
    {
        Console.WriteLine("---------------------------------------------------------\n" +
                          "           Willkommen zu diesem ZUFALLS-Dungeon          \n" +
                          "---------------------------------------------------------\n");
    }

    // Dungeon Generator: Räume, Wege, Start, Ende, Objekte
    public static char[,] GenerateDungeon(int height, int width)
    {
        // Größe anpassen (ungerade Werte um doppelte wänden entgegenzuwirken)
        if (height % 2 == 0) height++;
        if (width % 2 == 0) width++;

        // Dungeon mit Wänden füllen
        char[,] dungeon = new char[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                dungeon[y, x] = '#';

        // Räume erzeugen
        int anzahl = (width * height) / 50;
        int raumAnzahl = Math.Min(20, anzahl);
        raumAnzahl += random.Next(-2, 3);
        raumAnzahl = Math.Max(3, raumAnzahl);

        for (int r = 0; r < raumAnzahl; r++)
        {
            int raumBreite = random.Next(2, 4);
            int raumHöhe = random.Next(2, 4);
            int rx = random.Next(1, width - raumBreite - 1);
            int ry = random.Next(1, height - raumHöhe - 1);

            for (int y = ry; y < ry + raumHöhe; y++)
                for (int x = rx; x < rx + raumBreite; x++)
                    dungeon[y, x] = '.';
        }

        // Labyrinth erzeugen
        LabyrinthWege(dungeon, 1, 1);

        // Startpunkt setzen
        int sx, sy;
        do
        {
            sx = random.Next(1, width - 1);
            sy = random.Next(1, height - 1);
        }
        while (dungeon[sy, sx] != '.');
        dungeon[sy, sx] = 'S';

        // Endpunkt setzen (mit Mindestabstand)
        int ex, ey, abstand;
        do
        {
            ex = random.Next(1, width - 1);
            ey = random.Next(1, height - 1);
            abstand = Math.Abs(ex - sx) + Math.Abs(ey - sy);
        }
        while (dungeon[ey, ex] != '.' || abstand < 8);
        dungeon[ey, ex] = 'E';

        // Truhen und Fallen verteilen
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (dungeon[y, x] == '.' && random.Next(100) < objektChance)
                    dungeon[y, x] = random.Next(2) == 0 ? 'T' : 'F';

        return dungeon;
    }

    // Rekursiver Maze Generator (Depth-First-Search algorythmus)
    private static void LabyrinthWege(char[,] dungeon, int y, int x)
    {
        dungeon[y, x] = '.';

        int[][] dirs =
        {
            new int[]{ 0, 2 },
            new int[]{ 0,-2 },
            new int[]{ 2, 0 },
            new int[]{-2, 0 }
        };

        // Richtungen mischen
        for (int i = 0; i < dirs.Length; i++)
        {
            int r = random.Next(dirs.Length);
            int[] temp = dirs[i];
            dirs[i] = dirs[r];
            dirs[r] = temp;
        }

        // Punkte verbinden
        for (int i = 0; i < dirs.Length; i++)
        {
            int[] d = dirs[i];
            int nx = x + d[1];
            int ny = y + d[0];

            if (ny > 0 && ny < dungeon.GetLength(0) - 1 &&
                nx > 0 && nx < dungeon.GetLength(1) - 1 &&
                dungeon[ny, nx] == '#')
            {
                dungeon[y + d[0] / 2, x + d[1] / 2] = '.';
                LabyrinthWege(dungeon, ny, nx);
            }
        }
    }

    // Gibt den Dungeon farbig in der Konsole aus
    public static void FarbigeAusgabe(char[,] dungeon)
    {
        for (int y = 0; y < dungeon.GetLength(0); y++)
        {
            for (int x = 0; x < dungeon.GetLength(1); x++)
            {
                char c = dungeon[y, x];

                switch (c)
                {
                    case '.': Console.ForegroundColor = ConsoleColor.DarkGray; break;
                    case '#': Console.ForegroundColor = ConsoleColor.White; break;
                    case 'S': Console.ForegroundColor = ConsoleColor.Green; break;
                    case 'E': Console.ForegroundColor = ConsoleColor.Red; break;
                    case 'T': Console.ForegroundColor = ConsoleColor.Yellow; break;
                    case 'F': Console.ForegroundColor = ConsoleColor.DarkRed; break;
                    default: Console.ForegroundColor = ConsoleColor.Gray; break;
                }

                Console.Write(c);
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    // Liest eine gültige Zahl ein
    private static int EingabeZahl(string text, int min, int max)
    {
        Console.Write(text);
        string eingabe = Console.ReadLine();
        int zahl;

        while (!int.TryParse(eingabe, out zahl) || zahl < min || zahl > max)
        {
            Fehlermeldung($"Bitte eine Zahl zwischen {min} und {max} eingeben.");
            Console.Write(text);
            eingabe = Console.ReadLine();
        }

        return zahl;
    }

    // Liest Ja/Nein ein
    private static string EingabeJaNein(string text)
    {
        Console.Write(text);
        string eingabe = Console.ReadLine().Trim().ToLower();

        while (eingabe != "ja" && eingabe != "nein")
        {
            Fehlermeldung("Bitte nur Ja oder Nein eingeben.");
            Console.Write(text);
            eingabe = Console.ReadLine().Trim().ToLower();
        }

        return eingabe;
    }

    // Wandelt das Dungeon Array in Text um (für das Speichern)
    private static string ArrayToText(char[,] dungeon)
    {
        string dungeonText = "";

        for (int y = 0; y < dungeon.GetLength(0); y++)
        {
            for (int x = 0; x < dungeon.GetLength(1); x++)
                dungeonText += dungeon[y, x];

            dungeonText += "\n";
        }

        return dungeonText;
    }
}
