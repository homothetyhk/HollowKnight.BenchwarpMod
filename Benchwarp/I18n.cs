using Language;

namespace Benchwarp;

static class I18n
{
    private static Dictionary<string, string> _map = null;
    static I18n()
    {
        string name = Language.Language.CurrentLanguage() switch
        {
            LanguageCode.ZH => "zh",
            _ => null
        };
        if (name != null)
        {
            try
            {
                _map = JsonUtil.Deserialize<Dictionary<string, string>>($"Benchwarp.Resources.Langs.{name}.json");
            }
            catch (Exception e)
            {
                Benchwarp.instance.LogError(e);
            }
        }
    }

    public static string Localize(string key) 
    {
        if(_map == null)  return key;
        return _map.TryGetValue(key, out var v) ? v : key;
    }
}
