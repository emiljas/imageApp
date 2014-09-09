namespace ImageApp.Utils
{
    using System;
    using Windows.Foundation.Collections;
    using Windows.Storage;

    public abstract class LocalSettings
    {
        private string baseKey;
        private IPropertySet settings;

        public LocalSettings()
        {
            this.settings = ApplicationData.Current.LocalSettings.Values;
        }

        protected string BaseKey
        {
            set
            {
                this.baseKey = value;
            }
        }

        protected object this[string key]
        {
            get { return this.settings[key]; }
            set { this.settings[key] = value; }
        }

        public void InjectSettings(IPropertySet settings)
        {
            this.settings = settings;
        }

        protected bool Exists(string key)
        {
            return this.settings.ContainsKey(key);
        }

        protected string MakeKey(string key)
        {
            if (this.baseKey == null)
                throw new NullReferenceException("BaseKey");
            return this.baseKey + key;
        }
    }
}
