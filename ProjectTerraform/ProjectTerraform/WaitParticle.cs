using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    class WaitParticle
    {
        public int startx;
        public int endx;
        public int starty;
        public Color color;
        public Color backcolor;
        public float p;

        public WaitParticle(int x, int y, int w, Color c, Color b,float p)
        {
            startx = x;
            endx = x + w;
            starty = y;
            color = c;
            backcolor = b;
            this.p = p;
        }
    }
}
