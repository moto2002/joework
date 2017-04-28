namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class LoginMarketData : Mogo.GameData.GameData<LoginMarketData>
    {
        public static readonly string fileName = "xml/LoginMarketData";

        public int itemId { get; protected set; }

        public int price { get; protected set; }

        public int priceType { get; protected set; }

        public int version { get; protected set; }
    }
}

