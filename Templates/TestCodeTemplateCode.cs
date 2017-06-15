using TSLTestGenerator.DataModel;

namespace TSLTestGenerator.Templates
{
    public partial class TestCodeTemplate
    {
        public TestCodeGeneratorContext Context { get; }
        private ITSLTopLevelElement Element { get; }

        public TestCodeTemplate(TestCodeGeneratorContext context, ITSLTopLevelElement element)
        {
            Context = context;
            Element = element;
        }
    }
}