using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CombAlg1
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = GetGraph();
            try
            {
                using (var streamWriter = new StreamWriter("./out.txt", false, System.Text.Encoding.Default))
                {
                    var bipartiteGraph = GetBipartiteGrapg(graph);
                    if (bipartiteGraph == null)
                        streamWriter.WriteLine("N");
                    else
                    {
                        streamWriter.WriteLine("Y");
                        var positiveAnsver = BuildPositiveAnswer(bipartiteGraph);
                        streamWriter.WriteLine(BuildAnswerString(positiveAnsver[0]));
                        streamWriter.WriteLine("0");
                        streamWriter.WriteLine(BuildAnswerString(positiveAnsver[1]));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        static Dictionary<int, Parity> GetBipartiteGrapg(Dictionary<int, int[]> graph)
        {
            var nodes = graph.Keys as IEnumerable<int>;
            var start = nodes.Min();
            var marked = new Dictionary<int, Parity>();
            var stack = new Stack<Tuple<int, Parity>>();
            Parity ReverseParity(Parity x) => x == Parity.Even ? Parity.Odd : Parity.Even;
            stack.Push(new Tuple<int, Parity>(start, Parity.Even));
            while (stack.Count != 0)
            {
                var node = stack.Peek().Item1;
                var parity = stack.Pop().Item2;
                if (!marked.ContainsKey(node))
                {
                    marked.Add(node, parity);
                    foreach (var neigbor in graph[node])
                        stack.Push(new Tuple<int, Parity>(neigbor, ReverseParity(parity)));
                }
                else if (marked[node] == parity)
                {
                    continue;
                }
                else return null;
                
            }
            return marked;
        }

        static int[][] BuildPositiveAnswer(Dictionary<int, Parity> bipartiteGraph)
        {
            var evenPart = new List<int>();
            var oddPart = new List<int>();
            foreach (var node in bipartiteGraph.Keys)
                if (bipartiteGraph[node] == Parity.Even)
                    evenPart.Add(node);
                else
                    oddPart.Add(node);
            return new int[][] { evenPart.OrderBy(i => i).ToArray(), oddPart.OrderBy(i => i).ToArray() };
        }

        static Dictionary<int, int[]> GetGraph()
        {
            var path = "./in.txt";
            var graph = new Dictionary<int, int[]>();
            try
            {
                using (var streamReader = new StreamReader(path))
                {
                    var count = int.Parse(streamReader.ReadLine());
                    for (int i = 1; i <= count; i++)
                    {
                        var neighbors = streamReader.ReadLine().Split(' ').Select(x => int.Parse(x)).ToList();
                        neighbors.RemoveAt(neighbors.Count - 1);
                        graph.Add(i, neighbors.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            return graph;
        }

        static string BuildAnswerString(int[] part)
        {
            var stringBuilder = new StringBuilder();
            foreach (var node in part)
            {
                stringBuilder.Append(node);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }
    }

    enum Parity
    {
        Even,
        Odd
    }
}
