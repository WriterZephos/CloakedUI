using System.Collections.Generic;

namespace CloakedUI.Source.Assets.SubComponents
{
    public class AttributeStrategy
    {
        private Dictionary<string, object> Attributes { get; set; }

        public T GetAttribute<T>(string name)
        {
            object att;
            if (Attributes.TryGetValue(name, out att))
            {
                return (T)att;
            }
            return default(T);
        }
    }
}