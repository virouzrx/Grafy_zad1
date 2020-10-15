using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GrafyZad1
{
    public class GrafParsed
    {
        public static Dictionary<int, List<int>> graf = new Dictionary<int, List<int>>();
    }
    class Program
    {
        public static Stopwatch stopwatch = new Stopwatch();
        public static Regex nodeRegex = new Regex("^[0-9]*;$");
        public static Regex edgeRegex = new Regex("^[0-9]*\x20->\x20[0-9]*;$");
        static readonly string textFile = @"C:\Users\ASUS\Desktop\graph.dt";
        public static List<int> wynik = new List<int>();
        public static List<int> temp = new List<int>();

        public static void ParseGrafu()
        {
            List<int> wynik = new List<int>();
            List<int> temp = new List<int>();
            var tmp = GrafParsed.graf;
            if (File.Exists(textFile))
            {
                string[] tekst = File.ReadAllLines(textFile);
                foreach (var item in tekst)
                {
                    if (nodeRegex.IsMatch(item.ToString()))
                    {
                        tmp.Add(int.Parse(item.Remove(1)), new List<int>());
                    }
                    else if (edgeRegex.IsMatch(item.ToString()))
                    {
                        var numbers = item.Split(" -> ");
                        numbers[1] = numbers[1].Remove(1);
                        _ = Int32.TryParse(numbers[0], out int vertice);
                        _ = Int32.TryParse(numbers[1], out int edges);
                        tmp[vertice].Add(edges);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono pliku. Upewnij się, że podałeś właściwą ściężkę do pliku");
            }
        }
        public static void Dodawanie1()
        {
            foreach (var item in GrafParsed.graf)
            {
                foreach (var item2 in GrafParsed.graf)
                {
                    if (item2.Value.Contains(item.Key) && wynik.Contains(item.Key) == false)
                    {
                        wynik.Add(item.Key);
                    }
                }
            }
        }
        public static void Dodawanie2()
        {
            for (int i = 0; i < wynik.Count; i++)
            {
                for (int j = 0; j < GrafParsed.graf.Count; j++)
                {
                    if (temp[i] == j)
                    {
                        for (int k = 0; k < GrafParsed.graf[j].Count; k++)
                        {
                            if (wynik.Contains(GrafParsed.graf[j][k]) == false)
                            {
                                wynik.Add(temp[i]);
                            }
                        }
                    }
                }
            }
        }
        public static void Usuwanie()
        {
            foreach (var item in GrafParsed.graf)
            {
                if (wynik.Contains(item.Key))
                {
                    for (int i = 0; i < wynik.Count(); i++)
                    {
                        if (GrafParsed.graf[item.Key].Contains(wynik[i]))
                        {
                            wynik.Remove(item.Key);
                            temp.Add(item.Key);
                        }
                    }
                }
            }
        }

        public static void Szukanie()
        {
            foreach (var item in GrafParsed.graf)
            {
                var warunek = true;
                for (int j = 0; j < GrafParsed.graf.Count; j++)
                {
                    for (int x = 0; x < GrafParsed.graf[j].Count; x++)
                    {
                        if (GrafParsed.graf[j][x] == item.Key)
                        {
                            warunek = true;
                        }
                        else
                        {
                            warunek = false;
                        }
                    }
                }
                if (warunek == false && GrafParsed.graf[item.Key].Count == 0)
                {
                    wynik.Add(item.Key);
                }
            }
        }
        public static void Main()
        {
            stopwatch.Start();
            ParseGrafu();
            Dodawanie1();
            Usuwanie();
            Dodawanie2();
            Szukanie();
            stopwatch.Stop();

            foreach (var item in wynik)
            {
                Console.Write(item + " ");
            }
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00},{1:00}", ts.Seconds, ts.Milliseconds);
            Console.WriteLine("\nczas (w sekundach): " + elapsedTime);
        }
    }
}
