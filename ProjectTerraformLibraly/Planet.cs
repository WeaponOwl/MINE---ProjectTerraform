using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    public static class PlanetHelper
    {
        static public float BiCubicTexture(int[,] texture, int w, int h, float x, float y)
        {
            int x0 = (int)x % w;
            int y0 = (int)y % h;
            int xm1 = (x0 + w - 1) % w;
            int ym1 = (y0 + h - 1) % h;
            int xm2 = (x0 + w - 2) % w;
            int ym2 = (y0 + h - 2) % h;
            int x1 = (x0 + 1) % w;
            int y1 = (y0 + 1) % h;
            int x2 = (x0 + 2) % w;
            int y2 = (y0 + 2) % h;

            float fx = x - x0;
            float fy = y - y0;
            float fxm1 = (fx - 1);
            float fym1 = (fy - 1);
            float fxm2 = (fx - 2);
            float fym2 = (fy - 2);
            float fx1 = (fx + 1);
            float fy1 = (fy + 1);
            float fx2 = (fx + 2);
            float fy2 = (fy + 2);

            float b1 = (1 / 4.0f) * (fxm1 * fxm2 * fx1 * fym1 * fym2 * fy1);
            float b2 = (-1 / 4.0f) * (fx * fx1 * fxm2 * fym1 * fym2 * fy1);
            float b3 = (-1 / 4.0f) * (fy * fxm1 * fxm2 * fx1 * fy1 * fym2);
            float b4 = (1 / 4.0f) * (fx * fy * fx1 * fxm2 * fy1 * fym2);
            float b5 = (-1 / 12.0f) * (fx * fxm1 * fxm2 * fym1 * fym2 * fy1);
            float b6 = (-1 / 12.0f) * (fy * fxm1 * fxm2 * fx1 * fym1 * fym2);
            float b7 = (1 / 12.0f) * (fx * fy * fxm1 * fxm2 * fy1 * fym2);
            float b8 = (1 / 12.0f) * (fx * fy * fx1 * fxm2 * fym1 * fym2);
            float b9 = (1 / 12.0f) * (fx * fxm1 * fx1 * fym1 * fym2 * fy1);
            float b10 = (1 / 12.0f) * (fy * fxm1 * fxm2 * fx1 * fym1 * fy1);
            float b11 = (1 / 36.0f) * (fx * fy * fxm1 * fxm2 * fym1 * fym2);
            float b12 = (-1 / 12.0f) * (fx * fy * fxm1 * fx1 * fy1 * fym2);
            float b13 = (-1 / 12.0f) * (fx * fy * fx1 * fxm2 * fym1 * fy1);
            float b14 = (-1 / 36.0f) * (fx * fy * fxm1 * fx1 * fym1 * fym2);
            float b15 = (-1 / 36.0f) * (fx * fy * fxm1 * fxm2 * fym1 * fy1);
            float b16 = (1 / 36.0f) * (fx * fy * fxm1 * fx1 * fym1 * fy1);

            float value = b1 * texture[y0, x0] + b2 * texture[y0, x1] + b3 * texture[y1, x0] + b4 * texture[y1, x1];
            value += b5 * texture[y0, xm1] + b6 * texture[ym1, x0] + b7 * texture[y1, xm1] + b8 * texture[ym1, x1];
            value += b9 * texture[y0, x2] + b10 * texture[y2, x0] + b11 * texture[ym1, xm1] + b12 * texture[y1, x2];
            value += b13 * texture[y2, x1] + b14 * texture[ym1, x2] + b15 * texture[ym1, x2] + b16 * texture[y2, x2];

            return value;
        }
        static public float BiCubicTexture(float[,] texture, int w, int h, float x, float y)
        {
            x += w;
            if (x >= w) x -= w;
            if (x >= w) x -= w;
            y += h;
            if (y >= h) y -= h;
            if (y >= h) y -= h;

            if (y < 0 || x < 0 || y >= h || x >= w)
                x = 0;

            int x0 = (int)x % w;
            int y0 = (int)y % h;
            int xm1 = (x0 + w - 1) % w;
            int ym1 = (y0 + h - 1) % h;
            int xm2 = (x0 + w - 2) % w;
            int ym2 = (y0 + h - 2) % h;
            int x1 = (x0 + 1 + w) % w;
            int y1 = (y0 + 1 + h) % h;
            int x2 = (x0 + 2 + w) % w;
            int y2 = (y0 + 2 + h) % h;

            float fx = x - x0;
            float fy = y - y0;
            float fxm1 = (fx - 1);
            float fym1 = (fy - 1);
            float fxm2 = (fx - 2);
            float fym2 = (fy - 2);
            float fx1 = (fx + 1);
            float fy1 = (fy + 1);
            float fx2 = (fx + 2);
            float fy2 = (fy + 2);

            float b1 = (1 / 4.0f) * (fxm1 * fxm2 * fx1 * fym1 * fym2 * fy1);
            float b2 = (-1 / 4.0f) * (fx * fx1 * fxm2 * fym1 * fym2 * fy1);
            float b3 = (-1 / 4.0f) * (fy * fxm1 * fxm2 * fx1 * fy1 * fym2);
            float b4 = (1 / 4.0f) * (fx * fy * fx1 * fxm2 * fy1 * fym2);
            float b5 = (-1 / 12.0f) * (fx * fxm1 * fxm2 * fym1 * fym2 * fy1);
            float b6 = (-1 / 12.0f) * (fy * fxm1 * fxm2 * fx1 * fym1 * fym2);
            float b7 = (1 / 12.0f) * (fx * fy * fxm1 * fxm2 * fy1 * fym2);
            float b8 = (1 / 12.0f) * (fx * fy * fx1 * fxm2 * fym1 * fym2);
            float b9 = (1 / 12.0f) * (fx * fxm1 * fx1 * fym1 * fym2 * fy1);
            float b10 = (1 / 12.0f) * (fy * fxm1 * fxm2 * fx1 * fym1 * fy1);
            float b11 = (1 / 36.0f) * (fx * fy * fxm1 * fxm2 * fym1 * fym2);
            float b12 = (-1 / 12.0f) * (fx * fy * fxm1 * fx1 * fy1 * fym2);
            float b13 = (-1 / 12.0f) * (fx * fy * fx1 * fxm2 * fym1 * fy1);
            float b14 = (-1 / 36.0f) * (fx * fy * fxm1 * fx1 * fym1 * fym2);
            float b15 = (-1 / 36.0f) * (fx * fy * fxm1 * fxm2 * fym1 * fy1);
            float b16 = (1 / 36.0f) * (fx * fy * fxm1 * fx1 * fym1 * fy1);

            float t1 = texture[y0, x0];
            float t2 = texture[y0, x1];
            float t3 = texture[y1, x0];
            float t4 = texture[y1, x1];
            float t5 = texture[y0, xm1];
            float t6 = texture[ym1, x0];
            float t7 = texture[y1, xm1];
            float t8 = texture[ym1, x1];
            float t9 = texture[y0, x2];
            float t10 = texture[y2, x0];
            float t11 = texture[ym1, xm1];
            float t12 = texture[y1, x2];
            float t13 = texture[y2, x1];
            float t14 = texture[ym1, x2];
            float t15 = texture[ym1, x2];
            float t16 = texture[y2, x2];

            float value = b1 * texture[y0, x0] + b2 * texture[y0, x1] + b3 * texture[y1, x0] + b4 * texture[y1, x1];
            value += b5 * texture[y0, xm1] + b6 * texture[ym1, x0] + b7 * texture[y1, xm1] + b8 * texture[ym1, x1];
            value += b9 * texture[y0, x2] + b10 * texture[y2, x0] + b11 * texture[ym1, xm1] + b12 * texture[y1, x2];
            value += b13 * texture[y2, x1] + b14 * texture[ym1, x2] + b15 * texture[ym1, x2] + b16 * texture[y2, x2];

            return value;
        }
        static public float BiCubicTexture(bool[,] texture, int w, int h, float x, float y)
        {
            x += w;
            if (x >= w) x -= w;
            if (x >= w) x -= w;
            y += h;
            if (y >= h) y -= h;
            if (y >= h) y -= h;

            if (y < 0 || x < 0 || y >= h || x >= w)
                x = 0;

            int x0 = (int)x % w;
            int y0 = (int)y % h;
            int xm1 = (x0 + w - 1) % w;
            int ym1 = (y0 + h - 1) % h;
            int xm2 = (x0 + w - 2) % w;
            int ym2 = (y0 + h - 2) % h;
            int x1 = (x0 + 1 + w) % w;
            int y1 = (y0 + 1 + h) % h;
            int x2 = (x0 + 2 + w) % w;
            int y2 = (y0 + 2 + h) % h;

            float fx = x - x0;
            float fy = y - y0;
            float fxm1 = (fx - 1);
            float fym1 = (fy - 1);
            float fxm2 = (fx - 2);
            float fym2 = (fy - 2);
            float fx1 = (fx + 1);
            float fy1 = (fy + 1);
            float fx2 = (fx + 2);
            float fy2 = (fy + 2);

            float b1 = (1 / 4.0f) * (fxm1 * fxm2 * fx1 * fym1 * fym2 * fy1);
            float b2 = (-1 / 4.0f) * (fx * fx1 * fxm2 * fym1 * fym2 * fy1);
            float b3 = (-1 / 4.0f) * (fy * fxm1 * fxm2 * fx1 * fy1 * fym2);
            float b4 = (1 / 4.0f) * (fx * fy * fx1 * fxm2 * fy1 * fym2);
            float b5 = (-1 / 12.0f) * (fx * fxm1 * fxm2 * fym1 * fym2 * fy1);
            float b6 = (-1 / 12.0f) * (fy * fxm1 * fxm2 * fx1 * fym1 * fym2);
            float b7 = (1 / 12.0f) * (fx * fy * fxm1 * fxm2 * fy1 * fym2);
            float b8 = (1 / 12.0f) * (fx * fy * fx1 * fxm2 * fym1 * fym2);
            float b9 = (1 / 12.0f) * (fx * fxm1 * fx1 * fym1 * fym2 * fy1);
            float b10 = (1 / 12.0f) * (fy * fxm1 * fxm2 * fx1 * fym1 * fy1);
            float b11 = (1 / 36.0f) * (fx * fy * fxm1 * fxm2 * fym1 * fym2);
            float b12 = (-1 / 12.0f) * (fx * fy * fxm1 * fx1 * fy1 * fym2);
            float b13 = (-1 / 12.0f) * (fx * fy * fx1 * fxm2 * fym1 * fy1);
            float b14 = (-1 / 36.0f) * (fx * fy * fxm1 * fx1 * fym1 * fym2);
            float b15 = (-1 / 36.0f) * (fx * fy * fxm1 * fxm2 * fym1 * fy1);
            float b16 = (1 / 36.0f) * (fx * fy * fxm1 * fx1 * fym1 * fy1);

            float value = b1 * (texture[y0, x0] ? 1 : 0) + b2 * (texture[y0, x1] ? 1 : 0) + b3 * (texture[y1, x0] ? 1 : 0) + b4 * (texture[y1, x1] ? 1 : 0);
            value += b5 * (texture[y0, xm1] ? 1 : 0) + b6 * (texture[ym1, x0] ? 1 : 0) + b7 * (texture[y1, xm1] ? 1 : 0) + b8 * (texture[ym1, x1] ? 1 : 0);
            value += b9 * (texture[y0, x2] ? 1 : 0) + b10 * (texture[y2, x0] ? 1 : 0) + b11 * (texture[ym1, xm1] ? 1 : 0) + b12 * (texture[y1, x2] ? 1 : 0);
            value += b13 * (texture[y2, x1] ? 1 : 0) + b14 * (texture[ym1, x2] ? 1 : 0) + b15 * (texture[ym1, x2] ? 1 : 0) + b16 * (texture[y2, x2] ? 1 : 0);

            return value;
        }
    }
    public struct Score
    {
        public float building;
        public float energy;
        public float inventory;
        public float population;
        public float support;

        public float Total { get { return building + energy + inventory + population + support; } }

        public byte[] Packed()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

            bw.Write(building);
            bw.Write(energy);
            bw.Write(inventory);
            bw.Write(population);
            bw.Write(support);

            byte[] data = ms.ToArray();

            bw.Close();
            ms.Close();

            return data;
        }
        public byte[] PackedName(string name)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

            bw.Write(name);
            bw.Write(building);
            bw.Write(energy);
            bw.Write(inventory);
            bw.Write(population);
            bw.Write(support);

            byte[] data = ms.ToArray();

            bw.Close();
            ms.Close();

            return data;
        }
        public void Unpacked(byte[] data)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);

            building = br.ReadSingle();
            energy = br.ReadSingle();
            inventory = br.ReadSingle();
            population = br.ReadSingle();
            support = br.ReadSingle();

            br.Close();
            ms.Close();
        }
        public string UnpackedName(byte[] data)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);

            string name = br.ReadString();
            building = br.ReadSingle();
            energy = br.ReadSingle();
            inventory = br.ReadSingle();
            population = br.ReadSingle();
            support = br.ReadSingle();

            br.Close();
            ms.Close();

            return name;
        }
    }
    public class PlayerStation
    {
        public int id;
        public string name;
        public float credits;

        public PlayerStation(string name,int id)
        {
            this.name = name;
            credits = 5000;
            this.id = id;
        }

        public byte[] Packed()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(id);
            bw.Write(name);
            bw.Write(credits);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void Unpack(System.IO.BinaryReader br)
        {
            id = br.ReadInt32();
            name = br.ReadString();
            credits = br.ReadSingle();
        }
    }

    public class Star
    {
        public double size;
        public double radius;
        public double temperature;
        public double mass;

        public bool pirates, sunstrike, meteorites;

        public int planets_num;
        public Planet[] planets;

        public string name;

        double totalgametime;

        public List<UnitGroup> unitgroups;
        public List<PlayerStation> players;

        //0.00435 - radius of Sun in a.o.
        //8.7 = mass of sun in mass of erath/V of sun in a.o
        //10^(-4*[-1...1]) - function of star lighting

        public byte[] PackedDescription()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(size);
            bw.Write(radius);
            bw.Write(temperature);
            bw.Write(mass);
            bw.Write(totalgametime);
            bw.Write(name);

            bw.Write(pirates);
            bw.Write(sunstrike);
            bw.Write(meteorites);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedDescription(byte[] description)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(description);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            size = br.ReadDouble();
            radius = br.ReadDouble();
            temperature= br.ReadDouble();
            mass= br.ReadDouble();
            totalgametime = br.ReadDouble();
            name = br.ReadString();

            pirates=br.ReadBoolean();
            sunstrike=br.ReadBoolean();
            meteorites = br.ReadBoolean();

            br.Close();
            mem.Close();
        }
        public byte[] PackedData()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(planets_num);
            for (int i = 0; i < planets_num; i++)
            {
                bw.Write(planets[i].PackedDescription());
            }
            bw.Write(players.Count);
            foreach (PlayerStation p in players)
                bw.Write(p.Packed());

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

            planets_num = br.ReadInt32();
            planets = new Planet[planets_num];

            for (int i = 0; i < planets_num; i++)
            {
                planets[i] = new Planet(br);
                planets[i].star = this;
                planets[i].id = i;
            }

            int players_num = br.ReadInt32();
            for (int i = 0; i < players_num; i++)
            {
                PlayerStation p = new PlayerStation("", 0);
                p.Unpack(br);
                players.Add(p);
            }

            br.Close();
            mem.Close();
        }
        public byte[] PackedUnits()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(unitgroups.Count);
            for (int i = 0; i < unitgroups.Count; i++)
            {
                bw.Write(unitgroups[i].PackedData());
            }

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedUnits(byte[] data)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            int count = br.ReadInt32();
            unitgroups = new List<UnitGroup>();

            for (int i = 0; i < count; i++)
            {
                UnitGroup ug = new UnitGroup();
                ug.UnpackedData(br);

                unitgroups.Add(ug);
            }

            br.Close();
            mem.Close();
        }

        public Star()
        {
            Random rand = new Random();
            NormalRandom nrand = new NormalRandom();

            size = Math.Abs(nrand.Next());
            if (size < -1) do { size++; } while (size < -1);
            if (size > -1) do { size--; } while (size > 1);

            double startlighting = Math.Abs(Math.Pow(10, 4 * size));
            radius = startlighting * 0.00435;                                       //a.o.
            mass = 960000000000 * 4 / 3 * Math.PI * Math.Pow(radius, 3);            //massus Earth

            float temperature_argumemt = (float)(rand.NextDouble() * 7);
            temperature = (float)(26.389 * Math.Pow(temperature_argumemt, 6) -
                                 550 * Math.Pow(temperature_argumemt, 5) +
                                 4555.6 * Math.Pow(temperature_argumemt, 4) -
                                 19042 * Math.Pow(temperature_argumemt, 3) +
                                 41918 * Math.Pow(temperature_argumemt, 2) -
                                 44408 * temperature_argumemt + 19500);             //Kelvin

            totalgametime = 0;
            name = NameGenerator.GenerateStarName(rand);

            unitgroups = new List<UnitGroup>();
            players = new List<PlayerStation>();

            pirates = sunstrike = meteorites = true;
        }
        public Star(int seed)
        {
            Random rand = new Random(seed);
            NormalRandom nrand = new NormalRandom(seed);

            size = Math.Abs(nrand.Next());
            if (size < -1) do { size++; } while (size < -1);
            if (size > -1) do { size--; } while (size > 1);

            double startlighting = Math.Abs(Math.Pow(10, 4 * size));
            radius = startlighting * 0.00435;                                       //a.o.
            mass = 960000000000 * 4 / 3 * Math.PI * Math.Pow(radius, 3);            //massus Earth

            float temperature_argumemt = (float)(rand.NextDouble() * 7);
            temperature = (float)(26.389 * Math.Pow(temperature_argumemt, 6) -
                                 550 * Math.Pow(temperature_argumemt, 5) +
                                 4555.6 * Math.Pow(temperature_argumemt, 4) -
                                 19042 * Math.Pow(temperature_argumemt, 3) +
                                 41918 * Math.Pow(temperature_argumemt, 2) -
                                 44408 * temperature_argumemt + 19500);             //Kelvin

            totalgametime = 0;
            name = NameGenerator.GenerateStarName(rand);

            unitgroups = new List<UnitGroup>();
            players = new List<PlayerStation>();

            pirates = sunstrike = meteorites = true;
        }
        public Star(double size,double light, double temp)
        {
            this.size = size;
            double startlighting = light;
            radius = startlighting * 0.00435;                                       //a.o.
            mass = 960000000000 * 4 / 3 * Math.PI * Math.Pow(radius, 3);            //massus Earth

            temperature = temp;                                                      //Kelvin

            totalgametime = 0;
            name = NameGenerator.GenerateStarName();

            unitgroups = new List<UnitGroup>();
            players = new List<PlayerStation>();

            pirates = sunstrike = meteorites = true;
        }
        public Star(byte[] description)
        {
            UnpackedDescription(description);

            unitgroups = new List<UnitGroup>();
            players = new List<PlayerStation>();
        }

        public void GeneratePlanets(int num)
        {
            planets_num = num;
            planets = new Planet[planets_num];

            for (int i = 0; i < planets_num; i++)
            {
                planets[i] = new Planet(this);
                planets[i].id = i;
            }
        }
        public void GeneratePlanets(float planet_size,float planet_radius, float planet_axis)
        {
            planets_num = 1;
            planets = new Planet[1];

            planets[0] = new Planet(this, planet_size, planet_radius, planet_axis);
            planets[0].id = 0;
        }
        public void GeneratePlanets(int seed, int num)
        {
            planets_num = num;
            planets = new Planet[planets_num];

            for (int i = 0; i < planets_num; i++)
            {
                planets[i] = new Planet(this,seed);
                planets[i].id = i;
                seed++;
            }
        }

        public void Update(double ellapsedTime,double totalTime)
        {
            for (int i = 0; i < planets_num; i++)
                planets[i].Update(ellapsedTime, totalTime);

            for (int i = unitgroups.Count - 1; i >= 0; i--)
            {
                if (unitgroups[i].planetid_target >= 0)
                {
                    float distance = unitgroups[i].position.Length();

                    Vector2 tar = Vector2.UnitX;
                    if(unitgroups[i].planetid_target<planets.Length)
                        tar = planets[unitgroups[i].planetid_target].GetPosition(unitgroups[i].planetid_target);
                    else tar = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ((float)(GetTotalTimeForLighting() / distance *30)));

                    Vector2 dir = Vector2.Normalize(tar - unitgroups[i].position);

                    unitgroups[i].position = unitgroups[i].position + dir * (float)ellapsedTime * distance;

                    if ((unitgroups[i].position - tar).Length() < ellapsedTime)
                    {
                        if (unitgroups[i].planetid_target < planets.Length)
                        {
                            UnitGroup g = unitgroups[i];
                            Random rand = new Random();
                            g.position = new Vector2(rand.Next(planets[g.planetid_target].mapwidth), rand.Next(planets[g.planetid_target].mapheight));

                            planets[g.planetid_target].unitgroups.Add(g);
                            unitgroups.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    Vector2 tar = Vector2.Transform(new Vector2(15, 0), Matrix.CreateRotationZ((float)totalgametime / 3));
                    Vector2 dir = Vector2.Normalize(tar - unitgroups[i].position);
                    float distance = unitgroups[i].position.Length();
                    unitgroups[i].position = unitgroups[i].position + dir * (float)ellapsedTime * distance;

                    if (distance > 14 || (unitgroups[i].position - tar).Length() < ellapsedTime)
                        unitgroups.RemoveAt(i);
                }
            }

            totalgametime += ellapsedTime;
        }
        public void UpdateOnline(double ellapsedTime, double totalTime)
        {
            for (int i = 0; i < planets_num; i++)
                planets[i].UpdateOnline(ellapsedTime, totalTime);

            for (int i = unitgroups.Count - 1; i >= 0; i--)
            {
                if (unitgroups[i].planetid_target >= 0)
                {
                    float distance = unitgroups[i].position.Length();

                    Vector2 tar = planets[unitgroups[i].planetid_target].GetPosition(unitgroups[i].planetid_target);
                    Vector2 dir = Vector2.Normalize(tar - unitgroups[i].position);

                    unitgroups[i].position = unitgroups[i].position + dir * (float)ellapsedTime * distance;

                    if ((unitgroups[i].position - tar).Length() < ellapsedTime)
                    {
                        UnitGroup g = unitgroups[i];
                        Random rand = new Random();
                        g.position = new Vector2(rand.Next(planets[g.planetid_target].mapwidth), rand.Next(planets[g.planetid_target].mapheight));

                        planets[g.planetid_target].unitgroups.Add(g);
                        unitgroups.RemoveAt(i);
                    }
                }
                else
                {
                    Vector2 tar = Vector2.Transform(new Vector2(15, 0), Matrix.CreateRotationZ((float)totalgametime / 3));
                    Vector2 dir = Vector2.Normalize(tar - unitgroups[i].position);
                    float distance = unitgroups[i].position.Length();
                    unitgroups[i].position = unitgroups[i].position + dir * (float)ellapsedTime * distance;

                    if (distance > 14 || (unitgroups[i].position - tar).Length() < ellapsedTime)
                        unitgroups.RemoveAt(i);
                }
            }

            totalgametime += ellapsedTime;
        }

        public double GetTotalTimeForLighting()
        {
            //return totalgametime;
            return totalgametime / 20;
        }
        public double GetTotalTime() { return totalgametime; }

        public float GetSolarstrikeChance(float time)
        {
            float light = (float)radius / 0.00435f;
            float turbulance = (float)Math.Sqrt(Math.Sqrt(light)) * Constants.Map_sunstrikenexttime * 30;

            return turbulance;
        }

        public Score GetScore(int player_id)
        {
            Score score = new Score();

            foreach (Planet p in planets)
            {
                foreach (Map m in p.maps)
                {
                    if (m.player_id == player_id)
                    {
                        foreach (Building b in m.buildings)
                            score.building += b.power * b.height * (b.buildingtime <= 0 ? 2 : 0) * Building.GetBuildPrice(b.type);
                        score.energy += m.energyproduction*2;
                        for (int i = 0; i < m.inventory.Length; i++)
                            score.inventory += (m.inventory[i].count + Constants.GetPriceOfResource(i) + m.inventory[i].buildingproduct) / 100;
                        score.population += m.population * 3;
                        foreach (Shield s in m.shields)
                            score.support += s.size;
                        for (int i = 0; i < m.storageinfo.Length; i++)
                            score.support += m.storageinfo[i].maxcapability / 100;
                    }
                }
            }
            return score;
        }

        public bool TryPayMoney(int money, int player)
        {
            foreach (PlayerStation p in players)
            {
                if (p.id == player)
                {
                    if (p.credits >= money)
                    {
                        p.credits -= money;
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        public bool TryAddMoney(int money, int player)
        {
            foreach (PlayerStation p in players)
            {
                if (p.id == player)
                {
                    p.credits += money;
                }
            }
            return false;
        }
        public float GetMoney(int player)
        {
            foreach (PlayerStation p in players)
                if (p.id == player)
                    return p.credits;
            return -1;
        }

        public float[] GetAsteroids()
        {
            int sum = (int)(radius + temperature + mass);
            int points = sum % 10;
            int points2 = 1;

            if (points > 5) points2 += 0;
            else if (points > 1) points2 += 1;
            else points2 += 2;

            float[] asteroids = new float[points2];
            for (int i = 0; i < points2; i++)
            {
                asteroids[i] = ((sum + (i+1) * 151) % 297) / 100.0f;
            }

            return asteroids;
        }
        public int GetPlanetNum()
        {
            int seed = (int)((mass + radius + temperature) * 235);

            NormalRandom nrand = new NormalRandom(seed);
            int num = (int)nrand.Next(1, 13);
            if (num < 1) num = 1;
            if (num > 13) num = 13;

            return num;
        }
    }

    public class Planet
    {
        public Star star;
        public string name;

        public double albedo;

        public double semimajoraxis;
        public double semiminoraxis;
        public double sidusperiod;
        public double orbitalspeed;

        public double size;
        public double radius;
        public double mass;
        public double rotateperiod;
        public float axistilt;

        public double atmosherepleasure;
        public double mintemperature;
        public double maxtemperature;

        public double atmosherepleasureoffcet;
        public double temperatureoffcet;

        public int mapwidth, mapheight;
        public float[,] heightmap;
        public bool[,] watermap;

        public List<Map> maps;
        public List<UnitGroup> unitgroups;
        public List<PlanetModule> modules;
        public List<Meteorite> meteorites;

        public float timetosolarstrike;

        public int id = 0;

        public List<string> messages;

        public byte[] PackedDescription()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(name);
            bw.Write(albedo);
            bw.Write(semimajoraxis);
            bw.Write(semiminoraxis);
            bw.Write(sidusperiod);
            bw.Write(orbitalspeed);
            bw.Write(size);
            bw.Write(radius);
            bw.Write(mass);
            bw.Write(rotateperiod);
            bw.Write(axistilt);
            bw.Write(atmosherepleasure);
            bw.Write(mintemperature);
            bw.Write(maxtemperature);
            bw.Write(atmosherepleasureoffcet);
            bw.Write(temperatureoffcet);
            bw.Write(maps.Count);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedDescription(byte[] description)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(description);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            name = br.ReadString();
            albedo = br.ReadDouble();
            semimajoraxis = br.ReadDouble();
            semiminoraxis = br.ReadDouble();
            sidusperiod = br.ReadDouble();
            orbitalspeed = br.ReadDouble();
            size = br.ReadDouble();
            radius = br.ReadDouble();
            mass = br.ReadDouble();
            rotateperiod = br.ReadDouble();
            axistilt = br.ReadSingle();
            atmosherepleasure = br.ReadDouble();
            mintemperature = br.ReadDouble();
            maxtemperature = br.ReadDouble();
            atmosherepleasureoffcet = br.ReadDouble();
            temperatureoffcet = br.ReadDouble();
            int count = br.ReadInt32();
            maps = new List<Map>(count);

            br.Close();
            mem.Close();
        }
        public byte[] PackedData()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(mapwidth);
            bw.Write(mapheight);
            for (int i = 0; i < mapheight; i++)
                for (int j = 0; j < mapwidth; j++)
                {
                    bw.Write(heightmap[i, j]);
                    bw.Write(watermap[i, j]);
                }
            bw.Write(maps.Count);
            foreach (Map m in maps)
                bw.Write(m.PackedDescription());

            bw.Write(modules.Count);
            foreach (PlanetModule m in modules)
                bw.Write(m.PackedData());

            bw.Write(meteorites.Count);
            foreach (Meteorite m in meteorites)
                bw.Write(m.PackedData());

            bw.Write(timetosolarstrike);

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

            mapwidth = br.ReadInt32();
            mapheight = br.ReadInt32();

            heightmap = new float[mapheight, mapwidth];
            watermap = new bool[mapheight, mapwidth];
            for (int i = 0; i < mapheight; i++)
                for (int j = 0; j < mapwidth; j++)
                {
                    float h = br.ReadSingle();
                    bool w = br.ReadBoolean();
                    heightmap[i, j] = h;
                    watermap[i, j] = w;
                }
            int mapcount = br.ReadInt32();
            maps = new List<Map>();
            for (int i = 0; i < mapcount; i++)
            {
                Map map = new Map(br);
                maps.Add(map);
            }

            int modulescount = br.ReadInt32();
            modules = new List<PlanetModule>();
            for (int i = 0; i < modulescount; i++)
            {
                PlanetModule module = new PlanetModule(br);
                modules.Add(module);
            }

            int meteoritescount = br.ReadInt32();
            meteorites = new List<Meteorite>();
            for (int i = 0; i < meteoritescount; i++)
            {
                Meteorite meteorite = new Meteorite(br);
                meteorites.Add(meteorite);
            }

            timetosolarstrike = br.ReadSingle();

            br.Close();
            mem.Close();
        }
        public byte[] PackedUnits()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(unitgroups.Count);
            for (int i = 0; i < unitgroups.Count; i++)
            {
                bw.Write(unitgroups[i].PackedData());
            }

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedUnits(byte[] data)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            int count = br.ReadInt32();
            unitgroups = new List<UnitGroup>();

            for (int i = 0; i < count; i++)
            {
                UnitGroup ug = new UnitGroup();
                ug.UnpackedData(br);

                unitgroups.Add(ug);
            }

            br.Close();
            mem.Close();
        }
        public byte[] PackedSync()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(maps.Count);
            foreach (Map m in maps)
                bw.Write(m.PackedDescription());

            bw.Write(modules.Count);
            foreach (PlanetModule m in modules)
                bw.Write(m.PackedData());

            bw.Write(meteorites.Count);
            foreach (Meteorite m in meteorites)
                bw.Write(m.PackedData());

            bw.Write(unitgroups.Count);
            for (int i = 0; i < unitgroups.Count; i++)
            {
                bw.Write(unitgroups[i].PackedData());
            }

            bw.Write(timetosolarstrike);

            bw.Write(temperatureoffcet);
            bw.Write(atmosherepleasureoffcet);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedSync(byte[] data)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            int mapcount = br.ReadInt32();
            maps = new List<Map>();
            for (int i = 0; i < mapcount; i++)
            {
                Map map = new Map(br);
                maps.Add(map);
            }

            int modulescount = br.ReadInt32();
            modules = new List<PlanetModule>();
            for (int i = 0; i < modulescount; i++)
            {
                PlanetModule module = new PlanetModule(br);
                modules.Add(module);
            }

            int meteoritescount = br.ReadInt32();
            meteorites = new List<Meteorite>();
            for (int i = 0; i < meteoritescount; i++)
            {
                Meteorite meteorite = new Meteorite(br);
                meteorites.Add(meteorite);
            }

            int count = br.ReadInt32();
            unitgroups = new List<UnitGroup>();

            for (int i = 0; i < count; i++)
            {
                UnitGroup ug = new UnitGroup();
                ug.UnpackedData(br);

                unitgroups.Add(ug);
            }

            timetosolarstrike = br.ReadSingle();
            temperatureoffcet = br.ReadDouble();
            atmosherepleasureoffcet = br.ReadDouble();

            br.Close();
            mem.Close();
        }

        public Planet(Star star,int seed = 0)
        {
            Random rand;// = new Random();
            NormalRandom nrand;// = new NormalRandom();

            if (seed == 0)
            {
                rand = new Random();
                nrand = new NormalRandom();
            }
            else
            {
                rand = new Random(seed);
                nrand = new NormalRandom(seed);
            }

            this.star = star;

            //albedo = (float)(0.45 + nrand.Next() * 0.15);
            //albedo = 0.45;
            albedo = 0.25;

            //6371 - radius of earth

            size = Math.Abs(nrand.Next());
            if (size < -1) do { size++; } while (size < -1);
            if (size > -1) do { size--; } while (size > 1);
            size /= 1.5;

            radius = Math.Abs(size * 11500 + 13500);                                                       //km
            mass = (float)(4f / 3f * Math.PI * Math.Pow(radius / 6371f, 3)) * 
                   (float)(rand.NextDouble() * 0.05 + 0.17);                                                //mass Earth
            axistilt = (float)(nrand.Next() / 2) * 23f / 57f + 23f / 57f;                                   //radian

            semimajoraxis = Math.Abs((float)(star.radius / 0.00435 * nrand.Next(0.25, 4.75)));              //a.o.
            semiminoraxis = semimajoraxis / 100f * (100 - rand.Next(10));                                   //a.o.

            double orbitsize = (semimajoraxis + semiminoraxis) * 150000000 * (float)Math.PI;                //+7+2              //G = 6.67*10^-23
            orbitalspeed = Math.Sqrt(Math.Sqrt(Math.Sqrt(Math.Sqrt(orbitsize * star.mass * mass / semimajoraxis))));            //km/sec
            sidusperiod = orbitsize / orbitalspeed / 3600;                                                  //hours
            rotateperiod = 24;                                                                             //hours

            atmosherepleasure = mass * 101;                                        //kPa
            double maxtemp = Math.Sqrt(Math.Sqrt(Math.Pow(star.radius / semiminoraxis, 2) * (1 - albedo) / 4)) * star.temperature;
            double mintemp = Math.Sqrt(Math.Sqrt(Math.Pow(star.radius / semimajoraxis, 2) * (1 - albedo) / 4)) * star.temperature;
            mintemperature =  mintemp - 273 + mass * 3;
            maxtemperature =  maxtemp - 273 + mass * 5;
            atmosherepleasureoffcet = 0;
            temperatureoffcet = 0;

            GenerateMap();

            name = NameGenerator.GeneratePlanetName();
            maps = new List<Map>();

            unitgroups = new List<UnitGroup>();
            modules = new List<PlanetModule>();
            meteorites = new List<Meteorite>();

            timetosolarstrike = Constants.Map_sunstrikenexttime;
        }
        public Planet(Star star,float planet_size, float planet_radius, float planet_axis, int seed = 0)
        {
            Random rand;// = new Random();
            NormalRandom nrand;// = new NormalRandom();

            if (seed == 0)
            {
                rand = new Random();
                nrand = new NormalRandom();
            }
            else
            {
                rand = new Random(seed);
                nrand = new NormalRandom(seed);
            }

            this.star = star;

            //albedo = (float)(0.45 + nrand.Next() * 0.15);
            //albedo = 0.45;
            albedo = 0.25;

            //6371 - radius of earth
            size = planet_size / 1.5;
            radius = planet_radius;                                                                         //km
            mass = (float)(4f / 3f * Math.PI * Math.Pow(radius / 6371f, 3)) *
                   (float)(rand.NextDouble() * 0.05 + 0.17);                                                //mass Earth
            axistilt = (float)(nrand.Next() / 2) * 23f / 57f + 23f / 57f;                                   //radian

            semimajoraxis = planet_axis;                                                                    //a.o.
            semiminoraxis = semimajoraxis / 100f * (100 - rand.Next(10));                                   //a.o.

            double orbitsize = (semimajoraxis + semiminoraxis) * 150000000 * (float)Math.PI;                //+7+2              //G = 6.67*10^-23
            orbitalspeed = Math.Sqrt(Math.Sqrt(Math.Sqrt(Math.Sqrt(orbitsize * star.mass * mass / semimajoraxis))));            //km/sec
            sidusperiod = orbitsize / orbitalspeed / 3600;                                                  //hours
            rotateperiod = 24;                                                                             //hours

            atmosherepleasure = mass * 101;                                        //kPa
            double maxtemp = Math.Sqrt(Math.Sqrt(Math.Pow(star.radius / semiminoraxis, 2) * (1 - albedo) / 4)) * star.temperature;
            double mintemp = Math.Sqrt(Math.Sqrt(Math.Pow(star.radius / semimajoraxis, 2) * (1 - albedo) / 4)) * star.temperature;
            mintemperature = mintemp - 273 + mass * 3;
            maxtemperature = maxtemp - 273 + mass * 5;
            atmosherepleasureoffcet = 0;
            temperatureoffcet = 0;

            GenerateMap();

            name = NameGenerator.GeneratePlanetName();
            maps = new List<Map>();

            unitgroups = new List<UnitGroup>();
            modules = new List<PlanetModule>();
            meteorites = new List<Meteorite>();

            timetosolarstrike = Constants.Map_sunstrikenexttime;
        }
        public Planet(System.IO.BinaryReader br)
        {
            name = br.ReadString();
            albedo = br.ReadDouble();
            semimajoraxis = br.ReadDouble();
            semiminoraxis = br.ReadDouble();
            sidusperiod = br.ReadDouble();
            orbitalspeed = br.ReadDouble();
            size = br.ReadDouble();
            radius = br.ReadDouble();
            mass = br.ReadDouble();
            rotateperiod = br.ReadDouble();
            axistilt = br.ReadSingle();
            atmosherepleasure = br.ReadDouble();
            mintemperature = br.ReadDouble();
            maxtemperature = br.ReadDouble();
            atmosherepleasureoffcet = br.ReadDouble();
            temperatureoffcet = br.ReadDouble();

            int count = br.ReadInt32();
            maps = new List<Map>();
            for (int i = 0; i < count; i++)
                maps.Add(new Map());

                unitgroups = new List<UnitGroup>();
            modules = new List<PlanetModule>();
            meteorites = new List<Meteorite>();

            timetosolarstrike = Constants.Map_sunstrikenexttime;
        }
        public Planet(byte[] description)
        {
            UnpackedDescription(description);

            unitgroups = new List<UnitGroup>();
            modules = new List<PlanetModule>();
            meteorites = new List<Meteorite>();

            timetosolarstrike = Constants.Map_sunstrikenexttime;
        }

        public void GenerateMap()
        {
            Random rand = new Random();

            mapwidth = (int)(Math.Pow(((radius - 2000) / 23000), 2) * 896) + 128;
            while (mapwidth % 8 != 0) mapwidth++;
            mapheight = mapwidth / 2;

            int scale = mapheight / 4;
            int noizewidth = mapwidth / scale;
            int noizeheight = mapheight / scale;

            int[,] noize = new int[noizeheight, noizewidth];
            heightmap = new float[mapheight, mapwidth];
            watermap = new bool[mapheight, mapwidth];

            for (int i = 0; i < noizeheight; i++)
                for (int j = 0; j < noizewidth; j++)
                    noize[i, j] = rand.Next(255);

            float min = 1000, max = 0;

            int sample = 3;
            for (int i = 0; i < mapwidth * mapheight; i++)
            {
                int y = i / mapwidth;
                int x = i % mapwidth;

                heightmap[y, x] = 0;
                float weight = 1;

                for (int k = 0; k < sample; k++)
                {
                    int nx = (int)(x / weight) % mapwidth;
                    int ny = (int)(y / weight) % mapheight;
                    heightmap[y, x] += PlanetHelper.BiCubicTexture(noize, noizewidth, noizeheight, (float)nx / scale, (float)ny / scale) * weight;
                    weight *= 0.5f;
                }
                if (min > heightmap[y, x]) min = heightmap[y, x];
                if (max < heightmap[y, x]) max = heightmap[y, x];
            }

            for (int i = 0; i < mapwidth * mapheight; i++)
            {
                int y = i / mapwidth;
                int x = i % mapwidth;

                heightmap[y, x] = (heightmap[y, x] - min) / (max - min) * 2 - 1;
                watermap[y, x] = heightmap[y, x] <= 0;
            }

            for (int k = 0; k < 50; k++)
            {
                float sx = (float)rand.Next(mapwidth);
                float sy = (float)rand.Next(mapheight);

                do
                {
                    float val = PlanetHelper.BiCubicTexture(heightmap, mapwidth, mapheight, sx, sy);
                    float nx = 0, ny = 0;

                    int n = mapheight;
                    float size = 1;
                    for (int i = 0; i < n; i++)
                    {
                        Vector2 newn = new Vector2((float)Math.Cos(i / (2 * Math.PI)) * size, (float)Math.Sin(i / (2 * Math.PI)) * size);
                        float newval = PlanetHelper.BiCubicTexture(heightmap, mapwidth, mapheight, sx + newn.X, sy + newn.Y);
                        if (newval < val)
                        {
                            val = newval;
                            newn = Vector2.Transform(newn, Matrix.CreateRotationZ((float)((rand.NextDouble() - 0.5f) * Math.PI / 16)));
                            nx = newn.X;
                            ny = newn.Y;
                        }
                    }

                    for (float i = 0; i < size; i += 1)
                    {
                        int px = (int)(sx + nx / size);
                        int py = (int)(sy + ny / size);

                        if (px >= mapwidth) { px -= mapwidth; sx -= mapwidth; }
                        if (py >= mapheight) { py -= mapheight; sy -= mapheight; }
                        if (px < 0) { px += mapwidth; sx += mapwidth; }
                        if (py < 0) { py += mapheight; sy += mapheight; }

                        watermap[py, px] = true;
                        sx += nx / size; sy += ny / size;
                        sx %= mapwidth; sy %= mapheight;

                        if (heightmap[(int)sy, (int)sx] <= 0 || (nx == 0 && ny == 0)) break;
                    }

                    if (heightmap[(int)sy, (int)sx] <= 0) break;

                    if (nx == 0 && ny == 0)
                    {
                        int px = (int)(sx + nx / size);
                        int py = (int)(sy + ny / size);

                        if (px >= mapwidth) { px -= mapwidth; sx -= mapwidth; }
                        if (py >= mapheight) { py -= mapheight; sy -= mapheight; }
                        if (px < 0) { px += mapwidth; sx += mapwidth; }
                        if (py < 0) { py += mapheight; sy += mapheight; }

                        for (int i = -2; i < 2; i++)
                            for (int j = -2; j < 2; j++)
                            {
                                if (Math.Sqrt(i * i + j * j) <= 2)
                                {
                                    int kx = (px + j + mapwidth) % mapwidth, ky = (py + i + mapheight) % mapheight;
                                    watermap[ky, kx] = true;
                                }
                            }
                        break;
                    }

                } while (true);
            }
        }

        public void Update(double ellapsedTime, double totalTime)
        {
            for (int i = unitgroups.Count - 1; i >= 0; i--)
            {
                UnitGroup ug = unitgroups[i];

                Point[] offcets = new Point[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };
                Vector2 direction = new Vector2(ug.baseposiitionx, ug.baseposiitiony) - ug.position;

                if (direction.Length() <= ellapsedTime)
                {
                    int base_id = GetBaseId(ug.baseposiitionx, ug.baseposiitiony);
                    if (base_id >= 0)
                    {
                        bool ok = false;
                        for (int k = 0; k < ug.units.Count; k++)
                        {
                            maps[base_id].units.Add(ug.units[k]);

                            if (ug.units[k].type == Unit.Pirate || ug.units[k].type == Unit.AttackDrone)
                                ok = true;
                        }

                        if (ok)
                            maps[base_id].AddMessage(Language.Message_OnAttack);
                    }

                    unitgroups.RemoveAt(i);
                }

                foreach (Point p in offcets)
                {
                    Vector2 direction2 = new Vector2(ug.baseposiitionx + mapwidth * p.X, ug.baseposiitiony + mapheight * p.Y) - ug.position;

                    if (direction2.Length() < direction.Length()) direction = direction2;
                }

                direction.Normalize();
                ug.position = ug.position + direction * (float)ellapsedTime * 13;

                ug.position.X += mapwidth; while (ug.position.X >= mapwidth) ug.position.X -= mapwidth;
                ug.position.Y += mapheight; while (ug.position.Y >= mapheight) ug.position.Y -= mapheight;

                if (Math.Abs(ug.position.X - ug.baseposiitionx) < 4 && Math.Abs(ug.position.Y - ug.baseposiitiony) < 4)
                {
                    int base_id = GetBaseId(ug.baseposiitionx, ug.baseposiitiony);
                    if (base_id >= 0)
                    {
                        for (int k = 0; k < ug.units.Count; k++)
                            maps[base_id].units.Add(ug.units[k]);
                    }

                    unitgroups.RemoveAt(i);
                }
            }

            Random rand = new Random();
            rand.Next();

            if (ellapsedTime > 0)
            {
                int e = (int)(1 / ellapsedTime);
                if (e > 0)
                {
                    double time = star.GetTotalTimeForLighting() - 100 * ((int)star.GetTotalTimeForLighting() / 100);
                    bool timetotraders = ((ulong)(time * 10 * e) % (2 * 10 * 40) == 0);
                    bool timetohumans = ((ulong)(time * 10 * e) % (2 * 10 * 50) == 0);

                    for (int i = maps.Count - 1; i >= 0; i--)
                    {
                        UnitGroup ug = new UnitGroup();
                        short player = (short)rand.Next(32);
                        ug.player_id = player;

                        maps[i].Update((float)ellapsedTime, (float)totalTime);
                        if (timetotraders)
                        {
                            Vector2 pos = maps[i].GetRandomBaseDirection();

                            if (maps[i].isBuildingBuilded(Building.Exchanger) && maps[i].isBuildingBuilded(Building.Spaceport))
                            {
                                for (int j = 0; j < maps[i].buildings.Count; j++)
                                {
                                    if (maps[i].buildings[j].type == Building.Parking && maps[i].buildings[j].buildingtime <= 0)
                                    {
                                        if (rand.Next(4) == 0)
                                        {
                                            Unit u = new Unit(Unit.Merchant, pos.X, pos.Y);
                                            u.tar = maps[i].buildings[j].pos;
                                            u.command = new Command(commands.tradergoin, j);
                                            u.height = 3;
                                            u.player_id = player;
                                            ug.units.Add(u);
                                        }
                                    }
                                }
                            }
                        }
                        if (timetohumans)
                        {
                            Vector2 pos = maps[i].GetRandomBaseDirection();

                            if (maps[i].population < maps[i].storageinfo[Constants.Map_humans].maxcapability && maps[i].isBuildingBuilded(Building.Spaceport) && maps[i].inventory[(int)Resources.water].count > 0 &&
                                                                               (maps[i].inventory[(int)Resources.meat].count > 0 ||
                                                                               maps[i].inventory[(int)Resources.vegetables].count > 0 ||
                                                                               maps[i].inventory[(int)Resources.fruits].count > 0 ||
                                                                               maps[i].inventory[(int)Resources.fish].count > 0))
                            {
                                for (int j = 0; j < maps[i].buildings.Count; j++)
                                {
                                    if (maps[i].buildings[j].type == Building.Parking && maps[i].buildings[j].buildingtime <= 0)
                                    {
                                        Unit u = new Unit(Unit.HumanShip, pos.X, pos.Y);
                                        u.tar = maps[i].buildings[j].pos;
                                        u.command = new Command(commands.humangoin, j);
                                        u.height = 4;
                                        u.player_id = player;
                                        ug.units.Add(u);
                                    }
                                }
                            }
                        }

                        if (ug.units.Count > 0)
                        {
                            ug.planetid_target = id;
                            ug.baseposiitionx = (int)maps[i].position.X;
                            ug.baseposiitiony = (int)maps[i].position.Y;
                            ug.position = Vector2.Transform(new Vector2(15, 0), Matrix.CreateRotationZ((int)star.GetTotalTimeForLighting() * 8));
                            star.unitgroups.Add(ug);
                        }

                        if (maps[i].timetodestroy <= 0)
                        {
                            if (maps[i].player_id >= 0 && maps[i].player_id < star.players.Count) star.players[maps[i].player_id].credits += maps[i].inventory[(int)Resources.credits].count;
                            maps.RemoveAt(i);
                        }
                    }
                }

                for (int i = modules.Count - 1; i >= 0; i--)
                {
                    if (modules[i].type == 0)
                    {
                        //pleasure = 101
                        //temp = 20

                        double pleasuredif = Math.Sign(101 - atmosherepleasure);
                        double tempdif = Math.Sign(20 - (mintemperature + maxtemperature) / 2);

                        temperatureoffcet += tempdif * modules[i].power;
                        atmosherepleasureoffcet += pleasuredif * modules[i].power;

                        Vector2 dir = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ((float)totalTime / 10 + i));
                        modules[i].pos = modules[i].pos + dir * (float)ellapsedTime / 2;

                        if (modules[i].pos.X < 0) modules[i].pos.X = mapwidth;
                        if (modules[i].pos.Y < 0) modules[i].pos.Y = mapheight;
                        if (modules[i].pos.X >= mapwidth) modules[i].pos.X = 0;
                        if (modules[i].pos.Y >= mapheight) modules[i].pos.Y = 0;
                    }

                    if (modules[i].base_id >= maps.Count || maps[modules[i].base_id].currentresearchmode != 1) modules.RemoveAt(i);
                }

                if (temperatureoffcet != 0)
                {
                    temperatureoffcet -= Math.Sign(temperatureoffcet) * ellapsedTime / 5;
                    if (temperatureoffcet < 0.005) temperatureoffcet = 0;
                }
                if (atmosherepleasureoffcet != 0)
                {
                    atmosherepleasureoffcet -= Math.Sign(atmosherepleasureoffcet) * ellapsedTime / 6;
                    if (atmosherepleasureoffcet < 0.005) atmosherepleasureoffcet = 0;
                }

                for (int i = meteorites.Count - 1; i >= 0; i--)
                {
                    if (meteorites[i].timetohit > 0)
                    {
                        meteorites[i].timetohit -= (float)ellapsedTime;
                        if (meteorites[i].timetohit <= 0)
                        {
                            int base_id = GetBaseId((int)meteorites[i].pos.X, (int)meteorites[i].pos.Y);
                            if (base_id >= 0)
                            {
                                rand.Next(); rand.Next(); rand.Next(); rand.Next();
                                Meteorite m = new Meteorite(new Vector2(rand.Next(maps[base_id].width), rand.Next(maps[base_id].height)), 1, 3);
                                m.timetodestroy = 0;
                                maps[base_id].meteorites.Add(m);
                            }
                        }
                    }
                    else
                    {
                        if (meteorites[i].timetodestroy > 0) meteorites[i].timetodestroy -= (float)ellapsedTime;
                        else
                            meteorites.RemoveAt(i);
                    }
                }

                //launch meteorites
                if (star.meteorites)
                    if (rand.Next((int)GetMeteoriteChance((float)ellapsedTime)) == 0)
                    {
                        Meteorite m = new Meteorite(new Vector2(rand.Next(mapwidth), rand.Next(mapheight)), 1, 3);
                        if (maps.Count > 0 && rand.Next(10) == 0)
                        {
                            int id = rand.Next(maps.Count);
                            m.pos = maps[id].position + new Vector2(rand.Next(9) - 4, rand.Next(9) - 4);
                        }
                        meteorites.Add(m);
                    }

                //launch pirates
                if (star.pirates)
                    if (rand.Next((int)GetPirateChance((float)ellapsedTime)) == 0)
                    {
                        int q = 0;
                        foreach (Map m in maps)
                            if (rand.Next(3) == 0)
                            {
                                UnitGroup ug = new UnitGroup();
                                Vector2 start = m.GetRandomBaseDirection();
                                short player = (short)rand.Next(32);
                                ug.player_id = player;
                                foreach (Building b in m.buildings)
                                    if (rand.Next(8) == 0 && b.power > 0 && b.buildingtime <= 0)
                                    {
                                        Unit u = new Unit(Unit.Pirate, start.X, start.Y);
                                        u.command = new Command(commands.piraterob, (int)b.pos.X, (int)b.pos.Y);
                                        u.tar = b.pos;
                                        u.player_id = player;
                                        ug.units.Add(u);
                                    }

                                if (ug.units.Count > 0)
                                {
                                    ug.planetid_target = id;
                                    ug.baseposiitionx = (int)m.position.X;
                                    ug.baseposiitiony = (int)m.position.Y;
                                    ug.position = Vector2.Transform(new Vector2(15 + q, 0), Matrix.CreateRotationZ((int)star.GetTotalTimeForLighting() * 8));
                                    star.unitgroups.Add(ug);
                                    q++;
                                }
                            }
                    }
            }

            if (star.sunstrike)
            {
                timetosolarstrike -= (float)ellapsedTime;
                if (timetosolarstrike <= 0)
                {
                    foreach (Map m in maps)
                    {
                        float lighting = GetLighting((int)m.position.X, (int)m.position.Y, (float)star.GetTotalTimeForLighting());
                        if (lighting > 0.1f)
                        {
                            m.AddMessage(Language.Message_Sunstrike);
                            if (lighting > 1) lighting = 1;
                            foreach (Unit u in m.units)
                                if (u.type == Unit.Drone && !m.onShield(u.pos.X, u.pos.Y, Shield.Emmision))
                                {
                                    u.wait = Constants.Map_sunstrikeunitwaittime * ((lighting - 0.1f) / 0.9f);
                                    u.maxwait = Constants.Map_sunstrikeunitwaittime;
                                }
                        }
                    }
                    timetosolarstrike = (float)(Constants.Map_sunstrikenexttime + star.radius * Constants.Map_sunstrikenexttime + id);
                }
            }
        }
        public void UpdateOnline(double ellapsedTime, double totalTime)
        {
            for (int i = unitgroups.Count - 1; i >= 0; i--)
            {
                UnitGroup ug = unitgroups[i];

                Point[] offcets = new Point[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };
                Vector2 direction = new Vector2(ug.baseposiitionx, ug.baseposiitiony) - ug.position;

                if (direction.Length() <= ellapsedTime)
                {
                    int base_id = GetBaseId(ug.baseposiitionx, ug.baseposiitiony);
                    if (base_id >= 0)
                    {
                        bool ok = false;
                        for (int k = 0; k < ug.units.Count; k++)
                        {
                            maps[base_id].units.Add(ug.units[k]);

                            if (ug.units[k].type == Unit.Pirate || ug.units[k].type == Unit.AttackDrone)
                                ok = true;
                        }

                        if (ok)
                            maps[base_id].AddMessage(Language.Message_OnAttack);
                    }

                    unitgroups.RemoveAt(i);
                }

                foreach (Point p in offcets)
                {
                    Vector2 direction2 = new Vector2(ug.baseposiitionx + mapwidth * p.X, ug.baseposiitiony + mapheight * p.Y) - ug.position;

                    if (direction2.Length() < direction.Length()) direction = direction2;
                }

                direction.Normalize();
                ug.position = ug.position + direction * (float)ellapsedTime * 13;

                ug.position.X += mapwidth; while (ug.position.X >= mapwidth) ug.position.X -= mapwidth;
                ug.position.Y += mapheight; while (ug.position.Y >= mapheight) ug.position.Y -= mapheight;

                if (Math.Abs(ug.position.X - ug.baseposiitionx) < 4 && Math.Abs(ug.position.Y - ug.baseposiitiony) < 4)
                {
                    int base_id = GetBaseId(ug.baseposiitionx, ug.baseposiitiony);
                    if (base_id >= 0 && maps[base_id].units!=null)
                    {
                        for (int k = 0; k < ug.units.Count; k++)
                            maps[base_id].units.Add(ug.units[k]);
                    }

                    unitgroups.RemoveAt(i);
                }
            }

            if(maps!=null)
            for (int i = maps.Count - 1; i >= 0; i--)
            {
                maps[i].UpdateOnline((float)ellapsedTime, (float)totalTime);

                if (maps[i].timetodestroy <= 0 && maps[i].data!=null) maps.RemoveAt(i);
            }

            for (int i = modules.Count - 1; i >= 0; i--)
            {
                if (modules[i].type == 0)
                {
                    //pleasure = 101
                    //temp = 20

                    double pleasuredif = Math.Sign(101 - atmosherepleasure);
                    double tempdif = Math.Sign(20 - (mintemperature + maxtemperature) / 2);

                    temperatureoffcet += tempdif * modules[i].power;
                    atmosherepleasureoffcet += pleasuredif * modules[i].power;

                    Vector2 dir = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ((float)totalTime / 10 + i));
                    modules[i].pos = modules[i].pos + dir * (float)ellapsedTime / 2;

                    if (modules[i].pos.X < 0) modules[i].pos.X = mapwidth;
                    if (modules[i].pos.Y < 0) modules[i].pos.Y = mapheight;
                    if (modules[i].pos.X >= mapwidth) modules[i].pos.X = 0;
                    if (modules[i].pos.Y >= mapheight) modules[i].pos.Y = 0;
                }

                if (modules[i].base_id >= maps.Count || maps[modules[i].base_id].currentresearchmode != 1) modules.RemoveAt(i);
            }

            if (temperatureoffcet != 0)
            {
                temperatureoffcet -= Math.Sign(temperatureoffcet) * ellapsedTime / 5;
                if (temperatureoffcet < 0.005) temperatureoffcet = 0;
            }
            if (atmosherepleasureoffcet != 0)
            {
                atmosherepleasureoffcet -= Math.Sign(atmosherepleasureoffcet) * ellapsedTime / 6;
                if (atmosherepleasureoffcet < 0.005) atmosherepleasureoffcet = 0;
            }

            for (int i = meteorites.Count - 1; i >= 0; i--)
            {
                if (meteorites[i].timetohit > 0)
                {
                    meteorites[i].timetohit -= (float)ellapsedTime;
                    if (meteorites[i].timetohit <= 0)
                    {
                        int base_id = GetBaseId((int)meteorites[i].pos.X, (int)meteorites[i].pos.Y);
                        if (base_id >= 0 && maps[base_id].meteorites!=null)
                        {
                            Random rand = new Random();
                            rand.Next(); rand.Next(); rand.Next(); rand.Next();
                            Meteorite m = new Meteorite(new Vector2(rand.Next(maps[base_id].width), rand.Next(maps[base_id].height)), 1, 10);
                            maps[base_id].meteorites.Add(m);
                        }
                    }
                }
                else
                {
                    if (meteorites[i].timetodestroy > 0) meteorites[i].timetodestroy -= (float)ellapsedTime;
                    else
                        meteorites.RemoveAt(i);
                }
            }

            timetosolarstrike -= (float)ellapsedTime;
            if (timetosolarstrike <= 0)
            {
                if (maps != null)
                foreach (Map m in maps)
                {
                    float lighting = GetLighting((int)m.position.X, (int)m.position.Y, (float)star.GetTotalTimeForLighting());
                    if (lighting > 0.1f)
                    {
                        m.AddMessage(Language.Message_Sunstrike);
                        if (lighting > 1) lighting = 1;
                        foreach (Unit u in m.units)
                            if (u.type == Unit.Drone && !m.onShield(u.pos.X, u.pos.Y, Shield.Emmision))
                            {
                                u.wait = Constants.Map_sunstrikeunitwaittime * ((lighting - 0.1f) / 0.9f);
                                u.maxwait = Constants.Map_sunstrikeunitwaittime;
                            }
                    }
                }
            }
        }

        public bool CreateBaseRelief(int x, int y, ref Map map)
        {
            //foreach (Vector2 v in basepositions)
            foreach(Map m in maps)
                if (Math.Abs(m.position.X - x) + Math.Abs(m.position.Y - y) < 16) return false;

            int size = 8;

            Random r = new Random();
            float[,] height = new float[64, 64];
            bool[,] water = new bool[64, 64];

            float min = 1000, max = -1000;

            #region SetHeight
            for (int i = 0; i < map.height; i++)
            {
                for (int j = 0; j < map.width; j++)
                {
                    float dy = (i - map.height / 2) / (float)(map.height) * size;
                    float dx = (j - map.width / 2) / (float)(map.width) * size;

                    height[i, j] = PlanetHelper.BiCubicTexture(heightmap, mapwidth, mapheight, x + dx, y + dy);

                    if (min > height[i, j]) min = height[i, j];
                    if (max < height[i, j]) max = height[i, j];

                    if (PlanetHelper.BiCubicTexture(watermap, mapwidth, mapheight, x + dx, y + dy) > 0.5f) water[i, j] = true;
                }
            }
            #endregion

            if (max < 0) return false;

            #region Set main IDs
            for (int i = 0; i < map.height; i++)
            {
                for (int j = 0; j < map.width; j++)
                {
                    if (i == 0 || i == map.height - 1 || j == 0 || j == map.width - 1)
                    {
                        map.data[i, j].can_build = false;
                        map.data[i, j].can_move = false;
                    }

                    if (height[i, j] < 0 || water[i, j])
                    {
                        map.data[i, j].id = 0;
                        map.data[i, j].height = 0;
                    }
                    else
                    {
                        if (height[i, j] == 1) map.data[i, j].height = 15;
                        else map.data[i, j].height = (short)((int)(height[i, j] * 15) + 1);

                        map.data[i, j].height *= 2;

                        map.data[i, j].id = 16;
                    }
                }
            }
            #endregion

            #region Set height-rocks IDs
            for (int i = 0; i < map.height; i++)
            {
                for (int j = 0; j < map.width; j++)
                {
                    int[,] h = new int[3, 3];

                    h[0, 0] = i == 0 || j == 0 ? map.data[i, j].height : map.data[i - 1, j - 1].height;
                    h[0, 1] = i == 0 ? map.data[i, j].height : map.data[i - 1, j].height;
                    h[0, 2] = i == 0 || j == map.width - 1 ? map.data[i, j].height : map.data[i - 1, j + 1].height;

                    h[1, 0] = j == 0 ? map.data[i, j].height : map.data[i, j - 1].height;
                    h[1, 1] = map.data[i, j].height;
                    h[1, 2] = j == map.width - 1 ? map.data[i, j].height : map.data[i, j + 1].height;

                    h[2, 0] = i == map.height - 1 || j == 0 ? map.data[i, j].height : map.data[i + 1, j - 1].height;
                    h[2, 1] = i == map.height - 1 ? map.data[i, j].height : map.data[i + 1, j].height;
                    h[2, 2] = i == map.height - 1 || j == map.width - 1 ? map.data[i, j].height : map.data[i + 1, j + 1].height;


                    #region diagonal
                    if (h[1, 1] > h[1, 0] && h[1, 1] == h[1, 2] && h[1, 1] > h[0, 1] && h[1, 1] == h[2, 1])
                    {
                        short up = i == 0 ? map.data[i, j].id : map.data[i - 1, j].id;
                        map.data[i, j].ground_id = up;
                        map.data[i, j].id = 1;
                    }
                    else if (h[1, 1] > h[1, 2] && h[1, 1] == h[1, 0] && h[1, 1] > h[0, 1] && h[1, 1] == h[2, 1])
                    {
                        short up = i == 0 ? map.data[i, j].id : map.data[i - 1, j].id;
                        map.data[i, j].ground_id = up;
                        map.data[i, j].id = 3;
                    }
                    else if (h[1, 1] > h[1, 0] && h[1, 1] == h[1, 2] && h[1, 1] > h[2, 1] && h[1, 1] == h[0, 1])
                    {
                        map.data[i, j].id = 1 + 16 + 16;
                        map.data[i + 1, j].id = 1 + 16 + 16 + 16;
                        if (i < map.height - 2)
                        {
                            map.data[i + 2, j].ground_id = map.data[i + 2, j].id;
                            map.data[i + 2, j].id = 1 + 16 + 16 + 16 + 16;
                        }
                    }
                    else if (h[1, 1] > h[1, 2] && h[1, 1] == h[1, 0] && h[1, 1] > h[2, 1] && h[1, 1] == h[0, 1])
                    {
                        map.data[i, j].id = 3 + 16 + 16;
                        map.data[i + 1, j].id = 3 + 16 + 16 + 16;
                        if (i < map.height - 2)
                        {
                            map.data[i + 2, j].ground_id = map.data[i + 2, j].id;
                            map.data[i + 2, j].id = 3 + 16 + 16 + 16 + 16;
                        }
                    }
                    #endregion

                    else if (h[1, 1] > h[0, 1]) map.data[i, j].id = 2;
                    else if (h[1, 1] > h[2, 1])
                    {
                        map.data[i, j].id = 2 + 16 + 16;
                        map.data[i + 1, j].id = 2 + 16 + 16 + 16;
                        if (i < map.height - 2) map.data[i + 2, j].id = 2 + 16 + 16 + 16 + 16;
                    }
                    else if (h[1, 1] > h[1, 0]) map.data[i, j].id = 1 + 16;
                    else if (h[1, 1] > h[1, 2]) map.data[i, j].id = 3 + 16;
                }
            }
            #endregion

            #region Stairs

            for (int i = 0; i < map.height - 4; i++)
                for (int j = 0; j < map.width - 4; j++)
                {
                    foreach (MapHelper_Engine.ReplaceData d in MapHelper_Engine.templates)
                    {
                        bool ok = true;
                        for (int k = 0; k < d.ey; k++)
                        {
                            if (!ok) break;
                            for (int l = 0; l < d.ex; l++)
                                if (d.olddata[k, l] != -1 && d.olddata[k, l] != map.data[i + k, j + l].id) { ok = false; break; }
                        }

                        if (ok)
                        {
                            short nh = (short)(map.data[i + d.py, j + d.px].height - 1);
                            for (int k = 0; k < d.h; k++)
                            {
                                for (int l = 0; l < d.w; l++)
                                {
                                    if (d.newdata[k, l] > 0)
                                    {
                                        if (d.subground[k, l] == 1)
                                        {
                                            map.data[i + k, j + l].ground_id = map.data[i + k, j + l].id;
                                            map.data[i + k, j + l].subground = true;
                                            map.data[i + k, j + l].id = d.newdata[k, l];
                                            map.data[i + k, j + l].id = d.newdata[k, l];
                                        }
                                        else if (d.subground[k, l] == 2)
                                        {
                                            if (map.data[i + k, j + l].ground_id == -1)
                                            {
                                                if (map.data[i + k, j + l].id != 2)
                                                {
                                                    map.data[i + k, j + l].ground_id = map.data[i + k, j + l].id;
                                                    map.data[i + k, j + l].id = d.newdata[k, l];
                                                }
                                            }
                                            else
                                            {
                                                if (map.data[i + k, j + l].ground_id == 0) map.data[i + k, j + l].height = 0;
                                                else map.data[i + k, j + l].height = nh;
                                                map.data[i + k, j + l].ground_id = d.newdata[k, l];
                                                map.data[i + k, j + l].subground = true;
                                            }
                                        }
                                        else if (d.subground[k, l] == 0)
                                        {
                                            map.data[i + k, j + l].height = nh;
                                            map.data[i + k, j + l].id = d.newdata[k, l];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            #endregion

            #region SetPassabilityAndDecoraions
            List<int> trees = new List<int>();
            List<int> deadtrees = new List<int>();

            double temperature = (maxtemperature + mintemperature) / 2;
            if (temperature < 0)
            {
                trees.Add(160 + 5);
                deadtrees.Add(160 + 6);
            }
            else if (temperature < 60)
            {
                trees.Add(112);
                trees.Add(112 + 1);
                trees.Add(112 + 2);
                deadtrees.Add(112 + 3);
                deadtrees.Add(112 + 4);
            }
            else if (temperature < 220)
            {
                trees.Add(160 + 7);
                deadtrees.Add(160 + 8);
            }
            else if (temperature < 400)
            {
                trees.Add(160);
                trees.Add(160 + 1);
                trees.Add(160 + 2);
                deadtrees.Add(160 + 3);
                deadtrees.Add(160 + 4);
            }
            else if (temperature < 620)
            {
                trees.Add(160 + 9);
                deadtrees.Add(160 + 10);
            }
            else deadtrees.Add(160 + 10);
            

            for (int i = 0; i < map.height; i++)
            {
                for (int j = 0; j < map.width; j++)
                {
                    if ((map.data[i, j].id == 16 || map.data[i, j].id == 2 + 16) && r.Next(50) == 0)
                    {
                        if (r.Next(3) == 0 && trees.Count > 0) map.data[i, j].id = (short)trees[r.Next(trees.Count)];
                        else map.data[i, j].id = (short)deadtrees[r.Next(deadtrees.Count)];
                    }
                    if ((map.data[i, j].id == 16 || map.data[i, j].id == 2 + 16) && r.Next(4) == 0)
                    {
                        map.data[i, j].id = (short)(32 + r.Next(3)*16);
                    }

                    map.data[i, j].can_build = map.data[i, j].can_move = MapHelper_Engine.enabledata[map.data[i, j].id] == 0;
                    if (i == 0 || i == map.height - 1 || j == 0 || j == map.width - 1) map.data[i, j].can_build = false;
                }
            }
            #endregion

            return true;
        }
        public bool TestCreateBase(int x, int y)
        {
            Map map = new Map(64, 64);

            if (CreateBaseRelief(x, y, ref map))
            {
                int[,] way = new int[4, 2] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
                for (int q = 0; q < 4; q++)
                {
                    int bx = 32;
                    int by = 32;
                    do
                    {
                        map.AddBuilding(bx, by, Building.CommandCenter, true);
                        bx += way[q, 0];
                        by += way[q, 1];
                    } while (map.buildings.Count == 0 && bx < map.width - 6 && by < map.height - 4 && bx >= 0 && by >= 0);

                    if (bx < map.width - 6 && by < map.height - 4 && bx >= 0 && by >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public bool CreateBase(int x, int y,short id)
        {
            Map map = new Map(64, 64);
            Random rand = new Random();
            if (CreateBaseRelief(x, y, ref map))
            {
                #region Set resources

                for (int i = 0; i < map.height; i++)
                    for (int j = 0; j < map.width; j++)
                    {
                        bool sand = false;
                        bool glay = false;

                        for (int k = -4; k <= 6; k++)
                            for (int l = -4; l <= 6; l++)
                            {
                                int px = j + l;
                                int py = i + k;
                                if (map.onMap(px, py))
                                {
                                    //if water not far
                                    if (map.data[py, px].id == 0)
                                    {
                                        sand = rand.Next(Math.Abs(k)) == 0;
                                        glay = rand.Next(Math.Abs(l)) == 0;
                                    }
                                }
                            }

                        //-------------------mine
                        if (sand && map.data[i, j].id != 0) map.data[i, j].mineresource_id = (int)Resources.sand;
                        if (glay && map.data[i, j].id != 0 && map.data[i, j].height > 1 && rand.Next(3) == 0) map.data[i, j].mineresource_id = (int)Resources.glay;
                        #region fosphats
                        if (map.data[i, j].height > 8 && rand.Next(600) == 0)
                        {
                            int len = rand.Next(10) + 10;
                            int sx = j;
                            int sy = i;
                            int ex = j + len;
                            int ey = i - len;
                            int rex = ex >= map.width ? map.width : ex;
                            int rey = ey < 0 ? 0 : ey;

                            int py = sy;
                            for (int px = sx; px <= rex && py >= rey; px++, py--)
                            {
                                int size = rand.Next(Math.Min(px - sx, ex - px) / 2) + 1;
                                for (int k = -size / 2; k <= size / 2; k++)
                                    for (int l = -size / 2; l <= size / 2; l++)
                                    {
                                        int kx = px + l;
                                        int ky = py + k;
                                        //set fosphat
                                        if (map.onMap(kx, ky) && (map.data[ky, kx].id != 0))
                                        {
                                            map.data[ky, kx].mineresource_id = (int)Resources.fosphat;
                                        }
                                    }

                            }
                        }
                        #endregion
                        #region ore
                        if (map.data[i, j].id != 0 && rand.Next(Math.Abs(300 + 100 * map.data[i, j].height / 4)) == 0)
                        {
                            int steps = rand.Next(4) + 4;
                            int sx = j;
                            int sy = i;
                            for (int k = 0; k < steps; k++)
                            {
                                int len = rand.Next(4) + 8;
                                float dx = (float)(rand.Next(8) - 4);
                                float dy = (float)(rand.Next(8) - 4);
                                if (dx == 0) dx++; if (dy == 0) dy++;
                                float lxy = (float)Math.Sqrt(dx * dx + dy * dy);
                                dx /= lxy; dy /= lxy;

                                float px = sx;
                                float py = sy;

                                for (int q = 0; q < len; q++)
                                {
                                    if (py < 0) py += map.height;
                                    if (px < 0) px += map.width;
                                    if (py >= map.height) py -= map.height;
                                    if (px >= map.width) px -= map.width;

                                    map.data[(int)py, (int)px].mineresource_id = (int)Resources.ore;
                                    py += dy; px += dx;
                                }
                                sx = (int)px;
                                sy = (int)py;
                            }
                        }
                        #endregion
                        #region coal
                        if (map.data[i, j].id != 0 && rand.Next(Math.Abs(400 + 100 * map.data[i, j].height / 4)) == 0)
                        {
                            int sx = i, sy = j;
                            int steps = rand.Next(4);
                            for (int q = 0; q < steps; q++)
                            {
                                int radius = rand.Next(3) + 2 - q;
                                if (radius < 0) radius = 2;
                                for (int k = -radius; k <= radius; k++)
                                    for (int l = -radius; l <= radius; l++)
                                    {
                                        if (map.onMap(sx + l, sy + k) && Math.Sqrt(k * k + l * l) <= radius) map.data[sy + k, sx + l].mineresource_id = (int)Resources.coal;
                                    }
                                sx += rand.Next(radius * 2) - radius;
                                sy += rand.Next(radius * 2) - radius;

                                if (sy < 0) sy += map.height;
                                if (sx < 0) sx += map.width;
                                if (sy >= map.height) sy -= map.height;
                                if (sx >= map.width) sx -= map.width;
                            }
                        }
                        #endregion
                        #region artifacts
                        if ((sand || glay) && map.data[i, j].id != 0 && rand.Next(1000) == 0)
                        {
                            Vector2 p = Vector2.UnitX * 4;

                            for (int q = 0; q < 16; q++)
                            {
                                for (int k = 0; k <= 1; k++)
                                    for (int l = 0; l <= 1; l++)
                                        if (map.onMap(j + (int)p.X + l, i + k + (int)p.Y))
                                            map.data[i + k + (int)p.Y, j + (int)p.X + l].mineresource_id = (int)Resources.artifacts;

                                p = Vector2.Transform(p, Matrix.CreateRotationZ((float)Math.PI / 8));
                            }
                        }
                        #endregion
                        #region gems
                        if (map.data[i, j].id != 0 && map.data[i, j].height > 11 && rand.Next(600) == 0)
                        {
                            int steps = rand.Next(4);

                            int radius = rand.Next(3) + 2;
                            if (radius < 0) radius = 2;
                            for (int k = -radius; k <= radius; k++)
                                for (int l = -radius; l <= radius; l++)
                                {
                                    if (map.onMap(j + l, i + k) && Math.Sqrt(k * k + l * l) <= radius) map.data[i + k, j + l].mineresource_id = (int)Resources.gems;
                                }
                        }
                        #endregion
                        #region energyore
                        if (map.data[i, j].id != 0 && rand.Next(1000) == 0)
                        {
                            int len = rand.Next(2) + 4;
                            int sx = j;
                            int sy = i;
                            int ex = j + len;
                            int ey = i - len;
                            int rex = ex >= map.width ? map.width : ex;
                            int rey = ey < 0 ? 0 : ey;

                            int py = sy;
                            for (int px = sx; px <= rex && py >= rey; px++, py--)
                            {
                                int size = rand.Next(Math.Min(px - sx, ex - px)) * 2 + 1;
                                for (int k = -size / 2; k <= size / 2 + size % 2; k++)
                                    for (int l = -size / 2; l <= size / 2 + size % 2; l++)
                                    {
                                        int kx = px + l;
                                        int ky = py + k;
                                        //set E-ore
                                        if (map.onMap(kx, ky) && (map.data[ky, kx].id != 0))
                                        {
                                            map.data[ky, kx].mineresource_id = (int)Resources.energyore;
                                        }
                                    }

                            }
                        }
                        #endregion

                        //--------------------dirrick
                        #region water
                        if (map.data[i, j].id != 0 &&map.data[i, j].height>0&& rand.Next(300 + 100 * map.data[i, j].height / 2) == 0)
                        {
                            int sx = i, sy = j;
                            int steps = rand.Next(4);
                            for (int q = 0; q < steps; q++)
                            {
                                int radius = rand.Next(3) + 3 - q;
                                if (radius < 0) radius = 2;
                                for (int k = -radius; k <= radius; k++)
                                    for (int l = -radius; l <= radius; l++)
                                    {
                                        if (map.onMap(sx + l, sy + k) && Math.Sqrt(k * k + l * l) <= radius) map.data[sy + k, sx + l].dirrickresource_id = (int)Resources.water;
                                    }
                                sx += rand.Next(radius * 2) - radius;
                                sy += rand.Next(radius * 2) - radius;

                                if (sy < 0) sy += map.height;
                                if (sx < 0) sx += map.width;
                                if (sy >= map.height) sy -= map.height;
                                if (sx >= map.width) sx -= map.width;
                            }
                        }
                        if (sand && map.data[i, j].id != 0 && rand.Next(20) == 0)
                            map.data[i, j].dirrickresource_id = (int)Resources.water;
                        #endregion
                        #region oil,metan,rare
                        if (map.data[i, j].id != 0 && rand.Next(Math.Abs(500 + 100 * map.data[i, j].height / 2)) == 0)
                        {
                            int sx = i, sy = j;
                            int steps = rand.Next(4) + 7;
                            for (int q = 0; q < steps; q++)
                            {
                                int res = rand.Next(6);
                                switch (res)
                                {
                                    case 0:
                                    case 1:
                                    case 2: res = (int)Resources.oil; break;
                                    case 3:
                                    case 4: res = (int)Resources.metan; break;
                                    case 5: res = (int)Resources.rare_gas; break;
                                }
                                int radius = rand.Next(2) + 1;
                                for (int k = -radius; k <= radius; k++)
                                    for (int l = -radius; l <= radius; l++)
                                    {
                                        if (map.onMap(sx + l, sy + k) && Math.Sqrt(k * k + l * l) <= radius) map.data[sy + k, sx + l].dirrickresource_id = (short)res;
                                    }
                                sx += rand.Next(radius * 2) - radius;
                                sy += rand.Next(radius * 2) - radius;

                                if (sy < 0) sy += map.height;
                                if (sx < 0) sx += map.width;
                                if (sy >= map.height) sy -= map.height;
                                if (sx >= map.width) sx -= map.width;
                            }
                        }
                        #endregion
                    }
                #endregion

                int[,] way = new int[4, 2] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
                for (int q = 0; q < 4; q++)
                {
                    int bx = 32;
                    int by = 32;
                    do
                    {
                        map.AddBuilding(bx, 32, Building.CommandCenter, true);
                        bx += way[q, 0];
                        by += way[q, 1];
                    } while (map.buildings.Count == 0 && bx < map.width - 6 && by < map.height - 4 && bx >= 0 && by >= 0);

                    if (bx < map.width - 6 && by < map.height - 4 && bx >= 0 && by >= 0 && star.TryPayMoney(1500, id))
                    {
                        string name = NameGenerator.GeneratePlanetName();// "База " + (this.maps.Count + 1);
                        map.AddDrone(bx, 33);
                        map.planet = this;
                        map.baseid = this.maps.Count;
                        map.position = new Vector2(x, y);
                        map.name = name;
                        map.player_id = id;
                        this.maps.Add(map);
                        return true;
                    }
                }
            }
            return false;
        }

        public Vector3 GetNomal(int x, int y)
        {
            if (heightmap[y, x] <= 0)
            {
                return Vector3.Normalize(new Vector3(0, 0, 1));
            }
            else
            {
                float tl = heightmap[(y + mapheight - 1) % mapheight, (x + mapwidth - 1) % mapwidth];
                float l = heightmap[(y + mapheight - 1) % mapheight, x];
                float bl = heightmap[(y + mapheight - 1) % mapheight, (x + 1) % mapwidth];
                float b = heightmap[y, (x + 1) % mapwidth];
                float br = heightmap[(y + 1) % mapheight, (x + 1) % mapwidth];
                float r2 = heightmap[(y + 1) % mapheight, x];
                float tr = heightmap[(y + 1) % mapheight, (x + mapwidth - 1) % mapwidth];
                float t = heightmap[y, (x + mapwidth - 1) % mapwidth];

                float dX = tr + 2 * r2 + br - tl - 2 * l - bl;
                float dY = bl + 2 * b + br - tl - 2 * t - tr;

                return Vector3.Normalize(new Vector3(dX, -dY, 0.15f));
            }
        }
        public Vector3 GetAbsoluteNomal(int x, int y)
        {
            if (heightmap[y, x] <= 0)
            {
                Vector3 normal = new Vector3((float)Math.Sin(x * Math.PI * 2 / mapwidth), (float)Math.Cos(x * Math.PI * 2 / mapwidth), (y / mapheight * 2 - 1));
                //return Vector3.Normalize(new Vector3(0, 0, 1));
                //return normal;
                return Vector3.Zero;
            }
            else
            {
                float tl = heightmap[(y + mapheight - 1) % mapheight, (x + mapwidth - 1) % mapwidth];
                float l = heightmap[(y + mapheight - 1) % mapheight, x];
                float bl = heightmap[(y + mapheight - 1) % mapheight, (x + 1) % mapwidth];
                float b = heightmap[y, (x + 1) % mapwidth];
                float br = heightmap[(y + 1) % mapheight, (x + 1) % mapwidth];
                float r2 = heightmap[(y + 1) % mapheight, x];
                float tr = heightmap[(y + 1) % mapheight, (x + mapwidth - 1) % mapwidth];
                float t = heightmap[y, (x + mapwidth - 1) % mapwidth];

                float dX = tr + 2 * r2 + br - tl - 2 * l - bl;
                float dY = bl + 2 * b + br - tl - 2 * t - tr;
                Vector3 texnormal = Vector3.Normalize(new Vector3(dX, -dY, 0.15f));

                Vector3 normal = new Vector3((float)Math.Sin(x * Math.PI * 2 / mapwidth), (float)Math.Cos(x * Math.PI * 2 / mapwidth), (y / mapheight * 2 - 1));
                Vector3 tangent = new Vector3((float)Math.Sin((x + 1) * Math.PI * 2 / mapwidth), (float)Math.Cos((x + 1) * Math.PI * 2 / mapwidth), (y / mapheight * 2 - 1));
                Vector3 binormal = new Vector3((float)Math.Sin(x * Math.PI * 2 / mapwidth), (float)Math.Cos(x * Math.PI * 2 / mapwidth), ((y + 1) / mapheight * 2 - 1));

                //return texnormal;
                //return Vector3.Normalize((tangent * texnormal.X + binormal * texnormal.Y));
                return (tangent * texnormal.X + binormal * texnormal.Y)/1.5f;
            }
        }
        public int GetBaseId(int x, int y)
        {
            for (int i = 0; i < maps.Count; i++)
            {
                Vector2 v = maps[i].position;
                if (new Rectangle((int)v.X - 4, (int)v.Y - 4, 8, 8).Contains(x, y)) return i;
            }
            return -1;
        }

        public float GetLightingWithoutRelief(int x, int y, float offcet)
        {
            float a = (float)x / mapwidth * 6.28f;
            Vector3 bumpNormal = new Vector3((float)Math.Sin(a + offcet), (float)Math.Cos(a + offcet), (-((float)y / mapheight * 2 - 1)));
            bumpNormal = Vector3.Normalize(bumpNormal);
            float dark = Vector3.Dot(Vector3.Normalize(new Vector3(0, 0.5f, 1)), bumpNormal) / 1.1f;
            if (dark > 0.1f)
            {
                float lightIntensity = (float)Math.Sqrt(Vector3.Dot(bumpNormal, new Vector3(0, 0.5f, 1)));
                dark *= lightIntensity * 2;
            }
            else dark = 0.1f;

            return dark;
        }
        public float GetLighting(int x, int y, float offcet)
        {
            float a = (float)x / mapwidth * 6.28f;
            Vector3 normal = new Vector3((float)Math.Sin(a + offcet), (float)Math.Cos(a + offcet), (-((float)y / mapheight * 2 - 1)));

            Vector3 tangent = Vector3.Cross(normal, Vector3.UnitX);
            Vector3 binormal = Vector3.Cross(normal, Vector3.UnitZ);
            Vector3 bump = GetNomal(x, y);
            Vector3 bumpNormal = normal + (bump.X * tangent + bump.Y * binormal);
            bumpNormal = Vector3.Normalize(bumpNormal);
            float dark = Vector3.Dot(Vector3.Normalize(new Vector3(0, 0.5f, 1)), bumpNormal) / 1.1f;
            if (dark > 0.1f)
            {
                float lightIntensity = (float)Math.Sqrt(Vector3.Dot(bumpNormal, new Vector3(0, 0.5f, 1)));
                dark *= lightIntensity * 2;
            }
            else dark = 0.1f;

            return dark;
        }

        public Vector2 GetPosition(int id)
        {
            double distance = semimajoraxis / star.radius * 0.00435;
            if (distance > 4.75) distance = 4.75;
            if (distance < 0.25) distance = 0.25;

            Vector2 pos = Vector2.UnitY;
            pos = Vector2.Transform(pos, Matrix.CreateRotationZ((float)(id + distance - star.GetTotalTimeForLighting() / distance / 7)));

            return pos;
        }

        public float GetMeteoriteChance(float time)
        {
            float offcet = 0;
            foreach (PlanetModule m in modules)
                if (m.type == 1) offcet += m.power * time;

            float chance = (float)(Constants.Map_meteoritenexttime * mass / 5 - maps.Count * 2 + offcet);
            if (chance > int.MaxValue) chance = int.MaxValue;
            if (chance < 0) chance = -chance;

            return chance;
        }
        public float GetPirateChance(float time)
        {
            float offcet = 0;
            foreach (PlanetModule m in modules)
                if (m.type == 1) offcet += m.power * time * 20;

            return (float)(5000 - maps.Count * 4 + offcet);
        }

        public void AddMesage(string message)
        {
            if (messages != null)
                messages = new List<string>();
            messages.Add(message);
        }
    }
}
