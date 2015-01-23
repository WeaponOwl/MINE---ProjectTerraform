using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTerraform
{
    public static class Language
    {
        static string[] resources;
        static string[] buildings;
        static string[] science;
        static string[] messages;

        public static void LoadResources(string filename)
        {
            resources = System.IO.File.ReadAllLines(filename);
        }
        public static void LoadBuildings(string filename)
        {
            buildings = System.IO.File.ReadAllLines(filename);
        }
        public static void LoadScience(string filename)
        {
            science = System.IO.File.ReadAllLines(filename);
        }
        public static void LoadMessages(string filename)
        {
            messages = System.IO.File.ReadAllLines(filename);
        }

        public static string GetShortNameOfResource(int res)
        {
            if (res >= 0 && res < Map.maxresources) return resources[res];
            return "";
        }
        public static string GetNameOfResource(int res)
        {
            if (res >= 0 && res < Map.maxresources) return resources[Map.maxresources + res];
            return "";
        }

        public static string GetNameOfBuilding(int type)
        {
            switch (type)
            {
                case Building.CommandCenter:    return buildings[0];
                case Building.BuildCenter:      return buildings[1];
                case Building.StorageCenter:    return buildings[2];
                case Building.LinksCenter:      return buildings[3];
                case Building.ScienceCenter:    return buildings[4];
                case Building.DroidFactory:     return buildings[5];
                case Building.ProcessingFactory: return buildings[6];
                case Building.Mine:             return buildings[7];
                case Building.Dirrick:          return buildings[8];
                case Building.Farm:             return buildings[9];
                case Building.Generator:        return buildings[10];
                case Building.Warehouse:        return buildings[11];
                case Building.House:            return buildings[12];
                case Building.Parking:          return buildings[13];
                case Building.EnergyBank:       return buildings[14];
                case Building.InfoStorage:      return buildings[15];
                case Building.LuquidStorage:    return buildings[16];
                case Building.Spaceport:        return buildings[17];
                case Building.Exchanger:        return buildings[18];
                case Building.Laboratory:       return buildings[19]; 
                case Building.AtmosophereShield: return buildings[20];
                case Building.PowerShield:      return buildings[21];
                case Building.EmmisionShield:   return buildings[22];
                case Building.ClosedFarm:       return buildings[23];
                case Building.UnderShield:      return buildings[24];
                case Building.Collector:        return buildings[25];
                case Building.Beacon:           return buildings[26];
                case Building.Turret:           return buildings[27];
                case Building.AttackFactory:    return buildings[28];
                case Building.RocketParking:    return buildings[29];
                case Building.AttackParking:    return buildings[30];
                case Building.TerrainScaner:    return buildings[31];
                case Building.Builder:          return buildings[32];
                default: return "Error: Noname building";
            }
        }
        public static string GetDescriptionOfBuilding(int type)
        {
            switch (type)
            {
                case Building.CommandCenter: return buildings[33+0];
                case Building.BuildCenter: return buildings[33+1];
                case Building.StorageCenter: return buildings[33+2];
                case Building.LinksCenter: return buildings[33+3];
                case Building.ScienceCenter: return buildings[33+4];
                case Building.DroidFactory: return buildings[33+5];
                case Building.ProcessingFactory: return buildings[33+6];
                case Building.Mine: return buildings[33+7];
                case Building.Dirrick: return buildings[33+8];
                case Building.Farm: return buildings[33+9];
                case Building.Generator: return buildings[33+10];
                case Building.Warehouse: return buildings[33+11];
                case Building.House: return buildings[33+12];
                case Building.Parking: return buildings[33+13];
                case Building.EnergyBank: return buildings[33+14];
                case Building.InfoStorage: return buildings[33+15];
                case Building.LuquidStorage: return buildings[33+16];
                case Building.Spaceport: return buildings[33+17];
                case Building.Exchanger: return buildings[33+18];
                case Building.Laboratory: return buildings[33+19];
                case Building.AtmosophereShield: return buildings[33+20];
                case Building.PowerShield: return buildings[33+21];
                case Building.EmmisionShield: return buildings[33+22];
                case Building.ClosedFarm: return buildings[33+23];
                case Building.UnderShield: return buildings[33+24];
                case Building.Collector: return buildings[33+25];
                case Building.Beacon: return buildings[33+26];
                case Building.Turret: return buildings[33+27];
                case Building.AttackFactory: return buildings[33+28];
                case Building.RocketParking: return buildings[33+29];
                case Building.AttackParking: return buildings[33+30];
                case Building.TerrainScaner: return buildings[33 + 31];
                case Building.Builder: return buildings[33 + 32];
                default: return "Error: Noname building";
            }
        }

        public static string GetNameOfResearch(int type)
        {
            if (type <= 11 && type >= 0)
                return science[type * 2];
            return "Error: Unknown research";
        }
        public static string GetDescriptionOfResearch(int type)
        {
            if (type <= 11 && type >= 0)
                return science[type * 2 + 1];
            return "Error: Unknown research";
        }
        public static string GetNameOfProResearch(int type)
        {
            if (type <= 1 && type >= 0)
                return science[24+type * 2];
            return "Error: Unknown research";
        }
        public static string GetDescriptionOfProResearch(int type)
        {
            if (type <= 1 && type >= 0)
                return science[24+type * 2 + 1];
            return "Error: Unknown research";
        }

        public static string Message_MapCreated { get { return messages[0]; } }
        public static string Message_BuildingCreated { get { return messages[1]; } }
        public static string Message_BuildingDestroyed { get { return messages[2]; } }
        public static string Message_DroneCreated { get { return messages[3]; } }
        public static string Message_FighterCreated { get { return messages[4]; } }
        public static string Message_ResearchFinished { get { return messages[5]; } }
        public static string Message_ColonyHungry { get { return messages[6]; } }
        public static string Message_MeteoriteLanded { get { return messages[7]; } }
        public static string Message_PopulationUp { get { return messages[8]; } }
        public static string Message_MerchantWork { get { return messages[9]; } }
        public static string Message_PopulationDown { get { return messages[10]; } }
        public static string Message_OnAttack { get { return messages[11]; } }
        public static string Message_Sunstrike { get { return messages[12]; } }
        public static string Message_Missile { get { return messages[13]; } }
    }
}
