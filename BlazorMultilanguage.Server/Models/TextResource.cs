using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMultilanguage.Server.Models
{
    public class TextResource
    {
        [MaxLength(020)] public string Id { get; set; } // Key
        [MaxLength(500)] public string EN { get; set; } // English
        [MaxLength(500)] public string ES { get; set; } // Spanish
        [MaxLength(500)] public string PT { get; set; } // Portuguese
        [MaxLength(500)] public string RU { get; set; } // Russian 
        [MaxLength(500)] public string NO { get; set; } // Norwegian
        [MaxLength(500)] public string IT { get; set; } // Italian
        // ...

        public override string ToString() => $"{Id}: {EN}";
    }
}
