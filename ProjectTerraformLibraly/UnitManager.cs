using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    public enum commands
    {
        move,
        moveandbuild,
        patrol,
        tradergoin,
        tradergoaway,
        tradegodown,
        tradegoup,
        humangoin,
        humangoaway,
        humangodown,
        humangoup,
        patroltobeacon,
        piraterob,
        pirateaway,

        localtradergoin,
        localtradegodown,

        repair,
        gotoparking,
        goaway,
        attackmode,

        rocketflyup,
        rocketfallingdown
    }
    public class Command
    {
        public commands message;
        public int x;
        public int y;
        public int id;

        public Command(commands text, int x, int y)
        {
            message = text;
            this.x = x;
            this.y = y;
        }
        public Command(commands text, int id)
        {
            message = text;
            this.id = id;
        }
    }

    public class Unit
    {
        public const int Drone = 1;
        public const int Merchant = 2;
        public const int HumanShip = 3;
        public const int Pirate = 4;
        public const int LocalMerchant = 5;
        public const int AttackDrone = 6;
        public const int RocketAtom = 7;
        public const int RocketNeitron = 8;
        public const int RocketTwined = 9;

        public int type;
        public Vector2 pos;
        public float height;
        public Vector2 tar;
        public float speed;
        public int direction;
        public float wait;
        public float maxwait;
        public List<Vector2> waypoints;
        public Command command;
        public float[] inventory;
        public short player_id;
        public short motherbasex;
        public short motherbasey;

        public Unit(int type = 0, float x = 0, float y = 0)
        {
            this.type = type;
            this.pos = new Vector2(x, y);
            this.tar = this.pos;
            speed = 3;
            height = 0;
            if (type == Pirate || type == AttackDrone)
            {
                speed = 8;
                height = 2;
            }
            direction = 0;
            waypoints = new List<Vector2>();
            wait = 0;
        }
        public Unit(System.IO.BinaryReader br)
        {
            UnpackedData(br);
        }

        public static Rectangle GetSource(int type)
        {
            switch (type)
            {
                //case Drone: return new Rectangle(0, 0, 40, 40);
                case Drone: return new Rectangle(0, 100, 61, 50);
                case Merchant: return new Rectangle(0, 0, 125, 100);
                case HumanShip: return new Rectangle(0, 0, 125, 100);
                case LocalMerchant: return new Rectangle(0, 0, 125, 100);
                case Pirate: return new Rectangle(0, 150, 200, 100);
                case AttackDrone: return new Rectangle(0, 150, 200, 100);
                case RocketTwined: return new Rectangle(1552, 0, 16, 37);
                case RocketAtom: return new Rectangle(1552+16, 0, 16, 37);
                case RocketNeitron: return new Rectangle(1552+32, 0, 16, 37);
                default: return new Rectangle(0, 0, 32, 32);
            }
        }
        public static Vector2 GetAnchor(int type)
        {
            Rectangle rect = GetSource(type);
            return new Vector2(rect.Width / 2, rect.Height / 2);
        }
        public static Vector2 GetSize(int type)
        {
            switch (type)
            {
                case Drone: return new Vector2(24, 19);
                case Merchant: return new Vector2(40, 40);
                case HumanShip: return new Vector2(40, 40);
                case LocalMerchant: return new Vector2(40, 40);
                case Pirate: return new Vector2(40, 40);
                case AttackDrone: return new Vector2(40, 40);
                default: return new Vector2(1, 1);
            }
        }

        public byte[] PackedData()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(command.id);
            bw.Write(command.x);
            bw.Write(command.y);
            bw.Write((short)command.message);
            bw.Write(player_id);
            bw.Write(direction);
            bw.Write(height);
            bw.Write(pos.X);
            bw.Write(pos.Y);
            bw.Write(speed);
            bw.Write(tar.X);
            bw.Write(tar.Y);
            bw.Write(type);
            bw.Write(wait);
            bw.Write(maxwait);
            bw.Write(waypoints.Count);
            for (int k = 0; k < waypoints.Count; k++)
            {
                bw.Write(waypoints[k].X);
                bw.Write(waypoints[k].Y);
            }

            if (inventory != null)
            {
                bw.Write(inventory.Length);
                for (int i = 0; i < inventory.Length; i++)
                    bw.Write(inventory[i]);
            }
            else bw.Write((int)0);
            bw.Write(motherbasex);
            bw.Write(motherbasey);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedData(System.IO.BinaryReader br)
        {
            command = new Command(commands.humangoaway, 0);
            command.id = br.ReadInt32();
            command.x = br.ReadInt32();
            command.y = br.ReadInt32();
            command.message = (commands)br.ReadInt16();
            player_id = br.ReadInt16();
            direction = br.ReadInt32();
            height = br.ReadSingle();
            pos = new Vector2(br.ReadSingle(), br.ReadSingle());
            speed = br.ReadSingle();
            tar = new Vector2(br.ReadSingle(), br.ReadSingle());
            type = br.ReadInt32();
            wait = br.ReadSingle();
            maxwait = br.ReadSingle();
            int wc = br.ReadInt32();
            waypoints = new List<Vector2>();
            for (int j = 0; j < wc; j++)
                waypoints.Add(new Vector2(br.ReadSingle(), br.ReadSingle()));

            int inventorysize = br.ReadInt32();
            if (inventorysize > 0)
            {
                inventory = new float[inventorysize];

                for (int i = 0; i < inventory.Length; i++)
                    inventory[i] = br.ReadSingle();
            }
            motherbasex = br.ReadInt16();
            motherbasey = br.ReadInt16();
        }

        public void CreateInventory()
        {
            inventory = new float[Map.maxresources];
        }
    }

    public class UnitGroup
    {
        public Vector2 position;
        public int planetid_target;
        public int planetid_mother;
        public int baseposiitionx;
        public int baseposiitiony;
        public short player_id;

        public List<Unit> units;

        public UnitGroup()
        {
            units = new List<Unit>();
        }

        public byte[] PackedData()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(planetid_target);
            bw.Write(planetid_mother);
            bw.Write(baseposiitionx);
            bw.Write(baseposiitiony);
            bw.Write(position.X);
            bw.Write(position.Y);
            bw.Write(player_id);

            bw.Write(units.Count);
            for (int j = 0; j < units.Count; j++)
            {
                bw.Write(units[j].PackedData());
            }

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedData(System.IO.BinaryReader br)
        {
            planetid_target = br.ReadInt32();
            planetid_mother = br.ReadInt32();
            baseposiitionx = br.ReadInt32();
            baseposiitiony = br.ReadInt32();
            position = new Vector2(br.ReadSingle(), br.ReadSingle());
            player_id = br.ReadInt16();

            int ucount = br.ReadInt32();

            for (int j = 0; j < ucount; j++)
                units.Add(new Unit(br));
        }
    }
}
