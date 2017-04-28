
using System;
using System.CodeDom;

public class ItemProperty
{
    private CodeMemberProperty property;
    /// <summary>
    /// 属性
    /// </summary>
    public CodeMemberProperty Property { get { return property; } }

    public ItemProperty(string inName)
    {
        property = new CodeMemberProperty();
        property.HasGet = false;
        property.HasSet = false;
        property.Name = Stringer.FirstLetterUp(inName);
    }

    /// <summary>
    /// 属性类型
    /// </summary>
    /// <param name="inValue"></param>
    /// <param name="hasGet"></param>
    /// <param name="hasSet"></param>
    public void SetValueType(string inValueType = "")
    {
        property.Type = new CodeTypeReference(Stringer.ToType(inValueType));
    }

    /// <summary>
    /// Get属性
    /// </summary>
    /// <param name="inName"></param>
    public void SetGetName(string inName = "")
    {
        string s = inName == "" ? property.Name : inName;
        property.HasGet = true;
        property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "m_" + Stringer.FirstLetterLower(s))));
    }

    /// <summary>
    /// Set属性
    /// </summary>
    /// <param name="inName"></param>
    public void SetSetName(string inName = "")
    {
        string s = inName == "" ? property.Name : inName;
        property.HasSet = true;
        property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "m_" + Stringer.FirstLetterLower(s)), new CodePropertySetValueReferenceExpression()));
    }

    /// <summary>
    /// 访问修饰符
    /// </summary>
    public void SetModifier(MemberAttributes inAtt)
    {
        property.Attributes = inAtt;
    }

    /// <summary>
    /// 特性声明
    /// <para>eg. [Serializable]</para>
    /// </summary>
    /// <param name="inName">特性名</param>
    /// <param name="inValue">特性值</param>
    public void SetField(string inName, string inValue)
    {
        if (Stringer.IsNumber(inValue))
        {
            property.CustomAttributes.Add(new CodeAttributeDeclaration(inName, new CodeAttributeArgument(new CodePrimitiveExpression(int.Parse(inValue)))));
        }
    }

    /// <summary>
    /// 注释
    /// </summary>
    public void SetComment(string s)
    {
        property.Comments.Add(new CodeCommentStatement("<summary>", true));
        property.Comments.Add(new CodeCommentStatement(s, true));
        property.Comments.Add(new CodeCommentStatement("</summary>", true));
    }
}