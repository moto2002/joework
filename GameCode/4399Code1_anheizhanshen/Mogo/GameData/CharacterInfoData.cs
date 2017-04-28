namespace Mogo.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CharacterInfoData : Mogo.GameData.GameData<CharacterInfoData>
    {
        public static readonly string fileName = "xml/CharacterInfoData";

        public byte attack { get; protected set; }

        public string controller { get; set; }

        public byte defence { get; protected set; }

        public int discription { get; protected set; }

        public List<int> EquipList { get; protected set; }

        public Vector3 Location { get; protected set; }

        public byte range { get; protected set; }

        public int vocation { get; protected set; }

        public int Weapon { get; protected set; }
    }
}

