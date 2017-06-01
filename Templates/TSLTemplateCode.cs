using TSLTestGenerator.DataModel;

namespace TSLTestGenerator.Templates
{
    public partial class TSLTemplate
    {
        public TSLScript Script { get; }

        public TSLTemplate(TSLScript script)
        {
            Script = script;
        }
    }
}