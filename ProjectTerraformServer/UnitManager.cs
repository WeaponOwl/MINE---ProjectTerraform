using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    class Unit
    {
        public const int Drone = 1;
        public const int Merchant = 2;
        public const int HumanShip = 3;
        public const int Pirate = 4;

        public int type;
        public Vector2 pos;
        public float height;
        public Vector2 tar;
        public float speed;
        public int direction;
        public float wait;
        public List<Vector2> waypoints;
        public Command command;

        public Unit(int type=0,float x=0,float y=0)
        {
            this.type = type;
            this.pos = new Vector2(x,y);
            this.tar = this.pos;
            speed = 3;
            height = 0;
            if (type == Pirate)
            {
                speed = 8;
                height = 2;
            }
            direction = 0;
            waypoints = new List<Vector2>();
            wait = 0;
        }

        public static Rectangle GetSource(int type)
        {
            switch (type)
            {
                //case Drone: return new Rectangle(0, 0, 40, 40);
                case Drone: return new Rectangle(0, 100, 61, 50);
                case Merchant: return new Rectangle(0, 0, 125, 100);
                case HumanShip: return new Rectangle(0, 0, 125, 100);
                case Pirate: return new Rectangle(0, 150, 200, 100);
                default: return new Rectangle(0, 0, 32, 32);
            }
        }
        public static Vector2 GetAnchor(int type)
        {
            Rectangle rect = GetSource(type);
            return new Vector2(rect.Width/2, rect.Height/2);
        }
        public static Vector2 GetSize(int type)
        {
            switch (type)
            {
                case Drone: return new Vector2(24, 19);
                case Merchant: return new Vector2(40, 40);
                case HumanShip: return new Vector2(40, 40);
                default: return new Vector2(1, 1);
            }
        }
    }
}
