using System;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Traits;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// A reference to some other object within the AsyncAPI document. 
    /// </summary>
    public class Reference
    {
        public Reference(string id, string path)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        private readonly string _id;
        private readonly string _path;

        [JsonProperty("$ref")]
        public string Ref => string.Format(_path, _id);
        
        [JsonIgnore()]
        public string Id => _id;
    }
}