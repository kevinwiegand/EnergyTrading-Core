﻿namespace EnergyTrading.Mapping
{
    using System;
    using System.Xml;

    using EnergyTrading.Extensions;

    /// <summary>
    /// Base implementation of <see cref="INamespaceManager" /> that uses a <see cref="XmlNamespaceManager"/>.
    /// </summary>
    public class BaseNamespaceManager : INamespaceManager
    {
        private readonly XmlNamespaceManager manager;
        private readonly Func<string, string> nsQual;

        /// <summary>
        /// Creates a new instance of the <see cref="BaseNamespaceManager" /> class.
        /// </summary>
        /// <param name="manager">XmlNamespaceManager to use.</param>
        public BaseNamespaceManager(XmlNamespaceManager manager)
        {
            this.manager = manager;

            Func<string, string> f = this.QualifyNamespace;
            this.nsQual = f.Memoize();
        }

        /// <contentfrom cref="INamespaceManager.RegisterNamespace" />
        public void RegisterNamespace(string prefix, string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return;
            }

            var pf2 = this.LookupNamespace(uri);
            if (string.IsNullOrEmpty(pf2))
            {
                if (prefix.Contains(":"))
                {
                    throw new XmlException("The ':' character, hexadecimal value 0x3A, cannot be included in a name - " + prefix);
                }
                this.manager.AddNamespace(prefix, uri);
            }            
        }

        /// <contentfrom cref="INamespaceManager.LookupNamespace" />
        public string LookupNamespace(string prefix, bool xname = false)
        {
            var uri = this.manager.LookupNamespace(prefix);
            return uri == string.Empty || !xname ? uri : this.nsQual(uri);
        }

        /// <contentfrom cref="INamespaceManager.LookupPrefix" />
        public string LookupPrefix(string uri)
        {
            return this.manager.LookupPrefix(uri);
        }

        /// <contentfrom cref="INamespaceManager.NamespaceExists" />
        public bool NamespaceExists(string uri)
        {
            return !string.IsNullOrEmpty(this.manager.LookupPrefix(uri));
        }

        /// <contentfrom cref="INamespaceManager.PrefixExists" />
        public bool PrefixExists(string prefix)
        {
            return !string.IsNullOrEmpty(this.manager.LookupNamespace(prefix));
        }

        private string QualifyNamespace(string uri)
        {
            return uri == string.Empty ? string.Empty : string.Format("{{{0}}}", uri);
        }
    }
}