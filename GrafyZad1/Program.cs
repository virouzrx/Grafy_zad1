using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Grafy
{
    class Program
    {
        public static Regex nodeRegex = new Regex("^[0-9]*;$");
        public static Regex edgeRegex = new Regex("^[0-9]*\x20->\x20[0-9]*;$");
        static readonly string textFile = @"C:\Users\ASUS\Desktop\graph.dt"; //ścieżka do pliku 
        //public string[] stringSeparators = new string[] { " -> " };
        static void Main(string[] args)
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
            
            List<int> resultBuffer = new List<int>();
            //usuwanie punktów które nie mogą być w zbiorze U ponieważ mają tylko wyjścia (warunek z krawędzią (v,u))
            for (int j = 0; j < graph.Count; j++)
            {
                for (int i = 0; i < graph.Count; i++)
                {
                    if (graph[i].Contains(j) && resultBuffer.Contains(j) == false)
                    {
                        resultBuffer.Add(j);
                    }
                }
            }
            //wstępne usuwanie par 
            List<int> removedBuffer = new List<int>();//kolekcja przetrzymująca wierzchołki które początkowo były w rozwiązaniu ale później zostały usuniętę

            for (int i = 0; i < graph.Count; i++)
            {
                if (resultBuffer.Contains(i))
                {
                    for (int j = 0; j < resultBuffer.Count; j++)
                    {
                        if (graph[i].Contains(resultBuffer[j]))
                        {
                            resultBuffer.Remove(i);
                            removedBuffer.Add(i);//usunięte wierzchołki zapisujemy ponieważ nie wiemy czy nie usuneliśmy przez przypadek prawidłowego wierzchołka 
                        }
                    }
                }
            }

            //przywracanie wierzchołków które zostały błędnie usunięte przy usuwaniu par
            for (int i = 0; i < removedBuffer.Count; i++)
            {
                for (int j = 0; j < graph.Count; j++)
                {
                    if (removedBuffer[i] == j)
                    {
                        for (int x = 0; x < graph[j].Count; x++)
                        {
                            if (!resultBuffer.Contains(graph[j][x]))
                                resultBuffer.Add(removedBuffer[i]);
                        }
                    }
                }
            }
            //szukanie wierzchołków wiszących (tych które nie mają krawędzi)
            int counter;//licznik sprawdzający ilość połączeń wierzchoiłka z innymi wierzchołkami
            for (int i = 0; i < graph.Count; i++)
            {
                counter = 0;
                for (int j = 0; j < graph.Count; j++)
                {

                    for (int x = 0; x < graph[j].Count; x++)
                    {
                        if (graph[j][x] == i)
                        {
                            counter++;
                        }
                    }
                }
                if (counter == 0 && graph[i].Count == 0)
                {
                    resultBuffer.Add(i);
                }
            }
            //wyśwetlenie
          foreach (var element in resultBuffer)
            {
                Console.Write(element);
            }
        }

    }
}
