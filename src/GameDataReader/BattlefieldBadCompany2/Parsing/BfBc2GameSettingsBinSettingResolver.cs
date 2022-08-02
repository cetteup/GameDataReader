﻿using System.Text.RegularExpressions;
using GameDataReader.Common;
using GameDataReader.Common.Parsing;

namespace GameDataReader.BattlefieldBadCompany2.Parsing;

/// <summary>
/// Loops through configuration file settings until it finds the right one.
/// </summary>
internal class BfBc2GameSettingsBinSettingResolver : SettingResolver
{
    private readonly string _configContent;

    public BfBc2GameSettingsBinSettingResolver(string configContent)
    {
        _configContent = configContent;
    }

    /// <summary>
    /// Looks up the desired setting in a Bad Company 2 GameSettings.bin configuration file.
    /// </summary>
    public override Setting GetSetting(string settingKey)
    {
        /*
         * Since GameSettings.bin is a binary file, there is lots of unreadable stuff in there. But through all that,
         * a pattern of [key]...stuff...[value] emerges. We need to ignore some key for which there no (readable) values,
         * such as FlashValues, UIMenuTrackerPage_Store, UIMenuTrackerPage_MenuUnlocks etc.
         */
        var re = new Regex($@"(?<{Constants.RegexKeyGroupName}>[a-zA-Z][\w]+)(?<!UIMenu\w+|FlashValues)[\x00-\x1F\x7f-\x9f\u2122\ufffd\s]+(?<{Constants.RegexValueGroupName}>[\x20-\x7e]+)");
        foreach (Match match in re.Matches(_configContent))
        {
            var key = match.Groups[Constants.RegexKeyGroupName].Value;
            var value = match.Groups[Constants.RegexValueGroupName].Value;
            
            if (key != settingKey)
                continue;

            return new BfBc2GameSettingsBinSetting(value);
        }

        throw new GameDataReaderException(message:
            $"Couldn't find config setting: {settingKey}\r\n" +
            "Given config content was:\r\n" +
            _configContent);
    }
}