using System;
using System.Collections.Generic;
using System.Text;

public class VersionCodeInfo
{
    private List<int> m_tags = new List<int>();

    public VersionCodeInfo(string version)
    {
        if (!string.IsNullOrEmpty(version))
        {
            string[] strArray = version.Split(new char[] { '.' });
            for (int i = 0; i < strArray.Length; i++)
            {
                int num2;
                if (int.TryParse(strArray[i], out num2))
                {
                    this.m_tags.Add(num2);
                }
                else
                {
                    this.m_tags.Add(num2);
                }
            }
        }
    }

    public int Compare(VersionCodeInfo info)
    {
        int num = (this.m_tags.Count < info.m_tags.Count) ? this.m_tags.Count : info.m_tags.Count;
        for (int i = 0; i < num; i++)
        {
            if (this.m_tags[i] != info.m_tags[i])
            {
                return ((this.m_tags[i] > info.m_tags[i]) ? 1 : -1);
            }
        }
        return 0;
    }

    public string GetLowerVersion()
    {
        int num = this.m_tags[this.m_tags.Count - 1] - 1;
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < (this.m_tags.Count - 1); i++)
        {
            builder.AppendFormat("{0}.", this.m_tags[i]);
        }
        builder.Append(num);
        return builder.ToString();
    }

    public string GetUpperVersion()
    {
        int num = this.m_tags[this.m_tags.Count - 1] + 1;
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < (this.m_tags.Count - 1); i++)
        {
            builder.AppendFormat("{0}.", this.m_tags[i]);
        }
        builder.Append(num);
        return builder.ToString();
    }

    public string ToShortString()
    {
        StringBuilder builder = new StringBuilder();
        foreach (int num in this.m_tags)
        {
            builder.Append(num);
        }
        return builder.ToString();
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        foreach (int num in this.m_tags)
        {
            builder.AppendFormat("{0}.", num);
        }
        builder.Remove(builder.Length - 1, 1);
        return builder.ToString();
    }
}

