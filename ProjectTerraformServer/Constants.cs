namespace ProjectTerraform
{
    static class Constants
    {
        public const int Map_width = 64;
        public const int Map_height = 64;

        public const int Map_energy = 0;
        public const int Map_coins = 1;
        public const int Map_science = 2;
        public const int Map_rocks = 3;
        public const int Map_luquids = 4;
        public const int Map_humans = 5;

        public const float Map_warehoucestoragecapability = 400;
        public const float Map_infostoragestoragecapability = 500;
        public const float Map_housestoragecapability = 8;
        public const float Map_energybankstoragecapability = 500;
        public const float Map_reservourstoragecapability = 500;

        public const int Map_buildingparticlechance = 15;
        public const float Map_buildingparticlelife = 4;
        public const float Map_buildingwait = 10;

        public const int Unit_traderheight = 3;
        public const float Unit_traderwait = 1;
        public const float Unit_tradervalue = 100;
        public const int Unit_humanheight = 2;
        public const float Unit_humanwait = 1;

        public const float Unit_droneelectronicprice = 5;
        public const float Unit_dronemetalprice = 10;

        public const int Map_humannew = 10;
        public const int Map_humannewrand = 10;

        public const float Map_generatorproduce = 20;
        public const float Map_dronefactoryworktime = 3;
        public const int Map_laboratoryworkinghuman = 10;
        public const int Map_laboratorymaxworkinghuman = 10;

        public const int Map_timefornewtrader = 10;
        public const int Map_timefornewhuman = 10;
        public const float Map_parkingwaittime = 100;

        public const float Map_hungerstarttime = 20;
        public const float Map_hungernexttime = 2;

        public const float Map_meteoritenexttime = 70;
        public const float Map_meteoritespeed = 100;
        public const float Map_meteoriteexplotionspeed = 200;
        public const float Map_meteoriteexplotiomaxsize = 1000;
        public const int Map_meteoritedestroyarea = 4;

        public const float Map_sunstrikenexttime = 50;
        public const float Map_sunstrikeunitwaittime = 5;

        public const float Map_timetodestroybase = 120;

        public const float System_perkkoef = 0.1f;

        public const float Map_energyoreconsuming = 3;

        public const int Research_buildingspeedup = 2;
        public const int Research_unitspeedup = 3;
        public const int Research_terrainexplored = 0;
        public const int Research_optimizestorage = 4;
        public const int Research_optimizeore = 5;
        public const int Research_optimizingshield = 6;
        public const int Research_optimizeerergyproduce = 7;
        public const int Research_otimizeenergyconsume = 8;
        public const int Research_optimizeresearch = 1;
        public const int Research_orbitalmodules = 9;
        public const int Research_basicresearchartifact = 10;
        public const int Research_proresearchartifact = 11;

        public const int ProResearch_climatcontrol = 0;
        public const int ProResearch_planetdefence = 1;

        public const float Map_workinghumanprice = 0.003f;

        public const float Planet_optimalmintemperature = -50.0f;
        public const float Planet_optimalmaxtemperature = 50.0f;
        public const float Planet_optimalatmosherepleasure = 100.0f;
        public const float Planet_temperaturespeed = 0.2f;
        public const float Planet_preasurespeed = 0.2f;

        public const float Planet_timetonewrequest = 40;
        public const float Planet_timetorequest = 150;

        public const float Game_nextautosavetime = 120;
        public const float Map_unitbeaconwait = 3;

        public const float Planet_piratesnexttime = 120;

        static public float GetPriceOfResource(int res)
        {
            switch (res)
            {
                case (int)Resources.energy: return 1;
                case (int)Resources.credits: return 1;
                case (int)Resources.meat: return 0.1f;
                case (int)Resources.fish: return 0.1f;
                case (int)Resources.vegetables: return 0.1f;
                case (int)Resources.fruits: return 0.1f;
                case (int)Resources.alcohol: return 2f;
                case (int)Resources.energyore: return 0.5f;
                case (int)Resources.fosphat: return 0.18f;
                case (int)Resources.ore: return 0.1f;
                case (int)Resources.metal: return 0.6f;
                case (int)Resources.composition: return 0.8f;
                case (int)Resources.oil: return 0.16f;
                case (int)Resources.coal: return 0.1f;
                case (int)Resources.metan: return 0.8f;
                case (int)Resources.rare_gas: return 0.14f;
                case (int)Resources.water: return 0.02f;
                case (int)Resources.animals: return 0.6f;
                case (int)Resources.plants: return 0.4f;
                case (int)Resources.gems: return 1.8f;
                case (int)Resources.artifacts: return 1f;
                case (int)Resources.chemical: return 0.06f;
                case (int)Resources.electronics: return 0.2f;
                case (int)Resources.battery: return 0.4f;
                case (int)Resources.explosive: return 0.8f;
                case (int)Resources.medicine: return 0.6f;
                case (int)Resources.plastic: return 0.2f;
                case (int)Resources.biowaste: return 0.01f;
                case (int)Resources.sand: return 0.02f;
                case (int)Resources.glay: return 0.06f;
                case (int)Resources.rock: return 0.1f;
                case (int)Resources.beton: return 0.2f;
                default: return 1;
            }
        }

        static public string GetShortNameOfResource(int res)
        {
            switch (res)
            {
                case (int)Resources.energy:      return "Энергия";
                case (int)Resources.credits:     return "Кредиты";
                case (int)Resources.meat:        return "Мясо";
                case (int)Resources.fish:        return "Рыба";
                case (int)Resources.vegetables:  return "Фрукты";
                case (int)Resources.fruits:      return "Овощи";
                case (int)Resources.alcohol:     return "Алкоголь";
                case (int)Resources.energyore:   return "Эн. руда";
                case (int)Resources.fosphat:     return "Фосфаты";
                case (int)Resources.ore:         return "Руда";
                case (int)Resources.metal:       return "Металл";
                case (int)Resources.composition: return "Сплав";
                case (int)Resources.oil:         return "Нефть";
                case (int)Resources.coal:        return "Уголь";
                case (int)Resources.metan:       return "Метан";
                case (int)Resources.rare_gas:    return "Ин. газ";
                case (int)Resources.water:       return "Вода";
                case (int)Resources.animals:     return "Животные";
                case (int)Resources.plants:      return "Растения";
                case (int)Resources.gems:        return "Др. кам.";
                case (int)Resources.artifacts:   return "Артефакт";
                case (int)Resources.chemical:    return "Х. отход";
                case (int)Resources.electronics: return "Электрон.";
                case (int)Resources.battery:     return "Акамулят.";
                case (int)Resources.explosive:   return "Взрывч.";
                case (int)Resources.medicine:    return "Медицина";
                case (int)Resources.plastic:     return "Пластик";
                case (int)Resources.biowaste:    return "Б. отходы";
                case (int)Resources.sand:        return "Песок";
                case (int)Resources.glay:        return "Глина";
                case (int)Resources.rock:        return "Камни";
                case (int)Resources.beton:       return "Бетон";
                default: return "";
            }
        }

        static public string GetNameOfResource(int res)
        {
            switch (res)
            {
                case (int)Resources.energy: return "Энергия";
                case (int)Resources.credits: return "Кредиты";
                case (int)Resources.meat: return "Мясо";
                case (int)Resources.fish: return "Рыба";
                case (int)Resources.vegetables: return "Фрукты";
                case (int)Resources.fruits: return "Овощи";
                case (int)Resources.alcohol: return "Алкоголь";
                case (int)Resources.energyore: return "Энерго-руда";
                case (int)Resources.fosphat: return "Фосфаты";
                case (int)Resources.ore: return "Руда";
                case (int)Resources.metal: return "Металл";
                case (int)Resources.composition: return "Сплав";
                case (int)Resources.oil: return "Нефть";
                case (int)Resources.coal: return "Уголь";
                case (int)Resources.metan: return "Метан";
                case (int)Resources.rare_gas: return "Инертн. газ";
                case (int)Resources.water: return "Вода";
                case (int)Resources.animals: return "Животные";
                case (int)Resources.plants: return "Растения";
                case (int)Resources.gems: return "Драг. камни";
                case (int)Resources.artifacts: return "Артефакты";
                case (int)Resources.chemical: return "Хим. отходы";
                case (int)Resources.electronics: return "Электроника";
                case (int)Resources.battery: return "Акамуляторы";
                case (int)Resources.explosive: return "Взрывчатка";
                case (int)Resources.medicine: return "Медицина";
                case (int)Resources.plastic: return "Пластик";
                case (int)Resources.biowaste: return "Био отходы";
                case (int)Resources.sand: return "Песок";
                case (int)Resources.glay: return "Глина";
                case (int)Resources.rock: return "Камни";
                case (int)Resources.beton: return "Бетон";
                default: return "";
            }
        }
    }
}
