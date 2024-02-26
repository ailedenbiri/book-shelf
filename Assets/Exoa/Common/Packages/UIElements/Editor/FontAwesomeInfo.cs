using Exoa.Json;
using Exoa.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Style.Icon
{
    /// <summary>
    /// Class FontAwesomeInfo.
    /// Implements the <see cref="UnityEngine.ScriptableObject" />
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Plugins/FontAwesome", fileName = "FontAwesomeInfo", order = 0)]
    public sealed class FontAwesomeInfo : ScriptableObject
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void FactoryRegistry() => Initialize();
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static FontAwesomeInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    FontAwesomeInfo[] resources = Resources.LoadAll<FontAwesomeInfo>("FontAwesomeInfo");
                    if (resources != null && resources.Length > 0) _instance = resources[0];
                }
                return _instance;
            }
        }
        private static FontAwesomeInfo _instance;

        /// <summary>
        /// Factories the registry.
        /// </summary>
        public static void Initialize()
        {
            if (Instance == null) return;
            if (Application.isEditor)
            {
                Instance.fontCache.Clear();
                foreach (TextAsset textAsset in Instance.iconJson)
                {
                    if (textAsset == null || textAsset.text.IsNullOrEmpty()) continue;

                    Dictionary<string, object> output = JsonConvert.DeserializeObject<Dictionary<string, object>>(textAsset.text);
                    if (output == null)
                    {
                        Debug.LogError("Could not read the icon file");
                        continue;
                    }
                    foreach (KeyValuePair<string, object> tmp in output)
                    {
                        if (tmp.Key.IsNullOrEmpty() || tmp.Value == null) continue;
                        JObject detail = (JObject)tmp.Value;
                        JArray styles = (JArray)detail["styles"];
                        List<string> stylesList = styles.Values<string>().ToList();
                        string unicode = (string)detail["unicode"];

                        string id = "fa-" + tmp.Key;

                        if (UnityEngine.Icon.Contains(id)) continue;
                        if (styles == null) continue;

                        FontAwesome font = Instance?.iconFont?.Find(item => stylesList.Contains(item.ID));

                        if (font == null) continue;

                        Instance.fontCache.Add(new FontCache(id, unicode, font.font));
                        UnityEngine.Icon.Add(id, unicode, font.font);
                    }
                }
            }
            else
            {
                foreach (FontCache cache in Instance.fontCache)
                {
                    UnityEngine.Icon.Add(cache.ID, cache.unicode, cache.font);
                }
            }
        }

        /// <summary>
        /// Gets the icon json.
        /// </summary>
        /// <value>The icon json.</value>
        public List<TextAsset> iconJson { get => this._iconJson; }
        [SerializeField]
        private List<TextAsset> _iconJson = new List<TextAsset>();
        //private List<TextAsset> _iconJson = new List<TextAsset>( );

        /// <summary>
        /// Gets or sets the localize font.
        /// </summary>
        /// <value>The localize font.</value>
        public List<FontAwesome> iconFont { get => this._iconFont; }
        [SerializeField]
        private List<FontAwesome> _iconFont = new List<FontAwesome>();
        //private List<FontAwesome> _iconFont = new List<FontAwesome>( );

        /// <summary>
        /// Class FontAwesome.
        /// </summary>
        [System.Serializable]
        public sealed class FontAwesome : SerializableBehaviour
        {
            /// <summary>
            /// The identifier
            /// </summary>
            [SerializeField]
            public string ID;
            /// <summary>
            /// The font
            /// </summary>
            [SerializeField]
            public Font font;
        }

        /// <summary>
        /// The font cache
        /// </summary>
        [SerializeField]
        private List<FontCache> fontCache = new List<FontCache>();
        //private List<FontCache> fontCache = new List<FontCache>( );

        /// <summary>
        /// Class FontCache. This class cannot be inherited.
        /// </summary>
        [System.Serializable]
        public sealed class FontCache
        {
            /// <summary>
            /// The identifier
            /// </summary>
            [SerializeField]
            public string ID;
            /// <summary>
            /// The unicode
            /// </summary>
            [SerializeField]
            public string unicode;
            /// <summary>
            /// The font
            /// </summary>
            public Font font;
            /// <summary>
            /// Initializes a new instance of the <see cref="FontCache"/> class.
            /// </summary>
            /// <param name="ID">The identifier.</param>
            /// <param name="unicode">The unicode.</param>
            /// <param name="font">The font.</param>
            public FontCache(string ID, string unicode, Font font)
            {
                this.ID = ID;
                this.unicode = unicode;
                this.font = font;
            }
        }
    }
}
