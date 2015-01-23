using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    public class Building
    {
        //Administration
        public const int CommandCenter = 1;
        public const int BuildCenter = 2;
        public const int StorageCenter = 3;
        public const int LinksCenter = 4;
        public const int ScienceCenter = 5;

        //Manufacturing
        public const int DroidFactory = 11;
        public const int ProcessingFactory = 12;
        public const int Mine = 13;
        public const int Dirrick = 14;
        public const int Farm = 15;
        public const int Generator = 16;
        public const int ClosedFarm = 55;
        public const int AttackFactory = 17;

        //Storage
        public const int Warehouse = 21;
        public const int House = 22;
        public const int Parking = 23;
        public const int EnergyBank = 24;
        public const int InfoStorage = 25;
        public const int LuquidStorage = 26;
        public const int AttackParking = 27;
        public const int RocketParking = 28;

        //Links
        public const int Spaceport = 31;
        public const int Exchanger = 32;
        public const int Beacon = 57;
        public const int Builder = 59;

        //Laboratory
        public const int Laboratory = 41;
        public const int Collector = 42;
        public const int TerrainScaner = 43;

        //Shields
        public const int AtmosophereShield = 51;
        public const int PowerShield = 52;
        public const int EmmisionShield = 53;
        public const int Turret = 58;

        //Decor
        public const int UnderShield = 54;

        public const int TotalBuildings = 63;

        public int type;
        public Vector2 pos;
        public float buildingtime;
        public float maxbuildingtime;
        public float starttime;
        public bool isbuildingnow;
        public float power;
        public int recipte;
        public float wait;
        public float waitwithwork;
        public float worktime;
        public int workcount;
        public int height;
        public int oldheight;
        public int shieldid;
        public float health;
        public int lvl;

        public Building()
        {
            type = 0;
            isbuildingnow = false;
            power = 0;
            recipte = 0;
            wait = 0;
            worktime = 0;
            workcount = 0;
            height = 1;
            shieldid = -1;
            oldheight = 0;
            starttime = 0;
            health = 100;
            lvl = 1;
        }
        public Building(int type)
        {
            this.type = type;
            isbuildingnow = false;
            power = 0;
            recipte = 0;
            wait = 0;
            worktime = 0;
            workcount = 0;
            height = 1;
            oldheight = 0;
            shieldid = -1;
            starttime = 0;
            health = GetSource(type).Width;
            lvl = 1;
        }
        public Building(System.IO.BinaryReader br)
        {
            UnpackedData(br);
        }

        public static string GetName(int type)
        {
            return Language.GetNameOfBuilding(type);
        }
        public static string GetOverview(int type)
        {
            return Language.GetDescriptionOfBuilding(type);
        }
        public static Rectangle GetSource(int type)
        {
            switch (type)
            {
                case CommandCenter: return new Rectangle(0, 0, 192, 124);
                case BuildCenter: return new Rectangle(192, 0, 64, 60);
                case ScienceCenter: return new Rectangle(192, 60, 64, 64);
                case LinksCenter: return new Rectangle(192, 124, 64, 60);
                case StorageCenter: return new Rectangle(192, 184, 64, 67);

                case DroidFactory: return new Rectangle(332, 100, 128 + 12, 129 + 3);
                case ProcessingFactory: return new Rectangle(0, 192, 96 + 12, 62 + 3);
                case Mine: return new Rectangle(160, 251, 96, 48);
                case Dirrick: return new Rectangle(440, 232, 32 + 12, 65 + 3);
                case Farm: return new Rectangle(400, 33, 32, 32);
                case Generator: return new Rectangle(0, 124, 96 + 12, 65 + 3);

                case Warehouse: return new Rectangle(256, 0, 64 + 12, 48 + 3);
                case House: return new Rectangle(256, 102, 64 + 12, 48 + 3);
                case Parking: return new Rectangle(400, 0, 64, 33);
                case EnergyBank: return new Rectangle(256, 308, 64 + 12, 48 + 3);
                case InfoStorage: return new Rectangle(256, 410, 64 + 12, 48 + 3);
                case LuquidStorage: return new Rectangle(256, 204, 64 + 12, 49 + 3);

                case Spaceport: return new Rectangle(332, 0, 68, 97 + 3);
                case Exchanger: return new Rectangle(332, 328, 128 + 2, 83 + 3);

                case Laboratory: return new Rectangle(332, 232, 96 + 12, 96);

                case AtmosophereShield: return new Rectangle(400, 65, 32, 32);
                case PowerShield: return new Rectangle(400 + 32, 65, 32, 32);
                case EmmisionShield: return new Rectangle(400 + 64, 65, 32, 32);

                case UnderShield: return new Rectangle(472, 97, 32, 32);
                case ClosedFarm: return new Rectangle(0, 257, 64, 45);

                case Collector: return new Rectangle(148, 299, 108, 91);

                case Beacon: return new Rectangle(472, 161, 32, 40);
                case Turret: return new Rectangle(462, 300, 32, 80);

                case AttackFactory: return new Rectangle(116, 388, 140, 124);

                case RocketParking: return new Rectangle(108, 124, 64, 33);
                case AttackParking: return new Rectangle(332, 414, 64, 33);
                case TerrainScaner: return new Rectangle(396, 414, 64, 65);

                case Builder: return new Rectangle(332, 447, 32, 40);

                default: return new Rectangle(0, 0, 32, 32);
            }
        }
        public static Vector2 GetAnchor(int type)
        {
            Rectangle rect = GetSource(type);
            Vector2 anchor = new Vector2(rect.Width - 32, rect.Height - 32);
            int[] normal = new int[] {CommandCenter, ClosedFarm,BuildCenter,StorageCenter,LinksCenter,ScienceCenter,
                Mine, Parking, Farm, AtmosophereShield, PowerShield, EmmisionShield, UnderShield , Spaceport , 
                Beacon,Turret,RocketParking,AttackParking,TerrainScaner,Builder};
            bool ok = true;
            foreach (int i in normal) if (type == i) { ok = false; break; }
            if (ok) anchor -= new Vector2(6, 3);
            return anchor;
        }
        public static bool[,] GetPassability(int type)
        {
            switch (type)
            {
                case CommandCenter: return new bool[3, 6] { { false, true, true, true, true, false }, { true, true, true, true, true, true }, { false, true, true, true, true, false } };
                case BuildCenter: return new bool[1, 2] { { true, true } };
                case StorageCenter: return new bool[1, 2] { { true, true } };
                case LinksCenter: return new bool[1, 2] { { true, true } };
                case ScienceCenter: return new bool[1, 2] { { true, true } };

                case DroidFactory: return new bool[2, 4] { { true, true, true, true }, { true, true, true, true } };
                case ProcessingFactory: return new bool[2, 3] { { true, true, true }, { true, true, true } };
                case Mine: return new bool[2, 3] { { true, true, true }, { true, true, true } };
                case Dirrick: return new bool[1, 1] { { true } };
                case Farm: return new bool[1, 1] { { true } };
                case Generator: return new bool[2, 3] { { true, true, false }, { true, true, true } };

                case Warehouse: return new bool[1, 2] { { true, true } };
                case House: return new bool[1, 2] { { true, true } };
                case Parking: return new bool[1, 2] { { true, true } };
                case EnergyBank: return new bool[1, 2] { { true, true } };
                case InfoStorage: return new bool[1, 2] { { true, true } };
                case LuquidStorage: return new bool[1, 2] { { true, true } };

                case Spaceport: return new bool[1, 2] { { true, true } };
                case Exchanger: return new bool[2, 4] { { true, true, true, true }, { true, true, true, true } };

                case Laboratory: return new bool[2, 3] { { false, true, true }, { true, true, true } };

                case AtmosophereShield: return new bool[1, 1] { { true } };
                case PowerShield: return new bool[1, 1] { { true } };
                case EmmisionShield: return new bool[1, 1] { { true } };

                case ClosedFarm: return new bool[1, 2] { { true, true } };
                case UnderShield: return new bool[1, 1] { { true } };

                case Collector: return new bool[2, 3] { { false, true, true }, { true, true, true } };

                case Beacon: return new bool[1, 1] { { true } };
                case Turret: return new bool[1, 1] { { true } };

                case AttackFactory: return new bool[2, 4] { { true, true, true, true }, { true, true, true, true } };

                case RocketParking: return new bool[1, 2] { { true, true } };
                case AttackParking: return new bool[1, 2] { { true, true } };
                case TerrainScaner: return new bool[1, 2] { { true, true } };
                case Builder: return new bool[1, 1] { { true } };

                default: return new bool[1, 1] { { false } };
            }
        }
        public static Point GetSize(int type)
        {
            switch (type)
            {
                case CommandCenter: return new Point(6, 3);
                case BuildCenter: return new Point(2, 1);
                case StorageCenter: return new Point(2, 1);
                case LinksCenter: return new Point(2, 1);
                case ScienceCenter: return new Point(2, 1);

                case DroidFactory: return new Point(4, 2);
                case ProcessingFactory: return new Point(3, 2);
                case Mine: return new Point(3, 2);
                case Dirrick: return new Point(1, 1);
                case Farm: return new Point(1, 1);
                case Generator: return new Point(3, 2);

                case Warehouse: return new Point(2, 1);
                case House: return new Point(2, 1);
                case Parking: return new Point(2, 1);
                case EnergyBank: return new Point(2, 1);
                case InfoStorage: return new Point(2, 1);
                case LuquidStorage: return new Point(2, 1);

                case Spaceport: return new Point(2, 1);
                case Exchanger: return new Point(4, 2);

                case Laboratory: return new Point(3, 2);

                case AtmosophereShield: return new Point(1, 1);
                case PowerShield: return new Point(1, 1);
                case EmmisionShield: return new Point(1, 1);

                case ClosedFarm: return new Point(2, 1);
                case UnderShield: return new Point(1, 1);

                case Collector: return new Point(3, 2);

                case Beacon: return new Point(1, 1);
                case Turret: return new Point(1, 1);

                case AttackFactory: return new Point(4, 2);

                case RocketParking: return new Point(2, 1);
                case AttackParking: return new Point(2, 1);
                case TerrainScaner: return new Point(2, 1);
                case Builder: return new Point(1, 1);

                default: return new Point(1, 1);
            }
        }
        public static int GetType(int type)
        {
            switch (type)
            {
                case CommandCenter: return 0;
                case BuildCenter: return 0;
                case StorageCenter: return 0;
                case LinksCenter: return 0;
                case ScienceCenter: return 0;

                case DroidFactory: return 1;
                case ProcessingFactory: return 1;
                case Mine: return 2;
                case Dirrick: return 2;
                case Farm: return 2;
                case Generator: return 7;

                case Warehouse: return 4;
                case House: return 4;
                case Parking: return 4;
                case EnergyBank: return 4;
                case InfoStorage: return 4;
                case LuquidStorage: return 4;

                case Spaceport: return 5;
                case Exchanger: return 5;

                case Laboratory: return 3;

                case AtmosophereShield: return 6;
                case PowerShield: return 6;
                case EmmisionShield: return 6;

                case ClosedFarm: return 2;
                case UnderShield: return 7;

                case Collector: return 2;

                case Beacon: return 6;
                case Turret: return 6;

                case AttackFactory: return 1;

                case RocketParking: return 4;
                case AttackParking: return 4;
                case TerrainScaner: return 3;
                case Builder: return 6;

                default: return 7;
            }
        }
        public static float GetEnergy(int type)
        {
            switch (type)
            {
                case CommandCenter: return 5;
                case BuildCenter: return 1;
                case StorageCenter: return 1;
                case LinksCenter: return 1;
                case ScienceCenter: return 1;

                case DroidFactory: return 4;
                case ProcessingFactory: return 4;
                case Mine: return 10 / 3.0f;
                case Dirrick: return 10 / 7.0f;
                case Farm: return 0.75f;
                case Generator: return 0.1f;

                case Warehouse: return 1;
                case House: return 1;
                case Parking: return 0.5f;
                case EnergyBank: return 1;
                case InfoStorage: return 1;
                case LuquidStorage: return 1;

                case Spaceport: return 4;
                case Exchanger: return 4;

                case Laboratory: return 5;

                case AtmosophereShield: return 7;
                case PowerShield: return 7;
                case EmmisionShield: return 7;

                case ClosedFarm: return 1.5f;
                case UnderShield: return 1;

                case Collector: return 5;

                case Beacon: return 0.5f;

                case Turret: return 0.7f;

                case AttackFactory: return 4;

                case RocketParking: return 2.5f;
                case AttackParking: return 0.7f;
                case TerrainScaner: return 3;
                case Builder: return 2;

                default: return 1;
            }
        }
        public static float GetBuildTime(int type)
        {
            switch (type)
            {
                case CommandCenter: return 20;
                case BuildCenter: return 4;
                case StorageCenter: return 4;
                case LinksCenter: return 4;
                case ScienceCenter: return 4;

                case DroidFactory: return 15;
                case ProcessingFactory: return 15;
                case Mine: return 10;
                case Dirrick: return 8;
                case Farm: return 4;
                case Generator: return 10;

                case Warehouse: return 5;
                case House: return 5;
                case Parking: return 3;
                case EnergyBank: return 5;
                case InfoStorage: return 5;
                case LuquidStorage: return 5;

                case Spaceport: return 12;
                case Exchanger: return 12;

                case Laboratory: return 10;

                case AtmosophereShield: return 4;
                case PowerShield: return 4;
                case EmmisionShield: return 4;

                case ClosedFarm: return 6;
                case UnderShield: return 1;

                case Collector: return 9;

                case Beacon: return 1;

                case Turret: return 6.5f;

                case AttackFactory: return 15;

                case RocketParking: return 7;
                case AttackParking: return 3;
                case TerrainScaner: return 8;
                case Builder: return 3;

                default: return 1;
            }
        }
        public static float GetBuildPrice(int type)
        {
            return GetBuildTime(type) * 4;
        }

        public byte[] PackedData()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(buildingtime);
            bw.Write(health);
            bw.Write(height);
            bw.Write(isbuildingnow);
            bw.Write(lvl);
            bw.Write(maxbuildingtime);
            bw.Write(oldheight);
            bw.Write(pos.X);
            bw.Write(pos.Y);
            bw.Write(power);
            bw.Write(recipte);
            bw.Write(starttime);
            bw.Write(type);
            bw.Write(wait);
            bw.Write(waitwithwork);
            bw.Write(workcount);
            bw.Write(worktime);


            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedData(byte[] data)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            UnpackedData(br);

            br.Close();
            mem.Close();
        }
        public void UnpackedData(System.IO.BinaryReader br)
        {
            buildingtime = br.ReadSingle();
            health = br.ReadSingle();
            height = br.ReadInt32();
            isbuildingnow = br.ReadBoolean();
            lvl = br.ReadInt32();
            maxbuildingtime = br.ReadSingle();
            oldheight = br.ReadInt32();
            pos = new Vector2(br.ReadSingle(), br.ReadSingle());
            power = br.ReadSingle();
            recipte = br.ReadInt32();
            starttime = br.ReadSingle();
            type = br.ReadInt32();
            wait = br.ReadSingle();
            waitwithwork = br.ReadSingle();
            workcount = br.ReadInt32();
            worktime = br.ReadSingle();
            shieldid = -1;
        }
    }

    public struct Cell
    {
        public bool can_build;
        public bool can_move;
        public bool build_draw;
        public short build_id;
        public short id;
        public short ground_id;
        public bool subground;
        public short height;
        public short mineresource_id;
        public short dirrickresource_id;

        public Cell(short id)
        {
            can_build = true;
            can_move = true;
            build_draw = false;
            build_id = -1;
            this.id = id;
            ground_id = -1;
            height = 3;
            mineresource_id = -1;
            dirrickresource_id = -1;
            subground = false;
        }
    }

    public struct InventoryItem
    {
        public float count;
        public float exchangecount;
        public byte exchangetype;
        public int buildingproduct;
    }
    public struct BuildingTypeInfoItem
    {
        public float maxpower;
        public float consumepower;
        public float havepower;
    }
    public struct StorageInfoItem
    {
        public float maxcapability;
        public float currentcapability;
    }
    struct BuildingInfo
    {
        public int count;
        public int workincount;
        public float workingpower;
        public List<int> ids;
    }

    public class Shield
    {
        public const int Power = 0;
        public const int Emmision = 1;
        public const int Atmosphere = 2;

        public int type;
        public float size;
        public Vector2 pos;
        public float createtime;

        public Shield(int type, float size, Vector2 pos, float time)
        {
            this.type = type;
            this.size = size;
            this.pos = pos;
            createtime = time;
        }
    }
    public class Perk
    {
        public float storagebonus;
        public float movementbonus;
        public float sciencebonus;
        public float buildbonus;

        public float buildingspeedup;
        public float unitspeedup;
        public float optimizestorage;
        public float optimizeore;
        public float optimizingshield;
        public float optimizeerergyproduce;
        public float otimizeenergyconsume;
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

    public class ScienceItem
    {
        public int id;
        public float time;
        public string name;
        public string overview;
        public bool searched;
        public int[] reserchresources;
        public float[] reserchvalues;
        public int workscience;
        public float power;

        public ScienceItem(int id,float t, string n, int needscience, int[] res, float[] val, string o)
        {
            this.id = id;
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
    public class Science
    {
        public ScienceItem[] items;

        public Science(int num)
        {
            items = new ScienceItem[num];
        }
    }

    public class Map
    {
        public short player_id;

        public int width, height;
        public Cell[,] data;

        public List<Building> buildings;
        public List<Unit> units;

        public const int maxresources = 32;
        public InventoryItem[] inventory;
        public float energyproduction;
        public int population;
        public float timetohunger;

        public const int maxbuildingtype = 8;
        public BuildingTypeInfoItem[] buildingtypeinfo;

        public StorageInfoItem[] storageinfo;

        public string name;
        public bool energyboost;
        public bool scienceboost;
        public int maxbuildinglvl;

        //--set in generator
        public Planet planet;
        public int baseid;
        public Vector2 position;
        //--//

        public int[] selectedresearch;
        public int currentresearchmode;

        public float timetodestroy;
        public double createtime;

        public Science[] science;

        public List<Shield> shields;
        public Perk perk;

        public List<Meteorite> meteorites;

        public List<string> messages;

        public byte[] PackedDescription()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(player_id);
            bw.Write(position.X);
            bw.Write(position.Y);
            bw.Write(name);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedDescription(byte[] description)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(description);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            UnpackedDescription(br);

            br.Close();
            mem.Close();
        }
        public void UnpackedDescription(System.IO.BinaryReader br)
        {
            player_id = br.ReadInt16();
            position = new Vector2(br.ReadSingle(), br.ReadSingle());
            name = br.ReadString();

            shields = new List<Shield>();
        }

        public byte[] PackedData()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(width);
            bw.Write(height);

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    bw.Write(data[i, j].build_draw);
                    bw.Write(data[i, j].build_id);
                    bw.Write(data[i, j].can_build);
                    bw.Write(data[i, j].can_move);
                    bw.Write(data[i, j].dirrickresource_id);
                    bw.Write(data[i, j].ground_id);
                    bw.Write(data[i, j].height);
                    bw.Write(data[i, j].id);
                    bw.Write(data[i, j].mineresource_id);
                    bw.Write(data[i, j].subground);
                }

            bw.Write(buildings.Count);
            for (int i = 0; i < buildings.Count; i++)
            {
                bw.Write(buildings[i].PackedData());
            }

            bw.Write(units.Count);
            for (int i = 0; i < units.Count; i++)
            {
                bw.Write(units[i].PackedData());
            }

            for (int i = 0; i < maxresources; i++)
            {
                bw.Write(inventory[i].buildingproduct);
                bw.Write(inventory[i].count);
                bw.Write(inventory[i].exchangecount);
                bw.Write(inventory[i].exchangetype);
            }

            bw.Write(population);
            bw.Write(timetohunger);
            bw.Write(energyboost);
            bw.Write(maxbuildinglvl);

            for (int i = 0; i < maxbuildingtype; i++)
            {
                bw.Write(buildingtypeinfo[i].consumepower);
                bw.Write(buildingtypeinfo[i].havepower);
                bw.Write(buildingtypeinfo[i].maxpower);
            }

            for (int i = 0; i < 6; i++)
            {
                bw.Write(storageinfo[i].currentcapability);
                bw.Write(storageinfo[i].maxcapability);
            }

            bw.Write(selectedresearch.Length);
            for (int i = 0; i < selectedresearch.Length; i++)
                bw.Write(selectedresearch[i]);
            bw.Write(currentresearchmode);

            bw.Write(timetodestroy);
            bw.Write(createtime);

            bw.Write((int)science.Length);
            for (int i = 0; i < science.Length; i++)
            {
                bw.Write(science[i].items.Length);
                for (int j = 0; j < science[i].items.Length; j++)
                {
                    bw.Write(science[i].items[j].id);
                    bw.Write(science[i].items[j].name);
                    bw.Write(science[i].items[j].overview);
                    bw.Write(science[i].items[j].power);
                    bw.Write(science[i].items[j].reserchresources.Length);
                    for (int k = 0; k < science[i].items[j].reserchresources.Length; k++)
                        bw.Write(science[i].items[j].reserchresources[k]);
                    for (int k = 0; k < science[i].items[j].reserchvalues.Length; k++)
                        bw.Write(science[i].items[j].reserchvalues[k]);
                    bw.Write(science[i].items[j].searched);
                    bw.Write(science[i].items[j].time);
                    bw.Write(science[i].items[j].workscience);
                }
            }

            bw.Write(perk.buildbonus);
            bw.Write(perk.buildingspeedup);
            bw.Write(perk.movementbonus);
            bw.Write(perk.optimizeerergyproduce);
            bw.Write(perk.optimizeore);
            bw.Write(perk.optimizeresearch);
            bw.Write(perk.optimizestorage);
            bw.Write(perk.optimizingshield);
            bw.Write(perk.otimizeenergyconsume);
            bw.Write(perk.sciencebonus);
            bw.Write(perk.storagebonus);
            bw.Write(perk.unitspeedup);

            bw.Write(meteorites.Count);
            foreach (Meteorite m in meteorites)
                bw.Write(m.PackedData());

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedData(byte[] data)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
            System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

            UnpackedData(br);

            br.Close();
            mem.Close();
        }
        public void UnpackedData(System.IO.BinaryReader br)
        {
            width = br.ReadInt32();
            height = br.ReadInt32();

            this.data = new Cell[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    this.data[i, j] = new Cell(0);
                    this.data[i, j].build_draw = br.ReadBoolean();
                    this.data[i, j].build_id = br.ReadInt16();
                    this.data[i, j].can_build = br.ReadBoolean();
                    this.data[i, j].can_move = br.ReadBoolean();
                    this.data[i, j].dirrickresource_id = br.ReadInt16();
                    this.data[i, j].ground_id = br.ReadInt16();
                    this.data[i, j].height = br.ReadInt16();
                    this.data[i, j].id = br.ReadInt16();
                    this.data[i, j].mineresource_id = br.ReadInt16();
                    this.data[i, j].subground = br.ReadBoolean();
                }

            int bc = br.ReadInt32();
            buildings = new List<Building>();
            for (int i = 0; i < bc; i++)
            {
                Building b = new Building(br);
                buildings.Add(b);
            }

            int uc = br.ReadInt32();
            units = new List<Unit>();
            for (int i = 0; i < uc; i++)
            {
                Unit u = new Unit(br);

                units.Add(u);
            }

            inventory = new InventoryItem[maxresources];
            for (int i = 0; i < maxresources; i++)
            {
                inventory[i].buildingproduct = br.ReadInt32();
                inventory[i].count = br.ReadSingle();
                inventory[i].exchangecount = br.ReadSingle();
                inventory[i].exchangetype = br.ReadByte();
            }

            population = br.ReadInt32();
            timetohunger = br.ReadSingle();
            energyboost = br.ReadBoolean();
            maxbuildinglvl = br.ReadInt32();

            buildingtypeinfo = new BuildingTypeInfoItem[maxbuildingtype];
            for (int i = 0; i < maxbuildingtype; i++)
            {
                buildingtypeinfo[i].consumepower = br.ReadSingle();
                buildingtypeinfo[i].havepower = br.ReadSingle();
                buildingtypeinfo[i].maxpower = br.ReadSingle();
            }

            storageinfo = new StorageInfoItem[6];
            for (int i = 0; i < 6; i++)
            {
                storageinfo[i].currentcapability = br.ReadSingle();
                storageinfo[i].maxcapability = br.ReadSingle();
            }

            selectedresearch = new int[br.ReadInt32()];
            for (int i = 0; i < selectedresearch.Length; i++)
                selectedresearch[i] = br.ReadInt32();
            currentresearchmode = br.ReadInt32();

            timetodestroy = br.ReadSingle();
            createtime = br.ReadDouble();

            science = new Science[br.ReadInt32()];
            for (int i = 0; i < science.Length; i++)
            {
                science[i] = new Science(br.ReadInt32());
                //science[i].items = new ScienceItem[br.ReadInt32()];
                for (int j = 0; j < science[i].items.Length; j++)
                {
                    science[i].items[j] = new ScienceItem(0,0, "", 0, null, null, "");
                    science[i].items[j].id = br.ReadInt32();
                    science[i].items[j].name = br.ReadString();
                    science[i].items[j].overview = br.ReadString();
                    science[i].items[j].power = br.ReadSingle();
                    science[i].items[j].reserchresources = new int[br.ReadInt32()];
                    science[i].items[j].reserchvalues = new float[science[i].items[j].reserchresources.Length];
                    for (int k = 0; k < science[i].items[j].reserchresources.Length; k++)
                        science[i].items[j].reserchresources[k] = br.ReadInt32();
                    for (int k = 0; k < science[i].items[j].reserchvalues.Length; k++)
                        science[i].items[j].reserchvalues[k] = br.ReadSingle();
                    science[i].items[j].searched = br.ReadBoolean();
                    science[i].items[j].time = br.ReadSingle();
                    science[i].items[j].workscience = br.ReadInt32();
                }
            }

            perk = new Perk();
            perk.buildbonus = br.ReadSingle();
            perk.buildingspeedup = br.ReadSingle();
            perk.movementbonus = br.ReadSingle();
            perk.optimizeerergyproduce = br.ReadSingle();
            perk.optimizeore = br.ReadSingle();
            perk.optimizeresearch = br.ReadSingle();
            perk.optimizestorage = br.ReadSingle();
            perk.optimizingshield = br.ReadSingle();
            perk.otimizeenergyconsume = br.ReadSingle();
            perk.sciencebonus = br.ReadSingle();
            perk.storagebonus = br.ReadSingle();
            perk.unitspeedup = br.ReadSingle();

            int meteoritescount = br.ReadInt32();
            meteorites = new List<Meteorite>();
            for (int i = 0; i < meteoritescount; i++)
            {
                Meteorite meteorite = new Meteorite(br);
                meteorites.Add(meteorite);
            }
        }

        public byte[] PackedDataUnits()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(units.Count);
            for (int i = 0; i < units.Count; i++)
            {
                bw.Write(units[i].PackedData());
            }

            bw.Write(meteorites.Count);
            foreach (Meteorite m in meteorites)
                bw.Write(m.PackedData());

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public byte[] PackedDataBuildings()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(buildings.Count);
            for (int i = 0; i < buildings.Count; i++)
            {
                bw.Write(buildings[i].PackedData());
            }

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedDataUnits(System.IO.BinaryReader br)
        {
            int uc = br.ReadInt32();
            units = new List<Unit>();
            for (int i = 0; i < uc; i++)
            {
                Unit u = new Unit(br);

                units.Add(u);
            }

            int meteoritescount = br.ReadInt32();
            meteorites = new List<Meteorite>();
            for (int i = 0; i < meteoritescount; i++)
            {
                Meteorite meteorite = new Meteorite(br);
                meteorites.Add(meteorite);
            }
        }
        public void UnpackedDataBuildings(System.IO.BinaryReader br)
        {
            int bc = br.ReadInt32();
            buildings = new List<Building>();
            for (int i = 0; i < bc; i++)
            {
                Building b = new Building(br);
                buildings.Add(b);
            }
        }

        public byte[] PackedSync()
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(buildings.Count);
            for (int i = 0; i < buildings.Count; i++)
            {
                bw.Write(buildings[i].PackedData());
            }

            bw.Write(units.Count);
            for (int i = 0; i < units.Count; i++)
            {
                bw.Write(units[i].PackedData());
            }

            for (int i = 0; i < maxresources; i++)
            {
                bw.Write(inventory[i].buildingproduct);
                bw.Write(inventory[i].count);
                bw.Write(inventory[i].exchangecount);
                bw.Write(inventory[i].exchangetype);
            }

            bw.Write(population);
            bw.Write(timetohunger);

            bw.Write(meteorites.Count);
            foreach (Meteorite m in meteorites)
                bw.Write(m.PackedData());

            bw.Write(planet.timetosolarstrike);

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();

            return retbuf;
        }
        public void UnpackedSync(System.IO.BinaryReader br)
        {
            int bc = br.ReadInt32();
            buildings = new List<Building>();
            for (int i = 0; i < bc; i++)
            {
                Building b = new Building(br);
                buildings.Add(b);
            }

            shields = new List<Shield>();

            int uc = br.ReadInt32();
            units = new List<Unit>();
            for (int i = 0; i < uc; i++)
            {
                Unit u = new Unit(br);

                units.Add(u);
            }

            inventory = new InventoryItem[maxresources];
            for (int i = 0; i < maxresources; i++)
            {
                inventory[i].buildingproduct = br.ReadInt32();
                inventory[i].count = br.ReadSingle();
                inventory[i].exchangecount = br.ReadSingle();
                inventory[i].exchangetype = br.ReadByte();
            }

            population = br.ReadInt32();
            timetohunger = br.ReadSingle();

            int meteoritescount = br.ReadInt32();
            meteorites = new List<Meteorite>();
            for (int i = 0; i < meteoritescount; i++)
            {
                Meteorite meteorite = new Meteorite(br);
                meteorites.Add(meteorite);
            }

            planet.timetosolarstrike = br.ReadSingle();

            ResetBuildingsReflection();
        }

        public byte[] PacketBuildingsShadows()
        {
            bool[] data = new bool[64 * 64];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    data[i * width + j] = this.data[i, j].build_id >= 0;
                }

            int bytes = data.Length / 8;
            byte[] bdata = new byte[bytes];
            int bitIndex = 0, byteIndex = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i])
                {
                    bdata[byteIndex] |= (byte)(((byte)1) << bitIndex);
                }
                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bdata;
        }
        public bool[] UnpackedBuildingsShadows(byte[] array)
        {
            System.Collections.BitArray ba = new System.Collections.BitArray(array);
            bool[] data = new bool[64 * 64];
            ba.CopyTo(data, 0);
            return data;
        }

        public Map()
        {
            ;
        }
        public Map(int width = 64, int height = 64)
        {
            player_id = 0;
            energyboost = true;
            scienceboost = true;
            maxbuildinglvl = 1;

            this.width = width;
            this.height = height;
            data = new Cell[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    data[i, j] = new Cell(0);
                }

            buildings = new List<Building>();
            units = new List<Unit>();

            inventory = new InventoryItem[maxresources];
            population = 0;
            buildingtypeinfo = new BuildingTypeInfoItem[maxbuildingtype];
            for (int i = 0; i < maxbuildingtype; i++)
                buildingtypeinfo[i].maxpower = 1;

            storageinfo = new StorageInfoItem[6];
            storageinfo[Constants.Map_energy].currentcapability = 1010;
            storageinfo[Constants.Map_energy].maxcapability = 1010;
            storageinfo[Constants.Map_coins].maxcapability = -1;
            storageinfo[Constants.Map_science].maxcapability = 20;
            storageinfo[Constants.Map_rocks].maxcapability = 1000;
            storageinfo[Constants.Map_luquids].maxcapability = 200;
            storageinfo[Constants.Map_humans].maxcapability = 0;

            name = "";

            selectedresearch = new int[2];
            currentresearchmode = 0;
            timetodestroy = Constants.Map_timetodestroybase;
            createtime = 0;

            science = new Science[2];
            science[0] = new Science(10);
            science[0].items[0] = new ScienceItem(Constants.Research_terrainexplored,100, Language.GetNameOfResearch(Constants.Research_terrainexplored), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_terrainexplored));
            science[0].items[1] = new ScienceItem(Constants.Research_optimizeresearch, 100, Language.GetNameOfResearch(Constants.Research_optimizeresearch), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_optimizeresearch));
            science[0].items[2] = new ScienceItem(Constants.Research_buildingspeedup, 100, Language.GetNameOfResearch(Constants.Research_buildingspeedup), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_buildingspeedup));
            science[0].items[3] = new ScienceItem(Constants.Research_unitspeedup, 100, Language.GetNameOfResearch(Constants.Research_unitspeedup), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_unitspeedup));
            science[0].items[4] = new ScienceItem(Constants.Research_optimizestorage, 100, Language.GetNameOfResearch(Constants.Research_optimizestorage), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_optimizestorage));
            science[0].items[5] = new ScienceItem(Constants.Research_optimizeore, 100, Language.GetNameOfResearch(Constants.Research_optimizeore), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_optimizeore));
            science[0].items[6] = new ScienceItem(Constants.Research_optimizingshield, 100, Language.GetNameOfResearch(Constants.Research_optimizingshield), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_optimizingshield));
            science[0].items[7] = new ScienceItem(Constants.Research_optimizeerergyproduce, 100, Language.GetNameOfResearch(Constants.Research_optimizeerergyproduce), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_optimizeerergyproduce));
            science[0].items[8] = new ScienceItem(Constants.Research_otimizeenergyconsume, 100, Language.GetNameOfResearch(Constants.Research_otimizeenergyconsume), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_otimizeenergyconsume));
            science[0].items[9] = new ScienceItem(Constants.Research_orbitalmodules, 100, Language.GetNameOfResearch(Constants.Research_orbitalmodules), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_orbitalmodules));
            //science[0].items[10] = new ScienceItem(Constants.Research_basicresearchartifact,200, Language.GetNameOfResearch(Constants.Research_basicresearchartifact), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_basicresearchartifact));
            //science[0].items[11] = new ScienceItem(Constants.Research_proresearchartifact, 300, Language.GetNameOfResearch(Constants.Research_proresearchartifact), 3, new int[0], new float[0], Language.GetDescriptionOfResearch(Constants.Research_proresearchartifact));
            science[1] = new Science(2);
            science[1].items[0] = new ScienceItem(Constants.ProResearch_climatcontrol,200, Language.GetNameOfProResearch(Constants.ProResearch_climatcontrol), 2, new int[0], new float[0], Language.GetDescriptionOfProResearch(Constants.ProResearch_climatcontrol));
            science[1].items[1] = new ScienceItem(Constants.ProResearch_planetdefence, 200, Language.GetNameOfProResearch(Constants.ProResearch_planetdefence), 2, new int[0], new float[0], Language.GetDescriptionOfProResearch(Constants.ProResearch_planetdefence));

            shields = new List<Shield>();
            perk = new Perk();
            meteorites = new List<Meteorite>();

            inventory[(int)Resources.credits].count = 1000;
            inventory[(int)Resources.energy].count = 1010;

            //inventory[(int)Resources.metal].count = 1010;
            //inventory[(int)Resources.electronics].count = 1010;
        }
        public Map(System.IO.BinaryReader br)
        {
            UnpackedDescription(br);
        }

        public bool TryBuilding(int x, int y, int type)
        {
            if (x < 0 || y < 0 || x >= width || y >= height) return false;
            if (inventory[(int)Resources.credits].count < Building.GetBuildPrice(type)) return false;
            Point size = Building.GetSize(type);
            //Vector2 pos = buildings[data[y, x].build_id].pos;
            //if (data[y, x].build_id >= 0)
            //{
            //    //int x2 = (int)buildings[data[y, x].build_id].pos.X - 1;
            //    int x2 = (int)buildings[data[y, x].build_id].pos.X;
            //    int y2 = (int)buildings[data[y, x].build_id].pos.Y;
            //    x2 = x; y2 = y;
            //}
            //if (data[y + size.Y - 1, x + size.X-1].build_draw)
            if (data[y, x].build_draw && data[y, x].build_id<buildings.Count)
            {
                if (type == Building.Warehouse || type == Building.House)
                {
                    if (buildings[data[y, x].build_id].type == type && buildings[data[y, x].build_id].height < 5) return true;
                }

                if (type == Building.EnergyBank || type == Building.InfoStorage)
                {
                    if (buildings[data[y, x].build_id].type == type && buildings[data[y, x].build_id].height < 3) return true;
                }

                if (type == Building.LuquidStorage)
                {
                    if (buildings[data[y, x].build_id].type == type && buildings[data[y, x].build_id].height < 2) return true;
                }

                if (type == Building.AtmosophereShield || type == Building.EmmisionShield || type == Building.PowerShield)
                {
                    if (buildings[data[y, x].build_id].type == type && buildings[data[y, x].build_id].height < 7) return true;
                }
            }
            for (int i = 0; i < size.Y; i++)
                for (int j = 0; j < size.X; j++)
                {
                    if (!(data[y - i, x - j].can_build && data[y - i, x - j].height == data[y, x].height)) return false;
                }
            return true;
        }
        public void AddBuilding(int x, int y, int type,bool fastcreate=false)
        {
            Point size = Building.GetSize(type);
            if (TryBuilding(x + size.X - 1, y + size.Y - 1, type))
            {
                inventory[(int)Resources.credits].count -= Building.GetBuildPrice(type);

                if (data[y + size.Y - 1, x + size.X - 1].build_draw)
                {
                    if (type == Building.AtmosophereShield || type == Building.EmmisionShield || type == Building.PowerShield)
                    {
                        buildings[data[y, x].build_id].height++;
                        buildings[data[y, x].build_id].buildingtime += Building.GetBuildTime(type);
                        buildings[data[y, x].build_id].maxbuildingtime += Building.GetBuildTime(type);
                        return;
                    }
                    if (type == Building.Warehouse || type == Building.InfoStorage || type == Building.House || type == Building.EnergyBank || type == Building.LuquidStorage)
                    {
                        buildings[data[y, x].build_id].height++;
                        buildings[data[y, x].build_id].buildingtime += Building.GetBuildTime(type);
                        buildings[data[y, x].build_id].maxbuildingtime += Building.GetBuildTime(type);
                        return;
                    }
                }

                bool[,] pass = Building.GetPassability(type);

                buildings.Add(new Building(type));
                if(!fastcreate)
                    buildings[buildings.Count - 1].maxbuildingtime = buildings[buildings.Count - 1].buildingtime = Building.GetBuildTime(type);
                buildings[buildings.Count - 1].pos = new Vector2(x + size.X - 1, y + size.Y - 1);
                buildings[buildings.Count - 1].power = 1;
                if (planet != null && planet.star != null)
                    buildings[buildings.Count - 1].starttime = (float)planet.star.GetTotalTime();
                else buildings[buildings.Count - 1].starttime = 0;

                for (int i = 0; i < size.Y; i++)
                    for (int j = 0; j < size.X; j++)
                    {
                        data[y + i, x + j].can_build = false;
                        data[y + i, x + j].build_id = (short)(buildings.Count - 1);
                        data[y + i, x + j].can_move = !pass[i, j];
                    }
                data[y + size.Y - 1, x + size.X - 1].build_draw = true;

                if (type == Building.Farm)
                {
                    buildings[buildings.Count - 1].recipte = (x + y) % 3;
                    if (buildings[buildings.Count - 1].recipte > 0) buildings[buildings.Count - 1].recipte++;
                }
                if (type == Building.ClosedFarm) buildings[buildings.Count - 1].recipte = (x / 2 + y) % 4;

                if (type == Building.Collector) buildings[buildings.Count - 1].recipte = (x / 4 + y / 2) % 2;

                ResetPerks();
            }
        }
        public void DestroyBuilding(int px, int py)
        {
            int bid = data[py, px].build_id;
            bool offshields = false;
            if (data[py, px].build_id >= 0 && bid<buildings.Count)
            {
                int type = buildings[bid].type;
                if (type == Building.Warehouse && buildings[bid].buildingtime <= 0)
                    storageinfo[Constants.Map_rocks].maxcapability -= Constants.Map_warehoucestoragecapability;
                if (type == Building.InfoStorage && buildings[bid].buildingtime <= 0)
                    storageinfo[Constants.Map_science].maxcapability -= Constants.Map_infostoragestoragecapability;
                if (type == Building.House && buildings[bid].buildingtime <= 0)
                    storageinfo[Constants.Map_humans].maxcapability -= Constants.Map_housestoragecapability;
                if (type == Building.EnergyBank && buildings[bid].buildingtime <= 0)
                    storageinfo[Constants.Map_energy].maxcapability -= Constants.Map_energybankstoragecapability;
                if (type == Building.LuquidStorage && buildings[bid].buildingtime <= 0)
                    storageinfo[Constants.Map_luquids].maxcapability -= Constants.Map_reservourstoragecapability;
                if (type == Building.AtmosophereShield || type == Building.PowerShield || type == Building.EmmisionShield)
                {
                    shields.Clear();
                    offshields = true;
                }

                if (buildings[bid].buildingtime > 0) inventory[(int)Resources.credits].count += Building.GetBuildPrice(buildings[bid].type) * buildings[bid].buildingtime / Building.GetBuildTime(buildings[bid].type) * 2 / 3;
                inventory[(int)Resources.credits].count += Building.GetBuildPrice(buildings[bid].type) / 3;

                Point size = Building.GetSize(buildings[bid].type);
                int x = (int)buildings[bid].pos.X;
                int y = (int)buildings[bid].pos.Y;
                AddMessage(Language.Message_BuildingDestroyed + " " + Building.GetName(buildings[bid].type));
                buildings.RemoveAt(bid);
                for (int i = 0; i < size.Y; i++)
                    for (int j = 0; j < size.X; j++)
                    {
                        data[y - i, x - j].build_id = -1;
                        data[y - i, x - j].build_draw = false;
                        data[y - i, x - j].can_build = true;
                        data[y - i, x - j].can_move = true;
                    }
            }

            for (int k = 0; k < buildings.Count; k++)
            {
                if (offshields) buildings[k].shieldid = -1;

                Point size = Building.GetSize(buildings[k].type);
                int x = (int)buildings[k].pos.X;
                int y = (int)buildings[k].pos.Y;

                for (int i = 0; i < size.Y; i++)
                    for (int j = 0; j < size.X; j++)
                    {
                        data[y - i, x - j].build_id = (short)k;
                    }
            }
            ResetPerks();
        }

        public void ResetBuildingsReflection()
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    data[i, j].build_id = -1;
                }

            for (int k = 0; k < buildings.Count; k++)
            {
                buildings[k].shieldid = -1;

                Point size = Building.GetSize(buildings[k].type);
                int x = (int)buildings[k].pos.X;
                int y = (int)buildings[k].pos.Y;

                for (int i = 0; i < size.Y; i++)
                    for (int j = 0; j < size.X; j++)
                        data[y - i, x - j].build_id = (short)k;
            }
        }

        public void AddDrone(float x, float y)
        {
            units.Add(new Unit(Unit.Drone, x, y));
            units[units.Count - 1].command = new Command(commands.patrol, 0, 0);
        }
        public void AddAttackDrone(float x, float y)
        {
            units.Add(new Unit(Unit.AttackDrone, x, y));
            units[units.Count - 1].command = new Command(commands.gotoparking, 0, 0);
        }

        void ResetPerks()
        {
            int buildscenters = 0, storagecenters = 0, linkcenter = 0, sciencecenters = 0;
            foreach (Building b in buildings)
            {
                if (b.type == Building.BuildCenter) buildscenters++;
                if (b.type == Building.StorageCenter) storagecenters++;
                if (b.type == Building.LinksCenter) linkcenter++;
                if (b.type == Building.ScienceCenter) sciencecenters++;
            }

            perk.storagebonus = 1 + (storagecenters * Constants.System_perkkoef) / (1 + storagecenters * Constants.System_perkkoef);
            perk.buildbonus = 1 + (buildscenters * Constants.System_perkkoef) / (1 + buildscenters * Constants.System_perkkoef);
            perk.movementbonus = 1 + (linkcenter * Constants.System_perkkoef) / (1 + linkcenter * Constants.System_perkkoef);
            perk.sciencebonus = 1 + (sciencecenters * Constants.System_perkkoef) / (1 + sciencecenters * Constants.System_perkkoef);
        }
        int GetResourceType(int resource)
        {
            if (resource == 0) return 0;
            if (resource == (int)Resources.animals || resource == (int)Resources.plants) return Constants.Map_science;
            if (resource == 1) return 1;
            if (resource == (int)Resources.water || resource == (int)Resources.oil || resource == (int)Resources.biowaste || resource == (int)Resources.chemical) return 4;
            return 3;
        }

        List<Point> finishpoint;
        int PointComparer(Point x, Point y)
        {
            return Math.Abs(x.X - finishpoint[0].X) + Math.Abs(x.Y - finishpoint[0].Y) - Math.Abs(y.X - finishpoint[0].X) - Math.Abs(y.Y - finishpoint[0].Y);
        }
        public List<Vector2> GetWayToBuilding(Vector2 start, Vector2 end)
        {
            List<Vector2> way = new List<Vector2>();

            finishpoint = new List<Point>();
            if (data[(int)end.Y, (int)end.X].build_id>=0)
            {
                Point size = Building.GetSize(buildings[data[(int)end.Y, (int)end.X].build_id].type);
                for (int i = 0; i < size.Y; i++)
                {
                    if (data[(int)end.Y - i, (int)end.X + 1].can_move) finishpoint.Add(new Point((int)end.X + 1, (int)end.Y - i));
                    if (data[(int)end.Y - i, (int)end.X - size.X].can_move) finishpoint.Add(new Point((int)end.X - size.X, (int)end.Y - i));
                }
                for (int j = 0; j < size.X; j++)
                {
                    if (data[(int)end.Y + 1, (int)end.X - j].can_move) finishpoint.Add(new Point((int)end.X - j, (int)end.Y + 1));
                    if (data[(int)end.Y - size.Y, (int)end.X - j].can_move) finishpoint.Add(new Point((int)end.X - j, (int)end.Y - size.Y));
                }
            }

            if (finishpoint.Count == 0) return null;

            List<Point> open = new List<Point>();
            List<Point> closed = new List<Point>();
            open.Add(new Point((int)start.X, (int)start.Y));

            Point now = new Point(100000, 100000);

            bool finish = false;

            while (open.Count > 0 && !finish)
            {
                open.Sort(PointComparer);
                now = open[0];
                open.RemoveAt(0);

                for (int i = -1; i <= 1; i++)
                    if (!finish)
                        for (int j = -1; j <= 1; j++)
                        {
                            Point p = new Point(now.X + i, now.Y + j);
                            if (!(i == 0 && j == 0) && onMap(p.X, p.Y) && data[p.Y, p.X].can_move && Math.Abs(data[p.Y, p.X].height - data[now.Y, now.X].height) <= 1 && !closed.Contains(p))
                            {
                                if (finishpoint.Contains(p))
                                {
                                    finish = true; break;
                                }
                                if (!open.Contains(p)) open.Add(p);
                            }
                        }
                closed.Add(now);
            }

            int k = closed.Count - 1;
            int l = 100000;
            way.Add(end);
            foreach (Point p in finishpoint)
            {
                int nl = (int)Math.Abs(p.X - closed[k].X) + (int)Math.Abs(p.Y - closed[k].Y);
                if (nl < l) { l = nl; way[0] = new Vector2(p.X, p.Y); }
            }
            for (int i = closed.Count - 1; i >= 0; i--)
            {
                int dx = closed[i].X - closed[k].X;
                int dy = closed[i].Y - closed[k].Y;

                if (dx >= -1 && dx <= 1 && dy >= -1 && dy <= 1)
                {
                    k = i;
                    way.Add(new Vector2(closed[k].X, closed[k].Y));
                }
            }

            way.RemoveAt(way.Count - 1);
            way.Reverse();

            if (way.Count > 1 && (way[way.Count - 1] - way[way.Count - 2]).Length() > 2) return null;

            return way;
        }
        public List<Vector2> GetWay(Vector2 start, Vector2 end)
        {
            List<Vector2> way = new List<Vector2>();

            finishpoint = new List<Point>();
            finishpoint.Add(new Point((int)end.X, (int)end.Y));
            List<Point> open = new List<Point>();
            List<Point> closed = new List<Point>();
            open.Add(new Point((int)start.X, (int)start.Y));

            Point now = new Point(100000, 100000);

            bool finish = false;

            while (open.Count > 0 && !finish)
            {
                open.Sort(PointComparer);
                now = open[0];
                open.RemoveAt(0);

                for (int i = -1; i <= 1; i++)
                    if (!finish)
                        for (int j = -1; j <= 1; j++)
                        {
                            Point p = new Point(now.X + i, now.Y + j);
                            if (!(i == 0 && j == 0) && onMap(p.X, p.Y) && data[p.Y, p.X].can_move && Math.Abs(data[p.Y, p.X].height - data[now.Y, now.X].height) <= 1 && !closed.Contains(p))
                            {
                                if (finishpoint.Contains(p)) { finish = true; break; }
                                if (!open.Contains(p)) open.Add(p);
                            }
                        }
                closed.Add(now);
            }

            int k = closed.Count - 1;
            //way.Add(new Vector2(closed[k].X, closed[k].Y));
            way.Add(end);
            for (int i = closed.Count - 1; i >= 0; i--)
            {
                int dx = closed[i].X - closed[k].X;
                int dy = closed[i].Y - closed[k].Y;

                if (dx >= -1 && dx <= 1 && dy >= -1 && dy <= 1)
                {
                    k = i;
                    way.Add(new Vector2(closed[k].X, closed[k].Y));
                }
            }

            way.RemoveAt(way.Count - 1);
            way.Reverse();

            return way;
        }
        public Vector2 GetRandomBaseDirection()
        {
            List<Vector2> pos = new List<Vector2>();
            pos.Add(new Vector2(new Random().Next(width + 2) - 1, -1));

            int selectpos = new Random().Next(pos.Count);
            return pos[selectpos];
        }

        void TryAddResource(int resource, float count, bool iscommandcenter)
        {
            int type = GetResourceType(resource);
            if (storageinfo[type].maxcapability < 0)
            {
                inventory[resource].count += count;
                return;
            }
            if (iscommandcenter || type == (int)Resources.energy)
            {
                if (type == Constants.Map_rocks)
                {
                    if (storageinfo[type].currentcapability < storageinfo[type].maxcapability * perk.storagebonus * perk.optimizestorage)
                    {
                        storageinfo[type].currentcapability += count;
                        inventory[resource].count += count;
                    }
                }
                else
                {
                    if (storageinfo[type].currentcapability < storageinfo[type].maxcapability * perk.storagebonus)
                    {
                        storageinfo[type].currentcapability += count;
                        inventory[resource].count += count;
                    }
                }
            }
        }

        public void Update(float ellapsedtime, float totaltime)
        {
            Random rand = new Random();

            //Set building info
            BuildingInfo[] buildinginfo = new BuildingInfo[Building.TotalBuildings + 1];
            for (int i = 0; i < Building.TotalBuildings + 1; i++) buildinginfo[i].ids = new List<int>();
            for (int i = 0; i < buildings.Count; i++)
            {
                Building b = buildings[i];
                buildinginfo[b.type].count++;
                if (b.power > 0 && b.buildingtime <= 0)
                {
                    int type = Building.GetType(b.type);
                    buildinginfo[b.type].workingpower += buildingtypeinfo[type].maxpower;//type < 7 ? maxpowerofbuilding[type] : 1 * b.power;
                    buildinginfo[b.type].workincount++;
                    if (b.waitwithwork <= 0)
                        buildinginfo[b.type].ids.Add(i);
                }
            }

            for (int i = 0; i < maxbuildingtype; i++)
            {
                buildingtypeinfo[i].havepower = 0;
                buildingtypeinfo[i].consumepower = 0;
            }

            //Get temperature on base's height
            float fy = Math.Abs((position.Y - planet.mapheight / 2) / (float)(planet.mapheight / 2));
            float temperature = (float)(planet.maxtemperature + planet.temperatureoffcet - (planet.maxtemperature - planet.mintemperature) * fy);

            //Destroy ALL!!!
            bool iscommandcenter = buildinginfo[Building.CommandCenter].workincount > 0;
            if (iscommandcenter)
                timetodestroy = Constants.Map_timetodestroybase;
            timetodestroy -= ellapsedtime;

            //All human - scientists
            int workhumans = population;
            float sciencepower = 0; //-----------------------

            //Update storages empty places
            storageinfo[0].currentcapability = 0; 
            storageinfo[2].currentcapability = 0; 
            storageinfo[3].currentcapability = 0; 
            storageinfo[4].currentcapability = 0;

            for (int i = 0; i < maxresources; i++)
            {
                inventory[i].buildingproduct = 0;
                storageinfo[GetResourceType(i)].currentcapability += inventory[i].count;
            }

            List<int> fightersids = new List<int>();

            #region Unit update
            List<int> builderunitsid = new List<int>();
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].type == Unit.Pirate) fightersids.Add(i);
                if (units[i].type == Unit.AttackDrone && player_id != units[i].player_id) fightersids.Add(i);
                if (units[i].wait > 0) units[i].wait -= ellapsedtime;

                #region Move
                else if (units[i].pos != units[i].tar)
                {
                    Vector2 dir;
                    if (units[i].waypoints.Count > 0)
                        dir = units[i].waypoints[0] - units[i].pos;
                    else dir = units[i].tar - units[i].pos;
                    dir.Normalize();

                    units[i].pos += dir * units[i].speed * ellapsedtime * perk.movementbonus * perk.unitspeedup;

                    if (units[i].waypoints.Count > 0)
                    { 
                        if ((units[i].waypoints[0] - units[i].pos).LengthSquared() < 0.03f) 
                        { 
                            units[i].pos = units[i].waypoints[0]; 
                            units[i].waypoints.RemoveAt(0); 
                        } 
                    }
                    else if ((units[i].tar - units[i].pos).LengthSquared() < 0.03f) units[i].pos = units[i].tar;

                    #region Calculate direction
                    float cos = dir.X;
                    float sin = dir.Y;

                    float cos7 = (float)Math.Cos(7 * Math.PI / 8), cos5 = (float)Math.Cos(5 * Math.PI / 8), cos3 = (float)Math.Cos(3 * Math.PI / 8), cos1 = (float)Math.Cos(Math.PI / 8);

                    if (cos > cos7)
                    {
                        if (cos > cos5)
                        {
                            if (cos > cos3)
                            {
                                if (cos > cos1)
                                {
                                    units[i].direction = 2;
                                }
                                else
                                {
                                    units[i].direction = sin < 0 ? 3 : 1;
                                }
                            }
                            else
                            {
                                units[i].direction = sin < 0 ? 4 : 0;
                            }
                        }
                        else
                        {
                            units[i].direction = sin < 0 ? 5 : 7;
                        }
                    }
                    else
                    {
                        units[i].direction = 6;
                    }
                    #endregion
                }
                #endregion
                #region Process
                else
                {
                    // free drone
                    if (units[i].command.message == commands.patrol)
                    {
                        if (buildinginfo[Building.Beacon].ids.Count > 0)
                        {
                            units[i].waypoints = GetWayToBuilding(units[i].pos, buildings[buildinginfo[Building.Beacon].ids[rand.Next(buildinginfo[Building.Beacon].ids.Count)]].pos);
                            if (units[i].waypoints == null)
                            {
                                do
                                {
                                    units[i].tar = new Vector2(rand.Next(width), rand.Next(height));
                                } while (!data[(int)units[i].tar.Y, (int)units[i].tar.X].can_move);
                                units[i].waypoints = GetWay(units[i].pos, units[i].tar);
                            }
                            units[i].tar = units[i].waypoints[units[i].waypoints.Count - 1];
                            units[i].command.message = commands.patroltobeacon;
                        }
                        else
                        {
                            do
                            {
                                units[i].tar = new Vector2(rand.Next(width), rand.Next(height));
                            } while (!data[(int)units[i].tar.Y, (int)units[i].tar.X].can_move);
                            units[i].waypoints = GetWay(units[i].pos, units[i].tar);
                        }
                    }
                    else if (units[i].command.message == commands.repair)
                    {
                        int x = units[i].command.x;
                        int y = units[i].command.y;
                        if (data[y, x].build_id >= 0 && data[y, x].build_id < buildings.Count)
                        {
                            buildings[data[y, x].build_id].health += ellapsedtime * 10;

                            if (buildings[data[y, x].build_id].health >= Building.GetSource(buildings[data[y, x].build_id].type).Width)
                            {
                                units[i].command.message = commands.patrol;
                                buildings[data[y, x].build_id].health = Building.GetSource(buildings[data[y, x].build_id].type).Width;
                            }
                        }
                        else units[i].command.message = commands.patrol;
                    }
                    //drone to beacon
                    else if (units[i].command.message == commands.patroltobeacon)
                    {
                        units[i].tar = units[i].pos;
                        units[i].command.message = commands.patrol;
                        units[i].wait = Constants.Map_unitbeaconwait;
                        units[i].maxwait = units[i].wait;
                    }
                    //attackdrone find parking
                    else if (units[i].command.message == commands.gotoparking)
                    {
                        if (buildinginfo[Building.AttackParking].ids.Count > 0)
                        {
                            units[i].tar = buildings[buildinginfo[Building.AttackParking].ids[0]].pos;
                            units[i].wait = Constants.Map_unitbeaconwait;
                            units[i].maxwait = units[i].wait;
                            buildings[buildinginfo[Building.AttackParking].ids[0]].waitwithwork = units[i].wait;
                            buildinginfo[Building.AttackParking].ids.RemoveAt(0);
                        }
                        else
                        {
                            units[i].tar = new Vector2(rand.Next(width - 4) + 2, rand.Next(height - 4) + 2);
                            units[i].wait = Constants.Map_unitbeaconwait;
                            units[i].maxwait = units[i].wait;
                        }
                    }
                    else if (units[i].command.message == commands.goaway)
                    {
                        units.RemoveAt(i);
                    }
                    else if (units[i].command.message == commands.attackmode)
                    {
                        if (!onMap((int)units[i].pos.X, (int)units[i].pos.Y))
                        {
                            units[i].tar = new Vector2(rand.Next(width), rand.Next(height));
                        }
                        else
                        {
                            int b_id = data[(int)units[i].pos.Y, (int)units[i].pos.X].build_id;
                            if (b_id < buildings.Count && b_id >= 0)
                            {
                                buildings[b_id].health--;
                                buildings[b_id].power = 0;
                            }
                            if (units[i].inventory == null)
                                units[i].inventory = new float[1];
                            units[i].inventory[0]++;
                            units[i].tar = buildings[rand.Next(buildings.Count)].pos;
                            if (units[i].inventory[0] >= 4)
                            {
                                Vector2 pos = GetRandomBaseDirection();
                                Unit u = new Unit(Unit.AttackDrone, pos.X, pos.Y);
                                u.command = new Command(commands.gotoparking, 0);
                                u.tar = u.pos;
                                UnitGroup ug = new UnitGroup();
                                ug.baseposiitionx = units[i].motherbasex;
                                ug.baseposiitiony = units[i].motherbasey;
                                ug.player_id = units[i].player_id;
                                ug.units.Add(u);
                                ug.position = position;
                                planet.unitgroups.Add(ug);

                                units[i].command = new Command(commands.goaway, 0);
                                units[i].tar = pos;
                            }
                        }
                    }
                    //builer drone
                    else if (units[i].command.message == commands.moveandbuild)
                    {
                        //if building was not destroyed
                        if (data[units[i].command.y, units[i].command.x].build_id >= 0)
                        {
                            units[i].command.id = data[units[i].command.y, units[i].command.x].build_id;
                            if (buildings[units[i].command.id].buildingtime > 0)
                            {
                                buildings[units[i].command.id].isbuildingnow = true;
                                buildings[units[i].command.id].buildingtime -= (ellapsedtime * perk.buildbonus * perk.buildingspeedup) < buildings[units[i].command.id].buildingtime ? (ellapsedtime * perk.buildbonus * perk.buildingspeedup) : buildings[units[i].command.id].buildingtime;
                            }
                            else
                            {
                                units[i].command.message = commands.patrol;
                                buildings[units[i].command.id].power = 1;
                                buildings[units[i].command.id].starttime = totaltime;
                                buildings[units[i].command.id].wait = 0;

                                AddMessage(Language.Message_BuildingCreated + " " + Building.GetName(buildings[units[i].command.id].type));

                                if (buildings[units[i].command.id].type == Building.Warehouse)
                                    storageinfo[Constants.Map_rocks].maxcapability += Constants.Map_warehoucestoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.InfoStorage)
                                    storageinfo[Constants.Map_science].maxcapability += Constants.Map_infostoragestoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.House)
                                    storageinfo[Constants.Map_humans].maxcapability += Constants.Map_housestoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.EnergyBank)
                                    storageinfo[Constants.Map_energy].maxcapability += Constants.Map_energybankstoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.LuquidStorage)
                                    storageinfo[Constants.Map_luquids].maxcapability += Constants.Map_reservourstoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.Turret)
                                    buildings[i].wait = 9.3f;
                                buildings[units[i].command.id].oldheight = buildings[units[i].command.id].height;
                            }
                        }
                        else units[i].command.message = commands.patrol;
                    }
                    //pirat
                    else if (units[i].command.message == commands.piraterob)
                    {
                        int id = data[units[i].command.y, units[i].command.x].build_id;
                        buildings[id].power = 0;
                        buildings[id].health -= 10;

                        for (int q = 0; q < maxresources; q++)
                        {
                            if (rand.Next(3) == 0)
                                inventory[q].count -= inventory[q].count * (float)(0.01f + 0.04f * rand.NextDouble());
                        }

                        units[i].wait = 2;
                        units[i].maxwait = units[i].wait;
                        Vector2 pos = GetRandomBaseDirection();
                        units[i].command = new Command(commands.pirateaway, (int)pos.X, (int)pos.Y);
                        units[i].tar = pos;
                        units[i].waypoints.Clear();
                    }
                    else if (units[i].command.message == commands.pirateaway)
                    {
                        units.RemoveAt(i);
                        fightersids.Remove(i);
                    }
                    //rockets
                    else if (units[i].command.message == commands.rocketflyup)
                    {
                        units[i].height += ellapsedtime * 15;
                        if (units[i].height >= units[i].command.id) units.RemoveAt(i);
                    }
                    else if (units[i].command.message == commands.rocketfallingdown)
                    {
                        units[i].height -= ellapsedtime * 60;
                        if (units[i].height <= units[i].command.id)
                        {
                            AddMessage(Language.Message_Missile);
                            int type = units[i].type;
                            int rx = (int)units[i].pos.X;
                            int ry = (int)units[i].pos.Y;
                            int destroyarea = type == Unit.RocketAtom ? 5 : (type == Unit.RocketTwined ? 2 : 0);
                            int flasharea = type == Unit.RocketNeitron ? 7 : (type == Unit.RocketTwined ? 4 : 0);

                            for (int x = -destroyarea; x <= destroyarea; x++)
                                for (int y = -destroyarea; y <= destroyarea; y++)
                                {
                                    double l = Math.Sqrt(x * x + y * y);
                                    if (l <= destroyarea)
                                    {
                                        if (onMap(rx + x, ry + y) && data[ry + y, rx + x].build_id >= 0 && !onShield(rx + x, ry + y, Shield.Power))
                                            DestroyBuilding(rx + x, ry + y);
                                    }
                                }
                            for (int x = -flasharea; x <= flasharea; x++)
                                for (int y = -flasharea; y <= flasharea; y++)
                                {
                                    double l = Math.Sqrt(x * x + y * y);
                                    if (l <= flasharea)
                                    {
                                        if (onMap(rx + x, ry + y) && data[ry + y, rx + x].build_id >= 0 && !onShield(rx + x, ry + y, Shield.Emmision) && data[ry + y, rx + x].build_id < buildings.Count)
                                            buildings[data[ry + y, rx + x].build_id].power = 0;
                                    }
                                }

                            meteorites.Add(new Meteorite(units[i].pos, 4, 0));
                            units.RemoveAt(i);
                        }
                    }
                    #region Trader
                    else if (units[i].command.message == commands.tradergoin)
                        units[i].command = new Command(commands.tradegodown, 0);
                    else if (units[i].command.message == commands.tradergoaway)
                    {

                        UnitGroup ug = new UnitGroup();
                        ug.planetid_target = -1;
                        ug.planetid_mother = planet.id;
                        ug.position = planet.GetPosition(planet.id);
                        ug.player_id = units[i].player_id;
                        planet.star.unitgroups.Add(ug);
                        units.RemoveAt(i);
                    }
                    else if (units[i].command.message == commands.tradegodown)
                    {
                        units[i].height -= ellapsedtime;
                        if (units[i].height <= units[i].command.id)
                        {
                            units[i].height = units[i].command.id;
                            bool ok = false;
                            for (int k = 0; k < maxresources; k++)
                            {
                                //buy
                                if (inventory[k].exchangetype == 1 && (int)inventory[k].count < inventory[k].exchangecount)
                                {
                                    float count = Math.Min(inventory[k].exchangecount - inventory[k].count, Constants.Unit_tradervalue);
                                    float price = Math.Min(Constants.GetPriceOfResource(k) * count, inventory[(int)Resources.credits].count);
                                    count = price / Constants.GetPriceOfResource(k);

                                    inventory[(int)Resources.credits].count -= count;
                                    TryAddResource(k, count, iscommandcenter);
                                    ok = true;
                                }
                                //sell
                                if (inventory[k].exchangetype == 2 && (int)inventory[k].count > inventory[k].exchangecount)
                                {
                                    float count = Math.Min(inventory[k].count - inventory[k].exchangecount, Constants.Unit_tradervalue);
                                    inventory[k].count -= count;
                                    inventory[(int)Resources.credits].count += count * Constants.GetPriceOfResource(k) * 0.8f;
                                    ok = true;
                                }
                            }
                            units[i].command = new Command(commands.tradegoup, Constants.Unit_traderheight);
                            units[i].wait = Constants.Unit_traderwait;
                            units[i].maxwait = units[i].wait;

                            if (ok)
                                AddMessage(Language.Message_MerchantWork);
                        }
                    }
                    else if (units[i].command.message == commands.tradegoup)
                    {
                        if (data[(int)units[i].tar.Y, (int)units[i].tar.X].build_id >= 0)
                            buildings[data[(int)units[i].tar.Y, (int)units[i].tar.X].build_id].wait = 0;
                        units[i].height += ellapsedtime;
                        if (units[i].height >= units[i].command.id)
                        {
                            units[i].height = units[i].command.id;
                            Vector2 pos = GetRandomBaseDirection();
                            units[i].command = new Command(commands.tradergoaway, (int)pos.X, (int)pos.Y);
                            units[i].tar = pos;
                            units[i].waypoints.Clear();
                        }
                    }
                    #endregion
                    #region Humanship
                    else if (units[i].command.message == commands.humangoin)
                        units[i].command = new Command(commands.humangodown, 0);
                    else if (units[i].command.message == commands.humangoaway)
                        units.RemoveAt(i);
                    else if (units[i].command.message == commands.humangodown)
                    {
                        units[i].height -= ellapsedtime;
                        if (units[i].height <= units[i].command.id)
                        {
                            units[i].height = units[i].command.id;
                            int newhums = Math.Min(rand.Next(Constants.Map_humannewrand) + Constants.Map_humannew, (int)(storageinfo[Constants.Map_humans].maxcapability - storageinfo[Constants.Map_humans].currentcapability));
                            if (inventory[(int)Resources.meat].count + inventory[(int)Resources.fruits].count + inventory[(int)Resources.vegetables].count + inventory[(int)Resources.fish].count > 0 && inventory[(int)Resources.water].count > 0)
                            {
                                AddMessage(Language.Message_PopulationUp);
                                population += newhums;
                                storageinfo[Constants.Map_humans].currentcapability += newhums;
                            }
                            else
                            {
                                AddMessage(Language.Message_PopulationDown);
                                population -= newhums / 2;
                                if (population < 0) population = 0;
                                storageinfo[Constants.Map_humans].currentcapability = population;
                            }
                            units[i].command = new Command(commands.humangoup, Constants.Unit_humanheight);
                            units[i].wait = Constants.Unit_humanwait;
                            units[i].maxwait = units[i].wait;
                        }
                    }
                    else if (units[i].command.message == commands.humangoup)
                    {
                        buildings[data[(int)units[i].tar.Y, (int)units[i].tar.X].build_id].wait = 0;
                        units[i].height += ellapsedtime;
                        if (units[i].height >= units[i].command.id)
                        {
                            units[i].height = units[i].command.id;
                            units[i].command = new Command(commands.humangoaway, -1, -1);
                            units[i].tar = new Vector2(-1, -1);
                            units[i].waypoints.Clear();
                        }
                    }
                    #endregion
                    #region LocalTrader
                    else if (units[i].command.message == commands.localtradergoin)
                        units[i].command = new Command(commands.localtradegodown, 0);
                    else if (units[i].command.message == commands.localtradegodown)
                    {
                        units[i].height -= ellapsedtime;
                        if (units[i].height <= units[i].command.id)
                        {
                            units[i].height = units[i].command.id;
                            for (int k = 0; k < maxresources; k++)
                            {
                                if (units[i].inventory[k] > 0)
                                    TryAddResource(k, units[i].inventory[k], iscommandcenter);
                            }
                            units.RemoveAt(i);
                        }
                    }
                    #endregion
                }
                #endregion

                if (i < units.Count && (units[i].command.message == commands.patrol || units[i].command.message == commands.patroltobeacon))
                    builderunitsid.Add(i);
            }
            #endregion

            #region Builds update
            List<int> buildngsthatneeddestroyed = new List<int>();
            energyproduction = 0;

            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].health <= 0 && !buildngsthatneeddestroyed.Contains(i)) buildngsthatneeddestroyed.Add(i);
                if (buildings[i].wait > 0)
                {
                    buildings[i].wait -= ellapsedtime;
                    if (buildings[i].wait < 0)
                        buildings[i].wait = 0;
                }
                else
                {
                    #region Need Build?
                    if (buildings[i].buildingtime > 0)
                    {
                        //if (buildings[i].isbuildingnow) buildings[i].isbuildingnow = false;
                        if (!buildings[i].isbuildingnow)
                        {
                            //found new builder
                            if (builderunitsid.Count > 0)
                            {
                                int j = builderunitsid[0];
                                builderunitsid.RemoveAt(0);
                                units[j].command = new Command(commands.moveandbuild, i);
                                units[j].command.y = (int)buildings[i].pos.Y;
                                units[j].command.x = (int)buildings[i].pos.X;
                                units[j].waypoints = GetWayToBuilding(units[j].pos, buildings[i].pos);
                                if (units[j].waypoints == null)
                                {
                                    do
                                    {
                                        units[j].tar = new Vector2(rand.Next(width), rand.Next(height));
                                    } while (!data[(int)units[j].tar.Y, (int)units[j].tar.X].can_move);
                                    units[j].waypoints = GetWay(units[j].pos, units[j].tar);
                                    buildngsthatneeddestroyed.Add(i);
                                    break;
                                }
                                units[j].tar = units[j].waypoints[units[j].waypoints.Count - 1];
                                buildings[i].wait = Constants.Map_buildingwait;
                                break;
                            }
                        }
                    }
                    #endregion
                    #region No,thank
                    else
                    {
                        if (buildings[i].waitwithwork > 0)
                        {
                            buildings[i].waitwithwork -= ellapsedtime;
                            if (buildings[i].waitwithwork < 0)
                                buildings[i].waitwithwork = 0;
                        }
                        if (buildings[i].health < Building.GetSource(buildings[i].type).Width)
                        {
                            //found new builder
                            if (builderunitsid.Count > 0)
                            {
                                int j = builderunitsid[0];
                                builderunitsid.RemoveAt(0);
                                units[j].command = new Command(commands.repair, i);
                                units[j].command.y = (int)buildings[i].pos.Y;
                                units[j].command.x = (int)buildings[i].pos.X;
                                units[j].waypoints = GetWayToBuilding(units[j].pos, buildings[i].pos);
                                if (units[j].waypoints == null)
                                {
                                    do
                                    {
                                        units[j].tar = new Vector2(rand.Next(width), rand.Next(height));
                                    } while (!data[(int)units[j].tar.Y, (int)units[j].tar.X].can_move);
                                    units[j].waypoints = GetWay(units[j].pos, units[j].tar);
                                    buildngsthatneeddestroyed.Add(i);
                                    break;
                                }
                                units[j].tar = units[j].waypoints[units[j].waypoints.Count - 1];
                                buildings[i].waitwithwork = Constants.Map_buildingwait;
                                break;
                            }
                        }

                        int thisbuildingtype = Building.GetType(buildings[i].type);
                        Point thisbuildingsize = Building.GetSize(buildings[i].type);
                        int thisbuildingpos_x = (int)buildings[i].pos.X;
                        int thisbuildingpos_y = (int)buildings[i].pos.Y;
                        int temp;

                        #region Process
                        switch (buildings[i].type)
                        {
                            case Building.Generator:
                                int multiplier = 1;
                                if (energyboost && inventory[(int)Resources.energyore].count > 0)
                                {
                                    inventory[(int)Resources.energyore].count -= Constants.Map_energyoreconsuming * ellapsedtime;
                                    if (inventory[(int)Resources.energyore].count < 0) inventory[(int)Resources.energyore].count = 0;
                                    multiplier = 3;
                                }
                                energyproduction += multiplier * buildings[i].power * Constants.Map_generatorproduce * perk.optimizeerergyproduce;
                                TryAddResource((int)Resources.energy, multiplier * buildings[i].power * Constants.Map_generatorproduce * ellapsedtime * perk.optimizeerergyproduce,iscommandcenter);
                                inventory[(int)Resources.energy].buildingproduct++;
                                break;
                            case Building.Mine:
                                for (int k = 0; k < thisbuildingsize.X; k++)
                                    for (int j = 0; j < thisbuildingsize.Y; j++)
                                    {
                                        if (data[thisbuildingpos_y - k, thisbuildingpos_x - j].mineresource_id > 0)
                                            TryAddResource(data[thisbuildingpos_y - k, thisbuildingpos_x - j].mineresource_id, buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower * ellapsedtime, iscommandcenter);
                                        else TryAddResource((int)Resources.rock, buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower / 3 * ellapsedtime, iscommandcenter);
                                    }
                                break;
                            case Building.Dirrick: if (data[thisbuildingpos_y, thisbuildingpos_x].dirrickresource_id > 0)
                                    TryAddResource(data[thisbuildingpos_y, thisbuildingpos_x].dirrickresource_id, buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower * ellapsedtime, iscommandcenter);
                                break;
                            case Building.ProcessingFactory:
                                bool work = true;
                                float thisbuildingpow = buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower;
                                //if have all components
                                for (int k = 0; k < MapHelper_Engine.resiptes[buildings[i].recipte].incount.Length; k++)
                                {
                                    if (inventory[MapHelper_Engine.resiptes[buildings[i].recipte].inresourses[k]].count < MapHelper_Engine.resiptes[buildings[i].recipte].incount[k] * ellapsedtime * thisbuildingpow)
                                        work = false;
                                }
                                //continue
                                if (work)
                                {
                                    for (int k = 0; k < MapHelper_Engine.resiptes[buildings[i].recipte].incount.Length; k++)
                                    {
                                        float deletecount = MapHelper_Engine.resiptes[buildings[i].recipte].incount[k] * ellapsedtime * thisbuildingpow;
                                        inventory[MapHelper_Engine.resiptes[buildings[i].recipte].inresourses[k]].count -= deletecount;
                                        storageinfo[GetResourceType(i)].currentcapability -= deletecount;
                                    }
                                    float outres = MapHelper_Engine.resiptes[buildings[i].recipte].outcount * thisbuildingpow * ellapsedtime;
                                    if (MapHelper_Engine.resiptes[buildings[i].recipte].outresource == (int)Resources.metal) outres *= perk.optimizeore;

                                    TryAddResource(MapHelper_Engine.resiptes[buildings[i].recipte].outresource, outres, iscommandcenter);
                                    inventory[MapHelper_Engine.resiptes[buildings[i].recipte].outresource].buildingproduct++;
                                }
                                break;
                            case Building.DroidFactory:
                                if (buildings[i].worktime > 0)
                                {
                                    buildings[i].worktime -= ellapsedtime * buildingtypeinfo[thisbuildingtype].maxpower;
                                    if (buildings[i].worktime <= 0) AddDrone(buildings[i].pos.X + 0.5f, buildings[i].pos.Y + 0.5f);
                                }
                                if (buildings[i].worktime <= 0 && buildings[i].workcount > 0 && inventory[(int)Resources.metal].count >= Constants.Unit_dronemetalprice && inventory[(int)Resources.electronics].count >= Constants.Unit_droneelectronicprice)
                                {
                                    inventory[(int)Resources.metal].count -= Constants.Unit_dronemetalprice;
                                    inventory[(int)Resources.electronics].count -= Constants.Unit_droneelectronicprice;
                                    buildings[i].workcount--;
                                    buildings[i].worktime = Constants.Map_dronefactoryworktime;

                                    AddMessage(Language.Message_DroneCreated);
                                }
                                break;
                            case Building.Collector:
                                thisbuildingpow = buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower;
                                TryAddResource((int)Resources.animals + buildings[i].recipte, thisbuildingpow * ellapsedtime * 4, iscommandcenter);
                                break;
                            case Building.Laboratory:
                                thisbuildingpow = buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower;

                                int workhuminthislab = Math.Min(workhumans, Constants.Map_laboratoryworkinghuman);
                                float price = Math.Min(workhuminthislab * Constants.Map_workinghumanprice * thisbuildingpow, inventory[(int)Resources.credits].count);
                                if (price < 0) price = 0;
                                workhuminthislab = (int)(price / Constants.Map_workinghumanprice);
                                price = workhuminthislab * Constants.Map_workinghumanprice * thisbuildingpow;
                                if (price < 0) price = 0;

                                float scturbo = scienceboost ? inventory[(int)Resources.animals].count + inventory[(int)Resources.plants].count : 0;
                                //sciencelvl += workhuminthislab * perk.sciencebonus * perk.optimizeresearch * 4 * (scturbo > 0 ? 2 : 1);
                                if (scturbo > 0)
                                {
                                    inventory[(int)Resources.animals].count -= (inventory[(int)Resources.animals].count / scturbo) * 2 * ellapsedtime;
                                    inventory[(int)Resources.plants].count -= (inventory[(int)Resources.plants].count / scturbo) * 2 * ellapsedtime;
                                }

                                //normal research
                                if (currentresearchmode==0)
                                {
                                    if (science[currentresearchmode].items[selectedresearch[currentresearchmode]].time > 0)
                                    {
                                        inventory[(int)Resources.credits].count -= price;
                                        workhumans -= workhuminthislab;
                                        science[currentresearchmode].items[selectedresearch[currentresearchmode]].time -= ellapsedtime * workhuminthislab * perk.sciencebonus * perk.optimizeresearch / Constants.Map_laboratorymaxworkinghuman / 3 * (scturbo > 0 ? 2 : 1);
                                        if (science[currentresearchmode].items[selectedresearch[currentresearchmode]].time <= 0)
                                        {
                                            AddMessage(Language.Message_ResearchFinished + " " + science[currentresearchmode].items[selectedresearch[currentresearchmode]].name);

                                            science[currentresearchmode].items[selectedresearch[currentresearchmode]].time = 0;
                                            science[currentresearchmode].items[selectedresearch[currentresearchmode]].searched = true;
                                            switch (science[currentresearchmode].items[selectedresearch[currentresearchmode]].id)
                                            {
                                                case Constants.Research_buildingspeedup: perk.buildingspeedup += 0.2f; break;
                                                case Constants.Research_optimizeerergyproduce: perk.optimizeerergyproduce += 0.2f; break;
                                                case Constants.Research_optimizeore: perk.optimizeore += 0.2f; break;
                                                case Constants.Research_optimizeresearch: perk.optimizeresearch += 0.2f; break;
                                                case Constants.Research_optimizestorage: perk.optimizestorage += 0.2f; break;
                                                case Constants.Research_optimizingshield: perk.optimizingshield += 0.2f; break;
                                                case Constants.Research_otimizeenergyconsume: perk.otimizeenergyconsume += 0.2f; break;
                                                case Constants.Research_unitspeedup: perk.unitspeedup += 0.2f; break;
                                            }
                                        }
                                    }
                                }
                                //planetary research
                                if (currentresearchmode==1)
                                {
                                    if (science[currentresearchmode].items[selectedresearch[currentresearchmode]].time > 0)
                                    {
                                        inventory[(int)Resources.credits].count -= price;
                                        workhumans -= workhuminthislab;
                                        science[currentresearchmode].items[selectedresearch[currentresearchmode]].time -= ellapsedtime * workhuminthislab * perk.sciencebonus * perk.optimizeresearch / Constants.Map_laboratorymaxworkinghuman / 3 * (scturbo > 0 ? 2 : 1);
                                        science[currentresearchmode].items[selectedresearch[currentresearchmode]].searched = false;
                                        if (science[currentresearchmode].items[selectedresearch[currentresearchmode]].time <= 0)
                                        {
                                            AddMessage(Language.Message_ResearchFinished + " " + science[currentresearchmode].items[selectedresearch[currentresearchmode]].name);

                                            science[currentresearchmode].items[selectedresearch[currentresearchmode]].time = 0;
                                            science[currentresearchmode].items[selectedresearch[currentresearchmode]].searched = true;
                                        }
                                    }
                                    else
                                    {
                                        science[currentresearchmode].items[selectedresearch[currentresearchmode]].searched = true;
                                        inventory[(int)Resources.credits].count -= price;
                                        workhumans -= workhuminthislab;

                                        sciencepower = ellapsedtime * workhuminthislab * perk.sciencebonus * perk.optimizeresearch / Constants.Map_laboratorymaxworkinghuman / 3;
                                    }
                                }
                                break;
                            case Building.Farm:
                                thisbuildingpow = buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower;
                                if (onShield(thisbuildingpos_x, thisbuildingpos_y, Shield.Atmosphere) || (temperature > 10 && temperature < 40))
                                    TryAddResource((int)Resources.meat + buildings[i].recipte, thisbuildingpow * ellapsedtime, iscommandcenter);
                                break;
                            case Building.ClosedFarm:
                                thisbuildingpow = buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower;
                                TryAddResource((int)Resources.meat + buildings[i].recipte, thisbuildingpow * ellapsedtime, iscommandcenter);
                                break;
                            case Building.PowerShield:
                                if (buildings[i].shieldid < 0)
                                {
                                    shields.Add(new Shield(Shield.Power, buildings[i].height * 4, buildings[i].pos + new Vector2(0.5f, 0.5f), totaltime));
                                    buildings[i].shieldid = shields.Count - 1;
                                }
                                shields[buildings[i].shieldid].size = buildings[i].height * 2 * buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower * perk.optimizingshield;
                                break;
                            case Building.AtmosophereShield:
                                if (buildings[i].shieldid < 0)
                                {
                                    shields.Add(new Shield(Shield.Atmosphere, buildings[i].height * 4, buildings[i].pos + new Vector2(0.5f, 0.5f), totaltime));
                                    buildings[i].shieldid = shields.Count - 1;
                                }
                                shields[buildings[i].shieldid].size = buildings[i].height * 2 * buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower * perk.optimizingshield;
                                break;
                            case Building.EmmisionShield:
                                if (buildings[i].shieldid < 0)
                                {
                                    shields.Add(new Shield(Shield.Emmision, buildings[i].height * 4, buildings[i].pos + new Vector2(0.5f, 0.5f), totaltime));
                                    buildings[i].shieldid = shields.Count - 1;
                                }
                                shields[buildings[i].shieldid].size = buildings[i].height * 2 * buildings[i].power * buildingtypeinfo[thisbuildingtype].maxpower * perk.optimizingshield;
                                break;
                            case Building.Turret:
                                buildings[i].wait = 0.33f;
                                foreach (int id in fightersids)
                                {
                                    if (id < units.Count)
                                    {
                                        temp = (int)(units[id].pos - buildings[i].pos).Length();
                                        if (units[id].command.message == commands.piraterob && temp < 8)
                                        {
                                            if (units[id].type == Unit.Pirate)
                                            {
                                                Vector2 pos = GetRandomBaseDirection();
                                                units[id].command = new Command(commands.pirateaway, (int)pos.X, (int)pos.Y);
                                                units[id].tar = pos;
                                                units[id].waypoints.Clear();

                                                buildings[i].wait = 4.3f;
                                                break;
                                            }
                                            else if (units[id].type == Unit.AttackDrone)
                                            {
                                                Vector2 pos = GetRandomBaseDirection();
                                                Unit u = new Unit(Unit.AttackDrone, pos.X, pos.Y);
                                                u.command = new Command(commands.gotoparking, 0);
                                                u.tar = u.pos;
                                                UnitGroup ug = new UnitGroup();
                                                ug.baseposiitionx = units[i].motherbasex;
                                                ug.baseposiitiony = units[i].motherbasey;
                                                ug.player_id = units[i].player_id;
                                                ug.units.Add(u);
                                                ug.position = position;
                                                planet.unitgroups.Add(ug);

                                                units[i].command = new Command(commands.goaway, 0);
                                                units[i].tar = pos;
                                                units[i].waypoints.Clear();

                                                buildings[i].wait = 4.3f;
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case Building.AttackFactory:
                                if (buildings[i].worktime > 0)
                                {
                                    buildings[i].worktime -= ellapsedtime * buildingtypeinfo[thisbuildingtype].maxpower;
                                    if (buildings[i].worktime <= 0)
                                    {
                                        AddMessage(Language.Message_FighterCreated);
                                        AddAttackDrone(buildings[i].pos.X + 0.5f, buildings[i].pos.Y + 0.5f);
                                    }
                                }
                                if (buildings[i].worktime <= 0 && buildings[i].workcount > 0 && inventory[(int)Resources.metal].count >= Constants.Unit_attackdronemetalprice && inventory[(int)Resources.electronics].count >= Constants.Unit_attackdroneelectronicprice)
                                {
                                    inventory[(int)Resources.metal].count -= Constants.Unit_attackdronemetalprice;
                                    inventory[(int)Resources.electronics].count -= Constants.Unit_attackdroneelectronicprice;
                                    buildings[i].workcount--;
                                    buildings[i].worktime = Constants.Map_attackdronefactoryworktime;
                                }
                                break;
                        }
                        #endregion

                        float resources = Building.GetEnergy(buildings[i].type) * buildings[i].power / perk.otimizeenergyconsume;
                        if (thisbuildingtype < 7)
                        {
                            resources *= buildingtypeinfo[thisbuildingtype].maxpower;
                            buildingtypeinfo[thisbuildingtype].consumepower += resources;
                        }

                        if (resources > inventory[(int)Resources.energy].count)
                        {
                            buildings[i].power = 0;
                            //ms.AddMessage(MessageType.buildingoff, baseid, "Нехватка энергии: здание выключено", 1, Color.White);
                        }
                        else
                        {
                            if (thisbuildingtype < 7)
                                buildingtypeinfo[thisbuildingtype].havepower += resources;
                            inventory[(int)Resources.energy].count -= resources * ellapsedtime;
                        }
                    }
                    #endregion
                }
            }
            for (int i = buildngsthatneeddestroyed.Count - 1; i >= 0; i--)
            {
                int id = buildngsthatneeddestroyed[i];
                DestroyBuilding((int)buildings[id].pos.X, (int)buildings[id].pos.Y);
                //ms.AddMessage(MessageType.none, baseid, "Не найден путь к зданию. Здание уничтожено.", 3, Color.White);
            }
            #endregion

            for (int i = 0; i < maxresources; i++)
            {
                if (inventory[i].count < 0) inventory[i].count = 0;
            }

            //humans like eat
            if (population > 0)
            {
                float food = inventory[(int)Resources.meat].count + inventory[(int)Resources.fruits].count + inventory[(int)Resources.vegetables].count + inventory[(int)Resources.fish].count;
                float eat = 0.05f;
                float water = inventory[(int)Resources.water].count;
                if (food > 0 && water > 0)
                {
                    inventory[(int)Resources.meat].count -= Math.Min(population * eat * inventory[(int)Resources.meat].count / food * ellapsedtime, inventory[(int)Resources.meat].count);
                    inventory[(int)Resources.fruits].count -= Math.Min(population * eat * inventory[(int)Resources.fruits].count / food * ellapsedtime, inventory[(int)Resources.fruits].count);
                    inventory[(int)Resources.vegetables].count -= Math.Min(population * eat * inventory[(int)Resources.vegetables].count / food * ellapsedtime, inventory[(int)Resources.vegetables].count);
                    inventory[(int)Resources.fish].count -= Math.Min(population * eat * inventory[(int)Resources.fish].count / food * ellapsedtime, inventory[(int)Resources.fish].count);

                    inventory[(int)Resources.water].count -= Math.Min(population * eat * ellapsedtime, inventory[(int)Resources.water].count);

                    TryAddResource((int)Resources.biowaste, population * eat * ellapsedtime, iscommandcenter);
                    timetohunger = Constants.Map_hungerstarttime;
                }
                else
                {
                    //string text = "Колония голодает: ";
                    //if (food > 0 && water > 0) text += "нехватка еды и воды";
                    //else if (food > 0) text += "нехватка воды";
                    //else text += "нехватка еды";
                    //ms.AddMessage(MessageType.hunger, baseid, text, 1, Color.White);
                    timetohunger -= ellapsedtime;
                    if (timetohunger < 0)
                    {
                        timetohunger = Constants.Map_hungernexttime;
                        population--;
                        storageinfo[Constants.Map_humans].currentcapability--;

                        AddMessage(Language.Message_ColonyHungry);
                    }
                }
            }

            if (currentresearchmode == 1)
            {
                PlanetModule module = null;

                for (int i = 0; i < planet.modules.Count;i++ )
                {
                    if (planet.modules[i].base_id == baseid)
                    { module = planet.modules[i]; break; }
                }

                if (module == null)
                    module = new PlanetModule(position, selectedresearch[currentresearchmode], baseid);
                else
                    module.power = sciencepower / 4;
            }

            for (int i = meteorites.Count - 1; i >= 0; i--)
            {
                if (meteorites[i].timetohit > 0)
                {
                    meteorites[i].timetohit -= (float)ellapsedtime;
                    if (meteorites[i].timetohit <= 0)
                    {
                        int mx = (int)meteorites[i].pos.X;
                        int my = (int)meteorites[i].pos.Y;
                        int area = Constants.Map_meteoritedestroyarea;
                        meteorites[i].timetohit = -1;

                        if (onShield(mx, my, Shield.Power))
                        {
                            meteorites.RemoveAt(i);
                        }
                        else 
                        {
                            AddMessage(Language.Message_MeteoriteLanded);
                            for (int x = -area; x <= area; x++)
                                for (int y = -area; y <= area; y++)
                                {
                                    double l = Math.Sqrt(x * x + y * y);
                                    if (l <= area)
                                    {
                                        if (onMap(mx + x, my + y) && data[my + y, mx + x].build_id >= 0 && !onShield(mx + x, my + y, Shield.Power))
                                            DestroyBuilding(mx + x, my + y);
                                    }
                                }
                        }
                    }
                }
                else
                {
                    meteorites[i].timetodestroy += ellapsedtime * Constants.Map_meteoriteexplotionspeed;
                    if (meteorites[i].timetodestroy > Constants.Map_meteoriteexplotiomaxsize) meteorites.RemoveAt(i);
                }
            }
        }

        public void UpdateOnline(float ellapsedtime, float totaltime)
        {
            if (data != null) Update(ellapsedtime, totaltime);
        }

        public bool onMap(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
        public bool onShield(int x, int y, int type)
        {
            foreach (Shield s in shields)
            {
                if (s.type == type)
                {
                    Vector2 det = new Vector2(x, y) - s.pos;
                    det.Y *= 2;
                    if (det.Length() <= s.size) return true;
                }
            }
            return false;
        }
        public bool onShield(float x, float y, int type)
        {
            foreach (Shield s in shields)
            {
                if (s.type == type)
                {
                    Vector2 det = new Vector2(x, y) - s.pos;
                    det.Y *= 2;
                    if (det.Length() <= s.size) return true;
                }
            }
            return false;
        }

        public bool isBuildingBuilded(int type)
        {
            foreach (Building b in buildings)
                if (b.type == type && b.buildingtime <= 0) return true;
            return false;
        }
        public int getBuildingCount(int type)
        {
            int count = 0;
            foreach (Building b in buildings)
                if (b.type == type) count++;
            return count;
        }

        public void AddMessage(string message)
        {
            if (messages == null)
                messages = new List<string>();
            messages.Add(message);
        }
    }

    public class MapHelper_Engine
    {
        public static int[] enabledata = new int[] { 1,1,0,1,   1,1,1,1,   0,0,1,1,   0,0,1,1,
                                                     0,0,0,0,   0,0,0,0,   0,0,0,0,   0,0,0,0,
                                                     0,1,0,1,   0,1,1,0,   1,1,0,0,   0,0,1,1,
                                                     0,1,1,1,   0,0,0,0,   0,1,1,1,   1,1,1,1,
                                                     0,1,1,1,   0,0,0,0,   0,1,0,0,   0,0,1,1,
                                                     0,0,0,0,   0,0,0,0,   1,0,0,0,   0,0,0,1,
                                                     0,0,0,0,   0,0,0,0,   0,0,0,1,   0,0,0,0,
                                                     1,1,1,1,   1,0,0,0,   1,0,1,1,   1,1,0,1,
                                                     1,1,1,1,   1,1,1,1,   1,1,1,1,   1,1,1,1,
                                                     1,1,1,1,   1,1,1,1,   1,1,1,1,   1,1,1,1,
                                                     1,1,1,1,   1,1,1,1,   1,1,1,1,   1,1,1,1,
                                                     1,1,1,1,   1,1,1,1,   1,1,1,1,   1,1,1,1,
                                                     1,1,1,1,   1,1,1,1,   1,1,1,1,   1,1,1,1,
                                                     1,1,1,1,   1,1,1,1,   1,1,1,1,   1,1,1,1,
                                                     1,1,1,1,   1,1,1,1,   1,1,1,1,   1,1,1,1,
                                                     1,1,1,1,   1,1,1,1,   1,1,1,1,   1,1,1,1};

        public struct ReplaceData
        {
            public int w, h;
            public short[,] olddata;
            public short[,] newdata;
            public short[,] subground;
            public int ex, ey;
            public int px, py;

            public ReplaceData(int w, int h, short[,] o, short[,] n, short[,] s, int ex, int ey, int px, int py)
            {
                this.w = w;
                this.h = h;
                olddata = o;
                newdata = n;
                subground = s;
                this.ex = ex;
                this.ey = ey;
                this.px = px;
                this.py = py;
            }
        }
        #region Replace data
        public static ReplaceData[] templates = new ReplaceData[] 
        { 
        new ReplaceData(4,4,new short[4,4]{{-1, 3 + 32,-1,-1},{3 + 32, -1,-1,-1},{-1,-1,-1,-1},{-1,-1,-1,-1}},
                            new short[4,4]{{-1,13+64,14+64,-1},{12+80,13+80,14+80,15+80},{12+96,13+96,14+96,15+96},{12+112,13+112,14+112,-1}},
                            new short[4,4]{{0,0,1,0},{0,0,0,1},{0,0,0,1},{0,0,1,0}},
                        2,2,0,0),

        new ReplaceData(4,4,new short[4,4]{{-1, -1, 1 + 32,-1},{-1,-1,-1, 1 + 32},{-1,-1,-1,-1},{-1,-1,-1,-1}},
                            new short[4,4]{{-1,9+64,10+64,-1},{8+80,9+80,10+80,11+80},{8+96,9+96,10+96,11+96},{-1,9+112,10+112,11+112}},
                            new short[4,4]{{0,1,0,0},{1,0,0,0},{1,0,0,0},{0,1,0,0}},
                        4,2,3,0),

        new ReplaceData(3,4,new short[4,3]{{2+32, 2+32, 2+32},{2+48, 2+48, 2+48},{2+48, 2+48, 2+48},{-1,-1,-1}},
                            new short[4,3]{{4+48, 4+48, 4+48},{5+80,6+80,7+80},{5+96,6+96,7+96},{5+112,6+112,7+112}},
                            new short[4,3]{{0,0,0},{0,0,0},{0,0,0},{0,0,0}},
                        3,1,0,0),

        new ReplaceData(3,3,new short[3,3]{{-1,-1, -1},{-1,-1, -1},{2, 2, 2}},
                            new short[3,3]{{5+48, 6+48, 7+48},{5+64, 6+64, 7+64},{4+64, 4+64, 4+64}},
                            new short[3,3]{{0,0,0},{0,0,0},{0,0,0}},
                        3,3,2,2),

        new ReplaceData(3,3,new short[3,3]{{3+16,-1, -1},{3+16,-1, -1},{3+16,-1, -1}},
                            new short[3,3]{{8+48,6,7},{8+48,6+16,7+16},{3+16,6+32,7+32}},
                            new short[3,3]{{0,1,1},{0,0,0},{0,0,0}},
                        1,3,0,1),
        
        new ReplaceData(3,3,new short[3,3]{{-1,-1, 1+16},{-1,-1, 1+16},{-1,-1, 1+16}},
                            new short[3,3]{{4,5,8+64},{4+16,5+16,8+64},{4+32,5+32,1+16}},
                            new short[3,3]{{1,1,0},{0,0,0},{0,0,0}},
                        3,3,2,2),
        
        new ReplaceData(4,4,new short[4,4]{{-1,-1,-1,-1},{3,-1,-1,-1},{-1,3,-1,-1},{-1,-1,-1,-1}},
                            new short[4,4]{{12,13,14,-1},{12+16,13+16,14+16,15+16},{-1,13+32,14+32,15+32},{-1,-1,14+48,-1}},
                            new short[4,4]{{1,1,1,0},{0,0,0,1},{0,0,0,1},{0,0,2,0}},
                        4,3,0,2),
        
        new ReplaceData(4,4,new short[4,4]{{-1,-1,-1,-1},{-1,-1,-1,1},{-1,-1,1,-1},{-1,-1,-1,-1}},
                            new short[4,4]{{-1,9,10,11},{8+16,9+16,10+16,11+16},{8+32,9+32,10+32,-1},{-1,9+48,-1,-1}},
                            new short[4,4]{{0,1,1,1},{1,0,0,0},{1,0,0,0},{0,2,0,0}},
                        4,3,3,2)
        };

        #endregion

        public class Resiple
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
        #region Reciptes
        public static Resiple[] resiptes = new Resiple[] { 
                new Resiple(new int[] { 9 }, new float[] { 2 }, 10, 1),
                //new Resiple(new int[] { 10,28 }, new float[] { 1,1 }, 22, 1),
                new Resiple(new int[] { 8,9 }, new float[] { 2,1 }, 23, 1),
                new Resiple(new int[] { 18 }, new float[] { 2}, 24, 1),
                new Resiple(new int[] { 18 }, new float[] { 2}, 25, 1),
                new Resiple(new int[] { 12 }, new float[] { 1}, 26, 0.5f),
                new Resiple(new int[] { 28,29,30 }, new float[] { 2,1,1}, 31, 2),
                new Resiple(new int[] { 18,4 }, new float[] { 0.3f,1}, 6, 1),
                new Resiple(new int[] { 8,9 }, new float[] { 1,1}, 11, 1.5f),
                new Resiple(new int[] { 11,28 }, new float[] { 1,0.5f}, 22, 1)};
        #endregion
    }
}
