using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	public class CMail
	{
		public COM_MAIL_TYPE mailType;

		public byte subType;

		public int mailIndex;

		public COM_MAIL_STATE mailState;

		public bool autoDel;

		public string from;

		public string subject;

		public uint sendTime;

		public string mailContent;

		public string mailHyperlink;

		public ListView<CUseable> accessUseable = new ListView<CUseable>();

		public int accessUseableGeted;

		public bool isReceive;

		public bool isAccess;

		public ulong uid;

		public uint dwLogicWorldID;

		public byte relationType;

		public byte inviteType;

		public byte processType;

		public byte bMapType;

		public uint dwMapId;

		public uint dwGameSvrEntity;

		public int CanBeDeleted
		{
			get
			{
				int result = 0;
				if (this.mailState == 2)
				{
					if (this.accessUseable.get_Count() > 0)
					{
						for (int i = 0; i < this.accessUseable.get_Count(); i++)
						{
							if ((this.accessUseableGeted & 1 << i) == 1)
							{
								result = 10007;
								break;
							}
						}
					}
				}
				else
				{
					result = 10006;
				}
				return result;
			}
		}

		public CMail()
		{
		}

		public CMail(COM_MAIL_TYPE mailType, ref CSDT_GETMAIL_RES pkg)
		{
			this.mailType = mailType;
			this.subType = pkg.bMailType;
			this.mailIndex = pkg.iMailIndex;
			this.mailState = pkg.bMailState;
			this.autoDel = (pkg.bAutoDel > 0);
			this.from = Utility.UTF8Convert(pkg.szFrom);
			this.sendTime = pkg.dwSendTime;
			this.subject = Utility.UTF8Convert(pkg.szSubject, (int)pkg.bSubjectLen);
		}

		public void Read(CSDT_MAILOPTRES_READMAIL pkg)
		{
			this.isReceive = true;
			this.accessUseable.Clear();
			this.mailState = pkg.bMailState;
			this.ParseContentAndHyperlink(pkg.szContent, (int)pkg.wContentLen, ref this.mailContent, ref this.mailHyperlink);
			this.accessUseableGeted = 0;
			this.accessUseable = CMailSys.StAccessToUseable(pkg.astAccess, null, (int)pkg.bAccessCnt);
			for (int i = 0; i < this.accessUseable.get_Count(); i++)
			{
				if (pkg.astAccess[i].bGeted == 1)
				{
					this.accessUseableGeted |= 1 << i;
				}
			}
		}

		public void ParseContentAndHyperlink(sbyte[] srcContent, int srcContentLength, ref string content, ref string hyperlink)
		{
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < srcContentLength; i++)
			{
				if (srcContent[i].Equals(91))
				{
					num = i;
				}
				else if (srcContent[i].Equals(93))
				{
					num2 = i;
				}
			}
			if (0 < num && num < num2)
			{
				sbyte[] array = new sbyte[num];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = srcContent[j];
				}
				content = Utility.UTF8Convert(array, array.Length);
				sbyte[] array2 = new sbyte[num2 - num + 1 - 2];
				for (int k = 0; k < array2.Length; k++)
				{
					array2[k] = srcContent[num + 1 + k];
				}
				hyperlink = Utility.UTF8Convert(array2, array2.Length);
			}
			if (!CHyperLink.IsStandCommond(hyperlink))
			{
				content = Utility.UTF8Convert(srcContent, srcContentLength);
				hyperlink = null;
			}
		}
	}
}
