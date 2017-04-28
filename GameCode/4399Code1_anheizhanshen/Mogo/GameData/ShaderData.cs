namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class ShaderData : Mogo.GameData.GameData<ShaderData>
    {
        public static readonly string fileName = "xml/ShaderData";

        public string color { get; protected set; }

        public string name { get; protected set; }
    }
}

