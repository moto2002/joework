
using System;
using System.CodeDom;
using System.Collections.Generic;

/// <summary>
/// 构造函数
/// </summary>
public class ItemConstruct
{
    private CodeConstructor construct;
    /// <summary>
    /// 构造函数
    /// </summary>
    public CodeConstructor Struct { get { return construct; } }

    /// <summary>
    /// 无参
    /// </summary>
    public ItemConstruct()
    {
        construct = new CodeConstructor();
        construct.Attributes = MemberAttributes.Public;
    }

    /// <summary>
    /// 有参
    /// </summary>
    /// <param name="inPars"></param>
    public ItemConstruct(List<string> inPars)
    {
        construct = new CodeConstructor();
        for (int i = 0; i < inPars.Count; i++)
        {
            construct.Parameters.Add(new CodeParameterDeclarationExpression(inPars[i], "inArg" + i));
        }
        construct.Attributes = MemberAttributes.Public;
    }
}