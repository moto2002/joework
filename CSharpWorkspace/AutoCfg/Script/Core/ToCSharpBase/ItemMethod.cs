
using System;
using System.CodeDom;
using System.Collections.Generic;

public class ItemMethod
{
    private CodeMemberMethod method;
    public CodeMemberMethod Method { get { return method; } }

    protected internal CodeConditionStatement statement;

    public ItemMethod(string inName, MemberAttributes inAtt, List<string> inParameters)
    {
        method = new CodeMemberMethod();
        method.Name = inName;
        method.Attributes = inAtt;
        if (inParameters != null)
        {
            for (int i = 0; i < inParameters.Count; i++)
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(inParameters[i], "inArg" + i));
            }
        }

        statement = new CodeConditionStatement();
    }

    public void SetReturn(string s)
    {
        method.ReturnType = new CodeTypeReference(s);
    }
}