using Language;

namespace Benchwarp;

public static class Localization
{
    private static Dictionary<string, string> _map;

    internal static void HookLocalization()
    {
        On.Language.Language.DoSwitch += OnSwitchLanguage;
        SetLanguage(Language.Language.CurrentLanguage());
    }

    internal static void UnhookLocalization()
    {
        On.Language.Language.DoSwitch -= OnSwitchLanguage;
        _map = null;
    }

    private static void OnSwitchLanguage(On.Language.Language.orig_DoSwitch orig, LanguageCode newLang)
    {
        orig(newLang);
        SetLanguage(newLang);
    }

    private static void SetLanguage(LanguageCode code)
    {
        if (GetBenchwarpLanguageCode(code) is string name)
        {
            try
            {
                _map = JsonUtil.Deserialize<Dictionary<string, string>>($"Benchwarp.Resources.Langs.{name}.json");
            }
            catch (Exception e)
            {
                Benchwarp.instance.LogError($"Error changing language to {code}: {e}");
            }
        }
        else
        {
            _map = null;
        }
    }

    private static string GetBenchwarpLanguageCode(LanguageCode newLang)
    {
        return newLang switch
        {
            LanguageCode.ZH => "zh",
            LanguageCode.PT => "pt",
            _ => null
        };
    }

    public static string Localize(string text) 
    {
        if (Benchwarp.GS.OverrideLocalization) return text;

        return _map is not null && _map.TryGetValue(text, out string newText) ? newText : text;
    }
}
