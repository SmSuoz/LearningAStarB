using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace LearningAStarB
{
    class Map
    {
        private int width;
        private int height;
        private int[] cells;
        public Node[] nodes;

        private Dictionary<Node, Node> cameFrom;
        private Dictionary<Node, int> costSoFar;
        private PriorityQueue frontier;

        private Sprite sprite;

        public Map(int width, int height, int[] cells)
        {
            this.width = width;
            this.height = height;
            this.cells = cells;
            string path = "TextFile1.txt";
            nodes = new Node[cells.Length];

            sprite = new Sprite(1, 1);

            for (int i = 0; i < cells.Length; i++)
            {
                int x = i % width;
                int y = i / width;

                int cost;

                if (cells[i] <= 0)
                    cost = int.MaxValue;
                else
                    cost = cells[i];

                nodes[i] = new Node(x, y, cost);
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    if (nodes[index] == null)
                    {
                        continue;
                    }

                    // top
                    CheckNeighbour(nodes[index], x, y - 1);
                    // bottom
                    CheckNeighbour(nodes[index], x, y + 1);
                    // left
                    CheckNeighbour(nodes[index], x - 1, y);
                    // right
                    CheckNeighbour(nodes[index], x + 1, y);
                }
            }
        }

        private void CheckNeighbour(Node currNode, int cellX, int cellY)
        {
            if (cellX < 0 || cellX >= width)
            {
                return;
            }
            if (cellY < 0 || cellY >= height)
            {
                return;
            }
            int index = cellY * width + cellX;
            Node neighbour = nodes[index];
            if (neighbour != null)
            {
                currNode.AddNeighbour(neighbour);
            }
        }

        private int Heuristic(Node start, Node end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }

        private void AStar(Node start, Node end)
        {
            frontier = new PriorityQueue();
            cameFrom = new Dictionary<Node, Node>();
            costSoFar = new Dictionary<Node, int>();

            cameFrom[start] = start;
            costSoFar[start] = 0;
            frontier.Enqueue(start, Heuristic(start, end));

            while (!frontier.IsEmpty)
            {
                Node currNode = frontier.Dequeue();
                if (currNode == end)
                {
                    return;
                }
                foreach (Node nextNode in currNode.Neighbours)
                {
                    if (nextNode.Cost != int.MaxValue)
                    {
                        int newCost = costSoFar[currNode] + nextNode.Cost;

                        if (!costSoFar.ContainsKey(nextNode) || costSoFar[nextNode] > newCost)
                        {
                            costSoFar[nextNode] = newCost;
                            cameFrom[nextNode] = currNode;
                            frontier.Enqueue(nextNode, newCost + Heuristic(nextNode, end));
                        }
                    }
                }
            }
        }

        private Node GetNode(int x, int y)
        {
            if (x < 0 || x >= width)
                return null;
            if (y < 0 || y >= height)
                return null;
            return nodes[y * width + x];
        }

        // a partire dalle coordinate ricavi i nodi start e end **
        // utilizza a star per riempire il dictionary "cameFrom" **
        // calcola il path da percorrere a partire da "cameFrom" **
        // ritorna il path **
        public List<Node> GetPath(int startX, int startY, int endX, int endY)
        {
            List<Node> path = new List<Node>();
            Node start = GetNode(startX, startY);
            Node end = GetNode(endX, endY);
            if (start == null || end == null)
            {
                return path;
            }

            AStar(start, end);

            Node currNode = end;

            if (cameFrom.ContainsKey(currNode))
            {
                while (currNode != cameFrom[currNode])
                {
                    path.Add(currNode);
                    currNode = cameFrom[currNode];
                }
            }
            else
            {
                path.Clear();
                path.Add(start);
                return path;
            }

            path.Reverse();
            return path;
        }

        public void ChangeNode(int x, int y)
        {
            int index = y * width + x;
            if (nodes[index].Cost == int.MaxValue)
            {
                nodes[index].Cost = 1;
            }
            else
                nodes[index].Cost = int.MaxValue;


        }
        // Disegna la mappa
        // le celle == 0 sono nere
        // altrimenti sono bianche
        public void Draw()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sprite.position = new OpenTK.Vector2(x, y);
                    if (GetNode(x, y).Cost == int.MaxValue)
                    {
                        sprite.DrawSolidColor(0, 0, 0);
                    }
                    else
                    {
                        sprite.DrawSolidColor(255, 255, 255);
                    }
                }
            }
        }
    }
}
