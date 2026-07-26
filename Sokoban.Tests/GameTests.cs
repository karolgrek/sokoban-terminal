using System;
using System.Collections.Generic;
using Xunit;
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

        [Fact]
        public void Test_StaticObjectWall()
        {
            StaticObject staticObjectWall = new Wall(new Position(0, 0));

            Assert.True(staticObjectWall.Position.Equals(new Position(0, 0)));
            Assert.True(staticObjectWall.Repr() == '#');
            staticObjectWall.Update(EmptyNeighborhood());
            Assert.True(staticObjectWall.IsMoving() == false);

            GameObject gameObjectWall = staticObjectWall;

            Assert.True(gameObjectWall.Position.Equals(new Position(0, 0)));
            Assert.True(gameObjectWall.Repr() == '#');
            gameObjectWall.Update(EmptyNeighborhood());
            Assert.True(gameObjectWall.IsMoving() == null);
        }

        [Fact]
        public void Test_Player()
        {
            var empty = EmptyNeighborhood();
            Player player = new Player(new Position(0, 0));

            Assert.True(player.Repr() == 'o');
            player.Update(empty);
            Assert.True(player.IsMoving() == false);
            Assert.True(player.Position.Equals(new Position(0, 0)));

            player.SetDirection(Direction.North);
            Assert.True(player.Repr() == '^');
            Assert.True(player.IsMoving() == true);

            player.Update(empty);
            Assert.True(player.Position.Equals(new Position(0, -1)));

            var targetToNorth = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.North] = new Target(new Position(0, -2))
            };
            player.Update(targetToNorth);
            Assert.True(player.Position.Equals(new Position(0, -2)));

            var wallToNorth = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.North] = new Wall(new Position(0, -3))
            };
            player.Update(wallToNorth);
            Assert.True(player.Position.Equals(new Position(0, -2)));
            Assert.True(player.Direction == Direction.South);
        }

        [Fact]
        public void Test_Crate()
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
            
            Assert.True(crate.Position.Equals(new Position(1, -2)), "Crate should stay in place");

            var agentToNorthFacingTowards = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.North] = player
            };
            
            player.SetDirection(Direction.South);
            crate.Update(agentToNorthFacingTowards);
            
            Assert.True(crate.Position.Equals(new Position(1, -1)), $"Expected (1, -1), but got {crate.Position}");
        }

        [Fact]
        public void Test_Reset()
        {
            var empty = EmptyNeighborhood();
            Player player = new Player(new Position(0, 0));
            player.SetDirection(Direction.West);
            player.Update(empty);

            Crate crate = new Crate(new Position(1, -2));
            crate.Update(empty);

            player.Reset();
            Assert.True(player.Repr() == 'o');
            Assert.True(player.Position.Equals(new Position(0, 0)));
            Assert.True(player.Direction.HasValue == false);

            crate.Reset();
            Assert.True(crate.Position.Equals(new Position(1, -2)));
        }

        [Fact]
        public void Test_Crate_BlockedByWall()
        {
            var empty = EmptyNeighborhood();
            Player player = new Player(new Position(1, 0));
            Crate crate = new Crate(new Position(1, 1));
            Wall wall = new Wall(new Position(1, 2));

            player.SetDirection(Direction.South);

            var crateNeighbors = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.North] = player, 
                [Direction.South] = wall    
            };

            crate.Update(crateNeighbors);

            Assert.True(crate.Position.Equals(new Position(1, 1)), "Crate should not move into a wall");

            var playerNeighbors = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.South] = crate 
            };
            player.Update(playerNeighbors);

            Assert.True(player.Direction == Direction.North, "Player should bounce off the unmovable crate");
            Assert.True(player.Position.Equals(new Position(1, 0)));
        }

        [Fact]
        public void Test_Crate_BlockedByCrate()
        {
            var empty = EmptyNeighborhood();
            Player player = new Player(new Position(1, 0));
            Crate crate1 = new Crate(new Position(1, 1));
            Crate crate2 = new Crate(new Position(1, 2));

            player.SetDirection(Direction.South);

            var crate1Neighbors = new Dictionary<Direction, GameObject>(empty)
            {
                [Direction.North] = player,
                [Direction.South] = crate2 
            };

            crate1.Update(crate1Neighbors);

            Assert.True(crate1.Position.Equals(new Position(1, 1)), "Crate should not push another crate");
        }

        [Fact]
        public void Test_GameEngine_AgentCollision()
        {
            var mockConsole = new MockConsole();
            
            // Map with two agents facing each other
            // '>' faces East, '<' faces West. They have 1 empty space between them.
            // On Update, they will both step into the empty space and collide!
            string[] map = {
                "#####",
                "#> <#",
                "#####"
            };
            
            var engine = new Sokoban.Core.GameEngine(mockConsole, map, "Collision Test");
            
            // AgentsCollide() predicts the next step.
            // Since '>' moves East to (2,1) and '<' moves West to (2,1), they will collide on the next step.
            Assert.True(engine.AgentsCollide(), "Agents should predict a collision on the same tile");
        }
    }
}
