using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTerraform
{
    static class Helper
    {
        public static string[] help;
        public static string[] about;
        public static string[] chrono;
        public static string[] promo;

        public static void LoadHelp(string filename)
        {
            help = System.IO.File.ReadAllLines(filename);
        }
        public static void LoadAbout(string filename)
        {
            about = System.IO.File.ReadAllLines(filename);
        }
        public static void LoadChrono(string filename)
        {
            chrono = System.IO.File.ReadAllLines(filename);
        }
        public static void LoadPromo(string filename)
        {
            promo = System.IO.File.ReadAllLines(filename);
        }
    }
}
