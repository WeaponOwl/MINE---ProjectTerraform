using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace ProjectTerraform
{
    public abstract class Script
    {
        public string name;
        public float waittime = -1;

        abstract public void Launch();
        abstract public void Update();
        abstract public void Draw();

        abstract public void Save(System.IO.BinaryWriter bw);
        abstract public void Load(System.IO.BinaryReader br);

        #region Callbacks for interact
        SetStringDelegate showdialogdelegate;
        public void ShowDialog(string text)
        {
            showdialogdelegate(text);
        }
        public void ShowDialogDelegate(SetStringDelegate d)
        {
            showdialogdelegate = d;
        }

        SetStringDelegate showhintdelegate;
        public void ShowHint(string text)
        {
            showhintdelegate(text);
        }
        public void ShowHintDelegate(SetStringDelegate d)
        {
            showhintdelegate = d;
        }

        VoidDelegate finishmission;
        public void FinishMission()
        {
            finishmission();
        }
        public void FinishMissionDelegate(VoidDelegate d)
        {
            finishmission = d;
        }

        public void Wait(float a)
        {
            waittime = a;
        }

        SetIntDelegate setgamemode;
        public void SetGameMode(int usermode)
        {
            setgamemode(usermode);
        }
        public void SetGameModeDelegate(SetIntDelegate d)
        {
            setgamemode = d;
        }

        GetIntDelegate getgamemode;
        public int GetGameMode()
        {
            return getgamemode();
        }
        public void GetGameModeDelegate(GetIntDelegate d)
        {
            getgamemode = d;
        }

        VoidDelegate drawcursor;
        public void DrawCursor()
        {
            drawcursor();
        }
        public void DrawCursorDelegate(VoidDelegate d)
        {
            drawcursor = d;
        }

        VoidDelegate drawgui;
        public void DrawGui()
        {
            drawgui();
        }
        public void DrawGuiDelegate(VoidDelegate d)
        {
            drawgui = d;
        }

        SetStringIntIntColorDelegate drawtext;
        public void DrawText(string text, int a, int b, Color color)
        {
            drawtext(text, a, b, color);
        }
        public void DrawTextDelegate(SetStringIntIntColorDelegate d)
        {
            drawtext = d;
        }

        SetStringDelegate addtargettext;
        public void AddTargetText(string text)
        {
            addtargettext(text);
        }
        public void AddTargetTextDelegate(SetStringDelegate d)
        {
            addtargettext = d;
        }
        #endregion

        #region Get or test some data
        GetObjectDelegate getscreendelegate;
        public object GetScreen()
        {
            return getscreendelegate();
        }
        public void GetScreenDelegate(GetObjectDelegate d)
        {
            getscreendelegate = d;
        }

        GetObjectDelegate getscreensizedelegate;
        public object GetScreenSize()
        {
            return getscreensizedelegate();
        }
        public void GetScreenSizeDelegate(GetObjectDelegate d)
        {
            getscreensizedelegate = d;
        }

        GetObjectDelegate getcontentmanagerdelegate;
        public object GetContentManager()
        {
            return getcontentmanagerdelegate();
        }
        public void GetContentManagerDelegate(GetObjectDelegate d)
        {
            getcontentmanagerdelegate = d;
        }

        GetObjectDelegate getgraphicdevicedelegate;
        public object GetGraphicDevice()
        {
            return getgraphicdevicedelegate();
        }
        public void GetGraphicDeviceDelegate(GetObjectDelegate d)
        {
            getgraphicdevicedelegate = d;
        }

        GetStringDelegate getplayernamedelegate;
        public string PlayerName()
        {
            return getplayernamedelegate();
        }
        public void GetPlayerNameDelegate(GetStringDelegate d)
        {
            getplayernamedelegate = d;
        }

        GetBoolDelegate ispopup;
        public bool IsPopup()
        {
            return ispopup();
        }
        public void IsPopupDelegate(GetBoolDelegate d)
        {
            ispopup = d;
        }

        GetIntDelegate getpopup;
        public int GetPopup()
        {
            return getpopup();
        }
        public void GetPopupDelegate(GetIntDelegate d)
        {
            getpopup = d;
        }

        GetObjectDelegate getmap;
        public object GetMap()
        {
            return getmap();
        }
        public void GetMapDelegate(GetObjectDelegate d)
        {
            getmap = d;
        }

        GetObjectDelegate getstar;
        public object GetStar()
        {
            return getstar();
        }
        public void GetStarDelegate(GetObjectDelegate d)
        {
            getstar = d;
        }

        GetObjectDelegate getplanet;
        public object GetPlanet()
        {
            return getplanet();
        }
        public void GetPlanetDelegate(GetObjectDelegate d)
        {
            getplanet = d;
        }

        GetIntDelegate getlanguage;
        public int GetLanguage()
        {
            return getlanguage();
        }
        public void GetLanguageDelegate(GetIntDelegate d)
        {
            getlanguage = d;
        }

        GetFloatDelegate getellapsed;
        public float GetEllapced()
        {
            return getellapsed();
        }
        public void GetEllapcedDelegate(GetFloatDelegate d)
        {
            getellapsed = d;
        }
        #endregion

    }
    public delegate string  GetStringDelegate();
    public delegate bool    GetBoolDelegate();
    public delegate bool    GetBoolFromIntDelegate(int a);
    public delegate int     GetIntDelegate();
    public delegate float   GetFloatDelegate();
    public delegate object  GetObjectDelegate();

    public delegate void    SetStringDelegate(string text);
    public delegate void    SetStringIntIntColorDelegate(string text,int a,int b,Color color);
    public delegate void    SetFloatDelegate(float a);
    public delegate void    SetIntDelegate(int a);
    public delegate void    SetBoolDelegate(bool a);

    public delegate void    VoidDelegate();
}
