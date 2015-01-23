using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    class GradientPart
    {
        public Color color;
        public double position;

        public GradientPart(Color c, double pos)
        {
            color = c;
            position = pos;
        }
    }

    class Gradient
    {
        public List<GradientPart> points;

        public Gradient()
        {
            points = new List<GradientPart>();
        }

        public void AddPoint(GradientPart p)
        {
            if (points.Count == 0) { points.Add(p); return; }
            for (int i = 0; i < points.Count; i++)
                if (points[i].position > p.position) { points.Insert(i, p); return; }

            points.Add(p);
        }

        public Color GetColor(double pos)
        {
            if (points.Count == 0) return Color.Black;
            if (points.Count == 1) return points[0].color;
            if (points[0].position > pos) return points[0].color;
            for (int i = 1; i < points.Count; i++)
                if (points[i].position > pos)
                {
                    pos = (pos - points[i - 1].position) / (points[i].position - points[i - 1].position);
                    return Color.Lerp(points[i - 1].color, points[i].color, (float)pos);
                }

            return points[points.Count - 1].color;
        }
    }
}
