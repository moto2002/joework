/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：OpenFolderDialog
 * 简    述： 
 * 创建时间：2015/8/11 15:01:57
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;

namespace AutoCSharp
{
    public class OpenFolderDialog : System.Windows.Forms.IWin32Window
    {
        IntPtr _handle;
        public OpenFolderDialog(IntPtr handle)
        {
            _handle = handle;
        }

        IntPtr System.Windows.Forms.IWin32Window.Handle
        {
            get { return _handle; }
        }
    }
}
