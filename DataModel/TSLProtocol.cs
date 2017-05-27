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
        public TSLProtocol(string name, TSLProtocolType type, TSLType requestType, TSLType responseType)
        {
            Name = name;
            Type = type;
            RequestType = requestType;
            ResponseType = responseType;
        }

        public string Name { get; private set; }
        public TSLProtocolType Type { get; private set; }
        public TSLType RequestType { get; private set; }
        public TSLType ResponseType { get; private set; }

        public override string ToString() => $"protocol {Name}\n{{ Type: {Type}; Request: {RequestType}; Response: {ResponseType} }}";
    }
}
