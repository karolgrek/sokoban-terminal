using System;
using System.Collections.Generic;

namespace Sokoban.Models
{
    public abstract class GameObject
    {
        public Position Position { get; protected set; }
        private readonly Position initialPosition;
        protected GameObject(Position pos)
        {
            Position = pos;
            initialPosition = pos;
        }

        public abstract void Update(Dictionary<Direction, GameObject> neighbors);
        public virtual char Repr()
        {
            if (this is Wall) return '#';
            if (this is Target) return '.';
            if (this is Crate) return 'X';
            return '?';
        }
        public virtual bool? IsMoving() => null;
        public virtual void Reset()
        {
            Position = initialPosition;
        }
    }

    public abstract class StaticObject : GameObject
    {
        public StaticObject(Position pos) : base(pos) { }
        public new bool? IsMoving() => false;
        public override void Update(Dictionary<Direction, GameObject> neighbors){}
    }

    public class Wall : StaticObject
    {
        public Wall(Position p) : base(p) { }
    }

    public class Target : StaticObject
    {
        public Target(Position p) : base(p) { }
    }

    public class Agent : GameObject
    {
        public Direction? Direction { get; protected set; }
        private readonly Direction? initialDirection;

        public Agent (Position p, Direction? d) : base(p)
        {
            Direction = d;
            initialDirection = d;
        }

        public override char Repr()
        {
            if (Direction == null) return 'o';
            return ((Direction)Direction).ToSymbol();
        }

        public override bool? IsMoving() => Direction != null;

        public override void Update(Dictionary<Direction, GameObject> neighbors)
        {
            if (Direction == null) return;

            var frontDirection = (Direction)Direction;
            var frontObject = neighbors[frontDirection];

            if (frontObject == null || frontObject is Target)
            {
                Position = Position.Step(frontDirection);
            }
            else
            {
                Direction = frontDirection.Opposite();
            }
        }

        public override void Reset()
        {
            base.Reset();
            Direction = initialDirection;
        }
    }

    public class Player : Agent
    {
        public Player(Position p) : base(p, null) {}
        public void SetDirection(Direction? d) => Direction = d;
    }

    public class Crate : GameObject
    {
        public Crate(Position p) : base(p) { }

        public override void Update(Dictionary<Direction, GameObject> neighbors)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                var obj = neighbors[dir];
                var oppositeDir = dir.Opposite();

                if (obj is Agent agent && agent.Direction == oppositeDir)
                {
                    var ahead = neighbors[oppositeDir];
                    if (ahead == null || ahead is Target)
                    {
                        Position = Position.Step(oppositeDir);
                        return;
                    }
                }
            }
        }
        public override bool? IsMoving() => false;
    }
}
