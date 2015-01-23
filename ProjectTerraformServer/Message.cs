using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTerraform
{
    enum MessageType
    { 
        none,
        timetodestroybase,
        cannotaddresources,
        researchok,
        buildingoff,
        hunger,
        meteorite,
        sunstrike,
        destroy,
        builded,
        request,
        campaing,
        campaingface
    }

    class Message
    {
        public string text;
        public float time;
        public Color color;
        public MessageType mt;
        public int baseid;
        public int[] face;

        public Message(MessageType type,int b,string text, float life,Color color,int[] face=null)
        {
            this.mt = type;
            this.text = text;
            time = life;
            this.color = color;
            baseid = b;
            this.face = face;
        }
    }

    class MessageSystem
    {
        public Message[] messages;

        public MessageSystem()
        {
            messages = new Message[20];
        }

        public bool AddMessage(MessageType type, int b, string text, float life, Color color, int[] face = null)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                if (type != MessageType.none && b != -1 && messages[i] != null && messages[i].time > 0 && messages[i].mt == type && messages[i].baseid == b) return false;
                if (messages[i] == null || messages[i].time < 0) { messages[i] = new Message(type, b, text, life, color, face); return true; }
            }
            return false;
        }

        public void Update(float ellapsedtime)
        {
            for (int i = 0; i <messages.Length; i++)
            {
                if (messages[i] != null)
                {
                    messages[i].time -= ellapsedtime;
                    if (messages[i].time < 1)
                        messages[i].color = Color.Lerp(messages[i].color, new Color(0, 0, 0, 0), 1 / 30f);
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font, Texture2D guiset, Texture2D faces,Effect faceEffect, Planet planet, int width, bool all = true)
        {
            int h = 70;
            for (int i = 0; i < messages.Length;i++ )
            {
                if (messages[i] != null && messages[i].time > 0 && (all || messages[i].mt == MessageType.campaing || messages[i].mt == MessageType.campaingface || messages[i].mt == MessageType.request || messages[i].mt == MessageType.timetodestroybase))
                {
                    string text = messages[i].text;
                    Vector2 size = font.MeasureString(text);
                    if (size.X > width)
                    {
                        string[] split = text.Split(' ');
                        int sz = 0;
                        text = "";
                        for (int k = 0; k < split.Length; k++)
                        {
                            int szk = (int)font.MeasureString(split[k]+" ").X;
                            if (szk + sz > width)
                            {
                                text += "\n" + split[k]+" ";
                                sz = 0;
                            }
                            else
                            {
                                if (split[k].Length > 0)
                                    text += split[k] + " ";
                                sz += szk;
                            }
                        }
                        size = font.MeasureString(text);
                        messages[i].text = text;
                    }

                    if (messages[i].baseid >= 0 && messages[i].mt != MessageType.campaing)
                        if (planet != null && planet.map.Count > messages[i].baseid)
                            text = planet.map[messages[i].baseid].name + ":" + text;
                    if (messages[i].mt == MessageType.campaingface && messages[i].face!=null)
                    {
                        GuiObject.DrawPanel(spritebatch, guiset, new Rectangle(2, h - 2, 82, 82));
                        spritebatch.End();

                        //-----------------draw face
                        spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, faceEffect);
                        Rectangle faceRect = new Rectangle(3, h - 1, 80, 80);
                        spritebatch.Draw(faces, faceRect, new Rectangle(messages[i].face[0] % 10 * 40, messages[i].face[0] / 10 * 40, 40, 40), Color.White);
                        spritebatch.Draw(faces, faceRect, new Rectangle(messages[i].face[1] % 10 * 40, messages[i].face[1] / 10 * 40, 40, 40), Color.White);
                        spritebatch.Draw(faces, faceRect, new Rectangle(messages[i].face[2] % 10 * 40, messages[i].face[2] / 10 * 40, 40, 40), Color.White);
                        spritebatch.Draw(faces, faceRect, new Rectangle(messages[i].face[3] % 10 * 40, messages[i].face[3] / 10 * 40, 40, 40), Color.White);
                        if (messages[i].face[4] >= 0) spritebatch.Draw(faces, faceRect, new Rectangle(messages[i].face[4] % 10 * 40, messages[i].face[4] / 10 * 40, 40, 40), Color.White);
                        spritebatch.End();

                        spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                        spritebatch.DrawString(font, text, new Vector2(88, h), messages[i].color);
                        if (size.Y < 84) size.Y = 84;
                    }
                    else spritebatch.DrawString(font, text, new Vector2(4, h), messages[i].color);
                    h += (int)size.Y;
                    h += 10;
                }
            }
        }

        public void DrawNone(SpriteBatch spritebatch, SpriteFont font)
        {
            int h = 70;
            foreach (Message m in messages)
            {
                if (m != null)
                {
                    string text = m.text;
                    if (m.mt == MessageType.none)
                    spritebatch.DrawString(font, text, new Vector2(0, h), m.color);
                    h += (int)font.MeasureString(text).Y;
                    h += 10;
                }
            }
        }
    }
}
