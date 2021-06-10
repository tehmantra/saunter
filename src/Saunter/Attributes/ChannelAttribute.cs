using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ChannelAttribute : Attribute
    {
        public string Name { get; }

        public string Description { get; set; }

        public ChannelAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}