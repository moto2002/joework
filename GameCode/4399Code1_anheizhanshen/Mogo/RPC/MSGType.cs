namespace Mogo.RPC
{
    using System;

    public enum MSGType : ushort
    {
        BASEAPP = 0x6000,
        BASEAPPMGR = 0x2000,
        CELLAPP = 0x7000,
        DBMGR = 0x3000,
        LOGINAPP = 0x1000
    }
}

