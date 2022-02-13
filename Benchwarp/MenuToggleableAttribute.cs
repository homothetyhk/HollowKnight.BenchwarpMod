using System;

namespace Benchwarp
{
    public class MenuToggleableAttribute : Attribute
    {
        public string name;
        public string description;

        public MenuToggleableAttribute(string name, string description = "")
        {
            this.name = name;
            this.description = description;
        }
    }
}
