namespace TSLTestGenerator.DataModel
{
    public class TSLEnum : ITSLType, ITSLTopLevelElement
    {
        public string Name { get; }
        public bool DynamicLengthed => false;

        public TSLEnum(string name)
        {
            Name = name;
        }
    }
}
