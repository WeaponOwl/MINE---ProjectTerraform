using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTerraform
{
    class Resiple
    {
        public int[] inresourses;
        public float[] incount;
        public int outresource;
        public float outcount;

        public Resiple(int[] res, float[] count, int outres, float outvalue)
        {
            inresourses = res;
            incount = count;
            outresource = outres;
            outcount = outvalue;
        }
    }

    class Research
    {
        public float time;
        public string name;
        public string overview;
        public bool searched;

        public Research(float t, string n,string o)
        {
            time = t;
            name = n;
            searched = false;
            overview = o;
        }
    }

    class ProResearch
    {
        public float time;
        public string name;
        public string overview;
        public bool searched;
        public int[] reserchresources;
        public float[] reserchvalues;
        public int workscience;
        public float power;

        public ProResearch(float t, string n,int needscience,int[] res,float[] val,string o)
        {
            time = t;
            name = n;
            searched = false;
            reserchresources = res;
            reserchvalues = val;
            workscience = needscience;
            power = 0;
            overview = o;
        }
    }
}
