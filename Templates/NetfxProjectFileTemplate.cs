﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace TSLTestGenerator.Templates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\v-siyul.FAREAST\documents\visual studio 2017\Projects\TSLTestGenerator\TSLTestGenerator\Templates\NetfxProjectFileTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class NetfxProjectFileTemplate : NetfxProjectFileTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""..\..\..\packages\GraphEngine.Core.1.0.8850\build\GraphEngine.Core.props"" Condition=""Exists('..\..\..\packages\GraphEngine.Core.1.0.8850\build\GraphEngine.Core.props')"" />
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{0AE97FA8-E63E-48EA-8336-529FE0A9DBC0}</ProjectGuid>
    <OutputType>Library</OutputType>
    <RootNamespace>");
            
            #line 15 "C:\Users\v-siyul.FAREAST\documents\visual studio 2017\Projects\TSLTestGenerator\TSLTestGenerator\Templates\NetfxProjectFileTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TestName));
            
            #line default
            #line hidden
            this.Write("</RootNamespace>\r\n    <AssemblyName>");
            
            #line 16 "C:\Users\v-siyul.FAREAST\documents\visual studio 2017\Projects\TSLTestGenerator\TSLTestGenerator\Templates\NetfxProjectFileTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TestName));
            
            #line default
            #line hidden
            this.Write("</AssemblyName>\r\n    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>\r\n   " +
                    " <FileAlignment>512</FileAlignment>\r\n    <NuGetPackageImportStamp>\r\n    </NuGetP" +
                    "ackageImportStamp>\r\n  </PropertyGroup>\r\n  <PropertyGroup Condition=\" \'$(Configur" +
                    "ation)|$(Platform)\' == \'Debug|AnyCPU\' \">\r\n    <DebugSymbols>true</DebugSymbols>\r" +
                    "\n    <DebugType>full</DebugType>\r\n    <Optimize>false</Optimize>\r\n    <OutputPat" +
                    "h>bin\\Debug\\</OutputPath>\r\n    <DefineConstants>DEBUG;TRACE</DefineConstants>\r\n " +
                    "   <ErrorReport>prompt</ErrorReport>\r\n    <WarningLevel>4</WarningLevel>\r\n  </Pr" +
                    "opertyGroup>\r\n  <PropertyGroup Condition=\" \'$(Configuration)|$(Platform)\' == \'Re" +
                    "lease|AnyCPU\' \">\r\n    <DebugType>pdbonly</DebugType>\r\n    <Optimize>true</Optimi" +
                    "ze>\r\n    <OutputPath>bin\\Release\\</OutputPath>\r\n    <DefineConstants>TRACE</Defi" +
                    "neConstants>\r\n    <ErrorReport>prompt</ErrorReport>\r\n    <WarningLevel>4</Warnin" +
                    "gLevel>\r\n  </PropertyGroup>\r\n  <ItemGroup>\r\n    <Reference Include=\"Newtonsoft.J" +
                    "son, Version=6.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed, processo" +
                    "rArchitecture=MSIL\">\r\n      <HintPath>..\\..\\..\\packages\\Newtonsoft.Json.6.0.8\\li" +
                    "b\\net45\\Newtonsoft.Json.dll</HintPath>\r\n      <Private>True</Private>\r\n    </Ref" +
                    "erence>\r\n    <Reference Include=\"System\" />\r\n    <Reference Include=\"System.Core" +
                    "\" />\r\n    <Reference Include=\"System.Xml.Linq\" />\r\n    <Reference Include=\"Syste" +
                    "m.Data.DataSetExtensions\" />\r\n    <Reference Include=\"Microsoft.CSharp\" />\r\n    " +
                    "<Reference Include=\"System.Data\" />\r\n    <Reference Include=\"System.Net.Http\" />" +
                    "\r\n    <Reference Include=\"System.Xml\" />\r\n    <Reference Include=\"Trinity.Core, " +
                    "Version=1.0.8850.0, Culture=neutral, processorArchitecture=MSIL\">\r\n      <HintPa" +
                    "th>..\\..\\..\\packages\\GraphEngine.Core.1.0.8850\\lib\\Trinity.Core.dll</HintPath>\r\n" +
                    "      <Private>True</Private>\r\n    </Reference>\r\n    <Reference Include=\"nunit.f" +
                    "ramework, Version=3.6.1.0, Culture=neutral, PublicKeyToken=2638cd05610744eb, pro" +
                    "cessorArchitecture=MSIL\">\r\n      <HintPath>..\\..\\..\\packages\\NUnit.3.6.1\\lib\\net" +
                    "45\\nunit.framework.dll</HintPath>\r\n    </Reference>\r\n  </ItemGroup>\r\n  <ItemGrou" +
                    "p>\r\n");
            
            #line 61 "C:\Users\v-siyul.FAREAST\documents\visual studio 2017\Projects\TSLTestGenerator\TSLTestGenerator\Templates\NetfxProjectFileTemplate.tt"

    foreach (var csfile in CsFileList)
    {

            
            #line default
            #line hidden
            this.Write("    <Compile Include=\"");
            
            #line 65 "C:\Users\v-siyul.FAREAST\documents\visual studio 2017\Projects\TSLTestGenerator\TSLTestGenerator\Templates\NetfxProjectFileTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(csfile));
            
            #line default
            #line hidden
            this.Write("\" />\r\n");
            
            #line 66 "C:\Users\v-siyul.FAREAST\documents\visual studio 2017\Projects\TSLTestGenerator\TSLTestGenerator\Templates\NetfxProjectFileTemplate.tt"

    }

            
            #line default
            #line hidden
            this.Write("    <Compile Include=\"Utils.cs\" />\r\n  </ItemGroup>\r\n  <ItemGroup>\r\n    <None Incl" +
                    "ude=\"packages.config\" />\r\n    <TslCodegen Include=\"");
            
            #line 73 "C:\Users\v-siyul.FAREAST\documents\visual studio 2017\Projects\TSLTestGenerator\TSLTestGenerator\Templates\NetfxProjectFileTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TestName));
            
            #line default
            #line hidden
            this.Write(@".tsl"" />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <Target Name=""EnsureNuGetPackageBuildImports"" BeforeTargets=""PrepareForBuild"">
    <PropertyGroup>
      <ErrorText>This project references NuGet package(s) that are missing on this computer. Use NuGet Package Restore to download them.  For more information, see http://go.microsoft.com/fwlink/?LinkID=322105. The missing file is {0}.</ErrorText>
    </PropertyGroup>
    <Error Condition=""!Exists('..\..\..\packages\GraphEngine.Core.1.0.8850\build\GraphEngine.Core.props')"" Text=""$([System.String]::Format('$(ErrorText)', '..\..\..\packages\GraphEngine.Core.1.0.8850\build\GraphEngine.Core.props'))"" />
    <Error Condition=""!Exists('..\..\..\packages\GraphEngine.Core.1.0.8850\build\GraphEngine.Core.targets')"" Text=""$([System.String]::Format('$(ErrorText)', '..\..\..\packages\GraphEngine.Core.1.0.8850\build\GraphEngine.Core.targets'))"" />
  </Target>
  <Import Project=""..\..\..\packages\GraphEngine.Core.1.0.8850\build\GraphEngine.Core.targets"" Condition=""Exists('..\..\..\packages\GraphEngine.Core.1.0.8850\build\GraphEngine.Core.targets')"" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  -->
</Project>

");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 93 "C:\Users\v-siyul.FAREAST\documents\visual studio 2017\Projects\TSLTestGenerator\TSLTestGenerator\Templates\NetfxProjectFileTemplate.tt"

    public string TestName { get; }
    public string[] CsFileList { get; }

    public NetfxProjectFileTemplate(TestCodeGeneratorContext context, IEnumerable<string> csFileList)
    {
        TestName = context.TestName;
        CsFileList = csFileList.ToArray();
    }

        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public class NetfxProjectFileTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
