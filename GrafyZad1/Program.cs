using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GrafyZad1
{
    class Program
    {
        public static Regex nodeRegex = new Regex("^[0-9]*;$");
        public static Regex edgeRegex = new Regex("^[0-9]*\x20->\x20[0-9]*;$");
        static readonly string textFile = @"C:\Users\ASUS\Desktop\graph.dt"; //ścieżka do pliku 
        //public string[] stringSeparators = new string[] { " -> " };
        static void Main()
        {
            var graph = new Dictionary<int, List<int>>();
            if (File.Exists(textFile))
            {
                string[] lines = File.ReadAllLines(textFile);
                foreach (var line in lines)
                {
                    if (nodeRegex.IsMatch(line.ToString()))
                    {
                        graph.Add(int.Parse(line.Remove(1)), new List<int>());
                    }
                    else if (edgeRegex.IsMatch(line.ToString()))
                    {
                        //var numbers = line.Split(stringSeparator, StringSplitOptions.RemoveEmptyEntries)
                        var numbers = line.Split(" -> ");
                        numbers[1] = numbers[1].Remove(1);
                        _ = Int32.TryParse(numbers[0], out int vertice);
                        _ = Int32.TryParse(numbers[1], out int edges);
                        graph[vertice].Add(edges);
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
            List<int> wynik = new List<int>();
            foreach (var item in graph)
            {
                foreach (var item2 in graph)
                {
                    if (item2.Value.Contains(item.Key) && wynik.Contains(item.Key) == false)
                    {
                        wynik.Add(item.Key);
                    }
                }
            }
            List<int> temp = new List<int>();
            foreach (var item in graph)
            {
                if (wynik.Contains(item.Key))
                {
                    for (int i = 0; i < wynik.Count(); i++)
                    {
                        if (graph[item.Key].Contains(wynik[i]))
                        {
                            wynik.Remove(item.Key);
                            temp.Add(item.Key);
                        }
                    }
                }
            }

            for (int i = 0; i < wynik.Count; i++)
            {
                for (int j = 0; j < graph.Count; j++)
                {
                    if (temp[i] == j)
                    {
                        for (int k = 0; k < graph[j].Count; k++)
                        {
                            if (wynik.Contains(graph[j][k]) == false)
                            {
                                wynik.Add(temp[i]);
                            }
                        }
                    }
                }
            }

            foreach (var item in graph)
            {
                var warunek = true; 
                for (int j = 0; j < graph.Count; j++)
                {
                    for (int x = 0; x < graph[j].Count; x++)
                    {
                        if (graph[j][x] == item.Key)
                        {
                            warunek = true;
                        }
                        else
                        {
                            warunek = false;
                        }
                    }
                }
                if (warunek == false && graph[item.Key].Count == 0)
                {
                    wynik.Add(item.Key);
                }
            }
            foreach (var item in wynik)
                {
                    Console.Write(item + " ");
                }
            }
        }
    }
