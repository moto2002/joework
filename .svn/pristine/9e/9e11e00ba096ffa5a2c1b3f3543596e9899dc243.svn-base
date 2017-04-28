using Mogo.Util;
using System;

public abstract class DataLoader
{
    protected readonly string m_fileExtention;
    protected Action m_finished;
    protected static readonly bool m_isPreloadData = true;
    protected readonly bool m_isUseOutterConfig = SystemConfig.IsUseOutterConfig;
    protected Action<int, int> m_progress;
    protected readonly string m_resourcePath;

    protected DataLoader()
    {
        if (this.m_isUseOutterConfig)
        {
            this.m_resourcePath = SystemConfig.OutterPath + "data/";
            this.m_fileExtention = ".xml";
        }
        else
        {
            this.m_resourcePath = "data/";
            this.m_fileExtention = SystemConfig.CONFIG_FILE_EXTENSION;
        }
    }
}

