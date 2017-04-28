namespace Mogo.RPC
{
    using System;

    public enum LoginResult : byte
    {
        ACCOUNT_ILLEGAL = 9,
        FORBIDDEN_LOGIN = 3,
        INNER_ERR = 11,
        MULTILOGIN = 10,
        NO_SERVICE = 2,
        RET_ACCOUNT_PASSWD_NOMATCH = 1,
        SDK_VERIFY_FAILED = 8,
        SERVER_BUSY = 7,
        SIGN_ILLEGAL = 6,
        SUCCESS = 0,
        TIME_ILLEGAL = 5,
        TOO_MUCH = 4
    }
}

