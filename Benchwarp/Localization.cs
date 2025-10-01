using Language;

namespace Benchwarp;

public static class Localization
{
    private static Dictionary<string, string> _map;
    private static readonly HashSet<LanguageCode> _langs;

    static Localization()
    {
        _langs = [];
        foreach (string s in typeof(Localization).Assembly.GetManifestResourceNames())
        {
            string[] components = s.Split('.'); // Benchwarp.Resources.Langs.{code}.json
            if (components.Length != 5
                || components[2] != "Langs"
                ) continue;
            _langs.Add((LanguageCode)Enum.Parse(typeof(LanguageCode), components[3], ignoreCase: true));
        }
    }

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
        if (_langs.Contains(code))
        {
            try
            {
                _map = JsonUtil.Deserialize<Dictionary<string, string>>($"Benchwarp.Resources.Langs.{code.ToString().ToLowerInvariant()}.json");
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

    public static string Localize(string text) 
    {
        if (Benchwarp.GS.OverrideLocalization) return text;

        return _map is not null && _map.TryGetValue(text, out string newText) ? newText : text;
    }
}
