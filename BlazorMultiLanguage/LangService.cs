/*
BlazorMultiLanguage :: LangService
blazorspread.net
*/
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorMultiLanguage
{
    public class LangService
    {
        public static string CurrentCulture { get; private set; }
        public static string[] Cultures { get; private set; }

        // language data
        static ImmutableDictionary<string, string> _textResources;

        // event for update each component when language changed
        public event Action OnChange;
        void NotifyStateChanged() => OnChange?.Invoke();

        // preserve user language
        const string STORAGEKEY = "CurrentCulture";

        // to manage localStorage
        readonly IJSRuntime _jsRuntime;
        readonly HttpClient _httpClient;

        public LangService(IJSRuntime jsRuntime, IHttpClientFactory clientFactory) {
            _jsRuntime = jsRuntime;
            _httpClient = clientFactory.CreateClient("AspNetApi");
        }

        public async Task LoadLanguageAsync(string lang = null) {
            if (lang == null) {
                // get storage user language
                var l = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", STORAGEKEY);
                lang = l ?? "EN"; // set default
            }
            if (CurrentCulture == lang) {
                return;
            }
            CurrentCulture = lang;
            try {
                var url = $"api/TextResources/GetCulture/{lang}";
                var ls = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>(url);

                // let statics
                _textResources = ls.ToImmutableDictionary();

                // notify
                NotifyStateChanged();

                // save local storage
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", STORAGEKEY, lang);

            } catch {// empty
                _textResources = new Dictionary<string, string>().ToImmutableDictionary();
            }
            Console.WriteLine("Start CurrentLanguage: {0}", CurrentCulture);

        }

        public async Task<string[]> GetCultures() {
            if (Cultures is null) {
                Cultures= await _httpClient.GetFromJsonAsync<string[]>("api/TextResources/GetCultures");
            }
            return Cultures;
        }

        /// <summary>
        /// Get text from key, without method name 
        /// </summary>
        public virtual string this[string key]
        {
            get {
                if (_textResources.ContainsKey(key)) {
                    return _textResources[key];
                }
                return $"[{key}]"; // untranslate text
            }
        }
    }

    record TextResource
    (
        string Id, // Key
        string EN, // English
        string ES, // Spanish
        string PT, // Portuguese
        string RU, // Russian 
        string NO, // Norwegian
        string IT  // Italian
                   // ...
    );
}
