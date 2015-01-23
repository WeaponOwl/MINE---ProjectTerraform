using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    class Particle
    {
        public Vector2 pos;
        public Vector2 anchor;
        public Rectangle source;
        public float life;
        public float maxlife;
        public Vector2 speed;
        public Vector2 offcet;

        public Particle(Vector2 pos, Vector2 anchor, Rectangle source, float life, Vector2 speed)
        {
            this.pos = pos;
            this.anchor = anchor;
            this.source = source;
            maxlife = this.life = life;
            this.speed = speed;
            offcet = Vector2.Zero;
        }
    }

    class MapHelperCell
    {
        public float lighting;
        public short subterrainvertexid;
        public short subterrainvertexid2;
        public short subterrainvertexid3;
        public bool scaned;

        public MapHelperCell()
        {
            lighting = 0;
            subterrainvertexid = -1;
            subterrainvertexid2 = -1;
            subterrainvertexid3 = -1;
            scaned = false;
        }
    }

    class MapHelper
    {
        public int width, height;
        public MapHelperCell[,] data;
        public List<Particle> particles;

        public MapHelper(Map map)
        {
            width = map.width;
            height = map.height;

            data = new MapHelperCell[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    data[i, j] = new MapHelperCell();
                }

            particles = new List<Particle>();
        }
        public void Clear()
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    data[i, j].lighting = 0;
                    data[i, j].scaned = false;
                }
        }
    }
}
