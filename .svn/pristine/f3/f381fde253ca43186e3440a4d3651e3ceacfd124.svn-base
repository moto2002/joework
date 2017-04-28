namespace Mogo.Util
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LocalSetting
    {
        private int m_synCreateRequestCount = 5;

        public LocalSetting()
        {
            this.PassportSeenNotice = "";
            this.noticeMd5 = "";
            this.Passport = "";
            this.Password = "";
            this.IsDragMove = true;
            this.PlayerCountInScreen = 10;
            this.SoundVolume = 0.5f;
            this.MusicVolume = 0.3f;
            this.GuideTimes = new Dictionary<ulong, string>();
            this.IsShowGoldMetallurgyTipDialog = true;
            this.GoldMetallurgyTipDialogDisableDay = 0;
            this.IsShowBuyEnergyTipDialog = true;
            this.BuyEnergyTipDialogDisableDay = 0;
            this.IsShowArenaRevengeTipDialog = true;
            this.ArenaRevengeTipDialogDisableDay = 0;
            this.IsShowUpgradePowerTipDialog = true;
            this.UpgradePowerTipDialogDisableDay = 0;
            this.IsShowUpgradeDragonTipDialog = true;
            this.UpgradeDragonTipDialogDisableDay = 0;
            this.SelectedCharacter = "_0";
            this.GraphicQuality = 2;
        }

        public int ArenaRevengeTipDialogDisableDay { get; set; }

        public int BuyEnergyTipDialogDisableDay { get; set; }

        public int GoldMetallurgyTipDialogDisableDay { get; set; }

        public int GraphicQuality { get; set; }

        public Dictionary<ulong, string> GuideTimes { get; set; }

        public bool HasUploadInfo { get; set; }

        public int id { get; set; }

        public bool IsAutoLogin { get; set; }

        public bool IsDragMove { get; set; }

        public bool IsSavePassport { get; set; }

        public bool IsShowArenaRevengeTipDialog { get; set; }

        public bool IsShowBuyEnergyTipDialog { get; set; }

        public bool IsShowGoldMetallurgyTipDialog { get; set; }

        public bool IsShowUpgradeDragonTipDialog { get; set; }

        public bool IsShowUpgradePowerTipDialog { get; set; }

        public int LastMap { get; set; }

        public float MusicVolume { get; set; }

        public string noticeMd5 { get; set; }

        public string Passport { get; set; }

        public string PassportSeenNotice { get; set; }

        public string Password { get; set; }

        public int PlayerCountInScreen { get; set; }

        public string SelectedCharacter { get; set; }

        public int SelectedServer { get; set; }

        public float SoundVolume { get; set; }

        public int SynCreateRequestCount
        {
            get
            {
                return this.m_synCreateRequestCount;
            }
            set
            {
                if (value > 0)
                {
                    this.m_synCreateRequestCount = value;
                }
            }
        }

        public int UpgradeDragonTipDialogDisableDay { get; set; }

        public int UpgradePowerTipDialogDisableDay { get; set; }
    }
}

