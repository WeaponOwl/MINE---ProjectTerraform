using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTerraform
{
    struct GuiObjectState
    {
        public const int Layer = 1;
        public const int DarkLayer = 10;
        public const int LayerNotAligned = 12;
        public const int Button = 2;
        public const int List = 3;
        public const int Grid = 4;
        public const int BuildingList = 5;
        public const int Slider = 6;
        public const int PanelLight = 7;
        public const int PanelDark = 8;
        public const int MenuButton = 9;
        public const int ButtonUpDown = 11;

        public int type;
        public bool on_mouse;
        public bool l_press;
        public bool r_press;
        public bool l_click;
        public bool r_click;

        public GuiObjectState(int type)
        {
            this.type = type;
            on_mouse = false;
            l_click = false;
            l_press = false;
            r_click = false;
            r_press = false;
        }
    }

    class GuiObject
    {
        public Rectangle rect;
        public GuiObjectState state;
        public string[] text;
        public bool selected;
        public float reserved;
        public bool enable;

        public delegate void UpdateFunction(ref GuiObject me);
        public UpdateFunction Update;

        public GuiObject(int type ,int x, int y, int w, int h,UpdateFunction upd)
        {
            rect = new Rectangle(x, y, w, h);
            state = new GuiObjectState(type);
            Update = upd;
            text = null;
            selected = false;
            reserved = 0;
            enable = true;
        }
        public GuiObject(int type, int x, int y, int w, int h, UpdateFunction upd,string caption)
        {
            rect = new Rectangle(x, y, w, h);
            state = new GuiObjectState(type);
            Update = upd;
            text = new string[1];
            text[0] = caption;
            selected = false;
            reserved = 0;
            enable = true;
        }
        public GuiObject(int type, int x, int y, int w, int h, UpdateFunction upd, string[] data)
        {
            rect = new Rectangle(x, y, w, h);
            state = new GuiObjectState(type);
            Update = upd;
            text = new string[data.Length];
            for (int i = 0; i <= data.Length; i++)
                text[i] = data[i];
            selected = false;
            reserved = 0;
            enable = true;
        }

        void DrawButton(SpriteBatch spriteBatch, Texture2D guiset)
        {
            Rectangle source = new Rectangle(98, 68, 30, 30);
            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y, 14, 14), new Rectangle(source.X, source.Y, 14, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y, 14, 14), new Rectangle(source.X + 16, source.Y, 14, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + rect.Height - 14, 14, 14), new Rectangle(source.X, source.Y + 16, 14, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y + rect.Height - 14, 14, 14), new Rectangle(source.X + 16, source.Y + 16, 14, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y, rect.Width - 28, 14), new Rectangle(source.X + 14, source.Y, 2, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y + rect.Height - 14, rect.Width - 28, 14), new Rectangle(source.X + 14, source.Y + 16, 2, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + 14, 14, rect.Height - 28), new Rectangle(source.X, source.Y + 14, 14, 2), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y + 14, 14, rect.Height - 28), new Rectangle(source.X + 16, source.Y + 14, 14, 2), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y + 14, rect.Width - 28, rect.Height - 28), new Rectangle(source.X + 14, source.Y + 14, 2, 2), Color.White);
        }
        void DrawPanel(SpriteBatch spriteBatch, Texture2D guiset,bool state)
        {
            Rectangle source = new Rectangle(98 - (state ? 30 : 0), 98, 30, 30);
            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y, 14, 14), new Rectangle(source.X, source.Y, 14, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y, 14, 14), new Rectangle(source.X + 16, source.Y, 14, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + rect.Height - 14, 14, 14), new Rectangle(source.X, source.Y+16, 14, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y + rect.Height - 14, 14, 14), new Rectangle(source.X + 16, source.Y + 16, 14, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y, rect.Width - 28, 14), new Rectangle(source.X + 14, source.Y, 2, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y + rect.Height - 14, rect.Width - 28, 14), new Rectangle(source.X + 14, source.Y + 16, 2, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + 14, 14, rect.Height - 28), new Rectangle(source.X, source.Y+14, 14, 2), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y + 14, 14, rect.Height - 28), new Rectangle(source.X + 16, source.Y + 14, 14, 2), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y + 14, rect.Width-28, rect.Height - 28), new Rectangle(source.X + 14, source.Y + 14, 2, 2), Color.White);
        }
        void DrawSlider(SpriteBatch spriteBatch, Texture2D guiset)
        {
            DrawPanel(spriteBatch, guiset, rect, true);

            DrawPanel(spriteBatch, guiset, new Rectangle(rect.X, rect.Y, 15, rect.Height));
            DrawPanel(spriteBatch, guiset, new Rectangle(rect.X + rect.Width - 15, rect.Y, 15, rect.Height));

            int maxpos = rect.Width - 34 - 11;
            int x = (int)(reserved * maxpos);

            DrawPanel(spriteBatch, guiset, new Rectangle(rect.X + 15 + x, rect.Y, 15, rect.Height));
        }
        void DrawUpDownButton(SpriteBatch spriteBatch, Texture2D guiset)
        {
            Rectangle source = new Rectangle(98, 8, 30, 30);
            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y, 30, 30), new Rectangle(source.X, source.Y, 30, 30), Color.White);
        }

        public static void DrawPanel(SpriteBatch spriteBatch, Texture2D guiset, Rectangle rect,bool state=false)
        {
            Rectangle source = new Rectangle(98 - (state ? 30 : 0), 98, 30, 30);
            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y, 14, 14), new Rectangle(source.X, source.Y, 14, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y, 14, 14), new Rectangle(source.X + 16, source.Y, 14, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + rect.Height - 14, 14, 14), new Rectangle(source.X, source.Y + 16, 14, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y + rect.Height - 14, 14, 14), new Rectangle(source.X + 16, source.Y + 16, 14, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y, rect.Width - 28, 14), new Rectangle(source.X + 14, source.Y, 2, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y + rect.Height - 14, rect.Width - 28, 14), new Rectangle(source.X + 14, source.Y + 16, 2, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + 14, 14, rect.Height - 28), new Rectangle(source.X, source.Y + 14, 14, 2), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y + 14, 14, rect.Height - 28), new Rectangle(source.X + 16, source.Y + 14, 14, 2), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y + 14, rect.Width - 28, rect.Height - 28), new Rectangle(source.X + 14, source.Y + 14, 2, 2), Color.White);
        }
        public static void DrawDarkPanel(SpriteBatch spriteBatch, Texture2D guiset, Rectangle rect)
        {
            Rectangle source = new Rectangle(98, 98-60, 30, 30);
            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y, 14, 14), new Rectangle(source.X, source.Y, 14, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y, 14, 14), new Rectangle(source.X + 16, source.Y, 14, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + rect.Height - 14, 14, 14), new Rectangle(source.X, source.Y + 16, 14, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y + rect.Height - 14, 14, 14), new Rectangle(source.X + 16, source.Y + 16, 14, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y, rect.Width - 28, 14), new Rectangle(source.X + 14, source.Y, 2, 14), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y + rect.Height - 14, rect.Width - 28, 14), new Rectangle(source.X + 14, source.Y + 16, 2, 14), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + 14, 14, rect.Height - 28), new Rectangle(source.X, source.Y + 14, 14, 2), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width - 14, rect.Y + 14, 14, rect.Height - 28), new Rectangle(source.X + 16, source.Y + 14, 14, 2), Color.White);

            spriteBatch.Draw(guiset, new Rectangle(rect.X + 14, rect.Y + 14, rect.Width - 28, rect.Height - 28), new Rectangle(source.X + 14, source.Y + 14, 2, 2), Color.White);
        }
        public static void DrawRectangle(SpriteBatch spriteBatch, Texture2D guiset, Rectangle rect)
        {
            Rectangle source = new Rectangle(98 - 60, 98, 30, 30);
            spriteBatch.Draw(guiset, rect, source, Color.White);
        }
        public static void DrawRectangleOverline(SpriteBatch spriteBatch, Texture2D guiset, Rectangle rect)
        {
            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y, rect.Width, 1), new Rectangle(80, 0, 16, 16), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, 1), new Rectangle(80, 0, 16, 16), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X, rect.Y, 1, rect.Height), new Rectangle(80, 0, 16, 16), Color.White);
            spriteBatch.Draw(guiset, new Rectangle(rect.X + rect.Width, rect.Y, 1, rect.Height), new Rectangle(80, 0, 16, 16), Color.White);
        }
        public static void DrawRectangleTransparent(SpriteBatch spriteBatch, Texture2D guiset, Rectangle rect)
        {
            spriteBatch.Draw(guiset, rect, new Rectangle(10, 0, 10, 10), new Color(100,100,100,100));
        }

        void DrawText(SpriteBatch spriteBatch, SpriteFont font,string text,Color color)
        {
            Vector2 size = font.MeasureString(text);
            size.X = (int)(size.X / 2);
            size.Y = (int)(size.Y / 2);
            Vector2 pos = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            spriteBatch.DrawString(font, text, pos, color, 0, size, 1, SpriteEffects.None, 0);
        }
        void DrawText(SpriteBatch spriteBatch, SpriteFont font, string text,Rectangle rect)
        {
            Vector2 size = font.MeasureString(text);
            size.X = (int)(size.X / 2);
            size.Y = (int)(size.Y / 2);
            Vector2 pos = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            spriteBatch.DrawString(font, text, pos, Color.White, 0, size, 1, SpriteEffects.None, 0);
        }
        void DrawTextNotAligned(SpriteBatch spriteBatch, SpriteFont font, string text, Color color)
        {
            Vector2 pos = new Vector2(rect.X, rect.Y);
            spriteBatch.DrawString(font, text, pos, color);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D guiset, SpriteFont font)
        {
            if(enable)
            switch (state.type)
            {
                case GuiObjectState.Layer:
                    if (text != null) DrawText(spriteBatch, font, text[0], Color.White);
                    break;
                case GuiObjectState.DarkLayer:
                    if (text != null) DrawText(spriteBatch, font, text[0], Color.Black);
                    break;
                case GuiObjectState.LayerNotAligned:
                    if (text != null) DrawTextNotAligned(spriteBatch, font, text[0], Color.White);
                    break;
                case GuiObjectState.Button:
                    DrawButton(spriteBatch, guiset);
                    if (text != null) DrawText(spriteBatch, font, text[0], selected ? Color.Red : Color.White);
                    break;
                case GuiObjectState.MenuButton:
                    DrawPanel(spriteBatch, guiset, false);
                    if (text != null) DrawText(spriteBatch, font, text[0], selected ? Color.Red : Color.White);
                    break;
                case GuiObjectState.List:
                    spriteBatch.Draw(guiset, rect, Color.White);
                    if (text != null)
                    {
                        DrawText(spriteBatch, font, text[0], Color.White);
                    }
                    break;
                case GuiObjectState.PanelDark:
                    DrawPanel(spriteBatch, guiset, true);
                    break;
                case GuiObjectState.PanelLight:
                    DrawPanel(spriteBatch, guiset, false);
                    break;
                case GuiObjectState.Slider:
                    DrawSlider(spriteBatch, guiset);
                    break;
                case GuiObjectState.ButtonUpDown:
                    DrawUpDownButton(spriteBatch, guiset);
                    break;
                default: break;
            }
        }
    }

    class Form
    {
        public GuiObject[] elements;
        public int state;
        public bool enable;
        public bool ready;
        public Form child;

        public Form(int s, int num,bool enbl=true)
        {
            state = s;
            enable = enbl;
            ready = true;
            elements = new GuiObject[num];
            child = null;
        }
        public Form(int s, GuiObject[] data, bool enbl = true)
        {
            state = s;
            enable = enbl;
            elements = data;
            ready = true;
            child = null;
        }

        public void Ready()
        { 
            ready = false;
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].selected = false;
            }
        }
        public void Update(int s, MouseState ms)
        {
            if (child == null)
            {
                if (enable && ready && (state == s))
                {
                    for (int i = 0; i < elements.Length; i++)
                    {
                        if (elements[i].rect.Contains(ms.X, ms.Y))
                        {
                            elements[i].state.on_mouse = true;

                            if (ms.LeftButton == ButtonState.Pressed)
                            {
                                if (elements[i].state.l_press)
                                    elements[i].state.l_click = false;
                                else
                                    elements[i].state.l_click = true;
                                elements[i].state.l_press = true;
                                elements[i].selected = true;
                            }
                            else { elements[i].state.l_press = false; elements[i].state.l_click = false; }

                            if (ms.RightButton == ButtonState.Pressed)
                            {
                                if (elements[i].state.r_press)
                                    elements[i].state.r_click = false;
                                else
                                    elements[i].state.r_click = true;
                                elements[i].state.r_press = true;
                            }
                            else { elements[i].state.r_press = false; elements[i].state.r_click = false; }
                        }
                        else
                        {
                            elements[i].state.on_mouse = false;
                            elements[i].state.r_click = false;
                            elements[i].state.r_press = false;
                            elements[i].state.l_click = false;
                            elements[i].state.l_press = false;
                            elements[i].selected = false;
                        }
                        if (elements[i].enable && elements[i].Update != null) elements[i].Update(ref elements[i]);
                    }
                }
                if (!ready && ms.LeftButton == ButtonState.Released) ready = true;
            }
            else child.Update(s, ms);
        }
        public void Draw(int s,SpriteBatch spriteBatch, Texture2D guiset, SpriteFont font)
        {
            if (enable&&(state == s))
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i].Draw(spriteBatch, guiset, font);
                }

                if (child != null)
                    child.Draw(s, spriteBatch, guiset, font);
            }
        }
    }

    class Gui
    {
        public List<Form> forms;
        public Form popupform;
        public int popupformid;
        public string lastinputstring;
        public string helpstring;
        public string queststring;

        public Gui()
        {
            forms = new List<Form>();
            popupform = null;
            popupformid = -1;
        }
        public void Update(int s, MouseState ms)
        {
            helpstring = null;
            if (popupform == null)
            {
                foreach (Form f in forms)
                    f.Update(s, ms);
            }
            else
            {
                popupform.Update(0, ms);
            }
        }
        public void Draw(int s,SpriteBatch spriteBatch, Texture2D guiset, SpriteFont font)
        {
            foreach (Form f in forms)
            {
                f.Draw(s, spriteBatch, guiset, font);
            }
            if (popupform != null) popupform.Draw(0, spriteBatch, guiset, font);
            else if (queststring != null) spriteBatch.DrawString(font, " [ ! ] : "+queststring, new Vector2(0, 70), Color.Yellow);
        }

        public void MessageBox(string text, GuiObject.UpdateFunction yes, GuiObject.UpdateFunction no, int centerscreenX, int centerscreenY)
        {
            int sx = centerscreenX - 235 / 2;
            int sy = centerscreenY - 116 / 2;
            popupform=new Form(0, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark, sx, sy, 235, 116, null), 
                                          new GuiObject(GuiObjectState.Layer, sx, sy, 235, 116-30, null,text) ,
                                          new GuiObject(GuiObjectState.MenuButton, sx+10, sy+76, 235/2-20, 30, yes,"Да") ,
                                          new GuiObject(GuiObjectState.MenuButton, sx+235/2+10, sy+76, 235/2-20, 30, no,"Нет")});
            popupform.Ready();
            popupformid = 0;
        }

        public void InputBox(string text, string value, GuiObject.UpdateFunction key, GuiObject.UpdateFunction ok, GuiObject.UpdateFunction back, int centerscreenX, int centerscreenY)
        {
            int sx = centerscreenX - 435 / 2;
            int sy = centerscreenY - 156 / 2;
            popupform=new Form(0, new GuiObject[] { new GuiObject(GuiObjectState.PanelDark,    sx,             sy,         435, 156,   null),
                                          new GuiObject(GuiObjectState.PanelLight,   sx,             sy+156-90,  435, 36,    null),
                                          new GuiObject(GuiObjectState.Layer,        sx,             sy+156-90,  435, 36,    key,value),
                                          new GuiObject(GuiObjectState.MenuButton,   sx+(335-120)/2, sy+156-40,  112, 30,    ok,"Ок"),
                                          new GuiObject(GuiObjectState.MenuButton,   sx+(335+120)/2, sy+156-40,  112, 30,    back,"Отмена"),
                                          new GuiObject(GuiObjectState.Layer,        sx,             sy+20,      435, 30,    null,text)});
            popupform.Ready();
            popupformid = 1;
        }

        public void AddPopup(Form form)
        {
            if (popupform == null)
            {
                popupform = form;
                popupform.Ready();
            }
            else
            {
                Form f = popupform;

                while (f.child != null)
                    f = f.child;

                f.child = form;
                f.child.Ready();
            }
        }
        public void ClosePopup()
        {
            if (popupform != null)
            {
                if (popupform.child == null)
                    popupform = null;
                else
                {
                    Form f = popupform;

                    while (f.child.child != null)
                        f = f.child;

                    f.child = null;
                    f.Ready();
                }
            }
        }
        public Form GetPopup()
        {
            if (popupform == null)
                return null;
            else
            {
                Form f = popupform;

                while (f.child != null)
                    f = f.child;

                return f;
            }
        }
    }
}
