namespace EnergyTrading.Mapping
{
    using System;

    /// <summary>
    /// Exception raised for failure to locate an appropriate <see cref="IXmlMappingEngine" />
    /// </summary>
    [Serializable]
    public class XmlEngineResolutionException : MappingException
    {
        /// <summary>
        /// Gets the ASM schema version we are trying to locate a <see cref="IXmlMappingEngine" /> for.
        /// </summary>
        public string AsmVersion { get; private set; }

        /// <summary>
        /// Gets the ASM schema version we are trying to locate a <see cref="IXmlMappingEngine" /> for.
        /// </summary>
        [Obsolete("Use AsmVersion")]
        public string AdmVersion { get; private set; }

        /// <summary>
        /// Gets the 
        /// </summary>
        public XmlEngineResolutionErrorCode Code { get; private set; }

        public XmlEngineResolutionException(XmlEngineResolutionErrorCode code, string asmVersion)
        {
            this.Code = code;
            this.AsmVersion = asmVersion;
            this.AdmVersion = asmVersion;
        }

        public XmlEngineResolutionException(XmlEngineResolutionErrorCode code, string admVersion, Exception innerException) : base(string.Empty, innerException)
        {
            this.Code = code;
            this.AsmVersion = this.AsmVersion;
            this.AdmVersion = admVersion;
        }

        private string ParseCode()
        {
            if (this.Code == XmlEngineResolutionErrorCode.Undetermined)
            {
                return "Undetermined";
            }

            if ((this.Code & XmlEngineResolutionErrorCode.UnexpectedSchema) == XmlEngineResolutionErrorCode.UnexpectedSchema)
            {
                return "Unexpected schema";
            }

            var ret = "Message Version is";
            if ((this.Code & XmlEngineResolutionErrorCode.MessageVersionTooLow) == XmlEngineResolutionErrorCode.MessageVersionTooLow)
            {
                ret += " lower";
            }

            if ((this.Code & XmlEngineResolutionErrorCode.MessageVersionTooHigh) == XmlEngineResolutionErrorCode.MessageVersionTooHigh)
            {
                if (ret.EndsWith("lower "))
                {
                    ret += " and";
                }
                ret += " higher";
            }

            ret += " than the registered versions";
            return ret;
        }

        public override string Message
        {
            get { return "Unable to resolve IXmlMappingEngine for " + this.AsmVersion + " reason: " + this.ParseCode(); }
        }
    }
}