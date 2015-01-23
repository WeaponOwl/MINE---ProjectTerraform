using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTerraform
{
    public static class NameGenerator
    {
        static string[] abc = new string[]{"jaguar", "royal", "accomodate", "planet", "earth",
           "moon", "alpha", "mars", "persei", "columb",
           "vavilon", "venera", "odin", "cucumber", "generator",
           "vivek", "vivian", "crematoria", "luisiana", "orlean",
           "embarrasment", "bebomoro", "badaboom", "centurion", 
           "caesar", "jaconda", "gamma", "yakee", "kevlar", 
           "koala", "melancolia", "death", "peanut", 
           "plastic", "onion", "reactor", "fractal", "astro"};

        static string[] postfix = new string[]{"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX",
               "X", "Omega", "1", "7", "13", "AMP", "K", "L3", "AR7", "Crypto"};

        static string[] suffixes = new string[] { "te", "op", "-ni", "li", "cla", "-4", "/8", "-4.2" };

        static string vowel = "aeiouy";

        static string Turbulance(string s)
        {
            char[] c = s.ToCharArray();

            Random r = new Random();
            r.Next(); r.Next(); r.Next(); r.Next();

            for (int i = 0; i < c.Length; i++)
            {
                int id = r.Next(c.Length);
                char k = c[i];
                c[i] = c[id];
                c[id] = k;
            }

            s = "";
            for (int i = 0; i < c.Length; i++)
                s += c[i].ToString();

            return s;
        }
        static string Turbulance(string s,Random r)
        {
            char[] c = s.ToCharArray();

            for (int i = 0; i < c.Length; i++)
            {
                int id = r.Next(c.Length);
                char k = c[i];
                c[i] = c[id];
                c[id] = k;
            }

            s = "";
            for (int i = 0; i < c.Length; i++)
                s += c[i].ToString();

            return s;
        }

        static bool IsGood(string name)
        {
            if (name.Length >= 3)
            {
                for (int i = 0; i < vowel.Length; i += 1)
                {
                    char alp = vowel[i];
                    for (int j = 1; j < name.Length; j += 1)
                    {
                        if (name[j] == alp)
                            return true;
                    }
                }
            }
            
            return false;
        }

        public static string GeneratePlanetName()
        {
            Random r = new Random();
            r.Next(); r.Next(); r.Next(); r.Next();

            return GeneratePlanetName(r);
        }
        public static string GeneratePlanetName(Random r)
        {
            string s = abc[r.Next(abc.Length)];
            s = abc[r.Next(abc.Length)];
            s = Turbulance(s, r);
            if (r.Next(10) < 3)
                s = s + "-" + postfix[r.Next(postfix.Length)];
            s = s[0].ToString().ToUpper() + s.Substring(1);
            return s;
        }

        public static string GenerateStarName()
        {
            return GeneratePlanetName();
        }
        public static string GenerateStarName(Random r)
        {
            return GeneratePlanetName(r);
        }

        
    }
}
