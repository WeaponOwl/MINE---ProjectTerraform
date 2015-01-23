using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    struct ReplaceData
    {
        public int w, h;
        public int[,] olddata;
        public int[,] newdata;
        public int[,] subground;
        public int ex, ey;
        public int px, py;

        public ReplaceData(int w, int h, int[,] o, int[,] n,int[,] s, int ex, int ey,int px,int py)
        {
            this.w = w;
            this.h = h;
            olddata = o;
            newdata = n;
            subground = s;
            this.ex = ex;
            this.ey = ey;
            this.px = px;
            this.py = py;
        }
    }

    class RequestData
    {
        public int resource;
        public int count;
        public float time;

        public RequestData(int r, int c, float t)
        {
            resource = r;
            count = c;
            time = t;
        }
    }

    class Star
    {
        public double radius;
        public double temperture;
        public double mass;

        public int planets_num;
        public int planetseed;

        public float turbulance;

        public Planet[] planets;

        static public Gradient starcolors;

        public Star(Random rand,NormalRandom nrand,double light=-1,double t=-1)
        {
            planetseed = rand.Next();
            if (starcolors == null) SetConstants();
            //0.00435 - radius of Sun in a.o.
            //8.7 = mass of sun in mass of erath/V of sun in a.o
            //  10^(-4*[-1...1])

            double startlighting = light < 0 ? Math.Abs(Math.Pow(10, 4 * Math.Abs(nrand.Next() / 10))) : light;
            radius = startlighting * 0.00435;                                       //a.o.
            mass = 960000000000 * 4 / 3 * Math.PI * Math.Pow(radius, 3);            //massus Earth

            float tx = (float)(rand.NextDouble() * 7);
            temperture = t < 0 ? (float)(26.389 * Math.Pow(tx, 6) - 550 * Math.Pow(tx, 5) + 4555.6 * Math.Pow(tx, 4) - 19042 * Math.Pow(tx, 3) + 41918 * Math.Pow(tx, 2) - 44408 * tx + 19500) : t;     //Kelvin

            planets_num = (int)nrand.Next(1, 9);
            if (planets_num < 0) planets_num = -planets_num;
            if (planets_num == 0) planets_num++;

            turbulance = (float)Math.Sqrt(Math.Sqrt(startlighting)) * Constants.Map_sunstrikenexttime;

            planets = new Planet[planets_num];
        }

        public void GeneratePlanets()
        {
            Random rand = new Random(planetseed);
            NormalRandom nrand = new NormalRandom(planetseed + 1);
            planets = new Planet[planets_num];
            for (int i = 0; i < planets_num; i++)
                planets[i] = new Planet(this, rand, nrand);
        }

        public void GeneratePlanet(float pr,float pa)
        {
            Random rand = new Random(planetseed);
            NormalRandom nrand = new NormalRandom(planetseed + 1);
            planets_num = 1;
            planets = new Planet[planets_num];
            planets[0] = new Planet(this, rand, nrand, pr, pa);
        }

        static public void SetConstants()
        {
            starcolors = new Gradient();
            starcolors.AddPoint(new GradientPart(new Color(127, 178, 255), 30000));
            starcolors.AddPoint(new GradientPart(new Color(42, 189, 231), 10000));
            starcolors.AddPoint(new GradientPart(new Color(243, 248, 255), 7500));
            starcolors.AddPoint(new GradientPart(new Color(255, 255, 200), 6000));
            starcolors.AddPoint(new GradientPart(new Color(255, 255, 0), 4000));
            starcolors.AddPoint(new GradientPart(new Color(255, 0, 0), 3000));
            starcolors.AddPoint(new GradientPart(new Color(207, 0, 0), 2000));
        }
    }

    class Planet
    {
        #region Replace data
        static ReplaceData[] templates = new ReplaceData[]
        {new ReplaceData(4,4,new int[4,4]{{-1, 3 + 32,-1,-1},{3 + 32, -1,-1,-1},{-1,-1,-1,-1},{-1,-1,-1,-1}},
                             new int[4,4]{ { -1, 4, 5, -1 }, { 4, 5 + 16, 5 + 16, 5 }, { 4 + 16, 5 + 16, 5 + 16, 5 + 32 }, { 4 + 32, 4 + 16, 5 + 32, -1 } },
                             new int[4,4]{{0,0,1,0},{0,0,0,1},{0,0,0,1},{0,0,1,0}},
                             2,2,0,0),

        new ReplaceData(4,4,new int[4,4]{{-1, -1, 1 + 32,-1},{-1,-1,-1, 1 + 32},{-1,-1,-1,-1},{-1,-1,-1,-1}},
                             new int[4,4]{ { -1, 4+3, 5+3, -1 }, { 4+3, 4 + 16+3, 4 + 16+3, 5+3 }, { 4 + 32+3, 4 + 16+3, 4 + 16+3, 5 + 16 +3}, { -1, 4 + 32+3, 5 + 16+3, 5+32+3 } },
                             new int[4,4]{{0,1,0,0},{1,0,0,0},{1,0,0,0},{0,1,0,0}},
                             4,2,3,0),

        new ReplaceData(3,4,new int[4,3]{{2+32, 2+32, 2+32},{-1,-1, -1},{-1,-1,-1},{-1,-1, -1}},
                             new int[4,3]{{4+48,5+48,6+48},{4+64,5+64,6+64}, {4+64,5+64,6+64}, {4+64+16,5+64+16,6+64+16}},
                             new int[4,3]{{0,0,0},{0,0,0},{0,0,0},{0,0,0}},
                             3,1,0,0),
        
        new ReplaceData(3,2,new int[2,3]{{-1,-1, -1},{2, 2, 2}},
                             new int[2,3]{{4+64+32,5+64+32,6+64+32},{4+64+48,5+64+48,6+64+48}},
                             new int[2,3]{{0,0,0},{0,0,0}},
                             3,2,1,1),
        
        new ReplaceData(3,5,new int[5,3]{{3+16,-1, -1},{3+16,-1, -1},{3+16,-1, -1},{3+16,-1, -1},{3+16,-1, -1}},
                             new int[5,3]{{7+48,5,-1},{7+64,5+16,5},{7+64+16,5+16,6},{3+16,4+16,6},{3+16,4+32,6+32}},
                             new int[5,3]{{0,1,0},{0,0,1},{0,0,0},{0,0,0},{0,0,0}},
                             1,5,0,2),
        
        new ReplaceData(3,5,new int[5,3]{{-1,-1, 1+16},{-1,-1, 1+16},{-1,-1, 1+16},{-1,-1, 1+16},{-1,-1, 1+16}},
                             new int[5,3]{{-1,7,8+48},{7,7+16,8+64},{6+16,7+16,8+64+16},{6+16,8+16,1+16},{9+32,8+32,1+16}},
                             new int[5,3]{{0,1,0},{1,0,0},{0,0,0},{0,0,0},{0,0,0}},
                             3,5,2,2),
        
        new ReplaceData(3,3,new int[3,3]{{3,-1,-1},{-1,3,-1},{-1,-1,-1}},
                             new int[3,3]{{9,10,-1},{-1,10+16,11+16},{-1,-1,11+32}},
                             new int[3,3]{{0,1,0},{0,0,1},{0,0,2}},
                             2,3,0,1),
        
        new ReplaceData(3,3,new int[3,3]{{-1,-1,1},{-1,1,-1},{-1,-1,-1}},
                             new int[3,3]{{-1,13,14},{12+16,13+16,-1},{12+32,-1,-1}},
                             new int[3,3]{{0,1,0},{1,0,0},{2,0,0}},
                             3,3,2,1)};

        #endregion

        public Star star;

        public double totalgametime;

        public double albedo;

        public double semimajoraxis;
        public double semiminoraxis;
        public double sidusperiod; // days in year
        public double orbitalspeed;

        public double radius;
        public double mass;
        public double rotateperiod;
        public float axistilt;

        public double atmosherepleasure;
        public double mintemperature;
        public double maxtemperature;

        public double startatmosherepleasure;
        public double startmintemperature;
        public double startmaxtemperature;

        public int mapwidth, mapheight;
        public float[,] heightmap;
        public bool[,] watermap;

        public float credits;
        public List<MapManager> map;
        public List<Vector2> basepositions;

        public float meteoritechance;
        public float piratechance;

        public float meteoritelastcalltime;
        public float sunstrikelasrcalltime;
        public float requestlasrcalltime;
        public float pirateslastcalltime;

        public int defencesystemcount;
        public int climatcontrolcount;

        public Gradient gradient;

        public Target target;
        public Target current;

        public List<RequestData> request;

        public float[] inventory;

        public List<Meteorite> meteorites;

        float BiCubicTexture(int[,] texture, int w, int h, float x, float y)
        {
            int x0 = (int)x;
            int y0 = (int)y;
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
        float BiCubicTexture(float[,] texture, int w, int h, float x, float y)
        {
            x += w;
            if (x >= w) x -= w;
            if (x >= w) x -= w;
            y += h;
            if (y >= h) y -= h;
            if (y >= h) y -= h;

            if (y < 0 || x < 0 || y >= h || x >= w)
                x = 0;

            int x0 = (int)x;
            int y0 = (int)y;
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
        float BiCubicTexture(bool[,] texture, int w, int h, float x, float y)
        {
            x += w;
            if (x >= w) x -= w;
            if (x >= w) x -= w;
            y += h;
            if (y >= h) y -= h;
            if (y >= h) y -= h;

            if (y < 0 || x < 0 || y >= h || x >= w)
                x = 0;

            int x0 = (int)x;
            int y0 = (int)y;
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

            float value = b1 * (texture[y0, x0]?1:0) + b2 * (texture[y0, x1]?1:0) + b3 * (texture[y1, x0]?1:0) + b4 * (texture[y1, x1]?1:0);
            value += b5 * (texture[y0, xm1]?1:0) + b6 * (texture[ym1, x0]?1:0) + b7 * (texture[y1, xm1]?1:0) + b8 * (texture[ym1, x1]?1:0);
            value += b9 * (texture[y0, x2]?1:0) + b10 * (texture[y2, x0]?1:0) + b11 * (texture[ym1, xm1]?1:0) + b12 * (texture[y1, x2]?1:0);
            value += b13 * (texture[y2, x1]?1:0) + b14 * (texture[ym1, x2]?1:0) + b15 * (texture[ym1, x2]?1:0) + b16 * (texture[y2, x2]?1:0);

            return value;
        }

        public Planet(Star star,Random rand,NormalRandom nrand,float rad=-1,float axis=-1)
        {
            this.star = star;

            totalgametime = 0;

            inventory = new float[MapManager.maxresources];
            request = new List<RequestData>();

            meteorites = new List<Meteorite>();

            sunstrikelasrcalltime = Constants.Map_sunstrikenexttime;
            meteoritelastcalltime = Constants.Map_meteoritenexttime;
            requestlasrcalltime = Constants.Planet_timetonewrequest;
            pirateslastcalltime = Constants.Planet_piratesnexttime;
            map = new List<MapManager>();
            basepositions = new List<Vector2>();

            albedo = (float)(0.45 + nrand.Next() * 0.15);

            //6371 - radius of earth
            radius = rad < 0 ? (float)(nrand.Next() / 1.5f * 11500 + 13500) : rad;             //km
            if (radius < 0) radius = -radius;
            float v = (float)(4f / 3f * Math.PI * Math.Pow(radius / 6371f, 3));
            mass = v * (float)(rand.NextDouble() * 0.05 + 0.17);            //mass Earth
            axistilt = (float)(nrand.Next() / 2) * 23f / 57f + 23f / 57f;   //radian

            semimajoraxis = (float)(star.radius / 0.00435 * axis < 0 ? nrand.Next(0.25, 4.75) : axis);     //a.o.
            if (semimajoraxis < 0) semimajoraxis = -semimajoraxis;
            semiminoraxis = semimajoraxis / 100f * (100 - rand.Next(10));               //a.o.

            double orbitsize = (semimajoraxis + semiminoraxis)*150000000 * (float)Math.PI;
            orbitalspeed = Math.Sqrt(9.81 / 6367 * star.mass / (semimajoraxis + semiminoraxis)/2*4);   //km/sec
            sidusperiod = orbitsize / orbitalspeed / 3600;      //hours
            rotateperiod = 24f;     //hours

            atmosherepleasure = startatmosherepleasure = mass * 101; //kPa
            double maxtemp = Math.Sqrt(Math.Sqrt(Math.Pow(star.radius / semiminoraxis, 2) * (1 - albedo) / 4)) * star.temperture;
            double mintemp = Math.Sqrt(Math.Sqrt(Math.Pow(star.radius / semimajoraxis, 2) * (1 - albedo) / 4)) * star.temperture;
            mintemperature = startmintemperature = mintemp - 273 + mass * 4;
            maxtemperature = startmaxtemperature = maxtemp - 273 + mass * 10;

            meteoritechance = (float)(Constants.Map_meteoritenexttime * mass / 5);
            meteoritelastcalltime = meteoritechance;

            piratechance = Constants.Planet_piratesnexttime;

            target = new Target(10000, 500 + (float)mass * 5, 20 * (float)mass, 100 * (float)mass / 4, 20 + star.planets_num * 2, 20 + 20 * (float)mass, 1000 + (float)mass * 7);
            current = new Target(0, 0, 0, 0, 10, 10, 0);

            mapwidth = 512;
            mapheight = 256;

            if (radius > 6371 * 1.5) { mapwidth *= 2; mapheight *= 2; }
            if (radius < 6371 / 3) { mapwidth /= 2; mapheight /= 2; }
            if (radius < 6371 / 1.5) { mapwidth /= 2; mapheight /= 2; }

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
                    heightmap[y, x] += BiCubicTexture(noize, noizewidth, noizeheight, (float)nx / scale, (float)ny / scale) * weight;
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
                    float val = BiCubicTexture(heightmap, mapwidth, mapheight, sx, sy);
                    float nx = 0, ny = 0;

                    int n = mapheight;
                    float size = 1;
                    for (int i = 0; i < n; i++)
                    {
                        Vector2 newn = new Vector2((float)Math.Cos(i / (2 * Math.PI)) * size, (float)Math.Sin(i / (2 * Math.PI)) * size);
                        float newval = BiCubicTexture(heightmap, mapwidth, mapheight, sx + newn.X, sy + newn.Y);
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


            credits = 5000;

            double temp = (maxtemperature + mintemperature) / 2;
            temp += 120;
            float tileset = (float)temp / 70.0f;
            if (tileset > 12) tileset = 12;
            if (tileset < 0) tileset = 0;
            SetGradient(12 - tileset);
        }

        public void SetGradient(float tileset)
        {
            gradient = new Gradient();

            #region Set gradient
            Gradient[] data = new Gradient[13];

            data[0] = new Gradient();
            data[0].AddPoint(new GradientPart(new Color(211, 106, 12), -1));
            data[0].AddPoint(new GradientPart(new Color(233, 115, 0), -0.25));
            data[0].AddPoint(new GradientPart(new Color(196, 14, 0), 0));
            data[0].AddPoint(new GradientPart(new Color(29, 8, 2), 0.0000625));
            data[0].AddPoint(new GradientPart(new Color(42, 30, 20), 0.125));
            data[0].AddPoint(new GradientPart(new Color(81, 38, 4), 0.375));
            data[0].AddPoint(new GradientPart(new Color(175, 37, 0), 0.75));
            data[0].AddPoint(new GradientPart(new Color(255, 54, 0), 1));

            data[1] = new Gradient();
            data[1].AddPoint(new GradientPart(new Color(35, 23, 1), -1));
            data[1].AddPoint(new GradientPart(new Color(117, 83, 11), -0.25));
            data[1].AddPoint(new GradientPart(new Color(193, 172, 58), 0));
            data[1].AddPoint(new GradientPart(new Color(93, 31, 7), 0.0000625));
            data[1].AddPoint(new GradientPart(new Color(202, 156, 17), 0.125));
            data[1].AddPoint(new GradientPart(new Color(203, 159, 40), 0.375));
            data[1].AddPoint(new GradientPart(new Color(180, 104, 23), 0.75));
            data[1].AddPoint(new GradientPart(new Color(255, 236, 226), 1));

            data[2] = new Gradient();
            data[2].AddPoint(new GradientPart(new Color(69, 52, 23), -1));
            data[2].AddPoint(new GradientPart(new Color(91, 72, 39), -0.25));
            data[2].AddPoint(new GradientPart(new Color(152, 117, 57), 0));
            data[2].AddPoint(new GradientPart(new Color(187, 114, 43), 0.0000625));
            data[2].AddPoint(new GradientPart(new Color(214, 197, 89), 0.125));
            data[2].AddPoint(new GradientPart(new Color(184, 109, 36), 0.375));
            data[2].AddPoint(new GradientPart(new Color(117, 71, 22), 0.75));
            data[2].AddPoint(new GradientPart(new Color(231, 186, 122), 1));

            data[3] = new Gradient();
            data[3].AddPoint(new GradientPart(new Color(37, 92, 112), -1));
            data[3].AddPoint(new GradientPart(new Color(75, 118, 127), -0.25));
            data[3].AddPoint(new GradientPart(new Color(85, 175, 149), 0));
            data[3].AddPoint(new GradientPart(new Color(197, 191, 85), 0.0000625));
            data[3].AddPoint(new GradientPart(new Color(214, 197, 89), 0.125));
            data[3].AddPoint(new GradientPart(new Color(177, 112, 48), 0.375));
            data[3].AddPoint(new GradientPart(new Color(125, 87, 66), 0.75));
            data[3].AddPoint(new GradientPart(new Color(235, 173, 112), 1));

            data[4] = new Gradient();
            data[4].AddPoint(new GradientPart(new Color(17, 46, 53), -1));
            data[4].AddPoint(new GradientPart(new Color(42, 83, 100), -0.25));
            data[4].AddPoint(new GradientPart(new Color(54, 149, 138), 0));
            data[4].AddPoint(new GradientPart(new Color(225, 176, 93), 0.125));
            data[4].AddPoint(new GradientPart(new Color(167, 127, 58), 0.0000625));
            data[4].AddPoint(new GradientPart(new Color(139, 165, 47), 0.375));
            data[4].AddPoint(new GradientPart(new Color(92, 110, 10), 0.75));
            data[4].AddPoint(new GradientPart(new Color(226, 202, 124), 1));

            data[5] = new Gradient();
            data[5].AddPoint(new GradientPart(new Color(3, 40, 53), -1));
            data[5].AddPoint(new GradientPart(new Color(12, 113, 125), -0.25));
            data[5].AddPoint(new GradientPart(new Color(58, 193, 163), 0));
            data[5].AddPoint(new GradientPart(new Color(87, 113, 19), 0.0000625));
            data[5].AddPoint(new GradientPart(new Color(125, 164, 30), 0.125));
            data[5].AddPoint(new GradientPart(new Color(203, 201, 40), 0.375));
            data[5].AddPoint(new GradientPart(new Color(180, 104, 23), 0.75));
            data[5].AddPoint(new GradientPart(new Color(255, 236, 226), 1));

            data[6] = new Gradient();
            data[6].AddPoint(new GradientPart(new Color(3, 53, 33), -1));
            data[6].AddPoint(new GradientPart(new Color(24, 111, 79), -0.25));
            data[6].AddPoint(new GradientPart(new Color(58, 193, 140), 0));
            data[6].AddPoint(new GradientPart(new Color(87, 113, 19), 0.0000625));
            data[6].AddPoint(new GradientPart(new Color(198, 189, 51), 0.125));
            data[6].AddPoint(new GradientPart(new Color(176, 148, 37), 0.375));
            data[6].AddPoint(new GradientPart(new Color(152, 90, 24), 0.75));
            data[6].AddPoint(new GradientPart(new Color(212, 201, 195), 1));

            data[7] = new Gradient();
            data[7].AddPoint(new GradientPart(new Color(15, 55, 12), -1));
            data[7].AddPoint(new GradientPart(new Color(27, 120, 23), -0.25));
            data[7].AddPoint(new GradientPart(new Color(129, 203, 103), 0));
            data[7].AddPoint(new GradientPart(new Color(156, 122, 39), 0.0000625));
            data[7].AddPoint(new GradientPart(new Color(225, 159, 49), 0.125));
            data[7].AddPoint(new GradientPart(new Color(196, 103, 34), 0.375));
            data[7].AddPoint(new GradientPart(new Color(128, 74, 11), 0.75));
            data[7].AddPoint(new GradientPart(new Color(251, 204, 172), 1));

            data[8] = new Gradient();
            data[8].AddPoint(new GradientPart(new Color(12, 55, 39), -1));
            data[8].AddPoint(new GradientPart(new Color(27, 120, 93), -0.25));
            data[8].AddPoint(new GradientPart(new Color(103, 203, 169), 0));
            data[8].AddPoint(new GradientPart(new Color(156, 144, 39), 0.0000625));
            data[8].AddPoint(new GradientPart(new Color(214, 178, 53), 0.125));
            data[8].AddPoint(new GradientPart(new Color(164, 168, 29), 0.375));
            data[8].AddPoint(new GradientPart(new Color(113, 122, 15), 0.75));
            data[8].AddPoint(new GradientPart(new Color(255, 241, 231), 1));

            data[9] = new Gradient();
            data[9].AddPoint(new GradientPart(new Color(34, 100, 82), -1));
            data[9].AddPoint(new GradientPart(new Color(54, 124, 108), -0.25));
            data[9].AddPoint(new GradientPart(new Color(89, 172, 150), 0));
            data[9].AddPoint(new GradientPart(new Color(114, 130, 81), 0.0000625));
            data[9].AddPoint(new GradientPart(new Color(213, 193, 68), 0.125));
            data[9].AddPoint(new GradientPart(new Color(113, 155, 54), 0.375));
            data[9].AddPoint(new GradientPart(new Color(60, 106, 20), 0.75));
            data[9].AddPoint(new GradientPart(new Color(184, 169, 132), 1));

            data[10] = new Gradient();
            data[10].AddPoint(new GradientPart(new Color(23, 69, 67), -1));
            data[10].AddPoint(new GradientPart(new Color(39, 87, 91), -0.25));
            data[10].AddPoint(new GradientPart(new Color(68, 133, 127), 0));
            data[10].AddPoint(new GradientPart(new Color(149, 169, 106), 0.0000625));
            data[10].AddPoint(new GradientPart(new Color(214, 197, 89), 0.125));
            data[10].AddPoint(new GradientPart(new Color(100, 118, 68), 0.375));
            data[10].AddPoint(new GradientPart(new Color(112, 103, 36), 0.75));
            data[10].AddPoint(new GradientPart(new Color(184, 175, 132), 1));

            data[11] = new Gradient();
            data[11].AddPoint(new GradientPart(new Color(17, 51, 49), -1));
            data[11].AddPoint(new GradientPart(new Color(34, 75, 79), -0.25));
            data[11].AddPoint(new GradientPart(new Color(61, 120, 114), 0));
            data[11].AddPoint(new GradientPart(new Color(106, 167, 169), 0.0000625));
            data[11].AddPoint(new GradientPart(new Color(168, 227, 238), 0.125));
            data[11].AddPoint(new GradientPart(new Color(165, 159, 89), 0.375));
            data[11].AddPoint(new GradientPart(new Color(104, 91, 67), 0.75));
            data[11].AddPoint(new GradientPart(new Color(226, 219, 199), 1));

            data[12] = new Gradient();
            data[12].AddPoint(new GradientPart(new Color(16, 71, 77), -1));
            data[12].AddPoint(new GradientPart(new Color(25, 82, 87), -0.25));
            data[12].AddPoint(new GradientPart(new Color(55, 139, 131), 0));
            data[12].AddPoint(new GradientPart(new Color(83, 195, 173), 0.0000625));
            data[12].AddPoint(new GradientPart(new Color(168, 238, 222), 0.125));
            data[12].AddPoint(new GradientPart(new Color(88, 183, 177), 0.375));
            data[12].AddPoint(new GradientPart(new Color(43, 135, 134), 0.75));
            data[12].AddPoint(new GradientPart(new Color(199, 224, 226), 1));
            #endregion

            int a = (int)tileset+1;
            int b = (int)tileset;
            if (a < 0) a = 0; if (a > 12) a = 12;
            if (b < 0) b = 0; if (b > 12) b = 12;
            float f = tileset - b;

            for (int i = 0; i < data[0].points.Count; i++)
            {
                Color c2 = data[a].GetColor(data[0].points[i].position);
                Color c1 = data[b].GetColor(data[0].points[i].position);
                gradient.AddPoint(new GradientPart(Color.Lerp(c1, c2, f), data[0].points[i].position));
            }
        }

        public bool CreateBaseRelief(int x, int y, ref MapManager map)
        {
            foreach (Vector2 v in basepositions)
                if (Math.Abs(v.X - x) + Math.Abs(v.Y - y) < 16) return false;

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

                    height[i, j] = BiCubicTexture(heightmap, mapwidth, mapheight, x + dx, y + dy);

                    if (min > height[i, j]) min = height[i, j];
                    if (max < height[i, j]) max = height[i, j];

                    if (BiCubicTexture(watermap, mapwidth, mapheight, x + dx, y + dy) > 0.5f) water[i, j] = true;
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
                        else map.data[i, j].height = (int)(height[i, j] * 15) + 1;

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
                        int up = i == 0 ? map.data[i, j].id : map.data[i - 1, j].id;
                        map.data[i, j].ground_id = up;
                        map.data[i, j].id = 1;
                    }
                    else if (h[1, 1] > h[1, 2] && h[1, 1] == h[1, 0] && h[1, 1] > h[0, 1] && h[1, 1] == h[2, 1])
                    {
                        int up = i == 0 ? map.data[i, j].id : map.data[i - 1, j].id;
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
                    foreach (ReplaceData d in templates)
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
                            int nh = map.data[i + d.py, j + d.px].height - 1;
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
            for (int i = 0; i < map.height; i++)
            {
                for (int j = 0; j < map.width; j++)
                {
                    if ((map.data[i, j].id == 16 || map.data[i, j].id == 2 + 16) && r.Next(50) == 0)
                    {
                        map.data[i, j].id = 64 + r.Next(2) * 32;

                        double temp = (maxtemperature + mintemperature) / 2;
                        temp += 120;
                        float tileset = (float)temp / 70.0f;

                        if (tileset >= 12 || tileset <= 1)
                            map.data[i, j].id = 64 + 48 + 1 + r.Next(2);
                    }
                    if ((map.data[i, j].id == 16 || map.data[i, j].id == 2 + 16) && r.Next(15) == 0)
                    {
                        map.data[i, j].id = 64 + 48 + 3;
                    }

                    if ((map.data[i, j].id == 16 || map.data[i, j].id == 2 + 16) && r.Next(10) == 0)
                    {
                        map.data[i, j].id = 64 + 32 + 3;
                    }

                    map.data[i, j].can_build = map.data[i, j].can_move = MapManager.enabledata[map.data[i, j].id] == 0;
                    if (i == 0 || i == map.height - 1 || j == 0 || j == map.width - 1) map.data[i, j].can_build = false;
                }
            }
            #endregion

            return true;
        }

        public bool TestCreateBase(int x, int y)
        {
            MapManager map = new MapManager(64, 64);

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
        public bool CreateBase(int x, int y)
        {
            MapManager map = new MapManager(64, 64);
            Random rand = new Random();
            if (CreateBaseRelief(x, y, ref map))
            {
                #region Set resources

                for(int i=0;i<map.height;i++)
                    for (int j = 0; j < map.width; j++)
                    {
                        bool sand = false;
                        bool glay = false;

                        for(int k=-4;k<=6;k++)
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

                            int py=sy;
                            for (int px = sx; px <= rex&&py>=rey; px++, py--)
                            {
                                int size = rand.Next(Math.Min(px - sx, ex - px) / 2)+1;
                                for (int k = -size/2; k <= size/2; k++)
                                    for (int l = -size/2; l <= size/2; l++)
                                    {
                                        int kx = px + l;
                                        int ky = py + k;
                                        //set fosphat
                                        if (map.onMap(kx, ky)&&(map.data[ky, kx].id != 0))
                                        {
                                            map.data[ky, kx].mineresource_id = (int)Resources.fosphat;
                                        }
                                    }

                            }
                        }
                        #endregion
                        #region ore
                        if (map.data[i, j].id != 0 && rand.Next(300+100*map.data[i, j].height / 4) == 0)
                        {
                            int steps = rand.Next(4)+4;
                            int sx = j;
                            int sy = i;
                            for (int k = 0; k < steps; k++)
                            {
                                int len = rand.Next(4) + 8;
                                float dx = (float)(rand.Next(8) - 4);
                                float dy = (float)(rand.Next(8) - 4);
                                if (dx == 0) dx++; if (dy == 0) dy++;
                                float lxy = (float)Math.Sqrt(dx*dx+dy*dy);
                                dx/=lxy;dy/=lxy;

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
                        if (map.data[i, j].id != 0 && rand.Next(400 + 100 * map.data[i, j].height / 4) == 0)
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
                        if (map.data[i, j].id != 0 && rand.Next(300 + 100 * map.data[i, j].height / 2) == 0)
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
                        if (map.data[i, j].id != 0 && rand.Next(500 + 100 * map.data[i, j].height / 2) == 0)
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
                                        if (map.onMap(sx + l, sy + k) && Math.Sqrt(k * k + l * l) <= radius) map.data[sy + k, sx + l].dirrickresource_id = res;
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

                    if (bx < map.width - 6 && by < map.height - 4 && bx >= 0 && by >= 0)
                    {
                        string name = "База " + (this.map.Count + 1);
                        map.AddUnit(bx, 33, Unit.Drone);
                        map.planet = this;
                        map.baseid = this.map.Count;
                        map.position = new Vector2(x, y);
                        map.name = name;
                        credits -= 1000;
                        this.map.Add(map);
                        basepositions.Add(new Vector2(x, y));
                        return true;
                    }
                }
            }
            return false;
        }

        void SendRequest()
        {
            for (int i = request.Count - 1; i >= 0; i--)
            {
                if (inventory[request[i].resource] >= request[i].count)
                {
                    float res = request[i].count;
                    foreach (MapManager m in map)
                    {
                        res -= m.inventory[request[i].resource];
                        if (res < 0) { m.inventory[request[i].resource] = -res; res = 0; break; }
                    }

                    inventory[request[i].resource] -= request[i].count;
                    request.RemoveAt(i);
                    current.favor++;
                }
            }
        }

        public void Update(float ellapsedtime, float totaltime, MessageSystem ms)
        {
            defencesystemcount = 0;
            climatcontrolcount = 0;
            totalgametime += ellapsedtime / 20;

            for (int i = map.Count - 1; i >= 0; i--)
            {
                map[i].Update(ellapsedtime, totaltime, ms);
                if (map[i].proresearch[Constants.ProResearch_climatcontrol].searched && !map[i].sciencemode && map[i].selectedproresearch == Constants.ProResearch_climatcontrol) climatcontrolcount++;
                if (map[i].proresearch[Constants.ProResearch_planetdefence].searched && !map[i].sciencemode && map[i].selectedproresearch == Constants.ProResearch_planetdefence) defencesystemcount++;

                if (map[i].timetodestroy < 0) { credits += map[i].inventory[(int)Resources.credits]; map.RemoveAt(i); basepositions.RemoveAt(i); }

                foreach (Shield s in map[i].shields)
                    current.peace += (s.type == Shield.Emmision || s.type == Shield.Power) ? s.size / 7 / 13 * ellapsedtime : 0;
            }

            #region Meteorites
            meteoritelastcalltime -= ellapsedtime;
            if (meteoritelastcalltime <= 0)
            {
                meteoritelastcalltime = meteoritechance;

                if (defencesystemcount > 0) meteoritelastcalltime *= 2 * defencesystemcount;

                Random r = new Random();
                r.Next(); r.Next(); r.Next(); r.Next();

                Vector2 pos;
                float lighting = 1;

                do
                {
                    pos = new Vector2(r.Next(mapwidth), r.Next(mapheight));

                    if (basepositions.Count > 0)
                    {
                        if (r.Next(5) == 0)
                        {
                            int bsid = r.Next(basepositions.Count);
                            pos = basepositions[bsid];
                            pos += new Vector2(r.Next(8) - 4, r.Next(8) - 4);
                        }
                    }
                    lighting = GetLightingWithoutRelief((int)pos.X, (int)pos.Y, totaltime / 20);
                } while (lighting > 0.3f);

                int id = GetBaseId((int)pos.X, (int)pos.Y);

                if (id >= 0)
                {
                    Vector2 mpos = new Vector2(map[id].width + 1, -1), mtar = new Vector2(r.Next(map[id].width - 8) + 4, r.Next(map[id].height - 8) + 4);
                    map[id].meteorites.Add(new Meteorite(mpos, mtar, 10));
                    if (map[id].IsBeDestroyedByMeteor(mtar)) current.peace--;
                }
                meteorites.Add(new Meteorite(pos, pos, 0));
                meteorites[meteorites.Count - 1].explotion = 0;
            }
            #endregion

            for (int i = meteorites.Count - 1; i >= 0; i--)
            {
                meteorites[i].explotion += 4;

                if (meteorites[i].explotion > Constants.Map_meteoriteexplotiomaxsize / 10) meteorites.RemoveAt(i);
            }

            sunstrikelasrcalltime -= ellapsedtime;
            if (sunstrikelasrcalltime <= 0)
            {
                sunstrikelasrcalltime = star.turbulance;
                ms.AddMessage(MessageType.sunstrike, -1, "Солнечная буря", 1 + Constants.Map_sunstrikeunitwaittime, Color.White);
                foreach (MapManager m in map)
                {
                    float lighting = GetLighting((int)m.position.X, (int)m.position.Y, totaltime / 20);
                    if (lighting > 0.1f)
                    {
                        if (lighting > 1) lighting = 1;
                        foreach (Unit u in m.units)
                            if (u.type == Unit.Drone && !m.onShield(u.pos.X, u.pos.Y, Shield.Emmision))
                                u.wait = Constants.Map_sunstrikeunitwaittime * ((lighting - 0.1f) / 0.9f);
                    }
                }
            }

            pirateslastcalltime -= ellapsedtime;
            if (pirateslastcalltime <= 0)
            {
                Random rand = new Random();
                pirateslastcalltime = piratechance + rand.Next((int)piratechance / 2);

                if (map.Count > 0)
                {
                    int id = rand.Next(map.Count);
                    if (map[id].buildings.Count > 5)
                    {
                        List<int> buildings = new List<int>();

                        for (int i = 0; i < map[id].buildings.Count;i++ )
                            if (map[id].buildings[i].buildingtime <= 0 && map[id].buildings[i].power > 0) buildings.Add(i);

                        ms.AddMessage(MessageType.sunstrike, id, "Нападение пиратов", 3, Color.White);

                        int num = 2;

                        if (map[id].buildings.Count < 8) num = 2;
                        else if (map[id].buildings.Count < 15) num = 4;
                        else if (map[id].buildings.Count < 30) num = 6;
                        else if (map[id].buildings.Count < 48) num = 8;
                        else if (map[id].buildings.Count < 80) num = 10;
                        else num = 15;

                        for (int q = 0; q < num; q++)
                        {
                            if (buildings.Count > 0)
                            {
                                Vector2 pos = map[id].GetRandomBaseDirection();
                                map[id].AddUnit(pos.X, pos.Y, Unit.Pirate);

                                int rbid = rand.Next(buildings.Count);
                                int bid = buildings[rbid];
                                buildings.RemoveAt(rbid);
                                Point size = Building.GetSize(map[id].buildings[bid].type);
                                int x = (int)(map[id].buildings[bid].pos.X - size.X / 2);
                                int y = (int)(map[id].buildings[bid].pos.Y - size.Y / 2);

                                map[id].units[map[id].units.Count - 1].command = new Command(commands.piraterob, (int)x, (int)y);
                                map[id].units[map[id].units.Count - 1].tar = new Vector2((int)x, (int)y);
                                //map[id].units[map[id].units.Count - 1].waypoints.Clear();
                            }
                        }
                    }
                }
            }

            requestlasrcalltime -= ellapsedtime;
            if (requestlasrcalltime <= 0)
            {
                requestlasrcalltime = Constants.Planet_timetonewrequest;
                request.Add(new RequestData(new Random().Next(29) + 2, (int)(current.favor * 5), current.favor * 2 + Constants.Planet_timetorequest));
                ms.AddMessage(MessageType.request, -1, "Новый заказ!", 10, Color.White);
            }

            for (int i = request.Count - 1; i >= 0; i--)
            {
                request[i].time -= ellapsedtime;
                if (inventory[request[i].resource] >= request[i].count)
                {
                    float res = request[i].count;
                    foreach (MapManager m in map)
                    {
                        res -= m.inventory[request[i].resource];
                        if (res < 0) { m.inventory[request[i].resource] = -res; res = 0; break; }
                    }

                    inventory[request[i].resource] -= request[i].count;
                    request.RemoveAt(i);
                    current.favor++;
                    credits += Math.Max(current.favor, 0) * 100;
                    ms.AddMessage(MessageType.none, -1, "Заказ выполнен! Благосклонность выросла.", 10, Color.White);
                }

                if (i < request.Count)
                {
                    if (request[i].time < 0)
                    {
                        request.RemoveAt(i);
                        current.favor--;
                        ms.AddMessage(MessageType.none, -1, "Заказ не выполнен! Благосклонность упала.", 5, Color.White);
                    }
                }
            }

            if (climatcontrolcount > 0)
            {
                mintemperature += Math.Sign(Constants.Planet_optimalmintemperature - mintemperature) * ellapsedtime * Constants.Planet_temperaturespeed * climatcontrolcount;
                maxtemperature += Math.Sign(Constants.Planet_optimalmaxtemperature - maxtemperature) * ellapsedtime * Constants.Planet_temperaturespeed * climatcontrolcount;
                atmosherepleasure += Math.Sign(Constants.Planet_optimalatmosherepleasure - atmosherepleasure) * ellapsedtime * Constants.Planet_preasurespeed * climatcontrolcount;
            }
            else
            {
                mintemperature += Math.Sign(startmintemperature - mintemperature) * ellapsedtime * Constants.Planet_temperaturespeed * climatcontrolcount;
                maxtemperature += Math.Sign(startmaxtemperature - maxtemperature) * ellapsedtime * Constants.Planet_temperaturespeed * climatcontrolcount;
                atmosherepleasure += Math.Sign(startatmosherepleasure - atmosherepleasure) * ellapsedtime * Constants.Planet_preasurespeed * climatcontrolcount;
            }

            for (int i = 0; i < MapManager.maxresources; i++)
                inventory[i] = 0;

            current.credits = credits;
            current.temperature = (float)(mintemperature + (maxtemperature - mintemperature) / 2);
            current.preasure = (float)atmosherepleasure;
            current.peace += ellapsedtime / 20;
            current.population = 0;
            current.science = 0;
            foreach (MapManager m in map)
            {
                current.credits += m.GetCredits();
                current.population += m.GetPopulation();
                current.science += m.GetScience();

                for (int j = 0; j < MapManager.maxresources; j++)
                    inventory[j] += m.inventory[j];
            }
        }

        public int GetBaseId(int x, int y)
        {
            for (int i = 0; i < basepositions.Count; i++)
            {
                Vector2 v = basepositions[i];
                if (new Rectangle((int)v.X - 4, (int)v.Y - 4, 8, 8).Contains(x, y)) return i;
            }
            return -1;
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
    }
}
