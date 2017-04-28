using System;
using System.IO;
namespace Xamasoft.JsonClassGenerator.CodeWriters
{
    public class CSharpCodeWriter : ICodeWriter
    {
        private const string NoRenameAttribute = "[Obfuscation(Feature = \"renaming\", Exclude = true)]";
        private const string NoPruneAttribute = "[Obfuscation(Feature = \"trigger\", Exclude = false)]";
        public string FileExtension
        {
            get
            {
                return ".cs";
            }
        }
        public string DisplayName
        {
            get
            {
                return "C#";
            }
        }
        public string GetTypeName(JsonType type, IJsonClassGeneratorConfig config)
        {
            bool flag = !config.ExplicitDeserialization;
            switch (type.Type)
            {
                case JsonTypeEnum.Anything:
                    return "object";
                case JsonTypeEnum.String:
                    return "string";
                case JsonTypeEnum.Boolean:
                    return "bool";
                case JsonTypeEnum.Integer:
                    return "int";
                case JsonTypeEnum.Long:
                    return "long";
                case JsonTypeEnum.Float:
                    return "float";
                case JsonTypeEnum.Date:
                    return "DateTime";
                case JsonTypeEnum.NullableInteger:
                    return "int?";
                case JsonTypeEnum.NullableLong:
                    return "long?";
                case JsonTypeEnum.NullableFloat:
                    return "double?";
                case JsonTypeEnum.NullableBoolean:
                    return "bool?";
                case JsonTypeEnum.NullableDate:
                    return "DateTime?";
                case JsonTypeEnum.Object:
                    return type.AssignedName;
                case JsonTypeEnum.Array:
                    if (!flag)
                    {
                        return this.GetTypeName(type.InternalType, config) + "[]";
                    }
                  //  return "IList<" + this.GetTypeName(type.InternalType, config) + ">";
                    return "static Dictionary<string, " + this.GetTypeName(type.InternalType, config) + ">";
                case JsonTypeEnum.Dictionary:
                    return "Dictionary<string, " + this.GetTypeName(type.InternalType, config) + ">";
                case JsonTypeEnum.NullableSomething:
                    return "object";
                case JsonTypeEnum.NonConstrained:
                    return "object";
                default:
                    throw new NotSupportedException("Unsupported json type");
            }
        }
        private bool ShouldApplyNoRenamingAttribute(IJsonClassGeneratorConfig config)
        {
            return config.ApplyObfuscationAttributes && !config.ExplicitDeserialization && !config.UsePascalCase;
        }
        private bool ShouldApplyNoPruneAttribute(IJsonClassGeneratorConfig config)
        {
            return config.ApplyObfuscationAttributes && !config.ExplicitDeserialization && config.UseProperties;
        }
        public void WriteFileStart(IJsonClassGeneratorConfig config, TextWriter sw)
        {
            if (config.UseNamespaces)
            {
                string[] fileHeader = JsonClassGenerator.FileHeader;
                for (int i = 0; i < fileHeader.Length; i++)
                {
                    string str = fileHeader[i];
                    sw.WriteLine("// " + str);
                }
                sw.WriteLine();
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                if (this.ShouldApplyNoPruneAttribute(config) || this.ShouldApplyNoRenamingAttribute(config))
                {
                    sw.WriteLine("using System.Reflection;");
                }
                if (!config.ExplicitDeserialization && config.UsePascalCase)
                {
                    sw.WriteLine("using Newtonsoft.Json;");
                }
                //sw.WriteLine("using Newtonsoft.Json.Linq;");
                if (config.ExplicitDeserialization)
                {
                    sw.WriteLine("using JsonCSharpClassGenerator;");
                }
                if (config.SecondaryNamespace != null && config.HasSecondaryClasses && !config.UseNestedClasses)
                {
                    sw.WriteLine("using {0};", config.SecondaryNamespace);
                }
            }
            if (config.UseNestedClasses)
            {
                sw.WriteLine("    {0} class {1} ", config.InternalVisibility ? "internal" : "public", config.MainClass);
                sw.WriteLine("    {");
            }
        }
        public void WriteFileEnd(IJsonClassGeneratorConfig config, TextWriter sw)
        {
            if (config.UseNestedClasses)
            {
                sw.WriteLine("    }");
            }
        }
        public void WriteNamespaceStart(IJsonClassGeneratorConfig config, TextWriter sw, bool root)
        {
            sw.WriteLine();
            sw.WriteLine("namespace {0}", (root && !config.UseNestedClasses) ? config.Namespace : (config.SecondaryNamespace ?? config.Namespace));
            sw.WriteLine("{");
            sw.WriteLine();
        }
        public void WriteNamespaceEnd(IJsonClassGeneratorConfig config, TextWriter sw, bool root)
        {
            sw.WriteLine("}");
        }
        public void WriteClass(IJsonClassGeneratorConfig config, TextWriter sw, JsonType type)
        {
            string arg = config.InternalVisibility ? "internal" : "public";
            if (config.UseNestedClasses)
            {
                if (!type.IsRoot)
                {
                    if (this.ShouldApplyNoRenamingAttribute(config))
                    {
                        sw.WriteLine("        [Obfuscation(Feature = \"renaming\", Exclude = true)]");
                    }
                    if (this.ShouldApplyNoPruneAttribute(config))
                    {
                        sw.WriteLine("        [Obfuscation(Feature = \"trigger\", Exclude = false)]");
                    }
                    sw.WriteLine("        {0} class {1} ", arg, type.AssignedName);
                    sw.WriteLine("        {");
                }
            }
            else
            {
                if (this.ShouldApplyNoRenamingAttribute(config))
                {
                    sw.WriteLine("    [Obfuscation(Feature = \"renaming\", Exclude = true)]");
                }
                if (this.ShouldApplyNoPruneAttribute(config))
                {
                    sw.WriteLine("    [Obfuscation(Feature = \"trigger\", Exclude = false)]");
                }
                sw.WriteLine("    {0} class {1} ", arg, type.AssignedName);
                sw.WriteLine("    {");
            }
            string prefix = (config.UseNestedClasses && !type.IsRoot) ? "            " : "        ";
            bool flag = config.InternalVisibility && !config.UseProperties && !config.ExplicitDeserialization;
            if (flag)
            {
                sw.WriteLine("#pragma warning disable 0649");
                if (!config.UsePascalCase)
                {
                    sw.WriteLine();
                }
            }
            if (type.IsRoot && config.ExplicitDeserialization)
            {
                this.WriteStringConstructorExplicitDeserialization(config, sw, type, prefix);
            }
            if (config.ExplicitDeserialization)
            {
                if (config.UseProperties)
                {
                    this.WriteClassWithPropertiesExplicitDeserialization(sw, type, prefix);
                }
                else
                {
                    this.WriteClassWithFieldsExplicitDeserialization(sw, type, prefix);
                }
            }
            else
            {
                this.WriteClassMembers(config, sw, type, prefix);
            }
            if (flag)
            {
                sw.WriteLine();
                sw.WriteLine("#pragma warning restore 0649");
                sw.WriteLine();
            }
            if (config.UseNestedClasses && !type.IsRoot)
            {
                sw.WriteLine("        }");
            }
            if (!config.UseNestedClasses)
            {
                sw.WriteLine("    }");
            }
            sw.WriteLine();
        }
        private void WriteClassMembers(IJsonClassGeneratorConfig config, TextWriter sw, JsonType type, string prefix)
        {
            foreach (FieldInfo current in type.Fields)
            {
                if (config.UsePascalCase || config.ExamplesInDocumentation)
                {
                    sw.WriteLine();
                }
                if (config.ExamplesInDocumentation)
                {
                    sw.WriteLine(prefix + "/// <summary>");
                    sw.WriteLine(prefix + "/// Examples: " + current.GetExamplesText());
                    sw.WriteLine(prefix + "/// </summary>");
                }
                if (config.UsePascalCase)
                {
                    sw.WriteLine(prefix + "[JsonProperty(\"{0}\")]", current.JsonMemberName);
                }
                if (config.UseProperties)
                {
                    sw.WriteLine(prefix + "public {0} {1} {{ get; set; }}", current.Type.GetTypeName(), current.MemberName);
                }
                else
                {
                    sw.WriteLine(prefix + "public {0} {1};", current.Type.GetTypeName(), current.MemberName);
                }
            }
        }
        private void WriteClassWithPropertiesExplicitDeserialization(TextWriter sw, JsonType type, string prefix)
        {
            sw.WriteLine(prefix + "private JObject __jobject;");
            sw.WriteLine(prefix + "public {0}(JObject obj)", type.AssignedName);
            sw.WriteLine(prefix + "{");
            sw.WriteLine(prefix + "    this.__jobject = obj;");
            sw.WriteLine(prefix + "}");
            sw.WriteLine();
            foreach (FieldInfo current in type.Fields)
            {
                string text = null;
                if (current.Type.MustCache)
                {
                    text = "_" + char.ToLower(current.MemberName[0]) + current.MemberName.Substring(1);
                    sw.WriteLine(prefix + "[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]");
                    sw.WriteLine(prefix + "private {0} {1};", current.Type.GetTypeName(), text);
                }
                sw.WriteLine(prefix + "public {0} {1}", current.Type.GetTypeName(), current.MemberName);
                sw.WriteLine(prefix + "{");
                sw.WriteLine(prefix + "    get");
                sw.WriteLine(prefix + "    {");
                if (current.Type.MustCache)
                {
                    sw.WriteLine(prefix + "        if ({0} == null)", text);
                    sw.WriteLine(prefix + "            {0} = {1};", text, current.GetGenerationCode("__jobject"));
                    sw.WriteLine(prefix + "        return {0};", text);
                }
                else
                {
                    sw.WriteLine(prefix + "        return {0};", current.GetGenerationCode("__jobject"));
                }
                sw.WriteLine(prefix + "    }");
                sw.WriteLine(prefix + "}");
                sw.WriteLine();
            }
        }
        private void WriteStringConstructorExplicitDeserialization(IJsonClassGeneratorConfig config, TextWriter sw, JsonType type, string prefix)
        {
            sw.WriteLine();
            sw.WriteLine(prefix + "public {1}(string json)", config.InternalVisibility ? "internal" : "public", type.AssignedName);
            sw.WriteLine(prefix + "    : this(JObject.Parse(json))");
            sw.WriteLine(prefix + "{");
            sw.WriteLine(prefix + "}");
            sw.WriteLine();
        }
        private void WriteClassWithFieldsExplicitDeserialization(TextWriter sw, JsonType type, string prefix)
        {
            sw.WriteLine(prefix + "public {0}(JObject obj)", type.AssignedName);
            sw.WriteLine(prefix + "{");
            foreach (FieldInfo current in type.Fields)
            {
                sw.WriteLine(prefix + "    this.{0} = {1};", current.MemberName, current.GetGenerationCode("obj"));
            }
            sw.WriteLine(prefix + "}");
            sw.WriteLine();
            foreach (FieldInfo current2 in type.Fields)
            {
                sw.WriteLine(prefix + "public readonly {0} {1};", current2.Type.GetTypeName(), current2.MemberName);
            }
        }
    }
}
