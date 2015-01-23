using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTerraform
{
    static class LanguageHelper
    {
        static string[] template;

        public static void Load(string filename)
        {
            template = System.IO.File.ReadAllLines(filename);
        }

        public static string Message_IDGeted { get { return template[0]; } }
        public static string Message_HaveSplited { get { return template[1]; } }
        public static string Message_Parts { get { return template[2]; } }

        public static string Gui_Play { get { return template[3]; } }
        public static string Gui_Settings { get { return template[4]; } }
        public static string Gui_Help { get { return template[5]; } }
        public static string Gui_About { get { return template[6]; } }
        public static string Gui_Exit { get { return template[7]; } }
        public static string Gui_LeaveMission { get { return template[8]; } }
        public static string Gui_BackToGame { get { return template[9]; } }
        public static string Gui_Save { get { return template[10]; } }
        public static string Gui_Load { get { return template[11]; } }
        public static string Gui_ChangeNickname { get { return template[12]; } }

        public static string Gui_Tutorial { get { return template[13]; } }
        public static string Gui_Mission { get { return template[14]; } }
        public static string Gui_SmallSpace { get { return template[15]; } }
        public static string Gui_CalculateSystem { get { return template[16]; } }
        public static string Gui_Back { get { return template[17]; } }
        public static string Gui_Multyplayer { get { return template[18]; } }

        public static string Gui_Music { get { return template[19]; } }
        public static string Gui_on { get { return template[20]; } }
        public static string Gui_off { get { return template[21]; } }
        public static string Gui_MusicLevel { get { return template[22]; } }
        public static string Gui_Autosave { get { return template[23]; } }
        public static string Gui_Messages { get { return template[24]; } }

        public static string Gui_Chrono { get { return template[25]; } }
        public static string Gui_Promo { get { return template[26]; } }

        public static string Gui_Fly { get { return template[27]; } }
        public static string Gui_ToSystem { get { return template[28]; } }
        public static string Gui_ToPlanetMap { get { return template[29]; } }
        public static string Gui_Score { get { return template[30]; } }

        public static string Gui_CreateBase { get { return template[31]; } }
        public static string Gui_SelectBase { get { return template[32]; } }
        public static string Gui_Stream { get { return template[33]; } }
        public static string Gui_Modules { get { return template[34]; } }

        public static string Gui_ToPlanetOrbit { get { return template[35]; } }
        public static string Gui_EnergyMenu { get { return template[36]; } }
        public static string Gui_Storage { get { return template[37]; } }
        public static string Gui_Science { get { return template[38]; } }

        public static string BuildMenu_Administry { get { return template[39]; } }
        public static string BuildMenu_Manufacturing { get { return template[40]; } }
        public static string BuildMenu_Storage { get { return template[41]; } }
        public static string BuildMenu_Links { get { return template[42]; } }
        public static string BuildMenu_Science { get { return template[43]; } }
        public static string BuildMenu_Defence { get { return template[44]; } }
        public static string BuildMenu_Destroy { get { return template[45]; } }
        public static string BuildMenu_SwitchBuilding { get { return template[46]; } }
        public static string BuildMenu_CreditStream { get { return template[47]; } }

        public static string BuildMenu_CommandCenter { get { return template[48]; } }
        public static string BuildMenu_BuildCenter { get { return template[49]; } }
        public static string BuildMenu_StorageCenter { get { return template[50]; } }
        public static string BuildMenu_LinkCenter { get { return template[51]; } }
        public static string BuildMenu_ScienceCenter { get { return template[52]; } }
        public static string BuildMenu_DroneFactory { get { return template[53]; } }
        public static string BuildMenu_ProcesingFactory { get { return template[54]; } }
        public static string BuildMenu_Mine { get { return template[55]; } }
        public static string BuildMenu_Dirrick { get { return template[56]; } }
        public static string BuildMenu_Farm { get { return template[57]; } }
        public static string BuildMenu_ClosedFarm { get { return template[58]; } }
        public static string BuildMenu_Generator { get { return template[59]; } }
        public static string BuildMenu_Warehouse { get { return template[60]; } }
        public static string BuildMenu_House { get { return template[61]; } }
        public static string BuildMenu_EnergyStorage { get { return template[62]; } }
        public static string BuildMenu_InfoStorage { get { return template[63]; } }
        public static string BuildMenu_LiquidStorage { get { return template[64]; } }
        public static string BuildMenu_Spaceport { get { return template[65]; } }
        public static string BuildMenu_Market { get { return template[66]; } }
        public static string BuildMenu_Parking { get { return template[67]; } }
        public static string BuildMenu_DroneBeacone { get { return template[68]; } }
        public static string BuildMenu_Laboratory { get { return template[69]; } }
        public static string BuildMenu_Collector { get { return template[70]; } }
        public static string BuildMenu_AtmosphereShield { get { return template[71]; } }
        public static string BuildMenu_PowerShield { get { return template[72]; } }
        public static string BuildMenu_EmmisionShield { get { return template[73]; } }
        public static string BuildMenu_Turret { get { return template[74]; } }

        public static string Gui_Delete { get { return template[75]; } }
        public static string Gui_FindCatalog { get { return template[76]; } }

        public static string DrawText_DisconnectAfeter { get { return template[77]; } }
        public static string DrawText_Seconds { get { return template[78]; } }
        public static string DrawText_DisconectMode { get { return template[79]; } }
        public static string DrawText_Credits { get { return template[80]; } }
        public static string DrawText_Loading { get { return template[81]; } }

        public static string DrawText_Name { get { return template[82]; } }
        public static string DrawText_LengthToStar { get { return template[83]; } }
        public static string DrawText_Radius { get { return template[84]; } }
        public static string DrawText_Mass { get { return template[85]; } }
        public static string DrawText_Pleasure { get { return template[86]; } }
        public static string DrawText_Temperature { get { return template[87]; } }
        public static string DrawText_ao { get { return template[88]; } }
        public static string DrawText_km { get { return template[89]; } }
        public static string DrawText_massearth { get { return template[90]; } }
        public static string DrawText_atm { get { return template[91]; } }
        public static string DrawText_deg { get { return template[92]; } }
        public static string DrawText_Sector { get { return template[93]; } }
        public static string DrawText_kelvin { get { return template[94]; } }
        public static string DrawText_radiussun { get { return template[95]; } }
        public static string DrawText_masssun { get { return template[96]; } }

        public static string DrawText_Energy { get { return template[97]; } }
        public static string DrawText_Population { get { return template[98]; } }
        public static string DrawText_Credits2 { get { return template[99]; } }
        public static string DrawText_Energyshort { get { return template[100]; } }
        public static string DrawText_Populationshort { get { return template[101]; } }
        public static string DrawText_Credits2short { get { return template[102]; } }

        public static string Popup_Storage { get { return template[103]; } }
        public static string Popup_Import { get { return template[104]; } }
        public static string Popup_Export { get { return template[105]; } }
        public static string Popup_Left { get { return template[106]; } }
        public static string Popup_sciencepoints { get { return template[107]; } }

        public static string Popup_Administry { get { return template[108]; } }
        public static string Popup_Manufacturing { get { return template[109]; } }
        public static string Popup_Mining { get { return template[110]; } }
        public static string Popup_Science { get { return template[111]; } }
        public static string Popup_Links { get { return template[112]; } }
        public static string Popup_Defence { get { return template[113]; } }
        public static string Popup_Helping { get { return template[114]; } }
        public static string Popup_Name2 { get { return template[115]; } }
        public static string Popup_Type { get { return template[116]; } }
        public static string Popup_EnergyConsum { get { return template[117]; } }

        public static string Popup_StarLighting { get { return template[118]; } }
        public static string Popup_StarTemperature { get { return template[119]; } }
        public static string Popup_PlanerRadius { get { return template[120]; } }
        public static string Popup_PlanetLength { get { return template[121]; } }

        public static string Dialog_EnterBaseName { get { return template[122]; } }
        public static string Dialog_EnterPlayerName { get { return template[123]; } }
        public static string Dialog_ExitGame { get { return template[124]; } }
        public static string Dialog_SaveAs { get { return template[125]; } }
        public static string Dialog_CannotSaveInMultyplayer { get { return template[126]; } }
        public static string Dialog_DeleteSavedGame { get { return template[127]; } }
        public static string Dialog_InDevelopment { get { return template[128]; } }
        public static string Dialog_EnterSeverAdress { get { return template[129]; } }

        public static string Popup_EnergyPopup { get { return template[130]; } }
        public static string Popup_AllConsum { get { return template[131]; } }
        public static string Popup_AllProduce { get { return template[132]; } }
        public static string Popup_ResourcePopup { get { return template[133]; } }
        public static string Popup_Resource { get { return template[134]; } }
        public static string Popup_SciencePopup { get { return template[135]; } }
        public static string Popup_BaseScience { get { return template[136]; } }
        public static string Popup_ModuleScience { get { return template[137]; } }
        public static string Popup_QuestScience { get { return template[138]; } }
        public static string Popup_SendPopup { get { return template[139]; } }
        public static string Popup_Base { get { return template[140]; } }
        public static string Popup_Send { get { return template[141]; } }
        public static string Popup_InfoPopup { get { return template[142]; } }
        public static string Popup_Prodused { get { return template[143]; } }
        public static string Popup_StorageFulling { get { return template[144]; } }
        public static string Popup_Wait { get { return template[145]; } }
        public static string Popup_ComplateAfter { get { return template[146]; } }
        public static string Popup_Price { get { return template[147]; } }
        public static string Popup_Need { get { return template[148]; } }
        public static string Popup_Mining2 { get { return template[149]; } }
        public static string Popup_Collect { get { return template[150]; } }
        public static string Popup_Speedup { get { return template[151]; } }
        public static string Popup_PlanetModulesPopup { get { return template[152]; } }
        public static string Popup_GotoBase { get { return template[153]; } }

        public static string Popup_SystemComplate { get { return template[154]; } }
        public static string Popup_ScoreNeed { get { return template[155]; } }
        public static string Popup_AllTime { get { return template[156]; } }
        public static string Popup_AllScore { get { return template[157]; } }
        public static string Popup_ByBuildings { get { return template[158]; } }
        public static string Popup_ByEnergy { get { return template[159]; } }
        public static string Popup_ByStorage { get { return template[160]; } }
        public static string Popup_ByPopulation { get { return template[161]; } }
        public static string Popup_ByHelp { get { return template[162]; } }
        public static string Popup_ExitToMenu { get { return template[163]; } }
        public static string Popup_ContinueGame { get { return template[164]; } }

        public static string Gui_Sunstrikesshort { get { return template[165]; } }
        public static string Gui_Piratesshort { get { return template[166]; } }
        public static string Gui_Meteoritesshort { get { return template[167]; } }

        public static string Gui_Resolution { get { return template[168]; } }
        public static string Gui_Fullscreen { get { return template[169]; } }

        public static string Gui_Close { get { return template[170]; } }

        public static string BuildMenu_AttackFactory { get { return template[171]; } }
        public static string BuildMenu_Attack { get { return template[172]; } }

        public static string Gui_UseEnergyOre { get { return template[173]; } }

        public static string Gui_HumansNeedWater { get { return template[174]; } }
        public static string Gui_HumansNeedFood { get { return template[175]; } }

        public static string Gui_UseScienceBoost { get { return template[176]; } }

        public static string DrawText_PlanetsNum { get { return template[177]; } }

        public static string Gui_ChangeColor { get { return template[178]; } }

        public static string BuildMenu_AttackParking { get { return template[179]; } }
        public static string BuildMenu_RocketParking { get { return template[180]; } }
        public static string BuildMenu_TerrainScaner { get { return template[181]; } }
        public static string BuildMenu_Builder { get { return template[182]; } }

        public static string Popup_CurrentRocket { get { return template[183]; } }
        public static string Popup_CurrentRocketNone { get { return template[184]; } }
        public static string Popup_CurrentRocketAtom { get { return template[185]; } }
        public static string Popup_CurrentRocketNeitron { get { return template[186]; } }
        public static string Popup_CurrentRocketTwined { get { return template[187]; } }

        public static string Popup_LaunchRocket { get { return template[188]; } }

        public static string Gui_Reconect { get { return template[189]; } }

        public static string Gui_Attack { get { return template[190]; } }
        public static string Popup_AttackScreen { get { return template[191]; } }
        public static string Popup_LaunchFighters { get { return template[192]; } }
        public static string Popup_FromBase { get { return template[193]; } }
        public static string Popup_ToBase{ get { return template[194]; } }
        public static string Popup_FightersCount { get { return template[195]; } }

        public static string Gui_Language { get { return template[196]; } }
        public static string Gui_LanguageRus { get { return template[197]; } }
        public static string Gui_LanguageEng { get { return template[198]; } }
        public static string Gui_SayAll { get { return template[199]; } }
        public static string Gui_LoadingText { get { return template[200]; } }
        public static string Gui_Saved { get { return template[201]; } }
        public static string Gui_Loaded { get { return template[202]; } }
    }
}
