using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    public class PlanetModule
    {
        public Vector2 pos;
        public int type;
        public int base_id;
        public float power;

        public PlanetModule(Vector2 pos, int type, int base_id)
        {
            this.pos = pos;
            this.type = type;
            this.base_id = base_id;
            power = 0;
        }
        public PlanetModule(System.IO.BinaryReader br)
        {
            pos = new Vector2(br.ReadSingle(), br.ReadSingle());
            type = br.ReadInt32();
            base_id = br.ReadInt32();
            power = 0;
        }

        public byte[] PackedData()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(pos.X);
            bw.Write(pos.Y);
            bw.Write(type);
            bw.Write(base_id);
            bw.Write(power);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedData(byte[] data)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            pos = new Vector2(br.ReadSingle(), br.ReadSingle());
            type = br.ReadInt32();
            base_id = br.ReadInt32();
            power = br.ReadSingle();

            br.Close();
            mem.Close();
        }
    }

    public class Meteorite
    {
        public Vector2 pos;
        public float size;
        public float timetohit;
        public float timetodestroy;

        public Meteorite(Vector2 pos, float size, float timetohit)
        {
            this.pos = pos;
            this.size = size;
            this.timetohit = timetohit;
            timetodestroy = 3;
        }
        public Meteorite(System.IO.BinaryReader br)
        {
            pos = new Vector2(br.ReadSingle(), br.ReadSingle());
            size = br.ReadSingle();
            timetohit = br.ReadSingle();
            timetodestroy = br.ReadSingle();
        }

        public byte[] PackedData()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(pos.X);
            bw.Write(pos.Y);
            bw.Write(size);
            bw.Write(timetohit);
            bw.Write(timetodestroy);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedData(byte[] data)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            pos = new Vector2(br.ReadSingle(), br.ReadSingle());
            size = br.ReadSingle();
            timetohit = br.ReadSingle();
            timetodestroy = br.ReadSingle();

            br.Close();
            mem.Close();
        }
    }
}
