using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTerraform
{
    class Perk
    {
        public float storagebonus ;
        public float movementbonus ;
        public float sciencebonus ;
        public float buildbonus ;

        public float buildingspeedup ;
        public float unitspeedup ;
        public float optimizestorage ;
        public float optimizeore ;
        public float optimizingshield ;
        public float optimizeerergyproduce ;
        public float otimizeenergyconsume ;
        public float optimizeresearch;

        public Perk()
        {
            storagebonus = 1;
            movementbonus = 1;
            sciencebonus = 1;
            buildbonus = 1;

            buildingspeedup = 1;
            unitspeedup = 1;
            optimizestorage = 1;
            optimizeore = 1;
            optimizingshield = 1;
            optimizeerergyproduce = 1;
            otimizeenergyconsume = 1;
            optimizeresearch = 1;
        }
    }
}
