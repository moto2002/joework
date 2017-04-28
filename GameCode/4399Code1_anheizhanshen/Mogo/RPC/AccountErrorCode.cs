namespace Mogo.RPC
{
    using System;

    public enum AccountErrorCode : short
    {
        CREATED = 11,
        GENDER = 0x11,
        NAME_BANNED = 0x10,
        NAME_EXISTS = 15,
        NAME_INVALID = 14,
        NAME_TOO_LONG = 13,
        NAME_TOO_SHORT = 12,
        TOO_MUCH = 0x13,
        VOCATION = 0x12
    }
}

