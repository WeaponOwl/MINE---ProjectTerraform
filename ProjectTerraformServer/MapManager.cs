using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    class Building
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

        //Storage
        public const int Warehouse = 21;
        public const int House = 22;
        public const int Parking = 23;
        public const int EnergyBank = 24;
        public const int InfoStorage = 25;
        public const int LuquidStorage = 26;

        //Links
        public const int Spaceport = 31;
        public const int Exchanger = 32;
        public const int Beacon = 57;

        //Laboratory
        public const int Laboratory = 41;
        public const int Collector = 56;

        //Shields
        public const int AtmosophereShield = 51;
        public const int PowerShield = 52;
        public const int EmmisionShield = 53;
        public const int Turret = 58;

        //Decor
        public const int UnderShield = 54;

        public const int TotalBuildings = 58;

        public int type;
        public Vector2 pos;
        public float buildingtime;
        public float maxbuildingtime;
        public float starttime;
        public bool building;
        public float power;
        public int recipte;
        public float wait;
        public float worktime;
        public int workcount;
        public int height;
        public int oldheight;
        public int shieldid;
        public int health;
        public int lvl;

        public Building()
        {
            type = 0;
            building = false;
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
            building = false;
            power = 0;
            recipte = 0;
            wait = 0;
            worktime = 0;
            workcount = 0;
            height = 1;
            oldheight = 0;
            shieldid = -1;
            starttime = 0;
            health = 100;
            lvl = 1;
        }

        public static string GetName(int type)
        {
            switch (type)
            {
                case CommandCenter: return "Командный центр";
                case BuildCenter: return "Центр строительства";
                case StorageCenter: return "Центр хранения";
                case LinksCenter: return "Центр связей";
                case ScienceCenter: return "Центр науки";

                case DroidFactory: return "Фабрика дронов";
                case ProcessingFactory: return "Фабрика переработки";
                case Mine: return "Шахта";
                case Dirrick: return "Скважина";
                case Farm: return "Ферма";
                case Generator: return "Генератор";

                case Warehouse: return "Склад";
                case House: return "Дом";
                case Parking: return "Парковка";
                case EnergyBank: return "Хранилище энергии";
                case InfoStorage: return "Хранилище информации";
                case LuquidStorage: return "Резервуар";

                case Spaceport: return "Космопорт";
                case Exchanger: return "Биржа";

                case Laboratory: return "Лаборатория";

                case AtmosophereShield: return "Атмосферный щит";
                case PowerShield: return "Силовой щит";
                case EmmisionShield: return "Лучевой щит";

                case ClosedFarm: return "Крытая ферма";
                case UnderShield: return "Столб";

                case Collector: return "Рассадник";

                case Beacon: return "Маяк";
                case Turret: return "ЭМ вышка";

                default: return "Error: Noname building";
            }
        }
        public static string GetOverview(int type)
        {
            switch (type)
            {
                case CommandCenter: return "Главное здание базы. Занимается распределением ресурсов.";
                case BuildCenter: return "Увеличивает скорость строительства";
                case StorageCenter: return "Увеличивает размер хранилищ";
                case LinksCenter: return "Ускоряет дронов";
                case ScienceCenter: return "Ускоряет научную работу";

                case DroidFactory: return "Производит дронов";
                case ProcessingFactory: return "Перерабатывает ресурсы";
                case Mine: return "Добывает ископаемые";
                case Dirrick: return "Добывает жидкости";
                case Farm: return "Производит еду при благоприятном климате";
                case Generator: return "Производит энергию";

                case Warehouse: return "Хранилище для твердых ресурсов";
                case House: return "Простое жилье";
                case Parking: return "Посадочная площадка";
                case EnergyBank: return "Хранилище для энергии";
                case InfoStorage: return "Хранилище для научных образцов";
                case LuquidStorage: return "Хранилище для жидкостей";

                case Spaceport: return "Обеспечивает связь с кораблями";
                case Exchanger: return "Дает доступ к торговли";

                case Laboratory: return "Дает доступ к науке. Рабочее место для 10 человек";

                case AtmosophereShield: return "Создает благоприятный климат";
                case PowerShield: return "Создает силовое поле";
                case EmmisionShield: return "Создает лучевое поле";

                case ClosedFarm: return "Производит еду";
                case UnderShield: return "Опора";

                case Collector: return "Производит научные образцы";

                case Beacon: return "Собирает дронов в одной точке";

                case Turret: return "Базовая защита против пиратов";

                default: return "Error: Noname building";
            }
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

                case Collector: return new Rectangle(148, 299, 96 + 12, 95 + 3);

                case Beacon: return new Rectangle(472, 161, 32, 40);
                case Turret: return new Rectangle(462, 300, 32, 80);

                default: return new Rectangle(0, 0, 32, 32);
            }
        }
        public static Vector2 GetAnchor(int type)
        {
            Rectangle rect = GetSource(type);
            Vector2 anchor = new Vector2(rect.Width - 32, rect.Height - 32);
            int[] normal = new int[] {CommandCenter, ClosedFarm,BuildCenter,StorageCenter,LinksCenter,ScienceCenter,
                Mine, Parking, Farm, AtmosophereShield, PowerShield, EmmisionShield, UnderShield , Spaceport , Beacon,Turret};
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

                default: return 1;
            }
        }
        public static float GetBuildPrice(int type)
        {
            return GetBuildTime(type) * 4;
        }
    }
    struct BuildingInfo
    {
        public int count;
        public int workincount;
        public float workingpower;
        public List<int> ids;
    }
    struct Cell
    {
        public bool can_build;
        public bool can_move;
        public bool build;
        public bool build_draw;
        public int build_id;
        public int id;
        public int ground_id;
        public bool subground;
        public int height;
        public int mineresource_id;
        public int dirrickresource_id;

        public Cell(int id)
        {
            can_build = true;
            can_move = true;
            build = false;
            build_draw = false;
            subground = false;
            build_id = 0;
            this.id = id;
            ground_id = -1;
            height = 3;
            mineresource_id = -1;
            dirrickresource_id = -1;
        }
    }
    class Shield
    {
        public const int Power = 0;
        public const int Emmision = 1;
        public const int Atmosphere = 2;

        public int type;
        public float size;
        public Vector2 pos;
        public float createtime;

        public Shield(int type, float size, Vector2 pos,float time)
        {
            this.type = type;
            this.size = size;
            this.pos = pos;
            createtime = time;
        }
    }
    class Meteorite
    {
        public Vector2 pos;
        public Vector2 tar;
        public float height;
        public float explotion;

        public Meteorite(Vector2 p, Vector2 t, float h)
        {
            pos = p;
            tar = t;
            height = h;
            explotion = 0;
        }
    }
    class MapManager
    {
        public int width;
        public int height;
        public Cell[,] data;
        public float[,] lighting;
        public List<Building> buildings;
        public List<int>[,] unitslocation;
        public List<int>[,] particlelocation;
        public List<Unit> units;
        public List<Particle> particles;
        public List<Meteorite> meteorites;

        public const int maxresources = 32;
        public float[] inventory;
        public float oldenergy;
        public float[] exchangevalue;
        public int[] exchangetype;
        public int[] workingbuildingbyresourcetype;

        public string name;

        List<Point> finishpoint;

        public Planet planet;
        public int baseid;
        public Vector2 position;

        public static int[] enabledata = new int[] { 1,1,0,1,   0,1,0,1,   0,0,0,0,   0,0,0,0,
                                                     0,0,0,0,   1,0,0,0,   1,0,0,0,   0,0,0,0,
                                                     0,1,0,1,   1,0,0,0,   1,0,0,1,   1,0,0,0,
                                                     0,1,1,1,   0,0,0,0,   0,0,0,0,   0,0,0,0,
                                                     1,1,1,1,   0,0,0,0,   0,0,0,0,   0,0,0,0,
                                                     0,0,0,0,   0,0,0,0,   0,0,0,0,   0,0,0,0,
                                                     1,0,0,0,   0,0,0,0,   0,0,0,0,   0,0,0,0,
                                                     0,1,1,0,   0,0,0,0,   0,0,0,0,   0,0,0,0};

        public float[] maxpowerofbuilding;
        public float[] consumpowerofbuilding;
        public float[] havedpowerofbuilding;

        public float exchangelastcalltime;
        public float humanshiplastcalltime;
        public float hungertime;
        public int workhumans;

        public float[] maxstoragecapability;
        public float[] currentstoragecapability;
        
        public Resiple[] resiptes;

        public int selectedresearch;
        public int selectedproresearch;
        public bool sciencemode;
        public float timetodestroy;

        public Research[] research;
        public ProResearch[] proresearch;

        public BuildingInfo[] buildinginfo;

        public List<Shield> shields;

        public int humans;
        public bool iscommandcenter;

        public Perk perk;

        public float sciencelvl;

        void SetConstants()
        {
            maxpowerofbuilding = new float[7] { 1, 1, 1, 1, 1, 1, 1 };
            consumpowerofbuilding = new float[7] { 0, 0, 0, 0, 0, 0, 0 };
            havedpowerofbuilding = new float[7] { 0, 0, 0, 0, 0, 0, 0 };
            exchangevalue = new float[32];
            exchangetype = new int[32];
            workingbuildingbyresourcetype = new int[32];
            exchangelastcalltime = 0;
            humanshiplastcalltime = 0;
            hungertime = 0;
            workhumans = 0;
            maxstoragecapability = new float[6] { 1010, -1, 20, 1000, 200, 0 };
            currentstoragecapability = new float[6];
            currentstoragecapability[Constants.Map_energy] = 1010;

            selectedresearch = 0;
            selectedproresearch = 0;
            sciencemode = true;

            inventory = new float[maxresources];
            inventory[(int)Resources.energy] = 1010;
            inventory[(int)Resources.credits] = 1000;
            humans = 0;

            shields = new List<Shield>();
            meteorites = new List<Meteorite>();
            units = new List<Unit>();
            particles = new List<Particle>();
            buildings = new List<Building>();

            resiptes = new Resiple[] { 
                new Resiple(new int[] { 9 }, new float[] { 2 }, 10, 1),
                new Resiple(new int[] { 10,28 }, new float[] { 1,1 }, 22, 1),
                new Resiple(new int[] { 8,9 }, new float[] { 2,1 }, 23, 1),
                new Resiple(new int[] { 18 }, new float[] { 2}, 24, 1),
                new Resiple(new int[] { 18 }, new float[] { 2}, 25, 1),
                new Resiple(new int[] { 12 }, new float[] { 1}, 26, 0.5f),
                new Resiple(new int[] { 28,29,30 }, new float[] { 2,1,1}, 31, 2),
                new Resiple(new int[] { 18,4 }, new float[] { 0.3f,1}, 6, 1),
                new Resiple(new int[] { 8,9 }, new float[] { 1,1}, 11, 1.5f),
                new Resiple(new int[] { 11,28 }, new float[] { 1,0.5f}, 22, 1)};

            proresearch = new ProResearch[]{
                new ProResearch(200,"Контроль климата",2,new int[0],new float[0],"Изменяет температуру и давление на планете"),
                new ProResearch(200,"Планетарная защита",2,new int[0],new float[0],"Уменьшает шанс метеоритного дождя")};

            research = new Research[] { 
                new Research(100,"Исследование месности","Открывает полезные ископаемые"),
                new Research(100,"Институт науки","Ускоряет науку"),
                new Research(100,"Планирование построек","Ускорение строительства"), 
                new Research(100,"Анализ маршрутов","Увеличение скорости дронов"),
                new Research(100,"Алгоритмы упаковки","Увеличение размера хранилищ"),
                new Research(100,"Обогащение руды","Увеличение производства метала"),
                new Research(100,"Усиление щитов","Увеличение радиуса щитов"),
                new Research(100,"Улучшеные генераторы","Улеличеное производство энергии"),
                new Research(100,"Оптимизация потреления","Уменьшение расхода энергии"),
                new Research(100,"Орбитальные модули","Доступ к орбитальным модулям"),
                new Research(200,"Базовая археология","Стандартные процедуры при исследовании артефактов"),
                new Research(300,"Углубленная археология","Широкое исследование артефактов")
                };

            perk = new Perk();
        }

        public MapManager(int width = Constants.Map_width, int height = Constants.Map_height)
        {
            SetConstants();
            
            Random r = new Random(2);
            this.width = width;
            this.height = height;

            unitslocation = new List<int>[height, width];
            particlelocation = new List<int>[height, width];
            lighting = new float[height, width];
            data = new Cell[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    data[i, j] = new Cell(0);
                    unitslocation[i, j] = new List<int>();
                    particlelocation[i, j] = new List<int>();
                }

            //int scalex = 4, scaley = 8;
            //for (int i = 0; i < height / scaley; i++)
            //    for (int j = 0; j < width / scalex; j++)
            //    {
            //        if (r.Next(4) == 0)
            //        {
            //            for (int k = 0; k < 4; k++)
            //            {
            //                data[i * scaley + k, j * scalex].id = 1 + k * 8;
            //                data[i * scaley + k, j * scalex + 1].id = 3 + k * 8;
            //                data[i * scaley + k, j * scalex + 2].id = 5 + k * 8;
            //                data[i * scaley + k, j * scalex + 3].id = 7 + k * 8;

            //                data[i * scaley + k, j * scalex].can_build = false;
            //                data[i * scaley + k, j * scalex].can_move = false;
            //                data[i * scaley + k, j * scalex+1].can_build = false;
            //                data[i * scaley + k, j * scalex+1].can_move = false;
            //                data[i * scaley + k, j * scalex+2].can_build = false;
            //                data[i * scaley + k, j * scalex+2].can_move = false;
            //                data[i * scaley + k, j * scalex+3].can_build = false;
            //                data[i * scaley + k, j * scalex+3].can_move = false;
            //            }

            //            data[i * scaley + 4, j * scalex + 1].id = 35;
            //            data[i * scaley + 4, j * scalex + 2].id = 37;
            //        }
            //    }
        }

        public MapManager(string filename)
        {
            SetConstants();

            string[] strings = System.IO.File.ReadAllLines(filename);

            this.width = int.Parse(strings[1].Replace("width=", null));
            this.height = int.Parse(strings[2].Replace("height=", null));
            int buildcount = int.Parse(strings[3].Replace("buildings=", null));
            int dronecount = int.Parse(strings[4].Replace("drones=", null));

            unitslocation = new List<int>[height, width];
            particlelocation = new List<int>[height, width];
            data = new Cell[height, width];
            for (int i = 0; i < height; i++)
            {
                string[] substrings = strings[8 + i].Split(',');
                for (int j = 0; j < width; j++)
                {
                    string[] substrings2 = substrings[j].Split(';');
                    data[i, j] = new Cell(0);
                    unitslocation[i, j] = new List<int>();
                    particlelocation[i, j] = new List<int>();
                    data[i, j].id = int.Parse(substrings2[0]);
                    data[i, j].height = int.Parse(substrings2[1]);
                    data[i, j].mineresource_id = int.Parse(substrings2[2]);
                    data[i, j].dirrickresource_id = int.Parse(substrings2[3]);
                    data[i, j].can_build = enabledata[data[i, j].id] == 0;
                    data[i, j].can_move = data[i, j].can_build;

                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1) data[i, j].can_build = false;
                }
            }
            for (int i = 0; i < buildcount; i++)
            {
                string[] substring = strings[10 + height + i].Split(';');
                buildings.Add(new Building(int.Parse(substring[0])));
                buildings[buildings.Count - 1].pos = new Vector2(float.Parse(substring[1]), float.Parse(substring[2]));
                buildings[buildings.Count - 1].power = float.Parse(substring[3]);
                buildings[buildings.Count - 1].height = int.Parse(substring[4]);
            }
            for (int i = 0; i < dronecount; i++)
            {
                string[] substring = strings[12 + height + buildcount + i].Split(';');
                units.Add(new Unit(int.Parse(substring[0]), float.Parse(substring[1]), float.Parse(substring[2])));
                units[units.Count - 1].direction = int.Parse(substring[3]);
                units[units.Count - 1].command = new Command(commands.patrol, 0, 0);
            }

            for (int k = 0; k < buildings.Count; k++)
            {
                Point size = Building.GetSize(buildings[k].type);
                int x = (int)buildings[k].pos.X;
                int y = (int)buildings[k].pos.Y;
                bool[,] pass = Building.GetPassability(buildings[k].type);

                for (int i = 0; i < size.Y; i++)
                    for (int j = 0; j < size.X; j++)
                    {
                        data[y - i, x - j].build_id = k;
                        data[y - i, x - j].build = true;
                        data[y - i, x - j].can_build = false;
                        data[y - i, x - j].can_move = pass[i, j];
                    }
                data[y, x].build_draw = true;
            }

            ResetPerks();
        }

        public bool TryBuilding(int x, int y, int type)
        {
            if (x < 0 || y < 0 || x >= width || y >= height) return false;
            if (inventory[(int)Resources.credits] < Building.GetBuildPrice(type)) return false;
            Point size = Building.GetSize(type);
            if (data[y+size.Y-1, x+size.X-1].build_draw)
            {
                if (type == Building.Warehouse ||  type == Building.House)
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

                if(type == Building.AtmosophereShield||type == Building.EmmisionShield||type == Building.PowerShield)
                {
                    if (buildings[data[y, x].build_id].type == type && buildings[data[y, x].build_id].height < 7) return true;
                }
            }
            for (int i = 0; i < size.Y; i++)
                for (int j = 0; j < size.X; j++)
                {
                    if (!(data[y + i, x + j].can_build && data[y + i, x + j].height == data[y, x].height)) return false;
                }
            return true;
        }
        public void AddBuilding(int x, int y, int type,bool editor=false)
        {
            if (TryBuilding(x, y, type))
            {
                inventory[(int)Resources.credits] -= Building.GetBuildPrice(type);
                Point size = Building.GetSize(type);

                if (data[y+size.Y-1, x+size.X-1].build_draw)
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
                if (!editor)
                    buildings[buildings.Count - 1].maxbuildingtime = buildings[buildings.Count - 1].buildingtime = Building.GetBuildTime(type);
                buildings[buildings.Count - 1].pos = new Vector2(x + size.X - 1, y + size.Y - 1);
                buildings[buildings.Count - 1].power = 1;

                for (int i = 0; i < size.Y; i++)
                    for (int j = 0; j < size.X; j++)
                    {
                        data[y + i, x + j].build = true;
                        data[y + i, x + j].can_build = false;
                        data[y + i, x + j].build_id = buildings.Count - 1;
                        data[y + i, x + j].can_move = !pass[i, j];
                    }
                data[y + size.Y - 1, x + size.X - 1].build_draw = true;

                if (type == Building.Farm)
                {
                    buildings[buildings.Count - 1].recipte = (x + y) % 3;
                    if(buildings[buildings.Count - 1].recipte>0) buildings[buildings.Count - 1].recipte ++;
                }
                if (type == Building.ClosedFarm) buildings[buildings.Count - 1].recipte = (x / 2 + y) % 4;

                if (type == Building.Collector) buildings[buildings.Count - 1].recipte = (x / 4 + y / 2) % 2;

                ResetPerks();
            }
        }
        public void DestroyBuilding(int px, int py,MessageSystem ms)
        {
            int bid = data[py, px].build_id;
            bool offshields = false;
            if (data[py, px].build)
            {
                int type = buildings[bid].type;
                if (type == Building.Warehouse && buildings[bid].buildingtime <= 0)
                    maxstoragecapability[Constants.Map_rocks] -= Constants.Map_warehoucestoragecapability;
                if (type == Building.InfoStorage && buildings[bid].buildingtime <= 0)
                    maxstoragecapability[Constants.Map_science] -= Constants.Map_infostoragestoragecapability;
                if (type == Building.House && buildings[bid].buildingtime <= 0)
                    maxstoragecapability[Constants.Map_humans] -= Constants.Map_housestoragecapability;
                if (type == Building.EnergyBank && buildings[bid].buildingtime <= 0)
                    maxstoragecapability[Constants.Map_energy] -= Constants.Map_energybankstoragecapability;
                if (type == Building.LuquidStorage && buildings[bid].buildingtime <= 0)
                    maxstoragecapability[Constants.Map_luquids] -= Constants.Map_reservourstoragecapability;
                if (type == Building.AtmosophereShield || type == Building.PowerShield || type == Building.EmmisionShield)
                {
                    shields.Clear();
                    offshields = true;
                }

                if (buildings[bid].buildingtime > 0) inventory[(int)Resources.credits] += Building.GetBuildPrice(buildings[bid].type) * buildings[bid].buildingtime / Building.GetBuildTime(buildings[bid].type) * 2 / 3;
                inventory[(int)Resources.credits] += Building.GetBuildPrice(buildings[bid].type) / 3;

                Point size = Building.GetSize(buildings[bid].type);
                int x = (int)buildings[bid].pos.X;
                int y = (int)buildings[bid].pos.Y;
                buildings.RemoveAt(bid);
                for (int i = 0; i < size.Y; i++)
                    for (int j = 0; j < size.X; j++)
                    {
                        data[y - i, x - j].build = false;
                        data[y - i, x - j].build_id = 0;
                        data[y - i, x - j].build_draw = false;
                        data[y - i, x - j].can_build = true;
                        data[y - i, x - j].can_move = true;
                    }
                ms.AddMessage(MessageType.destroy, baseid, "Здание уничтожено", 1, Color.White);
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
                        data[y - i, x - j].build_id = k;
                    }
            }
            ResetPerks();
        }

        public void SetBadPassabilities()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1) data[i, j].can_build = false;
                }
            }
        }

        public void AddUnit(float x, float y,int type)
        {
            units.Add(new Unit(type, x, y));
            units[units.Count - 1].command = new Command(commands.patrol, 0, 0);
        }

        public void Update(float ellapsedtime, float totaltime, MessageSystem ms)
        {
            Random rand = new Random();

            //Set building info
            buildinginfo = new BuildingInfo[Building.TotalBuildings + 1];
            for (int i = 0; i < Building.TotalBuildings + 1; i++) buildinginfo[i].ids = new List<int>();
            for (int i = 0; i < buildings.Count; i++)
            {
                Building b = buildings[i];
                buildinginfo[b.type].count++;
                if (b.power > 0 && b.buildingtime <= 0)
                {
                    int type = Building.GetType(b.type);
                    buildinginfo[b.type].workingpower += type < 7 ? maxpowerofbuilding[type] : 1 * b.power;
                    buildinginfo[b.type].workincount++;
                    buildinginfo[b.type].ids.Add(i);
                }
            }

            for (int i = 0; i < 7; i++)
            {
                havedpowerofbuilding[i] = 0;
                consumpowerofbuilding[i] = 0;
            }

            sciencelvl = 0;

            //Get temperature on base's height
            float fy = Math.Abs((position.Y - planet.mapheight / 2) / (float)(planet.mapheight / 2));
            float temperature = (float)(planet.maxtemperature - (planet.maxtemperature - planet.mintemperature) * fy);

            //Destroy ALL!!!
            iscommandcenter = buildinginfo[Building.CommandCenter].workincount > 0;
            if (iscommandcenter)
                timetodestroy = Constants.Map_timetodestroybase;
            else
                ms.AddMessage(MessageType.timetodestroybase, baseid, "База уничтожится через: " + timetodestroy.ToString("0"), 1, Color.White);
            timetodestroy -= ellapsedtime;

            //All haman - scientists
            workhumans = humans;

            //Update storages empty places
            currentstoragecapability[0] = 0;currentstoragecapability[2] = 0;currentstoragecapability[3] = 0;currentstoragecapability[4] = 0;
            for (int i = 0; i < maxresources; i++)
            {
                workingbuildingbyresourcetype[i] = 0;
                currentstoragecapability[GetResourceType(i)] += inventory[i];
            }

            //update ships time
            exchangelastcalltime -= ellapsedtime;
            if (exchangelastcalltime < 0) exchangelastcalltime = 0;
            humanshiplastcalltime -= ellapsedtime;
            if (humanshiplastcalltime < 0) humanshiplastcalltime = 0;

            List<int> piratids = new List<int>();

            #region Unit update
            List<int> builderunitsid = new List<int>();
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].type == Unit.Pirate) piratids.Add(i);
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
                    { if ((units[i].waypoints[0] - units[i].pos).LengthSquared() < 0.01f) { units[i].pos = units[i].waypoints[0]; units[i].waypoints.RemoveAt(0); } }
                    else if ((units[i].tar - units[i].pos).LengthSquared() < 0.01f) units[i].pos = units[i].tar;

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
                    //drone to beacon
                    else if (units[i].command.message == commands.patroltobeacon)
                    {
                        units[i].tar = units[i].pos;
                        units[i].command.message = commands.patrol;
                        units[i].wait = Constants.Map_unitbeaconwait;
                    }
                    //builer drone
                    else if (units[i].command.message == commands.moveandbuild)
                    {
                        //if building was not destroyed
                        if (data[units[i].command.y, units[i].command.x].build)
                        {
                            units[i].command.id = data[units[i].command.y, units[i].command.x].build_id;
                            if (buildings[units[i].command.id].buildingtime > 0)
                            {
                                buildings[units[i].command.id].building = true;
                                buildings[units[i].command.id].buildingtime -= (ellapsedtime * perk.buildbonus * perk.buildingspeedup) < buildings[units[i].command.id].buildingtime ? (ellapsedtime * perk.buildbonus * perk.buildingspeedup) : buildings[units[i].command.id].buildingtime;
                                if (rand.Next(Constants.Map_buildingparticlechance) == 0)
                                    particles.Add(new Particle(buildings[units[i].command.id].pos,
                                                               Building.GetAnchor(buildings[units[i].command.id].type), 
                                                               Building.GetSource(buildings[units[i].command.id].type), 
                                                               Constants.Map_buildingparticlelife, 
                                                               new Vector2(0, -32)));
                            }
                            else
                            {
                                ms.AddMessage(MessageType.builded, baseid, "Здание построено", 1, Color.White);
                                units[i].command.message = commands.patrol;
                                buildings[units[i].command.id].power = 1;
                                buildings[units[i].command.id].starttime = totaltime;
                                buildings[units[i].command.id].wait = 0;

                                if (buildings[units[i].command.id].type == Building.Warehouse)
                                    maxstoragecapability[Constants.Map_rocks] += Constants.Map_warehoucestoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.InfoStorage)
                                    maxstoragecapability[Constants.Map_science] += Constants.Map_infostoragestoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.House)
                                    maxstoragecapability[Constants.Map_humans] += Constants.Map_housestoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.EnergyBank)
                                    maxstoragecapability[Constants.Map_energy] += Constants.Map_energybankstoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
                                if (buildings[units[i].command.id].type == Building.LuquidStorage)
                                    maxstoragecapability[Constants.Map_luquids] += Constants.Map_reservourstoragecapability * (buildings[units[i].command.id].height - buildings[units[i].command.id].oldheight);
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

                        for (int q = 0; q < maxresources; q++)
                        {
                            if (rand.Next(3) == 0)
                                inventory[q] -= inventory[q] * (float)(0.01f + 0.04f * rand.NextDouble());
                        }

                        units[i].wait = 2;
                        Vector2 pos = GetRandomBaseDirection();
                        units[i].command = new Command(commands.pirateaway, (int)pos.X, (int)pos.Y);
                        units[i].tar = pos;
                        units[i].waypoints.Clear();
                    }
                    else if (units[i].command.message == commands.pirateaway)
                    {
                        units.RemoveAt(i);
                        piratids.Remove(i);
                    }
                    #region Trader
                    else if (units[i].command.message == commands.tradergoin)
                        units[i].command = new Command(commands.tradegodown, 0);
                    else if (units[i].command.message == commands.tradergoaway)
                        units.RemoveAt(i);
                    else if (units[i].command.message == commands.tradegodown)
                    {
                        units[i].height -= ellapsedtime;
                        if (units[i].height <= units[i].command.id)
                        {
                            units[i].height = units[i].command.id;
                            for (int k = 0; k < maxresources; k++)
                            {
                                //buy
                                if (exchangetype[k] == 1 && (int)inventory[k] < exchangevalue[k])
                                {
                                    float count = Math.Min(exchangevalue[k] - inventory[k], Constants.Unit_tradervalue);
                                    float price = Math.Min(Constants.GetPriceOfResource(k) * count, inventory[(int)Resources.credits]);
                                    count = price / Constants.GetPriceOfResource(k);

                                    inventory[(int)Resources.credits] -= count;
                                    TryAddResource(k, count, ms);
                                }
                                //sell
                                if (exchangetype[k] == 2 && (int)inventory[k] > exchangevalue[k])
                                {
                                    float count = Math.Min(inventory[k] - exchangevalue[k], Constants.Unit_tradervalue);
                                    inventory[k] -= count;
                                    inventory[(int)Resources.credits] += count * Constants.GetPriceOfResource(k) * 0.8f;
                                }
                            }
                            units[i].command = new Command(commands.tradegoup, Constants.Unit_traderheight);
                            units[i].wait = Constants.Unit_traderwait;
                        }
                    }
                    else if (units[i].command.message == commands.tradegoup)
                    {
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
                            int newhums = Math.Min(rand.Next(Constants.Map_humannewrand) + Constants.Map_humannew, (int)(maxstoragecapability[Constants.Map_humans] - currentstoragecapability[Constants.Map_humans]));
                            if (inventory[(int)Resources.meat] + inventory[(int)Resources.fruits] + inventory[(int)Resources.vegetables] + inventory[(int)Resources.fish] > 0 && inventory[(int)Resources.water] > 0)
                            {
                                humans += newhums;
                                currentstoragecapability[Constants.Map_humans] += newhums;
                            }
                            else
                            {
                                humans -= newhums / 2;
                                if (humans < 0) humans = 0;
                                currentstoragecapability[Constants.Map_humans] = humans;
                            }
                            units[i].command = new Command(commands.humangoup, Constants.Unit_humanheight);
                            units[i].wait = Constants.Unit_humanwait;
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
                }
                #endregion

                if (i < units.Count && (units[i].command.message == commands.patrol || units[i].command.message == commands.patroltobeacon))
                    builderunitsid.Add(i);
            }
            #endregion

            #region Builds update
            List<int> buildngsthatneeddestroyed = new List<int>();

            for (int i = 0; i < buildings.Count; i++)
            {
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
                        if (buildings[i].building) buildings[i].building = false;
                        else
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
                                if (inventory[(int)Resources.energyore] > 0)
                                {
                                    inventory[(int)Resources.energyore] -= Constants.Map_energyoreconsuming * ellapsedtime;
                                    if (inventory[(int)Resources.energyore] < 0) inventory[(int)Resources.energyore] = 0;
                                    multiplier = 3;
                                }
                                TryAddResource((int)Resources.energy, multiplier * buildings[i].power * Constants.Map_generatorproduce * ellapsedtime * perk.optimizeerergyproduce);
                                workingbuildingbyresourcetype[(int)Resources.energy]++;//+= (int)buildings[i].power;
                                break;
                            case Building.Mine:
                                for (int k = 0; k < thisbuildingsize.X; k++)
                                    for (int j = 0; j < thisbuildingsize.Y; j++)
                                    {
                                        if (data[thisbuildingpos_y - k, thisbuildingpos_x - j].mineresource_id > 0)
                                            TryAddResource(data[thisbuildingpos_y - k, thisbuildingpos_x - j].mineresource_id, buildings[i].power * maxpowerofbuilding[thisbuildingtype] * ellapsedtime, ms);
                                        else TryAddResource((int)Resources.rock, buildings[i].power * maxpowerofbuilding[thisbuildingtype] / 3 * ellapsedtime, ms);
                                    }
                                break;
                            case Building.Dirrick: if (data[thisbuildingpos_y, thisbuildingpos_x].dirrickresource_id > 0)
                                    TryAddResource(data[thisbuildingpos_y, thisbuildingpos_x].dirrickresource_id, buildings[i].power * maxpowerofbuilding[thisbuildingtype] * ellapsedtime, ms);
                                break;
                            case Building.ProcessingFactory:
                                bool work = true;
                                float thisbuildingpow = buildings[i].power * maxpowerofbuilding[thisbuildingtype];
                                //if have all components
                                for (int k = 0; k < resiptes[buildings[i].recipte].incount.Length; k++)
                                {
                                    if (inventory[resiptes[buildings[i].recipte].inresourses[k]] < resiptes[buildings[i].recipte].incount[k] * ellapsedtime * thisbuildingpow)
                                        work = false;
                                }
                                //continue
                                if (work)
                                {
                                    for (int k = 0; k < resiptes[buildings[i].recipte].incount.Length; k++)
                                    {
                                        float deletecount = resiptes[buildings[i].recipte].incount[k] * ellapsedtime * thisbuildingpow;
                                        inventory[resiptes[buildings[i].recipte].inresourses[k]] -= deletecount;
                                        currentstoragecapability[GetResourceType(i)] -= deletecount;
                                    }
                                    float outres = resiptes[buildings[i].recipte].outcount * thisbuildingpow * ellapsedtime;
                                    if (resiptes[buildings[i].recipte].outresource == (int)Resources.metal) outres *= perk.optimizeore;

                                    TryAddResource(resiptes[buildings[i].recipte].outresource, outres, ms);
                                    workingbuildingbyresourcetype[resiptes[buildings[i].recipte].outresource] += (int)buildings[i].power;
                                } 
                                break;
                            case Building.DroidFactory:
                                if (buildings[i].worktime > 0)
                                {
                                    buildings[i].worktime -= ellapsedtime;
                                    if (buildings[i].worktime <= 0) AddUnit(buildings[i].pos.X + 0.5f, buildings[i].pos.Y + 0.5f, Unit.Drone);
                                }
                                if (buildings[i].worktime <= 0 && buildings[i].workcount > 0 && inventory[(int)Resources.metal] >= Constants.Unit_dronemetalprice && inventory[(int)Resources.electronics] >= Constants.Unit_droneelectronicprice)
                                {
                                    inventory[(int)Resources.metal] -= Constants.Unit_dronemetalprice;
                                    inventory[(int)Resources.electronics] -= Constants.Unit_droneelectronicprice;
                                    buildings[i].workcount--;
                                    buildings[i].worktime = Constants.Map_dronefactoryworktime;
                                }
                                break;
                            case Building.Collector:
                                TryAddResource((int)Resources.animals + buildings[i].recipte, buildings[i].power * ellapsedtime * 4, ms);
                                break;
                            case Building.Laboratory:
                                thisbuildingpow = buildings[i].power * maxpowerofbuilding[thisbuildingtype];

                                int workhuminthislab = Math.Min(workhumans, Constants.Map_laboratoryworkinghuman);
                                float price = Math.Min(workhuminthislab * Constants.Map_workinghumanprice * thisbuildingpow, inventory[(int)Resources.credits]);
                                if (price < 0) price = 0;
                                workhuminthislab = (int)(price / Constants.Map_workinghumanprice);
                                price = workhuminthislab * Constants.Map_workinghumanprice * thisbuildingpow;
                                if (price < 0) price = 0;

                                float scturbo = inventory[(int)Resources.animals] + inventory[(int)Resources.plants];
                                sciencelvl += workhuminthislab * perk.sciencebonus * perk.optimizeresearch * 4 * (scturbo > 0 ? 2 : 1);
                                if (scturbo > 0)
                                {
                                    inventory[(int)Resources.animals] -= (inventory[(int)Resources.animals] / scturbo) * 2 * ellapsedtime;
                                    inventory[(int)Resources.plants] -= (inventory[(int)Resources.plants] / scturbo) * 2 * ellapsedtime;
                                }

                                //normal research
                                if (sciencemode)
                                {
                                    if (research[selectedresearch].time > 0)
                                    {
                                        inventory[(int)Resources.credits] -= price;
                                        workhumans -= workhuminthislab;
                                        research[selectedresearch].time -= ellapsedtime * workhuminthislab * perk.sciencebonus * perk.optimizeresearch / Constants.Map_laboratorymaxworkinghuman / 3 * (scturbo > 0 ? 2 : 1);
                                        if (research[selectedresearch].time <= 0)
                                        {
                                            research[selectedresearch].time = 0;
                                            research[selectedresearch].searched = true;
                                            ms.AddMessage(MessageType.researchok, baseid, "Исследование завершено", 1, Color.White);
                                            switch (selectedresearch)
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
                                else
                                {
                                    if (proresearch[selectedproresearch].time > 0)
                                    {
                                        inventory[(int)Resources.credits] -= price;
                                        workhumans -= workhuminthislab;
                                        proresearch[selectedproresearch].time -= ellapsedtime * workhuminthislab * perk.sciencebonus * perk.optimizeresearch / Constants.Map_laboratorymaxworkinghuman / 3 * (scturbo > 0 ? 2 : 1);
                                        proresearch[selectedproresearch].searched = false;
                                        if (proresearch[selectedproresearch].time <= 0)
                                        {
                                            ms.AddMessage(MessageType.researchok, baseid, "Планетарное исследование завершено", 1, Color.White);
                                            proresearch[selectedproresearch].time = 0;
                                            proresearch[selectedproresearch].searched = true;
                                        }
                                    }
                                    else
                                    {
                                        proresearch[selectedproresearch].searched = true;
                                        inventory[(int)Resources.credits] -= price;
                                        workhumans -= workhuminthislab;
                                    }
                                }
                                break;
                            case Building.Farm:
                                if (onShield(thisbuildingpos_x, thisbuildingpos_y, Shield.Atmosphere) || (temperature > 10 && temperature < 40))
                                    TryAddResource((int)Resources.meat + buildings[i].recipte, buildings[i].power * ellapsedtime, ms);
                                break;
                            case Building.ClosedFarm:
                                TryAddResource((int)Resources.meat + buildings[i].recipte, buildings[i].power * ellapsedtime, ms);
                                break;
                            case Building.PowerShield:
                                if (buildings[i].shieldid < 0)
                                {
                                    shields.Add(new Shield(Shield.Power, buildings[i].height * 4, buildings[i].pos + new Vector2(0.5f, 0.5f), totaltime));
                                    buildings[i].shieldid = shields.Count - 1;
                                }
                                shields[buildings[i].shieldid].size = buildings[i].height * 2 * buildings[i].power * maxpowerofbuilding[thisbuildingtype] * perk.optimizingshield;
                                break;
                            case Building.AtmosophereShield:
                                if (buildings[i].shieldid < 0)
                                {
                                    shields.Add(new Shield(Shield.Atmosphere, buildings[i].height * 4, buildings[i].pos + new Vector2(0.5f, 0.5f), totaltime));
                                    buildings[i].shieldid = shields.Count - 1;
                                }
                                shields[buildings[i].shieldid].size = buildings[i].height * 2 * buildings[i].power * maxpowerofbuilding[thisbuildingtype] * perk.optimizingshield;
                                break;
                            case Building.EmmisionShield:
                                if (buildings[i].shieldid < 0)
                                {
                                    shields.Add(new Shield(Shield.Emmision, buildings[i].height * 4, buildings[i].pos + new Vector2(0.5f, 0.5f), totaltime));
                                    buildings[i].shieldid = shields.Count - 1;
                                }
                                shields[buildings[i].shieldid].size = buildings[i].height * 2 * buildings[i].power * maxpowerofbuilding[thisbuildingtype] * perk.optimizingshield;
                                break;
                            case Building.Turret:
                                buildings[i].wait = 0.33f;
                                foreach (int id in piratids)
                                {
                                    if (id < units.Count)
                                    {
                                        temp = (int)(units[id].pos - buildings[i].pos).Length();
                                        if (units[id].command.message == commands.piraterob && temp < 8)
                                        {
                                            //units[id].wait = 1.5f;
                                            Vector2 pos = GetRandomBaseDirection();
                                            units[id].command = new Command(commands.pirateaway, (int)pos.X, (int)pos.Y);
                                            units[id].tar = pos;
                                            units[id].waypoints.Clear();

                                            buildings[i].wait = 4.3f;
                                            break;
                                        }
                                    }
                                }
                                break;
                        }
                        #endregion

                        float resources = Building.GetEnergy(buildings[i].type) * buildings[i].power / perk.otimizeenergyconsume;
                        if (thisbuildingtype < 7)
                        {
                            resources *= maxpowerofbuilding[thisbuildingtype];
                            consumpowerofbuilding[thisbuildingtype] += resources;
                        }

                        if (resources > inventory[(int)Resources.energy])
                        {
                            buildings[i].power = 0;
                            ms.AddMessage(MessageType.buildingoff, baseid, "Нехватка энергии: здание выключено", 1, Color.White);
                        }
                        else
                        {
                            if (thisbuildingtype < 7)
                                havedpowerofbuilding[thisbuildingtype] += resources;
                            inventory[(int)Resources.energy] -= resources * ellapsedtime;
                        }
                    }
                    #endregion
                }
            }
            for (int i = buildngsthatneeddestroyed.Count - 1; i >= 0; i--)
            {
                int id = buildngsthatneeddestroyed[i];
                DestroyBuilding((int)buildings[id].pos.X, (int)buildings[id].pos.Y, ms);
                ms.AddMessage(MessageType.none, baseid, "Не найден путь к зданию. Здание уничтожено.", 3, Color.White);
            }
            #endregion

            //Update particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].offcet += particles[i].speed * ellapsedtime;
                particles[i].life -= ellapsedtime;
                if (particles[i].life <= 0) particles.RemoveAt(i);
            }

            for (int i = 0; i < maxresources; i++)
            {
                if (inventory[i] < 0) inventory[i] = 0;

                //if need trader - create trader
                if ((exchangetype[i] == 1 && inventory[i] < exchangevalue[i]) || (exchangetype[i] == 2 && inventory[i] > exchangevalue[i]))
                {
                    if (exchangelastcalltime > 0) break;
                    else if (buildings.Count > 0 && buildinginfo[Building.Spaceport].workincount > 0 && buildinginfo[Building.Parking].workincount>0)
                    {
                        exchangelastcalltime = Constants.Map_timefornewtrader;
                        int n = buildinginfo[Building.Spaceport].workincount;
                        for (int q = 0; q < n; q++)
                            exchangelastcalltime /= 1 + 1.0f / (q + 1);
                        exchangelastcalltime /= buildinginfo[Building.Spaceport].workingpower / buildinginfo[Building.Spaceport].workincount;

                        int id = buildinginfo[Building.Parking].ids[rand.Next(buildinginfo[Building.Parking].ids.Count)];
                        if (buildinginfo[Building.Spaceport].workincount >= 0 && id >= 0)
                        {
                            Vector2 pos = GetRandomBaseDirection();

                            units.Add(new Unit(Unit.Merchant, pos.X, pos.Y));
                            units[units.Count - 1].command = new Command(commands.tradergoin, (int)buildings[id].pos.X, (int)buildings[id].pos.Y);
                            units[units.Count - 1].tar = buildings[id].pos;
                            units[units.Count - 1].height = Constants.Unit_traderheight;
                            buildings[id].wait = Constants.Map_parkingwaittime;
                        }
                    }
                }
            }

            //humans like eat
            if (humans > 0)
            {
                float food = inventory[(int)Resources.meat] + inventory[(int)Resources.fruits] + inventory[(int)Resources.vegetables] + inventory[(int)Resources.fish];
                float eat = 0.05f;
                float water = inventory[(int)Resources.water];
                if (food > 0 && water > 0)
                {
                    inventory[(int)Resources.meat] -= Math.Min(humans * eat * inventory[(int)Resources.meat] / food * ellapsedtime, inventory[(int)Resources.meat]);
                    inventory[(int)Resources.fruits] -= Math.Min(humans * eat * inventory[(int)Resources.fruits] / food * ellapsedtime, inventory[(int)Resources.fruits]);
                    inventory[(int)Resources.vegetables] -= Math.Min(humans * eat * inventory[(int)Resources.vegetables] / food * ellapsedtime, inventory[(int)Resources.vegetables]);
                    inventory[(int)Resources.fish] -= Math.Min(humans * eat * inventory[(int)Resources.fish] / food * ellapsedtime, inventory[(int)Resources.fish]);

                    inventory[(int)Resources.water] -= Math.Min(humans * eat * ellapsedtime, inventory[(int)Resources.water]);

                    TryAddResource((int)Resources.biowaste, humans * eat * ellapsedtime);
                    hungertime = Constants.Map_hungerstarttime;
                }
                else
                {
                    string text = "Колония голодает: ";
                    if (food > 0 && water > 0) text += "нехватка еды и воды";
                    else if (food > 0) text += "нехватка воды";
                    else text += "нехватка еды";
                    ms.AddMessage(MessageType.hunger, baseid, text, 1, Color.White);
                    hungertime -= ellapsedtime;
                    if (hungertime < 0)
                    {
                        hungertime = Constants.Map_hungernexttime;
                        humans--;
                        currentstoragecapability[Constants.Map_humans]--;
                    }
                }
            }

            //if all humans requers ok - launch humanship
            if (currentstoragecapability[Constants.Map_humans] < maxstoragecapability[Constants.Map_humans] && 
                humanshiplastcalltime == 0 && 
                inventory[(int)Resources.meat] + inventory[(int)Resources.fruits] + inventory[(int)Resources.vegetables] + inventory[(int)Resources.fish] > 0 && 
                inventory[(int)Resources.water] > 0)
            {
                humanshiplastcalltime = Constants.Map_timefornewhuman;
                int id = buildinginfo[Building.Parking].ids[rand.Next(buildinginfo[Building.Parking].ids.Count)];
                if (buildinginfo[Building.Spaceport].workincount >= 0 && id >= 0)
                {
                    units.Add(new Unit(Unit.HumanShip, -1, -1));
                    units[units.Count - 1].command = new Command(commands.humangoin, (int)buildings[id].pos.X, (int)buildings[id].pos.Y);
                    units[units.Count - 1].tar = buildings[id].pos;
                    units[units.Count - 1].height = Constants.Unit_humanheight;
                    buildings[id].wait = Constants.Map_parkingwaittime;
                }
            }

            //update meteorites
            for (int i = meteorites.Count - 1; i >= 0; i--)
            {
                Vector2 d = meteorites[i].tar - meteorites[i].pos;
                Vector2 dir = Vector2.Normalize(d) * ellapsedtime * Constants.Map_meteoritespeed;

                if (dir.Length() > d.Length())
                {
                    meteorites[i].explotion += ellapsedtime * Constants.Map_meteoriteexplotionspeed;
                    if (meteorites[i].explotion > Constants.Map_meteoriteexplotiomaxsize) meteorites.RemoveAt(i);
                }
                else
                {
                    meteorites[i].pos = meteorites[i].pos + dir;
                    meteorites[i].height -= meteorites[i].height * (dir.Length() / (meteorites[i].tar - meteorites[i].pos).Length());

                    d = meteorites[i].tar - meteorites[i].pos;
                    dir = Vector2.Normalize(d) * ellapsedtime * Constants.Map_meteoritespeed;

                    if (dir.Length() > d.Length())
                    {
                        ms.AddMessage(MessageType.meteorite, baseid, "Метеоритный дождь", 1, Color.White);

                        int mx = (int)meteorites[i].pos.X;
                        int my = (int)meteorites[i].pos.Y;
                        int area = Constants.Map_meteoritedestroyarea;

                        for (int x = -area; x <= area; x++)
                            for (int y = -area; y <= area; y++)
                            {
                                double l = Math.Sqrt(x * x + y * y);
                                if (l <= area)
                                {
                                    if (onMap(mx + x, my + y) && data[my + y, mx + x].build && !onShield(mx + x, my + y, Shield.Power))
                                        DestroyBuilding(mx + x, my + y, ms);
                                }
                            }
                    }
                }
            }
        }

        public bool onMap(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
        public bool onShield(int x, int y,int type)
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

        int PointComparer(Point x, Point y)
        {
            return Math.Abs(x.X - finishpoint[0].X) + Math.Abs(x.Y - finishpoint[0].Y) - Math.Abs(y.X - finishpoint[0].X) - Math.Abs(y.Y - finishpoint[0].Y);
        }

        public List<Vector2> GetWayToBuilding(Vector2 start, Vector2 end)
        {
            List<Vector2> way = new List<Vector2>();
            
            finishpoint = new List<Point>();
            if (data[(int)end.Y, (int)end.X].build)
            {
                Point size = Building.GetSize(buildings[data[(int)end.Y, (int)end.X].build_id].type);
                for (int i = 0; i < size.Y; i++)
                {
                    if (data[(int)end.Y - i, (int)end.X + 1].can_move) finishpoint.Add(new Point((int)end.X +1, (int)end.Y - i));
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

        int GetResourceType(int resource)
        {
            if (resource == 0) return 0;
            if (resource == (int)Resources.animals || resource == (int)Resources.plants) return Constants.Map_science;
            if (resource == 1) return 1;
            if (resource == (int)Resources.water || resource == (int)Resources.oil || resource == (int)Resources.biowaste || resource == (int)Resources.chemical) return 4;
            return 3;
        }
        void TryAddResource(int resource, float count)
        { 
            int type = GetResourceType(resource);
            if (maxstoragecapability[type] < 0)
            {
                inventory[resource] += count;
                return;
            }
            if (iscommandcenter || type == (int)Resources.energy)
            {
                if (type == Constants.Map_rocks)
                {
                    if (currentstoragecapability[type] < maxstoragecapability[type] * perk.storagebonus * perk.optimizestorage)
                    {
                        currentstoragecapability[type] += count;
                        inventory[resource] += count;
                    }
                }
                else
                {
                    if (currentstoragecapability[type] < maxstoragecapability[type] * perk.storagebonus)
                    {
                        currentstoragecapability[type] += count;
                        inventory[resource] += count;
                    }
                }
            }
        }
        void TryAddResource(int resource, float count,MessageSystem ms)
        {
            int type = GetResourceType(resource);
            if (maxstoragecapability[type] < 0)
            {
                inventory[resource] += count;
                return;
            }
            if (iscommandcenter || type == (int)Resources.energy)
            {
                if (type == Constants.Map_rocks)
                {
                    if (currentstoragecapability[type] < maxstoragecapability[type] * perk.storagebonus * perk.optimizestorage)
                    {
                        currentstoragecapability[type] += count;
                        inventory[resource] += count;
                    }
                    else ms.AddMessage(MessageType.cannotaddresources, baseid, "Ресурсы не могут быть размещены! Хранилища заполнены!", 1, Color.White);
                }
                else
                {
                    if (currentstoragecapability[type] < maxstoragecapability[type] * perk.storagebonus)
                    {
                        currentstoragecapability[type] += count;
                        inventory[resource] += count;
                    }
                    else if (type != (int)Resources.energy) ms.AddMessage(MessageType.cannotaddresources, baseid, "Ресурсы не могут быть размещены! Хранилища заполнены!", 1, Color.White);
                }
            }
            if (!iscommandcenter) ms.AddMessage(MessageType.cannotaddresources, baseid, "Ресурсы не могут быть размещены! Требуется Коммандный центр!", 1, Color.White);
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

        public Vector2 GetRandomBaseDirection()
        {
            List<Vector2> pos = new List<Vector2>();
            pos.Add(new Vector2(new Random().Next(width + 2) - 1, -1));
            //for (int k = 0; k < planet.basepositions.Count; k++)
            //{
            //    if (k != baseid && planet.map[k].IsBuildingBuilded(Building.Exchanger))
            //    {
            //        Vector2 v = planet.basepositions[k] - planet.basepositions[baseid];
            //        int mody = v.Y > 0 ? -1 : 1;
            //        int modx = v.X > 0 ? 1 : -1;

            //        if (Math.Abs(v.Y) > Math.Abs(v.X))
            //        {
            //            v.X = v.X / v.Y;
            //            v.Y = 1;
            //        }
            //        else
            //        {
            //            v.Y = v.Y / v.X;
            //            v.X = 1;
            //        }

            //        v.X *= modx;
            //        v.Y *= mody;

            //        v.X = 32 + v.X * 36;
            //        v.Y = 32 + v.Y * 36;

            //        pos.Add(v);
            //    }
            //}

            int selectpos = new Random().Next(pos.Count);
            return pos[selectpos];
        }

        public float GetCredits(){ return inventory[(int)Resources.credits]; }
        public float GetScience() { return sciencelvl;}
        public float GetPopulation() { return humans;}

        public bool IsBeDestroyedByMeteor(Vector2 pos)
        {
            int mx = (int)pos.X;
            int my = (int)pos.Y;
            int area = Constants.Map_meteoritedestroyarea;

            for (int x = -area; x <= area; x++)
                for (int y = -area; y <= area; y++)
                {
                    double l = Math.Sqrt(x * x + y * y);
                    if (l <= area)
                    {
                        if (onMap(mx + x, my + y) && data[my + y, mx + x].build && !onShield(mx + x, my + y, Shield.Power))
                            return true;
                    }
                }
            return false;
        }
    }
}