using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTerraform
{
    class NormalRandom
    {
        Random rand;

        public NormalRandom()
        {
            rand = new Random();
        }
        public NormalRandom(int Seed)
        {
            rand = new Random(Seed);
        }

        public double Next()
        {
            double x1 = rand.NextDouble();
            double x2 = rand.NextDouble();

            return Math.Cos(2 * Math.PI * x1) * Math.Sqrt(-2 * Math.Log(x2));
        }
        public double Next(double min, double max)
        {
            double x1 = rand.NextDouble();
            double x2 = rand.NextDouble();

            double z =  Math.Cos(2 * Math.PI * x1) * Math.Sqrt(-2 * Math.Log(x2));

            return min + (z + 1) / 2 * (max - min);
        }
    }
}
