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
            int höhe;
            int breite;

            Console.WriteLine("Willkommen, bitte breite und länge eingeben: ");



            string eingabe = Console.ReadLine();
            while (!int.TryParse(eingabe, out höhe))
            {
                Console.Write("Erneut eingeben: ");
                eingabe = Console.ReadLine();
            }
            string eingabe2 = Console.ReadLine();
            while (!int.TryParse(eingabe2, out breite))
            {
                Console.Write("Erneut eingeben: ");
                eingabe2 = Console.ReadLine();
            }

            char[,] dunguen = new char[höhe, breite];

            FüllenMitWänden(dunguen);

            DungeonAusgeben(dunguen);
            Console.ReadLine();
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
