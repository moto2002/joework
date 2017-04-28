

using System;

public class BitCryto
{
    private short[] crytoKey;
    private int offsetOfKey;

    public BitCryto(short[] sKey)
    {
        this.crytoKey = sKey;
    }

    public byte Decode(byte inputByte)
    {
        if (this.offsetOfKey >= this.crytoKey.Length)
        {
            this.offsetOfKey = 0;
        }
        short num = this.crytoKey[this.offsetOfKey];
        this.offsetOfKey++;
        short num2 = (short)(inputByte - num);
        if (num2 < 0)
        {
            num2 = (short)(num2 + 0x100);
        }
        return (byte)num2;
    }

    public byte Encode(byte inputByte)
    {
        if (this.offsetOfKey >= this.crytoKey.Length)
        {
            this.offsetOfKey = 0;
        }
        ushort num = (ushort)this.crytoKey[this.offsetOfKey];
        this.offsetOfKey++;
        return (byte)((num + inputByte) & 0xff);
    }

    public void Reset()
    {
        this.offsetOfKey = 0;
    }
}


