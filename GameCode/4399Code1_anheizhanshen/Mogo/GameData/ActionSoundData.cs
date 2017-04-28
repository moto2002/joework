namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ActionSoundData : Mogo.GameData.GameData<ActionSoundData>
    {
        public static readonly string fileName = "xml/ActionSound";

        public int action { get; set; }

        public Dictionary<int, int> sound { get; set; }

        public int vocation { get; set; }
    }
}

