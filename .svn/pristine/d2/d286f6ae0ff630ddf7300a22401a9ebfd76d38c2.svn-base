
using System;
using System.CodeDom;
/// <summary>
/// 字段
/// <para>一般为私有eg. private int age</para>
/// </summary>
public class ItemField
{
    private CodeMemberField field;
    /// <summary>
    /// 字段
    /// </summary>
    public CodeMemberField Field { get { return field; } }

    /// <summary>
    /// 字段
    /// </summary>
    /// <param name="inName">字段名</param>
    /// <param name="inType">字段类型</param>
    /// <param name="inAtt">访问修饰符</param>
    public ItemField(string inName, string inType, MemberAttributes inAtt)
    {
        field = new CodeMemberField(Stringer.ToType(inType), "m_" + Stringer.FirstLetterLower(inName));
        field.Attributes = inAtt;
    }

    /// <summary>
    /// 字段
    /// <para>eg. public Dictionary<string,string> TestDic = new Dictionary<string,string>();</para>
    /// </summary>
    /// <param name="inLeft">字段类型</param>
    /// <param name="inFieldName"></param>
    /// <param name="inRight"></param>
    public ItemField(string inLeft, string inFieldName, string inRight = "", MemberAttributes inAtt = MemberAttributes.Private)
    {
        field = new CodeMemberField(inLeft, inFieldName);
        if (inRight != "")
        {
            CodeVariableReferenceExpression right = new CodeVariableReferenceExpression(inRight);
            field.InitExpression = right;
        }
        field.Attributes = inAtt;
    }

    /// <summary>
    /// 访问修饰符
    /// </summary>
    /// <param name="inAtt"></param>
    public void SetAttributes(MemberAttributes inAtt)
    {
        field.Attributes = inAtt;
    }

    /// <summary>
    /// 自定义特性
    /// <para>eg. [ProtoMember(1)]</para>
    /// </summary>
    /// <param name="inKey"></param>
    /// <param name="inValue"></param>
    public void AddAttributes(string inKey, object inValue)
    {
        field.CustomAttributes.Add(new CodeAttributeDeclaration(inKey, new CodeAttributeArgument(new CodePrimitiveExpression(inValue))));
    }
}