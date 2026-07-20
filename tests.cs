using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class Tests
{
    private static Dictionary<Direction, GameObject> EmptyNeighborhood()
    {
        return new Dictionary<Direction, GameObject>
        {
            { Direction.North, null },
            { Direction.East,  null },
            { Direction.South, null },
            { Direction.West,  null }
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
        //staticObjectWall.Position = new Position(0,1);
        Debug.Assert(staticObjectWall.Repr() == '#');
        staticObjectWall.Update(EmptyNeighborhood());
        Debug.Assert(staticObjectWall.IsMoving() == false);

        GameObject gameObjectWall = staticObjectWall;

        Debug.Assert(gameObjectWall.Position.Equals(new Position(0, 0)));
        Debug.Assert(gameObjectWall.Repr() == '#');
        gameObjectWall.Update(EmptyNeighborhood());
        // this under
        Console.WriteLine("Ismoving =" + gameObjectWall.IsMoving());
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
        

        // Initial state
        Player player = new Player(new Position(0, 0));
        player.SetDirection(Direction.North);
        Crate crate = new Crate(new Position(1, -2));
        
        Console.WriteLine("=== INITIAL STATE ===");
        PrintState(player, crate);
        Console.WriteLine();

        // Test 1: Player to the WEST of the crate, facing SOUTH (towards the crate)
        Console.WriteLine("=== Test 1: agentToWestFacingTowards ===");
        var agentToWestFacingAway = new Dictionary<Direction, GameObject>(empty)
        {
            [Direction.West] = player  // Player is to the west of the crate
        };
        Console.WriteLine("Neighborhood: Player to the WEST");
        PrintNeighborhood(agentToWestFacingAway);
        
        player.SetDirection(Direction.South);  // Faces SOUTH (towards the crate)
        Console.WriteLine("Player direction set to: SOUTH");
        PrintState(player, crate);
        
        crate.Update(agentToWestFacingAway);
        Console.WriteLine("AFTER crate.Update():");
        PrintState(player, crate);
        Console.WriteLine("Expected: Crate does NOT move (wall or obstacle ahead)");
        Console.WriteLine();
        
        Debug.Assert(crate.Position.Equals(new Position(1, -2)), "Crate should stay in place");

        // Test 2: Player to the NORTH of the crate, facing SOUTH (towards the crate)
        Console.WriteLine("=== Test 2: agentToNorthFacingTowards ===");
        var agentToNorthFacingTowards = new Dictionary<Direction, GameObject>(empty)
        {
            [Direction.North] = player  // Player is to the north of the crate
        };
        Console.WriteLine("Neighborhood: Player to the NORTH");
        PrintNeighborhood(agentToNorthFacingTowards);
        
        player.SetDirection(Direction.South);  // ❌ ERROR! Should be SOUTH
        Console.WriteLine("Player direction set to: NORTH (❌ SHOULD BE SOUTH!)");
        PrintState(player, crate);
        
        crate.Update(agentToNorthFacingTowards);
        Console.WriteLine("AFTER crate.Update():");
        PrintState(player, crate);
        Console.WriteLine($"Expected according to test: Crate at (1, -1)");
        Console.WriteLine($"ACTUAL RESULT: Crate at {crate.Position}");
        Console.WriteLine();
        
        Console.WriteLine($"Crate position after update: {crate.Position}");
        // The following line will FAIL because the player's direction is incorrect
        Debug.Assert(crate.Position.Equals(new Position(1, -1)), $"Expected (1, -1), but got {crate.Position}");
    }

    // Helper methods for printing
    private static void PrintState(Player player, Crate crate)
    {
        Console.WriteLine($"  Player: Position={player.Position}, Direction={player.Direction?.ToString() ?? "null"}");
        Console.WriteLine($"  Crate:  Position={crate.Position}");
    }

    private static void PrintNeighborhood(Dictionary<Direction, GameObject> neighbors)
    {
        foreach (var dir in new[] { Direction.North, Direction.East, Direction.South, Direction.West })
        {
            var obj = neighbors.ContainsKey(dir) ? neighbors[dir] : null;
            string objType = obj?.GetType().Name ?? "null";
            Console.WriteLine($"    {dir}: {objType}");
        }
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
