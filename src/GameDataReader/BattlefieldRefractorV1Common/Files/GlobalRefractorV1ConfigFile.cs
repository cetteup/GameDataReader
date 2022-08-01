﻿using GameDataReader.BattlefieldRefractorCommon.Files;

namespace GameDataReader.BattlefieldRefractorV1Common.Files;

internal class GlobalRefractorV1ConfigFile : ConfigFile<GlobalRefractorV1ConfigFile>
{
    private readonly string _gameName, _modName;
    

    public GlobalRefractorV1ConfigFile(string gameName, string modName)
    {
        _gameName = gameName;
        _modName = modName;
    }
    
    protected override string GetFilePath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return $@"{appData}\VirtualStore\Program Files (x86)\EA GAMES\{_gameName}\Mods\{_modName}\Settings\Profile.con";
    }

    public string GetCurrentlyActiveProfileName()
    {
        return GetSettingValue("game.setProfile");
    }
}