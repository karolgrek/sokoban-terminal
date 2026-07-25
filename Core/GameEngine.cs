using System;
using System.Linq;
using System.Collections.Generic;
using Sokoban.Models;

namespace Sokoban.Core
{
    public class GameEngine
    {
        private static readonly char[] AllowedMoves = {'a', 's', 'w', 'd', 'x', 'r', 'q'};
        private List<GameObject> objs;
        private List<List<GameObject>> board;
        private string levelName;

        public GameEngine(string[] map, string levelName = "")
        {
            this.levelName = levelName;
            int height = map.Length;
            int width = 0;

            objs = new List<GameObject>();

            for (int y = 0; y < map.Length; y++)
            {
                width = Math.Max(width, map[y].Length);
                for (int x = 0; x < map[y].Length; x++)
                {
                    Position newPosition = new Position(x, y);

                    switch (map[y][x])
                    {
                        case '#': objs.Add(new Wall(newPosition)); break;
                        case '.': objs.Add(new Target(newPosition)); break;
                        case '^': objs.Add(new Agent(newPosition, Direction.North)); break;
                        case '>': objs.Add(new Agent(newPosition, Direction.East)); break;
                        case 'v': objs.Add(new Agent(newPosition, Direction.South)); break;
                        case '<': objs.Add(new Agent(newPosition, Direction.West)); break;
                        case 'o': objs.Add(new Player(newPosition)); break;
                        case 'X': objs.Add(new Crate(newPosition)); break;
                        case 'x': objs.Add(new Crate(newPosition)); objs.Add(new Target(newPosition)); break;
                    }
                }
            }

            board = new List<List<GameObject>>();
            for (int y = 0; y < height; y++) {
                List<GameObject> row = new List<GameObject>();
                for (int x = 0; x < width; x++)
                    row.Add(null!);
                board.Add(row);
            }

            SetObjectsToBoard();
        }

        private void SetObjectsToBoard()
        {
            for (int y = 0; y < board.Count; y++)
            {
                for (int x = 0; x < board[0].Count; x++)
                    board[y][x] = null!;
            }

            foreach (GameObject obj in objs.Where(o => !(o is Target)))
            {
                board[obj.Position.y][obj.Position.x] = obj;
            }

            foreach (GameObject obj in objs.Where(o => o is Target))
            {
                if (board[obj.Position.y][obj.Position.x] == null)
                    board[obj.Position.y][obj.Position.x] = obj;
            }
        }

        public bool GameEnded()
        {
            List<Position> cratePositions = objs.Where(o => o is Crate).Select(c => c.Position).ToList();
            List<Position> targetPositions = objs.Where(o => o is Target).Select(t => t.Position).ToList();
            return cratePositions.Count == targetPositions.Count && cratePositions.All(targetPositions.Contains);
        }

        public void ShowBoard()
        {
            Console.Clear();
            if (!string.IsNullOrEmpty(levelName))
            {
                Console.WriteLine($"=== {levelName} ===");
                Console.WriteLine();
            }

            foreach (var row in board)
            {
                foreach(GameObject obj in row)
                {
                    if (obj == null)
                        Console.Write(" ");
                    else
                        Console.Write(obj.Repr());
                    Console.Write(" ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("\nMove: W,A,S,D | Restart: R | Quit: Q");
        }

        private char GetNextPlayerMove()
        {
            char input = Console.ReadKey().KeyChar;
            while (!AllowedMoves.Contains(input))
            {
                Console.WriteLine("Allowed moves are {0}", String.Join(", ", AllowedMoves));
                input = Console.ReadKey().KeyChar;
            }
            return input;
        }

        private Direction? MoveToDirection(char move)
        {
            switch (move)
            {
                case 'w': return Direction.North;
                case 'd': return Direction.East;
                case 's': return Direction.South;
                case 'a': return Direction.West;
                case 'x': return null;
            }
            return null; // unreachable
        }

        public void SetPlayerDirection(Direction? dir)
        {
            foreach (var obj in objs)
            {
                if (obj is Player p)
                    p.SetDirection(dir);
            }
        }

        private GameObject GetFromBoard(Position pos)
        {
            if (0 <= pos.x && pos.x < board[0].Count && 0 <= pos.y && pos.y < board.Count)
                return board[pos.y][pos.x];
            return null!;
        }

        public Dictionary<Direction, GameObject> GetNeighborhood(Position pos)
        {
            var neighborhood = new Dictionary<Direction, GameObject>();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                neighborhood[dir] = GetFromBoard(pos.Step(dir));
            return neighborhood;
        }

        public bool AgentsCollide()
        {
            var nextPositions = new HashSet<Position>();

            foreach (GameObject obj in objs)
            {
                if (obj is Agent a)
                {
                    // 1. Check if they bump into an adjacent agent directly
                    foreach (var entry in GetNeighborhood(a.Position))
                    {
                        if (entry.Value is Agent o) {
                            if (a.Direction != null && entry.Key == a.Direction.Value)
                                return true;
                        }
                    }

                    // 2. Predict next position to check for same-tile collision (moving into same empty space)
                    Position nextPos = a.Position;
                    if (a.Direction != null)
                    {
                        var frontObj = GetFromBoard(a.Position.Step(a.Direction.Value));
                        if (frontObj == null || frontObj is Target)
                        {
                            nextPos = a.Position.Step(a.Direction.Value);
                        }
                    }

                    if (nextPositions.Contains(nextPos))
                        return true;

                    nextPositions.Add(nextPos);
                }
            }
            return false;
        }

        public void Update()
        {
            foreach (GameObject obj in objs.Where(o => o is Crate))
            {
                obj.Update(GetNeighborhood(obj.Position));
            }
            SetObjectsToBoard();

            foreach (GameObject obj in objs.Where(o => !(o is Crate)))
            {
                obj.Update(GetNeighborhood(obj.Position));
            }
            SetObjectsToBoard();
        }

        public void Reset()
        {
            foreach (GameObject obj in objs)
                obj.Reset();
            SetObjectsToBoard();
        }

        public void GameLoop()
        {
            while (!GameEnded())
            {
                ShowBoard();

                char move = GetNextPlayerMove();
                if (move == 'r') {
                    Reset();
                    continue;
                } 
                else if (move == 'q') {
                    return;
                }
                SetPlayerDirection(MoveToDirection(move));

                if (AgentsCollide())
                    break;

                Update();
            }

            ShowBoard();
            if (AgentsCollide())
                Console.WriteLine("Agents collided.");
            else
                Console.WriteLine("Solved!");
        }
    }
}
