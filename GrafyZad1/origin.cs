using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Grafy
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, List<int>> graph = graph = readFromFile(args[0]);
            List<int> resultBuffer = new List<int>();
            for (int j = 0; j < graph.Count; j++)
            {
                for (int i = 0; i < graph.Count; i++)
                {
                    if (graph[i].Contains(j) && !resultBuffer.Contains(j))
                    {
                        resultBuffer.Add(j);
                        break;
                    }
                }
            }
            List<int> removedBuffer = new List<int>();

            for (int i = 0; i < graph.Count; i++)
            {
                if (resultBuffer.Contains(i))
                {
                    for (int j = 0; j < resultBuffer.Count; j++)
                    {
                        if (graph[i].Contains(resultBuffer[j]))
                        {
                            resultBuffer.Remove(i);
                            removedBuffer.Add(i);
                            break;
                        }
                    }
                }
            }
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
            int counter;
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
            resultBuffer.Sort();
            for (int i = 0; i < resultBuffer.Count; i++)
            {
                Console.Write($"{resultBuffer[i]} ");
            }
        }
        static Dictionary<int, List<int>> readFromFile(string path)
        {
            var lines = File.ReadAllLines(path)
                            .Skip(1);
            Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
            foreach (var line in lines)
            {
                if (line.Length <= 4 && line.Length > 1)
                {
                    graph.Add(int.Parse(line.Remove(line.Length - 1)), new List<int>());
                }
                else if (line.Length > 1)
                {
                    var numbers = line.Split(new string[] { " -> " }, StringSplitOptions.None);
                    numbers[1] = numbers[1].Remove(numbers[1].Length - 1);
                    graph[int.Parse(numbers[0])].Add(int.Parse(numbers[1]));
                }
            }
            return graph;
        }
    }
}
