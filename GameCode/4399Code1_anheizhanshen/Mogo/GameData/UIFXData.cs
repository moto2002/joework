namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class UIFXData : Mogo.GameData.GameData<UIFXData>
    {
        public static readonly string fileName = "xml/UIFX";

        public string attachedWidget { get; protected set; }

        public float duration { get; protected set; }

        public float fadetime { get; protected set; }

        public string fxPrefab { get; protected set; }

        public string goName { get; protected set; }

        public int logicType { get; protected set; }

        public int programType { get; protected set; }

        public int renderType { get; protected set; }
    }
}

