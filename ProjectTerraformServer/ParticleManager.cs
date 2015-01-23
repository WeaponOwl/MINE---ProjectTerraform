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
}
