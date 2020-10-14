using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Grafy
{
    //Andrzej Boba
    class Program
    {
        public static Regex nodeRegex = new Regex("^[0-9]*;$");
        public static Regex edgeRegex = new Regex("^[0-9]*\x20->\x20[0-9]*;$");
        static Dictionary<int, List<int>> ReadFromFile(string path)//odczytanie danych z pliku
        {
            var lines = File.ReadAllLines(path);
            Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
            foreach (var line in lines)
            {
                if (nodeRegex.IsMatch(line.ToString()))
                {
                    graph.Add(int.Parse(line.Remove(1)), new List<int>());
                }
                else if (edgeRegex.IsMatch(line.ToString()))
                {
                    var numbers = line.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                    numbers[1] = numbers[1].Remove(1);
                    graph[int.Parse(numbers[0])].Add(int.Parse(numbers[1]));
                }
                else
                {
                    continue;
                }
            }
            return graph;
        }
        static void Main(string[] args)
        {
            Dictionary<int, List<int>> graph = ReadFromFile(args[0]);
            //Dane są zapisane w postaci kolekcji kluczy i wartości, gdzie klucz to numer wierchołka a wartość to lista wierzchołków do których ten wierzchołek "wchodzi"
            List<int> resultBuffer = new List<int>();//kolekcja przetrzymująca wynik
            //usuwanie punktów które nie mogą być w zbiorze U ponieważ mają tylko wyjścia (warunek z krawędzią (v,u))
            for (int j = 0; j < graph.Count; j++)
            {
                for (int i = 0; i < graph.Count; i++)
                {
                    if (graph[i].Contains(j) && !resultBuffer.Contains(j))
                    {
                        resultBuffer.Add(j);
                        //break;
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
                            //break;
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
            for (int i = 0; i < resultBuffer.Count; i++)
            {
                Console.Write($"{resultBuffer[i]} ");
            }
        }

    }
}
