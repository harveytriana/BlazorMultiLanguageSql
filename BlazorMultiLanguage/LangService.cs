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
        public static string CurrentLanguage { get; private set; }

        // language data
        static ImmutableDictionary<string, string> _textResources;

        // event for update each component when language changed
        public event Action OnChange;
        void NotifyStateChanged() => OnChange?.Invoke();

        // preserve user language
        const string LSKEY = "AppLanguage";

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
                var l = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", LSKEY);
                lang = l ?? "EN"; // set default
            }
            if (CurrentLanguage == lang) {
                return;
            }
            try {
                var url = $"api/TextResources/GetCulture/{lang}";
                var ls = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>(url);
                CurrentLanguage = lang;

                // let statics
                _textResources = ls.ToImmutableDictionary();

                // notify
                NotifyStateChanged();

                // save local storage
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", LSKEY, lang);

            } catch {// empty
                _textResources = new Dictionary<string, string>().ToImmutableDictionary();
            }
        }

        public async Task<string[]> GetCultures() {
            return await _httpClient.GetFromJsonAsync<string[]>("api/TextResources/GetCultures}");
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
