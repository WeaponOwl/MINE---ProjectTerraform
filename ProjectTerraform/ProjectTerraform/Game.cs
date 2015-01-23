using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using NetComm;

namespace ProjectTerraform
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        const int MainMenuMode = 99;
        const int StartGameMenuMode = 98;
        const int SettingsMenuMode = 97;
        const int HelpMenuMode = 96;
        const int AboutMenuMode = 95;

        const int UniverceMode = 94;
        const int CatalogMode = 93;
        const int LoadMode = 92;

        const int ChronoMenuMode = 91;
        const int PromoMenuMode = 90;
        const int UserMode = 89;

        const int ConnectedMode = 100;
        const int DisconnectedMode = 101;
        const int StarOverviewMode = 102;
        const int PlanetOverviewMode = 103;
        const int PlanetMapMode = 104;
        const int LocalMapMode = 105;
        const int ScoreMode = 106;

        const int ReviewMode = 0;
        const int EnergyLocalMode = 2;
        const int DestroyLocalMode = 1;
        const int BuildLocalMode = 3;
        const int CreateBaseMode = 3;
        const int SelectBaseMode = 2;
        const int MarketBaseMode = 1;
        const int AttackBaseMode = 4;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Texture2D guiset;
        Texture2D tileset;
        Texture2D tilesettemplate;
        Texture2D buildset;
        Texture2D unitset;
        Texture2D minimap;
        Texture2D minimaprocket;
        Texture2D buildingparticlesset;
        Texture2D resourceset;
        Texture2D shieldset;
        Texture2D meteoriteset;
        Texture2D explotionset;
        Texture2D starset;
        Texture2D universetexture;
        Texture2D baseset;
        Texture2D fractionset;

        Texture2D planettexture;
        Texture2D planetnormal;

        Texture2D bigstar;
        Texture2D tuneltexture;

        RenderTarget2D powershieldslayer;
        RenderTarget2D emmisionshieldslayer;
        RenderTarget2D atmoshereshieldslayer;
        RenderTarget2D screencapture;

        Camera camera;
        Map map;
        MapHelper mapHelper;
        Univerce univerce;
        Star star;
        Planet planet;
        Gui gui;

        int localmapmode;
        int planetmode;
        int state;
        int state_old;
        int build_id;
        int startexchangeitem;
        int selectedbuildid;
        int startresearchitem;
        int startlaunchrocketitem;
        int launchrocketx;
        int launchrockety;
        int selectrocketbase;
        int startmedulesitem;
        int selectedbase;
        int selectedplanet;
        int selectedstar;
        int selectedsector;
        int selectedloadid;
        int starthelpstring;
        int startpromostring;
        int selectedmarketbase;
        int selectedmarketid;
        int selecttargetbase;
        int starttargetbaseitem;
        int countfighters;

        float catalogstarlighting;
        float catalogstartemperature;
        float catalogplanetradius;
        float catalogplanetsemiaxis;
        bool catalogsunstrikes, catalogmeteorites, catalogpirates;

        int[,] tradegrouptypes;
        int[,] tradegroupcount;

        int width;
        int height;
        bool fullscreen;
        int newwidth;
        int newheight;
        bool newfullscreen;
        bool music;
        bool autosave;
        bool messages;
        float musicvolume;
        Point[] displaymodes;
        int selecteddisplaymode;
        bool needresetgui;

        int nextautosavetime;
        //float scriptwaittime;
        float nextparticletime;

        MessageSystem messagesystem;
        KeyboardState oldkeyboardstate;
        bool createnewsaydialog = true;
        string[] filenames; 
        string[] fileinfos;

        int newbaseposition_x, newbaseposition_y;
        int selectedbaseposition_x, selectedbaseposition_y;
        int selectedbaseposition2_x, selectedbaseposition2_y;

        float timetonextautosave;

        List<Song> songs;

        Sphere planetSphere;
        Effect planetEffect;
        Effect mapEffect;
        Effect baselineShader;
        Effect starShader;
        Effect tileEffect;
        Effect fractiontileEffect;
        Effect tunelEffect;

        Color darkness;

        Gradient starGradient;
        Gradient[] planetGradient;

        NetComm.Client client;
        bool offlineMode;
        string hostIP;
        bool pause;

        byte[] splitedData;
        int packets_num;
        ushort currentpart;
        long currentlength;
        float disconect_time;
        //float nexstartime; // used in promo
        bool drawgui;
        bool loading;

        GameTime currentGameTime;

        short player_id = -1;
        string playername;
        int playerlanguage=0;
        const int LanguageEng = 0;
        const int LanguageRus = 1;

        float timetosyncronize;

        VertexPositionColorTexture[] terrainvertexes;
        short[] terrainindexes;

        VertexPositionColorTexture[] subterrainvertexes;
        short[] subterrainindexes;

        Script[] scriptes;
        Color[] fractioncolors;

        void LoadLanguage()
        {
            if (playerlanguage == 1)
            {
                LanguageHelper.Load("Content/Texts/rus_gui.lang");
                Language.LoadResources("Content/Texts/rus_resources.lang");
                Language.LoadBuildings("Content/Texts/rus_buildings.lang");
                Language.LoadScience("Content/Texts/rus_science.lang");
                Language.LoadMessages("Content/Texts/rus_messages.lang");
                Helper.LoadAbout("Content/Texts/rus_about.lang");
                Helper.LoadChrono("Content/Texts/rus_chrono.lang");
                Helper.LoadHelp("Content/Texts/rus_help.lang");
                Helper.LoadPromo("Content/Texts/rus_promo.lang");
            }
            else
            {
                LanguageHelper.Load("Content/Texts/eng_gui.lang");
                Language.LoadResources("Content/Texts/eng_resources.lang");
                Language.LoadBuildings("Content/Texts/eng_buildings.lang");
                Language.LoadScience("Content/Texts/eng_science.lang");
                Language.LoadMessages("Content/Texts/eng_messages.lang");
                Helper.LoadAbout("Content/Texts/eng_about.lang");
                Helper.LoadChrono("Content/Texts/eng_chrono.lang");
                Helper.LoadHelp("Content/Texts/eng_help.lang");
                Helper.LoadPromo("Content/Texts/eng_promo.lang");
            }
        }
        public Game()
        {
            drawgui = true;
            musicvolume = 1;
            starthelpstring = 0;
            nextautosavetime = 120;
            needresetgui = false;

            darkness = Color.Transparent;

            //timetonextautosave = Constants.Game_nextautosavetime;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            localmapmode = ReviewMode;

            messagesystem = new MessageSystem();

            state = MainMenuMode;
            build_id = 0;
            startexchangeitem = 0;
            selectedbuildid = 0;
            selectedbase = -1;
            selectedloadid = 0;

            catalogstarlighting = 0.8f;
            catalogstartemperature = 0.7f;
            catalogplanetradius = 0.4f;
            catalogplanetsemiaxis = 0.6f;

            pause = false;
            offlineMode = true;
            player_id = -1;

            if (LoadConfig())
            {
                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
                graphics.IsFullScreen = fullscreen;
                graphics.ApplyChanges();
            }
            else
            {
                playername = "Recruit";
                width = 800;
                height = 600;
                fullscreen = false;
                music = true;
                musicvolume = 1;
                autosave = true;
                messages = true;
                player_id = 0;
                playerlanguage = 0;

                SaveConfig();

                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
                graphics.IsFullScreen = fullscreen;
                graphics.ApplyChanges();
            }
            LoadLanguage();
        }

        #region Multiplayer processing
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

        const float SyncronizationRate = 2;

        bool normalexit = false;

        void client_Connected()
        {
            messagesystem.AddMessage("Connected", 5);
            state = ConnectedMode;
            client.SendData(new byte[1] { GetPlayerID });
            disconect_time = 300;
        }
        void client_Disconnected()
        {
            messagesystem.AddMessage("Disconnected", 4);
            state = DisconnectedMode;
            if (normalexit)
            {
                normalexit = false;
                state = MainMenuMode;
            }
        }
        void client_DataReceived(byte[] data, string id)
        {
            if (data[0] == GetPlayerID)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
                br.ReadByte();
                player_id = br.ReadInt16();
                br.Close();
                ms.Close();

                messagesystem.AddMessage(LanguageHelper.Message_IDGeted + ": " + player_id, 2);

                client.SendData(new byte[1] { StarDescrition });
            }
            if (data[0] == StarDescrition || data[0] == PlanetDescrition || data[0] == LocalMapData || data[0] == StarUnits || data[0] == StarSyncronize)
            {
                byte[] neodata = new byte[data.Length - 1];
                Array.Copy(data, 1, neodata, 0, data.Length - 1);
                ProcessData(data[0], 0, neodata);

                timetosyncronize = SyncronizationRate;
            }
            else if (data[0] == PlanetData || data[0] == LocalMapsDescriptions || data[0] == PlanetMapUnits || data[0] == PlanetSyncronize || data[0] == LocalSyncronize||data[0]==LocalMapBuildingShadows)
            {
                byte[] neodata = new byte[data.Length - 2];
                Array.Copy(data, 2, neodata, 0, data.Length - 2);
                ProcessData(data[0], data[1], neodata);

                timetosyncronize = SyncronizationRate;
            }
            else if (data[0] == SplitedData)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
                br.ReadByte();
                packets_num = br.ReadInt32();
                int data_l = br.ReadInt32();
                currentpart = 0;
                currentlength = 0;
                splitedData = new byte[data_l];
                br.Close();
                ms.Close();
                client.SendData(new byte[3] { SplitedDataPart, (byte)(currentpart / 256), (byte)(currentpart % 256) });

                messagesystem.AddMessage(LanguageHelper.Message_HaveSplited + " " + packets_num + " " + LanguageHelper.Message_Parts, 3);
            }
            else if (data[0] == SplitedDataPart)
            {
                if (splitedData != null)
                {
                    Array.Copy(data, 1, splitedData, currentlength, data.Length - 1);
                    currentlength += data.Length - 1;
                    currentpart++;
                    //packets_num--;

                    if (packets_num <= currentpart)
                    {
                        client_DataReceived(splitedData, "");
                        packets_num = 0;
                    }
                    else
                        client.SendData(new byte[3] { SplitedDataPart, (byte)(currentpart / 256), (byte)(currentpart % 256) });
                }
            }
            else if (data[0] == Pause)
            {
                pause = true;
                gui.popupformid = -1;
                gui.AddPopup(new Form(0, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark, (width - 400) / 2, (height - 100) / 2, 400, 100, null), new GuiObject(GuiObjectState.Layer, (width - 400) / 2, (height - 100) / 2, 400, 100, null, "Приостановлено") }));
            }
            else if (data[0] == UnPause)
            {
                pause = false;
                if (gui.popupformid == -1)
                    gui.ClosePopup();
            }
            else if (data[0] == MessagesSyncronize)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);

                br.ReadByte();
                int count = br.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    string str = br.ReadString();
                    messagesystem.AddMessage(str, 3);
                }

                br.Close();
                ms.Close();
            }
            else if (data[0] == Message)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);

                br.ReadByte();
                string text = br.ReadString();
                messagesystem.AddMessage(text, 10);

                br.Close();
                ms.Close();
            }
        }

        void ProcessData(byte command,byte id,byte[] data)
        {
            if (command == StarDescrition)
            {
                star = new Star(data);
                state = StarOverviewMode;
                client.SendData(new byte[1] { PlanetDescrition });
                scriptes = null;
            }
            else if (command == PlanetDescrition)
            {
                if (star != null)
                {
                    star.UnpackedData(data);
                    state = StarOverviewMode;
                    client.SendData(new byte[1] { StarUnits });
                }
            }
            else if (command == StarUnits || command == StarSyncronize)
            {
                if (star != null)
                {
                    star.UnpackedUnits(data);
                    state = StarOverviewMode;
                }
            }
            else if (command == PlanetData)
            {
                if (star != null && star.planets != null && star.planets[id] != null)
                {
                    DrawLoadScreen();

                    star.planets[id].UnpackedData(data);
                    //star.planets[id] = new Planet(star);
                    //messagesystem.AddMessage(MessageType.none, -1, "Have planet " + id + " data", 10, Color.White);
                    state = PlanetOverviewMode;

                    planet = star.planets[id];
                    planet.star = star;

                    CreatePlanetTexture(planet);
                    gui.forms[9].Ready();
                }
            }
            else if (command == LocalMapsDescriptions)
            {
                if (star != null && star.planets != null && star.planets[id] != null)
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                    System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                    int maps_count = br.ReadInt32();

                    star.planets[id].maps = new List<Map>();// (maps_count);
                    for (int i = 0; i < maps_count; i++)
                    {
                        Map map = new Map();
                        map.UnpackedDescription(br);
                        map.planet = star.planets[id];
                        star.planets[id].maps.Add(map);
                    }

                    mem.Close();
                    br.Close();

                    state = PlanetMapMode;
                    gui.forms[11].Ready();

                    client.SendData(new byte[2] { PlanetMapUnits, id });
                }
            }
            else if (command == PlanetMapUnits)
            {
                if (star != null)
                {
                    star.planets[id].UnpackedUnits(data);
                    state = PlanetMapMode;
                }
            }
            else if (command == LocalMapData)
            {
                DrawLoadScreen();

                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                int planet_id = br.ReadInt32();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int base_id = star.planets[planet_id].GetBaseId(x, y);
                if (base_id >= 0)
                {
                    planet = star.planets[planet_id];
                    map = star.planets[planet_id].maps[base_id];
                    map.UnpackedData(br);
                    if (map.planet == null) map.planet = planet;

                    mapHelper = new MapHelper(map);
                    selectedbase = id;
                    CreateLocalMapMinimapAndTexture(planet, map);

                    state = LocalMapMode;
                    planetmode = ReviewMode;

                    gui.forms[12].Ready();
                }

                mem.Close();
                br.Close();
            }
            else if (command == PlanetSyncronize)
            {
                star.planets[id].UnpackedSync(data);
            }
            else if (command == LocalSyncronize)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                //br.ReadByte(); br.ReadByte();
                int x = br.ReadInt32();
                int y = br.ReadInt32();
                int bid = star.planets[id].GetBaseId(x, y);
                if (bid >= 0)
                    star.planets[id].maps[bid].UnpackedSync(br);

                mem.Close();
                br.Close();
            }
            else if (command == LocalMapBuildingShadows)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                System.IO.BinaryReader br = new System.IO.BinaryReader(mem);

                byte[] bdata = br.ReadBytes(64 * 8);
                if (minimaprocket != null)
                {
                    bool[] buildings = planet.maps[selectrocketbase].UnpackedBuildingsShadows(bdata);
                    Color[] colors = new Color[64 * 64];
                    minimaprocket.GetData<Color>(colors);
                    for (int i = 0; i < 64 * 64; i++) if (buildings[i]) colors[i] = new Color(67, 73, 69);
                    minimaprocket.SetData<Color>(colors);
                }

                mem.Close();
                br.Close();
            }
        }

        #endregion

        void SetMenuState(int state)
        {
            switch(state)
            {
                case 0:gui.forms[1].elements[6].enable = false;
                       gui.forms[1].elements[7].enable = false;
                       gui.forms[1].elements[8].enable = false;
                       gui.forms[1].elements[9].enable = false;

                       gui.forms[1].elements[1].enable = true;
                       gui.forms[1].elements[5].enable = true;

                       gui.forms[1].elements[10].enable = true;
                       gui.forms[1].elements[11].enable = true;
                       gui.forms[1].elements[12].enable = true;
                       gui.forms[1].elements[13].enable = true;
                       break;
                case 1:gui.forms[1].elements[6].enable = true;
                       gui.forms[1].elements[7].enable = true;
                       gui.forms[1].elements[8].enable = true;
                       gui.forms[1].elements[9].enable = true;

                       gui.forms[1].elements[1].enable = false;
                       gui.forms[1].elements[5].enable = false;

                       gui.forms[1].elements[10].enable = false;
                       gui.forms[1].elements[11].enable = false;
                       gui.forms[1].elements[12].enable = false;
                       gui.forms[1].elements[13].enable = false;
                       break;
            }
        }
        void InitGui()
        {
            int startx = (width - 200) / 2;
            int starty = 240;
            int sizey = 30;

            int panelsizex = 557 + 16 + 17;
            int panelstartx = (width - 200 - panelsizex) / 2;
            int panelstarty = (height - 392) / 2;

            //0 - select planet
            gui.forms.Add(new Form(StarOverviewMode, new GuiObject[] { new GuiObject(GuiObjectState.Layer, 0, 0, width, height, StarOverviewModeMain) }));

            //1 - main menu
            gui.forms.Add(new Form(MainMenuMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,-10,220,height+20,null),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty,200,sizey,MainToGameMenuButton,LanguageHelper.Gui_Play),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35,200,sizey,MainToSettingsMenuButton,LanguageHelper.Gui_Settings),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*2,200,sizey,MainToHelpMenuButton,LanguageHelper.Gui_Help),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*3,200,sizey,MainToAboutMenuButton,LanguageHelper.Gui_About),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*4,200,sizey,ExitGameButton,LanguageHelper.Gui_Exit),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*4,200,sizey,LeaveMissionButton,LanguageHelper.Gui_LeaveMission),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty,200,sizey,BackGameMenuButton,LanguageHelper.Gui_BackToGame),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty-40,100,sizey,SaveButton,LanguageHelper.Gui_Save),
                                                             new GuiObject(GuiObjectState.MenuButton,startx+100,starty-40,100,sizey,LoadButton,LanguageHelper.Gui_Load),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty-100,200,sizey,MainChangePlayerNameButton,playername+" - "+LanguageHelper.Gui_ChangeNickname),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty-100-40,30,sizey,MainChangePlayerColorButton1,"<"),
                                                             new GuiObject(GuiObjectState.MenuButton,startx+170,starty-100-40,30,sizey,MainChangePlayerColorButton2,">"),
                                                             new GuiObject(GuiObjectState.DarkLayer,startx+30,starty-100-40,140,sizey,null,LanguageHelper.Gui_ChangeColor),}));
            SetMenuState(0);

            //2 - start game
            gui.forms.Add(new Form(StartGameMenuMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,-10,220,height+20,null),
                                                             new GuiObject(GuiObjectState.PanelDark,startx-10+230,starty-10,220,90,null),
                                                             new GuiObject(GuiObjectState.MenuButton,startx+230,starty,200,sizey,GoMission0Button,LanguageHelper.Gui_Tutorial),
                                                             new GuiObject(GuiObjectState.MenuButton,startx+230,starty+35,200,sizey,GoMission1Button,LanguageHelper.Gui_Mission+" 1"),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty,200,sizey,GameMenuModeToUniverseModeButton,LanguageHelper.Gui_SmallSpace),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35,200,sizey,GameMenuModeToCatalogModeButton,LanguageHelper.Gui_CalculateSystem),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*2+10,200,sizey,GameMenuModeToLoadModeButton,LanguageHelper.Gui_Load),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*3+20,200,sizey,GameMenuModeToMainMenuModeButton,LanguageHelper.Gui_Back),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty-35-10,200,sizey,GameMenuModeToConnectModeButton,LanguageHelper.Gui_Multyplayer)}));

            //3 - settings
            LoadResolutions();
            selecteddisplaymode = 0;
            for (int i = 0; i < displaymodes.Length; i++)
            {
                if (displaymodes[i].X == width && displaymodes[i].Y == height) selecteddisplaymode = i;
            }
                gui.forms.Add(new Form(SettingsMenuMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,-10,220,height+20,null),
                                                             new GuiObject(GuiObjectState.PanelDark,startx-110,starty-20-35*4-5,420,sizey+35*10+25,null),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty,200,sizey,SettingsMenuModeMusicButton,LanguageHelper.Gui_Music+": "+(music?LanguageHelper.Gui_on:LanguageHelper.Gui_off)),
                                                             new GuiObject(GuiObjectState.Layer,startx,starty+35,200,sizey,null,LanguageHelper.Gui_MusicLevel),
                                                             new GuiObject(GuiObjectState.Slider,startx-100,starty+65,400,sizey,SettingMenuModeMusicVolumeSlider),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*3,200,sizey,SettingsMenuModeAutosaveButton,LanguageHelper.Gui_Autosave+": "+(autosave?LanguageHelper.Gui_on:LanguageHelper.Gui_off)),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*4,200,sizey,SettingsMenuModeMessagesButton,LanguageHelper.Gui_Messages+": "+(messages?LanguageHelper.Gui_on:LanguageHelper.Gui_off)),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty+35*5+10,200,sizey,SettingsMenuModeToMainMenuButton,LanguageHelper.Gui_Back),
                                                             new GuiObject(GuiObjectState.Layer,startx,starty-35*3-5,200,sizey,null,LanguageHelper.Gui_Resolution),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty-35*2-5,200,sizey,SettingsMenuModeResolutionButton,displaymodes[selecteddisplaymode].X.ToString()+" x "+displaymodes[selecteddisplaymode].Y.ToString()),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty-35-5,200,sizey,SettingsMenuModeFullscreenButton,LanguageHelper.Gui_Fullscreen+": "+(!fullscreen?LanguageHelper.Gui_on:LanguageHelper.Gui_off)),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,starty-35*4-5,200,sizey,SettingsSetLanguage,LanguageHelper.Gui_Language+":"+(playerlanguage==LanguageRus?LanguageHelper.Gui_LanguageRus:LanguageHelper.Gui_LanguageEng)),}));
            gui.forms[3].elements[4].reserved = musicvolume;

            //4 - help
            gui.forms.Add(new Form(HelpMenuMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,-10,220,height+20,null),
                                                             new GuiObject(GuiObjectState.PanelDark,(width - 800) / 2,(height - 600) / 2,800,550,null),
                                                             new GuiObject(GuiObjectState.MenuButton,(width - 800) / 2-30+800,(height - 600) / 2,30,30,HelpMenuModeUpButton,"A"),
                                                             new GuiObject(GuiObjectState.MenuButton,(width - 800) / 2-30+800,(height - 600) / 2+550-30,30,30,HelpMenuModeDownButton,"V"),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,height-40,200,sizey,HelpMenuModeToMainMenuMenuButton,LanguageHelper.Gui_Back)}));

            //5 - about
            gui.forms.Add(new Form(AboutMenuMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,-10,220,height+20,null),
                                                             new GuiObject(GuiObjectState.PanelDark,(width - 690) / 2,(height - 200) / 2,690,200,null),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,height-130,200,sizey,AboutMenuModeToChronoModeButton,LanguageHelper.Gui_Chrono),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,height-90,200,sizey,AboutMenuModeToPromoModeButton,LanguageHelper.Gui_Promo),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,height-40,200,sizey,AboutMenuModeToMenuModeButton,LanguageHelper.Gui_Back)}));

            //6 - chrono
            startx = (width - 200) / 2;
            gui.forms.Add(new Form(ChronoMenuMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,-10,220,height+20,null),
                                                             new GuiObject(GuiObjectState.PanelDark,(width - 630) / 2,(height - 350) / 2,630,360,null),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,height-40,200,sizey,ChonoMenuModeToAboutMenuModeButton,LanguageHelper.Gui_Back)}));

            //7 - promo
            gui.forms.Add(new Form(PromoMenuMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,-10,220,height+20,null),
                                                             new GuiObject(GuiObjectState.PanelDark,(width - 800) / 2,(height - 600) / 2,800,550,null),
                                                             new GuiObject(GuiObjectState.MenuButton,(width - 800) / 2-30+800,(height - 600) / 2,30,30,PromoMenuModeUpButton,"A"),
                                                             new GuiObject(GuiObjectState.MenuButton,(width - 800) / 2-30+800,(height - 600) / 2+550-30,30,30,PromoMenuModeDownButton,"V"),
                                                             new GuiObject(GuiObjectState.MenuButton,startx,height-40,200,sizey,PromoMenuModeToAboutMenuModeButton,LanguageHelper.Gui_Back)}));

            //8 - univerce
            int un_size = 600;
            int un_startx = (width - un_size - 200) / 2;
            int un_starty = (height - un_size) / 2;
            gui.forms.Add(new Form(UniverceMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,un_startx,un_starty,un_size,un_size,null),
                                                                 new GuiObject(GuiObjectState.PanelDark,un_startx+4,un_starty+4,un_size-8,un_size-8,UniverceButton),
                                                                 new GuiObject(GuiObjectState.PanelDark,un_startx+un_size,un_starty,200,un_size,null),
                                                                 new GuiObject(GuiObjectState.MenuButton,un_startx+un_size,un_starty+un_size-100,200,sizey,UniverceModeToStartGameMenuModeButton,LanguageHelper.Gui_Back),
                                                                 new GuiObject(GuiObjectState.MenuButton,un_startx+un_size,un_starty+un_size-100-35,200,sizey,UniverceModeToStarOverviewModeButton,LanguageHelper.Gui_Fly)}));

            //9 - planet overview
            gui.forms.Add(new Form(PlanetOverviewMode, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark, 0, 0, width, sizey, null) ,
                                                                         new GuiObject(GuiObjectState.Button, 0, 0, 30, sizey, AllModesToMenuMode,"M") ,
                                                                         new GuiObject(GuiObjectState.Button, 31, 0, 100, sizey,PlanetOverviewModeToStarOverviewMode, LanguageHelper.Gui_ToSystem),
                                                                         new GuiObject(GuiObjectState.Button, 132, 0, 160, sizey,PlanetOverviewModeToPlanetMapMode, LanguageHelper.Gui_ToPlanetMap)}));

            //10 - star overview
            gui.forms.Add(new Form(StarOverviewMode, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark, 0, 0, width, sizey, null) ,
                                                                       new GuiObject(GuiObjectState.Button, 0, 0, 30, sizey, AllModesToMenuMode,"M"),
                                                                       new GuiObject(GuiObjectState.Button, 31, 0, 100, sizey, StarOverviewModeOpenScoreButton,LanguageHelper.Gui_Score),}));

            //11 - planet map
#if _DEMO
            gui.forms.Add(new Form(PlanetMapMode, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark, 0, 0, width, sizey, null) ,
                                                                    new GuiObject(GuiObjectState.Button, 0, 0, 30, sizey, AllModesToMenuMode,"M"),
                                                                    new GuiObject(GuiObjectState.Button, 31, 0, 100, sizey,PlanetOverviewModeToStarOverviewMode, LanguageHelper.Gui_ToSystem),
                                                                    new GuiObject(GuiObjectState.Button, 132, 0, 160, sizey,PlanetMapModeCreateBase, LanguageHelper.Gui_CreateBase),
                                                                    new GuiObject(GuiObjectState.Button, 293, 0, 160, sizey,PlanetMapModeToLocalMapMode, LanguageHelper.Gui_SelectBase),
                                                                    new GuiObject(GuiObjectState.Button, 501, 0, 90, sizey,PlanetMapMenuOpenMarketMenuWraper, LanguageHelper.Gui_Stream),
                                                                    new GuiObject(GuiObjectState.Button, 592, 0, 90, sizey,PlanetMapMenuOpenModulesMenu, LanguageHelper.Gui_Modules),
                                                                    new GuiObject(GuiObjectState.Button, 683, 0, 90, sizey,StarOverviewModeOpenScoreButton, LanguageHelper.Gui_Score),
                                                                    new GuiObject(GuiObjectState.Layer, 0, sizey, width, height-sizey, PlanetMapModeMain),
                                                                    new GuiObject(GuiObjectState.Button, 774, 0, 30, sizey,PlanetMapMenuToPlanetOverview, "i"),
                                                                    new GuiObject(GuiObjectState.PanelDark, 0, height-sizey, width, sizey, null)}));
#else
            gui.forms.Add(new Form(PlanetMapMode, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark, 0, 0, width, sizey, null) ,
                                                                    new GuiObject(GuiObjectState.Button, 0, 0, 30, sizey, AllModesToMenuMode,"M"),
                                                                    new GuiObject(GuiObjectState.Button, 31, 0, 100, sizey,PlanetOverviewModeToStarOverviewMode, LanguageHelper.Gui_ToSystem),
                                                                    new GuiObject(GuiObjectState.Button, 132, 0, 160, sizey,PlanetMapModeCreateBase, LanguageHelper.Gui_CreateBase),
                                                                    new GuiObject(GuiObjectState.Button, 293, 0, 160, sizey,PlanetMapModeToLocalMapMode, LanguageHelper.Gui_SelectBase),
                                                                    new GuiObject(GuiObjectState.Button, 501, 0, 90, sizey,PlanetMapMenuOpenMarketMenuWraper, LanguageHelper.Gui_Stream),
                                                                    new GuiObject(GuiObjectState.Button, 592, 0, 90, sizey,PlanetMapMenuOpenModulesMenu, LanguageHelper.Gui_Modules),
                                                                    new GuiObject(GuiObjectState.Button, 683, 0, 90, sizey,StarOverviewModeOpenScoreButton, LanguageHelper.Gui_Score),
                                                                    new GuiObject(GuiObjectState.Layer, 0, sizey, width, height-sizey, PlanetMapModeMain),
                                                                    new GuiObject(GuiObjectState.Button, 774, 0, 30, sizey,PlanetMapMenuToPlanetOverview, "i"),
                                                                    new GuiObject(GuiObjectState.PanelDark, 0, height-sizey, width, sizey, null),
                                                                    new GuiObject(GuiObjectState.Button, 501, sizey, 90, sizey,PlanetMapMenuOpenAttackMenuWraper, LanguageHelper.Gui_Attack)}));
#endif


            //12 - local map
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark, 0, 0, width, sizey, null),
                                                                   new GuiObject(GuiObjectState.Button, 0, 0, 30, sizey, AllModesToMenuMode,"M"),
                                                                   new GuiObject(GuiObjectState.Button, 31, 0, 130, sizey,LocalMapModeToPlanetMapMode, LanguageHelper.Gui_ToPlanetOrbit),
                                                                   new GuiObject(GuiObjectState.Button, 162, 0, 130, sizey,LocalMapMenuOpenEnergyMenu, LanguageHelper.Gui_EnergyMenu),
                                                                   new GuiObject(GuiObjectState.Button, 293, 0, 130, sizey,LocalMapMenuOpenInventoryMenu, LanguageHelper.Gui_Storage),
                                                                   new GuiObject(GuiObjectState.Button, 424, 0, 130, sizey,LocalMapMenuOpenScienceMenu, LanguageHelper.Gui_Science),
                                                                   new GuiObject(GuiObjectState.Layer, 0, sizey, width-200, height-sizey, LocalMapModeMain),
                                                                   new GuiObject(GuiObjectState.Layer, width-190, 38, 180, 180, LocalMapMinimapButton)}));

            int offcety = 35;
            //13 - building menu
#if _DEMO
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsToManagementButton, LanguageHelper.BuildMenu_Administry),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsToManufacturingButton, LanguageHelper.BuildMenu_Manufacturing),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsToStorageButton, LanguageHelper.BuildMenu_Storage),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3, 200,sizey, LocalMapModeBuildingsToLinksButton, LanguageHelper.BuildMenu_Links),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*4, 200,sizey, LocalMapModeBuildingsToScienceButton, LanguageHelper.BuildMenu_Science),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*5, 200,sizey, LocalMapModeBuildingsToDefenceButton, LanguageHelper.BuildMenu_Defence),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*6+10, 200,sizey, LocalMapModeBuildingsDestroyButton, LanguageHelper.BuildMenu_Destroy),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*7+10, 200,sizey, LocalMapModeBuildingsSwitchButton, LanguageHelper.BuildMenu_SwitchBuilding),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*8+20, 200,sizey, LocalMapModeCreditsStreamButton, LanguageHelper.BuildMenu_CreditStream),}));
#else 
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsToManagementButton, LanguageHelper.BuildMenu_Administry),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsToManufacturingButton, LanguageHelper.BuildMenu_Manufacturing),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsToStorageButton, LanguageHelper.BuildMenu_Storage),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3, 200,sizey, LocalMapModeBuildingsToAttackButton, LanguageHelper.BuildMenu_Attack),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*4, 200,sizey, LocalMapModeBuildingsToLinksButton, LanguageHelper.BuildMenu_Links),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*5, 200,sizey, LocalMapModeBuildingsToScienceButton, LanguageHelper.BuildMenu_Science),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*6, 200,sizey, LocalMapModeBuildingsToDefenceButton, LanguageHelper.BuildMenu_Defence),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*7+10, 200,sizey, LocalMapModeBuildingsDestroyButton, LanguageHelper.BuildMenu_Destroy),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*8+10, 200,sizey, LocalMapModeBuildingsSwitchButton, LanguageHelper.BuildMenu_SwitchBuilding),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*9+15, 200,sizey, LocalMapModeCreditsStreamButton, LanguageHelper.BuildMenu_CreditStream),}));
#endif

            //14 - admin building menu
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsCommandCenterButton, LanguageHelper.BuildMenu_CommandCenter),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsBuildCenterButton, LanguageHelper.BuildMenu_BuildCenter),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsStorageCenterButton, LanguageHelper.BuildMenu_StorageCenter),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3, 200,sizey, LocalMapModeBuildingsLinkCenterButton, LanguageHelper.BuildMenu_LinkCenter),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*4, 200,sizey, LocalMapModeBuildingsScienceCenterButton, LanguageHelper.BuildMenu_ScienceCenter),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*5+10, 200,sizey, LocalMapModeBuildingsManagementBackButton, LanguageHelper.Gui_Back)}, false));
            //15 - manuf building menu
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsDroinFactoryButton, LanguageHelper.BuildMenu_DroneFactory),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsProcessingFactoryButton, LanguageHelper.BuildMenu_ProcesingFactory),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsMineButton, LanguageHelper.BuildMenu_Mine),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3, 200,sizey, LocalMapModeBuildingsDirrickButton, LanguageHelper.BuildMenu_Dirrick),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*4, 200,sizey, LocalMapModeBuildingsFarmButton, LanguageHelper.BuildMenu_Farm),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*5, 200,sizey, LocalMapModeBuildingsCloseFarmButton, LanguageHelper.BuildMenu_ClosedFarm),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*6, 200,sizey, LocalMapModeBuildingsGeneratorButton, LanguageHelper.BuildMenu_Generator),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*7+10, 200,sizey, LocalMapModeBuildingsManufacturingBackButton, LanguageHelper.Gui_Back)}, false));

            //16 - storage building menu
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsWarehouseButton, LanguageHelper.BuildMenu_Warehouse),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsHouseButton, LanguageHelper.BuildMenu_House),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsEnergyBankButton, LanguageHelper.BuildMenu_EnergyStorage),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3, 200,sizey, LocalMapModeBuildingsInformationBankButton, LanguageHelper.BuildMenu_InfoStorage),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*4, 200,sizey, LocalMapModeBuildingsLuquidStorageButton, LanguageHelper.BuildMenu_LiquidStorage),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*5+10, 200,sizey, LocalMapModeBuildingsStorageBackButton, LanguageHelper.Gui_Back)}, false));
            //17 - links building menu
#if _DEMO
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsSpaceportButton, LanguageHelper.BuildMenu_Spaceport),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsExchangeButton, LanguageHelper.BuildMenu_Market),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsParckingButton, LanguageHelper.BuildMenu_Parking),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3, 200,sizey, LocalMapModeBuildingsBeaconButton, LanguageHelper.BuildMenu_DroneBeacone),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*4+10, 200,sizey, LocalMapModeBuildingsLinksBackButton, LanguageHelper.Gui_Back)}, false));
#else
           gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsSpaceportButton, LanguageHelper.BuildMenu_Spaceport),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsExchangeButton, LanguageHelper.BuildMenu_Market),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsParckingButton, LanguageHelper.BuildMenu_Parking),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3, 200,sizey, LocalMapModeBuildingsBeaconButton, LanguageHelper.BuildMenu_DroneBeacone),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*4, 200,sizey, LocalMapModeBuildingsBuilderButton, LanguageHelper.BuildMenu_Builder),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*5+10, 200,sizey, LocalMapModeBuildingsLinksBackButton, LanguageHelper.Gui_Back)}, false));
#endif
            //18 - science building menu
#if _DEMO
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsLaboratoryButton, LanguageHelper.BuildMenu_Laboratory),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsCollectorButton, LanguageHelper.BuildMenu_Collector),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2+10, 200,sizey, LocalMapModeBuildingsScienceBackButton, LanguageHelper.Gui_Back)}, false));
#else
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsLaboratoryButton, LanguageHelper.BuildMenu_Laboratory),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsCollectorButton, LanguageHelper.BuildMenu_Collector),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsTerrinScanerButton, LanguageHelper.BuildMenu_TerrainScaner),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3+10, 200,sizey, LocalMapModeBuildingsScienceBackButton, LanguageHelper.Gui_Back)}, false));
#endif

            //19 - defence building menu
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty, 200,sizey, LocalMapModeBuildingsAtmoshereShieldButton, LanguageHelper.BuildMenu_AtmosphereShield),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety, 200,sizey, LocalMapModeBuildingsPowerShieldButton, LanguageHelper.BuildMenu_PowerShield),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2, 200,sizey, LocalMapModeBuildingsEmmisionShieldButton, LanguageHelper.BuildMenu_EmmisionShield),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3, 200,sizey, LocalMapModeBuildingsTurretButton, LanguageHelper.BuildMenu_Turret),
                                                               new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*4, 200,sizey, LocalMapModeBuildingsDefenceBackButton, LanguageHelper.Gui_Back) }, false));

            //20 - load
            int l_size = 600;
            int l_startx = (width - l_size - 200) / 2;
            int l_starty = (height - l_size) / 2;
            gui.forms.Add(new Form(LoadMode, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,l_startx,l_starty,l_size,l_size,null),
                                                                 new GuiObject(GuiObjectState.PanelDark,l_startx+4,l_starty+4,l_size-8,l_size-8,LoadModeButton),
                                                                 new GuiObject(GuiObjectState.PanelDark,l_startx+l_size,l_starty,200,l_size,null),
                                                                 new GuiObject(GuiObjectState.MenuButton,l_startx+l_size,l_starty+l_size-100,200,sizey,LoadModeBackButton,LanguageHelper.Gui_Back),
                                                                 new GuiObject(GuiObjectState.MenuButton,l_startx+l_size,l_starty+l_size-100-135,200,sizey,LoadModeSelectButton,LanguageHelper.Gui_Load),
                                                                 new GuiObject(GuiObjectState.MenuButton,l_startx+l_size,l_starty+l_size-100-135+50,200,sizey,LoadModeDeleteButton,LanguageHelper.Gui_Delete)}));

            //21 - catalog
            int c_size = 400;
            int c_startx = (width - c_size) / 2;
            int c_starty = (height - c_size) / 2 + 40;
            catalogsunstrikes = catalogmeteorites = catalogpirates = true;
            gui.forms.Add(new Form(CatalogMode, new GuiObject[] {
                                                               new GuiObject(GuiObjectState.PanelDark, c_startx-10, c_starty-40, 420, 35*11+35, null),
                                                               new GuiObject(GuiObjectState.Slider, c_startx, c_starty, 400, 30, CatalogModeStarSizeSlider),
                                                               new GuiObject(GuiObjectState.Slider, c_startx, c_starty+35*2, 400, 30, CatalogModeStarTemperatureSlider),
                                                               new GuiObject(GuiObjectState.Slider, c_startx, c_starty+35*4, 400, 30, CatalogModePlatetSizeSlider),
                                                               new GuiObject(GuiObjectState.Slider, c_startx, c_starty+35*6, 400, 30, CatalogModePlanetLengthSlider),
                                                               new GuiObject(GuiObjectState.MenuButton, c_startx, c_starty+35*7+5, 120, 30, CatalogModeSunStrikesButton,LanguageHelper.Gui_Sunstrikesshort + ": " + (catalogsunstrikes ? LanguageHelper.Gui_on : LanguageHelper.Gui_off)),
                                                               new GuiObject(GuiObjectState.MenuButton, c_startx+140, c_starty+35*7+5, 120, 30, CatalogModePiratesButton,LanguageHelper.Gui_Piratesshort + ": " + (catalogpirates ? LanguageHelper.Gui_on : LanguageHelper.Gui_off)),
                                                               new GuiObject(GuiObjectState.MenuButton, c_startx+280, c_starty+35*7+5, 120, 30, CatalogModeMeteoritesButton,LanguageHelper.Gui_Meteoritesshort + ": " + (catalogmeteorites ? LanguageHelper.Gui_on : LanguageHelper.Gui_off)),
                                                               new GuiObject(GuiObjectState.MenuButton, c_startx+100, c_starty+35*8+10, 200, 30, CatalogModeFoundButton,LanguageHelper.Gui_FindCatalog),
                                                               new GuiObject(GuiObjectState.MenuButton, c_startx+100, c_starty+35*9+15, 200, 30, CatalogModeBackButton,LanguageHelper.Gui_Back)}));
            gui.forms[21].elements[1].reserved = catalogstarlighting; 
            gui.forms[21].elements[2].reserved = catalogstartemperature; 
            gui.forms[21].elements[3].reserved = catalogplanetradius; 
            gui.forms[21].elements[4].reserved = catalogplanetsemiaxis;

            //22 - attack building menu
            gui.forms.Add(new Form(LocalMapMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty,200,sizey, LocalMapModeBuildingsAttackFactoryButton, LanguageHelper.BuildMenu_AttackFactory),
                                                                   new GuiObject(GuiObjectState.Button, width - 200, starty+offcety,200,sizey, LocalMapModeBuildingsAttackParkingButton, LanguageHelper.BuildMenu_AttackParking),
                                                                   new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*2,200,sizey, LocalMapModeBuildingsRocketParkingButton, LanguageHelper.BuildMenu_RocketParking),
                                                                   new GuiObject(GuiObjectState.Button, width - 200, starty+offcety*3+10, 200,sizey, LocalMapModeBuildingsAttackBackButton, LanguageHelper.Gui_Back)}, false));

            //23 - disconect
            gui.forms.Add(new Form(DisconnectedMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty-100,200,sizey, DisconectReconectButton, LanguageHelper.Gui_Reconect),
                                                                       new GuiObject(GuiObjectState.Button, width - 200, starty+offcety-100,200,sizey, DisconectExitButton, LanguageHelper.Gui_Back)}));

            //24 - connected
            gui.forms.Add(new Form(ConnectedMode, new GuiObject[] { new GuiObject(GuiObjectState.Button, width - 200, starty-100,200,sizey, DisconectReconectButton, LanguageHelper.Gui_Reconect),
                                                                    new GuiObject(GuiObjectState.Button, width - 200, starty+offcety-100,200,sizey, DisconectExitButton, LanguageHelper.Gui_Back)}));
        }
        void LoadResolutions()
        {
            List<Point> displaymodeslist = new List<Point>();
            foreach (DisplayMode dm in GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                Point p = new Point(dm.Width, dm.Height);
                if (p.X >= 800 && p.Y >= 600 && !displaymodeslist.Contains(p))
                {
                    displaymodeslist.Add(p);
                }
            }
            displaymodes = displaymodeslist.ToArray();
        }

        protected override void Initialize()
        {
            gui = new Gui();
            camera = new Camera();
            state = MainMenuMode;

            starGradient = new Gradient();
            starGradient.AddPoint(new GradientPart(new Color(127, 178, 255), 30000));
            starGradient.AddPoint(new GradientPart(new Color(42, 189, 231), 10000));
            starGradient.AddPoint(new GradientPart(new Color(243, 248, 255), 7500));
            starGradient.AddPoint(new GradientPart(new Color(255, 255, 200), 6000));
            starGradient.AddPoint(new GradientPart(new Color(255, 255, 0), 4000));
            starGradient.AddPoint(new GradientPart(new Color(255, 0, 0), 3000));
            starGradient.AddPoint(new GradientPart(new Color(207, 0, 0), 2000));

            #region Set gradient
            planetGradient = new Gradient[13];

            planetGradient[0] = new Gradient();
            planetGradient[0].AddPoint(new GradientPart(new Color(211, 106, 12), -1));
            planetGradient[0].AddPoint(new GradientPart(new Color(233, 115, 0), -0.25));
            planetGradient[0].AddPoint(new GradientPart(new Color(196, 14, 0), 0));
            planetGradient[0].AddPoint(new GradientPart(new Color(29, 8, 2), 0.0000625));
            planetGradient[0].AddPoint(new GradientPart(new Color(42, 30, 20), 0.125));
            planetGradient[0].AddPoint(new GradientPart(new Color(81, 38, 4), 0.375));
            planetGradient[0].AddPoint(new GradientPart(new Color(175, 37, 0), 0.75));
            planetGradient[0].AddPoint(new GradientPart(new Color(255, 54, 0), 1));

            planetGradient[1] = new Gradient();
            planetGradient[1].AddPoint(new GradientPart(new Color(35, 23, 1), -1));
            planetGradient[1].AddPoint(new GradientPart(new Color(117, 83, 11), -0.25));
            planetGradient[1].AddPoint(new GradientPart(new Color(193, 172, 58), 0));
            planetGradient[1].AddPoint(new GradientPart(new Color(93, 31, 7), 0.0000625));
            planetGradient[1].AddPoint(new GradientPart(new Color(202, 156, 17), 0.125));
            planetGradient[1].AddPoint(new GradientPart(new Color(203, 159, 40), 0.375));
            planetGradient[1].AddPoint(new GradientPart(new Color(180, 104, 23), 0.75));
            planetGradient[1].AddPoint(new GradientPart(new Color(255, 236, 226), 1));

            planetGradient[2] = new Gradient();
            planetGradient[2].AddPoint(new GradientPart(new Color(69, 52, 23), -1));
            planetGradient[2].AddPoint(new GradientPart(new Color(91, 72, 39), -0.25));
            planetGradient[2].AddPoint(new GradientPart(new Color(152, 117, 57), 0));
            planetGradient[2].AddPoint(new GradientPart(new Color(187, 114, 43), 0.0000625));
            planetGradient[2].AddPoint(new GradientPart(new Color(214, 197, 89), 0.125));
            planetGradient[2].AddPoint(new GradientPart(new Color(184, 109, 36), 0.375));
            planetGradient[2].AddPoint(new GradientPart(new Color(117, 71, 22), 0.75));
            planetGradient[2].AddPoint(new GradientPart(new Color(231, 186, 122), 1));

            planetGradient[3] = new Gradient();
            planetGradient[3].AddPoint(new GradientPart(new Color(37, 92, 112), -1));
            planetGradient[3].AddPoint(new GradientPart(new Color(75, 118, 127), -0.25));
            planetGradient[3].AddPoint(new GradientPart(new Color(85, 175, 149), 0));
            planetGradient[3].AddPoint(new GradientPart(new Color(197, 191, 85), 0.0000625));
            planetGradient[3].AddPoint(new GradientPart(new Color(214, 197, 89), 0.125));
            planetGradient[3].AddPoint(new GradientPart(new Color(177, 112, 48), 0.375));
            planetGradient[3].AddPoint(new GradientPart(new Color(125, 87, 66), 0.75));
            planetGradient[3].AddPoint(new GradientPart(new Color(235, 173, 112), 1));

            planetGradient[4] = new Gradient();
            planetGradient[4].AddPoint(new GradientPart(new Color(17, 46, 53), -1));
            planetGradient[4].AddPoint(new GradientPart(new Color(42, 83, 100), -0.25));
            planetGradient[4].AddPoint(new GradientPart(new Color(54, 149, 138), 0));
            planetGradient[4].AddPoint(new GradientPart(new Color(225, 176, 93), 0.125));
            planetGradient[4].AddPoint(new GradientPart(new Color(167, 127, 58), 0.0000625));
            planetGradient[4].AddPoint(new GradientPart(new Color(139, 165, 47), 0.375));
            planetGradient[4].AddPoint(new GradientPart(new Color(92, 110, 10), 0.75));
            planetGradient[4].AddPoint(new GradientPart(new Color(226, 202, 124), 1));

            planetGradient[5] = new Gradient();
            planetGradient[5].AddPoint(new GradientPart(new Color(3, 40, 53), -1));
            planetGradient[5].AddPoint(new GradientPart(new Color(12, 113, 125), -0.25));
            planetGradient[5].AddPoint(new GradientPart(new Color(58, 193, 163), 0));
            planetGradient[5].AddPoint(new GradientPart(new Color(87, 113, 19), 0.0000625));
            planetGradient[5].AddPoint(new GradientPart(new Color(125, 164, 30), 0.125));
            planetGradient[5].AddPoint(new GradientPart(new Color(203, 201, 40), 0.375));
            planetGradient[5].AddPoint(new GradientPart(new Color(180, 104, 23), 0.75));
            planetGradient[5].AddPoint(new GradientPart(new Color(255, 236, 226), 1));

            planetGradient[6] = new Gradient();
            planetGradient[6].AddPoint(new GradientPart(new Color(3, 53, 33), -1));
            planetGradient[6].AddPoint(new GradientPart(new Color(24, 111, 79), -0.25));
            planetGradient[6].AddPoint(new GradientPart(new Color(58, 193, 140), 0));
            planetGradient[6].AddPoint(new GradientPart(new Color(87, 113, 19), 0.0000625));
            planetGradient[6].AddPoint(new GradientPart(new Color(198, 189, 51), 0.125));
            planetGradient[6].AddPoint(new GradientPart(new Color(176, 148, 37), 0.375));
            planetGradient[6].AddPoint(new GradientPart(new Color(152, 90, 24), 0.75));
            planetGradient[6].AddPoint(new GradientPart(new Color(212, 201, 195), 1));

            planetGradient[7] = new Gradient();
            planetGradient[7].AddPoint(new GradientPart(new Color(15, 55, 12), -1));
            planetGradient[7].AddPoint(new GradientPart(new Color(27, 120, 23), -0.25));
            planetGradient[7].AddPoint(new GradientPart(new Color(129, 203, 103), 0));
            planetGradient[7].AddPoint(new GradientPart(new Color(156, 122, 39), 0.0000625));
            planetGradient[7].AddPoint(new GradientPart(new Color(225, 159, 49), 0.125));
            planetGradient[7].AddPoint(new GradientPart(new Color(196, 103, 34), 0.375));
            planetGradient[7].AddPoint(new GradientPart(new Color(128, 74, 11), 0.75));
            planetGradient[7].AddPoint(new GradientPart(new Color(251, 204, 172), 1));

            planetGradient[8] = new Gradient();
            planetGradient[8].AddPoint(new GradientPart(new Color(12, 55, 39), -1));
            planetGradient[8].AddPoint(new GradientPart(new Color(27, 120, 93), -0.25));
            planetGradient[8].AddPoint(new GradientPart(new Color(103, 203, 169), 0));
            planetGradient[8].AddPoint(new GradientPart(new Color(156, 144, 39), 0.0000625));
            planetGradient[8].AddPoint(new GradientPart(new Color(214, 178, 53), 0.125));
            planetGradient[8].AddPoint(new GradientPart(new Color(164, 168, 29), 0.375));
            planetGradient[8].AddPoint(new GradientPart(new Color(113, 122, 15), 0.75));
            planetGradient[8].AddPoint(new GradientPart(new Color(255, 241, 231), 1));

            planetGradient[9] = new Gradient();
            planetGradient[9].AddPoint(new GradientPart(new Color(34, 100, 82), -1));
            planetGradient[9].AddPoint(new GradientPart(new Color(54, 124, 108), -0.25));
            planetGradient[9].AddPoint(new GradientPart(new Color(89, 172, 150), 0));
            planetGradient[9].AddPoint(new GradientPart(new Color(114, 130, 81), 0.0000625));
            planetGradient[9].AddPoint(new GradientPart(new Color(213, 193, 68), 0.125));
            planetGradient[9].AddPoint(new GradientPart(new Color(113, 155, 54), 0.375));
            planetGradient[9].AddPoint(new GradientPart(new Color(60, 106, 20), 0.75));
            planetGradient[9].AddPoint(new GradientPart(new Color(184, 169, 132), 1));

            planetGradient[10] = new Gradient();
            planetGradient[10].AddPoint(new GradientPart(new Color(23, 69, 67), -1));
            planetGradient[10].AddPoint(new GradientPart(new Color(39, 87, 91), -0.25));
            planetGradient[10].AddPoint(new GradientPart(new Color(68, 133, 127), 0));
            planetGradient[10].AddPoint(new GradientPart(new Color(149, 169, 106), 0.0000625));
            planetGradient[10].AddPoint(new GradientPart(new Color(214, 197, 89), 0.125));
            planetGradient[10].AddPoint(new GradientPart(new Color(100, 118, 68), 0.375));
            planetGradient[10].AddPoint(new GradientPart(new Color(112, 103, 36), 0.75));
            planetGradient[10].AddPoint(new GradientPart(new Color(184, 175, 132), 1));

            planetGradient[11] = new Gradient();
            planetGradient[11].AddPoint(new GradientPart(new Color(17, 51, 49), -1));
            planetGradient[11].AddPoint(new GradientPart(new Color(34, 75, 79), -0.25));
            planetGradient[11].AddPoint(new GradientPart(new Color(61, 120, 114), 0));
            planetGradient[11].AddPoint(new GradientPart(new Color(106, 167, 169), 0.0000625));
            planetGradient[11].AddPoint(new GradientPart(new Color(168, 227, 238), 0.125));
            planetGradient[11].AddPoint(new GradientPart(new Color(165, 159, 89), 0.375));
            planetGradient[11].AddPoint(new GradientPart(new Color(104, 91, 67), 0.75));
            planetGradient[11].AddPoint(new GradientPart(new Color(226, 219, 199), 1));

            planetGradient[12] = new Gradient();
            planetGradient[12].AddPoint(new GradientPart(new Color(16, 71, 77), -1));
            planetGradient[12].AddPoint(new GradientPart(new Color(25, 82, 87), -0.25));
            planetGradient[12].AddPoint(new GradientPart(new Color(55, 139, 131), 0));
            planetGradient[12].AddPoint(new GradientPart(new Color(83, 195, 173), 0.0000625));
            planetGradient[12].AddPoint(new GradientPart(new Color(168, 238, 222), 0.125));
            planetGradient[12].AddPoint(new GradientPart(new Color(88, 183, 177), 0.375));
            planetGradient[12].AddPoint(new GradientPart(new Color(43, 135, 134), 0.75));
            planetGradient[12].AddPoint(new GradientPart(new Color(199, 224, 226), 1));
            #endregion

            InitGui();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            client = new Client();

            client.Connected += new NetComm.Client.ConnectedEventHandler(client_Connected);
            client.Disconnected += new NetComm.Client.DisconnectedEventHandler(client_Disconnected);
            client.DataReceived += new NetComm.Client.DataReceivedEventHandler(client_DataReceived);

            hostIP = "localhost";

            spriteBatch = new SpriteBatch(GraphicsDevice);

            powershieldslayer = new RenderTarget2D(GraphicsDevice, width - 200, height);
            emmisionshieldslayer = new RenderTarget2D(GraphicsDevice, width - 200, height);
            atmoshereshieldslayer = new RenderTarget2D(GraphicsDevice, width - 200, height);

            guiset = Content.Load<Texture2D>("Textures/gui");
            tilesettemplate = Content.Load<Texture2D>("Textures/tileset");
            buildset = Content.Load<Texture2D>("Textures/buildset");
            buildingparticlesset = Content.Load<Texture2D>("Textures/buildset particles");
            unitset = Content.Load<Texture2D>("Textures/unitset");
            resourceset = Content.Load<Texture2D>("Textures/resourceset");
            shieldset = Content.Load<Texture2D>("Textures/shield");
            meteoriteset = Content.Load<Texture2D>("Textures/meteorite");
            explotionset = Content.Load<Texture2D>("Textures/explotion");
            starset = Content.Load<Texture2D>("Textures/smallstar");
            bigstar = Content.Load<Texture2D>("Textures/bigstar");
            fractionset = Content.Load<Texture2D>("Textures/fractions");

            starShader = Content.Load<Effect>("Shaders/StarShader");
            mapEffect = Content.Load<Effect>("Shaders/PlanetShader");
            planetEffect = Content.Load<Effect>("Shaders/PlanetShader3D");
            tileEffect = Content.Load<Effect>("Shaders/TileShader");
            baselineShader = Content.Load<Effect>("Shaders/LineShader");
            fractiontileEffect = Content.Load<Effect>("Shaders/FractionTileShader");
            tunelEffect = Content.Load<Effect>("Shaders/TunelShader");

            baseset = Content.Load<Texture2D>("Textures/baseset");

            font = Content.Load<SpriteFont>("Fonts/debug");

            songs = new List<Song>();
            songs.Add(Content.Load<Song>("Music/411328_Milkroad"));
            songs.Add(Content.Load<Song>("Music/452673_Heroic_TicToc"));
            songs.Add(Content.Load<Song>("Music/467242_ODB"));
            songs.Add(Content.Load<Song>("Music/514439_Supermassive-Proxim"));
            songs.Add(Content.Load<Song>("Music/517163_Space-Exploration-P"));
            songs.Add(Content.Load<Song>("Music/519889_Buubdurub---Frontie"));
            songs.Add(Content.Load<Song>("Music/530833_Another-Day-in-Spac"));
            songs.Add(Content.Load<Song>("Music/556258_Beacon"));
            songs.Add(Content.Load<Song>("Music/559911_Catchy-Orbit-OST-Tr"));

            if (music)
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(songs[6]);
                }
                MediaPlayer.Volume = musicvolume;
            }

            univerce = new Univerce();
            universetexture = new Texture2D(GraphicsDevice, 592, 592);
            tuneltexture = new Texture2D(GraphicsDevice, 128, 128);
            CreateUniverceTexture();

            tunelEffect.Parameters["Texture"].SetValue(tuneltexture);

            screencapture = new RenderTarget2D(GraphicsDevice, width, height,false,SurfaceFormat.NormalizedByte4,DepthFormat.Depth24Stencil8);

            LoadShaderConfig();

            fractioncolors = new Color[fractionset.Height];
            fractionset.GetData<Color>(0, new Rectangle(0, 0, 1, fractionset.Height), fractioncolors, 0, fractionset.Height);


            //LoadGame("Content/Saves/autosave.save");
            //selectedplanet = 0;
            //selectedbase = 0;
            //planet = star.planets[0];
            //map = planet.maps[0];
            //mapHelper = new MapHelper(map);
            //CreatePlanetTexture(planet);
            //CreateLocalMapMinimapAndTexture(planet, map);
            //state = LocalMapMode;
        }

        protected override void UnloadContent()
        {
            client.Disconnect();
            SaveConfig();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            camera.mouse = new Vector2(ms.X, ms.Y);
            camera.mouseWheelOld = camera.mouseWheel;
            camera.mouseWheel = ms.ScrollWheelValue;
            if (this.IsActive)
                gui.Update(state, ms);
            messagesystem.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            currentGameTime = gameTime;

            if (ks.IsKeyDown(Keys.F9) && !oldkeyboardstate.IsKeyDown(Keys.F9))
            {
                pause = !pause;

                if (!offlineMode)
                {
                    if (pause) client.SendData(new byte[] { Pause });
                    else client.SendData(new byte[] { UnPause });
                }
            }

            if (!createnewsaydialog && !ks.IsKeyDown(Keys.Enter))
                createnewsaydialog = true;

            #region disconected
            if (state == DisconnectedMode)
            {
                if (disconect_time <= 0)
                {
                    client.Connect(hostIP, 3666, playername);
                    disconect_time = 1;
                }
                else disconect_time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            #endregion
            #region connected
            if (state == ConnectedMode)
            {
                if (disconect_time <= 0)
                {
                    client.Disconnect();
                }
                else disconect_time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            #endregion

            if (offlineMode)
            {
                if (state == StarOverviewMode || state == PlanetOverviewMode || state == PlanetMapMode || state == LocalMapMode)
                {
                    if (!pause)
                    {
                        star.Update(gameTime.ElapsedGameTime.TotalSeconds, gameTime.TotalGameTime.TotalSeconds);

                        if (scriptes!=null)foreach (Script s in scriptes)
                        {
                            s.waittime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (s.waittime < 0)
                            {
                                s.Update();
                            }
                        }

                        Score score = star.GetScore(player_id);
                        if (score.Total >= 100000)
                            OpenScore(score);

                        if (autosave)
                        {
                            timetonextautosave -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (timetonextautosave < 0)
                            {
                                DateTime date = DateTime.Now;
                                SaveGame("Content/Saves/" + star.name + "_autosave.save");
                                timetonextautosave = nextautosavetime;
                            }
                        }

                        foreach(Planet p in star.planets)
                            foreach(Map m in p.maps)
                            {
                                if (m.player_id == player_id&&m.messages!=null)
                                {
                                    foreach (string s in m.messages)
                                    {
                                        messagesystem.AddMessage(m.name + ":" + s, 3);
                                    }
                                    m.messages.Clear();
                                }
                            }

                        if (ks.IsKeyDown(Keys.Enter) && !oldkeyboardstate.IsKeyDown(Keys.Enter) && createnewsaydialog)
                        {
                            gui.InputBox(LanguageHelper.Gui_SayAll, playername + ":", MessageInputKey, MessageInputOk, MessageInputCansel, width / 2, height / 2);
                        }
                    }
                }

                if (state == UserMode)
                {
                    if (scriptes!=null)foreach (Script s in scriptes)
                    {
                        s.waittime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (s.waittime < 0)
                        {
                            s.Update();
                        }
                    }
                }
            }
            else
            {
                if (!pause)
                {
                    if (star != null)
                    {
                        star.UpdateOnline(gameTime.ElapsedGameTime.TotalSeconds, gameTime.TotalGameTime.TotalSeconds);
                    }

                    if (scriptes!=null)foreach (Script s in scriptes)
                    {
                        s.waittime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (s.waittime < 0)
                        {
                            s.Update();
                        }
                    }

                    timetosyncronize -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timetosyncronize <= 0)
                    {
                        timetosyncronize = SyncronizationRate;
                        if (packets_num <= 0)
                        {
                            if (state == StarOverviewMode)
                                client.SendData(new byte[] { StarSyncronize });
                            if (state == PlanetMapMode)
                                client.SendData(new byte[] { PlanetSyncronize, (byte)selectedplanet });
                            if (state == LocalMapMode)
                            {
                                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                                bw.Write(LocalSyncronize);
                                bw.Write((byte)selectedplanet);
                                bw.Write((int)map.position.X);
                                bw.Write((int)map.position.Y);

                                byte[] membuf = mem.GetBuffer();
                                byte[] retbuf = new byte[bw.BaseStream.Position];
                                Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                                bw.Close();
                                mem.Close();
                                client.SendData(retbuf);
                            }
                        }

                        System.IO.MemoryStream mem2 = new System.IO.MemoryStream();
                        System.IO.BinaryWriter bw2 = new System.IO.BinaryWriter(mem2);

                        bw2.Write(MessagesSyncronize);
                        bw2.Write(player_id);

                        byte[] membuf2 = mem2.GetBuffer();
                        byte[] retbuf2 = new byte[bw2.BaseStream.Position];
                        Array.Copy(membuf2, retbuf2, bw2.BaseStream.Position);

                        bw2.Close();
                        mem2.Close();
                        client.SendData(retbuf2);
                    }

                    if (ks.IsKeyDown(Keys.Enter) && !oldkeyboardstate.IsKeyDown(Keys.Enter) && createnewsaydialog)
                    {
                        gui.InputBox(LanguageHelper.Gui_SayAll, playername + ":", MessageInputKey, MessageInputOk, MessageInputCansel, width / 2, height / 2);
                    }
                }

                if (state == UserMode)
                {
                    if (scriptes!=null)foreach (Script s in scriptes)
                    {
                        s.waittime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (s.waittime < 0)
                        {
                            s.Update();
                        }
                    }
                }
            }

            oldkeyboardstate = ks;

            if (state == LocalMapMode)
            {
                camera.onx = ((int)camera.x + ms.X) / 32;
                camera.ony = ((int)camera.y + ms.Y) / 32;

                if (gui.popupform == null || ks.IsKeyDown(Keys.Space))
                {
                    bool hfly = false;
                    bool vfly = false;
                    if (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.Left) || ms.X < 2) { hfly = true; if (camera.dx > -1000)camera.dx -= 25; }
                    if (ks.IsKeyDown(Keys.D) || ks.IsKeyDown(Keys.Right) || ms.X > width - 2) { hfly = true; if (camera.dx < 1000)camera.dx += 25; }
                    if (ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.Down) || ms.Y > height - 2) { vfly = true; if (camera.dy < 1000)camera.dy += 25; }
                    if (ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.Up) || ms.Y < 2) { vfly = true; if (camera.dy >- 1000)camera.dy -= 25; }

                    if (camera.dx < 0 && camera.x > 0)
                    {
                        camera.x += camera.dx * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                        if (camera.x < 0)
                        {
                            camera.dx = 0;
                            camera.x = 0;
                        }
                    }
                    if (camera.dx > 0 && camera.x < (map.width * 32 - width + 200 - 10))
                    {
                        camera.x += camera.dx * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                        if (camera.x > (map.width * 32 - width + 200 - 10))
                        {
                            camera.dx = 0;
                            camera.x = (map.width * 32 - width + 200 - 10);
                        }
                    }
                    if (!hfly) camera.dx -= camera.dx * 0.1f;
                    if (camera.dy < 0 && camera.y > 0)
                    {
                        camera.y += camera.dy * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                        if (camera.y < 0)
                        {
                            camera.dy = 0;
                            camera.y = 0;
                        }
                    }
                    if (camera.dy > 0 && camera.y < map.height * 32 - height - 10 + 20)
                    {
                        camera.y += camera.dy * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                        if (camera.y > map.height * 32 - height - 10 + 20)
                        {
                            camera.dy = 0;
                            camera.y = map.height * 32 - height - 10 + 20;
                        }
                    }
                    if (!vfly) camera.dy -= camera.dy * 0.1f;
                }
            }

            //if (state == StarOverviewMode)
            //{
            //    if (nexstartime < 0)
            //    {
            //        star = new Star();
            //        Random rand = new Random();
            //        star.GeneratePlanets(rand.Next(12) + 1);

            //        nexstartime = 3;
            //    }
            //    nexstartime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}

            if (needresetgui)
            {
                gui = new Gui();
                LoadLanguage();
                InitGui();
                needresetgui = false;
            }

            if (music && MediaPlayer.State == MediaState.Stopped)
            {
                Random rand = new Random();
                rand.Next();rand.Next();rand.Next();rand.Next();
                MediaPlayer.Play(songs[rand.Next(songs.Count)]);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (loading)
            {
                //GraphicsDevice.Clear(Color.Black);
                DrawLoadingTunel();
                spriteBatch.Begin();
                spriteBatch.DrawString(font, LanguageHelper.Gui_LoadingText, new Vector2(width / 4 * 3, height / 2),Color.White);
                spriteBatch.End();
            }
            else
            {
                #region Connected
                if (state == ConnectedMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);
                    if (messages) messagesystem.Draw(spriteBatch, font, width);
                    spriteBatch.DrawString(font, LanguageHelper.DrawText_DisconnectAfeter + " " + disconect_time.ToString("0.00") + " " + LanguageHelper.DrawText_Seconds, new Vector2(width * 3 / 4, height / 2), Color.White);
                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion

                #region Disconnected
                if (state == DisconnectedMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);
                    if (messages) messagesystem.Draw(spriteBatch, font, width);
                    spriteBatch.DrawString(font, LanguageHelper.DrawText_DisconectMode, Vector2.Zero, Color.White);
                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion

                #region Staroverview
                if (state == StarOverviewMode)
                {
                    if (star != null)
                    {
                        Color starcolor = starGradient.GetColor(star.temperature);
                        Color backstarcolor = Color.Lerp(Color.Black, starcolor, 0.1f);
                        GraphicsDevice.Clear(backstarcolor);

                        int starradius = (int)((star.size + 1.1) * (width < height ? width : height) / 20);
                        DrawStar(new Rectangle(width / 2 - starradius, height / 2 - starradius, starradius * 2, starradius * 2), starcolor);

                        float[] asteroids = star.GetAsteroids();
                        int minavaiblelength = starradius * 2;  // 0.25
                        int maxavaiblelength = (width < height ? width : height);  // 4.75
                        Random rand = new Random();

                        for (int i = 0; i < asteroids.Length; i++)
                        {
                            float l = asteroids[i];
                            float light = (float)(star.GetTotalTimeForLighting() / l / 7);
                            int num = (int)(l * 30);

                            for (int k = 0; k < num; k++)
                            {
                                float newl = l + (k % 5 - 2) / 10.0f;
                                if (newl < 1.0f)
                                    newl = 1.0f;
                                Vector2 v = Vector2.Transform(new Vector2(newl, 0), Matrix.CreateRotationZ(-light + k * 6.28f / num));
                                double scale = minavaiblelength + (maxavaiblelength - minavaiblelength) * ((l - 0.25) / (4.5f));
                                Vector2 v2 = new Vector2(width / 2 + v.X * (float)scale / 2, height / 2 + v.Y * (float)scale / 2);

                                DrawPlanet(new Rectangle((int)v2.X, (int)v2.Y, 2, 2), new Color(0, 0, 0, 128));

                                DrawLine(v2, v2 + v * 100, new Color(0, 0, 0, 128), backstarcolor);
                            }
                        }

                        if (star.planets != null)
                        {
                            Vector2 nearestplanet = new Vector2(0, 0);
                            float nearestdistance = float.MaxValue;
                            int nearestplanetid = 0;

                            spriteBatch.Begin();

                            for (int i = 0; i < star.planets_num; i++)
                            {
                                double distance = star.planets[i].semimajoraxis / star.radius * 0.00435;
                                if (distance > 4.75) distance = 4.75;
                                if (distance < 0.25) distance = 0.25;

                                Vector2 pos = star.planets[i].GetPosition(i);
                                double scale = minavaiblelength + (maxavaiblelength - minavaiblelength) * ((distance - 0.25) / (4.5));

                                pos = pos * (float)scale / 2;
                                int planetsize = (int)((star.planets[i].radius / 11500) * 8);
                                if (planetsize < 6) planetsize = 6;
                                Vector2 planetpos = new Vector2(width / 2 + pos.X, height / 2 + pos.Y);
                                DrawPlanet(new Rectangle((int)(width / 2 + pos.X - planetsize / 2), (int)(height / 2 + pos.Y - planetsize / 2), planetsize, planetsize), GetColoFromPlanetGradient(star.planets[i], 0));

                                Vector2 right = new Vector2(pos.Y, -pos.X);
                                right = Vector2.Normalize(right) * planetsize / 2;
                                VertexPositionColor[] vertexes = new VertexPositionColor[4];
                                short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };
                                vertexes[0] = new VertexPositionColor(new Vector3(planetpos + right, 0), new Color(0, 0, 0, 128));
                                vertexes[1] = new VertexPositionColor(new Vector3(planetpos - right, 0), new Color(0, 0, 0, 128));
                                vertexes[2] = new VertexPositionColor(new Vector3(planetpos + (pos - right) * 100, 0), new Color(0, 0, 0, 128));
                                vertexes[3] = new VertexPositionColor(new Vector3(planetpos + (pos + right) * 100, 0), new Color(0, 0, 0, 128));
                                foreach (EffectPass pass in baselineShader.CurrentTechnique.Passes)
                                {
                                    pass.Apply();
                                    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertexes, 0, 4, indexes, 0, 2);
                                }

                                float newnearestdistance = (planetpos - camera.mouse).LengthSquared();
                                if (newnearestdistance < 10000 && newnearestdistance < nearestdistance)
                                {
                                    nearestdistance = newnearestdistance;
                                    nearestplanet = planetpos;
                                    nearestplanetid = i;
                                }

                                int count = 0;
                                if (star.planets[i].maps != null)
                                    count = star.planets[i].maps.Count;
                                string text = star.planets[i].name + " (" + count.ToString() + ")";
                                Vector2 size = font.MeasureString(text);
                                //spriteBatch.DrawString(font, text, planetpos - new Vector2((int)(size.X / 2), (int)((planetpos.Y < 25 ? -1 : 1) * (5 + (int)size.Y) - 1)), Color.Black);
                                Vector2 nameposition = planetpos - new Vector2((int)(size.X / 2), (int)((planetpos.Y < 25 ? -1 : 1) * (5 + (int)size.Y)));
                                DrawSolidRectangle(new Rectangle((int)nameposition.X - 4, (int)nameposition.Y - 4, (int)size.X + 8, (int)size.Y + 8), new Color(0.1f, 0.1f, 0.1f, 0.1f));
                                spriteBatch.DrawString(font, text, nameposition, i == selectedplanet ? Color.LightGreen : Color.White);
                            }

                            if (nearestdistance < float.MaxValue)
                                DrawLine(camera.mouse, nearestplanet, new Color(0, 0, 0, 128), GetColoFromPlanetGradient(star.planets[nearestplanetid], 0));
                            //C3.XNA.Primitives2D.DrawLine(spriteBatch, camera.mouse, nearestplanet, new Color(100, 100, 100, 100));
                            spriteBatch.End();
                        }

                        if (star.unitgroups != null)
                        {
                            spriteBatch.Begin();
                            for (int i = 0; i < star.unitgroups.Count; i++)
                            {
                                int planet_id = star.unitgroups[i].planetid_target >= 0 ? star.unitgroups[i].planetid_target : star.unitgroups[i].planetid_mother;
                                double pdistance = 1;
                                if (planet_id < star.planets.Length)
                                    pdistance = star.planets[planet_id].semimajoraxis / star.radius * 0.00435;

                                Vector2 pos = star.unitgroups[i].position;

                                //int minavaiblelength = starradius * 2;  // 0.25
                                //int maxavaiblelength = (width < height ? width : height);               // 4.75

                                if (pdistance > 4.75) pdistance = 4.75;
                                if (pdistance < 0.25) pdistance = 0.25;
                                double pscale = minavaiblelength + (maxavaiblelength - minavaiblelength) * ((pdistance - 0.25) / (4.5));

                                pos = pos * (float)pscale / 2;

                                DrawRectangle(new Rectangle((int)(width / 2 + pos.X - 2), (int)(height / 2 + pos.Y - 2), 4, 4), GetPlayerColor(star.unitgroups[i].player_id));
                            }
                            spriteBatch.End();
                        }

                        foreach (PlayerStation p in star.players)
                        {
                            Vector2 pos = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ((-(float)star.GetTotalTime() + p.id * 7 + p.id * 5 + p.id) / 7));
                            double pscale = minavaiblelength + (maxavaiblelength - minavaiblelength) * (0.75 / 4.5);

                            pos = pos * (float)pscale / 2;
                            Color color = GetPlayerColor(p.id);
                            DrawRectangle(new Rectangle((int)(width / 2 + pos.X - 4), (int)(height / 2 + pos.Y - 4), 8, 8), color);
                            DrawRectangle(new Rectangle((int)(width / 2 + pos.X - 2), (int)(height / 2 + pos.Y - 2), 4, 4), color);
                        }
                    }
                    else GraphicsDevice.Clear(Color.Black);

                    if (drawgui)
                    {
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                        gui.Draw(state, spriteBatch, guiset, font);
                        if (messages) messagesystem.Draw(spriteBatch, font, width);
                        spriteBatch.DrawString(font, LanguageHelper.DrawText_Credits + " :" + star.GetMoney(player_id), new Vector2(150, 5), Color.White);
                        if (packets_num > 0)
                        {
                            spriteBatch.DrawString(font, LanguageHelper.DrawText_Loading + ": " + currentpart + "/" + packets_num, new Vector2(0, 20), Color.White);
                        }
                        spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                        spriteBatch.End();
                    }
                }
                #endregion

                #region Planetoverview
                if (state == PlanetOverviewMode)
                {
                    //GraphicsDevice.Clear(Color.Black);
                    Color starcolor = starGradient.GetColor(star.temperature);
                    GraphicsDevice.Clear(Color.Lerp(Color.Black, starcolor, 0.1f));
                    //GraphicsDevice.Clear(Color.Black);

                    //GraphicsDevice.BlendState = BlendState.Additive;
                    GraphicsDevice.DepthStencilState = DepthStencilState.None;

                    double startlighting = planet.star.size + 1.1;
                    //double startlighting = planet.star.radius / 0.00435;
                    int size = (int)(bigstar.Height * startlighting / 2);
                    Rectangle rect = new Rectangle(width - 150 - size, height / 4 - size, size * 2, size * 2);
                    DrawStar(rect, starcolor);

                    #region Draw planet
                    GraphicsDevice.BlendState = BlendState.Opaque;
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                    planetSphere.effect.Parameters["PlanetTexture"].SetValue(planettexture);
                    planetSphere.effect.Parameters["PlanetNormal"].SetValue(planetnormal);
                    planetSphere.effect.Parameters["AmbientColor"].SetValue(GetColoFromPlanetGradient(planet, 0).ToVector4());
                    planetSphere.graphicsDevice = GraphicsDevice;

                    float aspect = ((float)width) / height;
                    planetSphere.Draw(Matrix.CreateRotationY((float)(gameTime.TotalGameTime.TotalSeconds)) * Matrix.CreateRotationX(planet.axistilt),
                                      Matrix.CreateLookAt(new Vector3(-8.5f, 0.5f, 1), new Vector3(3, 0.5f, 1), Vector3.Up),
                                      Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspect, 0.1f, 100f));

                    GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    GraphicsDevice.DepthStencilState = DepthStencilState.None;
                    GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                    GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

                    GraphicsDevice.Textures[0] = null;
                    GraphicsDevice.Textures[1] = null;
                    #endregion

                    if (drawgui)
                    {
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);

                        gui.Draw(state, spriteBatch, guiset, font);

                        Rectangle rect2 = new Rectangle(width - 400, 70, 300, 240);
                        GuiObject.DrawDarkPanel(spriteBatch, guiset, rect2);
                        string text = LanguageHelper.DrawText_Name + ": " + planet.name;
                        text += "\n";
                        text += "\n" + LanguageHelper.DrawText_LengthToStar + ": " + planet.semimajoraxis.ToString("0.00") + " " + LanguageHelper.DrawText_ao;
                        text += "\n" + LanguageHelper.DrawText_Radius + " " + planet.radius.ToString("0.00") + " " + LanguageHelper.DrawText_km;
                        text += "\n" + LanguageHelper.DrawText_Mass + " " + planet.mass.ToString("0.00") + " " + LanguageHelper.DrawText_massearth;
                        text += "\n" + LanguageHelper.DrawText_Pleasure + " " + planet.atmosherepleasure.ToString("0.00") + (planet.atmosherepleasureoffcet != 0 ? ("[" + (planet.atmosherepleasureoffcet > 0 ? "+" : "") + (planet.atmosherepleasureoffcet.ToString("0.0")) + "]") : "") + " " + LanguageHelper.DrawText_atm;
                        text += "\n" + LanguageHelper.DrawText_Temperature + " " + ((planet.maxtemperature + planet.mintemperature) / 2).ToString("0.0") + (planet.temperatureoffcet != 0 ? ("[" + (planet.temperatureoffcet > 0 ? "+" : "") + (planet.temperatureoffcet.ToString("0.0")) + "]") : "") + " " + LanguageHelper.DrawText_deg;
                        Vector2 size2 = font.MeasureString(text);
                        spriteBatch.DrawString(font, text, new Vector2(rect2.X + (rect2.Width - (int)size2.X) / 2, rect2.Y + (rect2.Height - (int)size2.Y) / 2), Color.White);

                        if (messages) messagesystem.Draw(spriteBatch, font, width);
                        spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                        spriteBatch.End();
                    }
                }
                #endregion

                #region Menu
                if (state == MainMenuMode ||
                    state == StartGameMenuMode ||
                    state == SettingsMenuMode)
                //state == HelpMenuMode ||
                //state == AboutMenuMode||
                //state == UniverceMode ||
                //state == CatalogMode)
                //state == ChronoMenuMode||
                //state == PromoMenuMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);
                    if (messages) messagesystem.Draw(spriteBatch, font, width);
                    if (state == MainMenuMode && gui.forms[1].elements[13].enable)
                    {
                        string text = LanguageHelper.Gui_ChangeColor;
                        Vector2 size = font.MeasureString(text);
                        Rectangle rect = gui.forms[1].elements[13].rect;
                        spriteBatch.DrawString(font, text, new Vector2(rect.X + (rect.Width - (int)size.X) / 2, rect.Y + (rect.Height - (int)size.Y) / 2), GetPlayerColor(player_id));
                    }
                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion

                #region Univerce
                if (state == UniverceMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);
                    spriteBatch.End();

                    //if sector not selected - draw galaxy
                    if (selectedsector < 0)
                    {
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        spriteBatch.Draw(universetexture, gui.forms[8].elements[1].rect, Color.White);

                        int start_x = gui.forms[8].elements[1].rect.X;
                        int start_y = gui.forms[8].elements[1].rect.Y;

                        int x = ((int)camera.mouse.X - gui.forms[8].elements[1].rect.X) / 74; //selectedsector % 8;
                        int y = ((int)camera.mouse.Y - gui.forms[8].elements[1].rect.Y) / 74; //selectedsector / 8;

                        if (x >= 0 && x < 8 && y >= 0 && y < 8)
                        {
                            spriteBatch.Draw(guiset, new Rectangle(start_x + x * 74, start_y + y * 74, 74, 74), new Rectangle(80, 0, 16, 16), new Color(0.2f, 0.2f, 0.2f, 0.2f));
                            spriteBatch.DrawString(font, ((char)((int)'A' + x)).ToString() + ((char)((int)'1' + y)).ToString(), camera.mouse + new Vector2(15, 15), Color.White);
                        }
                        spriteBatch.End();
                    }
                    else
                    {
                        int x = selectedsector % 8;
                        int y = selectedsector / 8;

                        int start_x = gui.forms[8].elements[1].rect.X;
                        int start_y = gui.forms[8].elements[1].rect.Y;

                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        spriteBatch.Draw(universetexture, gui.forms[8].elements[1].rect, new Rectangle(x * 74, y * 74, 74, 74), Color.Black);

                        spriteBatch.DrawString(font, LanguageHelper.DrawText_Sector + " " + ((char)((int)'A' + x)).ToString() + ((char)((int)'1' + y)).ToString(), new Vector2(start_x + 665, start_y + 10), Color.White);

                        foreach (int i in univerce.subsectors[selectedsector])
                        {
                            Vector2 pos = (univerce.positions[i] - new Vector2(x * 74, y * 74)) * 8;
                            pos.X += start_x; pos.Y += start_y;

                            float a = (float)((univerce.stars[i].size + 1) / 2);
                            int starradius = (int)(4 + 4 * a);

                            spriteBatch.Draw(starset, new Rectangle((int)pos.X - starradius + 4, (int)pos.Y - starradius + 4, starradius * 2, starradius * 2), starGradient.GetColor(univerce.stars[i].temperature));
                        }

                        spriteBatch.End();
                    }

                    if (selectedstar >= 0)
                    {
                        int start_x = gui.forms[8].elements[1].rect.X;
                        int start_y = gui.forms[8].elements[1].rect.Y;

                        float a = (float)((univerce.stars[selectedstar].size + 1) / 2);
                        int size = (int)(96 * a + 96);

                        DrawStar(new Rectangle(start_x + 600 + 96 - size / 2, start_y + 40 + 96 - size / 2, size, size), starGradient.GetColor(univerce.stars[selectedstar].temperature));

                        string text = LanguageHelper.DrawText_Name + ": " + univerce.stars[selectedstar].name + "\n\n" +
                                      LanguageHelper.DrawText_Temperature + ": " + univerce.stars[selectedstar].temperature.ToString("0") + " " + LanguageHelper.DrawText_kelvin + "\n" +
                                      LanguageHelper.DrawText_Radius + ": " + (univerce.stars[selectedstar].radius / 0.00435).ToString("0.00") + " " + LanguageHelper.DrawText_radiussun + "\n" +
                                      LanguageHelper.DrawText_Mass + ": " + (univerce.stars[selectedstar].mass / 333000).ToString("0.00") + " " + LanguageHelper.DrawText_masssun + "\n" +
                                      LanguageHelper.DrawText_PlanetsNum + ": " + (univerce.stars[selectedstar].GetPlanetNum()).ToString();
                        spriteBatch.Begin();
                        spriteBatch.DrawString(font, text, new Vector2(start_x + 610, start_y + 250), Color.White);
                        spriteBatch.End();
                    }

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    if (messages) messagesystem.Draw(spriteBatch, font, width);
                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion

                #region PlanetMap
                if (state == PlanetMapMode)
                {
                    GraphicsDevice.Clear(Color.Black);

                    int freew = width;
                    int freeh = height - 60;
                    if (freew > freeh * 2) freew = freeh * 2;
                    if (freew < freeh * 2) freeh = freew / 2;

                    int startx = (width - freew) / 2;
                    int starty = (height - freeh) / 2;

                    DrawPlanetMap(GetColoFromPlanetGradient(planet, 0), (float)star.GetTotalTimeForLighting(), planettexture, planetnormal, new Rectangle(startx, starty, freew, freeh), mapEffect);

                    int size = 8;
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    for (int i = 0; i < planet.maps.Count; i++)
                    {
                        Vector2 v = planet.maps[i].position;
                        float dx = (size / 2) * freew / planet.mapwidth;
                        float dy = (size / 2) * freeh / planet.mapheight;
                        Rectangle r = new Rectangle((int)(startx + (v.X - size / 2) * freew / planet.mapwidth), (int)(starty + (v.Y - size / 2) * freeh / planet.mapheight), (int)dx * 2, (int)dy * 2);
                        float dark = planet.GetLightingWithoutRelief((int)v.X, (int)v.Y, (float)star.GetTotalTimeForLighting());
                        spriteBatch.Draw(baseset, r, new Rectangle(0, 0, 16, 16), new Color(dark, dark, dark));
                        string name = planet.maps[i].name + " [" + planet.maps[i].position.X.ToString("0") + ":" + planet.maps[i].position.Y.ToString("0") + "]";
                        Vector2 namesize = font.MeasureString(name);
                        Vector2 bazepos = new Vector2((int)(startx + (v.X) * freew / planet.mapwidth - namesize.X / 2), (int)(starty + (v.Y) * freeh / planet.mapheight - size - 2 - namesize.Y));
                        spriteBatch.End();
                        DrawSolidRectangle(new Rectangle((int)bazepos.X - 4, (int)bazepos.Y - 4, (int)namesize.X + 8, (int)namesize.Y + 8), new Color(0.1f, 0.1f, 0.1f, 0.1f));
                        DrawRectangle(new Rectangle((int)bazepos.X - 4, (int)bazepos.Y - 4, (int)namesize.X + 8, (int)namesize.Y + 8), GetPlayerColor(planet.maps[i].player_id));
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                        //spriteBatch.DrawString(font, name, new Vector2((int)(startx + (v.X) * freew / planet.mapwidth - namesize.X / 2), (int)(starty + (v.Y) * freeh / planet.mapheight - size - 2 - namesize.Y - 1)), Color.Black);
                        spriteBatch.DrawString(font, name, bazepos, Color.White);
                    }

                    for (int i = 0; i < planet.unitgroups.Count; i++)
                    {
                        Vector2 v = planet.unitgroups[i].position;
                        float dx = (size / 2) * freew / planet.mapwidth;
                        float dy = (size / 2) * freeh / planet.mapheight;
                        Rectangle r = new Rectangle((int)(startx + v.X * freew / planet.mapwidth - 2), (int)(starty + v.Y * freeh / planet.mapheight - 2), (int)4, (int)4);
                        DrawRectangle(r, GetPlayerColor(planet.unitgroups[i].player_id));
                    }

                    if (planetmode == MarketBaseMode && selectedbaseposition_x != -1 && selectedbaseposition_y != -1)
                    {
                        if (selectedbaseposition2_x != -1 && selectedbaseposition2_y != -1)
                            DrawLine(new Vector2(startx + selectedbaseposition_x * freew / planet.mapwidth, starty + selectedbaseposition_y * freeh / planet.mapheight), new Vector2(startx + selectedbaseposition2_x * freew / planet.mapwidth, starty + selectedbaseposition2_y * freeh / planet.mapheight), Color.GreenYellow, Color.GreenYellow);
                        else
                            DrawLine(new Vector2(startx + selectedbaseposition_x * freew / planet.mapwidth, starty + selectedbaseposition_y * freeh / planet.mapheight), camera.mouse, Color.GreenYellow, Color.GreenYellow);
                    }
                    if (planetmode == AttackBaseMode && selectedbaseposition_x != -1 && selectedbaseposition_y != -1)
                    {
                        if (selectedbaseposition2_x != -1 && selectedbaseposition2_y != -1)
                            DrawLine(new Vector2(startx + selectedbaseposition_x * freew / planet.mapwidth, starty + selectedbaseposition_y * freeh / planet.mapheight), new Vector2(startx + selectedbaseposition2_x * freew / planet.mapwidth, starty + selectedbaseposition2_y * freeh / planet.mapheight), Color.OrangeRed, Color.OrangeRed);
                        else
                            DrawLine(new Vector2(startx + selectedbaseposition_x * freew / planet.mapwidth, starty + selectedbaseposition_y * freeh / planet.mapheight), camera.mouse, Color.OrangeRed, Color.OrangeRed);
                    }

                    foreach (PlanetModule m in planet.modules)
                    {
                        Vector2 v = m.pos;
                        //float dx = (size / 2) * freew / planet.mapwidth;
                        //float dy = (size / 2) * freeh / planet.mapheight;
                        Rectangle r = new Rectangle((int)(startx + v.X * freew / planet.mapwidth - 2), (int)(starty + v.Y * freeh / planet.mapheight - 2), (int)4, (int)4);

                        DrawRectangle(r, Color.Green);
                    }

                    foreach (Meteorite m in planet.meteorites)
                    {
                        Vector2 v = m.pos;
                        //float dx = (size / 2) * freew / planet.mapwidth;
                        // float dy = (size / 2) * freeh / planet.mapheight;
                        int posx = (int)(startx + v.X * freew / planet.mapwidth);
                        int posy = (int)(starty + v.Y * freeh / planet.mapheight);

                        if (m.timetohit > 0)
                        {
                            DrawLine(posx - 1, posy - 4, posx - 1, posy + 2, Color.White, Color.White);
                            DrawLine(posx - 4, posy - 1, posx + 2, posy - 1, Color.White, Color.White);

                            DrawLine(posx, posy - 3, posx, posy + 3, Color.Red, Color.Red);
                            DrawLine(posx - 3, posy, posx + 3, posy, Color.Red, Color.Red);
                        }
                        else
                        {
                            Color color = Color.Red;
                            if (m.timetodestroy > 1.5) color.A = (byte)(255 * (1.5f - (m.timetodestroy - 1.5f)));
                            Rectangle r = new Rectangle((int)(posx - (6 - m.timetodestroy)), (int)(posy - (6 - m.timetodestroy)), (int)((6 - m.timetodestroy)), (int)((6 - m.timetodestroy)));
                            DrawRectangle(r, color);
                        }
                    }

                    spriteBatch.End();

                    if (drawgui)
                    {
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);

                        gui.Draw(state, spriteBatch, guiset, font);
                        if (messages) messagesystem.Draw(spriteBatch, font, width);

                        //Draw interactive forms
                        int panelsizex = 557 + 16 + 17;
                        int panelstartx = (width - panelsizex) / 2;
                        int panelstarty = (height - 392) / 2;

                        #region popup
                        Form popupform = gui.GetPopup();
                        if (popupform != null)
                        {
                            //market
                            if (gui.popupformid == 5)
                            {
                                //5 - name 1
                                //13 - type 1
                                //21 - updown 1
                                //29 - name 2
                                //37 - type 2
                                //45 - updown 2
                                for (int k = 0; k < 2; k++)//k - id of base: first or second
                                    for (int i = 0; i < 8; i++)
                                    {
                                        if (tradegrouptypes[k, i] == -1)
                                        {
                                            popupform.elements[5 + i + (k * 24)].enable = false;
                                            popupform.elements[21 + i + (k * 24)].enable = false;
                                            spriteBatch.DrawString(font, "+", new Vector2(popupform.elements[13 + i + (k * 24)].rect.X + 11, popupform.elements[13 + i + (k * 24)].rect.Y + 5), Color.White);
                                        }
                                        else
                                        {
                                            popupform.elements[5 + i + (k * 24)].enable = true;
                                            popupform.elements[21 + i + (k * 24)].enable = true;

                                            popupform.elements[5 + i + (k * 24)].text[0] = Language.GetShortNameOfResource(tradegrouptypes[k, i]);
                                            int count = (int)tradegroupcount[k, i];
                                            string countstring = count >= 1000000 ? ((count / 1000).ToString() + "K") : count.ToString();
                                            spriteBatch.DrawString(font, "x " + countstring, new Vector2(popupform.elements[5 + i + (k * 24)].rect.X + popupform.elements[5 + i + (k * 24)].rect.Width + 6, popupform.elements[5 + i + (k * 24)].rect.Y + 5), Color.White);
                                            Rectangle rect = popupform.elements[13 + i + (k * 24)].rect;
                                            spriteBatch.Draw(resourceset, new Rectangle(rect.X + (rect.Width - 16) / 2, rect.Y + (rect.Height - 16) / 2, 16, 16), new Rectangle((tradegrouptypes[k, i]) % 8 * 16, (tradegrouptypes[k, i]) / 8 * 16, 16, 16), Color.White);
                                        }
                                    }
                                spriteBatch.Draw(guiset, new Rectangle(panelstartx + panelsizex / 2, popupform.elements[13].rect.Y - 1, 1, 250), new Rectangle(98, 38, 1, 1), Color.White);
                            }

                            //submarket
                            if (gui.popupformid == 6)
                            {
                                int x = ((int)camera.mouse.X - popupform.elements[1].rect.X) / 32;
                                int y = ((int)camera.mouse.Y - popupform.elements[1].rect.Y) / 32;

                                if (popupform.elements[1].rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))
                                    GuiObject.DrawDarkPanel(spriteBatch, guiset, new Rectangle(popupform.elements[1].rect.X + x * 32, popupform.elements[1].rect.Y + y * 32, 32, 32));

                                for (int i = 0; i < Map.maxresources; i++)
                                {
                                    int rx = i % 8;
                                    int ry = i / 8;

                                    spriteBatch.Draw(resourceset, new Rectangle(popupform.elements[1].rect.X + rx * 32 + 8, popupform.elements[1].rect.Y + ry * 32 + 8, 16, 16), new Rectangle(rx * 16, ry * 16, 16, 16), Color.White);
                                }
                            }

                            //modules
                            if (gui.popupformid == 9)
                            {
                                for (int i = 0; i < 9; i++)
                                    popupform.elements[8 + i].enable = false;

                                for (int i = 0; i < 9; i++)
                                {
                                    if (i == planet.modules.Count) break;

                                    popupform.elements[8 + i].enable = true;
                                    Rectangle rect = gui.popupform.elements[8 + i].rect;
                                    spriteBatch.DrawString(font, planet.maps[planet.modules[startmedulesitem + i].base_id].name + " - " + (planet.modules[startmedulesitem + i].type == 0 ? "Климат" : "Защита"), new Vector2(panelstartx + 8, rect.Y + 5), Color.White);
                                }

                            }

                            //attack
                            if (gui.popupformid == 22)
                            {
                                spriteBatch.Draw(guiset, new Rectangle(panelstartx + 295, popupform.elements[4].rect.Y + 37 + 23, 1, 280), new Rectangle(98, 38, 1, 1), Color.White);

                                if (minimaprocket != null)
                                    spriteBatch.Draw(minimaprocket, popupform.elements[5].rect, Color.White);
                            }
                        }
                        #endregion

                        if (planetmode == AttackBaseMode)
                            spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20 - 12, 3 * 16, 12, 16), Color.White);
                        else
                            spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                        spriteBatch.End();
                    }

                }
                #endregion

                #region LocalMap
                if (state == LocalMapMode)
                {
                    #region Draw shields
                    GraphicsDevice.SetRenderTarget(powershieldslayer);
                    GraphicsDevice.Clear(new Color(0, 0, 0, 0));

                    spriteBatch.Begin();
                    foreach (Shield s in map.shields)
                    {
                        if (s.type == Shield.Power)
                        {
                            float size = s.size * 32;
                            spriteBatch.Draw(shieldset, new Rectangle((int)(s.pos.X * 32 - camera.x - size), (int)(s.pos.Y * 32 - camera.y - size / 2), (int)(size * 2), (int)(size)), Color.Blue);
                        }
                    }
                    spriteBatch.End();

                    GraphicsDevice.SetRenderTarget(emmisionshieldslayer);
                    GraphicsDevice.Clear(new Color(0, 0, 0, 0));

                    spriteBatch.Begin();
                    foreach (Shield s in map.shields)
                    {
                        if (s.type == Shield.Emmision)
                        {
                            float size = s.size * 32;
                            spriteBatch.Draw(shieldset, new Rectangle((int)(s.pos.X * 32 - camera.x - size), (int)(s.pos.Y * 32 - camera.y - size / 2), (int)(size * 2), (int)(size)), Color.Red);
                        }
                    }
                    spriteBatch.End();

                    GraphicsDevice.SetRenderTarget(atmoshereshieldslayer);
                    GraphicsDevice.Clear(new Color(0, 0, 0, 0));

                    spriteBatch.Begin();
                    foreach (Shield s in map.shields)
                    {
                        if (s.type == Shield.Atmosphere)
                        {
                            float size = s.size * 32;
                            spriteBatch.Draw(shieldset, new Rectangle((int)(s.pos.X * 32 - camera.x - size), (int)(s.pos.Y * 32 - camera.y - size / 2), (int)(size * 2), (int)(size)), Color.Green);
                        }
                    }
                    spriteBatch.End();
                    #endregion

                    GraphicsDevice.SetRenderTarget(screencapture);
                    GraphicsDevice.Clear(Color.DarkBlue);

                    //spriteBatch.Begin();
                    int startx = (int)(camera.x / 32) - 1; if (startx < 0) startx = 0;
                    int starty = (int)(camera.y / 32) - 1; if (starty < 0) starty = 0;
                    int endx = startx + (width / 32); if (endx > map.width) endx = map.width;
                    int endy = starty + (height / 32) + 6; if (endy > map.height) endy = map.height;

                    tileEffect.Parameters["Camera"].SetValue(Matrix.CreateTranslation(-(int)camera.x, -(int)camera.y, 0));
                    //tileEffect.CurrentTechnique = null;
                    fractiontileEffect.Parameters["Camera"].SetValue(Matrix.CreateTranslation(-(int)camera.x, -(int)camera.y, 0));
                    fractiontileEffect.Parameters["FractionTexture"].SetValue(fractionset);
                    fractiontileEffect.Parameters["FractionTemplate1"].SetValue(new Color(255, 243, 46).ToVector3());
                    fractiontileEffect.Parameters["FractionTemplate2"].SetValue(new Color(223, 213, 52).ToVector3());

                    #region Prepare map to draw
                    mapHelper.Clear();
                    List<WaitParticle> waitparticles = new List<WaitParticle>();
                    nextparticletime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    for (int i = 0; i < map.buildings.Count; i++)
                    {
                        Point size = Building.GetSize(map.buildings[i].type);
                        Point pos = new Point((int)map.buildings[i].pos.X, (int)map.buildings[i].pos.Y);
                        int lightsize = (size.X + size.Y) * 2 / 3;
                        if (map.buildings[i].type == Building.Turret && map.buildings[i].power > 0) lightsize = map.buildings[i].wait >= 1 ? 3 : 8;
                        if (map.buildings[i].type == Building.TerrainScaner && map.player_id == player_id)
                        {
                            int scansize = Constants.Building_ScanerSize;
                            for (int x = -scansize; x < size.X + scansize; x++)
                                for (int y = -scansize; y < size.Y + scansize; y++)
                                {
                                    int px = x <= 0 ? x : (x >= size.X ? x - size.X + 1 : 0);
                                    int py = y <= 0 ? y : (y >= size.Y ? y - size.Y + 1 : 0);
                                    float length = new Vector2(px, py * 2).Length();
                                    if (map.onMap(pos.X - x, pos.Y - y) && length <= scansize + 1)
                                        mapHelper.data[pos.Y - y, pos.X - x].scaned = true;
                                }
                        }

                        if (map.buildings[i].buildingtime <= 0)
                        {
                            for (int x = -lightsize; x < size.X + lightsize; x++)
                                for (int y = -lightsize; y < size.Y + lightsize; y++)
                                {
                                    int px = x <= 0 ? x : (x >= size.X ? x - size.X + 1 : 0);
                                    int py = y <= 0 ? y : (y >= size.Y ? y - size.Y + 1 : 0);
                                    float length = new Vector2(px, py * 2).Length();
                                    if (map.onMap(pos.X - x, pos.Y - y) && length <= lightsize + 1)
                                        mapHelper.data[pos.Y - y, pos.X - x].lighting += (lightsize + 1 - length) / 7;
                                }
                        }
                        else
                        {
                            if (map.buildings[i].isbuildingnow)
                            {
                                if (nextparticletime <= 0)
                                    mapHelper.particles.Add(new Particle(new Vector2(pos.X, pos.Y), Building.GetAnchor(map.buildings[i].type), Building.GetSource(map.buildings[i].type), 2, new Vector2(0, -30)));
                                map.buildings[i].isbuildingnow = false;
                            }
                        }
                    }

                    if (nextparticletime < 0)
                        nextparticletime = 0.15f;

                    for (int i = mapHelper.particles.Count - 1; i >= 0; i--)
                    {
                        mapHelper.particles[i].offcet += mapHelper.particles[i].speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        mapHelper.particles[i].life -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (mapHelper.particles[i].life <= 0) mapHelper.particles.RemoveAt(i);
                    }

                    for (int i = 0; i < map.shields.Count; i++)
                    {
                        for (int x = -(int)map.shields[i].size; x <= (int)map.shields[i].size; x++)
                            for (int y = -(int)(map.shields[i].size) / 2; y <= (int)(map.shields[i].size / 2); y++)
                            {
                                Point pos = new Point((int)map.shields[i].pos.X, (int)map.shields[i].pos.Y);
                                Vector2 det = new Vector2(x, y * 2);
                                float length = det.Length();
                                if (map.onMap(pos.X + x, pos.Y + y) && length <= map.shields[i].size)
                                    mapHelper.data[pos.Y + y, pos.X + x].lighting += (map.shields[i].size - length) / 5;
                            }
                    }

                    float offcet = (float)star.GetTotalTimeForLighting();
                    float dark = planet.GetLightingWithoutRelief((int)map.position.X, (int)map.position.Y, offcet);
                    Color sunlighting = new Color(dark, dark, dark);
                    for (int i = starty; i < endy; i++)
                    {
                        for (int j = startx; j < endx; j++)
                        {
                            int k = (i * map.height + j) * 4;

                            float c1 = mapHelper.data[i, j].lighting;
                            float c2 = j >= map.width - 1 ? c1 : mapHelper.data[i, j + 1].lighting;
                            float c3 = j >= map.width - 1 || i >= map.height - 1 ? c1 : mapHelper.data[i + 1, j + 1].lighting;
                            float c4 = i >= map.height - 1 ? c1 : mapHelper.data[i + 1, j].lighting;

                            terrainvertexes[k].Color = Color.Lerp(sunlighting, Color.White, c1 / 3); ;
                            terrainvertexes[k + 1].Color = Color.Lerp(sunlighting, Color.White, c2 / 3);
                            terrainvertexes[k + 2].Color = Color.Lerp(sunlighting, Color.White, c3 / 3);
                            terrainvertexes[k + 3].Color = Color.Lerp(sunlighting, Color.White, c4 / 3);
                            if (mapHelper.data[i, j].subterrainvertexid >= 0)
                            {
                                subterrainvertexes[mapHelper.data[i, j].subterrainvertexid * 4].Color = terrainvertexes[k].Color;
                                subterrainvertexes[mapHelper.data[i, j].subterrainvertexid * 4 + 1].Color = terrainvertexes[k + 1].Color;
                                subterrainvertexes[mapHelper.data[i, j].subterrainvertexid * 4 + 2].Color = terrainvertexes[k + 2].Color;
                                subterrainvertexes[mapHelper.data[i, j].subterrainvertexid * 4 + 3].Color = terrainvertexes[k + 3].Color;
                            }
                            if (mapHelper.data[i, j].subterrainvertexid2 >= 0)
                            {
                                subterrainvertexes[mapHelper.data[i, j].subterrainvertexid2 * 4].Color = terrainvertexes[k].Color;
                                subterrainvertexes[mapHelper.data[i, j].subterrainvertexid2 * 4 + 1].Color = terrainvertexes[k + 1].Color;
                                subterrainvertexes[mapHelper.data[i, j].subterrainvertexid2 * 4 + 2].Color = terrainvertexes[k + 2].Color;
                                subterrainvertexes[mapHelper.data[i, j].subterrainvertexid2 * 4 + 3].Color = terrainvertexes[k + 3].Color;
                            }
                            if (mapHelper.data[i, j].subterrainvertexid3 >= 0)
                            {
                                if (i > 1)
                                {
                                    short yoffcet = (short)(map.width * 4 * 2);
                                    short yoffcet2 = (short)(map.width * 4);
                                    subterrainvertexes[mapHelper.data[i, j].subterrainvertexid3 * 4].Color = terrainvertexes[k - yoffcet].Color;
                                    subterrainvertexes[mapHelper.data[i, j].subterrainvertexid3 * 4 + 1].Color = terrainvertexes[k + 1 - yoffcet].Color;
                                    subterrainvertexes[mapHelper.data[i, j].subterrainvertexid3 * 4 + 2].Color = terrainvertexes[k + 2 - yoffcet2].Color;
                                    subterrainvertexes[mapHelper.data[i, j].subterrainvertexid3 * 4 + 3].Color = terrainvertexes[k + 3 - yoffcet2].Color;
                                }
                                else
                                {
                                    subterrainvertexes[mapHelper.data[i, j].subterrainvertexid3 * 4].Color = terrainvertexes[k].Color;
                                    subterrainvertexes[mapHelper.data[i, j].subterrainvertexid3 * 4 + 1].Color = terrainvertexes[k + 1].Color;
                                    subterrainvertexes[mapHelper.data[i, j].subterrainvertexid3 * 4 + 2].Color = terrainvertexes[k + 2].Color;
                                    subterrainvertexes[mapHelper.data[i, j].subterrainvertexid3 * 4 + 3].Color = terrainvertexes[k + 3].Color;
                                }
                            }
                        }
                    }

                    #endregion

                    #region Draw map
                    Color lighting = GetColoFromPlanetGradient(planet, planet.heightmap[(int)map.position.Y, (int)map.position.X]);
                    int gray = (lighting.R + lighting.G + lighting.B) / 3;
                    lighting = Color.Lerp(lighting, Color.White, gray / 255f + 0.3f);
                    lighting = new Color(new Vector3(lighting.R / 255f * sunlighting.R / 255f,
                                                     lighting.G / 255f * sunlighting.G / 255f,
                                                     lighting.B / 255f * sunlighting.B / 255f));


                    GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    tileEffect.Parameters["Texture"].SetValue(tileset);

                    foreach (EffectPass pass in tileEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList,
                                            subterrainvertexes,
                                            0,
                                            subterrainvertexes.Length,
                                            subterrainindexes,
                                            0,
                                            subterrainindexes.Length / 3);
                    }

                    for (int i = starty; i < endy; i++)
                    {
                        foreach (EffectPass pass in tileEffect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            //GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList,
                            //                    terrainvertexes,
                            //                    0,
                            //                    map.width * 4,
                            //                    terrainindexes,
                            //                    i * map.width * 6,
                            //                    map.width * 2);

                            GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList,
                                                terrainvertexes,
                                                i * map.width * 4,
                                                map.width * 4,
                                                terrainindexes,
                                                0,
                                                map.width * 2);
                        }
                    }

                    float ydepthoffset = 0.6f / map.height;
                    for (int i = starty; i < endy; i++)
                        for (int j = startx; j < endx; j++)
                        {
                            if (map.data[i, j].build_draw && map.data[i, j].build_id < map.buildings.Count && map.data[i, j].build_id >= 0)
                            {
                                Building b = map.buildings[map.data[i, j].build_id];
                                float transparent = 0.5f + (0.5f - (b.buildingtime / b.maxbuildingtime) / 2);
                                Rectangle source = Building.GetSource(map.buildings[map.data[i, j].build_id].type);
                                Vector2 anchor = Building.GetAnchor(map.buildings[map.data[i, j].build_id].type);

                                Color light = Color.Lerp(lighting, Color.White, mapHelper.data[i, j].lighting);

                                if (b.type == Building.Farm)
                                    source.X += (source.Width * (b.recipte == 0 ? 0 : (b.recipte - 1)));
                                else if (b.type == Building.ClosedFarm)
                                    source.Y += (source.Height * b.recipte);
                                else if (b.type == Building.Turret && b.buildingtime <= 0 && b.wait <= 0 && b.power > 0)
                                    source.Y += source.Height;
                                else if (b.type == Building.RocketParking)
                                    source.Y += (source.Height * b.recipte);


                                if (b.height > 1)
                                {
                                    if (b.type == Building.AtmosophereShield || b.type == Building.PowerShield || b.type == Building.EmmisionShield)
                                    {
                                        Rectangle tempsource = Building.GetSource(Building.UnderShield);
                                        DrawFractionSprite(buildset, new Vector2(j * 32, i * 32) - anchor, tempsource, b.buildingtime > 0 ? new Color(transparent, transparent, transparent, transparent) : light, GetZ(i * 32), map.player_id);
                                        tempsource.Y += tempsource.Height;
                                        for (int h = 1; h < b.height - 1; h++)
                                            DrawFractionSprite(buildset, new Vector2(j * 32, i * 32 - h * 16) - anchor, tempsource, b.buildingtime > 0 ? new Color(transparent, transparent, transparent, transparent) : light, GetZ(i * 32), map.player_id);
                                        DrawFractionSprite(buildset, new Vector2(j * 32, i * 32 - (b.height - 1) * 16) - anchor, source, b.buildingtime > 0 ? new Color(transparent, transparent, transparent, transparent) : light, GetZ(i * 32), map.player_id);
                                    }
                                    else
                                    {
                                        DrawFractionSprite(buildset, new Vector2(j * 32, i * 32) - anchor, source, b.buildingtime > 0 ? new Color(transparent, transparent, transparent, transparent) : light, GetZ(i * 32), map.player_id);
                                        source.Y += source.Height;
                                        for (int h = 1; h < b.height; h++)
                                            DrawFractionSprite(buildset, new Vector2(j * 32, i * 32 - h * 16) - anchor, source, b.buildingtime > 0 ? new Color(transparent, transparent, transparent, transparent) : light, GetZ(i * 32), map.player_id);
                                    }
                                }
                                else
                                    DrawFractionSprite(buildset, new Vector2(j * 32, i * 32) - anchor, source, b.buildingtime > 0 ? new Color(transparent, transparent, transparent, transparent) : light, GetZ(i * 32), map.player_id);

                                if (b.power == 0 && b.buildingtime == 0)
                                {
                                    Point size = Building.GetSize(b.type);
                                    size.X--; size.Y--;
                                    DrawFractionSprite(guiset, new Vector2(j * 32 - size.X * 16, i * 32 - size.Y * 16), new Rectangle(32, 32, 32, 32), Color.White, GetZ(i * 32), map.player_id);
                                }

                                if (b.buildingtime > 0)
                                {
                                    Color c = new Color(117, 78, 47);
                                    Color bc = new Color(224, 107, 2);
                                    waitparticles.Add(new WaitParticle(j * 32 - (int)anchor.X + 1, i * 32 + 37, source.Width - 2, c, bc, b.buildingtime / b.maxbuildingtime));
                                }

                                if (b.health < source.Width)
                                {
                                    Color c = new Color(117, 0, 47);
                                    Color bc = new Color(224, 0, 2);
                                    waitparticles.Add(new WaitParticle(j * 32 - (int)anchor.X + 1, i * 32 + 37 - 10, source.Width - 2, c, bc, ((float)b.health) / source.Width));

                                    //DrawLine(j * 32 - (int)anchor.X, i * 32 + 30, j * 32 - (int)anchor.X + (int)b.health, i * 32 + 31, Color.Red, Color.Red);
                                }
                            }

                            if (build_id > 0 && !map.data[i, j].can_build)
                                DrawSprite(guiset, new Rectangle(j * 32, i * 32, 32, 32), new Rectangle(30, 66, 32, 32), Color.White, 0);

                            if (build_id == Building.Mine)
                                if (map.data[i, j].mineresource_id >= 0 && (map.science[0].items[Constants.Research_terrainexplored].searched || mapHelper.data[i, j].scaned))
                                    DrawSprite(resourceset, new Rectangle(j * 32 + 8, i * 32 + 8, 16, 16), new Rectangle(map.data[i, j].mineresource_id % 8 * 16, map.data[i, j].mineresource_id / 8 * 16, 16, 16), Color.White, 0);
                            if (build_id == Building.Dirrick)
                                if (map.data[i, j].dirrickresource_id >= 0 && (map.science[0].items[Constants.Research_terrainexplored].searched || mapHelper.data[i, j].scaned))
                                    DrawSprite(resourceset, new Rectangle(j * 32 + 8, i * 32 + 8, 16, 16), new Rectangle(map.data[i, j].dirrickresource_id % 8 * 16, map.data[i, j].dirrickresource_id / 8 * 16, 16, 16), Color.White, 0);
                        }


                    foreach (Unit u in map.units)
                    {
                        if (u.pos.X >= 0 && u.pos.Y >= 0 && u.pos.X < map.width && u.pos.Y < map.height)
                        {
                            Rectangle source = Unit.GetSource(u.type);
                            if (u.type != Unit.RocketAtom && u.type != Unit.RocketTwined && u.type != Unit.RocketNeitron)
                                source.X = source.Width * u.direction;
                            else if (u.command.message == commands.rocketfallingdown)
                            {
                                source.Y += source.Height;
                                source.Height = -source.Height;
                            }

                            Vector2 pos = new Vector2((int)(u.pos.X * 32), (int)(u.pos.Y * 32) - (int)(u.height * 32));

                            Color light = Color.Lerp(lighting, Color.White, mapHelper.data[(int)(u.pos.Y), (int)(u.pos.X)].lighting);

                            Vector2 anchor = Unit.GetAnchor(u.type);
                            DrawFractionSprite(unitset, pos - anchor, source, light, GetZ((int)(u.pos.Y * 32 + anchor.Y)), u.player_id);

                            if (u.wait > 0 && (u.command != null && (u.command.message != commands.gotoparking && u.command.message != commands.tradegoup)))
                            {
                                Color c = new Color(117, 78, 47);
                                Color bc = new Color(224, 107, 2);
                                waitparticles.Add(new WaitParticle((int)pos.X - (int)anchor.X, (int)pos.Y + 5, source.Width, c, bc, u.wait / u.maxwait));
                            }
                        }
                    }

                    foreach (Particle p in mapHelper.particles)
                    {
                        float transparent = p.life / p.maxlife;
                        DrawSprite(buildingparticlesset, p.pos * 32 + p.offcet - p.anchor, p.source, new Color(transparent, transparent, transparent, transparent), GetZ(p.pos.Y * 32 + 38));
                    }

                    if (waitparticles.Count > 0)
                    {
                        foreach (WaitParticle p in waitparticles)
                        {
                            p.startx -= (int)camera.x;
                            p.starty -= (int)camera.y;
                            p.endx -= (int)camera.x;

                            int s = (int)((p.endx - p.startx) * p.p);
                            DrawLine(p.startx, p.starty + 1, p.endx, p.starty + 1, p.backcolor, p.backcolor);
                            DrawLine(p.endx - s, p.starty + 1, p.endx, p.starty + 1, p.color, p.color);

                            DrawLine(p.startx, p.starty, p.endx, p.starty, p.backcolor, p.backcolor);
                            DrawLine(p.startx, p.starty + 2, p.endx, p.starty + 2, p.backcolor, p.backcolor);
                        }
                        waitparticles.Clear();
                    }

                    spriteBatch.Begin();
                    foreach (Meteorite m in map.meteorites)
                    {
                        if (m.timetohit <= 0)
                        {
                            Color color = Color.White;
                            if (m.timetodestroy > Constants.Map_meteoriteexplotiomaxsize / 2)
                            {
                                float tr = 1 - (m.timetodestroy - (Constants.Map_meteoriteexplotiomaxsize / 2)) / (Constants.Map_meteoriteexplotiomaxsize / 2);
                                color = new Color(tr, tr, tr, tr);
                            }
                            spriteBatch.Draw(explotionset, new Rectangle((int)((m.pos.X * 32) - camera.x - m.timetodestroy + meteoriteset.Width / 2 - 5), 10 + (int)((m.pos.Y * 32) - camera.y - m.timetodestroy), (int)(m.timetodestroy * 2), (int)(m.timetodestroy * 1.5f)), color);
                        }
                        else
                        {
                            spriteBatch.Draw(meteoriteset, new Rectangle((int)(((m.pos.X - m.timetohit * 100) * 32) - camera.x), (int)(((m.pos.Y - m.timetohit * 100) * 32) - camera.y + m.timetohit * 100 * 32), meteoriteset.Width, meteoriteset.Height / 2), new Color(0, 0, 0, 0.1f));
                            spriteBatch.Draw(meteoriteset, new Vector2((int)((m.pos.X - m.timetohit * 100) * 32) - camera.x, (int)((m.pos.Y - m.timetohit * 100) * 32) - camera.y - meteoriteset.Height / 2), Color.White);
                        }
                    }
                    #endregion

                    #region Draw transparent building is need
                    if (build_id > 0 && camera.mouse.X < width - 200)
                    {
                        Point size = Building.GetSize(build_id);
                        spriteBatch.Draw(guiset, new Vector2((camera.onx - size.X + 1) * 32 - camera.x, (camera.ony - size.Y + 1) * 32 - camera.y), new Rectangle(32, 0, 16, 16), new Color(128, 128, 128, 128));
                        spriteBatch.Draw(guiset, new Vector2((camera.onx) * 32 - camera.x + 16, (camera.ony - size.Y + 1) * 32 - camera.y), new Rectangle(48, 0, 16, 16), new Color(128, 128, 128, 128));
                        spriteBatch.Draw(guiset, new Vector2((camera.onx) * 32 - camera.x + 16, (camera.ony) * 32 - camera.y + 16), new Rectangle(48, 16, 16, 16), new Color(128, 128, 128, 128));
                        spriteBatch.Draw(guiset, new Vector2((camera.onx - size.X + 1) * 32 - camera.x, (camera.ony) * 32 - camera.y + 16), new Rectangle(32, 16, 16, 16), new Color(128, 128, 128, 128));
                        spriteBatch.Draw(buildset, new Vector2(camera.onx * 32 - camera.x, camera.ony * 32 - camera.y) - Building.GetAnchor(build_id), Building.GetSource(build_id), new Color(128, 128, 128, 128));
                        if (gui.popupform == null) spriteBatch.DrawString(font, Building.GetBuildPrice(build_id).ToString() + " Кр.", camera.mouse + new Vector2(15, 0), Color.White);
                    }
                    #endregion

                    spriteBatch.Draw(powershieldslayer, new Vector2(0, 0), new Color(0.05f, 0.05f, 0.05f, 0.05f));
                    spriteBatch.Draw(emmisionshieldslayer, new Vector2(0, 0), new Color(0.05f, 0.05f, 0.05f, 0.05f));
                    spriteBatch.Draw(atmoshereshieldslayer, new Vector2(0, 0), new Color(0.05f, 0.05f, 0.05f, 0.05f));

                    if (planet.timetosolarstrike < 1)
                    {
                        float baselighting = planet.GetLighting((int)map.position.X, (int)map.position.Y, offcet);
                        if (baselighting > 0.1f)
                        {
                            if (baselighting > 1) baselighting = 1;
                            float transparent = (1 - planet.timetosolarstrike) / 2 * ((baselighting - 0.1f) / 0.9f);
                            spriteBatch.Draw(explotionset, new Rectangle(0, 0, width - 200, height), new Rectangle(256, 256, 1, 1), new Color(transparent, transparent, transparent, transparent));
                        }
                    }

                    spriteBatch.End();

                    GraphicsDevice.SetRenderTarget(null);
                    spriteBatch.Begin();
                    spriteBatch.Draw(screencapture, new Vector2(0, 0), Color.White);
                    spriteBatch.End();

                    if (drawgui)
                    {
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);

                        if (gui.popupform != null)
                            for (int i = 0; i < width - 200; i += 32)
                                for (int j = 0; j < height; j += 32)
                                {
                                    spriteBatch.Draw(guiset, new Vector2(i, j), new Rectangle(64, 48, 32, 32), Color.White);
                                }

                        //Draw background
                        spriteBatch.Draw(guiset, new Rectangle(width - 200, 0, 200, height), new Rectangle(30, 98, 30, 30), Color.White);
                        spriteBatch.Draw(guiset, new Rectangle(0, 0, width, 30), new Rectangle(0, 98, 30, 30), Color.White);
                        spriteBatch.Draw(guiset, new Rectangle(0, height - 30, width, 30), new Rectangle(0, 98 - 30, 30, 30), Color.White);

                        GuiObject.DrawPanel(spriteBatch, guiset, new Rectangle(0, 0, width, 30));
                        GuiObject.DrawPanel(spriteBatch, guiset, new Rectangle(0, height - 30, width, 30));
                        GuiObject.DrawPanel(spriteBatch, guiset, new Rectangle(width - 200, 230, 200, height - 230 - 30));
                        GuiObject.DrawDarkPanel(spriteBatch, guiset, new Rectangle(width - 200, 30, 200, 200));

                        //Draw minimap
                        Rectangle mapRect = new Rectangle(width - 190 + 180 * startx / map.width, 38 + 180 * starty / map.height, 180 * (endx - startx) / map.width + 1, 180 * (endy - starty) / map.height + 1);
                        spriteBatch.Draw(minimap, new Rectangle(width - 190, 38, 180, 180), sunlighting);
                        foreach (Building b in map.buildings)
                        {
                            Point size = Building.GetSize(b.type);
                            int x = (int)b.pos.X - size.X + 1;
                            int y = (int)b.pos.Y - size.Y + 1;
                            spriteBatch.Draw(guiset, new Rectangle(width - 190 + 180 * x / map.width, 38 + 180 * y / map.height, 180 * size.X / map.width, 180 * size.Y / map.height), new Rectangle(10, 0, 10, 10), sunlighting);
                        }
                        foreach (Unit u in map.units)
                        {
                            spriteBatch.Draw(guiset, new Rectangle(width - 190 + (int)(180 * u.pos.X / map.width), 38 + (int)(180 * u.pos.Y / map.height), 1, 1), new Rectangle(10, 10, 10, 10), sunlighting);
                        }
                        GuiObject.DrawRectangleTransparent(spriteBatch, guiset, mapRect);

                        //Draw info
                        string text = LanguageHelper.DrawText_Energy + ": " + IntToShortString((int)map.inventory[(int)Resources.energy].count);
                        text += "   " + LanguageHelper.DrawText_Population + ": " + IntToShortString((int)map.population);
                        text += "   " + LanguageHelper.DrawText_Credits2 + ": " + IntToShortString((int)map.inventory[(int)Resources.credits].count);

                        if (font.MeasureString(text).X > width - 529)
                        {
                            text = LanguageHelper.DrawText_Energyshort + ": " + IntToShortString((int)map.inventory[(int)Resources.energy].count);
                            text += "   " + LanguageHelper.DrawText_Populationshort + ": " + IntToShortString((int)map.population);
                            text += "   " + LanguageHelper.DrawText_Credits2short + ": " + IntToShortString((int)map.inventory[(int)Resources.credits].count);
                        }

                        Vector2 tsize = font.MeasureString(text);

                        gui.Draw(state, spriteBatch, guiset, font);
                        if (messages) messagesystem.Draw(spriteBatch, font, width);

                        spriteBatch.DrawString(font, text, new Vector2(529 + (int)((width - 529 - tsize.X) / 2), 6), Color.White);

                        //Draw interactive forms
                        int panelsizex = 557 + 16 + 17;
                        int panelstartx = (width - 200 - panelsizex) / 2;
                        int panelstarty = (height - 392) / 2;

                        #region popup
                        Form popupform = gui.GetPopup();
                        if (popupform != null)
                        {
                            //energy menu
                            if (gui.popupformid == 2)
                            {
                                // 12 - index of first element of current consum
                                // 26 - index of first slider
                                popupform.elements[12].text[0] = (popupform.elements[26].reserved * 100).ToString("0") + "%";
                                popupform.elements[13].text[0] = (popupform.elements[27].reserved * 100).ToString("0") + "%";
                                popupform.elements[14].text[0] = (popupform.elements[28].reserved * 100).ToString("0") + "%";
                                popupform.elements[15].text[0] = (popupform.elements[29].reserved * 100).ToString("0") + "%";
                                popupform.elements[16].text[0] = (popupform.elements[30].reserved * 100).ToString("0") + "%";
                                popupform.elements[17].text[0] = (popupform.elements[31].reserved * 100).ToString("0") + "%";
                                popupform.elements[18].text[0] = (popupform.elements[32].reserved * 100).ToString("0") + "%";

                                float totalconsume = 0;
                                for (int i = 0; i < Map.maxbuildingtype; i++)
                                    totalconsume += map.buildingtypeinfo[i].consumepower;

                                popupform.elements[19].text[0] = (map.buildingtypeinfo[0].consumepower * 100 / totalconsume).ToString("0") + "%";
                                popupform.elements[20].text[0] = (map.buildingtypeinfo[1].consumepower * 100 / totalconsume).ToString("0") + "%";
                                popupform.elements[21].text[0] = (map.buildingtypeinfo[2].consumepower * 100 / totalconsume).ToString("0") + "%";
                                popupform.elements[22].text[0] = (map.buildingtypeinfo[3].consumepower * 100 / totalconsume).ToString("0") + "%";
                                popupform.elements[23].text[0] = (map.buildingtypeinfo[4].consumepower * 100 / totalconsume).ToString("0") + "%";
                                popupform.elements[24].text[0] = (map.buildingtypeinfo[5].consumepower * 100 / totalconsume).ToString("0") + "%";
                                popupform.elements[25].text[0] = (map.buildingtypeinfo[6].consumepower * 100 / totalconsume).ToString("0") + "%";

                                popupform.elements[35].text[0] = totalconsume.ToString("0");
                                popupform.elements[36].text[0] = map.energyproduction.ToString("0");
                            }
                            //energy inventory
                            if (gui.popupformid == 3)
                            {
                                //5 - index of first name
                                //14 - index of first rectangle's icon
                                //25 - index of slider
                                //26 - index of first updown button
                                //35 - index of first exchange button
                                for (int i = 0; i < 9; i++)
                                {
                                    popupform.elements[5 + i].text[0] = Language.GetShortNameOfResource(startexchangeitem + i);
                                    int count = (int)map.inventory[startexchangeitem + i].count;
                                    string countstring = count >= 1000000 ? ((count / 1000).ToString() + "K") : count.ToString();
                                    spriteBatch.DrawString(font, "x " + countstring, new Vector2(popupform.elements[5 + i].rect.X + popupform.elements[5 + i].rect.Width + 6, popupform.elements[5 + i].rect.Y + 5), Color.White);
                                    Rectangle rect = popupform.elements[14 + i].rect;
                                    spriteBatch.Draw(resourceset, new Rectangle(rect.X + (rect.Width - 16) / 2, rect.Y + (rect.Height - 16) / 2, 16, 16), new Rectangle((startexchangeitem + i) % 8 * 16, (startexchangeitem + i) / 8 * 16, 16, 16), Color.White);

                                    if (map.inventory[startexchangeitem + i].exchangetype == 0)
                                    {
                                        popupform.elements[35 + i].text[0] = LanguageHelper.Popup_Storage;
                                        popupform.elements[26 + i].enable = false;
                                    }
                                    if (map.inventory[startexchangeitem + i].exchangetype == 1)
                                    {
                                        popupform.elements[35 + i].text[0] = LanguageHelper.Popup_Import;
                                        popupform.elements[26 + i].enable = true;

                                        count = (int)map.inventory[startexchangeitem + i].exchangecount;
                                        countstring = count >= 1000000 ? ((count / 1000).ToString() + "K") : count.ToString();
                                        spriteBatch.DrawString(font, "x " + countstring, new Vector2(popupform.elements[35 + i].rect.X + popupform.elements[35 + i].rect.Width + 20, popupform.elements[35 + i].rect.Y + 5), Color.White);
                                    }
                                    if (map.inventory[startexchangeitem + i].exchangetype == 2)
                                    {
                                        popupform.elements[35 + i].text[0] = LanguageHelper.Popup_Export;
                                        popupform.elements[26 + i].enable = true;

                                        count = (int)map.inventory[startexchangeitem + i].exchangecount;
                                        countstring = count >= 1000000 ? ((count / 1000).ToString() + "K") : count.ToString();
                                        spriteBatch.DrawString(font, "x " + countstring, new Vector2(popupform.elements[35 + i].rect.X + popupform.elements[35 + i].rect.Width + 20, popupform.elements[35 + i].rect.Y + 5), Color.White);
                                    }
                                }

                                Rectangle sliderrect = popupform.elements[25].rect;
                                int sliderh = sliderrect.Height / Map.maxresources * 10;
                                int sliderpos = sliderrect.Height / Map.maxresources * startexchangeitem;
                                GuiObject.DrawPanel(spriteBatch, guiset, new Rectangle(sliderrect.X, sliderrect.Y + sliderpos, sliderrect.Width, sliderh));

                                spriteBatch.Draw(guiset, new Rectangle(270, popupform.elements[14].rect.Y - 1, 1, 280), new Rectangle(98, 38, 1, 1), Color.White);
                                spriteBatch.Draw(guiset, new Rectangle(popupform.elements[4].rect.X, popupform.elements[14].rect.Y - 1, 1, 280), new Rectangle(98, 38, 1, 1), Color.White);
                            }
                            //science
                            if (gui.popupformid == 4)
                            {
                                //11 - index of first panel
                                //10 - index of info panel
                                for (int i = 0; i < 9; i++)
                                {
                                    if (i == map.science[map.currentresearchmode].items.Length) break;

                                    Rectangle rect = popupform.elements[11 + i].rect;
                                    spriteBatch.DrawString(font, map.science[map.currentresearchmode].items[startresearchitem + i].name, new Vector2(rect.X + 5, rect.Y + 5), Color.White);
                                }

                                Rectangle sliderrect = popupform.elements[7].rect;

                                if (map.science[map.currentresearchmode].items.Length > 9)
                                {
                                    int sliderh = sliderrect.Height / map.science[map.currentresearchmode].items.Length * 10;
                                    int sliderpos = sliderrect.Height / map.science[map.currentresearchmode].items.Length * startresearchitem;
                                    GuiObject.DrawPanel(spriteBatch, guiset, new Rectangle(sliderrect.X, sliderrect.Y + sliderpos, sliderrect.Width, sliderh));
                                }
                                else GuiObject.DrawPanel(spriteBatch, guiset, sliderrect);

                                spriteBatch.Draw(guiset, new Rectangle(popupform.elements[4].rect.X, popupform.elements[4].rect.Y + 37 + 23, 1, 280), new Rectangle(98, 38, 1, 1), Color.White);
                                spriteBatch.Draw(guiset, new Rectangle(panelstartx + 24 + 40, popupform.elements[4].rect.Y + 37 + 23, 1, 280), new Rectangle(98, 38, 1, 1), Color.White);

                                Rectangle inforect = popupform.elements[10].rect;
                                string[] name = map.science[map.currentresearchmode].items[map.selectedresearch[map.currentresearchmode]].name.Split(' ');

                                int h = 15;
                                for (int i = 0; i < name.Length; i++)
                                {
                                    Vector2 namesize = font.MeasureString(name[i]);
                                    spriteBatch.DrawString(font, name[i], new Vector2(inforect.X + (inforect.Width - (int)namesize.X) / 2, inforect.Y + h), Color.White);
                                    h += 20;
                                }
                                h += 10;

                                string[] info = { LanguageHelper.Popup_Left, (map.science[map.currentresearchmode].items[map.selectedresearch[map.currentresearchmode]].time + 1).ToString("0") + " " + LanguageHelper.Popup_sciencepoints };

                                for (int i = 0; i < info.Length; i++)
                                {
                                    Vector2 infosize = font.MeasureString(info[i]);
                                    spriteBatch.DrawString(font, info[i], new Vector2(inforect.X + (inforect.Width - (int)infosize.X) / 2, inforect.Y + h), Color.White);
                                    h += 20;
                                }
                                h += 10;

                                string[] overview = map.science[map.currentresearchmode].items[map.selectedresearch[map.currentresearchmode]].overview.Split(' ');
                                for (int i = 0; i < overview.Length; i++)
                                {
                                    Vector2 overviewsize = font.MeasureString(overview[i]);
                                    spriteBatch.DrawString(font, overview[i], new Vector2(inforect.X + (inforect.Width - (int)overviewsize.X) / 2, inforect.Y + h), Color.White);
                                    h += 20;
                                }
                            }
                            //info
                            if (gui.popupformid == 7 && map.buildings.Count > selectedbuildid)
                            {
                                //5 - index of place for image
                                //6 - name
                                //7 - type
                                //8 - energy consum
                                //9 - info text

                                Rectangle place = popupform.elements[5].rect;
                                Rectangle source = Building.GetSource(map.buildings[selectedbuildid].type);
                                spriteBatch.Draw(buildset, new Rectangle(place.X + (place.Width - source.Width) / 2, place.Y + (place.Height - source.Height) / 2, source.Width, source.Height), source, Color.White);
                                string[] types = new string[] { LanguageHelper.Popup_Administry, LanguageHelper.Popup_Manufacturing, LanguageHelper.Popup_Mining, LanguageHelper.Popup_Science, LanguageHelper.Popup_Storage, LanguageHelper.Popup_Links, LanguageHelper.Popup_Defence, LanguageHelper.Popup_Helping };
                                int type = Building.GetType(map.buildings[selectedbuildid].type);

                                popupform.elements[6].text[0] = LanguageHelper.Popup_Name2 + ": " + Building.GetName(map.buildings[selectedbuildid].type) + "[" + map.buildings[selectedbuildid].lvl + "]";
                                popupform.elements[7].text[0] = LanguageHelper.Popup_Type + ": " + types[type];
                                popupform.elements[8].text[0] = LanguageHelper.Popup_EnergyConsum + ": " + (map.buildings[selectedbuildid].height + map.buildings[selectedbuildid].power * map.buildingtypeinfo[type].maxpower * Building.GetEnergy(map.buildings[selectedbuildid].type)).ToString("0.0");

                                int resource_id = map.buildings[selectedbuildid].recipte + 2;

                                switch (map.buildings[selectedbuildid].type)
                                {
                                    case Building.Farm:
                                    case Building.ClosedFarm:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        place.Width = 16; place.Height = 16;
                                        source = new Rectangle((resource_id % 8) * 16, (resource_id / 8) * 16, 16, 16);
                                        spriteBatch.Draw(resourceset, place, source, Color.White);
                                        spriteBatch.DrawString(font, " x " + (map.buildings[selectedbuildid].power * map.buildingtypeinfo[Building.GetType(map.buildings[selectedbuildid].type)].maxpower).ToString("0.00"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                        break;
                                    case Building.Generator:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        place.Width = 16; place.Height = 16;
                                        source = new Rectangle(((int)Resources.energy % 8) * 16, ((int)Resources.energy / 8) * 16, 16, 16);
                                        spriteBatch.Draw(resourceset, place, source, Color.White);
                                        float energyproduce = map.buildings[selectedbuildid].power * Constants.Map_generatorproduce * map.perk.optimizeerergyproduce;
                                        spriteBatch.DrawString(font, " x " + (energyproduce).ToString("0.00"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                        break;
                                    case Building.Warehouse:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.storageinfo[Constants.Map_rocks].currentcapability.ToString("0") + " \\ " + map.storageinfo[Constants.Map_rocks].maxcapability.ToString("0"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.House:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.storageinfo[Constants.Map_humans].currentcapability.ToString("0") + " \\ " + map.storageinfo[Constants.Map_humans].maxcapability.ToString("0"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        place.X = popupform.elements[9].rect.X;
                                        if (map.inventory[(int)Resources.water].count <= 0)
                                            spriteBatch.DrawString(font, LanguageHelper.Gui_HumansNeedWater, new Vector2(place.X + 25 + 5, place.Y + 40), Color.White);
                                        if (map.inventory[(int)Resources.vegetables].count +
                                            map.inventory[(int)Resources.meat].count +
                                            map.inventory[(int)Resources.fish].count +
                                            map.inventory[(int)Resources.fruits].count <= 0)
                                            spriteBatch.DrawString(font, LanguageHelper.Gui_HumansNeedFood, new Vector2(place.X + 25 + 5, place.Y + 60), Color.White);
                                        break;
                                    case Building.EnergyBank:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.storageinfo[Constants.Map_science].currentcapability.ToString("0") + " \\ " + map.storageinfo[Constants.Map_science].maxcapability.ToString("0"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.InfoStorage:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.storageinfo[Constants.Map_energy].currentcapability.ToString("0") + " \\ " + map.storageinfo[Constants.Map_energy].maxcapability.ToString("0"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.LuquidStorage:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.storageinfo[Constants.Map_luquids].currentcapability.ToString("0") + " \\ " + map.storageinfo[Constants.Map_luquids].maxcapability.ToString("0"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.DroidFactory:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;

                                        spriteBatch.DrawString(font, " " + map.buildings[selectedbuildid].workcount.ToString("0"), new Vector2(place.X + 5, place.Y), Color.White);

                                        place = popupform.elements[11].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;

                                        spriteBatch.DrawString(font, " " + map.buildings[selectedbuildid].worktime.ToString("0.00"), new Vector2(place.X + 5, place.Y), Color.White);

                                        place = popupform.elements[12].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        place.Width = 16; place.Height = 16;
                                        source = new Rectangle(((int)Resources.metal % 8) * 16, ((int)Resources.metal / 8) * 16, 16, 16);
                                        spriteBatch.Draw(resourceset, place, source, Color.White);
                                        spriteBatch.DrawString(font, " x " + (Constants.Unit_dronemetalprice).ToString("0"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                        place.Y += 20;
                                        source = new Rectangle(((int)Resources.electronics % 8) * 16, ((int)Resources.electronics / 8) * 16, 16, 16);
                                        spriteBatch.Draw(resourceset, place, source, Color.White);
                                        spriteBatch.DrawString(font, " x " + (Constants.Unit_droneelectronicprice).ToString("0"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                        break;
                                    case Building.ProcessingFactory:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        place.Width = 16; place.Height = 16;
                                        source = new Rectangle((MapHelper_Engine.resiptes[map.buildings[selectedbuildid].recipte].outresource % 8) * 16, (MapHelper_Engine.resiptes[map.buildings[selectedbuildid].recipte].outresource / 8) * 16, 16, 16);
                                        spriteBatch.Draw(resourceset, place, source, Color.White);
                                        float thisbuildingpow = map.buildings[selectedbuildid].power * map.buildingtypeinfo[Building.GetType(map.buildings[selectedbuildid].type)].maxpower;
                                        spriteBatch.DrawString(font, " x " + (MapHelper_Engine.resiptes[map.buildings[selectedbuildid].recipte].outcount * thisbuildingpow *
                                            ((MapHelper_Engine.resiptes[map.buildings[selectedbuildid].recipte].outresource == (int)Resources.metal) ? map.perk.optimizeore : 1)).ToString("0.00"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);

                                        place.Y += popupform.elements[11].rect.Y - popupform.elements[9].rect.Y;
                                        for (int k = 0; k < MapHelper_Engine.resiptes[map.buildings[selectedbuildid].recipte].inresourses.Length; k++)
                                        {
                                            source = new Rectangle((MapHelper_Engine.resiptes[map.buildings[selectedbuildid].recipte].inresourses[k] % 8) * 16, (MapHelper_Engine.resiptes[map.buildings[selectedbuildid].recipte].inresourses[k] / 8) * 16, 16, 16);
                                            spriteBatch.Draw(resourceset, place, source, Color.White);
                                            spriteBatch.DrawString(font, " x " + (MapHelper_Engine.resiptes[map.buildings[selectedbuildid].recipte].incount[k] * thisbuildingpow).ToString("0.00"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                            place.Y += 20;
                                        }
                                        break;
                                    case Building.Mine:
                                        float[] inv = new float[Map.maxresources];
                                        Point p = Building.GetSize(map.buildings[selectedbuildid].type);
                                        int x = (int)map.buildings[selectedbuildid].pos.X;
                                        int y = (int)map.buildings[selectedbuildid].pos.Y;
                                        thisbuildingpow = map.buildings[selectedbuildid].power * map.buildingtypeinfo[Building.GetType(map.buildings[selectedbuildid].type)].maxpower;

                                        for (int k = 0; k < p.X; k++)
                                            for (int j = 0; j < p.Y; j++)
                                            {
                                                if (map.data[y - k, x - j].mineresource_id > 0)
                                                    inv[map.data[y - k, x - j].mineresource_id] += thisbuildingpow;
                                                else inv[(int)Resources.rock] += thisbuildingpow / 3;
                                            }

                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        place.Width = 16; place.Height = 16;

                                        for (int i = 0; i < Map.maxresources; i++)
                                        {
                                            if (inv[i] > 0)
                                            {
                                                source = new Rectangle((i % 8) * 16, (i / 8) * 16, 16, 16);
                                                spriteBatch.Draw(resourceset, place, source, Color.White);
                                                spriteBatch.DrawString(font, " x " + (inv[i]).ToString("0.00"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                                place.Y += 20;
                                            }
                                        }

                                        break;
                                    case Building.Dirrick:
                                        inv = new float[Map.maxresources];
                                        p = Building.GetSize(map.buildings[selectedbuildid].type);
                                        x = (int)map.buildings[selectedbuildid].pos.X;
                                        y = (int)map.buildings[selectedbuildid].pos.Y;
                                        thisbuildingpow = map.buildings[selectedbuildid].power * map.buildingtypeinfo[Building.GetType(map.buildings[selectedbuildid].type)].maxpower;

                                        for (int k = 0; k < p.X; k++)
                                            for (int j = 0; j < p.Y; j++)
                                            {
                                                if (map.data[y - k, x - j].dirrickresource_id > 0)
                                                    inv[map.data[y - k, x - j].dirrickresource_id] += thisbuildingpow;
                                            }

                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        place.Width = 16; place.Height = 16;

                                        for (int i = 0; i < Map.maxresources; i++)
                                        {
                                            if (inv[i] > 0)
                                            {
                                                source = new Rectangle((i % 8) * 16, (i / 8) * 16, 16, 16);
                                                spriteBatch.Draw(resourceset, place, source, Color.White);
                                                spriteBatch.DrawString(font, " x " + (inv[i]).ToString("0.00"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                                place.Y += 20;
                                            }
                                        }
                                        break;
                                    case Building.Collector:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        place.Width = 16; place.Height = 16;
                                        thisbuildingpow = map.buildings[selectedbuildid].power * map.buildingtypeinfo[Building.GetType(map.buildings[selectedbuildid].type)].maxpower;

                                        source = new Rectangle(((map.buildings[selectedbuildid].recipte + (int)Resources.animals) % 8) * 16, ((map.buildings[selectedbuildid].recipte + (int)Resources.animals) / 8) * 16, 16, 16);
                                        spriteBatch.Draw(resourceset, place, source, Color.White);
                                        spriteBatch.DrawString(font, " x " + (thisbuildingpow * 4).ToString("0.00"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                        break;
                                    case Building.BuildCenter:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.perk.buildbonus.ToString("0%"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.LinksCenter:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.perk.movementbonus.ToString("0%"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.StorageCenter:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.perk.storagebonus.ToString("0%"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.ScienceCenter:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.perk.sciencebonus.ToString("0%"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.CommandCenter:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        spriteBatch.DrawString(font, map.perk.buildbonus.ToString("0%") + '\n' + map.perk.movementbonus.ToString("0%") + '\n' +
                                                                     map.perk.storagebonus.ToString("0%") + '\n' + map.perk.sciencebonus.ToString("0%"), new Vector2(place.X + 25 + 5, place.Y), Color.White);
                                        break;
                                    case Building.AttackFactory:
                                        place = popupform.elements[9].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;

                                        spriteBatch.DrawString(font, " " + map.buildings[selectedbuildid].workcount.ToString("0"), new Vector2(place.X + 5, place.Y), Color.White);

                                        place = popupform.elements[11].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;

                                        spriteBatch.DrawString(font, " " + map.buildings[selectedbuildid].worktime.ToString("0.00"), new Vector2(place.X + 5, place.Y), Color.White);

                                        place = popupform.elements[12].rect;
                                        place.X += place.Width;
                                        place.Y = place.Y + (place.Height - 16) / 2;
                                        place.Width = 16; place.Height = 16;
                                        source = new Rectangle(((int)Resources.metal % 8) * 16, ((int)Resources.metal / 8) * 16, 16, 16);
                                        spriteBatch.Draw(resourceset, place, source, Color.White);
                                        spriteBatch.DrawString(font, " x " + (Constants.Unit_attackdronemetalprice).ToString("0"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                        place.Y += 20;
                                        source = new Rectangle(((int)Resources.electronics % 8) * 16, ((int)Resources.electronics / 8) * 16, 16, 16);
                                        spriteBatch.Draw(resourceset, place, source, Color.White);
                                        spriteBatch.DrawString(font, " x " + (Constants.Unit_attackdroneelectronicprice).ToString("0"), new Vector2(place.X + source.Width + 5, place.Y), Color.White);
                                        break;
                                    case Building.RocketParking:
                                        place = popupform.elements[10].rect;
                                        Vector2 f = font.MeasureString(popupform.elements[10].text[0]);
                                        int sx = place.X + (place.Width - (int)f.X) / 2 + (int)f.X;
                                        int sy = place.Y;
                                        spriteBatch.DrawString(font, map.buildings[selectedbuildid].wait.ToString("0.00"), new Vector2(sx, sy + 5), Color.White);

                                        text = LanguageHelper.Popup_CurrentRocketNone;
                                        if (map.buildings[selectedbuildid].recipte == 1) text = LanguageHelper.Popup_CurrentRocketAtom;
                                        if (map.buildings[selectedbuildid].recipte == 2) text = LanguageHelper.Popup_CurrentRocketNeitron;
                                        if (map.buildings[selectedbuildid].recipte == 3) text = LanguageHelper.Popup_CurrentRocketTwined;

                                        place = popupform.elements[9].rect;
                                        f = font.MeasureString(popupform.elements[9].text[0]);
                                        sx = place.X + (place.Width - (int)f.X) / 2 + (int)f.X;
                                        sy = place.Y;
                                        spriteBatch.DrawString(font, text, new Vector2(sx, sy + 5), Color.White);
                                        break;
                                }
                            }
                            //launch rocket
                            if (gui.popupformid == 24)
                            {
                                //9 - index of first panel
                                //8 - index of minimap
                                for (int i = 0; i < 9; i++)
                                {
                                    if (i == planet.maps.Count) break;

                                    Rectangle rect = popupform.elements[9 + i].rect;
                                    spriteBatch.DrawString(font, planet.maps[startlaunchrocketitem + i].name, new Vector2(rect.X + 5, rect.Y + 5), (startlaunchrocketitem + i) == selectedbase ? Color.Gray : (startlaunchrocketitem + i) == selectrocketbase ? Color.Red : Color.White);
                                }

                                spriteBatch.Draw(guiset, new Rectangle(popupform.elements[8].rect.X - 11, popupform.elements[4].rect.Y + 37 + 23, 1, 280), new Rectangle(98, 38, 1, 1), Color.White);
                                spriteBatch.Draw(guiset, new Rectangle(popupform.elements[8].rect.X + popupform.elements[8].rect.Width + 11, popupform.elements[4].rect.Y + 37 + 23, 1, 280), new Rectangle(98, 38, 1, 1), Color.White);

                                if (minimaprocket != null)
                                    spriteBatch.Draw(minimaprocket, popupform.elements[8].rect, Color.White);

                                int lx = popupform.elements[8].rect.X + launchrocketx * popupform.elements[8].rect.Width / 64;
                                int ly = popupform.elements[8].rect.Y + launchrockety * popupform.elements[8].rect.Height / 64;

                                spriteBatch.End();
                                VertexBufferBinding[] vb = GraphicsDevice.GetVertexBuffers();
                                IndexBuffer ib = GraphicsDevice.Indices;

                                DrawLine(popupform.elements[8].rect.X, ly, popupform.elements[8].rect.X + popupform.elements[8].rect.Width, ly, Color.Green, Color.Green);
                                DrawLine(lx, popupform.elements[8].rect.Y, lx, popupform.elements[8].rect.Y + popupform.elements[8].rect.Height, Color.Green, Color.Green);

                                GraphicsDevice.SetVertexBuffers(vb);
                                GraphicsDevice.Indices = ib;
                                spriteBatch.Begin();
                            }
                        }
                        #endregion

                        if (gui.helpstring != null)
                        {
                            Vector2 helpstringsize = font.MeasureString(gui.helpstring);
                            Rectangle rect = new Rectangle((int)(width - 200 - helpstringsize.X - 4 - 5), (int)(height - 30 - helpstringsize.Y - 4 - 4), (int)(helpstringsize.X + 8), (int)(helpstringsize.Y + 8));
                            spriteBatch.Draw(guiset, rect, new Rectangle(80, 0, 16, 16), new Color(0, 0, 0, 0.5f));
                            spriteBatch.DrawString(font, gui.helpstring, new Vector2(rect.X + 4, rect.Y + 4), Color.White);
                        }

                        spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, 16 * localmapmode, 12, 16), Color.White);
                        //spriteBatch.DrawString(font, camera.onx + " " + camera.ony + " " + map.data[(camera.ony + map.height) % map.height, (camera.onx + map.width) % map.width].build_draw, camera.mouse + Vector2.UnitY * 30, Color.White);
                        spriteBatch.End();
                    }
                }
                #endregion

                #region Help
                if (state == HelpMenuMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);
                    int startx = (width - 800) / 2;
                    int starty = (height - 600) / 2;
                    int h = starty + 15;
                    for (int i = starthelpstring; i < Helper.help.Length; i++)
                    {
                        spriteBatch.DrawString(font, Helper.help[i], new Vector2(startx + 15, h), Color.White);
                        h += (int)font.MeasureString(Helper.help[i]).Y + 4;
                        if (i < Helper.help.Length - 1 && h - starty + (int)font.MeasureString(Helper.help[i + 1]).Y + 4 > 530) break;
                    }
                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion
                #region About
                if (state == AboutMenuMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);
                    int startx = (width - 690) / 2;
                    int starty = (height - 200) / 2;
                    int h = starty + 15;
                    for (int i = 0; i < Helper.about.Length; i++)
                    {
                        spriteBatch.DrawString(font, Helper.about[i], new Vector2(startx + 15, h), Color.White);
                        h += (int)font.MeasureString(Helper.about[i]).Y + 4;
                    }
                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion
                #region Crono
                if (state == ChronoMenuMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);
                    int startx = (width - 630) / 2;
                    int starty = (height - 350) / 2;
                    int h = starty + 15;
                    for (int i = 0; i < Helper.chrono.Length; i++)
                    {
                        spriteBatch.DrawString(font, Helper.chrono[i], new Vector2(startx + 15, h), Color.White);
                        h += (int)font.MeasureString(Helper.chrono[i]).Y + 4;
                    }
                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion
                #region Promo
                if (state == PromoMenuMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);
                    int startx = (width - 800) / 2;
                    int starty = (height - 600) / 2;
                    int h = starty + 15;
                    for (int i = startpromostring; i < Helper.promo.Length; i++)
                    {
                        spriteBatch.DrawString(font, Helper.promo[i], new Vector2(startx + 15, h), Color.White);
                        h += (int)font.MeasureString(Helper.promo[i]).Y + 4;
                        if (i < Helper.promo.Length - 1 && h - starty + (int)font.MeasureString(Helper.promo[i + 1]).Y + 4 > 530) break;
                    }
                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion

                #region Load
                if (state == LoadMode)
                {
                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    int size = 600;
                    int startx = (width - size - 200) / 2;
                    int starty = (height - size) / 2;

                    gui.Draw(state, spriteBatch, guiset, font);

                    string text;
                    Vector2 v;
                    int offcety = 0;

                    if (filenames != null && filenames.Length > 0)
                        for (int i = 0; i < filenames.Length; i++)
                        {
                            text = filenames[i] + "\t[" + fileinfos[i] + "]";
                            v = font.MeasureString(text);
                            if (i == selectedloadid)
                                GuiObject.DrawPanel(spriteBatch, guiset, new Rectangle(startx + 10, starty + 10 + offcety, size - 20, (int)v.Y));
                            spriteBatch.DrawString(font, text, new Vector2(startx + 13, starty + 10 + offcety), Color.White);
                            offcety += (int)v.Y + 8;
                            if (offcety > size - 20) break;
                        }

                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);

                    spriteBatch.End();
                }
                #endregion

                #region Catalog
                if (state == CatalogMode)
                {
                    int c_size = 400;
                    int c_startx = (width - c_size) / 2;
                    int c_starty = (height - c_size) / 2 + 40;

                    GraphicsDevice.Clear(Color.Black);
                    DrawTunel();

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    gui.Draw(state, spriteBatch, guiset, font);

                    string text = LanguageHelper.Popup_StarLighting + ": " + Math.Abs(Math.Pow(10, 4 * ((catalogstarlighting * 2) / 10))).ToString("0.######");
                    Vector2 v = font.MeasureString(text);
                    spriteBatch.DrawString(font, text, new Vector2((width - (int)v.X) / 2, c_starty - 30), Color.White);

                    float tx = catalogstartemperature * 7;
                    float t = (float)(26.389 * Math.Pow(tx, 6) -
                                 550 * Math.Pow(tx, 5) +
                                 4555.6 * Math.Pow(tx, 4) -
                                 19042 * Math.Pow(tx, 3) +
                                 41918 * Math.Pow(tx, 2) -
                                 44408 * tx + 19500);
                    text = LanguageHelper.Popup_StarTemperature + ": " + t.ToString("0") + " " + LanguageHelper.DrawText_kelvin;
                    v = font.MeasureString(text);
                    spriteBatch.DrawString(font, text, new Vector2((width - (int)v.X) / 2, c_starty - 30 + 35 * 2), Color.White);

                    text = LanguageHelper.Popup_PlanerRadius + ": " + ((catalogplanetradius * 2 - 1) / 1.5f * 11500 + 13500).ToString("0") + " " + LanguageHelper.DrawText_km;
                    v = font.MeasureString(text);
                    spriteBatch.DrawString(font, text, new Vector2((width - (int)v.X) / 2, c_starty - 30 + 35 * 4), Color.White);

                    text = LanguageHelper.Popup_PlanetLength + ": " + (catalogplanetsemiaxis * 4.5 + 0.25).ToString("0.##") + " " + LanguageHelper.DrawText_ao;
                    v = font.MeasureString(text);
                    spriteBatch.DrawString(font, text, new Vector2((width - (int)v.X) / 2, c_starty - 30 + 35 * 6), Color.White);

                    spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
                    spriteBatch.End();
                }
                #endregion

                #region UserMode
                if (state == UserMode)
                {
                    if (scriptes!=null)foreach (Script s in scriptes)
                    {
                        s.Draw();
                    }
                }
                #endregion

                drawgui = true;
            }
            base.Draw(gameTime);
        }

        #region Draw functions
        void DrawLoadScreen()
        {
            loading = true;
            this.Tick();
            loading = false;
        }
        void DrawTunel()
        {
            VertexPositionColorTexture[] vertixes = new VertexPositionColorTexture[4];
            short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };

            vertixes[0] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            vertixes[1] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));
            vertixes[2] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            vertixes[3] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));

            tunelEffect.CurrentTechnique = tunelEffect.Techniques["Technique1"];
            tunelEffect.Parameters["offcetx"].SetValue((float)currentGameTime.TotalGameTime.TotalSeconds*3);
            tunelEffect.Parameters["offcety"].SetValue((float)currentGameTime.TotalGameTime.TotalSeconds/40);
            foreach (EffectPass pass in tunelEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
            }
        }
        void DrawLoadingTunel()
        {
            VertexPositionColorTexture[] vertixes = new VertexPositionColorTexture[4];
            short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };

            Color color = Color.White;
            vertixes[0] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), color, new Vector2(0, 1));
            vertixes[1] = new VertexPositionColorTexture(new Vector3(1, -1, 0), color, new Vector2(1, 1));
            vertixes[2] = new VertexPositionColorTexture(new Vector3(1, 1, 0), color, new Vector2(1, 0));
            vertixes[3] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), color, new Vector2(0, 0));

            tunelEffect.CurrentTechnique = tunelEffect.Techniques["Technique1"];
            tunelEffect.Parameters["offcetx"].SetValue((float)currentGameTime.TotalGameTime.TotalSeconds * 3);
            tunelEffect.Parameters["offcety"].SetValue((float)currentGameTime.TotalGameTime.TotalSeconds / 40);
            foreach (EffectPass pass in tunelEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
            }
        }
        void DrawStar(Rectangle pos, Color color)
        {
            VertexPositionColorTexture[] vertixes = new VertexPositionColorTexture[4];
            short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };

            vertixes[0] = new VertexPositionColorTexture(new Vector3(pos.X, pos.Y, 0), color, new Vector2(0, 1));
            vertixes[1] = new VertexPositionColorTexture(new Vector3(pos.X + pos.Width, pos.Y, 0), color, new Vector2(1, 1));
            vertixes[2] = new VertexPositionColorTexture(new Vector3(pos.X + pos.Width, pos.Y + pos.Height, 0), color, new Vector2(1, 0));
            vertixes[3] = new VertexPositionColorTexture(new Vector3(pos.X, pos.Y + pos.Height, 0), color, new Vector2(0, 0));

            starShader.CurrentTechnique = starShader.Techniques["Technique1"];
            foreach (EffectPass pass in starShader.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
            }
        }
        void DrawPlanet(Rectangle pos, Color color)
        {
            VertexPositionColorTexture[] vertixes = new VertexPositionColorTexture[4];
            short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };

            vertixes[0] = new VertexPositionColorTexture(new Vector3(pos.X, pos.Y, 0), color, new Vector2(0, 1));
            vertixes[1] = new VertexPositionColorTexture(new Vector3(pos.X + pos.Width, pos.Y, 0), color, new Vector2(1, 1));
            vertixes[2] = new VertexPositionColorTexture(new Vector3(pos.X + pos.Width, pos.Y + pos.Height, 0), color, new Vector2(1, 0));
            vertixes[3] = new VertexPositionColorTexture(new Vector3(pos.X, pos.Y + pos.Height, 0), color, new Vector2(0, 0));

            starShader.CurrentTechnique = starShader.Techniques["Technique2"];
            foreach (EffectPass pass in starShader.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
            }
        }
        void DrawPlanetMap(Color color,float offcet,Texture2D texture, Texture normal, Rectangle pos, Effect effect)
        {
            effect.Parameters["AmbientColor"].SetValue(color.ToVector4());
            effect.Parameters["Offcet"].SetValue(offcet);
            effect.Parameters["Texture"].SetValue(texture);
            effect.Parameters["Normal"].SetValue(normal);
            VertexPositionTexture[] vertixes = new VertexPositionTexture[4];
            short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };

            float x1 = (float)(pos.X) / width * 2 - 1;
            float y1 = (float)(pos.Y) / height * 2 - 1;
            float x2 = (float)((pos.X + pos.Width)) / width * 2 - 1;
            float y2 = (float)((pos.Y + pos.Height)) / height * 2 - 1;

            vertixes[0] = new VertexPositionTexture(new Vector3(x1, y1, 0), new Vector2(0, 1));
            vertixes[1] = new VertexPositionTexture(new Vector3(x2, y1, 0), new Vector2(1, 1));
            vertixes[2] = new VertexPositionTexture(new Vector3(x2, y2, 0), new Vector2(1, 0));
            vertixes[3] = new VertexPositionTexture(new Vector3(x1, y2, 0), new Vector2(0, 0));

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
            }
            GraphicsDevice.Textures[0] = null;
            GraphicsDevice.Textures[1] = null;
        }
        void DrawTile(Texture2D texture,int i, int j, int id,Color sunlight)
        {
            VertexPositionColorTexture[] vertixes = new VertexPositionColorTexture[4];
            short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };

            float c1 = mapHelper.data[i, j].lighting;
            float c2 = j >= map.width - 1 ? c1 : mapHelper.data[i, j + 1].lighting;
            float c3 = j >= map.width - 1 || i >= map.height - 1 ? c1 : mapHelper.data[i + 1, j + 1].lighting;
            float c4 = i >= map.height - 1 ? c1 : mapHelper.data[i + 1, j].lighting;

            float stx = 32.0f / texture.Width;
            float sty = 32.0f / texture.Height;
            float tx = id % 16 * stx;
            float ty = id / 16 * sty;

            vertixes[0] = new VertexPositionColorTexture(new Vector3(j * 32, i * 32, 0), Color.Lerp(sunlight, Color.White, c1 / 3), new Vector2(tx, ty));
            vertixes[1] = new VertexPositionColorTexture(new Vector3(j * 32 + 32, i * 32, 0), Color.Lerp(sunlight, Color.White, c2 / 3), new Vector2(tx + stx, ty));
            vertixes[2] = new VertexPositionColorTexture(new Vector3(j * 32 + 32, i * 32 + 32, 0), Color.Lerp(sunlight, Color.White, c3 / 3), new Vector2(tx + stx, ty + sty));
            vertixes[3] = new VertexPositionColorTexture(new Vector3(j * 32, i * 32 + 32, 0), Color.Lerp(sunlight, Color.White, c4 / 3), new Vector2(tx, ty + sty));

            foreach (EffectPass pass in tileEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
            }
        }
        void DrawSprite(Texture2D texture, Rectangle pos, Rectangle source, Color light,float z)
        {
            tileEffect.Parameters["Texture"].SetValue(texture);

            VertexPositionColorTexture[] vertixes = new VertexPositionColorTexture[4];
            short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };

            float stx = (float)source.Width / texture.Width;
            float sty = (float)source.Height / texture.Height;
            float tx = (float)source.X / texture.Width;
            float ty = (float)source.Y / texture.Height;

            vertixes[0] = new VertexPositionColorTexture(new Vector3(pos.X, pos.Y, z), light, new Vector2(tx, ty));
            vertixes[1] = new VertexPositionColorTexture(new Vector3(pos.X + pos.Width, pos.Y, z), light, new Vector2(tx + stx, ty));
            vertixes[2] = new VertexPositionColorTexture(new Vector3(pos.X + pos.Width, pos.Y + pos.Height, z), light, new Vector2(tx + stx, ty + sty));
            vertixes[3] = new VertexPositionColorTexture(new Vector3(pos.X, pos.Y + pos.Height, z), light, new Vector2(tx, ty + sty));

            foreach (EffectPass pass in tileEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
            }
        }
        void DrawSprite(Texture2D texture, Vector2 pos, Rectangle source, Color light,float z)
        {
            DrawSprite(texture, new Rectangle((int)pos.X, (int)pos.Y, source.Width, source.Height), source, light,z);
        }
        void DrawFractionSprite(Texture2D texture, Rectangle pos, Rectangle source, Color light,float z,int fraction)
        {
            fractiontileEffect.Parameters["Texture"].SetValue(texture);
            fractiontileEffect.Parameters["Fraction"].SetValue((fraction + 0.5f) / 32);

            VertexPositionColorTexture[] vertixes = new VertexPositionColorTexture[4];
            //short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };
            short[] indexes = new short[6] { 0, 1, 2, 0, 2, 3 };

            float stx = (float)source.Width / texture.Width;
            float sty = (float)source.Height / texture.Height;
            float tx = (float)source.X / texture.Width;
            float ty = (float)source.Y / texture.Height;

            vertixes[0] = new VertexPositionColorTexture(new Vector3(pos.X, pos.Y, z), light, new Vector2(tx, ty));
            vertixes[1] = new VertexPositionColorTexture(new Vector3(pos.X + pos.Width, pos.Y, z), light, new Vector2(tx + stx, ty));
            vertixes[2] = new VertexPositionColorTexture(new Vector3(pos.X + pos.Width, pos.Y + pos.Height - 1, z), light, new Vector2(tx + stx, ty + sty));
            vertixes[3] = new VertexPositionColorTexture(new Vector3(pos.X, pos.Y + pos.Height - 1, z), light, new Vector2(tx, ty + sty));

            foreach (EffectPass pass in fractiontileEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
            }
        }
        void DrawFractionSprite(Texture2D texture, Vector2 pos, Rectangle source, Color light, float z, int fraction)
        {
            DrawFractionSprite(texture, new Rectangle((int)pos.X, (int)pos.Y, source.Width, source.Height), source, light, z, fraction);
        }

        void DrawLine(Vector2 s, Vector2 e, Color cs, Color ce)
        {
            VertexPositionColor[] vertexes = new VertexPositionColor[2]{new VertexPositionColor(new Vector3(s.X,s.Y,0),cs),
                                                                        new VertexPositionColor(new Vector3(e.X,e.Y,0),ce)};
            foreach (EffectPass pass in baselineShader.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertexes, 0, 1);
            }
        }
        void DrawLine(int x1,int y1,int x2,int y2, Color cs, Color ce)
        {
            VertexPositionColor[] vertexes = new VertexPositionColor[2]{new VertexPositionColor(new Vector3(x1,y1,0),cs),
                                                                        new VertexPositionColor(new Vector3(x2,y2,0),ce)};

            foreach (EffectPass pass in baselineShader.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertexes, 0, 1);
            }
        }
        void DrawRectangle(Rectangle r, Color c)
        {
            VertexPositionColor[] vertexes = new VertexPositionColor[5]{new VertexPositionColor(new Vector3(r.X,        r.Y,0),c),
                                                                        new VertexPositionColor(new Vector3(r.X+r.Width,r.Y,0),c),
                                                                        new VertexPositionColor(new Vector3(r.X+r.Width,r.Y+r.Height,0),c),
                                                                        new VertexPositionColor(new Vector3(r.X,        r.Y+r.Height,0),c),
                                                                        new VertexPositionColor(new Vector3(r.X,        r.Y,0),c)};
            foreach (EffectPass pass in baselineShader.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertexes, 0, 4);
            }
        }
        void DrawSolidRectangle(Rectangle r, Color c)
        {
            VertexPositionColor[] vertexes = new VertexPositionColor[4]{new VertexPositionColor(new Vector3(r.X,        r.Y,0),c),
                                                                        new VertexPositionColor(new Vector3(r.X+r.Width,r.Y,0),c),
                                                                        new VertexPositionColor(new Vector3(r.X+r.Width,r.Y+r.Height,0),c),
                                                                        new VertexPositionColor(new Vector3(r.X,        r.Y+r.Height,0),c)};
            short[] indexes = new short[6] { 0, 1, 2, 0, 2, 3 };
            foreach (EffectPass pass in baselineShader.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertexes, 0, 4, indexes, 0, 2);
            }
        }

        static Color FromAhsb(int alpha, float hue, float saturation, float brightness)
        {
            if (0 > alpha
                || 255 < alpha)
            {
                throw new ArgumentOutOfRangeException(
                    "alpha",
                    alpha,
                    "Value must be within a range of 0 - 255.");
            }

            if (0f > hue
                || 360f < hue)
            {
                throw new ArgumentOutOfRangeException(
                    "hue",
                    hue,
                    "Value must be within a range of 0 - 360.");
            }

            if (0f > saturation
                || 1f < saturation)
            {
                throw new ArgumentOutOfRangeException(
                    "saturation",
                    saturation,
                    "Value must be within a range of 0 - 1.");
            }

            if (0f > brightness
                || 1f < brightness)
            {
                throw new ArgumentOutOfRangeException(
                    "brightness",
                    brightness,
                    "Value must be within a range of 0 - 1.");
            }

            if (0 == saturation)
            {
                return new Color(Convert.ToInt32(brightness * 255),
                                    Convert.ToInt32(brightness * 255),
                                    Convert.ToInt32(brightness * 255),
                                    alpha);
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < brightness)
            {
                fMax = brightness - (brightness * saturation) + saturation;
                fMin = brightness + (brightness * saturation) - saturation;
            }
            else
            {
                fMax = brightness + (brightness * saturation);
                fMin = brightness - (brightness * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (300f <= hue)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = (hue * (fMax - fMin)) + fMin;
            }
            else
            {
                fMid = fMin - (hue * (fMax - fMin));
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return new Color(iMid, iMax, iMin, alpha);
                case 2:
                    return new Color(iMin, iMax, iMid, alpha);
                case 3:
                    return new Color(iMin, iMid, iMax, alpha);
                case 4:
                    return new Color(iMid, iMin, iMax, alpha);
                case 5:
                    return new Color(iMax, iMin, iMid, alpha);
                default:
                    return new Color(iMax, iMid, iMin, alpha);
            }
        }
        static Color AddHSB(Color c, int h, int s, int b)
        {
            System.Drawing.Color col = System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);

            float oh = col.GetHue();
            float os = col.GetSaturation();
            float ob = col.GetBrightness();

            oh += h;
            if (oh < 0) oh += 3600;
            oh = oh - ((int)oh / 360) * 360;

            if (s > 0) os = os + (1 - os) * s / 100f;
            else os = os + (0 - os) * -s / 100f;
            if (b > 0) ob = ob + (1 - ob) * b / 100f;
            else ob = ob + (0 - ob) * -b / 100f;

            if (ob < 0) ob = -ob;

            return FromAhsb(c.A, oh, os, ob);
        }
        static string IntToShortString(int i)
        {
            string s;
            int k = 0;
            while (i >= 1000){i /= 1000;k++;}
            s = i.ToString();
            for (int q = 0; q < k; q++)s += "K";
            return s;
        }

        float GetZ(float y)
        {
            return (map.height * 32 - y) / map.height / 32*3/4+0.125f;
        }
        void CreateUniverceTexture()
        {
            int un_size = universetexture.Width;
            Color[] un_colors = new Color[un_size * un_size];

            for (int i = 0; i < un_size * un_size; i++)
                un_colors[i] = Color.Black;

            for (int i = 0; i < univerce.stars.Length; i++)
            {
                Vector2 pos = univerce.positions[i];
                un_colors[(int)(pos.Y * un_size) + (int)pos.X] = starGradient.GetColor(univerce.stars[i].temperature);

                Point[] p = new Point[4];
                p[0] = new Point(-1, 0);
                p[1] = new Point(1, 0);
                p[2] = new Point(0, 1);
                p[3] = new Point(0, -1);

                for (int k = 0; k < 4; k++)
                {
                    if (pos.X + p[k].X >= 0 && pos.X + p[k].X < 592 &&
                        pos.Y + p[k].Y >= 0 && pos.Y + p[k].Y < 592)
                    {
                        Color c_old = un_colors[(int)((pos.Y + p[k].Y) * un_size) + (int)pos.X + p[k].X];
                        Color c = un_colors[(int)(pos.Y * un_size) + (int)pos.X];

                        un_colors[(int)((pos.Y + p[k].Y) * un_size) + (int)pos.X + p[k].X] = Color.Lerp(c, c_old, 0.70f);
                    }
                }
            }

            //lines
            for (int i = 1; i < 8; i++)
            {
                for (int j = 0; j < 592; j++)
                {
                    un_colors[i * 74 + j * 592] = Color.Lerp(un_colors[i * 74 + j * 592], Color.White, 0.15f);
                    un_colors[i * 74 * 592 + j] = Color.Lerp(un_colors[i * 74 * 592 + j], Color.White, 0.15f);
                }
            }

            universetexture.SetData<Color>(un_colors);

            //---------------------------------------------------------

            int tun_size = tuneltexture.Width;
            Color[] tun_colors = new Color[tuneltexture.Width * tuneltexture.Height];
            Random rand = new Random();
            for (int i = 0; i < tuneltexture.Width * tuneltexture.Height; i++)
            {
                int gray = rand.Next(256) / 50;
                tun_colors[i] = new Color(gray, gray, gray);
            }
            for (int i = 0; i < tun_size*2.4; i++)
            {
                int x = rand.Next(tuneltexture.Width);
                int y = rand.Next(tuneltexture.Height);

                int id = y * tuneltexture.Width + x;
                if (i % 2 == 0)
                    tun_colors[id] = new Color(90, 90, 90);
                else
                    tun_colors[id] = new Color(66, 66, 66);
            }
            tuneltexture.SetData<Color>(tun_colors);
        }
        void CreatePlanetTexture(Planet planet)
        {
            int planetwidth = planet.mapwidth;
            int planetheight = planet.mapheight;

            Color[] colors = new Color[planetwidth * planetheight];

            Random r = new Random();

            for (int i = 0; i < planetwidth * planetheight; i++)
            {
                int y = i / planetwidth;
                int x = i % planetwidth;

                int gray = (int)(planet.heightmap[y, x] * 128 + 128);
                int rgray = (int)(planet.heightmap[(y + planetheight - 1) % planetheight, (x + 1) % planetwidth] * 128 + 128);

                float fy = Math.Abs((y - planetheight / 2) / (float)(planetheight / 2));
                float temperature = (float)(planet.maxtemperature - (planet.maxtemperature - planet.mintemperature) * fy);

                if (planet.watermap[y, x] && planet.heightmap[y, x] > 0)
                    colors[i] = GetColoFromPlanetGradient(planet,0);
                else
                    colors[i] = GetColoFromPlanetGradient(planet, planet.heightmap[y, x]);

                //int g = (int)((planet.heightmap[y, x]+1)*128);
                //colors[i] = new Color(gray, gray, gray);
            }

            for (int x = 0; x < 32; x++)
            {
                int i = x * planetwidth / 32;
                for (int j = 0; j < planetheight; j++)
                    colors[i + j * planetwidth] = AddHSB(colors[i + j * planetwidth], 0, 0, -20);
            }
            for (int y = 0; y < 16; y++)
            {
                int j = y * planetheight / 16;
                for (int i = 0; i < planetwidth; i++)
                    colors[i + j * planetwidth] = AddHSB(colors[i + j * planetwidth], 0, 0, -20);
            }

            if (planettexture == null || planettexture.Width != planetwidth || planettexture.Height != planetheight)
                planettexture = new Texture2D(GraphicsDevice, planet.mapwidth, planet.mapheight);
            planettexture.SetData<Color>(colors);

            if (planetnormal == null || planetnormal.Width != planetwidth || planetnormal.Height != planetheight)
                planetnormal = new Texture2D(GraphicsDevice, planet.mapwidth, planet.mapheight);

            for (int i = 0; i < planetwidth * planetheight; i++)
            {
                int y = i / planetwidth;
                int x = i % planetwidth;

                Vector3 normal = planet.GetAbsoluteNomal(x, y);

                colors[i] = new Color((normal + Vector3.One) / 2);
            }
            planetnormal.SetData<Color>(colors);

            double sizemod = planet.radius / 6371;
            sizemod = sizemod + (1 - sizemod) / 1.1;

            sizemod = planet.radius / 6371;
            planetSphere = new Sphere((float)sizemod, graphics.GraphicsDevice, 32);
            planetSphere.effect = planetEffect;
        }
        void CreateMinimap(Planet planet, Map map,ref Texture2D minimaptexture)
        {
            minimaptexture = new Texture2D(GraphicsDevice, 64, 64);
            Color[] data = new Color[64 * 64];
            Random r = new Random();

            float h = planet.heightmap[(int)map.position.Y, (int)map.position.X];
            if (h < 0.0000625) h = 0.0000626f;
            Color temp = GetColoFromPlanetGradient(planet, h);
            Color watercolor = GetColoFromPlanetGradient(planet, 0);
            float th = PlanetHelper.BiCubicTexture(planet.heightmap, planet.mapwidth, planet.mapheight, map.position.X, map.position.Y);
            int temph = 0;
            if (th > 0)
            {
                if (th == 1) temph = 15;
                else temph = (short)((int)(th * 15) + 1);
            }
            temph *= 2;

            //------------------------------------------------
            int size = 8;
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 64; j++)
                {
                    float dy = (i - 64 / 2) / (float)(64) * size;
                    float dx = (j - 64 / 2) / (float)(64) * size;
                    float height = PlanetHelper.BiCubicTexture(planet.heightmap, planet.mapwidth, planet.mapheight, map.position.X + dx, map.position.Y + dy);
                    short mheight = 0;

                    if (height > 0)
                    {
                        if (height == 1) mheight = 15;
                        else mheight = (short)((int)(height * 15) + 1);
                    }
                    mheight *= 2;

                    data[i * 64 + j] = mheight <= 0?watercolor:temp;
                    data[i * 64 + j] = AddHSB(data[i * 64 + j], 0, 0, (mheight - temph) * 4);
                }
            minimaptexture.SetData<Color>(data);
        }
        void CreateLocalMapMinimapAndTexture(Planet planet,Map map)
        {
            minimap = new Texture2D(GraphicsDevice, map.width, map.height);
            Color[] data = new Color[map.height * map.width];
            Random r = new Random();

            float h = planet.heightmap[(int)map.position.Y, (int)map.position.X];
            if (h < 0.0000625) h = 0.0000626f;
            Color temp = GetColoFromPlanetGradient(planet,h);
            int temph = map.data[map.height / 2, map.width / 2].height;

            //------------------------------------------------
            int subterrain = 0;
            for (int i = 0; i < map.height; i++)
                for (int j = 0; j < map.width; j++)
                {
                    if (map.data[i, j].height <= 0)
                        data[i * map.width + j] = GetColoFromPlanetGradient(planet, map.data[i, j].height / 30.0f);
                    else
                        data[i * map.width + j] = temp;
                    data[i * map.width + j] = AddHSB(data[i * map.width + j], 0, 0, (map.data[i, j].height - temph) * 4);
                    if (map.data[i, j].id == 64) data[i * map.width + j] = AddHSB(data[i * map.width + j], 20, 20, -29);

                    if (map.data[i, j].subground) subterrain++;
                    if (map.data[i, j].ground_id >= 0) subterrain++;
                    if (map.data[i, j].id == 112 ||
                        map.data[i, j].id == 113 ||
                        map.data[i, j].id == 114 ||
                        map.data[i, j].id == 115 ||
                        map.data[i, j].id == 116 ||
                        map.data[i, j].id == 160 ||
                        map.data[i, j].id == 161 ||
                        map.data[i, j].id == 162 ||
                        map.data[i, j].id == 163 ||
                        map.data[i, j].id == 164 ||
                        map.data[i, j].id == 165 ||
                        map.data[i, j].id == 166 ||
                        map.data[i, j].id == 167 ||
                        map.data[i, j].id == 168 ||
                        map.data[i, j].id == 169 ||
                        map.data[i, j].id == 170) subterrain++;
                }
            minimap.SetData<Color>(data);

            //-------------------------------------------------------------------------------------
            terrainvertexes = new VertexPositionColorTexture[map.width * map.height * 4];
            //terrainindexes = new short[map.width * map.height * 6];
            terrainindexes = new short[map.width * 6];
            subterrainvertexes = new VertexPositionColorTexture[subterrain * 4];
            subterrainindexes = new short[subterrain * 6];
            subterrain=0;
            //----------------------------short[] indexes = new short[6] { 0, 1, 3, 1, 2, 3 };

            float ydepthoffset = 0.6f / map.height;

            for (int j = 0; j < map.width; j++)
            {
                terrainindexes[6 * j] = (short)(4 * j);
                terrainindexes[6 * j + 1] = (short)(4 * j + 1);
                terrainindexes[6 * j + 2] = (short)(4 * j + 3);
                terrainindexes[6 * j + 3] = (short)(4 * j + 1);
                terrainindexes[6 * j + 4] = (short)(4 * j + 2);
                terrainindexes[6 * j + 5] = (short)(4 * j + 3);
            }

            for (int i = 0; i < map.height; i++)
                for (int j = 0; j < map.width; j++)
                {
                    int k = i * map.height + j;

                    int id = map.data[i, j].id;
                    float stx = 32.0f / tilesettemplate.Width;
                    float sty = 32.0f / tilesettemplate.Height;
                    float tx = id % 16 * stx;
                    float ty = id / 16 * sty;

                    terrainvertexes[4 * k] =     new VertexPositionColorTexture(new Vector3(j * 32, i * 32,             GetZ(i * 32)), Color.White, new Vector2(tx, ty));
                    terrainvertexes[4 * k + 1] = new VertexPositionColorTexture(new Vector3(j * 32 + 32, i * 32,        GetZ(i * 32)), Color.White, new Vector2(tx + stx, ty));
                    terrainvertexes[4 * k + 2] = new VertexPositionColorTexture(new Vector3(j * 32 + 32, i * 32 + 32,   GetZ(i * 32)), Color.White, new Vector2(tx + stx, ty + sty));
                    terrainvertexes[4 * k + 3] = new VertexPositionColorTexture(new Vector3(j * 32, i * 32 + 32,        GetZ(i * 32)), Color.White, new Vector2(tx, ty + sty));

                    //terrainindexes[6 * k] = (short)(4 * k);
                    //terrainindexes[6 * k + 1] = (short)(4 * k + 1);
                    //terrainindexes[6 * k + 2] = (short)(4 * k + 3);
                    //terrainindexes[6 * k + 3] = (short)(4 * k + 1);
                    //terrainindexes[6 * k + 4] = (short)(4 * k + 2);
                    //terrainindexes[6 * k + 5] = (short)(4 * k + 3);

                    if (map.data[i, j].ground_id >= 0)
                    {
                        mapHelper.data[i, j].subterrainvertexid = (short)subterrain;
                        k = subterrain;
                        subterrain++;
                        id = map.data[i, j].ground_id;
                        tx = id % 16 * stx;
                        ty = id / 16 * sty;

                        subterrainvertexes[4 * k] = new VertexPositionColorTexture(new Vector3(     j * 32,         i * 32,         GetZ(i*32-1)), Color.White, new Vector2(tx, ty));
                        subterrainvertexes[4 * k + 1] = new VertexPositionColorTexture(new Vector3( j * 32 + 32,    i * 32,         GetZ(i*32-1)), Color.White, new Vector2(tx + stx, ty));
                        subterrainvertexes[4 * k + 2] = new VertexPositionColorTexture(new Vector3( j * 32 + 32,    i * 32 + 32,    GetZ(i*32-1)), Color.White, new Vector2(tx + stx, ty + sty));
                        subterrainvertexes[4 * k + 3] = new VertexPositionColorTexture(new Vector3( j * 32,         i * 32 + 32,    GetZ(i*32-1)), Color.White, new Vector2(tx, ty + sty));

                        subterrainindexes[6 * k] = (short)(4 * k);
                        subterrainindexes[6 * k + 1] = (short)(4 * k + 1);
                        subterrainindexes[6 * k + 2] = (short)(4 * k + 3);
                        subterrainindexes[6 * k + 3] = (short)(4 * k + 1);
                        subterrainindexes[6 * k + 4] = (short)(4 * k + 2);
                        subterrainindexes[6 * k + 5] = (short)(4 * k + 3);
                    }
                    if (map.data[i, j].subground)
                    {
                        mapHelper.data[i, j].subterrainvertexid2 = (short)subterrain;
                        k = subterrain;
                        subterrain++;
                        id = map.data[i, j].height == 0 ? 0 : 16;
                        tx = id % 16 * stx;
                        ty = id / 16 * sty;

                        subterrainvertexes[4 * k] = new VertexPositionColorTexture(new Vector3(     j * 32,         i * 32,         GetZ(i*32-2)), Color.White, new Vector2(tx, ty));
                        subterrainvertexes[4 * k + 1] = new VertexPositionColorTexture(new Vector3( j * 32 + 32,    i * 32,         GetZ(i*32-2)), Color.White, new Vector2(tx + stx, ty));
                        subterrainvertexes[4 * k + 2] = new VertexPositionColorTexture(new Vector3( j * 32 + 32,    i * 32 + 32,    GetZ(i*32-2)), Color.White, new Vector2(tx + stx, ty + sty));
                        subterrainvertexes[4 * k + 3] = new VertexPositionColorTexture(new Vector3( j * 32,         i * 32 + 32,    GetZ(i*32-2)), Color.White, new Vector2(tx, ty + sty));

                        subterrainindexes[6 * k] = (short)(4 * k);
                        subterrainindexes[6 * k + 1] = (short)(4 * k + 1);
                        subterrainindexes[6 * k + 2] = (short)(4 * k + 3);
                        subterrainindexes[6 * k + 3] = (short)(4 * k + 1);
                        subterrainindexes[6 * k + 4] = (short)(4 * k + 2);
                        subterrainindexes[6 * k + 5] = (short)(4 * k + 3);
                    }
                    if (map.data[i, j].id == 112 || 
                        map.data[i, j].id == 113 || 
                        map.data[i, j].id == 114 || 
                        map.data[i, j].id == 115 ||
                        map.data[i, j].id == 116 ||
                        map.data[i, j].id == 160 ||
                        map.data[i, j].id == 161 ||
                        map.data[i, j].id == 162 ||
                        map.data[i, j].id == 163 ||
                        map.data[i, j].id == 164 ||
                        map.data[i, j].id == 165 ||
                        map.data[i, j].id == 166 ||
                        map.data[i, j].id == 167 ||
                        map.data[i, j].id == 168 ||
                        map.data[i, j].id == 169 ||
                        map.data[i, j].id == 170)
                    {
                        mapHelper.data[i, j].subterrainvertexid3 = (short)subterrain;
                        k = subterrain;
                        subterrain++;
                        id = map.data[i, j].id - 32;
                        tx = id % 16 * stx;
                        ty = id / 16 * sty;

                        subterrainvertexes[4 * k] = new VertexPositionColorTexture(new Vector3(     j * 32,         i * 32 - 64,    GetZ(i*32+1)), Color.White, new Vector2(tx, ty));
                        subterrainvertexes[4 * k + 1] = new VertexPositionColorTexture(new Vector3( j * 32 + 32,    i * 32 - 64,    GetZ(i*32+1)), Color.White, new Vector2(tx + stx, ty));
                        subterrainvertexes[4 * k + 2] = new VertexPositionColorTexture(new Vector3( j * 32 + 32,    i * 32,         GetZ(i*32+1)), Color.White, new Vector2(tx + stx, ty + sty+sty));
                        subterrainvertexes[4 * k + 3] = new VertexPositionColorTexture(new Vector3( j * 32,         i * 32,         GetZ(i*32+1)), Color.White, new Vector2(tx, ty + sty+sty));

                        subterrainindexes[6 * k] = (short)(4 * k);
                        subterrainindexes[6 * k + 1] = (short)(4 * k + 1);
                        subterrainindexes[6 * k + 2] = (short)(4 * k + 3);
                        subterrainindexes[6 * k + 3] = (short)(4 * k + 1);
                        subterrainindexes[6 * k + 4] = (short)(4 * k + 2);
                        subterrainindexes[6 * k + 5] = (short)(4 * k + 3);
                    }
                }
            //-------------------------------------------------------------------------------------

            tileset = tilesettemplate;
            tileset = new Texture2D(GraphicsDevice, tilesettemplate.Width, tilesettemplate.Height);
            data = new Color[tilesettemplate.Width * tilesettemplate.Height];
            int[] ids = new int[tilesettemplate.Width * tilesettemplate.Height];
            tilesettemplate.GetData<Color>(data);

            List<Color> pallete = new List<Color>();
            pallete.Add(data[0]);
            for (int i = 0; i < data.Length; i++)
            {
                int id = pallete.IndexOf(data[i]);
                if (id == -1) { pallete.Add(data[i]); id = pallete.Count - 1; }
                ids[i] = id;
            }

            Color grass = GetColoFromPlanetGradient(planet, planet.heightmap[(int)map.position.Y, (int)map.position.X]);
            Color water = GetColoFromPlanetGradient(planet, -0.01f);
            Color rock = AddHSB(grass, -19, -9, -24);
            Color treeleaves = AddHSB(grass, 20, 20, -37);

            for (int i = 0; i < pallete.Count; i++)
            {
                Color c = pallete[i];
                if (c.R == 0 && c.G == 0 && c.B == 0) c = new Color(0, 0, 0, 0);
                else if (c.R == 252 && c.G == 125 && c.B == 42) c = water;                              //water
                else if (c.R == 232 && c.G == 191 && c.B == 117) c = AddHSB(grass, 0, 0, 15);           //outline
                else if (c.R == 225 && c.G == 175 && c.B == 92) c = grass;	                            //grass
                else if (c.R == 229 && c.G == 184 && c.B == 105) c = AddHSB(grass, -1, -10, 5);         //light hill grass (start left)
                else if (c.R == 219 && c.G == 162 && c.B == 75) c = AddHSB(grass, -1, -10, -10);        //dark hill grass (stairs right)
                else if (c.R == 223 && c.G == 169 && c.B == 84) c = AddHSB(grass, -1, -10, -5);         //normal hill grass (starts down and up)
                else if (c.R == 207 && c.G == 136 && c.B == 46) c = AddHSB(grass, -1, -20, -20);        //vertical dark grass
                else if (c.R == 217 && c.G == 158 && c.B == 69) c = AddHSB(grass, -1, -20, -10);        //vertical normal grass
                else if (c.R == 220 && c.G == 163 && c.B == 76) c = AddHSB(grass, -1, 0, -5);           //tree shadow 1
                else if (c.R == 161 && c.G == 108 && c.B == 62) c = AddHSB(rock, 0, 0, -10);            //first shadow after vertical light grass
                else if (c.R == 127 && c.G == 86 && c.B == 58) c = AddHSB(rock, 0, 0, -30);	            //first shadow after vertical dark grass
                else if (c.R == 144 && c.G == 97 && c.B == 60) c = AddHSB(rock, 0, 0, -20);	            //first shadow after vertical normal grass
                else if (c.R == 177 && c.G == 128 && c.B == 81) c = AddHSB(rock, 0, 0, 0);              //second shadow after vertical light grass
                else if (c.R == 146 && c.G == 106 && c.B == 76) c = AddHSB(rock, 0, 0, -20);            //second shadow after vertical dark grass
                else if (c.R == 161 && c.G == 117 && c.B == 78) c = AddHSB(rock, 0, 0, -10);	        //second shadow after vertical normal grass
                else if (c.R == 186 && c.G == 140 && c.B == 92) c = AddHSB(rock, 0, 0, 10);	            //light rock
                else if (c.R == 157 && c.G == 118 && c.B == 87) c = AddHSB(rock, 0, 0, -10);            //dark rocks
                else if (c.R == 171 && c.G == 129 && c.B == 89) c = rock;                               //vertical normal rocks
                else if (c.R == 207 && c.G == 134 && c.B == 45) c = treeleaves;                         //tree leaves
                else if (c.R == 188 && c.G == 121 && c.B == 41) c = AddHSB(treeleaves, 0, 0, -15);      //tree dark leaves
                else if (c.R == 227 && c.G == 182 && c.B == 105) c = AddHSB(grass, -1, -10, 5);         //light hill grass (start left)
                else if (c.R == 214 && c.G == 150 && c.B == 61) c = AddHSB(grass, -1, 0, -10);          //tree shadow 2
                else c = new Color(0,0,0,0);
                pallete[i] = c;
            }

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = pallete[ids[i]];
            }

            tileset.SetData<Color>(data);
        }
        Color GetColoFromPlanetGradient(Planet planet,float height)
        {
            float tileset = (float)((planet.maxtemperature + planet.mintemperature) / 2 + 120) / 70;
            if (tileset > 12) tileset = 12;
            if (tileset < 0) tileset = 0;
            tileset = 12 - tileset;

            Gradient gradient = new Gradient();

            int a = (int)tileset + 1;
            int b = (int)tileset;
            if (a < 0) a = 0; if (a > 12) a = 12;
            if (b < 0) b = 0; if (b > 12) b = 12;
            float f = tileset - b;

            for (int i = 0; i < planetGradient[0].points.Count; i++)
            {
                Color c2 = planetGradient[a].GetColor(planetGradient[0].points[i].position);
                Color c1 = planetGradient[b].GetColor(planetGradient[0].points[i].position);
                gradient.AddPoint(new GradientPart(Color.Lerp(c1, c2, f), planetGradient[0].points[i].position));
            }

            return gradient.GetColor(height);
        }
        Color GetPlayerColor(int player_id)
        {
            if (player_id >= 0 && player_id < fractioncolors.Length)
                return fractioncolors[player_id];
            return fractioncolors[fractioncolors.Length - 1];
        }

        string AddEnterIntoText(string text, int width)
        {
            string newtext = "";

            string[] words = text.Split(new char[] {' '}, StringSplitOptions.None);

            float currentwidth = 0;
            for (int i = 0; i < words.Length; i++)
            {
                int k = words[i].LastIndexOf('\n');
                if (k == -1)
                {
                    Vector2 size = font.MeasureString(words[i] + " ");
                    currentwidth += size.X;
                }
                else
                {
                    Vector2 size = font.MeasureString(words[i].Substring(k, words[i].Length - k) + " ");
                    currentwidth = size.X;
                }

                if (currentwidth < width)
                {
                    newtext += words[i] + " ";
                }
                else
                {
                    currentwidth = 0;
                    newtext += "\n" + words[i] + " ";
                }

            }

            return newtext;
        }

        void LoadShaderConfig()
        {
            Matrix proj = Matrix.Identity;
            proj.M11 = 2.0f / width;
            proj.M22 = -2.0f / height;
            proj.M41 = -1;
            proj.M42 = 1;
            tileEffect.Parameters["Proj"].SetValue(proj);
            baselineShader.Parameters["Projection"].SetValue(proj);
            fractiontileEffect.Parameters["Proj"].SetValue(proj);
            starShader.Parameters["Projection"].SetValue(proj);

            screencapture = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.NormalizedByte4, DepthFormat.Depth24Stencil8);
            powershieldslayer = new RenderTarget2D(GraphicsDevice, width - 200, height);
            emmisionshieldslayer = new RenderTarget2D(GraphicsDevice, width - 200, height);
            atmoshereshieldslayer = new RenderTarget2D(GraphicsDevice, width - 200, height);
        }
        #endregion

        #region Main process delegates
        void SelectBase(int id,int bx,int by)
        {
            if (offlineMode)
            {
                DrawLoadScreen();

                map = planet.maps[id];
                mapHelper = new MapHelper(map);
                selectedbase = id;
                CreateLocalMapMinimapAndTexture(planet, map);

                state = LocalMapMode;
                planetmode = ReviewMode;

                gui.forms[12].Ready();
            }
            else
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                bw.Write(LocalMapData);
                bw.Write(selectedplanet);
                bw.Write(bx);
                bw.Write(by);

                byte[] membuf = mem.GetBuffer();
                byte[] retbuf = new byte[bw.BaseStream.Position];
                Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                bw.Close();
                mem.Close();

                client.SendData(retbuf);
            }
        }
        void StarOverviewModeMain(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (star != null && star.planets != null)
                {
                    Vector2 nearestplanet = new Vector2(0, 0);
                    float nearestdistance = float.MaxValue;
                    int starradius = (int)((Math.Log10(star.radius / 0.00435) / 4 + 1.1) * (width < height ? width : height) / 20);
                    byte selected_planet = 0;

                    for (int i = 0; i < star.planets_num; i++)
                    {
                        double distance = star.planets[i].semimajoraxis / star.radius * 0.00435;
                        if (distance > 4.75) distance = 4.75;
                        if (distance < 0.25) distance = 0.25;

                        Vector2 pos = star.planets[i].GetPosition(i);

                        int minavaiblelength = starradius * 2;  // 0.25
                        int maxavaiblelength = (width < height ? width : height);  // 4.75
                        double scale = minavaiblelength + (maxavaiblelength - minavaiblelength) * ((distance - 0.25) / (4.5));

                        pos = pos * (float)scale / 2;
                        Vector2 planetpos = new Vector2(width / 2 + pos.X, height / 2 + pos.Y);

                        float newnearestdistance = (planetpos - camera.mouse).LengthSquared();
                        if (newnearestdistance < 10000 && newnearestdistance < nearestdistance)
                        {
                            nearestdistance = newnearestdistance;
                            nearestplanet = planetpos;
                            selected_planet = (byte)i;
                            selectedplanet = selected_planet;
                        }
                    }

                    if (nearestdistance < float.MaxValue)
                    {
                        if (offlineMode)
                        {
                            DrawLoadScreen();

                            planet = star.planets[selected_planet];
                            state = PlanetOverviewMode;
                            CreatePlanetTexture(planet);
                            gui.forms[9].Ready();
                        }
                        else
                        {
                            if (star.planets[selected_planet].heightmap != null)
                            {
                                planet = star.planets[selected_planet];
                                state = PlanetOverviewMode;
                                CreatePlanetTexture(planet);
                                gui.forms[9].Ready();

                                packets_num = 0;
                                currentpart = 0;
                                currentlength = 0;
                                splitedData = null;
                            }
                            else
                            {
                                packets_num = 0;
                                currentpart = 0;
                                currentlength = 0;
                                splitedData = null;

                                client.SendData(new byte[2] { PlanetData, selected_planet });
                            }
                        }
                    }
                }
            }
        }
        void PlanetMapModeMain(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                int freew = width;
                int freeh = height - 60;
                if (freew > freeh * 2) freew = freeh * 2;
                if (freew < freeh * 2) freeh = freew / 2;
                int startx = (width - freew) / 2;
                int starty = (height - freeh) / 2;

                float x = (camera.mouse.X - startx) / freew;
                float y = (camera.mouse.Y - starty) / freeh;

                int bx = (int)(x * planet.mapwidth);
                int by = (int)(y * planet.mapheight);

                if (planetmode == CreateBaseMode)
                {
                    newbaseposition_x = (int)(x * planet.mapwidth);
                    newbaseposition_y = (int)(y * planet.mapheight);
                    if (planet.TestCreateBase((int)(x * planet.mapwidth), (int)(y * planet.mapheight)))
                    {
                        newbaseposition_x = (int)(x * planet.mapwidth);
                        newbaseposition_y = (int)(y * planet.mapheight);

                        gui.InputBox(LanguageHelper.Dialog_EnterBaseName, NameGenerator.GeneratePlanetName(), BaseNameInputKey, BaseNameInputOk, BaseNameInputCansel, width / 2, height / 2);
                    }
                }

                if (planetmode == SelectBaseMode)
                {
                    int id = planet.GetBaseId(bx, by);
                    if (id >= 0)
                    {
                        SelectBase(id, bx, by);
                    }
                }

                if (planetmode == MarketBaseMode || planetmode==AttackBaseMode)
                {
                    if (selectedbaseposition_x == -1 && selectedbaseposition_y == -1 && planet.GetBaseId(bx, by) >= 0)
                    {
                        selectedbaseposition_x = bx;
                        selectedbaseposition_y = by;
                    }
                    else if (selectedbaseposition2_x == -1 && selectedbaseposition2_y == -1 && planet.GetBaseId(bx, by) >= 0)
                    {
                        selectedbaseposition2_x = bx;
                        selectedbaseposition2_y = by;

                        if (planetmode == MarketBaseMode)
                            PlanetMapMenuOpenMarketMenu();

                        int base1_id = planet.GetBaseId(selectedbaseposition_x, selectedbaseposition_y);
                        int base2_id = planet.GetBaseId(selectedbaseposition2_x, selectedbaseposition2_y);
                        if (base1_id >= 0 && base2_id >= 0 && planetmode == MarketBaseMode)
                        {
                            gui.popupform.elements[53].text[0] = planet.maps[base1_id].name;
                            gui.popupform.elements[54].text[0] = planet.maps[base2_id].name;
                        }
                        if (base1_id >= 0 && base2_id >= 0 && planetmode == AttackBaseMode)
                        {
                            starttargetbaseitem = base1_id;
                            SelectTargetBase(base2_id);
                            PlanetMapMenuOpenAttackMenu();
                        }

                        planetmode = ReviewMode;
                    }
                }
            }
            if (me.state.r_click) planetmode = ReviewMode;
        }
        void LocalMapModeMain(ref GuiObject me)
        {
            if (me.state.l_click && map.player_id == player_id)
            {
                if (build_id > 0 && map.TryBuilding(camera.onx, camera.ony, build_id))
                {
                    if (offlineMode)
                    {
                        Point size = Building.GetSize(build_id);
                        map.AddBuilding(camera.onx - size.X + 1, camera.ony - size.Y + 1, build_id);
                        //build_id = -1;
                    }
                    else
                    {
                        Point size = Building.GetSize(build_id);
                        map.AddBuilding(camera.onx - size.X + 1, camera.ony - size.Y + 1, build_id);

                        System.IO.MemoryStream mem = new System.IO.MemoryStream();
                        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                        bw.Write(LocalMapAddBuilding);
                        bw.Write(selectedplanet);
                        bw.Write((int)map.position.X);
                        bw.Write((int)map.position.Y);
                        bw.Write(camera.onx - size.X + 1);
                        bw.Write(camera.ony - size.Y + 1);
                        bw.Write(build_id);

                        byte[] membuf = mem.GetBuffer();
                        byte[] retbuf = new byte[bw.BaseStream.Position];
                        Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                        bw.Close();
                        mem.Close();

                        client.SendData(retbuf);

                        //build_id = -1;
                    }
                }
                if (localmapmode == DestroyLocalMode)
                {
                    if (offlineMode)
                    {
                        if (map.data[camera.ony, camera.onx].build_id >= 0)
                            map.DestroyBuilding(camera.onx, camera.ony);
                    }
                    else
                    {
                        if (map.data[camera.ony, camera.onx].build_id >= 0)
                            map.DestroyBuilding(camera.onx, camera.ony);

                        System.IO.MemoryStream mem = new System.IO.MemoryStream();
                        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                        bw.Write(LocalMapDestroyBuilding);
                        bw.Write(selectedplanet);
                        bw.Write((int)map.position.X);
                        bw.Write((int)map.position.Y);
                        bw.Write(camera.onx);
                        bw.Write(camera.ony);

                        byte[] membuf = mem.GetBuffer();
                        byte[] retbuf = new byte[bw.BaseStream.Position];
                        Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                        bw.Close();
                        mem.Close();

                        client.SendData(retbuf);
                    }
                }
                if (localmapmode == EnergyLocalMode)
                {
                    if (offlineMode)
                    {
                        if (map.data[camera.ony, camera.onx].build_id >= 0)
                            map.buildings[map.data[camera.ony, camera.onx].build_id].power = map.buildings[map.data[camera.ony, camera.onx].build_id].power == 1 ? 0 : 1;
                    }
                    else
                    {
                        if (map.data[camera.ony, camera.onx].build_id >= 0)
                            map.buildings[map.data[camera.ony, camera.onx].build_id].power = map.buildings[map.data[camera.ony, camera.onx].build_id].power == 1 ? 0 : 1;

                        System.IO.MemoryStream mem = new System.IO.MemoryStream();
                        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                        bw.Write(LocalMapSwitchBuilding);
                        bw.Write(selectedplanet);
                        bw.Write((int)map.position.X);
                        bw.Write((int)map.position.Y);
                        bw.Write(camera.onx);
                        bw.Write(camera.ony);

                        byte[] membuf = mem.GetBuffer();
                        byte[] retbuf = new byte[bw.BaseStream.Position];
                        Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                        bw.Close();
                        mem.Close();

                        client.SendData(retbuf);
                    }
                }
            }
            if (me.state.r_click)
            {
                if (localmapmode != ReviewMode)
                    localmapmode = ReviewMode;
                else if (build_id == -1 && map.player_id == player_id)
                {
                    selectedbuildid = map.data[camera.ony, camera.onx].build_id;
                    if (selectedbuildid >= 0)
                        OpenLocalMapModeInfoPopup();
                }
                else
                {
                    localmapmode = ReviewMode;
                    build_id = -1;
                }
            }
        }
        void LocalMapMinimapButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                int x = (int)camera.mouse.X - me.rect.X;
                int y = (int)camera.mouse.Y - me.rect.Y;

                camera.x = (int)(x / 180f * map.width * 32 - (width - 200) / 2);
                camera.y = (int)(y / 180f * map.height * 32 - height / 2);

                if (camera.x < 0) camera.x = 0;
                if (camera.x > map.width * 32 - width + 200 - 10) camera.x = map.width * 32 - width + 200 - 10;
                if (camera.y > map.height * 32 - height - 10 + 20) camera.y = map.height * 32 - height - 10 + 20;
                if (camera.y < -20) camera.y = -20f;
            }
        }
        void LoadScript(string filename,ref Script script)
        {
            AppDomain app = AppDomain.CurrentDomain;
            System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFile(app.BaseDirectory + filename);
            script = (Script)(ass.CreateInstance("Script"));
            script.name = filename;
            script.GetEllapcedDelegate(GetEllapced);
            script.DrawCursorDelegate(DrawCursor);
            script.DrawGuiDelegate(DrawGui);
            script.GetScreenDelegate(GetScreen);
            script.GetScreenSizeDelegate(GetScreenSize);
            script.GetGraphicDeviceDelegate(GetGraphicsDevice);
            script.GetContentManagerDelegate(GetContentManager);
            script.GetPlayerNameDelegate(GetPlayerName);
            script.ShowDialogDelegate(ShowDialog);
            script.GetGameModeDelegate(GetGameMode);
            script.SetGameModeDelegate(SetGameMode);
            script.IsPopupDelegate(IsPopup);
            script.GetPopupDelegate(GetPopup);
            script.ShowHintDelegate(ShowHint);
            script.GetMapDelegate(GetMap);
            script.GetStarDelegate(GetStar);
            script.GetPlanetDelegate(GetPlanet);
            script.FinishMissionDelegate(FinishMission);
            script.GetLanguageDelegate(GetLanguage);
            script.DrawTextDelegate(DrawText);
            script.AddTargetTextDelegate(AddTargetText);
            script.Launch();
        }
        #endregion

        #region Script delegates

        float GetEllapced()
        {
            return (float)currentGameTime.ElapsedGameTime.TotalSeconds;
        }
        void DrawCursor()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(guiset, camera.mouse, new Rectangle(20, planetmode * 16, 12, 16), Color.White);
            spriteBatch.End();
        }
        void DrawGui()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            gui.Draw(state, spriteBatch, guiset, font);
            if (messages) messagesystem.Draw(spriteBatch, font, width);
            spriteBatch.End();
        }
        object GetScreenSize()
        {
            return new Point(width, height);
        }
        object GetContentManager()
        {
            return Content;
        }
        object GetScreen()
        {
            //screencapture = new RenderTarget2D(GraphicsDevice, width, height);
            GraphicsDevice.SetRenderTarget(screencapture);
            drawgui = false;
            Draw(currentGameTime);
            GraphicsDevice.SetRenderTarget(null);

            //System.IO.FileStream fs = System.IO.File.Open("screen.png", System.IO.FileMode.Create);
            //screencapture.SaveAsPng(fs, width, height);
            //fs.Close();

            return screencapture;
        }
        object GetGraphicsDevice()
        {
            return GraphicsDevice;
        }
        string GetPlayerName()
        {
            return playername;
        }
        void ShowDialog(string text)
        {
            text = AddEnterIntoText(text, 450);
            Vector2 size = font.MeasureString(text);
            int sizex = (int)size.X + 55;
            int startx = (width - sizex) / 2;
            int starty = (height - (int)size.Y) / 2;
            gui.AddPopup( new Form(0, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,starty-10,sizex+20,(int)size.Y+20,null),
                                                        new GuiObject(GuiObjectState.LayerNotAligned,startx+55,starty,sizex,(int)size.Y,null,text),
                                                        new GuiObject(GuiObjectState.PanelDark,startx,starty,45,45,null),
                                                        new GuiObject(GuiObjectState.Layer,startx,starty,45,45,null,"[ ! ]"),
                                                        new GuiObject(GuiObjectState.MenuButton,startx+sizex+15,starty-10,45,45,LocalMapModeClosePopup,"x"),}));
            gui.popupformid = -1;
        }
        void ShowHint(string text)
        {
            text = AddEnterIntoText(text, 500);
            Vector2 size = font.MeasureString(text);
            int sizex = (int)size.X;
            int startx = (width - sizex) / 2;
            int starty = (height - (int)size.Y) / 2;
            gui.AddPopup( new Form(0, new GuiObject[]{new GuiObject(GuiObjectState.PanelDark,startx-10,starty-10,sizex+20,(int)size.Y+20,null),
                                                        new GuiObject(GuiObjectState.LayerNotAligned,startx,starty,sizex,(int)size.Y,null,text),
                                                        new GuiObject(GuiObjectState.MenuButton,startx+sizex+15,starty-10,45,45,LocalMapModeClosePopup,"x"),}));
            gui.popupformid = -1;
        }
        void SetGameMode(int mode)
        {
            state = mode;
        }
        int GetGameMode()
        {
            return state;
        }
        bool IsPopup()
        {
            return gui.popupform != null;
        }
        int GetPopup()
        {
            return gui.popupformid;
        }
        object GetMap()
        {
            if (state == LocalMapMode)
                return map;
            return null;
        }
        object GetStar()
        {
            if (state == StarOverviewMode||state==PlanetOverviewMode||state==PlanetMapMode||state==LocalMapMode)
                return star;
            return null;
        }
        object GetPlanet()
        {
            if (state == PlanetOverviewMode || state == PlanetMapMode || state == LocalMapMode)
                return planet;
            return null;
        }
        void FinishMission()
        {
            OpenScore(star.GetScore(player_id));
            gui.popupform.elements[10].enable = true;
            gui.popupform.elements[11].enable = false;
            SetMenuState(0);
        }
        int GetLanguage()
        {
            return playerlanguage;
        }
        void DrawText(string text, int a, int b,Color color)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, new Vector2(a, b), color);
            spriteBatch.End();
        }
        void AddTargetText(string text)
        {
            gui.queststring = text;
        }
        #endregion

        #region Gui delegates
        void MainChangePlayerNameButton(ref GuiObject me)
        {
            if (me.state.l_click)
                gui.InputBox(LanguageHelper.Dialog_EnterPlayerName, playername, ChangePlayerNameKey, ChangePlayerNameOk, ChangePlayerNameBack, width / 2, height / 2);
        }
        void MainChangePlayerColorButton2(ref GuiObject me)
        {
            if (me.state.l_click)
                player_id = (short)(player_id <= 30 ? (player_id + 1) : 0);
        }
        void MainChangePlayerColorButton1(ref GuiObject me)
        {
            if (me.state.l_click)
                player_id = (short)(player_id > 0 ? (player_id - 1) : 31);
        }

        void ChangePlayerNameKey(ref GuiObject me)
        {
         KeyboardState ks = Keyboard.GetState();

            Keys[] keys = ks.GetPressedKeys();
            bool upper = ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift);
            foreach (Keys k in keys)
            {
                if (oldkeyboardstate.IsKeyUp(k))
                {
                    string s = "";
                    switch (k)
                    {
                        case Keys.NumPad1:
                        case Keys.D1: s = "1"; break;
                        case Keys.NumPad2:
                        case Keys.D2: s = "2"; break;
                        case Keys.NumPad3:
                        case Keys.D3: s = "3"; break;
                        case Keys.NumPad4:
                        case Keys.D4: s = "4"; break;
                        case Keys.NumPad5:
                        case Keys.D5: s = "5"; break;
                        case Keys.NumPad6:
                        case Keys.D6: s = "6"; break;
                        case Keys.NumPad7:
                        case Keys.D7: s = "7"; break;
                        case Keys.NumPad8:
                        case Keys.D8: s = "8"; break;
                        case Keys.NumPad9:
                        case Keys.D9: s = "9"; break;
                        case Keys.NumPad0:
                        case Keys.D0: s = "0"; break;
                        case Keys.Space: s = " "; break;
                        case Keys.OemComma: s = ","; break;
                        case Keys.OemMinus: s = "-"; break;
                        case Keys.OemPeriod: s = "."; break;
                        case Keys.Enter: me.state.l_click = true; ChangePlayerNameOk(ref me); return;
                        case Keys.Escape: me.state.l_click = true; ChangePlayerNameBack(ref me); return;
                        case Keys.Back: if (me.text[0].Length > 0) me.text[0] = me.text[0].Substring(0, me.text[0].Length - 1); break;
                        default: s = k.ToString(); break;
                    }
                    if (s.Length == 1) me.text[0] += (upper ? s.ToUpper() : s.ToLower());
                }
            }
        }
        void ChangePlayerNameOk(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.lastinputstring = gui.popupform.elements[2].text[0];
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();

                playername = gui.lastinputstring;
                gui.forms[1].elements[10].text[0] = playername + " - "+LanguageHelper.Gui_ChangeNickname;

                SaveConfig();
            }
        }
        void ChangePlayerNameBack(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();
            }
        }

        void SaveConfig()
        {
            System.IO.FileStream fs = System.IO.File.Open("config.cfg", System.IO.FileMode.OpenOrCreate);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);

            bw.Write(playername);
            bw.Write(width);
            bw.Write(height);
            bw.Write(fullscreen);
            bw.Write(music);
            bw.Write(musicvolume);
            bw.Write(autosave);
            bw.Write(messages);
            bw.Write(player_id);
            bw.Write(playerlanguage);

            newwidth = width;
            newheight = height;
            newfullscreen = fullscreen;

            bw.Close();
            fs.Close();
        }
        bool LoadConfig()
        {
            try
            {
                System.IO.FileStream fs = System.IO.File.Open("config.cfg", System.IO.FileMode.Open);
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);

                try
                {
                    playername = br.ReadString();
                    width = br.ReadInt32();
                    height = br.ReadInt32();
                    fullscreen = br.ReadBoolean();
                    music = br.ReadBoolean();
                    musicvolume = br.ReadSingle();
                    autosave = br.ReadBoolean();
                    messages = br.ReadBoolean();
                    player_id = br.ReadInt16();
                    playerlanguage = br.ReadInt32();

                    newwidth = width;
                    newheight = height;
                    newfullscreen = fullscreen;
                }
                catch
                {
                    br.Close();
                    fs.Close();
                    return false;
                }

                br.Close();
                fs.Close();
                return true;
            }
            catch { return false; }
        }

        //--------------------------------------Menu functions
        void MainToGameMenuButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[2].Ready(); state = StartGameMenuMode; } }
        void MainToSettingsMenuButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[3].Ready(); state = SettingsMenuMode; } }
        void MainToHelpMenuButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[4].Ready(); state = HelpMenuMode; } }
        void MainToAboutMenuButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[5].Ready(); state = AboutMenuMode; } }
        void ExitGameButton(ref GuiObject me) { if (me.state.l_click) gui.MessageBox(LanguageHelper.Gui_LeaveMission, ExitYesButton, ExitNoButton, width / 2, height / 2); }

        //--------------------------------------Gui helper functions
        void SliderUpdate(ref GuiObject me)
        {
            int maxpos = me.rect.Width - 34 - 11;
            int startx = me.rect.X + (me.rect.Width - maxpos) / 2;

            int dx = (int)camera.mouse.X - startx;
            if (dx <= 0)
            {
                me.reserved -= 1.0f / maxpos;
                if (me.reserved < 0) me.reserved = 0;
            }
            else if (dx >= maxpos)
            {
                me.reserved += 1.0f / maxpos;
                if (me.reserved > 1) me.reserved = 1;
            }
            else me.reserved = ((float)dx) / maxpos;
        }

        void BackGameMenuButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (state_old == MainMenuMode)
                    state = state_old;
                    //----------------set ready for every form
                else
                {
                    state = StarOverviewMode;
                    gui.forms[10].Ready();
                    //foreach (Form f in gui.forms)
                    //    f.Ready();
                }
            }
        }
        void LeaveMissionButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (!offlineMode)
                    client.Disconnect();

                map = null;
                planet = null;
                star = null;
                normalexit = true;
                SetMenuState(0);
                scriptes = null;
            }
        }
        void LoadButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                state = LoadMode;
                selectedloadid = 0;
                filenames = System.IO.Directory.GetFiles("Content/Saves/", "*.save");
                fileinfos=new string[filenames.Length];
                for (int i = 0; i < filenames.Length; i++)
                {
                    DateTime dt = System.IO.File.GetCreationTime(filenames[i]);
                    fileinfos[i] = dt.ToShortDateString() + "/" + dt.ToShortTimeString();
                    filenames[i] = filenames[i].Substring(14);
                }
            }
        }
        void SaveButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.InputBox(LanguageHelper.Dialog_SaveAs, "autosave", SaveInputKey, SaveInputOk, SaveInputCansel, width / 2, height / 2);
            }
        }

        void SaveInputKey(ref GuiObject me)
        {
            KeyboardState ks = Keyboard.GetState();

            Keys[] keys = ks.GetPressedKeys();
            bool upper = ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift);
            foreach (Keys k in keys)
            {
                if (oldkeyboardstate.IsKeyUp(k))
                {
                    string s = "";
                    switch (k)
                    {
                        case Keys.NumPad1:
                        case Keys.D1: s = "1"; break;
                        case Keys.NumPad2:
                        case Keys.D2: s = "2"; break;
                        case Keys.NumPad3:
                        case Keys.D3: s = "3"; break;
                        case Keys.NumPad4:
                        case Keys.D4: s = "4"; break;
                        case Keys.NumPad5:
                        case Keys.D5: s = "5"; break;
                        case Keys.NumPad6:
                        case Keys.D6: s = "6"; break;
                        case Keys.NumPad7:
                        case Keys.D7: s = "7"; break;
                        case Keys.NumPad8:
                        case Keys.D8: s = "8"; break;
                        case Keys.NumPad9:
                        case Keys.D9: s = "9"; break;
                        case Keys.NumPad0:
                        case Keys.D0: s = "0"; break;
                        case Keys.Space: s = " "; break;
                        case Keys.OemComma: s = ","; break;
                        case Keys.OemMinus: s = "-"; break;
                        case Keys.OemPeriod: s = "."; break;
                        case Keys.Enter: me.state.l_click = true; SaveInputOk(ref me); return;
                        case Keys.Escape: me.state.l_click = true; SaveInputCansel(ref me); return;
                        case Keys.Back: if (me.text[0].Length > 0) me.text[0] = me.text[0].Substring(0, me.text[0].Length - 1); break;
                        default: s = k.ToString(); break;
                    }
                    if (s.Length == 1) me.text[0] += (upper ? s.ToUpper() : s.ToLower());
                }
            }
        }
        void SaveInputOk(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.lastinputstring = gui.popupform.elements[2].text[0];
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();

                if (offlineMode)
                {
                    string filename = "Content/Saves/" + gui.lastinputstring + ".save";

                    SaveGame(filename);
                }
                else
                {
                    messagesystem.AddMessage(LanguageHelper.Dialog_CannotSaveInMultyplayer, 2);
                }
            }
        }
        void SaveInputCansel(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();
            }
        }

        void MessageInputKey(ref GuiObject me)
        {
            KeyboardState ks = Keyboard.GetState();

            Keys[] keys = ks.GetPressedKeys();
            bool upper = ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift);
            foreach (Keys k in keys)
            {
                if (oldkeyboardstate.IsKeyUp(k))
                {
                    string s = "";
                    switch (k)
                    {
                        case Keys.NumPad1:
                        case Keys.D1: s = "1"; break;
                        case Keys.NumPad2:
                        case Keys.D2: s = "2"; break;
                        case Keys.NumPad3:
                        case Keys.D3: s = "3"; break;
                        case Keys.NumPad4:
                        case Keys.D4: s = "4"; break;
                        case Keys.NumPad5:
                        case Keys.D5: s = "5"; break;
                        case Keys.NumPad6:
                        case Keys.D6: s = "6"; break;
                        case Keys.NumPad7:
                        case Keys.D7: s = "7"; break;
                        case Keys.NumPad8:
                        case Keys.D8: s = "8"; break;
                        case Keys.NumPad9:
                        case Keys.D9: s = "9"; break;
                        case Keys.NumPad0:
                        case Keys.D0: s = "0"; break;
                        case Keys.Space: s = " "; break;
                        case Keys.OemComma: s = ","; break;
                        case Keys.OemMinus: s = "-"; break;
                        case Keys.OemPeriod: s = "."; break;
                        case Keys.Enter: me.state.l_click = true; MessageInputOk(ref me); return;
                        case Keys.Escape: me.state.l_click = true; MessageInputCansel(ref me); return;
                        case Keys.Back: if (me.text[0].Length > 0) me.text[0] = me.text[0].Substring(0, me.text[0].Length - 1); break;
                        default: s = k.ToString(); break;
                    }
                    if (s.Length == 1) me.text[0] += (upper ? s.ToUpper() : s.ToLower());
                }
            }
        }
        void MessageInputOk(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.lastinputstring = gui.popupform.elements[2].text[0];
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();

                createnewsaydialog = false;

                if (offlineMode)
                {
                    messagesystem.AddMessage(gui.lastinputstring, 10);
                }
                else
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

                    bw.Write(Message);
                    bw.Write(gui.lastinputstring);

                    client.SendData(ms.GetBuffer());

                    bw.Close();
                    ms.Close();
                }
            }
        }
        void MessageInputCansel(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();
            }
        }

        void LoadModeButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                int size = 600;
                int startx = (width - size - 200) / 2;
                int starty = (height - size) / 2;

                string text;
                Vector2 v;
                int offcety = 0;

                for (int i = 0; i < filenames.Length; i++)
                {

                    text = filenames[i];
                    v = font.MeasureString(text);
                    if (new Rectangle(startx + 10, starty + 10 + offcety, size - 20, (int)v.Y).Contains((int)camera.mouse.X, (int)camera.mouse.Y))
                        selectedloadid = i;
                    offcety += (int)v.Y + 8;
                }
            }
        }
        void LoadModeBackButton(ref GuiObject me) { if (me.state.l_click) { state = StartGameMenuMode;} }
        void LoadModeSelectButton(ref GuiObject me)
        {
            if (me.state.l_click && filenames.Length > 0)
                LoadGame("Content/Saves/" + filenames[selectedloadid]);
        }
        void LoadModeDeleteButton(ref GuiObject me)
        {
            if (me.state.l_click && filenames.Length > 0)
                gui.MessageBox(LanguageHelper.Dialog_DeleteSavedGame, DeleteGame, ExitNoButton, width / 2, height / 2);
        }

        void DeleteGame(ref GuiObject me)
        {
            if (me.state.l_click && filenames.Length > 0)
            {
                System.IO.File.Delete("Content/Saves/" + filenames[selectedloadid]);
                filenames = System.IO.Directory.GetFiles("Content/Saves/", "*.save");
                fileinfos = new string[filenames.Length];
                for (int i = 0; i < filenames.Length; i++)
                {
                    DateTime dt = System.IO.File.GetCreationTime(filenames[i]);
                    fileinfos[i] = dt.ToShortDateString() + "/" + dt.ToShortTimeString();
                    filenames[i] = filenames[i].Substring(14);
                }
                if (selectedloadid >= filenames.Length) selectedloadid = filenames.Length - 1;
                if (filenames.Length == 0) selectedloadid = 0;

                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();
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
            if (scriptes != null)
            {
                bw.Write(true);
                bw.Write(scriptes.Length);
                for (int i = 0; i < scriptes.Length; i++)
                {
                    bw.Write(scriptes[i].name);
                    scriptes[i].Save(bw);
                    bw.Write(scriptes[i].waittime);
                }
            }
            else bw.Write(false);

            bw.Write(player_id);

            bw.Close();
            fs.Close();

            //messagesystem.AddMessage("Сохранено: " + filename, 2);
            messagesystem.AddMessage(LanguageHelper.Gui_Saved + ": " + filename, 2);
        }
        void LoadGame(string filename)
        {
            DrawLoadScreen();

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

            bool isscript = br.ReadBoolean();
            if (isscript)
            {
                scriptes = new Script[1];
                int length = br.ReadInt32();
                scriptes = new Script[length];
                for (int i = 0; i < length; i++)
                {
                    string name = br.ReadString();
                    LoadScript(name, ref scriptes[i]);
                    scriptes[i].Load(br);
                    scriptes[i].waittime = br.ReadSingle();
                }
            }
            else
            {
                scriptes = null;
            }

            player_id = br.ReadInt16();

            state = StarOverviewMode;
            gui.forms[10].Ready();

            br.Close();
            fs.Close();

            timetonextautosave = nextautosavetime;
            messagesystem.AddMessage(LanguageHelper.Gui_Loaded + ": " + filename, 2);
        }

        void LocalMapModeCreditsStreamButton(ref GuiObject me)
        {
            if (me.state.l_click&&map.player_id==player_id)
            {
                gui.popupformid = 11;
                int s_size = 600;
                int s_startx = (width - s_size-200) / 2;
                int s_starty = (height - 90-37) / 2;

                gui.AddPopup( new Form(0, new GuiObject[] { 
                    new GuiObject(GuiObjectState.PanelDark, s_startx,   s_starty + 37,  s_size,     90, null) ,
                    new GuiObject(GuiObjectState.PanelDark, s_startx,   s_starty,       s_size-120, 38, null),
                    new GuiObject(GuiObjectState.Layer,     s_startx,   s_starty,       s_size-120, 38, null, "Потоки"),

                    new GuiObject(GuiObjectState.MenuButton,     s_startx+10,           s_starty+60,       (s_size-20)/2-5, 38, CreditStreamTo, "Пл. Станция 500 кр. >"),
                    new GuiObject(GuiObjectState.MenuButton,     s_startx+s_size/2+5,   s_starty+60,       (s_size-20)/2-5, 38, CreditStremFrom, "< 500 кр."+map.name),
                    
                    new GuiObject(GuiObjectState.PanelDark, s_startx+s_size-50, s_starty, 50, 38, LocalMapModeClosePopup),
                    new GuiObject(GuiObjectState.Layer, s_startx+s_size-50, s_starty, 50, 38, null, "x")}));

                gui.popupform.elements[3].text[0] = "Пл. Станция x " + map.planet.star.GetMoney(player_id).ToString("0") + " | 500 кр. >";
                gui.popupform.elements[4].text[0] = "< 500 кр. | "+map.name+" x "+map.inventory[(int)Resources.credits].count.ToString("0");
            }
        }

        void CreditStreamTo(ref GuiObject me) 
        {
            if (me.state.l_click && map.planet.star.TryPayMoney(500, player_id))
            {
                map.inventory[(int)Resources.credits].count += 500;
                gui.popupform.elements[3].text[0] = "Пл. Станция x " + map.planet.star.GetMoney(player_id).ToString("0") + " | 500 кр. >";
                gui.popupform.elements[4].text[0] = "< 500 кр. | "+map.name+" x "+map.inventory[(int)Resources.credits].count.ToString("0");

                if (!offlineMode)
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                    bw.Write(CreditStream);
                    bw.Write(selectedplanet);
                    bw.Write((int)map.position.X);
                    bw.Write((int)map.position.Y);
                    bw.Write(player_id);
                    bw.Write((byte)0);

                    byte[] membuf = mem.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem.Close();

                    client.SendData(retbuf);
                }
            }
        }
        void CreditStremFrom(ref GuiObject me) 
        { 
            if (me.state.l_click && map.inventory[(int)Resources.credits].count >= 500) 
            { 
                map.inventory[(int)Resources.credits].count -= 500; 
                map.planet.star.TryAddMoney(500, player_id); 
                gui.popupform.elements[3].text[0] = "Пл. Станция x " + map.planet.star.GetMoney(player_id).ToString("0") + " | 500 кр. >";
                gui.popupform.elements[4].text[0] = "< 500 кр. | "+map.name+" x "+map.inventory[(int)Resources.credits].count.ToString("0");

                if (!offlineMode)
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                    bw.Write(CreditStream);
                    bw.Write(selectedplanet);
                    bw.Write((int)map.position.X);
                    bw.Write((int)map.position.Y);
                    bw.Write(player_id);
                    bw.Write((byte)1);

                    byte[] membuf = mem.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem.Close();

                    client.SendData(retbuf);
                }
            } 
        }

        void GoMission0Button(ref GuiObject me) 
        { 
            if (me.state.l_click) 
            {
                DrawLoadScreen();

                star = new Star();
                star.GeneratePlanets(3);
                player_id = 6;
                star.players.Add(new PlayerStation(playername, player_id));
                star.pirates = false;
                star.meteorites = false;
                star.sunstrike = false;
                selectedplanet = -1;
                state = StarOverviewMode;

                timetonextautosave = float.MaxValue;

                scriptes = new Script[1];
                LoadScript("Content/Mutators/tutorial.dll",ref scriptes[0]);
            } 
        }
        void GoMission1Button(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                DrawLoadScreen();

                star = new Star(0.25, 10, 6700);
                star.GeneratePlanets(7);
                player_id = 18;
                star.players.Add(new PlayerStation(playername, player_id));
                star.pirates = false;
                star.meteorites = true;
                star.sunstrike = false;
                selectedplanet = -1;
                state = StarOverviewMode;

                timetonextautosave = nextautosavetime;

                scriptes = new Script[1];
                LoadScript("Content/Mutators/episode1_update.dll",ref scriptes[0]);
            }
        }
        void GameMenuModeToUniverseModeButton(ref GuiObject me) { if (me.state.l_click) { state = UniverceMode; selectedsector = -1; selectedstar = -1; gui.forms[8].Ready(); } }
        void GameMenuModeToCatalogModeButton(ref GuiObject me) { if (me.state.l_click) { state = CatalogMode; gui.forms[21].Ready(); } }
        void GameMenuModeToLoadModeButton(ref GuiObject me) { LoadButton(ref me); }
        void GameMenuModeToMainMenuModeButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[1].Ready(); state = MainMenuMode; } }

        void SettingsMenuModeMusicButton(ref GuiObject me) { if (me.state.l_click) { music = !music; me.text[0] = LanguageHelper.Gui_Music+": " + (music ? LanguageHelper.Gui_on : LanguageHelper.Gui_off); if (!music)MediaPlayer.Stop(); else MediaPlayer.Play(songs[6]); } }
        void SettingMenuModeMusicVolumeSlider(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); musicvolume = me.reserved; MediaPlayer.Volume = me.reserved; } }
        void SettingsMenuModeAutosaveButton(ref GuiObject me) { if (me.state.l_click) { autosave = !autosave; me.text[0] = LanguageHelper.Gui_Autosave + ": " + (autosave ? LanguageHelper.Gui_on : LanguageHelper.Gui_off); } }
        void SettingsMenuModeMessagesButton(ref GuiObject me) { if (me.state.l_click) { messages = !messages; me.text[0] = LanguageHelper.Gui_Messages + ": " + (messages ? LanguageHelper.Gui_on : LanguageHelper.Gui_off); } }
        void SettingsMenuModeToMainMenuButton(ref GuiObject me) 
        { 
            if (me.state.l_click) 
            { 
                gui.forms[1].Ready(); state = MainMenuMode;
                if (newwidth != width || newheight != height || newfullscreen != fullscreen)
                {
                    graphics.PreferredBackBufferWidth = newwidth;
                    graphics.PreferredBackBufferHeight = newheight;
                    graphics.IsFullScreen = newfullscreen;

                    graphics.ApplyChanges();
                    width = newwidth;
                    fullscreen = newfullscreen;
                    height = newheight;

                    //width = 1350;
                    //height = 650;

                    SaveConfig();
                    LoadShaderConfig();
                }
                LoadLanguage();
                needresetgui = true;
            } 
        }
        void SettingsMenuModeResolutionButton(ref GuiObject me) 
        { 
            if (me.state.l_click) 
            {
                selecteddisplaymode++;
                selecteddisplaymode %= displaymodes.Length;
                newwidth = displaymodes[selecteddisplaymode].X;
                newheight = displaymodes[selecteddisplaymode].Y;

                gui.forms[3].elements[9].text[0] = newwidth.ToString() + " x " + newheight.ToString();
            } 
        }
        void SettingsMenuModeFullscreenButton(ref GuiObject me) { if (me.state.l_click) { newfullscreen = !newfullscreen; me.text[0] = LanguageHelper.Gui_Fullscreen + ": " + (!newfullscreen ? LanguageHelper.Gui_on : LanguageHelper.Gui_off); } }
        void SettingsSetLanguage(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                playerlanguage = (playerlanguage + 1) % 2;
                gui.forms[3].elements[11].text[0] = LanguageHelper.Gui_Language + ":" + (playerlanguage == LanguageRus ? LanguageHelper.Gui_LanguageRus : LanguageHelper.Gui_LanguageEng);
            }
        }

        void HelpMenuModeUpButton(ref GuiObject me)
        {
            if (me.state.l_click && starthelpstring > 0)
            {
                starthelpstring--;
            }

            if (camera.mouseWheel != camera.mouseWheelOld)
            {
                starthelpstring -= (camera.mouseWheel - camera.mouseWheelOld) / 100;
                if (starthelpstring < 0) starthelpstring = 0;
                if (starthelpstring > Helper.help.Length - 20) starthelpstring = Helper.help.Length - 20;
            }
        }
        void HelpMenuModeDownButton(ref GuiObject me) 
        {
            if (me.state.l_click && starthelpstring > Helper.help.Length - 20)
            {
                starthelpstring++;
            }
        }
        void HelpMenuModeToMainMenuMenuButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[1].Ready(); state = MainMenuMode; } }
        void AboutMenuModeToChronoModeButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[6].Ready(); state = ChronoMenuMode; } }
        void AboutMenuModeToPromoModeButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[7].Ready(); state = PromoMenuMode; } }
        void AboutMenuModeToMenuModeButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[1].Ready(); state = MainMenuMode; } }

        void ChonoMenuModeToAboutMenuModeButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[5].Ready(); state = AboutMenuMode; } }
        void PromoMenuModeUpButton(ref GuiObject me) 
        {
            if (me.state.l_click && startpromostring > 0)
            {
                startpromostring--;
            }

            if (camera.mouseWheel != camera.mouseWheelOld)
            {
                startpromostring -= (camera.mouseWheel - camera.mouseWheelOld) / 100;
                if (startpromostring < 0) startpromostring = 0;
                if (startpromostring > Helper.promo.Length - 20) startpromostring = Helper.promo.Length - 20;
            }
        }
        void PromoMenuModeDownButton(ref GuiObject me) 
        {
            if (me.state.l_click && startpromostring > Helper.promo.Length - 20)
            {
                startpromostring++;
            }
        }
        void PromoMenuModeToAboutMenuModeButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[5].Ready(); state = AboutMenuMode; } }

        void UniverceButton(ref GuiObject me) 
        {
            if (me.state.l_click)
            {
                if (selectedsector < 0)
                {
                    int x = (int)camera.mouse.X - me.rect.X;
                    int y = (int)camera.mouse.Y - me.rect.Y;

                    x /= 74; y /= 74;
                    if (x < 0) x = 0; if (x > 7) x = 7;
                    if (y < 0) y = 0; if (y > 7) y = 7;

                    selectedsector = y * 8 + x;
                }
                else
                {
                    int x = selectedsector % 8;
                    int y = selectedsector / 8;

                    int start_x = gui.forms[8].elements[1].rect.X;
                    int start_y = gui.forms[8].elements[1].rect.Y;

                    foreach (int i in univerce.subsectors[selectedsector])
                    {
                        Vector2 pos = (univerce.positions[i] - new Vector2(x * 74, y * 74)) * 8;
                        pos.X += start_x; pos.Y += start_y;
                        //int starradius = (int)((Math.Log10(univerce.stars[i].radius / 0.00435) / 4 + 1.1) * 5);

                        float a = (float)((univerce.stars[i].size + 1) / 2);
                        int starradius = (int)(4 + 4 * a);

                        Rectangle rect = new Rectangle((int)pos.X - starradius + 4, (int)pos.Y - starradius + 4, starradius * 2, starradius * 2);

                        if (rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))
                        {
                            selectedstar = i;
                            selectedplanet = -1;
                            break;
                        }
                    }
                }
            }
        }
        void UniverceModeToStartGameMenuModeButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (selectedsector >= 0)
                    selectedsector = -1;
                else state = StartGameMenuMode;
            }
        }
        void UniverceModeToStarOverviewModeButton(ref GuiObject me) 
        {
            if (me.state.l_click && selectedstar>0)
            {
                star = univerce.stars[selectedstar];
                star.players.Add(new PlayerStation(playername, player_id));
                int seed = (int)((star.mass + star.radius + star.temperature) * 235);

                star.GeneratePlanets(seed, star.GetPlanetNum());
                state = StarOverviewMode;
                selectedplanet = -1;

                timetonextautosave = nextautosavetime;
            }
        }

        void GameMenuModeToConnectModeButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.InputBox(LanguageHelper.Dialog_EnterSeverAdress, "localhost", ConnectInputKey, ConnectInputOk, ConnectInputCansel, width / 2, height / 2);
            }
        }
        void ConnectInputKey(ref GuiObject me)
        {
            KeyboardState ks = Keyboard.GetState();

            Keys[] keys = ks.GetPressedKeys();
            bool upper = ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift);
            foreach (Keys k in keys)
            {
                if (oldkeyboardstate.IsKeyUp(k))
                {
                    string s = "";
                    switch (k)
                    {
                        case Keys.NumPad1:
                        case Keys.D1: s = "1"; break;
                        case Keys.NumPad2:
                        case Keys.D2: s = "2"; break;
                        case Keys.NumPad3:
                        case Keys.D3: s = "3"; break;
                        case Keys.NumPad4:
                        case Keys.D4: s = "4"; break;
                        case Keys.NumPad5:
                        case Keys.D5: s = "5"; break;
                        case Keys.NumPad6:
                        case Keys.D6: s = "6"; break;
                        case Keys.NumPad7:
                        case Keys.D7: s = "7"; break;
                        case Keys.NumPad8:
                        case Keys.D8: s = "8"; break;
                        case Keys.NumPad9:
                        case Keys.D9: s = "9"; break;
                        case Keys.NumPad0:
                        case Keys.D0: s = "0"; break;
                        case Keys.Space: s = " "; break;
                        case Keys.OemComma: s = ","; break;
                        case Keys.OemMinus: s = "-"; break;
                        case Keys.OemPeriod: s = "."; break;
                        case Keys.Enter: me.state.l_click = true; ConnectInputOk(ref me); return;
                        case Keys.Escape: me.state.l_click = true; ConnectInputCansel(ref me); return;
                        case Keys.Back: if (me.text[0].Length > 0) me.text[0] = me.text[0].Substring(0, me.text[0].Length - 1); break;
                        default: s = k.ToString(); break;
                    }
                    if (s.Length == 1) me.text[0] += (upper ? s.ToUpper() : s.ToLower());
                }
            }
        }
        void ConnectInputOk(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.lastinputstring = gui.popupform.elements[2].text[0];
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();

                hostIP = gui.lastinputstring;

                //client.SendBufferSize = 16000;
                //client.ReceiveBufferSize = 16000;

                client.Connect(hostIP, 3666, playername);
                //client.SendBufferSize = 80000;
                offlineMode = false;
            }
        }
        void ConnectInputCansel(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();
            }
        }

        void StarOverviewModeOpenScoreButton(ref GuiObject me)
        {
            if (me.state.l_click)
                OpenScore(star.GetScore(player_id));
        }

        void AllModesToMenuMode(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                state_old = state;
                state = MainMenuMode;
                SetMenuState(1);
                gui.forms[1].Ready();
            }
        }
        void PlanetOverviewModeToStarOverviewMode(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                state = StarOverviewMode;
                gui.forms[10].Ready();
            }
        }
        void PlanetOverviewModeToPlanetMapMode(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (offlineMode)
                {
                    state = PlanetMapMode;
                    gui.forms[11].Ready();
                }
                else
                {
                    client.SendData(new byte[] { LocalMapsDescriptions, (byte)selectedplanet });
                }
            }
        }
        void PlanetMapMenuToPlanetOverview(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                state = PlanetOverviewMode;
                gui.forms[9].Ready();
            }
        }

        void PlanetMapModeCreateBase(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                planetmode = CreateBaseMode;
            }
        }
        void PlanetMapModeToLocalMapMode(ref GuiObject me) 
        { 
            if (me.state.l_click) 
            { 
                planetmode = SelectBaseMode; 
            } 
        }

        void BaseNameInputKey(ref GuiObject me)
        {
            KeyboardState ks = Keyboard.GetState();

            Keys[] keys = ks.GetPressedKeys();
            bool upper = ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift);
            foreach (Keys k in keys)
            {
                if (oldkeyboardstate.IsKeyUp(k))
                {
                    string s = "";
                    switch (k)
                    {
                        case Keys.NumPad1:
                        case Keys.D1: s = "1"; break;
                        case Keys.NumPad2:
                        case Keys.D2: s = "2"; break;
                        case Keys.NumPad3:
                        case Keys.D3: s = "3"; break;
                        case Keys.NumPad4:
                        case Keys.D4: s = "4"; break;
                        case Keys.NumPad5:
                        case Keys.D5: s = "5"; break;
                        case Keys.NumPad6:
                        case Keys.D6: s = "6"; break;
                        case Keys.NumPad7:
                        case Keys.D7: s = "7"; break;
                        case Keys.NumPad8:
                        case Keys.D8: s = "8"; break;
                        case Keys.NumPad9:
                        case Keys.D9: s = "9"; break;
                        case Keys.NumPad0:
                        case Keys.D0: s = "0"; break;
                        case Keys.Space: s = " "; break;
                        case Keys.OemComma: s = ","; break;
                        case Keys.OemMinus: s = "-"; break;
                        case Keys.OemPeriod: s = "."; break;
                        case Keys.Enter: me.state.l_click = true; BaseNameInputOk(ref me); return;
                        case Keys.Escape: me.state.l_click = true; BaseNameInputCansel(ref me); return;
                        case Keys.Back: if (me.text[0].Length > 0) me.text[0] = me.text[0].Substring(0, me.text[0].Length - 1); break;
                        default: s = k.ToString(); break;
                    }
                    if (s.Length == 1) me.text[0] += (upper ? s.ToUpper() : s.ToLower());
                }
            }
        }
        void BaseNameInputOk(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.lastinputstring = gui.popupform.elements[2].text[0];
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();

                if (offlineMode)
                {
                    DrawLoadScreen();

                    if (planet.CreateBase(newbaseposition_x, newbaseposition_y, player_id))
                    {
                        map = planet.maps[planet.maps.Count - 1];
                        map.player_id = player_id;
                        state = LocalMapMode;
                        planetmode = ReviewMode;
                        mapHelper = new MapHelper(map);
                        CreateLocalMapMinimapAndTexture(planet, map);

                        gui.forms[12].Ready();
                    }
                }
                else
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                    bw.Write(LocalMapCreate);
                    bw.Write(selectedplanet);
                    bw.Write(newbaseposition_x);
                    bw.Write(newbaseposition_y);
                    bw.Write(player_id);

                    byte[] membuf = mem.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem.Close();

                    client.SendData(retbuf);
                }
            }
        }
        void BaseNameInputCansel(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();

                planetmode = ReviewMode;
            }
        }

        void CatalogModeStarSizeSlider(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); catalogstarlighting = me.reserved; }}
        void CatalogModeStarTemperatureSlider(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); catalogstartemperature = me.reserved; } }
        void CatalogModePlatetSizeSlider(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); catalogplanetradius = me.reserved; } }
        void CatalogModePlanetLengthSlider(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); catalogplanetsemiaxis = me.reserved; } }
        void CatalogModeFoundButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                DrawLoadScreen();

                float sl, st, pr, pa;
                double size = (Math.Abs(catalogstarlighting * 2)) / 10;
                sl = (float)Math.Abs(Math.Pow(10, 4 * size));
                float tx = catalogstartemperature * 7;
                st = (float)(26.389 * Math.Pow(tx, 6) -
                             550 * Math.Pow(tx, 5) +
                             4555.6 * Math.Pow(tx, 4) -
                             19042 * Math.Pow(tx, 3) +
                             41918 * Math.Pow(tx, 2) -
                             44408 * tx + 19500);

                star = new Star(size, sl, st);
                star.players.Add(new PlayerStation(playername, player_id));

                star.pirates = catalogpirates;
                star.meteorites = catalogmeteorites;
                star.sunstrike = catalogsunstrikes;

                pr = (catalogplanetradius * 2 - 1) / 1.5f * 11500 + 13500;
                pa = (float)(star.radius / 0.00435 * (catalogplanetsemiaxis * 4.5f + 0.25f));

                star.GeneratePlanets(catalogplanetradius, pr, pa);
                selectedplanet = -1;
                state = StarOverviewMode;

                scriptes = null;

                timetonextautosave = nextautosavetime;
            }
        }
        void CatalogModeBackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[2].Ready(); state = StartGameMenuMode; } }
        void CatalogModeSunStrikesButton(ref GuiObject me) { if (me.state.l_click) { catalogsunstrikes = !catalogsunstrikes; me.text[0] = LanguageHelper.Gui_Sunstrikesshort + ": " + (catalogsunstrikes ? LanguageHelper.Gui_on : LanguageHelper.Gui_off); } }
        void CatalogModePiratesButton(ref GuiObject me) { if (me.state.l_click) { catalogpirates = !catalogpirates; me.text[0] = LanguageHelper.Gui_Piratesshort + ": " + (catalogpirates ? LanguageHelper.Gui_on : LanguageHelper.Gui_off); } }
        void CatalogModeMeteoritesButton(ref GuiObject me) { if (me.state.l_click) { catalogmeteorites = !catalogmeteorites; me.text[0] = LanguageHelper.Gui_Meteoritesshort + ": " + (catalogmeteorites ? LanguageHelper.Gui_on : LanguageHelper.Gui_off); } }

        void LocalMapModeToPlanetMapMode(ref GuiObject me) { if (me.state.l_click) { gui.forms[11].Ready(); state = PlanetMapMode; } }

        void LocalMapModeBuildingsToManagementButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[13].enable = false; gui.forms[14].enable = true; gui.forms[14].Ready(); } }
        void LocalMapModeBuildingsToManufacturingButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[13].enable = false; gui.forms[15].enable = true; gui.forms[15].Ready(); } }
        void LocalMapModeBuildingsToStorageButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[13].enable = false; gui.forms[16].enable = true; gui.forms[16].Ready(); } }
        void LocalMapModeBuildingsToLinksButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[13].enable = false; gui.forms[17].enable = true; gui.forms[17].Ready(); } }
        void LocalMapModeBuildingsToScienceButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[13].enable = false; gui.forms[18].enable = true; gui.forms[18].Ready(); } }
        void LocalMapModeBuildingsToDefenceButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[13].enable = false; gui.forms[19].enable = true; gui.forms[19].Ready(); } }
        void LocalMapModeBuildingsDestroyButton(ref GuiObject me) { if (me.state.l_click) { localmapmode = (localmapmode == DestroyLocalMode ? ReviewMode : DestroyLocalMode); } }
        void LocalMapModeBuildingsSwitchButton(ref GuiObject me) { if (me.state.l_click) { localmapmode = (localmapmode == EnergyLocalMode ? ReviewMode : EnergyLocalMode); } }
        void LocalMapModeBuildingsToAttackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[13].enable = false; gui.forms[22].enable = true; gui.forms[22].Ready(); } }

        void LocalMapModeBuildingsCommandCenterButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.CommandCenter; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.CommandCenter); }
        void LocalMapModeBuildingsBuildCenterButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.BuildCenter; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.BuildCenter); }
        void LocalMapModeBuildingsStorageCenterButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.StorageCenter; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.StorageCenter); }
        void LocalMapModeBuildingsLinkCenterButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.LinksCenter; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.LinksCenter); }
        void LocalMapModeBuildingsScienceCenterButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.ScienceCenter; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.ScienceCenter); }
        void LocalMapModeBuildingsManagementBackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[14].enable = false; gui.forms[13].enable = true; gui.forms[13].Ready(); } }

        void LocalMapModeBuildingsDroinFactoryButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.DroidFactory; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.DroidFactory); }
        void LocalMapModeBuildingsProcessingFactoryButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.ProcessingFactory; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.ProcessingFactory); }
        void LocalMapModeBuildingsMineButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Mine; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Mine); }
        void LocalMapModeBuildingsDirrickButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Dirrick; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Dirrick); }
        void LocalMapModeBuildingsFarmButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Farm; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Farm); }
        void LocalMapModeBuildingsCloseFarmButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.ClosedFarm; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.ClosedFarm); }
        void LocalMapModeBuildingsGeneratorButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Generator; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Generator); }
        void LocalMapModeBuildingsManufacturingBackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[15].enable = false; gui.forms[13].enable = true; gui.forms[13].Ready(); } }

        void LocalMapModeBuildingsAttackFactoryButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.AttackFactory; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.AttackFactory); }
        void LocalMapModeBuildingsAttackParkingButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.AttackParking; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.AttackParking); }
        void LocalMapModeBuildingsRocketParkingButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.RocketParking; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.RocketParking); }
        void LocalMapModeBuildingsAttackBackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[22].enable = false; gui.forms[13].enable = true; gui.forms[13].Ready(); } }

        void LocalMapModeBuildingsWarehouseButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Warehouse; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Warehouse); }
        void LocalMapModeBuildingsHouseButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.House; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.House); }
        void LocalMapModeBuildingsEnergyBankButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.EnergyBank; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.EnergyBank); }
        void LocalMapModeBuildingsInformationBankButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.InfoStorage; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.InfoStorage); }
        void LocalMapModeBuildingsLuquidStorageButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.LuquidStorage; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.LuquidStorage); }
        void LocalMapModeBuildingsStorageBackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[16].enable = false; gui.forms[13].enable = true; gui.forms[13].Ready(); } }

        void LocalMapModeBuildingsSpaceportButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Spaceport; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Spaceport); }
        void LocalMapModeBuildingsExchangeButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Exchanger; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Exchanger); }
        void LocalMapModeBuildingsParckingButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Parking; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Parking); }
        void LocalMapModeBuildingsBeaconButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Beacon; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Beacon); }
        void LocalMapModeBuildingsBuilderButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Builder; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Builder); }
        void LocalMapModeBuildingsLinksBackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[17].enable = false; gui.forms[13].enable = true; gui.forms[13].Ready(); } }

        void LocalMapModeBuildingsLaboratoryButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Laboratory; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Laboratory); }
        void LocalMapModeBuildingsCollectorButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Collector; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Collector); }
        void LocalMapModeBuildingsTerrinScanerButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.TerrainScaner; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.TerrainScaner); }
        void LocalMapModeBuildingsScienceBackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[18].enable = false; gui.forms[13].enable = true; gui.forms[13].Ready(); } }

        void LocalMapModeBuildingsAtmoshereShieldButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.AtmosophereShield; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.AtmosophereShield); }
        void LocalMapModeBuildingsPowerShieldButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.PowerShield; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.PowerShield); }
        void LocalMapModeBuildingsEmmisionShieldButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.EmmisionShield; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.EmmisionShield); }
        void LocalMapModeBuildingsTurretButton(ref GuiObject me) { if (me.state.l_click)build_id = Building.Turret; if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = Building.GetOverview(Building.Turret); }
        void LocalMapModeBuildingsDefenceBackButton(ref GuiObject me) { if (me.state.l_click) { gui.forms[19].enable = false; gui.forms[13].enable = true; gui.forms[13].Ready(); } }

        void LocalMapModeClosePopup(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();
            }
        }
        void LocalMapMenuOpenEnergyMenu(ref GuiObject me)
        {
            if (me.state.l_click && map.player_id == player_id)
            {
                gui.popupformid = 2;

                int panelsizex = 557 + 16 + 17;
                int panelstartx = (width - 200 - panelsizex) / 2;
                int panelstarty = (height - 392) / 2;

                gui.AddPopup( new Form(0, new GuiObject[] { 
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty + 37, panelsizex, 325, null) ,
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty, panelsizex-120, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx, panelstarty, panelsizex-120, 38, null, LanguageHelper.Popup_EnergyPopup),
                        new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex-50, panelstarty,50, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx+panelsizex-50, panelstarty, 50, 38, LocalMapModeClosePopup, "x"),

                        new GuiObject(GuiObjectState.Layer,panelstartx,panelstarty+37+24,150,30,null,LanguageHelper.Popup_Administry),
                        new GuiObject(GuiObjectState.Layer,panelstartx,panelstarty+37+24+31,150,30,null,LanguageHelper.Popup_Manufacturing),
                        new GuiObject(GuiObjectState.Layer,panelstartx,panelstarty+37+24+62,150,30,null,LanguageHelper.Popup_Mining),
                        new GuiObject(GuiObjectState.Layer,panelstartx,panelstarty+37+24+93,150,30,null,LanguageHelper.Popup_Science),
                        new GuiObject(GuiObjectState.Layer,panelstartx,panelstarty+37+24+124,150,30,null,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.Layer,panelstartx,panelstarty+37+24+155,150,30,null,LanguageHelper.BuildMenu_Links),
                        new GuiObject(GuiObjectState.Layer,panelstartx,panelstarty+37+24+186,150,30,null,LanguageHelper.Popup_Defence),

                        new GuiObject(GuiObjectState.Layer,panelstartx+150+10,panelstarty+37+24,40,30,null,"100%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+10,panelstarty+37+24+31,40,30,null,"80%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+10,panelstarty+37+24+62,40,30,null,"100%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+10,panelstarty+37+24+93,40,30,null,"100%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+10,panelstarty+37+24+124,40,30,null,"90%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+10,panelstarty+37+24+155,40,30,null,"100%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+10,panelstarty+37+24+186,40,30,null,"100%"),

                        new GuiObject(GuiObjectState.Layer,panelstartx+150+60,panelstarty+37+24,40,30,null,"10%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+60,panelstarty+37+24+31,40,30,null,"20%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+60,panelstarty+37+24+62,40,30,null,"20%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+60,panelstarty+37+24+93,40,30,null,"10%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+60,panelstarty+37+24+124,40,30,null,"20%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+60,panelstarty+37+24+155,40,30,null,"10%"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+150+60,panelstarty+37+24+186,40,30,null,"10%"),

                        new GuiObject(GuiObjectState.Slider,panelstartx + 284, panelstarty + 37+24, panelsizex-308, 30, LocalMapModeEnergyPopupSlider1),
                        new GuiObject(GuiObjectState.Slider,panelstartx + 284, panelstarty + 37+24 + 31, panelsizex-308, 30, LocalMapModeEnergyPopupSlider2),
                        new GuiObject(GuiObjectState.Slider,panelstartx + 284, panelstarty + 37+24 + 62, panelsizex-308, 30, LocalMapModeEnergyPopupSlider3),
                        new GuiObject(GuiObjectState.Slider,panelstartx + 284, panelstarty + 37+24 + 93, panelsizex-308, 30, LocalMapModeEnergyPopupSlider4),
                        new GuiObject(GuiObjectState.Slider,panelstartx + 284, panelstarty + 37+24 + 124, panelsizex-308, 30, LocalMapModeEnergyPopupSlider5),
                        new GuiObject(GuiObjectState.Slider,panelstartx + 284, panelstarty + 37+24 + 155, panelsizex-308, 30, LocalMapModeEnergyPopupSlider6),
                        new GuiObject(GuiObjectState.Slider,panelstartx + 284, panelstarty + 37+24 + 186, panelsizex-308, 30, LocalMapModeEnergyPopupSlider7),
                
                        new GuiObject(GuiObjectState.Layer,panelstartx+24,panelstarty+37+24+186+45,250,30,null,LanguageHelper.Popup_AllConsum),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+270,panelstarty+37+24+186+45,250,30,null,LanguageHelper.Popup_AllProduce),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24,panelstarty+37+24+186+75,250,30,null,"34"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+270,panelstarty+37+24+186+75,250,30,null,"0")}));

                if (map != null)
                {
                    gui.popupform.elements[26].reserved = map.buildingtypeinfo[0].maxpower;
                    gui.popupform.elements[27].reserved = map.buildingtypeinfo[1].maxpower;
                    gui.popupform.elements[28].reserved = map.buildingtypeinfo[2].maxpower;
                    gui.popupform.elements[29].reserved = map.buildingtypeinfo[3].maxpower;
                    gui.popupform.elements[30].reserved = map.buildingtypeinfo[4].maxpower;
                    gui.popupform.elements[31].reserved = map.buildingtypeinfo[5].maxpower;
                    gui.popupform.elements[32].reserved = map.buildingtypeinfo[6].maxpower;                 
                }
            }
        }

        void UpdateBuildingMaxProductive(int type, float value)
        {
            if (offlineMode)
            {
                map.buildingtypeinfo[type].maxpower = value;
            }
            else
            {
                map.buildingtypeinfo[type].maxpower = value;

                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                bw.Write(LocalMapSwitchMaxEnergy);

                bw.Write(selectedplanet);
                bw.Write((int)map.position.X);
                bw.Write((int)map.position.Y);
                bw.Write((byte)type);
                bw.Write(value);

                byte[] membuf = mem.GetBuffer();
                byte[] retbuf = new byte[bw.BaseStream.Position];
                Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                bw.Close();
                mem.Close();

                client.SendData(retbuf);
            }
        }
        void LocalMapModeEnergyPopupSlider1(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); UpdateBuildingMaxProductive(0, me.reserved); } }
        void LocalMapModeEnergyPopupSlider2(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); UpdateBuildingMaxProductive(1, me.reserved); } }
        void LocalMapModeEnergyPopupSlider3(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); UpdateBuildingMaxProductive(2, me.reserved); } }
        void LocalMapModeEnergyPopupSlider4(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); UpdateBuildingMaxProductive(3, me.reserved); } }
        void LocalMapModeEnergyPopupSlider5(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); UpdateBuildingMaxProductive(4, me.reserved); } }
        void LocalMapModeEnergyPopupSlider6(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); UpdateBuildingMaxProductive(5, me.reserved); } }
        void LocalMapModeEnergyPopupSlider7(ref GuiObject me) { if (me.state.l_click) { SliderUpdate(ref me); UpdateBuildingMaxProductive(6, me.reserved); } }

        void LocalMapMenuOpenInventoryMenu(ref GuiObject me)
        {
            if (me.state.l_click && map.player_id == player_id)
            {
                gui.popupformid = 3;

                int panelsizex = 557 + 16 + 17;
                int panelstartx = (width - 200 - panelsizex) / 2;
                int panelstarty = (height - 392) / 2;

                gui.AddPopup( new Form(0, new GuiObject[] { 
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty + 37, panelsizex, 325, null) ,
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty, panelsizex-120, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx, panelstarty, panelsizex-120, 38, null, LanguageHelper.Popup_ResourcePopup),
                        new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex-50, panelstarty,50, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx+panelsizex-50, panelstarty, 50, 38, LocalMapModeClosePopup, "x"),

                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24,100,30,null,LanguageHelper.Popup_Resource+"1"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+31,100,30,null,LanguageHelper.Popup_Resource+"2"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+62,100,30,null,LanguageHelper.Popup_Resource+"3"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+93,100,30,null,LanguageHelper.Popup_Resource+"4"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+124,100,30,null,LanguageHelper.Popup_Resource+"5"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+155,100,30,null,LanguageHelper.Popup_Resource+"6"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+186,100,30,null,LanguageHelper.Popup_Resource+"7"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+217,100,30,null,LanguageHelper.Popup_Resource+"8"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+248,100,30,null,LanguageHelper.Popup_Resource+"9"),

                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24,30,30,null),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+31,30,30,null),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+62,30,30,null),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+93,30,30,null),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+124,30,30,null),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+155,30,30,null),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+186,30,30,null),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+217,30,30,null),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+248,30,30,null),

                        new GuiObject(GuiObjectState.Button,panelstartx+551,panelstarty+37+24,30,30,LocalMapModeInventoryUpButton),
                        new GuiObject(GuiObjectState.Button,panelstartx+551,panelstarty+37+24+325-48-30,30,30,LocalMapModeInventoryDownButton),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+551,panelstarty+37+24+38,30,217-8-8,LocalMapModeInventorySlider),

                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24,30,30,LocalMapModeInventoryPopupChangeValueButton1),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24+31,30,30,LocalMapModeInventoryPopupChangeValueButton2),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24+62,30,30,LocalMapModeInventoryPopupChangeValueButton3),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24+93,30,30,LocalMapModeInventoryPopupChangeValueButton4),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24+124,30,30,LocalMapModeInventoryPopupChangeValueButton5),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24+155,30,30,LocalMapModeInventoryPopupChangeValueButton6),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24+186,30,30,LocalMapModeInventoryPopupChangeValueButton7),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24+217,30,30,LocalMapModeInventoryPopupChangeValueButton8),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+24+462,panelstarty+37+24+248,30,30,LocalMapModeInventoryPopupChangeValueButton9),

                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24,80,30,LocalMapModeInventoryPopupChangeModeButton1,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24+31,80,30,LocalMapModeInventoryPopupChangeModeButton2,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24+62,80,30,LocalMapModeInventoryPopupChangeModeButton3,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24+93,80,30,LocalMapModeInventoryPopupChangeModeButton4,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24+124,80,30,LocalMapModeInventoryPopupChangeModeButton5,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24+155,80,30,LocalMapModeInventoryPopupChangeModeButton6,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24+186,80,30,LocalMapModeInventoryPopupChangeModeButton7,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24+217,80,30,LocalMapModeInventoryPopupChangeModeButton8,LanguageHelper.Popup_Storage),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+266,panelstarty+37+24+248,80,30,LocalMapModeInventoryPopupChangeModeButton9,LanguageHelper.Popup_Storage),

                        }));
            }
        }

        void UpdateInventoryExchangeMode(int index,int mode, float value)
        {
            if (map.isBuildingBuilded(Building.Exchanger))
            {

                int[] importres = new int[] { (int)Resources.alcohol, (int)Resources.battery, (int)Resources.beton, (int)Resources.biowaste, (int)Resources.chemical, (int)Resources.coal, (int)Resources.composition,
                                    (int)Resources.electronics,(int)Resources.energyore,(int)Resources.explosive,(int)Resources.fish,(int)Resources.fosphat,(int)Resources.fruits,(int)Resources.gems,
                                    (int)Resources.glay,(int)Resources.meat,(int)Resources.medicine,(int)Resources.metal,(int)Resources.metan,(int)Resources.oil,(int)Resources.ore,(int)Resources.plastic,
                                    (int)Resources.rare_gas,(int)Resources.rock,(int)Resources.sand,(int)Resources.vegetables,(int)Resources.water};
                int[] bustedexportres = new int[] { (int)Resources.energy, (int)Resources.credits };

                map.inventory[index].exchangetype = (byte)mode;
                map.inventory[index].exchangecount = value;

                if (map.inventory[index].exchangetype == 1 && Array.IndexOf(importres, index) < 0) map.inventory[index].exchangetype = 2;
                if (map.inventory[index].exchangetype == 2 && Array.IndexOf(bustedexportres, index) >= 0) map.inventory[index].exchangetype = 0;

                if (!offlineMode)
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                    bw.Write(LocalMapSwitchExchange);

                    bw.Write(selectedplanet);
                    bw.Write((int)map.position.X);
                    bw.Write((int)map.position.Y);
                    bw.Write(index);
                    bw.Write((byte)mode);
                    bw.Write(value);

                    byte[] membuf = mem.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem.Close();

                    client.SendData(retbuf);
                }
            }
        }
        void UpdateInventoryExchangeValue(int index, Rectangle rect)
        {
            float dx = camera.mouse.X - rect.X;
            float dy = camera.mouse.Y - rect.Y;

            if (dx > dy) map.inventory[index].exchangecount += 10;
            else map.inventory[index].exchangecount -= 10;

            if (map.inventory[index].exchangecount < 0) map.inventory[index].exchangecount = 0;
            else if (!offlineMode)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                bw.Write(LocalMapSwitchExchange);

                bw.Write(selectedplanet);
                bw.Write((int)map.position.X);
                bw.Write((int)map.position.Y);
                bw.Write(index);
                bw.Write(map.inventory[index].exchangetype);
                bw.Write(map.inventory[index].exchangecount);

                byte[] membuf = mem.GetBuffer();
                byte[] retbuf = new byte[bw.BaseStream.Position];
                Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                bw.Close();
                mem.Close();

                client.SendData(retbuf);
            }
        }
        void LocalMapModeInventoryPopupChangeModeButton1(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem, (map.inventory[startexchangeitem].exchangetype + 1) % 3, 0); }
        void LocalMapModeInventoryPopupChangeModeButton2(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem + 1, (map.inventory[startexchangeitem + 1].exchangetype + 1) % 3, 0); }
        void LocalMapModeInventoryPopupChangeModeButton3(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem + 2, (map.inventory[startexchangeitem + 2].exchangetype + 1) % 3, 0); }
        void LocalMapModeInventoryPopupChangeModeButton4(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem + 3, (map.inventory[startexchangeitem + 3].exchangetype + 1) % 3, 0); }
        void LocalMapModeInventoryPopupChangeModeButton5(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem + 4, (map.inventory[startexchangeitem + 4].exchangetype + 1) % 3, 0); }
        void LocalMapModeInventoryPopupChangeModeButton6(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem + 5, (map.inventory[startexchangeitem + 5].exchangetype + 1) % 3, 0); }
        void LocalMapModeInventoryPopupChangeModeButton7(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem + 6, (map.inventory[startexchangeitem + 6].exchangetype + 1) % 3, 0); }
        void LocalMapModeInventoryPopupChangeModeButton8(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem + 7, (map.inventory[startexchangeitem + 7].exchangetype + 1) % 3, 0); }
        void LocalMapModeInventoryPopupChangeModeButton9(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeMode(startexchangeitem + 8, (map.inventory[startexchangeitem + 8].exchangetype + 1) % 3, 0); }

        void LocalMapModeInventoryPopupChangeValueButton1(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem, me.rect); }
        void LocalMapModeInventoryPopupChangeValueButton2(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem + 1, me.rect); }
        void LocalMapModeInventoryPopupChangeValueButton3(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem + 2, me.rect); }
        void LocalMapModeInventoryPopupChangeValueButton4(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem + 3, me.rect); }
        void LocalMapModeInventoryPopupChangeValueButton5(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem + 4, me.rect); }
        void LocalMapModeInventoryPopupChangeValueButton6(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem + 5, me.rect); }
        void LocalMapModeInventoryPopupChangeValueButton7(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem + 6, me.rect); }
        void LocalMapModeInventoryPopupChangeValueButton8(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem + 7, me.rect); }
        void LocalMapModeInventoryPopupChangeValueButton9(ref GuiObject me) { if (me.state.l_click)UpdateInventoryExchangeValue(startexchangeitem + 8, me.rect); }

        void LocalMapModeInventoryUpButton(ref GuiObject me)
        {
            if (me.state.l_click)
                if (startexchangeitem > 0) startexchangeitem--;
        }
        void LocalMapModeInventoryDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
                if (startexchangeitem < Map.maxresources - 9) startexchangeitem++;
        }
        void LocalMapModeInventorySlider(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                float pos = camera.mouse.Y - me.rect.Y;
                startexchangeitem = (int)pos * Map.maxresources / me.rect.Height;

                if (startexchangeitem > Map.maxresources - 9) startexchangeitem = Map.maxresources - 9;
                if (startexchangeitem < 0) startexchangeitem = 0;
            }

            if (camera.mouseWheel != camera.mouseWheelOld)
            {
                int dw = camera.mouseWheelOld - camera.mouseWheel;
                startexchangeitem += dw / 30;
                if (startexchangeitem > Map.maxresources - 9) startexchangeitem = Map.maxresources - 9;
                if (startexchangeitem < 0) startexchangeitem = 0;
            }
        }

        void LocalMapMenuOpenScienceMenu(ref GuiObject me)
        {
            if (me.state.l_click && map.player_id == player_id)
            {
                gui.popupformid = 4;

                int panelsizex = 557 + 16 + 17;
                int panelstartx = (width - 200 - panelsizex) / 2;
                int panelstarty = (height - 392) / 2;

                gui.AddPopup( new Form(0, new GuiObject[] { 
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty + 37, panelsizex, 325, null) ,
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty, panelsizex-120, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx, panelstarty, panelsizex-120, 38, null, LanguageHelper.Popup_SciencePopup),
                        new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex-50, panelstarty,50, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx+panelsizex-50, panelstarty, 50, 38, LocalMapModeClosePopup, "x"),

                        new GuiObject(GuiObjectState.Button,panelstartx+551,panelstarty+37+24,30,30,LocalMapModeScienceUpButton),
                        new GuiObject(GuiObjectState.Button,panelstartx+551,panelstarty+37+24+325-48-30,30,30,LocalMapModeScienceDownButton),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+551,panelstarty+37+24+38,30,217-8-8,LocalMapModeScienceSlider),

                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24,panelstarty+37+24,40,30,LocalMapModeScienceSwitchButton1,LanguageHelper.Popup_BaseScience),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+24+5,panelstarty+37+55,35,30,LocalMapModeScienceSwitchButton2,LanguageHelper.Popup_ModuleScience),

                        new GuiObject(GuiObjectState.PanelDark,panelstartx+panelsizex-224,panelstarty+37+24,150,325-24-24,null),

                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24,254,30,LocalMapModeSciencePopupSelectButton1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24+31,254,30,LocalMapModeSciencePopupSelectButton2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24+62,254,30,LocalMapModeSciencePopupSelectButton3),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24+93,254,30,LocalMapModeSciencePopupSelectButton4),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24+124,254,30,LocalMapModeSciencePopupSelectButton5),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24+155,254,30,LocalMapModeSciencePopupSelectButton6),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24+186,254,30,LocalMapModeSciencePopupSelectButton7),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24+217,254,30,LocalMapModeSciencePopupSelectButton8),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+88,panelstarty+37+24+248,254,30,LocalMapModeSciencePopupSelectButton9),
                        }));
            }
        }

        void LocalMapModeScienceSwitchButton1(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (map.currentresearchmode != 0)
                {
                    gui.popupform.elements[8].rect.X -= 5;
                    gui.popupform.elements[8].rect.Width += 5;

                    gui.popupform.elements[9].rect.X += 5;
                    gui.popupform.elements[9].rect.Width -= 5;

                    map.currentresearchmode = 0;
                    startresearchitem = 0;

                    //reset string
                    for (int i = 0; i < 9; i++)
                        gui.popupform.elements[11 + i].enable = false;
                    for (int i = 0; i < 9; i++)
                        if (i == map.science[map.currentresearchmode].items.Length) break;
                        else gui.popupform.elements[11 + i].enable = true;
                }
            }
        }
        void LocalMapModeScienceSwitchButton2(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (map.currentresearchmode != 1)
                {
                    gui.popupform.elements[9].rect.X -= 5;
                    gui.popupform.elements[9].rect.Width += 5;

                    gui.popupform.elements[8].rect.X += 5;
                    gui.popupform.elements[8].rect.Width -= 5;

                    map.currentresearchmode = 1;
                    startresearchitem = 0;

                    //reset string
                    for (int i = 0; i < 9; i++)
                        gui.popupform.elements[11 + i].enable = false;
                    for (int i = 0; i < 9; i++)
                        if (i == map.science[map.currentresearchmode].items.Length) break;
                        else gui.popupform.elements[11 + i].enable = true;
                }
            }
        }
        void LocalMapModeScienceUpButton(ref GuiObject me)
        {
            if (me.state.l_click)
                if (startresearchitem > 0) startresearchitem--;
        }
        void LocalMapModeScienceDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
                if (startresearchitem < map.science[map.currentresearchmode].items.Length - 9) startresearchitem++;
        }
        void LocalMapModeScienceSlider(ref GuiObject me)
        {
            if (me.state.l_click && map.science[map.currentresearchmode].items.Length > 9)
            {
                float pos = camera.mouse.Y - me.rect.Y;
                startresearchitem = (int)pos * map.science[map.currentresearchmode].items.Length / me.rect.Height;

                if (startresearchitem > map.science[map.currentresearchmode].items.Length - 9) startresearchitem = map.science[map.currentresearchmode].items.Length - 9;
                if (startresearchitem < 0) startexchangeitem = 0;
            }

            if (camera.mouseWheel != camera.mouseWheelOld && map.science[map.currentresearchmode].items.Length > 9)
            {
                int dw = camera.mouseWheelOld - camera.mouseWheel;
                startresearchitem += dw / 30;
                if (startresearchitem > map.science[map.currentresearchmode].items.Length - 9) startresearchitem = map.science[map.currentresearchmode].items.Length - 9;
                if (startresearchitem < 0) startresearchitem = 0;
            }
        }

        void UpdateSelecteResearch(int mode,int id)
        {
            map.currentresearchmode = mode;
            map.selectedresearch[mode] = id;

            if (!offlineMode)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                bw.Write(LocalMapSwitchResearch);

                bw.Write(selectedplanet);
                bw.Write((int)map.position.X);
                bw.Write((int)map.position.Y);
                bw.Write(mode);
                bw.Write(id);

                byte[] membuf = mem.GetBuffer();
                byte[] retbuf = new byte[bw.BaseStream.Position];
                Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                bw.Close();
                mem.Close();

                client.SendData(retbuf);
            }
        }
        void LocalMapModeSciencePopupSelectButton1(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem].overview; }
        void LocalMapModeSciencePopupSelectButton2(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem + 1); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem + 1].overview; }
        void LocalMapModeSciencePopupSelectButton3(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem + 2); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem + 2].overview; }
        void LocalMapModeSciencePopupSelectButton4(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem + 3); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem + 3].overview; }
        void LocalMapModeSciencePopupSelectButton5(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem + 4); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem + 4].overview; }
        void LocalMapModeSciencePopupSelectButton6(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem + 5); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem + 5].overview; }
        void LocalMapModeSciencePopupSelectButton7(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem + 6); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem + 6].overview; }
        void LocalMapModeSciencePopupSelectButton8(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem + 7); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem + 7].overview; }
        void LocalMapModeSciencePopupSelectButton9(ref GuiObject me) { if (me.state.l_click)UpdateSelecteResearch(map.currentresearchmode, startresearchitem + 8); if (me.rect.Contains((int)camera.mouse.X, (int)camera.mouse.Y))gui.helpstring = map.science[map.currentresearchmode].items[startresearchitem + 8].overview; }

        void PlanetMapMenuOpenAttackMenu()
        {
            gui.popupformid = 22;
            starttargetbaseitem = 0;
            countfighters = 0;

            int panelsizex = 557 + 16 + 17;
            int panelstartx = (width - panelsizex) / 2;
            int panelstarty = (height - 392) / 2;

            gui.AddPopup(new Form(0, new GuiObject[] { 
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty + 37, panelsizex, 325, null) ,
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty, panelsizex-120, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx, panelstarty, panelsizex-120, 38, null, LanguageHelper.Popup_AttackScreen),
                        new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex-50, panelstarty,50, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx+panelsizex-50, panelstarty, 50, 38, LocalMapModeClosePopup, "x"),

                        new GuiObject(GuiObjectState.PanelDark,panelstartx+panelsizex-224-88+24+28,panelstarty+37+24+26,225,225,null),

                        new GuiObject(GuiObjectState.Layer,panelstartx+28,panelstarty+37+24+50,200,30,null,LanguageHelper.Popup_FromBase+" "+planet.maps[starttargetbaseitem].name),
                        new GuiObject(GuiObjectState.Layer,panelstartx+28,panelstarty+37+24+90,200,30,null,LanguageHelper.Popup_FightersCount+": 0"),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+42+200,panelstarty+37+24+90,30,30,PlanetMapModeAttackPopupUpDownButton),
                        new GuiObject(GuiObjectState.Layer,panelstartx+28,panelstarty+37+24+130,200,30,null,LanguageHelper.Popup_ToBase+" "+planet.maps[selecttargetbase].name+" [m]"),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+48,panelstarty+37+24+230,200,30,PlanetMapModeAttackPopupLaunchFighters,LanguageHelper.Popup_LaunchFighters),
                        }));
        }
        void PlanetMapMenuOpenAttackMenuWraper(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                planetmode = AttackBaseMode;
                selectedbaseposition_x = -1;
                selectedbaseposition_y = -1;
                selectedbaseposition2_x = -1;
                selectedbaseposition2_y = -1;
            }
        }
        void PlanetMapModeAttackPopupLaunchFighters(ref GuiObject me) 
        {
            if (me.state.l_click)
            {
                if (!offlineMode)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

                    bw.Write(AttackBase);
                    bw.Write(planet.id);
                    bw.Write((int)planet.maps[starttargetbaseitem].position.X);
                    bw.Write((int)planet.maps[starttargetbaseitem].position.Y);
                    bw.Write((int)planet.maps[selecttargetbase].position.X);
                    bw.Write((int)planet.maps[selecttargetbase].position.Y);
                    bw.Write(countfighters);

                    client.SendData(ms.GetBuffer());

                    bw.Close();
                    ms.Close();
                }
                else
                {
                    int realcountfighters = 0;
                    foreach (Unit u in planet.maps[starttargetbaseitem].units)
                        if (u.type == Unit.AttackDrone && u.command.message == commands.gotoparking)
                            realcountfighters++;
                    countfighters = Math.Min(countfighters, realcountfighters);

                    int k = 0;
                    Vector2 target = planet.maps[starttargetbaseitem].GetRandomBaseDirection();
                    foreach (Unit u in planet.maps[starttargetbaseitem].units)
                        if (u.type == Unit.AttackDrone && u.command.message == commands.gotoparking)
                        {
                            u.command = new Command(commands.goaway, (int)target.X, (int)target.Y);
                            k++;
                            if (k >= countfighters) break;
                        }

                    UnitGroup ug = new UnitGroup();
                    ug.player_id = player_id;
                    ug.position = planet.maps[starttargetbaseitem].position;
                    ug.baseposiitionx = (int)planet.maps[selecttargetbase].position.X;
                    ug.baseposiitiony = (int)planet.maps[selecttargetbase].position.Y;
                    for (k = 0; k < countfighters; k++)
                    {
                        Unit u = new Unit(Unit.AttackDrone, target.X, target.Y);
                        u.player_id = player_id;
                        u.command = new Command(commands.attackmode, 0);
                        u.motherbasex = (short)planet.maps[starttargetbaseitem].position.X;
                        u.motherbasey = (short)planet.maps[starttargetbaseitem].position.Y;
                        ug.units.Add(u);
                    }
                    planet.unitgroups.Add(ug);
                }

                gui.ClosePopup();
            }
        }
        void PlanetMapModeAttackPopupUpDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                float dx = camera.mouse.X - me.rect.X;
                float dy = camera.mouse.Y - me.rect.Y;

                if (dx > dy)
                {
                    countfighters++;
                }
                else
                {
                    countfighters--;
                    if (countfighters < 0)
                        countfighters = 0;
                }

                gui.popupform.elements[7].text[0] = LanguageHelper.Popup_FightersCount + ": " + countfighters;
            }
        }
        void SelectTargetBase(int id)
        {
            selecttargetbase = id;
            if (selecttargetbase < planet.maps.Count)
            {
                CreateMinimap(planet, planet.maps[selecttargetbase], ref minimaprocket);

                if (offlineMode)
                {
                    bool[] buildings = planet.maps[selecttargetbase].UnpackedBuildingsShadows(planet.maps[selecttargetbase].PacketBuildingsShadows());
                    Color[] colors = new Color[64 * 64];
                    minimaprocket.GetData<Color>(colors);
                    for (int i = 0; i < 64 * 64; i++) if (buildings[i]) colors[i] = new Color(67, 73, 69);
                    minimaprocket.SetData<Color>(colors);
                }
                else
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

                    bw.Write(LocalMapBuildingShadows);
                    bw.Write((int)planet.id);
                    bw.Write((int)planet.maps[selecttargetbase].position.X);
                    bw.Write((int)planet.maps[selecttargetbase].position.Y);

                    client.SendData(ms.GetBuffer());

                    bw.Close();
                    ms.Close();
                }
            }
        }

        void PlanetMapMenuOpenMarketMenu()
        {
            tradegrouptypes = new int[2, 8];
            tradegroupcount = new int[2, 8];
            for (int i = 0; i < 8; i++) { tradegrouptypes[0, i] = -1; tradegrouptypes[1, i] = -1; }

            gui.popupformid = 5;

            int panelsizex = 557 + 16 + 17;
            int panelstartx = (width - panelsizex) / 2;
            int panelstarty = (height - 392) / 2;

            gui.AddPopup( new Form(0, new GuiObject[] { 
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty + 37, panelsizex, 325, null) ,
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty, panelsizex-120, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx, panelstarty, panelsizex-120, 38, null, LanguageHelper.Popup_SendPopup),
                        new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex-50, panelstarty,50, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx+panelsizex-50, panelstarty, 50, 38, LocalMapModeClosePopup, "x"),

                        //5
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+31,100,30,null,LanguageHelper.Popup_Resource+"2"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+62,100,30,null,LanguageHelper.Popup_Resource+"3"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+93,100,30,null,LanguageHelper.Popup_Resource+"4"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+124,100,30,null,LanguageHelper.Popup_Resource+"5"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+155,100,30,null,LanguageHelper.Popup_Resource+"6"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+186,100,30,null,LanguageHelper.Popup_Resource+"7"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+217,100,30,null,LanguageHelper.Popup_Resource+"8"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+24+248,100,30,null,LanguageHelper.Popup_Resource+"9"),

                        //13
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+31,30,30,LocalMapModeMarketPopupTypeButton1Base1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+62,30,30,LocalMapModeMarketPopupTypeButton2Base1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+93,30,30,LocalMapModeMarketPopupTypeButton3Base1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+124,30,30,LocalMapModeMarketPopupTypeButton4Base1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+155,30,30,LocalMapModeMarketPopupTypeButton5Base1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+186,30,30,LocalMapModeMarketPopupTypeButton6Base1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+217,30,30,LocalMapModeMarketPopupTypeButton7Base1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+248,30,30,LocalMapModeMarketPopupTypeButton8Base1),

                        //22
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex/2-30-24,panelstarty+37+24+31,30,30,LocalMapModeMarketPopupValueButton1Base1),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex/2-30-24,panelstarty+37+24+62,30,30,LocalMapModeMarketPopupValueButton2Base1),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex/2-30-24,panelstarty+37+24+93,30,30,LocalMapModeMarketPopupValueButton3Base1),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex/2-30-24,panelstarty+37+24+124,30,30,LocalMapModeMarketPopupValueButton4Base1),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex/2-30-24,panelstarty+37+24+155,30,30,LocalMapModeMarketPopupValueButton5Base1),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex/2-30-24,panelstarty+37+24+186,30,30,LocalMapModeMarketPopupValueButton6Base1),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex/2-30-24,panelstarty+37+24+217,30,30,LocalMapModeMarketPopupValueButton7Base1),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex/2-30-24,panelstarty+37+24+248,30,30,LocalMapModeMarketPopupValueButton8Base1),

                        //30
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+24+31,100,30,null,LanguageHelper.Popup_Resource+"2"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+24+62,100,30,null,LanguageHelper.Popup_Resource+"3"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+24+93,100,30,null,LanguageHelper.Popup_Resource+"4"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+24+124,100,30,null,LanguageHelper.Popup_Resource+"5"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+24+155,100,30,null,LanguageHelper.Popup_Resource+"6"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+24+186,100,30,null,LanguageHelper.Popup_Resource+"7"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+24+217,100,30,null,LanguageHelper.Popup_Resource+"8"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+24+248,100,30,null,LanguageHelper.Popup_Resource+"9"),

                        //38
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24+panelsizex/2,panelstarty+37+24+31,30,30,LocalMapModeMarketPopupTypeButton1Base2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24+panelsizex/2,panelstarty+37+24+62,30,30,LocalMapModeMarketPopupTypeButton2Base2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24+panelsizex/2,panelstarty+37+24+93,30,30,LocalMapModeMarketPopupTypeButton3Base2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24+panelsizex/2,panelstarty+37+24+124,30,30,LocalMapModeMarketPopupTypeButton4Base2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24+panelsizex/2,panelstarty+37+24+155,30,30,LocalMapModeMarketPopupTypeButton5Base2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24+panelsizex/2,panelstarty+37+24+186,30,30,LocalMapModeMarketPopupTypeButton6Base2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24+panelsizex/2,panelstarty+37+24+217,30,30,LocalMapModeMarketPopupTypeButton7Base2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24+panelsizex/2,panelstarty+37+24+248,30,30,LocalMapModeMarketPopupTypeButton8Base2),

                        //46
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex-30-24,panelstarty+37+24+31,30,30,LocalMapModeMarketPopupValueButton1Base2),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex-30-24,panelstarty+37+24+62,30,30,LocalMapModeMarketPopupValueButton2Base2),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex-30-24,panelstarty+37+24+93,30,30,LocalMapModeMarketPopupValueButton3Base2),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex-30-24,panelstarty+37+24+124,30,30,LocalMapModeMarketPopupValueButton4Base2),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex-30-24,panelstarty+37+24+155,30,30,LocalMapModeMarketPopupValueButton5Base2),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex-30-24,panelstarty+37+24+186,30,30,LocalMapModeMarketPopupValueButton6Base2),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex-30-24,panelstarty+37+24+217,30,30,LocalMapModeMarketPopupValueButton7Base2),
                        new GuiObject(GuiObjectState.ButtonUpDown,panelstartx+panelsizex-30-24,panelstarty+37+24+248,30,30,LocalMapModeMarketPopupValueButton8Base2),

                        //54
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30,panelstarty+37+14,200,30,null,LanguageHelper.Popup_Base+" 1"),
                        new GuiObject(GuiObjectState.Layer,panelstartx+24+8+30+panelsizex/2,panelstarty+37+14,200,30,null,LanguageHelper.Popup_Base+" 2"),
                        new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex/2-150-10,panelstarty+37+14+298-10+40,320,50, null),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+panelsizex/2-150,panelstarty+37+14+298+40,300,30,LocalMapModeMarketPopupSend,LanguageHelper.Popup_Send)
                        }));
        }
        void PlanetMapMenuOpenMarketMenuWraper(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                planetmode = MarketBaseMode;
                selectedbaseposition_x = -1;
                selectedbaseposition_y = -1;
                selectedbaseposition2_x = -1;
                selectedbaseposition2_y = -1;
            }
        }

        void LocalMapModeMarketPopupSend(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                int base1_id = planet.GetBaseId(selectedbaseposition_x, selectedbaseposition_y);
                int base2_id = planet.GetBaseId(selectedbaseposition2_x, selectedbaseposition2_y);
                if (base1_id >= 0 && base2_id >= 0)
                {
                    Vector2 pos = map.GetRandomBaseDirection();
                    List<UnitGroup> uglist = new List<UnitGroup>();

                    //1
                    if (planet.maps[base1_id].player_id == player_id)
                    for (int w = 0; w < 8; w++)
                        if (tradegrouptypes[0, w] >= 0)
                        {
                            if (planet.maps[base1_id].isBuildingBuilded(Building.Exchanger) && planet.maps[base1_id].isBuildingBuilded(Building.Spaceport))
                            {
                                for (int j = 0; j < planet.maps[base1_id].buildings.Count; j++)
                                {
                                    if (planet.maps[base1_id].buildings[j].type == Building.Parking && planet.maps[base1_id].buildings[j].buildingtime <= 0)
                                    {
                                        Unit u = new Unit(Unit.LocalMerchant, pos.X, pos.Y);
                                        u.tar = planet.maps[base2_id].buildings[j].pos;
                                        u.command = new Command(commands.localtradergoin, j);
                                        u.height = 3;

                                        u.CreateInventory();
                                        for (int q = 0; q < 8; q++)
                                            if (tradegrouptypes[0, q] >= 0)
                                            {
                                                float count = Math.Min(planet.maps[base1_id].inventory[tradegrouptypes[0, q]].count, tradegroupcount[0, q]);
                                                u.inventory[tradegrouptypes[0, q]] += count;
                                                planet.maps[base1_id].inventory[tradegrouptypes[0, q]].count -= count;
                                            }

                                        UnitGroup ug = new UnitGroup();
                                        ug.player_id = player_id;
                                        ug.position = planet.maps[base1_id].position;
                                        ug.units.Add(u);

                                        ug.baseposiitionx = (int)planet.maps[base2_id].position.X;
                                        ug.baseposiitiony = (int)planet.maps[base2_id].position.Y;

                                        planet.unitgroups.Add(ug);

                                        uglist.Add(ug);

                                        break;
                                    }
                                }
                            }
                            break;
                        }

                    //2
                    if (planet.maps[base2_id].player_id == player_id)
                    for (int w = 0; w < 8; w++)
                        if (tradegrouptypes[1, w] >= 0)
                        {
                            if (planet.maps[base2_id].isBuildingBuilded(Building.Exchanger) && planet.maps[base2_id].isBuildingBuilded(Building.Spaceport))
                            {
                                for (int j = 0; j < planet.maps[base2_id].buildings.Count; j++)
                                {
                                    if (planet.maps[base2_id].buildings[j].type == Building.Parking && planet.maps[base2_id].buildings[j].buildingtime <= 0)
                                    {
                                        Unit u = new Unit(Unit.LocalMerchant, pos.X, pos.Y);
                                        u.tar = planet.maps[base1_id].buildings[j].pos;
                                        u.command = new Command(commands.localtradergoin, j);
                                        u.height = 3;

                                        u.CreateInventory();
                                        for (int q = 0; q < 8; q++)
                                            if (tradegrouptypes[1, q] >= 0)
                                            {
                                                float count = Math.Min(planet.maps[base2_id].inventory[tradegrouptypes[1, q]].count, tradegroupcount[1, q]);
                                                u.inventory[tradegrouptypes[1, q]] += count;
                                                planet.maps[base2_id].inventory[tradegrouptypes[1, q]].count -= count;
                                            }

                                        UnitGroup ug = new UnitGroup();
                                        ug.player_id = player_id;
                                        ug.position = planet.maps[base2_id].position;
                                        ug.units.Add(u);

                                        ug.baseposiitionx = (int)planet.maps[base1_id].position.X;
                                        ug.baseposiitiony = (int)planet.maps[base1_id].position.Y;

                                        planet.unitgroups.Add(ug);

                                        uglist.Add(ug);

                                        break;
                                    }
                                }
                            }

                            break;
                        }

                    if (!offlineMode && uglist.Count > 0)
                    {
                        System.IO.MemoryStream mem = new System.IO.MemoryStream();
                        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                        bw.Write(LocalMapAddUnitGroup);
                        bw.Write(selectedplanet);
                        bw.Write(uglist.Count);

                        foreach (UnitGroup ug in uglist)
                            bw.Write(ug.PackedData());

                        byte[] membuf = mem.GetBuffer();
                        byte[] retbuf = new byte[bw.BaseStream.Position];
                        Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                        bw.Close();
                        mem.Close();

                        client.SendData(retbuf);
                    }

                    LocalMapModeClosePopup(ref me);

                    selectedbaseposition_x = -1;
                    selectedbaseposition_y = -1;
                    selectedbaseposition2_x = -1;
                    selectedbaseposition2_y = -1;
                }
            }
        }

        void UpdateMarketExchangeType(int b,int id,int value)
        {
            selectedmarketbase = b;
            selectedmarketid = id;

            int panelsizex = resourceset.Width * 2;
            int panelsizey = resourceset.Height * 2;
            gui.popupformid = 6;
            gui.AddPopup(new Form(0, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark, (width - panelsizex-48) / 2, (height - panelsizey-48) / 2, panelsizex+48, panelsizey+48+32+8, null) ,
                                                       new GuiObject(GuiObjectState.PanelDark, (width - panelsizex) / 2, (height - panelsizey) / 2, panelsizex, panelsizey, LocalMapModeMarketPopupSelectResource),
                                                       new GuiObject(GuiObjectState.Button, (width - panelsizex) / 2+panelsizex/2-16, (height - panelsizey) / 2+panelsizey+8, 32, 32, LocalMapModeMarketPopupSelectResourceNull,"x")}));
        }
        void LocalMapModeMarketPopupSelectResource(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                int x = ((int)camera.mouse.X - me.rect.X)/32;
                int y = ((int)camera.mouse.Y - me.rect.Y)/32;

                tradegrouptypes[selectedmarketbase, selectedmarketid] = y * 8 + x;

                gui.ClosePopup();
                gui.popupformid = 5;
            }

            if (me.state.r_click)
            {
                gui.ClosePopup();
                gui.popupformid = 5;
            }
            
        }
        void LocalMapModeMarketPopupSelectResourceNull(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                tradegrouptypes[selectedmarketbase, selectedmarketid] = -1;

                gui.ClosePopup();
                gui.popupformid = 5;
            }
        }
        void UpdateMarketExchangeValue(int b, int id, Rectangle rect)
        {
            float dx = camera.mouse.X - rect.X;
            float dy = camera.mouse.Y - rect.Y;

            if (dx > dy) tradegroupcount[b, id] += 10;
            else tradegroupcount[b, id] -= 10;

            if (tradegroupcount[b, id] < 0) tradegroupcount[b, id] = 0;
        }
        void LocalMapModeMarketPopupTypeButton1Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(0, 0, tradegrouptypes[0, 0] == Map.maxresources - 1 ? -1 : tradegrouptypes[0, 0] + 1); }
        void LocalMapModeMarketPopupTypeButton2Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(0, 1, tradegrouptypes[0, 1] == Map.maxresources - 1 ? -1 : tradegrouptypes[0, 1] + 1); }
        void LocalMapModeMarketPopupTypeButton3Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(0, 2, tradegrouptypes[0, 2] == Map.maxresources - 1 ? -1 : tradegrouptypes[0, 2] + 1); }
        void LocalMapModeMarketPopupTypeButton4Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(0, 3, tradegrouptypes[0, 3] == Map.maxresources - 1 ? -1 : tradegrouptypes[0, 3] + 1); }
        void LocalMapModeMarketPopupTypeButton5Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(0, 4, tradegrouptypes[0, 4] == Map.maxresources - 1 ? -1 : tradegrouptypes[0, 4] + 1); }
        void LocalMapModeMarketPopupTypeButton6Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(0, 5, tradegrouptypes[0, 5] == Map.maxresources - 1 ? -1 : tradegrouptypes[0, 5] + 1); }
        void LocalMapModeMarketPopupTypeButton7Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(0, 6, tradegrouptypes[0, 6] == Map.maxresources - 1 ? -1 : tradegrouptypes[0, 6] + 1); }
        void LocalMapModeMarketPopupTypeButton8Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(0, 7, tradegrouptypes[0, 7] == Map.maxresources - 1 ? -1 : tradegrouptypes[0, 7] + 1); }

        void LocalMapModeMarketPopupTypeButton1Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(1, 0, tradegrouptypes[1, 0] == Map.maxresources - 1 ? -1 : tradegrouptypes[1, 0] + 1); }
        void LocalMapModeMarketPopupTypeButton2Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(1, 1, tradegrouptypes[1, 1] == Map.maxresources - 1 ? -1 : tradegrouptypes[1, 1] + 1); }
        void LocalMapModeMarketPopupTypeButton3Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(1, 2, tradegrouptypes[1, 2] == Map.maxresources - 1 ? -1 : tradegrouptypes[1, 2] + 1); }
        void LocalMapModeMarketPopupTypeButton4Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(1, 3, tradegrouptypes[1, 3] == Map.maxresources - 1 ? -1 : tradegrouptypes[1, 3] + 1); }
        void LocalMapModeMarketPopupTypeButton5Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(1, 4, tradegrouptypes[1, 4] == Map.maxresources - 1 ? -1 : tradegrouptypes[1, 4] + 1); }
        void LocalMapModeMarketPopupTypeButton6Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(1, 5, tradegrouptypes[1, 5] == Map.maxresources - 1 ? -1 : tradegrouptypes[1, 5] + 1); }
        void LocalMapModeMarketPopupTypeButton7Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(1, 6, tradegrouptypes[1, 6] == Map.maxresources - 1 ? -1 : tradegrouptypes[1, 6] + 1); }
        void LocalMapModeMarketPopupTypeButton8Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeType(1, 7, tradegrouptypes[1, 7] == Map.maxresources - 1 ? -1 : tradegrouptypes[1, 7] + 1); }

        void LocalMapModeMarketPopupValueButton1Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(0, 0, me.rect); }
        void LocalMapModeMarketPopupValueButton2Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(0, 1, me.rect); }
        void LocalMapModeMarketPopupValueButton3Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(0, 2, me.rect); }
        void LocalMapModeMarketPopupValueButton4Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(0, 3, me.rect); }
        void LocalMapModeMarketPopupValueButton5Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(0, 4, me.rect); }
        void LocalMapModeMarketPopupValueButton6Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(0, 5, me.rect); }
        void LocalMapModeMarketPopupValueButton7Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(0, 6, me.rect); }
        void LocalMapModeMarketPopupValueButton8Base1(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(0, 7, me.rect); }

        void LocalMapModeMarketPopupValueButton1Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(1, 0, me.rect); }
        void LocalMapModeMarketPopupValueButton2Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(1, 1, me.rect); }
        void LocalMapModeMarketPopupValueButton3Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(1, 2, me.rect); }
        void LocalMapModeMarketPopupValueButton4Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(1, 3, me.rect); }
        void LocalMapModeMarketPopupValueButton5Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(1, 4, me.rect); }
        void LocalMapModeMarketPopupValueButton6Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(1, 5, me.rect); }
        void LocalMapModeMarketPopupValueButton7Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(1, 6, me.rect); }
        void LocalMapModeMarketPopupValueButton8Base2(ref GuiObject me) { if (me.state.l_click)UpdateMarketExchangeValue(1, 7, me.rect); }

        void OpenLocalMapModeInfoPopup()
        {
            gui.popupformid = 7;

            int panelsizex = 557 + 16 + 17;
            int panelstartx = (width - panelsizex - 200) / 2;
            int panelstarty = (height - 392) / 2;

            gui.AddPopup(new Form(0, new GuiObject[] { 
                         new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty + 37, panelsizex, 325, null) ,
                         new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty, panelsizex-120, 38, null),
                         new GuiObject(GuiObjectState.Layer, panelstartx, panelstarty, panelsizex-120, 38, null, LanguageHelper.Popup_InfoPopup),
                         new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex-50, panelstarty,50, 38, null),
                         new GuiObject(GuiObjectState.Layer, panelstartx+panelsizex-50, panelstarty, 50, 38, LocalMapModeClosePopup, "x"),
                         new GuiObject(GuiObjectState.PanelDark, panelstartx + 30, panelstarty + 30+37, 192+16, 325-60, null),
                         new GuiObject(GuiObjectState.Layer, panelstartx + 268, panelstarty + 30+37, panelsizex-298, 30, null,LanguageHelper.Popup_Name2+": "),
                         new GuiObject(GuiObjectState.Layer, panelstartx + 268, panelstarty + 30+37+30, panelsizex-298, 30, null,LanguageHelper.Popup_Type+": "),
                         new GuiObject(GuiObjectState.Layer, panelstartx + 268, panelstarty + 30+37+60, panelsizex-298, 30, null,LanguageHelper.Popup_EnergyConsum+": "),}));

            GuiObject[] newobjects = null;

            switch (map.buildings[selectedbuildid].type)
            {
                case Building.ClosedFarm:
                case Building.Farm:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268+25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2-25, 30, null, LanguageHelper.Popup_Prodused+": "),
                                                   new GuiObject(GuiObjectState.ButtonUpDown, panelstartx + 268+230, panelstarty + 30 + 37 + 160, 30, 30, LocaMapModeInfoPopupFarmUpDownButton)};
                    break;
                case Building.Generator:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Prodused + ": ") ,
                                 new GuiObject(GuiObjectState.MenuButton,panelstartx + 268 + 25, panelstarty + 80 + 37 + 160, 241, 30,LocalMapChangeEnergyBoost,LanguageHelper.Gui_UseEnergyOre+": "+(map!=null&&map.energyboost?LanguageHelper.Gui_on:LanguageHelper.Gui_off))};
                    break;
                case Building.Laboratory:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.MenuButton, panelstartx + 268 + 25, panelstarty + 80 + 37 + 160, 241, 30, LocalMapChangeScienceBoost, LanguageHelper.Gui_UseScienceBoost + ": " + (map != null && map.scienceboost ? LanguageHelper.Gui_on : LanguageHelper.Gui_off)) };
                    break;
                case Building.Warehouse:
                case Building.House:
                case Building.EnergyBank:
                case Building.InfoStorage:
                case Building.LuquidStorage:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_StorageFulling+": ") };
                    break;
                case Building.DroidFactory:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Wait+": ") ,
                                 new GuiObject(GuiObjectState.ButtonUpDown, panelstartx + 268+230, panelstarty + 30 + 37 + 160, 30, 30, LocaMapModeInfoPopupListUpDownButton),
                                 new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160+30, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_ComplateAfter+": "),
                                 new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160+60, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Price+": ")};
                    break;
                case Building.ProcessingFactory:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268+25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2-25, 30, null,  LanguageHelper.Popup_Prodused+": "),
                                                   new GuiObject(GuiObjectState.ButtonUpDown, panelstartx + 268+230, panelstarty + 30 + 37 + 160, 30, 30, LocaMapModeInfoPopupFabricUpDownButton),
                                                   new GuiObject(GuiObjectState.Layer, panelstartx + 268+25, panelstarty + 30 + 37 + 160+30, (panelsizex - 298) / 2-25, 30, null, LanguageHelper.Popup_Need+": ")};
                    break;
                case Building.Mine:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Mining2+": ") };
                    break;
                case Building.Dirrick:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Mining2+": ") };
                    break;
                case Building.Collector:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268+25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2-25, 30, null, LanguageHelper.Popup_Collect+": "),
                                                   new GuiObject(GuiObjectState.ButtonUpDown, panelstartx + 268+230, panelstarty + 30 + 37 + 160, 30, 30, LocaMapModeInfoPopupCollectorUpDownButton)};
                    break;
                case Building.BuildCenter:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Speedup + ": ") };
                    break;
                case Building.LinksCenter:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Speedup + ": ") };
                    break;
                case Building.StorageCenter:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Speedup + ": ") };
                    break;
                case Building.ScienceCenter:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Speedup + ": ") };
                    break;
                case Building.CommandCenter:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Speedup + ": ") };
                    break;
                case Building.AttackFactory:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Wait+": ") ,
                                 new GuiObject(GuiObjectState.ButtonUpDown, panelstartx + 268+230, panelstarty + 30 + 37 + 160, 30, 30, LocaMapModeInfoPopupListUpDownButton),
                                 new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160+30, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_ComplateAfter+": "),
                                 new GuiObject(GuiObjectState.Layer, panelstartx + 268 + 25, panelstarty + 30 + 37 + 160+60, (panelsizex - 298) / 2 - 25, 30, null, LanguageHelper.Popup_Price+": ")};
                    break;
                case Building.RocketParking:
                    newobjects = new GuiObject[] { new GuiObject(GuiObjectState.Layer, panelstartx + 268+25, panelstarty + 140, (panelsizex - 298) / 2-25, 30, null, LanguageHelper.Popup_CurrentRocket+": "),
                                                   new GuiObject(GuiObjectState.Layer, panelstartx + 268+25, panelstarty + 30 + 140, (panelsizex - 298) / 2-25, 30, null, LanguageHelper.Popup_Wait+": "),
                                                   new GuiObject(GuiObjectState.MenuButton, panelstartx + 268 + 25, panelstarty + 70 + 140, 241, 30, LocaMapModeInfoPopupRocketLaunch,LanguageHelper.Popup_LaunchRocket),
                                                   new GuiObject(GuiObjectState.MenuButton, panelstartx + 268 + 25, panelstarty + 70 + 140+96, 241, 30, LocaMapModeInfoPopupRocketTwined,LanguageHelper.Popup_CurrentRocketTwined),
                                                   new GuiObject(GuiObjectState.MenuButton, panelstartx + 268 + 25, panelstarty + 70 + 140+32, 241, 30, LocaMapModeInfoPopupRocketAtom,LanguageHelper.Popup_CurrentRocketAtom),
                                                   new GuiObject(GuiObjectState.MenuButton, panelstartx + 268 + 25, panelstarty + 70 + 140+64, 241, 30, LocaMapModeInfoPopupRocketNeitron,LanguageHelper.Popup_CurrentRocketNeitron),};
                    break;
            }

            if (newobjects != null)
            {
                GuiObject[] tmp = new GuiObject[newobjects.Length + gui.popupform.elements.Length];

                Array.Copy(gui.popupform.elements, tmp, gui.popupform.elements.Length);
                Array.Copy(newobjects, 0, tmp, gui.popupform.elements.Length, newobjects.Length);
                gui.popupform.elements = tmp;
            }

            GuiObject[] tmp2 = new GuiObject[gui.popupform.elements.Length + 1];
            GuiObject[] tobj = new GuiObject[1] { new GuiObject(GuiObjectState.MenuButton, panelstartx + 30 + 192 + 16 - 30 - 4, panelstarty + 30 + 37 + 4, 30, 30, LocalMapModeUpgradeBuilding, "Up"), };
            tobj[0].enable = map != null && map.maxbuildinglvl > 1;

            Array.Copy(gui.popupform.elements, tmp2, gui.popupform.elements.Length);
            Array.Copy(tobj, 0, tmp2, gui.popupform.elements.Length, 1);

            gui.popupform.elements = tmp2;

        }
        void LocalMapChangeEnergyBoost(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (map != null)
                {
                    map.energyboost = !map.energyboost;
                    me.text[0] = LanguageHelper.Gui_UseEnergyOre + ": " + (map != null && map.energyboost ? LanguageHelper.Gui_on : LanguageHelper.Gui_off);
                }
            }
        }

        void LocalMapChangeScienceBoost(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (map != null)
                {
                    map.scienceboost = !map.scienceboost;
                    me.text[0] = LanguageHelper.Gui_UseScienceBoost + ": " + (map != null && map.scienceboost ? LanguageHelper.Gui_on : LanguageHelper.Gui_off);
                }
            }
        }
        void LocalMapModeUpgradeBuilding(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (map != null && map.buildings.Count > selectedbuildid &&map.buildings[selectedbuildid].lvl<map.maxbuildinglvl&& map.inventory[(int)Resources.credits].count >= Building.GetBuildPrice(map.buildings[selectedbuildid].type)/4)
                {
                    map.buildings[selectedbuildid].lvl++;
                    map.buildings[selectedbuildid].buildingtime += Building.GetBuildTime(map.buildings[selectedbuildid].type)/3;
                    map.buildings[selectedbuildid].maxbuildingtime += Building.GetBuildTime(map.buildings[selectedbuildid].type)/3;
                    map.inventory[(int)Resources.credits].count -= Building.GetBuildPrice(map.buildings[selectedbuildid].type) / 4;
                }
            }
        }

        void LocaMapModeInfoPopupFarmUpDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                float dx = camera.mouse.X - me.rect.X;
                float dy = camera.mouse.Y - me.rect.Y;

                if (dx > dy)
                {
                    map.buildings[selectedbuildid].recipte++;
                    if (map.buildings[selectedbuildid].type == Building.Farm && map.buildings[selectedbuildid].recipte == 1) map.buildings[selectedbuildid].recipte++;
                    if (map.buildings[selectedbuildid].recipte > 3) map.buildings[selectedbuildid].recipte %= 4;
                }
                else
                {
                    map.buildings[selectedbuildid].recipte--;
                    if (map.buildings[selectedbuildid].type == Building.Farm && map.buildings[selectedbuildid].recipte == 1) map.buildings[selectedbuildid].recipte--;
                    if (map.buildings[selectedbuildid].recipte < 0) map.buildings[selectedbuildid].recipte += 4;
                }

                if (!offlineMode)
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                    bw.Write(LocalMapChangeInfoRecipte);
                    bw.Write(selectedplanet);
                    bw.Write((int)map.position.X);
                    bw.Write((int)map.position.Y);
                    bw.Write((int)map.buildings[selectedbuildid].pos.X);
                    bw.Write((int)map.buildings[selectedbuildid].pos.Y);
                    bw.Write(map.buildings[selectedbuildid].recipte);

                    byte[] membuf = mem.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem.Close();

                    client.SendData(retbuf);
                }
            }
        }
        void LocaMapModeInfoPopupListUpDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                float dx = camera.mouse.X - me.rect.X;
                float dy = camera.mouse.Y - me.rect.Y;

                if (dx > dy)
                {
                    map.buildings[selectedbuildid].workcount++;
                }
                else if (map.buildings[selectedbuildid].workcount > 0)
                {
                    map.buildings[selectedbuildid].workcount--;
                }

                if (!offlineMode)
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                    bw.Write(LocalMapChangeInfoWorkcount);
                    bw.Write(selectedplanet);
                    bw.Write((int)map.position.X);
                    bw.Write((int)map.position.Y);
                    bw.Write((int)map.buildings[selectedbuildid].pos.X);
                    bw.Write((int)map.buildings[selectedbuildid].pos.Y);
                    bw.Write(map.buildings[selectedbuildid].workcount);

                    byte[] membuf = mem.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem.Close();

                    client.SendData(retbuf);
                }
            }
        }
        void LocaMapModeInfoPopupFabricUpDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                float dx = camera.mouse.X - me.rect.X;
                float dy = camera.mouse.Y - me.rect.Y;

                if (dx > dy)
                {
                    map.buildings[selectedbuildid].recipte++;
                    if (map.buildings[selectedbuildid].recipte >= MapHelper_Engine.resiptes.Length)
                        map.buildings[selectedbuildid].recipte = 0;
                }
                else
                {
                    map.buildings[selectedbuildid].recipte--;
                    if (map.buildings[selectedbuildid].recipte < 0)
                        map.buildings[selectedbuildid].recipte = MapHelper_Engine.resiptes.Length - 1;
                }

                if (!offlineMode)
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                    bw.Write(LocalMapChangeInfoRecipte);
                    bw.Write(selectedplanet);
                    bw.Write((int)map.position.X);
                    bw.Write((int)map.position.Y);
                    bw.Write((int)map.buildings[selectedbuildid].pos.X);
                    bw.Write((int)map.buildings[selectedbuildid].pos.Y);
                    bw.Write(map.buildings[selectedbuildid].recipte);

                    byte[] membuf = mem.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem.Close();

                    client.SendData(retbuf);
                }
            }
        }
        void LocaMapModeInfoPopupCollectorUpDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                map.buildings[selectedbuildid].recipte++;
                map.buildings[selectedbuildid].recipte %= 2;

                if (!offlineMode)
                {
                    System.IO.MemoryStream mem = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);

                    bw.Write(LocalMapChangeInfoRecipte);
                    bw.Write(selectedplanet);
                    bw.Write((int)map.position.X);
                    bw.Write((int)map.position.Y);
                    bw.Write((int)map.buildings[selectedbuildid].pos.X);
                    bw.Write((int)map.buildings[selectedbuildid].pos.Y);
                    bw.Write(map.buildings[selectedbuildid].recipte);

                    byte[] membuf = mem.GetBuffer();
                    byte[] retbuf = new byte[bw.BaseStream.Position];
                    Array.Copy(membuf, retbuf, bw.BaseStream.Position);

                    bw.Close();
                    mem.Close();

                    client.SendData(retbuf);
                }
            }
        }
        void LocaMapModeInfoPopupRocketLaunch(ref GuiObject me)
        {
            if (me.state.l_click) 
            {
                gui.popupformid = 24;

                int panelsizex = 557 + 16 + 17;
                int panelstartx = (width - 200 - panelsizex) / 2;
                int panelstarty = (height - 392) / 2;

                gui.ClosePopup();
                gui.AddPopup( new Form(0, new GuiObject[] { 
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty + 37, panelsizex, 325, null) ,
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty, panelsizex-120, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx, panelstarty, panelsizex-120, 38, null, LanguageHelper.Popup_LaunchRocket),
                        new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex-50, panelstarty,50, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx+panelsizex-50, panelstarty, 50, 38, LocalMapModeClosePopup, "x"),

                        new GuiObject(GuiObjectState.Button,panelstartx+551,panelstarty+37+24,30,30,LocalMapModeLaunchRocketUpButton),
                        new GuiObject(GuiObjectState.Button,panelstartx+551,panelstarty+37+24+325-48-30,30,30,LocalMapModeLaunchRocketDownButton),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+551,panelstarty+37+24+38,30,217-8-8,LocalMapModeLaunchRocketSlider),

                        new GuiObject(GuiObjectState.PanelDark,panelstartx+panelsizex-224-88+24,panelstarty+37+24+26-15,225,225,LocalMapModeRocketPopupSetPosition),

                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24,254,30,LocalMapModeRocketPopupSelectButton1),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+31,254,30,LocalMapModeRocketPopupSelectButton2),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+62,254,30,LocalMapModeRocketPopupSelectButton3),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+93,254,30,LocalMapModeRocketPopupSelectButton4),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+124,254,30,LocalMapModeRocketPopupSelectButton5),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+155,254,30,LocalMapModeRocketPopupSelectButton6),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+186,254,30,LocalMapModeRocketPopupSelectButton7),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+217,254,30,LocalMapModeRocketPopupSelectButton8),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+24,panelstarty+37+24+248,254,30,LocalMapModeRocketPopupSelectButton9),

                        new GuiObject(GuiObjectState.MenuButton,panelstartx+panelsizex-224-88+24+50,panelstarty+37+24+26-30+225+30,125,30,LocalMapModeRocketPopupLaunchRocket,LanguageHelper.Popup_LaunchRocket),
                        }));

                SelectRocketBase(0);
            }
        }

        void LocalMapModeLaunchRocketUpButton(ref GuiObject me)
        {
            if (me.state.l_click)
                if (startlaunchrocketitem > 0) startlaunchrocketitem--;
        }
        void LocalMapModeLaunchRocketDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
                if (startlaunchrocketitem < planet.maps.Count - 9) startlaunchrocketitem++;
        }
        void LocalMapModeLaunchRocketSlider(ref GuiObject me)
        {
            if (me.state.l_click && planet.maps.Count > 9)
            {
                float pos = camera.mouse.Y - me.rect.Y;
                startlaunchrocketitem = (int)pos * planet.maps.Count / me.rect.Height;

                if (startlaunchrocketitem > planet.maps.Count - 9) startlaunchrocketitem = planet.maps.Count - 9;
                if (startlaunchrocketitem < 0) startlaunchrocketitem = 0;
            }

            if (camera.mouseWheel != camera.mouseWheelOld && planet.maps.Count > 9)
            {
                int dw = camera.mouseWheelOld - camera.mouseWheel;
                startlaunchrocketitem += dw / 30;
                if (startlaunchrocketitem > planet.maps.Count - 9) startlaunchrocketitem = planet.maps.Count - 9;
                if (startlaunchrocketitem < 0) startlaunchrocketitem = 0;
            }
        }
        void LocalMapModeRocketPopupSelectButton1(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem); }
        void LocalMapModeRocketPopupSelectButton2(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem + 1); }
        void LocalMapModeRocketPopupSelectButton3(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem + 2); }
        void LocalMapModeRocketPopupSelectButton4(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem + 3); }
        void LocalMapModeRocketPopupSelectButton5(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem + 4); }
        void LocalMapModeRocketPopupSelectButton6(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem + 5); }
        void LocalMapModeRocketPopupSelectButton7(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem + 6); }
        void LocalMapModeRocketPopupSelectButton8(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem + 7); }
        void LocalMapModeRocketPopupSelectButton9(ref GuiObject me) { if (me.state.l_click)SelectRocketBase(startlaunchrocketitem + 8); }
        void SelectRocketBase(int id)
        {
            selectrocketbase = id;
            launchrocketx = 32;
            launchrockety = 32;
            if (selectrocketbase < planet.maps.Count)
            {
                CreateMinimap(planet, planet.maps[selectrocketbase], ref minimaprocket);

                if (offlineMode)
                {
                    bool[] buildings = planet.maps[selectrocketbase].UnpackedBuildingsShadows(planet.maps[selectrocketbase].PacketBuildingsShadows());
                    Color[] colors = new Color[64 * 64];
                    minimaprocket.GetData<Color>(colors);
                    for (int i = 0; i < 64 * 64; i++) if (buildings[i]) colors[i] = new Color(67, 73, 69);
                    minimaprocket.SetData<Color>(colors);
                }
                else
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

                    bw.Write(LocalMapBuildingShadows);
                    bw.Write((int)planet.id);
                    bw.Write((int)planet.maps[selectrocketbase].position.X);
                    bw.Write((int)planet.maps[selectrocketbase].position.Y);

                    client.SendData(ms.GetBuffer());

                    bw.Close();
                    ms.Close();
                }
            }
        }

        void LocalMapModeRocketPopupLaunchRocket(ref GuiObject me)
        {
            if (me.state.l_click && map.buildings[selectedbuildid].wait<=0)
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
                    Unit r = new Unit(type, launchrocketx, launchrockety);
                    r.height = 100;
                    r.command = new Command(commands.rocketfallingdown, 0);
                    ug.units.Add(r);
                    ug.baseposiitionx = (int)planet.maps[selectrocketbase].position.X;
                    ug.baseposiitiony = (int)planet.maps[selectrocketbase].position.Y;
                    ug.position = map.position;

                    planet.unitgroups.Add(ug);
                    map.units.Add(u);

                    if (!offlineMode)
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

                        bw.Write(LaunchRocket);
                        bw.Write(planet.id);
                        bw.Write((int)map.position.X);
                        bw.Write((int)map.position.Y);
                        bw.Write((int)map.buildings[selectedbuildid].pos.X);
                        bw.Write((int)map.buildings[selectedbuildid].pos.Y);
                        bw.Write((int)planet.maps[selectrocketbase].position.X);
                        bw.Write((int)planet.maps[selectrocketbase].position.Y);
                        bw.Write(launchrocketx);
                        bw.Write(launchrockety);

                        client.SendData(ms.GetBuffer());

                        bw.Close();
                        ms.Close();
                    }
                }
            }
        }
        void LocalMapModeRocketPopupSetPosition(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                launchrocketx = (64*((int)camera.mouse.X - me.rect.X))/me.rect.Width;
                launchrockety = (64*((int)camera.mouse.Y - me.rect.Y))/me.rect.Height;
            }
        }
        void LocaMapModeInfoPopupRocketAtom(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (map.inventory[(int)Resources.electronics].count >= Constants.Map_RocketBuildingElectronicPrice &&
                   map.inventory[(int)Resources.energyore].count >= Constants.Map_RocketBuildingEnergyOrePrice &&
                   map.inventory[(int)Resources.metal].count >= Constants.Map_RocketBuildingMetalPrice)
                {
                    map.inventory[(int)Resources.electronics].count -= Constants.Map_RocketBuildingElectronicPrice;
                    map.inventory[(int)Resources.energyore].count -= Constants.Map_RocketBuildingEnergyOrePrice;
                    map.inventory[(int)Resources.metal].count -= Constants.Map_RocketBuildingMetalPrice;

                    map.buildings[selectedbuildid].recipte = 1;
                    map.buildings[selectedbuildid].wait = Constants.Map_RocketBuildingTime;

                    OnlineSendRocketData();
                }
            }
        }
        void LocaMapModeInfoPopupRocketNeitron(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (map.inventory[(int)Resources.electronics].count >= Constants.Map_RocketBuildingElectronicPrice &&
                   map.inventory[(int)Resources.energyore].count >= Constants.Map_RocketBuildingEnergyOrePrice &&
                   map.inventory[(int)Resources.metal].count >= Constants.Map_RocketBuildingMetalPrice)
                {
                    map.inventory[(int)Resources.electronics].count -= Constants.Map_RocketBuildingElectronicPrice;
                    map.inventory[(int)Resources.energyore].count -= Constants.Map_RocketBuildingEnergyOrePrice;
                    map.inventory[(int)Resources.metal].count -= Constants.Map_RocketBuildingMetalPrice;

                    map.buildings[selectedbuildid].recipte = 2;
                    map.buildings[selectedbuildid].wait = Constants.Map_RocketBuildingTime;

                    OnlineSendRocketData();
                }
            }
        }
        void LocaMapModeInfoPopupRocketTwined(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                if (map.inventory[(int)Resources.electronics].count >= Constants.Map_RocketBuildingElectronicPrice &&
                   map.inventory[(int)Resources.energyore].count >= Constants.Map_RocketBuildingEnergyOrePrice &&
                   map.inventory[(int)Resources.metal].count >= Constants.Map_RocketBuildingMetalPrice)
                {
                    map.inventory[(int)Resources.electronics].count -= Constants.Map_RocketBuildingElectronicPrice;
                    map.inventory[(int)Resources.energyore].count -= Constants.Map_RocketBuildingEnergyOrePrice;
                    map.inventory[(int)Resources.metal].count -= Constants.Map_RocketBuildingMetalPrice;

                    map.buildings[selectedbuildid].recipte = 3;
                    map.buildings[selectedbuildid].wait = Constants.Map_RocketBuildingTime;

                    OnlineSendRocketData();
                }
            }
        }
        void OnlineSendRocketData()
        {
            if (!offlineMode)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);

                bw.Write(CreateRocket);
                bw.Write(selectedplanet);
                bw.Write((int)map.position.X);
                bw.Write((int)map.position.Y);
                bw.Write((int)map.buildings[selectedbuildid].pos.X);
                bw.Write((int)map.buildings[selectedbuildid].pos.Y);
                bw.Write((int)map.buildings[selectedbuildid].recipte);

                client.SendData(ms.GetBuffer());

                bw.Close();
                ms.Close();
            }
        }

        void PlanetMapMenuOpenModulesMenu(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.popupformid = 9;

                int panelsizex = 557 + 16 + 17;
                int panelstartx = (width - panelsizex) / 2;
                int panelstarty = (height - 392) / 2;

                gui.AddPopup( new Form(0, new GuiObject[] { 
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty + 37, panelsizex, 325, null) ,
                        new GuiObject(GuiObjectState.PanelDark, panelstartx, panelstarty, panelsizex-120, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx, panelstarty, panelsizex-120, 38, null, LanguageHelper.Popup_PlanetModulesPopup),
                        new GuiObject(GuiObjectState.PanelDark, panelstartx+panelsizex-50, panelstarty,50, 38, null),
                        new GuiObject(GuiObjectState.Layer, panelstartx+panelsizex-50, panelstarty, 50, 38, LocalMapModeClosePopup, "x"),
            
                        new GuiObject(GuiObjectState.Button,panelstartx+551,panelstarty+37+24,30,30,PlanetMapModeModulesUpButton),
                        new GuiObject(GuiObjectState.Button,panelstartx+551,panelstarty+37+24+325-48-30,30,30,PlanetMapModeModulesDownButton),
                        new GuiObject(GuiObjectState.PanelDark,panelstartx+551,panelstarty+37+24+38,30,217-8-8,PlanetMapModeModulesSlider),
            
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24,154,30,PlanetMapModeModulesGoToBaseButton1,LanguageHelper.Popup_GotoBase),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24+31,154,30,PlanetMapModeModulesGoToBaseButton2,LanguageHelper.Popup_GotoBase),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24+62,154,30,PlanetMapModeModulesGoToBaseButton3,LanguageHelper.Popup_GotoBase),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24+93,154,30,PlanetMapModeModulesGoToBaseButton4,LanguageHelper.Popup_GotoBase),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24+124,154,30,PlanetMapModeModulesGoToBaseButton5,LanguageHelper.Popup_GotoBase),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24+155,154,30,PlanetMapModeModulesGoToBaseButton6,LanguageHelper.Popup_GotoBase),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24+186,154,30,PlanetMapModeModulesGoToBaseButton7,LanguageHelper.Popup_GotoBase),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24+217,154,30,PlanetMapModeModulesGoToBaseButton8,LanguageHelper.Popup_GotoBase),
                        new GuiObject(GuiObjectState.MenuButton,panelstartx+551-154-8,panelstarty+37+24+248,154,30,PlanetMapModeModulesGoToBaseButton9,LanguageHelper.Popup_GotoBase),}));
            }
        }

        void PlanetMapModeModulesUpButton(ref GuiObject me)
        {
            if (me.state.l_click)
                if (startmedulesitem > 0) startmedulesitem--;
        }
        void PlanetMapModeModulesDownButton(ref GuiObject me)
        {
            if (me.state.l_click)
                if (startmedulesitem < planet.modules.Count - 9) startresearchitem++;
        }
        void PlanetMapModeModulesSlider(ref GuiObject me)
        {
            if (me.state.l_click && planet.modules.Count > 9)
            {
                float pos = camera.mouse.Y - me.rect.Y;
                startmedulesitem = (int)pos * planet.modules.Count / me.rect.Height;

                if (startmedulesitem > planet.modules.Count - 9) startmedulesitem = planet.modules.Count - 9;
                if (startmedulesitem < 0) startmedulesitem = 0;
            }

            if (camera.mouseWheel != camera.mouseWheelOld && planet.modules.Count > 9)
            {
                int dw = camera.mouseWheelOld - camera.mouseWheel;
                startmedulesitem += dw / 30;
                if (startmedulesitem > planet.modules.Count - 9) startmedulesitem = planet.modules.Count - 9;
                if (startmedulesitem < 0) startmedulesitem = 0;
            }
        }

        void PlanetMapModeModulesGoToBaseButton1(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }
        void PlanetMapModeModulesGoToBaseButton2(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem+1].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }
        void PlanetMapModeModulesGoToBaseButton3(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem+2].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }
        void PlanetMapModeModulesGoToBaseButton4(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem+3].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }
        void PlanetMapModeModulesGoToBaseButton5(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem+4].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }
        void PlanetMapModeModulesGoToBaseButton6(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem+5].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }
        void PlanetMapModeModulesGoToBaseButton7(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem+6].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }
        void PlanetMapModeModulesGoToBaseButton8(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem+7].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }
        void PlanetMapModeModulesGoToBaseButton9(ref GuiObject me) { if (me.state.l_click) { int id = planet.maps[startmedulesitem+8].baseid; SelectBase(id, (int)planet.maps[id].position.X, (int)planet.maps[id].position.Y); } }

        void OpenScore(Score score)
        {
            int s_size = 400;
            int s_startx = (width - s_size) / 2;
            int s_starty = (height - s_size) / 2 + 40;

            gui.popupformid = -1;
            gui.AddPopup( new Form(0, new GuiObject[] { 
                    new GuiObject(GuiObjectState.PanelDark, s_startx,   s_starty + 37,  s_size,     325, null) ,
                    new GuiObject(GuiObjectState.PanelDark, s_startx,   s_starty,       s_size-120, 38, null),
                    new GuiObject(GuiObjectState.Layer,     s_startx,   s_starty,       s_size-120, 38, null, score.Total>=100000?LanguageHelper.Popup_SystemComplate:LanguageHelper.Popup_ScoreNeed),
                    
                    new GuiObject(GuiObjectState.Layer,     s_startx+20,s_starty+37+10, 125, 38, null, LanguageHelper.Popup_AllTime+": "+star.GetTotalTime().ToString("0")),
                    new GuiObject(GuiObjectState.Layer,     s_startx+10,s_starty+37+40, 125, 38, null, LanguageHelper.Popup_AllScore+": "+score.Total.ToString("0")),

                    new GuiObject(GuiObjectState.Layer,     s_startx+110,s_starty+37+90, 25, 38, null, LanguageHelper.Popup_ByBuildings+": "+score.building.ToString("0")),
                    new GuiObject(GuiObjectState.Layer,     s_startx+110,s_starty+37+90+30, 25, 38, null, LanguageHelper.Popup_ByEnergy+": "+score.energy.ToString("0")),
                    new GuiObject(GuiObjectState.Layer,     s_startx+110,s_starty+37+90+60, 25, 38, null, LanguageHelper.Popup_ByStorage+": "+score.inventory.ToString("0")),
                    new GuiObject(GuiObjectState.Layer,     s_startx+110,s_starty+37+90+90, 25, 38, null, LanguageHelper.Popup_ByPopulation+": "+score.population.ToString("0")),
                    new GuiObject(GuiObjectState.Layer,     s_startx+110,s_starty+37+90+120, 25, 38, null, LanguageHelper.Popup_ByHelp+": "+score.support.ToString("0")),

                    new GuiObject(GuiObjectState.MenuButton,s_startx+10,s_starty+37+90+180, (s_size-20)/2-5, 30, ScorePopupExit, LanguageHelper.Popup_ExitToMenu),
                    new GuiObject(GuiObjectState.MenuButton,s_startx+s_size/2+5,s_starty+37+90+180, (s_size-20)/2-5, 30, ScorePopupClose, LanguageHelper.Popup_ContinueGame)
            }));

            gui.popupform.elements[10].enable = score.Total >= 100000;
        }
        void ScorePopupExit(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                state_old = state;
                state = MainMenuMode;
                SetMenuState(1);
                gui.forms[1].Ready();
                gui.ClosePopup();
            }
        }
        void ScorePopupClose(ref GuiObject me)
        {
            if (me.state.l_click) gui.ClosePopup();
        }

        void DisconectReconectButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                client.Connect(hostIP, 3666, playername);
                disconect_time = 1;
            }
        }
        void DisconectExitButton(ref GuiObject me)
        {
            if(me.state.l_click)
            {
                client.Disconnect();
                state = MainMenuMode;
                SetMenuState(0);
            }
        }
        //--------------------------------------MessageBox functions
        void ExitYesButton(ref GuiObject me)
        {
            if (me.state.l_click)
                this.Exit();
        }
        void ExitNoButton(ref GuiObject me)
        {
            if (me.state.l_click)
            {
                gui.ClosePopup();
                foreach (Form f in gui.forms)
                    f.Ready();
                //gui.inputbox.Ready();
            }
        }

        #endregion
    }
}
