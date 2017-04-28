namespace Mogo.Util
{
    using System;

    public enum LayerMask
    {
        Character = 0x100,
        Default = 1,
        Monster = 0x800,
        Npc = 0x1000,
        Terrain = 0x200,
        Trap = 0x20000
    }
}

