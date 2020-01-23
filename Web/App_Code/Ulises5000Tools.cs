using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


namespace Ulises5000Configuration
{
    public class ToolsUlises5000Section : ConfigurationSection 
    {

        [ConfigurationProperty("Version", DefaultValue = 0, IsRequired = true)]
        public int Version
        {
            get
            { 
                return (int)this["Version"]; 
            }
        }


        [ConfigurationProperty( "Tools", IsRequired = true )]
        public ConfigToolsCollection Tools {
            get {
                return base["Tools"] as ConfigToolsCollection;
            }
        }

        [ConfigurationCollection(typeof(Tool), AddItemName = "Tool")]
        public class ConfigToolsCollection : ConfigurationElementCollection, IEnumerable<Tool>
        {

            protected override ConfigurationElement CreateNewElement()
            {
                return new Tool();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                var l_configElement = element as Tool;
                if (l_configElement != null)
                    return l_configElement.Name;
                else
                    return null;
            }

            public Tool this[int index]
            {
                get
                {
                    return BaseGet(index) as Tool;
                }
            }

            new public Tool this[string Name]
            {
                get
                {
                    return (Tool)BaseGet(Name);
                }
            }


            #region IEnumerable<ConfigElement> Members

            IEnumerator<Tool> IEnumerable<Tool>.GetEnumerator()
            {
                return (from i in Enumerable.Range(0, this.Count)
                        select this[i])
                        .GetEnumerator();
            }

            #endregion
        }

        public class Tool : ConfigurationElement
        {

            [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
            public string Name
            {
                get
                {
                    return base["Name"] as string;
                }
            }
        }

        private static ToolsUlises5000Section _Instance = null;
        public static ToolsUlises5000Section Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = (ToolsUlises5000Section)System.Web.Configuration.WebConfigurationManager.GetSection("ToolsUlises5000Section");
                }
                return _Instance;
            }
        }
    }
}

