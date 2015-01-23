using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    public class Univerce
    {
        public Star[] stars;
        public Vector2[] positions;
        public int stars_num;

        public List<int>[] subsectors;

        public Univerce()
        {
            Random rand = new Random(1234);
            NormalRandom nrand = new NormalRandom(1234);

            stars_num = 800;
            stars = new Star[stars_num];
            positions = new Vector2[stars_num];

            subsectors = new List<int>[64];
            for (int i = 0; i < 64; i++) subsectors[i] = new List<int>();

            int min_starinsector = 3;
            int branches = 3;
            int stars_numonbranch = stars_num - 64 * min_starinsector;
            for (int i = 0; i < stars_numonbranch; i++)
            {
                int k = i % branches;
                float l = (float)rand.NextDouble();
                float f = k * (float)Math.PI / branches * 2 + l * 2 * (float)Math.PI;

                float dx = (float)Math.Cos(f) * l;
                float dy = (float)Math.Sin(f) * l;

                int offcet = 13;
                int x = (int)(dx * 592 / 2 + 592 / 2 + (float)nrand.Next() * offcet - offcet / 2);
                int y = (int)(dy * 592 / 2 + 592 / 2 + (float)nrand.Next() * offcet - offcet / 2);

                x += 592; x %= 592;
                y += 592; y %= 592;

                positions[i] = new Vector2(x, y);
                stars[i] = new Star(rand.Next());

                subsectors[(y / 74) * 8 + x / 74].Add(i);
            }
            for (int i = stars_numonbranch; i < stars_num; i++)
            {
                int sector_id = (i - stars_numonbranch) / min_starinsector;

                int sector_x = (sector_id % 8) * 74;
                int sector_y = (sector_id / 8) * 74;

                int x = rand.Next(sector_x+3, sector_x + 71);
                int y = rand.Next(sector_y+3, sector_y + 71);

                positions[i] = new Vector2(x, y);
                stars[i] = new Star(rand.Next());

                subsectors[(y / 74) * 8 + x / 74].Add(i);
            }
        }
    }
}
