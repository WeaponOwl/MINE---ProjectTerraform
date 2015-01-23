using System;

public class Script : ProjectTerraform.Script
{
    const int StarOverviewMode = 102;
    const int PlanetOverviewMode = 103;
    const int PlanetMapMode = 104;
    const int LocalMapMode = 105;

    int state;
    string[] text;
    string target;

    public Script()
    {
        state = 0;
    }

    public override void Launch()
    {
        int lang = GetLanguage();
        if (lang == 1)
            text = System.IO.File.ReadAllLines("Content/Texts/rus_tutorial.lang");
        else text = System.IO.File.ReadAllLines("Content/Texts/eng_tutorial.lang");
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
            text = System.IO.File.ReadAllLines("Content/Texts/rus_tutorial.lang");
        else text = System.IO.File.ReadAllLines("Content/Texts/eng_tutorial.lang");

        if (state == 0)
            AddTargetText(text[12]);
        else if (state == 1)
            AddTargetText(text[13]);
        else if (state == 3)
            AddTargetText(text[14]);
        else if (state == 4)
            AddTargetText(text[15]);
        else if (state == 5)
            AddTargetText(text[16]);
        else if (state == 7)
            AddTargetText(text[17]);
        else if (state == 8)
            AddTargetText(text[18]);
        else if (state == 10)
            AddTargetText(text[19]);
    }

    public override void Update()
    {
        if (state == 0)
        {
            if (!IsPopup() && GetGameMode() == StarOverviewMode)
            {
                ShowDialog(text[0]);
                AddTargetText(text[12]);
                state++;
            }
            else Wait(3);
        }
        else if (state == 1)
        {
            if (!IsPopup() && GetGameMode() == PlanetMapMode)
            {
                ProjectTerraform.Planet planet = (ProjectTerraform.Planet)GetPlanet();
                ShowDialog(text[1] + (planet != null ? (planet.name != null ? planet.name : "") : "") + text[2]);
                AddTargetText(text[13]);
                state++;
            }
            else Wait(3);
        }
        else if (state == 2)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                AddTargetText(null);
                ShowDialog(text[3]);
                state++;
                Wait(3);
            }
            else Wait(3);
        }
        else if (state == 3)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                ShowDialog(text[4]);
                AddTargetText(text[14]);
                state++;
            }
            else Wait(3);
        }
        else if (state == 4)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                ProjectTerraform.Map map = (ProjectTerraform.Map)GetMap();

                if (map != null && map.buildings != null && map.isBuildingBuilded(ProjectTerraform.Building.Generator))
                {
                    ShowDialog(text[5]);
                    AddTargetText(text[15]);
                    state++;
                }
                else Wait(3);
            }
            else Wait(3);
        }
        else if (state == 5)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                ProjectTerraform.Map map = (ProjectTerraform.Map)GetMap();

                if (map != null && map.buildings != null && map.isBuildingBuilded(ProjectTerraform.Building.Mine))
                {
                    ShowDialog(text[6]);
                    AddTargetText(text[16]);
                    state++;
                }
                else Wait(3);
            }
            else Wait(3);
        }
        else if (state == 6)
        {
            if (GetPopup() == 7)
            {
                state++;
                AddTargetText(null);
            }
            else Wait(1);
        }
        else if (state == 7)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                ShowDialog(text[7]);
                AddTargetText(text[17]);
                state++;
            }
            else Wait(3);
        }
        else if (state == 8)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                ProjectTerraform.Map map = (ProjectTerraform.Map)GetMap();

                if (map != null && map.buildings != null && map.isBuildingBuilded(ProjectTerraform.Building.Exchanger)
                                                         && map.isBuildingBuilded(ProjectTerraform.Building.Spaceport)
                                                         && map.isBuildingBuilded(ProjectTerraform.Building.Parking))
                {
                    ShowDialog(text[8]);
                    AddTargetText(text[18]);
                    state++;
                }
                else Wait(3);
            }
            else Wait(3);
        }
        else if (state == 9)
        {
            if (GetPopup() == 3)
            {
                state++;
            }
            else Wait(1);
        }
        else if (state == 10)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                ProjectTerraform.Map map = (ProjectTerraform.Map)GetMap();
                ProjectTerraform.InventoryItem[] inv = map != null ? map.inventory : null;

                if (inv != null)
                    for (int i = 0; i < inv.Length; i++)
                        if (inv[i].exchangetype != 1)
                        {
                            ShowDialog(text[9]);
                            AddTargetText(text[19]);
                            state++;
                            break;
                        }
                Wait(3);
            }
            else Wait(3);
        }
        else if (state == 11)
        {
            //if (!IsPopup() && GetGameMode() == LocalMapMode)
            //{
            //    ProjectTerraform.Map map = (ProjectTerraform.Map)GetMap();
            //    ProjectTerraform.InventoryItem[] inv = map != null ? map.inventory : null;

            //    if (inv != null)
            //        for (int i = 0; i < inv.Length; i++)
            //            if (inv[i].exchangetype == 1)
            //            {
            //                ShowDialog(text[9]);
            //                state++;
            //                break;
            //            }
            //    Wait(3);
            //}
            //else Wait(3);
            state++;
        }
        else if (state == 12)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                ProjectTerraform.Map map = (ProjectTerraform.Map)GetMap();
                if (map != null && map.buildings != null && map.isBuildingBuilded(ProjectTerraform.Building.House))
                {
                    ProjectTerraform.InventoryItem[] inv = map != null ? map.inventory : null;

                    if (inv != null && inv[(int)ProjectTerraform.Resources.water].count > 0 && (inv[(int)ProjectTerraform.Resources.meat].count > 0 ||
                                                                                inv[(int)ProjectTerraform.Resources.fruits].count > 0 ||
                                                                                inv[(int)ProjectTerraform.Resources.vegetables].count > 0 ||
                                                                                inv[(int)ProjectTerraform.Resources.fish].count > 0))
                    {
                        state++;
                        ShowDialog(text[10]);
                        AddTargetText(null);
                    }
                    else Wait(3);
                }
                else Wait(3);
            }
            else Wait(3);
        }
        else if (state == 13)
        {
            if (GetPopup() == 4)
            {
                state++;
            }
            else Wait(1);
        }
        else if (state == 14)
        {
            if (!IsPopup() && GetGameMode() == LocalMapMode)
            {
                ShowDialog(text[11]);
                state++;
            }
            else Wait(3);
        }
    }
    public override void Draw()
    {
        ;
    }
}