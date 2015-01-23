using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTerraform
{
    struct Message
    {
        public string text;
        public float life;

        public Message(string text, float life)
        {
            this.text = text;
            this.life = life;
        }
    }
    class MessageSystem
    {
        public Message[] messages;

        public MessageSystem()
        {
            messages = new Message[20];
            for(int i=0;i<20;i++)messages[i]=new Message("",-1);
        }

        public bool AddMessage(string text, float life)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                if (messages[i].text == null || messages[i].life < 0) { messages[i] = new Message(text, life); return true; }
            }
            return false;
        }

        public void Update(float ellapsedtime)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                if (messages[i].life > 0)
                {
                    messages[i].life -= ellapsedtime;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font, int width)
        {
            int h = 110;
            for (int i = 0; i < messages.Length;i++ )
            {
                if (messages[i].life > 0)
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

                    Color color = Color.White;
                    if (messages[i].life < 1) color.A = (byte)(255 * (1 - messages[i].life));
                    spritebatch.DrawString(font, text, new Vector2(4, h), color);
                    h += (int)size.Y;
                    h += 10;
                }
            }
        }
    }
}
