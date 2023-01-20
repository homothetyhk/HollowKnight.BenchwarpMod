namespace Benchwarp
{
    public class MenuIntAttribute : Attribute
    {
        public string name;
        public string description;
        public string[] values;

        public MenuIntAttribute(string name, int min, int max, int step, string description = "")
        {
            this.name = name;
            this.description = description;
            this.values = Enumerable.Range(0, (max - min + 1) / step).Select(x => (x * step + min).ToString()).ToArray();
        }
    }
}
