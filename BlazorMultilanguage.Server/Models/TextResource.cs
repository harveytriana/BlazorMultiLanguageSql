/*
BlazorMultiLanguage :: TextResource
blazorspread.net
*/
using System.ComponentModel.DataAnnotations;

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

        // to enabled in linq get by name 
        public string Get(string culture) {
            return culture switch {
                "EN" => EN,
                "ES" => ES,
                "DE" => PT,
                "RU" => RU,
                "NO" => NO,
                "IT" => IT,
                _ => EN,
            };
        }

        // to update by neme (manage)
        public string Set(string culture, string value) {
            return culture switch {
                "EN" => EN = value,
                "ES" => ES = value,
                "DE" => PT = value,
                "RU" => RU = value,
                "NO" => NO = value,
                "IT" => IT = value,
                _ => EN = value,
            };
        }
    }
}
