﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 14.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Genie.Templates.Dapper
{
    using Genie.Base;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "F:\Projects\Genie\Genie\Templates\Dapper\SimpleMemberMap.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class SimpleMemberMap : SimpleMemberMapBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("using System;\r\nusing System.Reflection;\r\n\r\nnamespace ");
            
            #line 6 "F:\Projects\Genie\Genie\Templates\Dapper\SimpleMemberMap.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GenerationContext.BaseNamespace));
            
            #line default
            #line hidden
            this.Write(".Dapper\r\n{\r\n\t/// <summary>\r\n    /// Represents simple memeber map for one of targ" +
                    "et parameter or property or field to source DataReader column\r\n    /// </summary" +
                    ">\r\n    sealed partial class SimpleMemberMap : SqlMapper.IMemberMap\r\n    {\r\n     " +
                    "   private readonly string _columnName;\r\n        private readonly PropertyInfo _" +
                    "property;\r\n        private readonly FieldInfo _field;\r\n        private readonly " +
                    "ParameterInfo _parameter;\r\n\r\n        /// <summary>\r\n        /// Creates instance" +
                    " for simple property mapping\r\n        /// </summary>\r\n        /// <param name=\"c" +
                    "olumnName\">DataReader column name</param>\r\n        /// <param name=\"property\">Ta" +
                    "rget property</param>\r\n        public SimpleMemberMap(string columnName, Propert" +
                    "yInfo property)\r\n        {\r\n            if (columnName == null)\r\n               " +
                    " throw new ArgumentNullException(\"columnName\");\r\n\r\n            if (property == n" +
                    "ull)\r\n                throw new ArgumentNullException(\"property\");\r\n\r\n          " +
                    "  _columnName = columnName;\r\n            _property = property;\r\n        }\r\n\r\n   " +
                    "     /// <summary>\r\n        /// Creates instance for simple field mapping\r\n     " +
                    "   /// </summary>\r\n        /// <param name=\"columnName\">DataReader column name</" +
                    "param>\r\n        /// <param name=\"field\">Target property</param>\r\n        public " +
                    "SimpleMemberMap(string columnName, FieldInfo field)\r\n        {\r\n            if (" +
                    "columnName == null)\r\n                throw new ArgumentNullException(\"columnName" +
                    "\");\r\n\r\n            if (field == null)\r\n                throw new ArgumentNullExc" +
                    "eption(\"field\");\r\n\r\n            _columnName = columnName;\r\n            _field = " +
                    "field;\r\n        }\r\n\r\n        /// <summary>\r\n        /// Creates instance for sim" +
                    "ple constructor parameter mapping\r\n        /// </summary>\r\n        /// <param na" +
                    "me=\"columnName\">DataReader column name</param>\r\n        /// <param name=\"paramet" +
                    "er\">Target constructor parameter</param>\r\n        public SimpleMemberMap(string " +
                    "columnName, ParameterInfo parameter)\r\n        {\r\n            if (columnName == n" +
                    "ull)\r\n                throw new ArgumentNullException(\"columnName\");\r\n\r\n        " +
                    "    if (parameter == null)\r\n                throw new ArgumentNullException(\"par" +
                    "ameter\");\r\n\r\n            _columnName = columnName;\r\n            _parameter = par" +
                    "ameter;\r\n        }\r\n\r\n        /// <summary>\r\n        /// DataReader column name\r" +
                    "\n        /// </summary>\r\n        public string ColumnName\r\n        {\r\n          " +
                    "  get { return _columnName; }\r\n        }\r\n\r\n        /// <summary>\r\n        /// T" +
                    "arget member type\r\n        /// </summary>\r\n        public Type MemberType\r\n     " +
                    "   {\r\n            get\r\n            {\r\n                if (_field != null)\r\n     " +
                    "               return _field.FieldType;\r\n\r\n                if (_property != null" +
                    ")\r\n                    return _property.PropertyType;\r\n\r\n                if (_pa" +
                    "rameter != null)\r\n                    return _parameter.ParameterType;\r\n\r\n      " +
                    "          return null;\r\n            }\r\n        }\r\n\r\n        /// <summary>\r\n     " +
                    "   /// Target property\r\n        /// </summary>\r\n        public PropertyInfo Prop" +
                    "erty\r\n        {\r\n            get { return _property; }\r\n        }\r\n\r\n        ///" +
                    " <summary>\r\n        /// Target field\r\n        /// </summary>\r\n        public Fie" +
                    "ldInfo Field\r\n        {\r\n            get { return _field; }\r\n        }\r\n\r\n      " +
                    "  /// <summary>\r\n        /// Target constructor parameter\r\n        /// </summary" +
                    ">\r\n        public ParameterInfo Parameter\r\n        {\r\n            get { return _" +
                    "parameter; }\r\n        }\r\n    }\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public class SimpleMemberMapBase
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