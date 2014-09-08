namespace ImageApp.Utils
{
    using System;
    using Windows.Foundation.Collections;
    using Windows.Storage;

    public abstract class ApplicationState
    {
        private string baseKey;

        protected string BaseKey
        {
            set
            {
                this.baseKey = value;
            }
        }

        private IPropertySet Settings
        {
            get
            {
                return ApplicationData.Current.R;
            }
        }

        protected object this[string key]
        {
            get { return this.Settings[key]; }
            set { this.Settings[key] = value; }
        }

        protected bool Exists(string key)
        {
            return this.Settings.ContainsKey(key);
        }

        protected string MakeKey(string key)
        {
            if (this.baseKey == null)
                throw new NullReferenceException("BaseKey");
            return this.baseKey + key;
        }
    }
}
