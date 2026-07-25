using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sokoban.Models;

namespace Sokoban.Tests
{
    public class GameTests
    {
        private static Dictionary<Direction, GameObject> EmptyNeighborhood()
        {
            return new Dictionary<Direction, GameObject>
            {
                { Direction.North, null! },
                { Direction.East,  null! },
                { Direction.South, null! },
                { Direction.West,  null! }
            };
        }

        public static void RunAll()
        {
            Test_StaticObjectWall();
            Test_Player();
            Test_Crate();
            Test_Reset();
     
            Console.WriteLine("All tests passed.");
        }

        private static void Test_StaticObjectWall()
        {
            StaticObject staticObjectWall = new Wall(new Position(0, 0));

            Debug.Assert(staticObjectWall.Position.Equals(new Position(0, 0)));
            Debug.Assert(staticObjectWall.Repr() == '#');
            staticObjectWall.Update(EmptyNeighborhood());
            Debug.Assert(staticObjectWall.IsMoving() == false);

            GameObject gameObjectWall = staticObjectWall;

            Debug.Assert(gameObjectWall.Position.Equals(new Position(0, 0)));
            Debug.Assert(gameObjectWall.Repr() == '#');
            gameObjectWall.Update(EmptyNeighborhood());
            Debug.Assert(gameObjectWall.IsMoving() == null);
        }

        private static void Test_Player()
        {
            var empty = EmptyNeighborhood();
            Player player = new Player(new Position(0, 0));

            Debug.Assert(player.Repr() == 'o');
            player.Update(empty);
            Debug.Assert(player.IsMoving() == false);
            Debug.Assert(player.Position.Equals(new Position(0, 0)));

            player.SetDirection(Direction.North);
            Debug.Assert(player.Repr() == '^');
            Debug.Assert(player.IsMoving() == true);

            player.Update(empty);
            Debug.Assert(player.Position.Equals(new Position(0, -1)));

            var targetToNorth = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.North] = new Target(new Position(0, -2))
            };
            player.Update(targetToNorth);
            Debug.Assert(player.Position.Equals(new Position(0, -2)));

            var wallToNorth = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.North] = new Wall(new Position(0, -3))
            };
            player.Update(wallToNorth);
            Debug.Assert(player.Position.Equals(new Position(0, -2)));
            Debug.Assert(player.Direction == Direction.South);
        }

        private static void Test_Crate()
        {
            var empty = EmptyNeighborhood();
            
            Player player = new Player(new Position(0, 0));
            player.SetDirection(Direction.North);
            Crate crate = new Crate(new Position(1, -2));
            
            var agentToWestFacingAway = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.West] = player 
            };
            
            player.SetDirection(Direction.South);
            crate.Update(agentToWestFacingAway);
            
            Debug.Assert(crate.Position.Equals(new Position(1, -2)), "Crate should stay in place");

            var agentToNorthFacingTowards = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.North] = player
            };
            
            player.SetDirection(Direction.South);
            crate.Update(agentToNorthFacingTowards);
            
            Debug.Assert(crate.Position.Equals(new Position(1, -1)), $"Expected (1, -1), but got {crate.Position}");
        }

        private static void Test_Reset()
        {
            var empty = EmptyNeighborhood();
            Player player = new Player(new Position(0, 0));
            player.SetDirection(Direction.West);
            player.Update(empty);

            Crate crate = new Crate(new Position(1, -2));
            crate.Update(empty);

            player.Reset();
            Debug.Assert(player.Repr() == 'o');
            Debug.Assert(player.Position.Equals(new Position(0, 0)));
            Debug.Assert(player.Direction.HasValue == false);

            crate.Reset();
            Debug.Assert(crate.Position.Equals(new Position(1, -2)));
        }

        public static void Main()
        {
            RunAll();
        }
    }
}
