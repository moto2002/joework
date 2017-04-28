namespace Mogo.RPC
{
    using System;

    public enum DefCheckResult : byte
    {
        ENUM_LOGIN_CHECK_ENTITY_DEF_NOMATCH = 1,
        ENUM_LOGIN_CHECK_NO_SERVICE = 2,
        ENUM_LOGIN_CHECK_SUCCESS = 0
    }
}

