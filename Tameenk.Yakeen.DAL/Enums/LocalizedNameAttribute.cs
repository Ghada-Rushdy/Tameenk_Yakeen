using System;
using System.Globalization;
using System.Linq;
using System.Resources;


namespace Tameenk.Yakeen.DAL.Enums
{
    public class LocalizedNameAttribute : Attribute
    {
        #region Fields

        private string _propertyName;

        #endregion

        #region Ctor

        public LocalizedNameAttribute(Type type, string propertyName)
        {
            _propertyName = propertyName;
            var props = type.GetProperties();
            if (props != null)
            {
                var resourceManagerProp = props.FirstOrDefault(p => p.Name == "ResourceManager");
                if (resourceManagerProp != null)
                {
                    ResourceManager = resourceManagerProp.GetValue(null) as ResourceManager;
                }
            }
        }

        #endregion

        #region Property
        public string Name
        {
            get
            {
                return ResourceManager.GetString(_propertyName);
            }
        }
        public string GetName(CultureInfo cultureInfo = null)
        {
            if (cultureInfo != null)
                return ResourceManager.GetString(_propertyName, cultureInfo);

            return ResourceManager.GetString(_propertyName);
        }
        public ResourceManager ResourceManager
        {
            get;
            private set;
        }
        #endregion
    }
}
