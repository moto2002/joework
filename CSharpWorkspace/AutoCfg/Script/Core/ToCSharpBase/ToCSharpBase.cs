
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
public class ToCSharpBase
{
    protected internal CodeTypeDeclaration classer;

    /// <summary>
    /// 命名空间列表
    /// </summary>
    protected internal List<string> usingList = new List<string>();

    /// <summary>
    /// 类继承列表
    /// </summary>
    protected internal List<string> ineritList = new List<string>();

    /// <summary>
    /// 构造函数列表
    /// </summary>
    protected internal List<ItemConstruct> constructList = new List<ItemConstruct>();

    /// <summary>
    /// 字段列表(一般为私有)
    /// </summary>
    protected internal List<ItemField> fieldList = new List<ItemField>();

    /// <summary>
    /// 属性列表
    /// </summary>
    protected internal List<ItemProperty> propertyList = new List<ItemProperty>();

    /// <summary>
    /// 方法列表
    /// </summary>
    protected internal List<ItemMethod> methodList = new List<ItemMethod>();

    protected string spaceName;
    protected internal string className;
    protected string folderName;

    public ToCSharpBase(string inSpace, string inClassName, string inFolderName)
    {
        spaceName = inSpace.Trim();
        className = Stringer.FirstLetterUp(inClassName);
        folderName = inFolderName;
        classer = new CodeTypeDeclaration(className);
        classer.IsClass = true;
    }

    internal virtual void OnCreate() { }

    public void Create()
    {
        OnCreate();

        CodeCompileUnit unit = new CodeCompileUnit();

        // 命名空间
        CodeNamespace nameSpace = new CodeNamespace(spaceName);

        // 引用
        usingList.ForEach(i => nameSpace.Imports.Add(new CodeNamespaceImport(i)));

        // 类的访问限制符
        classer.TypeAttributes = System.Reflection.TypeAttributes.Public;

        // 类的继承列表
        if (ineritList.Count > 0)
            ineritList.ForEach(i => classer.BaseTypes.Add(new CodeTypeReference(i)));

        nameSpace.Types.Add(classer);
        unit.Namespaces.Add(nameSpace);

        // 构造函数
        constructList.ForEach(i => classer.Members.Add(i.Struct));

        // 字段
        fieldList.ForEach(i => classer.Members.Add(i.Field));

        // 属性
        propertyList.ForEach(i => classer.Members.Add(i.Property));

        // 方法列表
        methodList.ForEach(i => classer.Members.Add(i.Method));

        // 创建.cs文件
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        options.BracingStyle = "C";
        options.BlankLinesBetweenMembers = true;

        using (System.IO.StreamWriter sw = new StreamWriter((folderName == "" ? "" : folderName + "/") + className + ".cs"))
        {
            provider.GenerateCodeFromCompileUnit(unit, sw, options);
        }
    }

    /// <summary>
    /// 类继承(界面输入)
    /// <para>以 "," 分开</para>
    /// </summary>
    /// <param name="inStr"></param>
    public void SetInherit(string inStr)
    {
        if (inStr.Trim() != "")
        {
            string[] l = inStr.Split(',');
            for (int i = 0; i < l.Length; i++)
            {
                ineritList.Add(l[i].Trim());
            }
        }
    }

    /// <summary>
    /// 增加注释
    /// </summary>
    /// <param name="s"></param>
    /// <param name="inTarget"></param>
    public void SetComment(string s, CodeTypeMember inTarget)
    {
        inTarget.Comments.Add(new CodeCommentStatement("<summary>", true));
        inTarget.Comments.Add(new CodeCommentStatement(s, true));
        inTarget.Comments.Add(new CodeCommentStatement("</summary>", true));
    }

    /// <summary>
    /// 增加语句
    /// <para>eg. string[] ss = inArg0.Split('^')</para>
    /// <para>eg. _key = unit.Parse(ss[i]);</para>
    /// </summary>
    public CodeStatement Line(string inLeft, string inRight)
    {
        return new CodeAssignStatement(new CodeVariableReferenceExpression(inLeft), new CodeVariableReferenceExpression(inRight));
    }

    /// <summary>
    /// 条件语句
    /// </summary>
    public void AddConditionStatement(CodeConditionStatement inStatement)
    {
        methodList[0].Method.Statements.Add(inStatement);
    }

    /// <summary>
    /// For 循环
    /// </summary>
    /// <param name="inType">循环的类型 eg. typeof(int)</param>
    /// <param name="inTypeName">循环变量 eg. i</param>
    /// <param name="inTypeValue">循环变量的起始值</param>
    /// <param name="inLessthan">循环的极值</param>
    /// <returns></returns>
    public CodeIterationStatement AddFor(Type inType, string inTypeName, object inTypeValue, string inLessthan)
    {
        CodeIterationStatement curFor = new CodeIterationStatement();
        curFor.InitStatement = new CodeVariableDeclarationStatement(inType, inTypeName, new CodePrimitiveExpression(inTypeValue));
        curFor.TestExpression = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(inTypeName), CodeBinaryOperatorType.LessThan, new CodeVariableReferenceExpression(inLessthan));
        curFor.IncrementStatement = new CodeAssignStatement(new CodeVariableReferenceExpression(inTypeName), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(inTypeName), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));
        return curFor;
    }
}