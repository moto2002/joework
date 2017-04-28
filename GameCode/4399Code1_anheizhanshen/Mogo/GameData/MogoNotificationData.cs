namespace Mogo.GameData
{
    using System;
    using System.Runtime.CompilerServices;

    public class MogoNotificationData : Mogo.GameData.GameData<MogoNotificationData>
    {
        public static readonly string fileName = "xml/notification";

        public int content { get; protected set; }

        public string Content
        {
            get
            {
                return LanguageData.GetContent(this.content);
            }
        }

        public int tag { get; protected set; }

        public string time { get; protected set; }

        public int title { get; protected set; }

        public string Title
        {
            get
            {
                return LanguageData.GetContent(this.title);
            }
        }

        public int titleBanner { get; protected set; }

        public string TitleBanner
        {
            get
            {
                return LanguageData.GetContent(this.titleBanner);
            }
        }

        public int type { get; protected set; }
    }
}

