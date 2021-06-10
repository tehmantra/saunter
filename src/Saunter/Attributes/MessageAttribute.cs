using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MessageAttribute : Attribute
    {
        public MessageAttribute(Type payloadType)
        {
            PayloadType = payloadType;
        }

        public Type PayloadType { get; }

        /// <summary>
        /// A machine-friendly name for the message.
        /// Defaults to the generated schemaId.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A human-friendly title for the message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short summary of what the message is about.
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// A verbose explanation of the message.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }
    }
}