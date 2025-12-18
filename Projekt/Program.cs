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
            int höhe = EingabeZahl("Höhe: ");
            int breite = EingabeZahl("Breite: ");

            char[,] dunguen = new char[höhe, breite];

            FüllenMitWänden(dunguen);

            
            DungeonAusgeben(dunguen);
            Console.ReadLine();
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
    

        // Wände Füllen mit #
        static void FüllenMitWänden(char[,] karte)
        {
            for (int y = 0; y < karte.GetLength(0); y++)
                for (int x = 0; x < karte.GetLength(1); x++)
                    karte[y, x] = '#';
        }

        // Dunguen ausgeben
        static void DungeonAusgeben(char[,] karte)
        {
            for (int y = 0; y < karte.GetLength(0); y++)
            {
                for (int x = 0; x < karte.GetLength(1); x++)
                {
                    char feld = karte[y, x];
                    Console.Write(feld);
                }
                Console.WriteLine();
            }
        }
    }
}
