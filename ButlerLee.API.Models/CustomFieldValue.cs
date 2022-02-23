using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ButlerLee.API.Models
{
    public class CustomField
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public int IsPublic { get; set; }
        
        //public string Type { get; set; }
        //public IEnumerable<PossibleValue> PossibleValues { get; set; }
        //public string VarName { get; set; }
        //public int? SortOrder { get; set; }
        //public string ObjectType { get; set; }
        //public int? AccountId { get; set; }
    }

    public class CustomFieldValue
    {
        public int Id { get; set; }

        public int CustomFieldId { get; set; }

        public object Value { get; set; }

        public CustomField CustomField { get; set; }
    }

    public class PossibleValue
    {
        public string Value { get; set; } 
    }

}
