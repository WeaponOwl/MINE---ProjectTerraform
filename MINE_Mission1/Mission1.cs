using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Script : ProjectTerraform.Script
{
    const int UserMode = 89;
    const int StarOverviewMode = 102;

    int state;
    string[] text;
    RenderTarget2D starsystemtexture;
    Effect shader;
    GraphicsDevice gd;
    float discortion = 1;
    bool discortiondecrease = true;

    string target;

    public Script()
    {
        state = 0;
    }

    public override void Launch()
    {
        int lang = GetLanguage();
        if (lang == 1)
            text = System.IO.File.ReadAllLines("Content/Texts/rus_episode1_update.lang");
        else text = System.IO.File.ReadAllLines("Content/Texts/eng_episode1_update.lang");

        //state = 9;
    }

    public override void Save(System.IO.BinaryWriter bw)
    {
        bw.Write(state);
    }
    public override void Load(System.IO.BinaryReader br)
    {
        state = br.ReadInt32();
        int lang = GetLanguage();
        if (lang == 1)
            text = System.IO.File.ReadAllLines("Content/Texts/rus_episode1_update.lang");
        else text = System.IO.File.ReadAllLines("Content/Texts/eng_episode1_update.lang");
    }

    public override void Update()
    {
        if (state == 0)
        {
            if (!IsPopup())
            {
                ShowHint("\n " + text[0] + "\n ");
                state++;
            }
            else Wait(1);
        }
        else if (state == 1)
        {
            if (!IsPopup())
            {
                string t = text[1];
                for (int i = 2; i < 17; i++)
                    t += text[i] + "\n";

                ShowHint(t);
                AddTargetText(text[57]);
                state++;
            }
            else Wait(0.5f);
        }
        else
        {
            ProjectTerraform.Map map = (ProjectTerraform.Map)GetMap();
            if (map != null)
            {
                int basicid = -1;
                int proid = -1;
                for (int i = 0; i < map.science[0].items.Length; i++)
                {
                    if (map.science[0].items[i].id == ProjectTerraform.Constants.Research_basicresearchartifact)
                        basicid = i;
                    if (map.science[0].items[i].id == ProjectTerraform.Constants.Research_proresearchartifact)
                        proid = i;
                }
                if (map.inventory[(int)ProjectTerraform.Resources.artifacts].count >= 50)
                {
                    if (basicid == -1 && !IsPopup())
                    {
                        ProjectTerraform.ScienceItem[] newscience = new ProjectTerraform.ScienceItem[map.science[0].items.Length + 1];
                        map.science[0].items.CopyTo(newscience, 0);
                        newscience[map.science[0].items.Length] = new ProjectTerraform.ScienceItem(ProjectTerraform.Constants.Research_basicresearchartifact,
                                                                                                   200,
                                                                                                   text[53],
                                                                                                   3, new int[0], new float[0],
                                                                                                   text[54]);
                        map.science[0].items = newscience;
                        map.inventory[(int)ProjectTerraform.Resources.artifacts].count -= 50;
                        ShowHint("\n " + text[18] + "\n ");
                        if (state == 2)
                        {
                            state = 3;
                            AddTargetText(text[58]);
                        }
                    }
                }
                if (proid == -1 && basicid != -1 && map.science[0].items[basicid].searched && !IsPopup())
                {
                    ProjectTerraform.ScienceItem[] newscience = new ProjectTerraform.ScienceItem[map.science[0].items.Length + 1];
                    map.science[0].items.CopyTo(newscience, 0);
                    newscience[map.science[0].items.Length] = new ProjectTerraform.ScienceItem(ProjectTerraform.Constants.Research_proresearchartifact,
                                                                                               300,
                                                                                               text[55],
                                                                                               3, new int[0], new float[0],
                                                                                               text[56]);
                    map.science[0].items = newscience;
                    ShowHint("\n " + text[20] + "\n " + text[21] + "\n " + text[22] + "\n ");
                    if (state == 5)
                    {
                        state = 6;
                        AddTargetText(text[59]);
                    }
                }
                if (proid != -1 && map.science[0].items[proid].searched && !IsPopup())
                {
                    if (state == 6)
                    {
                        state = 7;
                        AddTargetText(null);
                        ShowHint("\n " + text[30] + "\n " + text[31] + "\n " + text[32] + "\n " + text[33] + "\n " + text[34] + "\n " + text[35] + "\n " + text[36] + "\n " + text[37] + "\n ");
                    }
                    map.maxbuildinglvl = 2;
                }
            }

            if (state == 3)
            {
                Wait(20);
                state = 4;
            }
            else if (state == 4 && !IsPopup())
            {
                Wait(1);
                state = 5;
                ShowHint("\n " + text[24] + "\n " + text[25] + "\n " + text[26] + "\n " + text[27] + "\n " + text[28] + "\n ");
                ProjectTerraform.Star star = (ProjectTerraform.Star)GetStar();
                if (star != null) star.pirates = true;
            }
            else if (state == 7)
            {
                Wait(40);
                state = 8;
            }
            else if (state == 8 && !IsPopup())
            {
                Wait(40);
                state = 9;
                ShowHint("\n " + text[38] + "\n " + text[39] + "\n " + text[40]);
                ProjectTerraform.Star star = (ProjectTerraform.Star)GetStar();
                if (star != null) star.pirates = false;
            }
            else if (state == 9 && !IsPopup())
            {
                SetGameMode(StarOverviewMode);
                Wait(30);
                state = 10;
                ShowHint("\n " + text[42] + "\n " + text[43] + "\n " + text[44] + "\n " + text[45] + "\n " + text[46] + "\n ");

                ProjectTerraform.Star star = (ProjectTerraform.Star)GetStar();

                ProjectTerraform.UnitGroup ug = new ProjectTerraform.UnitGroup();
                ProjectTerraform.Unit u = new ProjectTerraform.Unit(0, 0, 0);
                ug.units.Add(u);

                ug.player_id = -1;
                ug.position = new Vector2(30, 0);
                ug.planetid_target = 128;
                ug.planetid_mother = 0;

                star.unitgroups.Add(ug);
            }
            else if (state == 10 && !IsPopup())
            {
                state = 11;
                ShowHint("\n " + text[49] + "\n ");
            }
            else if (state == 11 && !IsPopup())
            {
                SetGameMode(StarOverviewMode);
                starsystemtexture = (RenderTarget2D)GetScreen();
                state = 12;
                Wait(10);
                SetGameMode(UserMode);

                Microsoft.Xna.Framework.Content.ContentManager Content = (Microsoft.Xna.Framework.Content.ContentManager)GetContentManager();
                shader = Content.Load<Effect>("Shaders/BarellShader");
                //shader = Content.Load<Effect>("Shaders/TileShader");
                shader.Parameters["Texture"].SetValue(starsystemtexture);

                gd = (GraphicsDevice)GetGraphicDevice();

                Matrix proj = Matrix.Identity;
                proj.M11 = 2.0f / gd.Viewport.Width;
                proj.M22 = -2.0f / gd.Viewport.Height;
                proj.M41 = -1;
                proj.M42 = 1;
                shader.Parameters["Proj"].SetValue(proj);
                shader.Parameters["Camera"].SetValue(Matrix.Identity);
            }
            else if (state == 12 && !IsPopup())
            {
                state = 13;
                ShowHint("\n " + text[51] + "\n ");
            }
            else if (state == 13 && !IsPopup())
            {
                //SetGameMode(StarOverviewMode);
                state = 14;
                FinishMission();
            }
            else
            {
                Wait(1);
            }
        }
    }

    public override void Draw() 
    {
        //DrawText(target, 0, 70, Microsoft.Xna.Framework.Color.Yellow);

        if (state == 12 || state == 13 || state == 14)
        {
            //GraphicsDevice gd = (GraphicsDevice)GetGraphicDevice();

            gd.Clear(Color.Black);

            //int sizex = (int)(gd.Viewport.AspectRatio*12);
            //int sizey = 12;
            int sizex = 10;
            int sizey = 10;
            Rectangle rect = new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height);

            VertexPositionColorTexture[] vertixes = new VertexPositionColorTexture[(sizex + 1) * (sizey + 1)];
            short[] indexes = new short[6*sizex*sizey];
            //{ 0, 1, 3, 1, 2, 3 };

            for (int i = 0; i < sizey; i++)
            {
                for (int j = 0; j < sizex; j++)
                {
                    int x1 = rect.X + rect.Width * j / sizex;
                    int y1 = rect.Y + rect.Height * i / sizey;
                    int x2 = rect.X + rect.Width * (j + 1) / sizex;
                    int y2 = rect.Y + rect.Height * (i + 1) / sizey;

                    int i1 = i * (sizey + 1) + j;
                    int i2 = i * (sizey+1) + j + 1;
                    int i3 = (i+1) * (sizey+1) + j + 1;
                    int i4 = (i+1) * (sizey+1) + j;

                    vertixes[i1] = new VertexPositionColorTexture(
                        new Vector3(x1, y1, 0), 
                        Color.White, 
                        new Vector2(((float)j) / sizex, ((float)i) / sizey));

                    vertixes[i2] = new VertexPositionColorTexture(
                        new Vector3(x2, y1, 0), 
                        Color.White, 
                        new Vector2(((float)j + 1) / sizex, ((float)i) / sizey));

                    vertixes[i3] = new VertexPositionColorTexture(
                        new Vector3(x2, y2, 0), 
                        Color.White, 
                        new Vector2(((float)j + 1) / sizex, ((float)i + 1) / sizey));

                    vertixes[i4] = new VertexPositionColorTexture(
                        new Vector3(x1, y2, 0), 
                        Color.White, 
                        new Vector2(((float)j) / sizex, ((float)i + 1) / sizey));

                    indexes[((i * sizey) + j) * 6] = (short)i1;
                    indexes[((i * sizey) + j) * 6 + 1] = (short)i2;
                    indexes[((i * sizey) + j) * 6 + 2] = (short)i4;
                    indexes[((i * sizey) + j) * 6 + 3] = (short)i2;
                    indexes[((i * sizey) + j) * 6 + 4] = (short)i3;
                    indexes[((i * sizey) + j) * 6 + 5] = (short)i4;
                }
            }

            //vertixes[0] = new VertexPositionColorTexture(new Vector3(0, 0, 0), Color.White, new Vector2(0, 0));
            //vertixes[1] = new VertexPositionColorTexture(new Vector3(gd.Viewport.Width, 0, 0), Color.White, new Vector2(1, 0));
            //vertixes[2] = new VertexPositionColorTexture(new Vector3(gd.Viewport.Width, gd.Viewport.Height, 0), Color.White, new Vector2(1, 1));
            //vertixes[3] = new VertexPositionColorTexture(new Vector3(0, gd.Viewport.Height, 0), Color.White, new Vector2(0, 1));

            shader.CurrentTechnique = shader.Techniques["Technique1"];
            shader.Parameters["BarrelPower"].SetValue(discortion);
            foreach (EffectPass pass in shader.CurrentTechnique.Passes)
            {
                pass.Apply();
                //gd.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, 4, indexes, 0, 2);
                gd.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertixes, 0, (sizey + 1) * (sizex + 1), indexes, 0, sizex * sizey * 2);
            }

            DrawGui();
            DrawCursor();

            if (discortiondecrease)
            {
                discortion -= GetEllapced()/5;
                if (discortion < 0.4f)
                    discortiondecrease = false;
            }
            else discortion += GetEllapced()*3;
        }
    }
}