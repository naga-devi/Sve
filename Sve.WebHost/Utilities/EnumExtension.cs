namespace Sve.WebHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NameValue
    {
        public string Name { get; set; }
        public object Id { get; set; }
    }

    public static class EnumExtension
    {
        public static List<NameValue> EnumToList<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<int>().Select(
                e => new NameValue
                {
                    Id = e,
                    Name = Enum.GetName(typeof(T), e)
                })).ToList();
        }
    }
}
