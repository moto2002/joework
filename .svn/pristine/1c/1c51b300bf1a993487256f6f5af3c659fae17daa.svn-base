namespace Mogo.Util
{
    using System;
    using System.Runtime.CompilerServices;

    public class ServerInfo
    {
        public bool CanLogin()
        {
            return ((this.flag != 3) || (this.flag == 4));
        }

        public string GetInfo()
        {
            if ((this.text != null) && (this.text != string.Empty))
            {
                return this.text;
            }
            if (this.flag == 3)
            {
                return "server close";
            }
            if (this.flag == 4)
            {
                return "maintaining...";
            }
            return string.Empty;
        }

        public int flag { get; set; }

        public int id { get; set; }

        public string ip { get; set; }

        public string name { get; set; }

        public int port { get; set; }

        public string text { get; set; }
    }
}

