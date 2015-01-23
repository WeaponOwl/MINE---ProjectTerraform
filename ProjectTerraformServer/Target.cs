using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTerraform
{
    struct Target
    {
        public float credits;
        public float population;
        public float temperature;
        public float preasure;
        public float favor;
        public float peace;
        public float science;
        public string text;

        public Target(float cr, float pop, float temp, float preas, float fav, float pe,float sc)
        {
            text = "";
            credits=cr;
            population=pop;
            temperature=temp;
            preasure=preas;
            favor=fav;
            peace=pe;
            science = sc;
        }
    }
}
