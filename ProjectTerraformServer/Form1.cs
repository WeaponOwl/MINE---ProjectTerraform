using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NetComm;
using ProjectTerraform;

namespace ProjectTerraformServer
{
    public struct Packet
    {
        public byte[] packet;

        public Packet(byte[] p)
        {
            packet = p;
        }

        public void Send(NetComm.Host host, string id)
        {
            host.SendData(id, packet);
        }

        public Packet[] Split(NetComm.Host host)
        {
            int length = host.SendBufferSize-16;//host.SendBufferSize - 8;
            int data_l = packet.Length / length;
            int data_m = packet.Length % length;
            data_l += data_m == 0 ? 0 : 1;
            Packet[] data = new Packet[data_l + 1];

            int k=0;
            for (int i = 0; i < packet.Length; i += length)
            {
                int l = Math.Min(packet.Length - i, length);
                data[k + 1].packet = new byte[l];
                Array.Copy(packet, i, data[k + 1].packet, 0, l);
                k++;
            }

            data[0].packet = new byte[1 + 4 + 4];
            data[0].packet[0] = 100;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(data[0].packet, 1, 8);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);
            bw.Write(data_l);
            bw.Write(packet.Length);
            bw.Close();
            ms.Close();
            return data;
        }
    }

    public struct Player
    {
        public string name;
        public Packet[] splitedData;
        public Score score;
        public bool ingame;
    }

    public partial class Form1 : Form
    {
        NetComm.Host host;
        double totalTime;
        double pauseTime;
        double ellapsedTime;
        Star star;
        bool serverlaunched;
        bool serverpaused;
        int selectedplanet;

        const byte StarDescrition = 1;
        const byte PlanetDescrition = 2;
        const byte PlanetData = 3;
        const byte LocalMapsDescriptions = 4;
        const byte LocalMapData = 5;
        const byte LocalMapCreate = 6;
        const byte LocalMapAddBuilding = 7;
        const byte LocalMapDestroyBuilding = 8;
        const byte LocalMapSwitchBuilding = 9;
        const byte LocalMapSwitchMaxEnergy = 10;
        const byte LocalMapSwitchExchange = 11;
        const byte LocalMapSwitchResearch = 12;
        const byte StarUnits = 13;
        const byte PlanetMapUnits = 14;
        const byte LocalMapChangeInfoRecipte = 15;
        const byte LocalMapChangeInfoWorkcount = 16;
        const byte LocalMapAddUnitGroup = 17;
        const byte CreditStream = 18;

        const byte StarSyncronize = 19;
        const byte PlanetSyncronize = 20;
        const byte LocalSyncronize = 21;

        const byte Pause = 22;
        const byte UnPause = 23;

        const byte BeginGame = 24;
        const byte CreateRocket = 25;
        const byte LaunchRocket = 26;
        const byte LocalMapBuildingShadows = 27;
        const byte AttackBase = 28;
        const byte MessagesSyncronize = 29;
        const byte Message = 30;

        const byte GetScore = 98;
        const byte GetPlayerID = 99;
        const byte SplitedData = 100;
        const byte SplitedDataPart = 101;

        double oldTime = 0;
        double startTime = 0;
        double timeToMessagesClear = 10;

        Player[] players;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Language.LoadResources("Content/Texts/rus_resources.lang");
            Language.LoadBuildings("Content/Texts/rus_buildings.lang");
            Language.LoadScience("Content/Texts/rus_science.lang");
            Language.LoadMessages("Content/Texts/rus_messages.lang");

            players = new Player[32];
            players[0].name = "Server";
            serverlaunched = false;
            serverpaused = false;
            Random rand = new Random();

            int light_argument = rand.Next(-100, 100);
            float light = (float)Math.Pow(10, 4 * light_argument / 100);

            hScrollBar1.Value = light_argument;

            float temperature_argumemt = (float)(rand.NextDouble() * 7);
            float temperature = (float)(26.389 * Math.Pow(temperature_argumemt, 6) -
                                 550 * Math.Pow(temperature_argumemt, 5) +
                                 4555.6 * Math.Pow(temperature_argumemt, 4) -
                                 19042 * Math.Pow(temperature_argumemt, 3) +
                                 41918 * Math.Pow(temperature_argumemt, 2) -
                                 44408 * temperature_argumemt + 19500);

            hScrollBar2.Value = (int)(temperature_argumemt * 100);

            star = new Star(light_argument / 100.0, light, temperature);
            star.GeneratePlanets((int)numericUpDown1.Value);

            textBox2.Text = star.name;
            numericUpDown1.Value = star.planets_num;

            listBox1.Items.Clear();
            foreach (Planet p in star.planets)
                listBox1.Items.Add(p.name);
            listBox1.SelectedIndex = 0;

            selectedplanet = 0;
            textBox3.Text = star.planets[0].name;
            int axis_argument = (int)(star.planets[0].semimajoraxis * 0.00435 / star.radius * 100);
            if (axis_argument < 25) axis_argument = 25;
            if (axis_argument > 475) axis_argument = 475;
            hScrollBar3.Value = axis_argument;
            int size_argument = (int)(star.planets[0].radius / 100);
            if (size_argument < 20) size_argument = 20;
            if (size_argument > 250) size_argument = 250;
            hScrollBar4.Value = size_argument;

            checkBox1.Checked = star.meteorites;
            checkBox2.Checked = star.sunstrike;
            checkBox3.Checked = star.pirates;

            DrawMap();
        }

        byte[] AppendTypeToData(byte type, byte[] data)
        {
            byte[] neodata = new byte[data.Length + 1];
            neodata[0] = type;
            data.CopyTo(neodata, 1);
            return neodata;
        }
        byte[] AppendTypeToData(byte type, byte id,byte[] data)
        {
            byte[] neodata = new byte[data.Length + 2];
            neodata[0] = type;
            neodata[1] = id;
            data.CopyTo(neodata, 2);
            return neodata;
        }
        byte[] CreateStarDescription()
        {
            return AppendTypeToData(StarDescrition, star.PackedDescription());
        }
        byte[] CreatePlanetsDescription()
        {
            return AppendTypeToData(PlanetDescrition, star.PackedData());
        }
        byte[] CreatePlanetsData(byte id)
        {
            return AppendTypeToData(PlanetData, id, star.planets[id].PackedData());
        }
        byte[] CreateLocalMapsDescription(byte id)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(LocalMapsDescriptions);
            bw.Write(id);
            int maps_count = star.planets[id].maps.Count;
            bw.Write(maps_count);

            for (int i = 0; i < star.planets[id].maps.Count; i++)
                bw.Write(star.planets[id].maps[i].PackedDescription());

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();
            return retbuf;
        }
        byte[] CreateLocalMapData(int planet_id, int base_id)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

            bw.Write(LocalMapData);
            bw.Write(planet_id);
            bw.Write((int)star.planets[planet_id].maps[base_id].position.X);
            bw.Write((int)star.planets[planet_id].maps[base_id].position.Y);

            bw.Write(star.planets[planet_id].maps[base_id].PackedData());

            byte[] membuf = mem.GetBuffer();
            byte[] retbuf = new byte[bw.BaseStream.Position];
            Array.Copy(membuf, retbuf, bw.BaseStream.Position);

            bw.Close();
            mem.Close();
            return retbuf;
        }

        int GetPlayerId(string name)
        {
            for (int i = 0; i < players.Length; i++)
                if (players[i].name != null && players[i].name == name)
                    return i;

            return -1;
        }

        void Server_onConnection(string id)
        {
            console.AppendText(id + " connected!" + Environment.NewLine);
            for (int i = 0; i < players.Length; i++)
                if (players[i].name == null || players[i].name == "")
                {
                    players[i].name = id;
                    playersListBox.Items.Add("[" + i + "]" + id);
                    star.players.Add(new PlayerStation(id, i));
                    break;
                }
        }
        void Server_lostConnection(string id)
        {
            try
            {
                console.AppendText(id + " disconnected" + Environment.NewLine);

                for (int i = 0; i < players.Length; i++)
                    if (players[i].name != null && players[i].name == id)
                    {
                        players[i].name = null;
                        playersListBox.Items.Remove("[" + i + "]" + id);
                        players[i].ingame = false;
                        break;
                    }
            }
            catch { ;}
        }
        void Server_DataReceived(string id, byte[] data)
        {
            #region Get star description
            if (data[0] == StarDescrition)
            {
                console.AppendText("Send star description to " + id + Environment.NewLine);
                host.SendData(id, CreateStarDescription());
            }
            #endregion
            #region Get planet description
            else if (data[0] == PlanetDescrition)
            {
                console.AppendText("Send planets description to " + id + Environment.NewLine);
                host.SendData(id, CreatePlanetsDescription());
            }
            #endregion
            #region Get planet data
            else if (data[0] == PlanetData)
            {
                console.AppendText("Send planet" + data[1] + " data to " + id + Environment.NewLine);
                Packet p = new Packet(CreatePlanetsData(data[1]));
                int _id = GetPlayerId(id);
                players[_id].splitedData = p.Split(host);
                host.SendData(id, players[_id].splitedData[0].packet);
            }
            #endregion
            #region Get maps description
            else if (data[0] == LocalMapsDescriptions)
            {
                console.AppendText("Send maps decriptions from planet" + data[1] + " data to " + id + Environment.NewLine);
                byte[] send_data = CreateLocalMapsDescription(data[1]);

                if (send_data.Length > host.SendBufferSize - 16)
                {
                    Packet p = new Packet(send_data);
                    int _id = GetPlayerId(id);
                    players[_id].splitedData = p.Split(host);
                    host.SendData(id, players[_id].splitedData[0].packet);
                }
                else
                {
                    host.SendData(id, send_data);
                }
            }
            #endregion
            #region Create map
            else if (data[0] == LocalMapCreate)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                short player_id = br.ReadInt16();

                star.planets[planet_id].CreateBase(x, y, player_id);

                br.Close();
                mem.Close();

                data[0] = LocalMapsDescriptions;
                Server_DataReceived(id, data);
            }
            #endregion
            #region Get local map data
            else if (data[0] == LocalMapData)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);

                br.Close();
                mem.Close();

                byte[] send_data = CreateLocalMapData(planet_id,base_id);

                if (send_data.Length > host.SendBufferSize - 16)
                {
                    Packet p = new Packet(send_data);
                    int _id = GetPlayerId(id);
                    players[_id].splitedData = p.Split(host);
                    host.SendData(id, players[_id].splitedData[0].packet);
                }
                else
                {
                    host.SendData(id, send_data);
                }
            }
            #endregion
            #region Build building
            else if (data[0] == LocalMapAddBuilding)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                int bx = br.ReadInt32();
                int by = br.ReadInt32();
                int build_id = br.ReadInt32();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0 && build_id >= 0 && star.planets[planet_id].maps[base_id].TryBuilding(bx, by, build_id))
                {
                    star.planets[planet_id].maps[base_id].AddBuilding(bx, by, build_id);
                    console.AppendText(id + " create building" + Environment.NewLine);
                }
            }
            #endregion
            #region Destroy building
            else if (data[0] == LocalMapDestroyBuilding)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                int bx = br.ReadInt32();
                int by = br.ReadInt32();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0)
                {
                    star.planets[planet_id].maps[base_id].DestroyBuilding(bx, by);
                    console.AppendText(id + " destroy building" + Environment.NewLine);
                }
            }
            #endregion
            #region Energy switch building
            else if (data[0] == LocalMapSwitchBuilding)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                int bx = br.ReadInt32();
                int by = br.ReadInt32();
                int build_id = star.planets[planet_id].maps[base_id].data[by, bx].build_id;

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0 && build_id >= 0)
                {
                    star.planets[planet_id].maps[base_id].buildings[build_id].power = star.planets[planet_id].maps[base_id].buildings[build_id].power == 1 ? 0 : 1;
                    console.AppendText(id + " switch building" + Environment.NewLine);
                }
            }
            #endregion
            #region Energy max energy
            else if (data[0] == LocalMapSwitchMaxEnergy)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                byte b_type = br.ReadByte();
                float value = br.ReadSingle();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0 && b_type >= 0)
                {
                    star.planets[planet_id].maps[base_id].buildingtypeinfo[b_type].maxpower = value;
                    console.AppendText(id + " switch max building power" + Environment.NewLine);
                }
            }
            #endregion
            #region Energy exchange
            else if (data[0] == LocalMapSwitchExchange)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                int index = br.ReadInt32();
                byte mode = br.ReadByte();
                float value = br.ReadSingle();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0 && index >= 0)
                {
                    star.planets[planet_id].maps[base_id].inventory[index].exchangetype = mode;
                    star.planets[planet_id].maps[base_id].inventory[index].exchangecount = value;
                    console.AppendText(id + " switch exchange" + Environment.NewLine);
                }
            }
            #endregion
            #region Energy science
            else if (data[0] == LocalMapSwitchResearch)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                int mode = br.ReadInt32();
                int s_id = br.ReadInt32();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0 && s_id >= 0)
                {
                    star.planets[planet_id].maps[base_id].currentresearchmode = mode;
                    star.planets[planet_id].maps[base_id].selectedresearch[mode] = s_id;
                    console.AppendText(id + " switch science" + Environment.NewLine);
                }
            }
            #endregion
            #region Star units
            else if (data[0] == StarUnits)
            {
                console.AppendText("Send star units to " + id + Environment.NewLine);
                host.SendData(id, AppendTypeToData(StarUnits, star.PackedUnits()));
            }
            #endregion
            #region Planet units
            else if (data[0] == PlanetMapUnits)
            {
                console.AppendText("Send planet units to " + id + Environment.NewLine);
                host.SendData(id, AppendTypeToData(PlanetMapUnits, data[1], star.planets[data[1]].PackedUnits()));
            }
            #endregion
            #region Change recipte
            else if (data[0] == LocalMapChangeInfoRecipte)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                x = br.ReadInt32();
                y = br.ReadInt32();
                int build_id = star.planets[planet_id].maps[base_id].data[y, x].build_id;
                int recipte = br.ReadInt32();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0 && build_id >= 0)
                {
                    star.planets[planet_id].maps[base_id].buildings[build_id].recipte = recipte;
                    console.AppendText(id + " switch recipte" + Environment.NewLine);
                }
            }
            #endregion
            #region Change workcount
            else if (data[0] == LocalMapChangeInfoWorkcount)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                x = br.ReadInt32();
                y = br.ReadInt32();
                int build_id = star.planets[planet_id].maps[base_id].data[y, x].build_id;
                int workcount = br.ReadInt32();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0 && build_id >= 0)
                {
                    star.planets[planet_id].maps[base_id].buildings[build_id].workcount = workcount;
                    console.AppendText(id + " switch workcount" + Environment.NewLine);
                }
            }
            #endregion
            #region Add unitgroups
            else if (data[0] == LocalMapAddUnitGroup)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int count = br.ReadInt32();

                if (planet_id >= 0)
                {
                    for(int i=0;i<count;i++)
                    {
                        UnitGroup ug = new UnitGroup();
                        ug.UnpackedData(br);

                        star.planets[planet_id].unitgroups.Add(ug);
                    }

                    console.AppendText(id + " create unitgroups" + Environment.NewLine);
                }

                br.Close();
                mem.Close();
            }
            #endregion
            #region Creditstream
            else if (data[0] == CreditStream)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                short player_id = br.ReadInt16();
                byte t = br.ReadByte();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0)
                {
                    if (t == 0)
                    {
                        star.TryPayMoney(500, player_id);
                        star.planets[planet_id].maps[base_id].inventory[(int)Resources.credits].count += 500;
                    }
                    else
                    {
                        star.TryAddMoney(500, player_id);
                        star.planets[planet_id].maps[base_id].inventory[(int)Resources.credits].count -= 500;
                    }

                    console.AppendText(id + " change stream" + Environment.NewLine);
                }
            }
            #endregion
            #region CreateRocket
            else if (data[0] == CreateRocket)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                int bx = br.ReadInt32();
                int by = br.ReadInt32();
                int recipte = br.ReadInt32();

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0)
                {
                    int build_id = star.planets[planet_id].maps[base_id].data[by, bx].build_id;
                    if (build_id >= 0)
                    {
                        Map map = star.planets[planet_id].maps[base_id];
                        if (map.inventory[(int)Resources.electronics].count >= Constants.Map_RocketBuildingElectronicPrice &&
                            map.inventory[(int)Resources.energyore].count >= Constants.Map_RocketBuildingEnergyOrePrice &&
                            map.inventory[(int)Resources.metal].count >= Constants.Map_RocketBuildingMetalPrice)
                        {
                            map.inventory[(int)Resources.electronics].count -= Constants.Map_RocketBuildingElectronicPrice;
                            map.inventory[(int)Resources.energyore].count -= Constants.Map_RocketBuildingEnergyOrePrice;
                            map.inventory[(int)Resources.metal].count -= Constants.Map_RocketBuildingMetalPrice;

                            star.planets[planet_id].maps[base_id].buildings[build_id].recipte = recipte;
                            star.planets[planet_id].maps[base_id].buildings[build_id].wait = Constants.Map_RocketBuildingTime;
                            console.AppendText(id + " create rocket" + Environment.NewLine);
                        }
                    }
                }
            }
            #endregion
            #region GetShadows
            else if (data[0] == LocalMapBuildingShadows)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);

                br.Close();
                mem.Close();

                if (planet_id >= 0 && base_id >= 0)
                {
                    host.SendData(id, AppendTypeToData(LocalMapBuildingShadows, star.planets[planet_id].maps[base_id].PacketBuildingsShadows()));
                    console.AppendText(id + " send buildings shadows" + Environment.NewLine);
                }
            }
            #endregion
            #region LaunchRocket
            else if (data[0] == LaunchRocket)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int basex = br.ReadInt32();
                int basey = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(basex, basey);
                int rocketlaunchx = br.ReadInt32();
                int rocketlaunchy = br.ReadInt32();
                int basetargetx = br.ReadInt32();
                int basetargety = br.ReadInt32();
                int rockettargetx = br.ReadInt32();
                int rockettargety = br.ReadInt32();

                br.Close();
                mem.Close();

                if (base_id >= 0)
                {
                    Map map = star.planets[planet_id].maps[base_id];
                    Planet planet = star.planets[planet_id];
                    int selectedbuildid = map.data[rocketlaunchy, rocketlaunchx].build_id;
                    if (selectedbuildid >= 0 && selectedbuildid < map.buildings.Count && map.buildings[selectedbuildid].wait <= 0 && map.buildings[selectedbuildid].type == Building.RocketParking)
                    {
                        int type = 0;
                        switch (map.buildings[selectedbuildid].recipte)
                        {
                            case 1: type = Unit.RocketAtom; break;
                            case 2: type = Unit.RocketNeitron; break;
                            case 3: type = Unit.RocketTwined; break;
                        }
                        map.buildings[selectedbuildid].recipte = 0;

                        if (type != 0)
                        {
                            Unit u = new Unit(type, map.buildings[selectedbuildid].pos.X, map.buildings[selectedbuildid].pos.Y);
                            u.command = new Command(commands.rocketflyup, 100);
                            u.player_id = map.player_id;

                            UnitGroup ug = new UnitGroup();
                            ug.player_id = map.player_id;
                            Unit r = new Unit(type, rockettargetx, rockettargety);
                            r.height = 100;
                            r.command = new Command(commands.rocketfallingdown, 0);
                            ug.units.Add(r);
                            ug.baseposiitionx = basetargetx;
                            ug.baseposiitiony = basetargety;
                            ug.position = map.position;

                            planet.unitgroups.Add(ug);
                            map.units.Add(u);

                            console.AppendText(id + " raunch rocket" + Environment.NewLine);
                        }
                    }
                }
            }
            #endregion
            #region AttackBase
            else if (data[0] == AttackBase)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                int planet_id = br.ReadInt32();
                int startbasex = br.ReadInt32();
                int startbasey = br.ReadInt32();
                int selectbasex = br.ReadInt32();
                int selectbasey = br.ReadInt32();
                int countfighters = br.ReadInt32();
                if (planet_id >= 0)
                {
                    int starttargetbaseitem = star.planets[planet_id].GetBaseId(startbasex, startbasey);
                    int selecttargetbase = star.planets[planet_id].GetBaseId(startbasex, startbasey);
                    if (starttargetbaseitem >= 0 && starttargetbaseitem < star.planets[planet_id].maps.Count &&
                       selecttargetbase >= 0 && selecttargetbase < star.planets[planet_id].maps.Count)
                    {
                        short player_id = star.planets[planet_id].maps[starttargetbaseitem].player_id;

                        br.Close();
                        mem.Close();

                        int realcountfighters = 0;
                        foreach (Unit u in star.planets[planet_id].maps[starttargetbaseitem].units)
                            if (u.type == Unit.AttackDrone && u.command.message == commands.gotoparking)
                                realcountfighters++;
                        countfighters = Math.Min(countfighters, realcountfighters);

                        int k = 0;
                        Microsoft.Xna.Framework.Vector2 target = star.planets[planet_id].maps[starttargetbaseitem].GetRandomBaseDirection();
                        foreach (Unit u in star.planets[planet_id].maps[starttargetbaseitem].units)
                            if (u.type == Unit.AttackDrone && u.command.message == commands.gotoparking)
                            {
                                u.command = new Command(commands.goaway, (int)target.X, (int)target.Y);
                                k++;
                                if (k >= countfighters) break;
                            }

                        UnitGroup ug = new UnitGroup();
                        ug.player_id = player_id;
                        ug.position = star.planets[planet_id].maps[starttargetbaseitem].position;
                        ug.baseposiitionx = selectbasex;
                        ug.baseposiitiony = selectbasey;
                        for (k = 0; k < countfighters; k++)
                        {
                            Unit u = new Unit(Unit.AttackDrone, target.X, target.Y);
                            u.player_id = player_id;
                            u.command = new Command(commands.attackmode, 0);
                            u.motherbasex = (short)startbasex;
                            u.motherbasey = (short)startbasey;
                            ug.units.Add(u);
                        }
                        star.planets[planet_id].unitgroups.Add(ug);

                        console.AppendText(id + " raunch fighters" + Environment.NewLine);
                    }
                }
            }
            #endregion

            #region Sync star
            else if (data[0] == StarSyncronize)
            {
                console.AppendText("Send star sync to " + id + Environment.NewLine);
                byte[] send_data = AppendTypeToData(StarSyncronize, star.PackedUnits());
                if (send_data.Length > host.SendBufferSize - 16)
                {
                    Packet p = new Packet(send_data);
                    int _id = GetPlayerId(id);
                    players[_id].splitedData = p.Split(host);
                    host.SendData(id, players[_id].splitedData[0].packet);
                }
                else
                {
                    host.SendData(id, send_data);
                }
            }
            #endregion
            #region Planet sync
            else if (data[0] == PlanetSyncronize)
            {
                console.AppendText("Send planet sync to " + id + Environment.NewLine);
                byte[] send_data = AppendTypeToData(PlanetSyncronize, data[1], star.planets[data[1]].PackedSync());
                if (send_data.Length > host.SendBufferSize - 16)
                {
                    Packet p = new Packet(send_data);
                    int _id = GetPlayerId(id);
                    players[_id].splitedData = p.Split(host);
                    host.SendData(id, players[_id].splitedData[0].packet);
                }
                else
                {
                    host.SendData(id, send_data);
                }
            }
            #endregion
            #region Map sync
            else if (data[0] == LocalSyncronize)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                byte planet_id = br.ReadByte();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);

                br.Close();
                mem.Close();

                if (base_id >= 0 && planet_id >= 0)
                {
                    console.AppendText("Send local sync to " + id + Environment.NewLine);
                    byte[] data2 = star.planets[data[1]].maps[base_id].PackedSync();
                    byte[] send_data = new byte[data2.Length + data.Length];
                    data.CopyTo(send_data, 0);
                    data2.CopyTo(send_data, data.Length);

                    if (send_data.Length > host.SendBufferSize - 16)
                    {
                        Packet p = new Packet(send_data);
                        int _id = GetPlayerId(id);
                        players[_id].splitedData = p.Split(host);
                        host.SendData(id, players[_id].splitedData[0].packet);
                    }
                    else
                    {
                        host.SendData(id, send_data);
                    }
                }
            }
            #endregion
            #region Message sync
            else if (data[0] == MessagesSyncronize)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                br.ReadByte();
                short player_id = br.ReadInt16();

                br.Close();
                mem.Close();

                if (player_id >= 0)
                {
                    System.IO.MemoryStream mem2 = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem2);

                    bw.Write(MessagesSyncronize);
                    List<string> messages = new List<string>();

                    foreach (Planet p in star.planets)
                        foreach (Map m in p.maps)
                        {
                            if (m.player_id == player_id && m.messages != null)
                            {
                                foreach (string s in m.messages)
                                {
                                    messages.Add(m.name + ":" + s);
                                }
                                m.messages.Clear();
                            }
                        }

                    bw.Write(messages.Count);
                    foreach (string s in messages)
                        bw.Write(s);

                    byte[] membuf = mem2.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem2.Close();

                    if (retbuf.Length > host.SendBufferSize - 16)
                    {
                        Packet p = new Packet(retbuf);
                        int _id = GetPlayerId(id);
                        players[_id].splitedData = p.Split(host);
                        host.SendData(id, players[_id].splitedData[0].packet);
                    }
                    else
                    {
                        host.SendData(id, retbuf);
                    }
                }
            }
            #endregion

            #region PauseAdnUnpause
            else if (data[0] == Pause)
            {
                serverpaused = true;

                console.AppendText("Server pause.\n");
                button5.Text = "Продолжить";
                label1.Text = "Сервер приостановлен";

                pauseTime = DateTime.Now.TimeOfDay.TotalSeconds;

                host.Brodcast(new byte[] { Pause });
            }
            else if (data[0] == UnPause)
            {
                serverpaused = false;

                console.AppendText("Server unpause.\n");
                button5.Text = "Пауза";

                startTime += DateTime.Now.TimeOfDay.TotalSeconds - pauseTime;

                host.Brodcast(new byte[] { UnPause });
            }
            #endregion
            #region Message
            else if (data[0] == Message)
            {
                host.Brodcast(data);
            }
            #endregion

            #region GetID
            else if (data[0] == GetPlayerID)
            {
                for (int i = 0; i < players.Length; i++)
                    if (players[i].name != null && players[i].name == id)
                    {
                        System.IO.MemoryStream mem = new System.IO.MemoryStream();
                        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                        bw.Write(GetPlayerID);
                        bw.Write((short)i);

                        host.SendData(id, mem.ToArray());

                        bw.Close();
                        mem.Close();

                        break;
                    }
            }
            #endregion
            #region Process splited and else
            else if (data[0] == SplitedDataPart)
            {
                ushort spritedpart = (ushort)(data[1] * 256 + data[2]);
                console.AppendText("Send splited part #" + spritedpart + " to " + id + Environment.NewLine);
                int _id = GetPlayerId(id);

                byte[] senddata = new byte[players[_id].splitedData[spritedpart + 1].packet.Length + 1];
                senddata[0] = SplitedDataPart;

                Array.Copy(players[_id].splitedData[spritedpart + 1].packet, 0, senddata, 1, players[_id].splitedData[spritedpart + 1].packet.Length);

                host.SendData(id, senddata);
            }
            else console.AppendText("Uncnown command from " + id + Environment.NewLine);
            #endregion
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(serverlaunched)
                host.CloseConnection();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (serverlaunched)
            {
                if (!serverpaused)
                {
                    ellapsedTime = totalTime - oldTime;

                    star.Update(ellapsedTime, totalTime);

                    for (int i = 1; i < players.Length; i++)
                    {
                        if (players[i].name != null && players[i].name != "")
                        {
                            players[i].score = star.GetScore(i);
                        }
                    }

                    for (int i = 1; i < players.Length; i++)
                    {
                        if (players[i].name != null && players[i].name != "" && players[i].score.Total >= 100000)
                        {
                            byte[] data = players[i].score.PackedName(players[i].name);

                            for (int k = 1; k < players.Length; k++)
                            {
                                host.SendData(players[i].name, data);
                            }

                            break;
                        }
                    }

                    label1.Text = "Игровое время: " + totalTime.ToString("0.0");

                    oldTime = totalTime;
                    totalTime = DateTime.Now.TimeOfDay.TotalSeconds - startTime;
                    timeToMessagesClear-=ellapsedTime;

                    if (timeToMessagesClear <= 0)
                    {
                        foreach (Planet p in star.planets)
                            foreach (Map m in p.maps)
                                if (m.messages != null)
                                    m.messages.Clear();
                        timeToMessagesClear = 10;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            star.name = NameGenerator.GenerateStarName();
            textBox2.Text = star.name;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            star.planets[listBox1.SelectedIndex].name = NameGenerator.GeneratePlanetName();
            listBox1.Items[listBox1.SelectedIndex] = star.planets[listBox1.SelectedIndex].name;
            textBox3.Text = star.planets[listBox1.SelectedIndex].name;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                selectedplanet = listBox1.SelectedIndex;
                textBox3.Text = star.planets[selectedplanet].name;

                int axis_argument = (int)(star.planets[selectedplanet].semimajoraxis * 0.00435 / star.radius * 100);
                if (axis_argument < 25) axis_argument = 25;
                if (axis_argument > 475) axis_argument = 475;
                hScrollBar3.Value = axis_argument;
                int size_argument = (int)(star.planets[selectedplanet].radius / 100);
                if (size_argument < 20) size_argument = 20;
                if (size_argument > 250) size_argument = 250;
                hScrollBar4.Value = size_argument;

                DrawMap();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int num = star.planets_num;
            Planet[] pl = star.planets;

            star.planets_num = (int)numericUpDown1.Value;
            star.planets = new Planet[star.planets_num];
            //star.GeneratePlanets((int)numericUpDown1.Value);

            for (int i = 0; i < star.planets_num; i++)
            {
                if (i < num)
                    star.planets[i] = pl[i];
                else
                {
                    star.planets[i] = new Planet(star);
                    star.planets[i].id = i;
                }
            }

            listBox1.Items.Clear();
            foreach (Planet p in star.planets)
                listBox1.Items.Add(p.name);

            if (selectedplanet >= star.planets_num)
            {
                selectedplanet--;
                textBox3.Text = star.planets[selectedplanet].name;
                int axis_argument = (int)(star.planets[selectedplanet].semimajoraxis * 0.00435 / star.radius * 100);
                if (axis_argument < 25) axis_argument = 25;
                if (axis_argument > 475) axis_argument = 475;
                hScrollBar3.Value = axis_argument;
                int size_argument = (int)(star.planets[selectedplanet].radius / 100);
                hScrollBar4.Value = size_argument;
            }
            listBox1.SelectedIndex = selectedplanet;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            float old = (float)star.radius;

            float light = (float)Math.Pow(10, 4 * hScrollBar1.Value / 100.0f);
            star.radius = light * 0.00435;
            star.mass = 960000000000 * 4 / 3 * Math.PI * Math.Pow(star.radius, 3);
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            float temperature_argumemt = hScrollBar2.Value / 100.0f;
            star.temperature = (float)(26.389 * Math.Pow(temperature_argumemt, 6) -
                                 550 * Math.Pow(temperature_argumemt, 5) +
                                 4555.6 * Math.Pow(temperature_argumemt, 4) -
                                 19042 * Math.Pow(temperature_argumemt, 3) +
                                 41918 * Math.Pow(temperature_argumemt, 2) -
                                 44408 * temperature_argumemt + 19500);
        }

        private void hScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            star.planets[selectedplanet].radius = hScrollBar4.Value * 100;
            star.planets[selectedplanet].mass = (float)(4f / 3f * Math.PI * Math.Pow(star.planets[selectedplanet].radius / 6371f, 3)) *
                   (float)(0.17);
        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            star.planets[selectedplanet].semimajoraxis = Math.Abs((float)(star.radius / 0.00435 * hScrollBar3.Value / 100));
            star.planets[selectedplanet].semiminoraxis = star.planets[selectedplanet].semimajoraxis / 100f * 90;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            star.planets[selectedplanet].GenerateMap();

            DrawMap();
        }

        void DrawMap()
        {
            int width = star.planets[selectedplanet].mapwidth;
            int height = star.planets[selectedplanet].mapheight;
            float[,] map = star.planets[selectedplanet].heightmap;

            Bitmap image = new Bitmap(width/2, height);

            System.Drawing.Imaging.BitmapData data = image.LockBits(new Rectangle(0, 0, width/2, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            unsafe
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width/2; j++)
                    {
                        int gray = (int)(255 * ((map[i, j] + 1) / 2));
                        gray = (gray / 16) * 16;
                        *(((int*)data.Scan0) + i * height + j) = Color.FromArgb(255, gray, gray, gray).ToArgb();
                    }
            }

            image.UnlockBits(data);

            pictureBox1.BackgroundImage = image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serverlaunched = !serverlaunched;

            if (serverlaunched)
            {
                console.AppendText("Star system created.\n");
                console.AppendText("\tradius - " + (star.radius / 0.00435).ToString("0.00000") + " \n");
                console.AppendText("\ttemp - " + star.temperature.ToString("0") + " \n");

                startTime = DateTime.Now.TimeOfDay.TotalSeconds;

                host = new Host(3666);
                //host.SendBufferSize = 80000;

                //host.SendBufferSize = 16000;
                //host.ReceiveBufferSize = 16000;

                host.onConnection += new NetComm.Host.onConnectionEventHandler(Server_onConnection);
                host.lostConnection += new NetComm.Host.lostConnectionEventHandler
                            (Server_lostConnection);
                host.DataReceived += new NetComm.Host.DataReceivedEventHandler(Server_DataReceived);

                host.StartConnection();

                console.AppendText("Server started.\n");

                button2.Text = "Остановить сервер";
                tabControl1.SelectTab(1);
            }
            else
            {
                host.CloseConnection();
                console.AppendText("Server stoped.\n");

                button2.Text = "Запустить сервер";
                label1.Text = "Сервер не запущен";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            serverpaused = !serverpaused;

            if (serverpaused)
            {
                console.AppendText("Server pause.\n");
                button5.Text = "Продолжить";
                label1.Text = "Сервер приостановлен";

                pauseTime = DateTime.Now.TimeOfDay.TotalSeconds;

                host.Brodcast(new byte[] { Pause });
            }
            else
            {
                console.AppendText("Server unpause.\n");
                button5.Text = "Пауза";

                startTime += DateTime.Now.TimeOfDay.TotalSeconds - pauseTime;

                host.Brodcast(new byte[] { UnPause });
            }
        }

        private void playersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int num = int.Parse(((string)playersListBox.SelectedItem).Split(new char[] { '[', ']' })[1]);
                numericUpDown2.Value = num;

                numericUpDown2.Visible = true; label10.Visible = true;
            }
            catch { numericUpDown2.Visible = false; label10.Visible = false; }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string[] strs = ((string)playersListBox.SelectedItem).Split(new char[] { '[', ']' });
                int num = int.Parse(strs[1]);

                Player p = players[num];
                players[num].name = null;
                int dx = (int)numericUpDown2.Value - num;

                while (players[(int)numericUpDown2.Value].name != null && players[(int)numericUpDown2.Value].name != "")
                {
                    if ((int)numericUpDown2.Value == num) break;
                    numericUpDown2.Value += dx;
                }

                players[(int)numericUpDown2.Value] = p;
                playersListBox.Items[playersListBox.SelectedIndex] = "[" + (int)numericUpDown2.Value + "]" + strs[2];

                for (int i = 0; i < star.players.Count; i++)
                    if (star.players[i].id == num) star.players[i].id = (int)numericUpDown2.Value;

                foreach (Planet pl in star.planets)
                {
                    foreach (Map m in pl.maps)
                    {
                        if (m.player_id == num)
                            m.player_id = (short)numericUpDown2.Value;

                        foreach (Unit u in m.units)
                            if (u.player_id == num)
                                u.player_id = (short)numericUpDown2.Value;
                    }

                    foreach (UnitGroup g in pl.unitgroups)
                    {
                        if (g.player_id == num)
                            g.player_id = (short)numericUpDown2.Value;

                        foreach (Unit u in g.units)
                            if (u.player_id == num)
                                u.player_id = (short)numericUpDown2.Value;
                    }
                }

                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                bw.Write(GetPlayerID);
                bw.Write((short)numericUpDown2.Value);

                host.SendData(strs[2], mem.ToArray());

                bw.Close();
                mem.Close();
            }
            catch { ;}
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveGame(saveFileDialog1.FileName);
                console.AppendText("Game saved: " + saveFileDialog1.FileName);
            }
        }

        void SaveGame(string filename)
        {
            System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Create);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);

            bw.Write((int)4);   //version
            byte[] sdc = star.PackedDescription();
            byte[] sd = star.PackedData();
            bw.Write(sdc.Length);
            bw.Write(sdc);
            bw.Write(sd.Length);
            bw.Write(sd);
            foreach (Planet p in star.planets)
            {
                byte[] pd = p.PackedData();
                bw.Write(pd.Length);
                bw.Write(pd);
                foreach (Map m in p.maps)
                {
                    byte[] md = m.PackedData();
                    bw.Write(md.Length);
                    bw.Write(md);
                }
            }

            bw.Write(players.Length);
            foreach (Player p in players)
            {
                if (p.name != null) bw.Write(p.name);
                else bw.Write("");
            }

            bw.Close();
            fs.Close();
        }
        void LoadGame(string filename)
        {
            System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Open);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);

            int ver = br.ReadInt32();
            int sdcl = br.ReadInt32();
            byte[] sdc = br.ReadBytes(sdcl);
            star = new Star(sdc);
            int sdl = br.ReadInt32();
            byte[] sd = br.ReadBytes(sdl);
            star.UnpackedData(sd);

            foreach (Planet p in star.planets)
            {
                int pdl = br.ReadInt32();
                byte[] pd = br.ReadBytes(pdl);
                p.UnpackedData(pd);
                foreach (Map m in p.maps)
                {
                    int mdl = br.ReadInt32();
                    byte[] md = br.ReadBytes(mdl);
                    m.UnpackedData(md);
                    m.planet = p;
                }
            }

            int plcount = br.ReadInt32();
            players = new Player[plcount];
            for (int i = 0; i < plcount; i++)
            {
                players[i].name = br.ReadString();
            }

            br.Close();
            fs.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadGame(openFileDialog1.FileName);
                console.AppendText("Game loaded: " + openFileDialog1.FileName);

                string[] connected = new string[playersListBox.Items.Count];
                for (int i = 0; i < playersListBox.Items.Count; i++)
                {
                    connected[i] = (string)playersListBox.Items[i];
                }

                string[] pls = new string[players.Length-1];
                for (int i = 1; i < players.Length; i++)
                {
                    pls[i-1] = "[" + i + "]" + players[i].name;
                }

                Form2 seter = new Form2(connected, pls);
                if (seter.ShowDialog(this) == DialogResult.OK)
                {
                    connected = seter.GetResault();

                    foreach (string s in connected)
                    {
                        string[] subs = s.Split(new char[] { '[', ']' });

                        System.IO.MemoryStream mem = new System.IO.MemoryStream();
                        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                        bw.Write(GetPlayerID);
                        bw.Write(short.Parse(subs[1]));

                        host.SendData(subs[2], mem.ToArray());

                        bw.Close();
                        mem.Close();
                    }

                    playersListBox.Items.Clear();
                    playersListBox.Items.AddRange(connected);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            star.meteorites = checkBox1.Checked;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            star.sunstrike = checkBox2.Checked;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            star.pirates = checkBox3.Checked;
        }
    }
}
