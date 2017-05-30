namespace TSLTestGenerator.DataModel
{
    public enum TSLProtocolType
    {
        Syn,
        Asyn,
        Http,
    }

    public class TSLProtocol : ITSLTopLevelElement
    {
        public TSLProtocol(string name, TSLProtocolType type, ITSLType requestType, ITSLType responseType)
        {
            Name = name;
            Type = type;
            RequestType = requestType;
            ResponseType = responseType;
        }

        public string Name { get; }
        public TSLProtocolType Type { get; }
        public ITSLType RequestType { get; }
        public ITSLType ResponseType { get; }

        public override string ToString() => $"protocol {Name}\n{{\n  Type: {Type};\n  Request: {RequestType};\n  Response: {ResponseType};\n}}";
    }
}
