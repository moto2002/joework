namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class ControllerOfWeaponData : Mogo.GameData.GameData<ControllerOfWeaponData>
    {
        public static readonly string fileName = "xml/ControllerOfWeapon";

        public string controller { get; protected set; }

        public string controllerInCity { get; protected set; }
    }
}

