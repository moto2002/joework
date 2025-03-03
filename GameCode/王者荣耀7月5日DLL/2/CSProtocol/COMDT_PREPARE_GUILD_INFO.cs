using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_PREPARE_GUILD_INFO : ProtocolObject
	{
		public COMDT_PREPARE_GUILD_BRIEF_INFO stBriefInfo;

		public COMDT_GUILD_MEMBER_BRIEF_INFO[] astMemberInfo;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 153u;

		public static readonly int CLASS_ID = 363;

		public COMDT_PREPARE_GUILD_INFO()
		{
			this.stBriefInfo = (COMDT_PREPARE_GUILD_BRIEF_INFO)ProtocolObjectPool.Get(COMDT_PREPARE_GUILD_BRIEF_INFO.CLASS_ID);
			this.astMemberInfo = new COMDT_GUILD_MEMBER_BRIEF_INFO[20];
			for (int i = 0; i < 20; i++)
			{
				this.astMemberInfo[i] = (COMDT_GUILD_MEMBER_BRIEF_INFO)ProtocolObjectPool.Get(COMDT_GUILD_MEMBER_BRIEF_INFO.CLASS_ID);
			}
		}

		public override TdrError.ErrorType construct()
		{
			return TdrError.ErrorType.TDR_NO_ERROR;
		}

		public TdrError.ErrorType pack(ref byte[] buffer, int size, ref int usedSize, uint cutVer)
		{
			if (buffer == null || buffer.GetLength(0) == 0 || size > buffer.GetLength(0))
			{
				return TdrError.ErrorType.TDR_ERR_INVALID_BUFFER_PARAMETER;
			}
			TdrWriteBuf tdrWriteBuf = ClassObjPool<TdrWriteBuf>.Get();
			tdrWriteBuf.set(ref buffer, size);
			TdrError.ErrorType errorType = this.pack(ref tdrWriteBuf, cutVer);
			if (errorType == TdrError.ErrorType.TDR_NO_ERROR)
			{
				buffer = tdrWriteBuf.getBeginPtr();
				usedSize = tdrWriteBuf.getUsedSize();
			}
			tdrWriteBuf.Release();
			return errorType;
		}

		public override TdrError.ErrorType pack(ref TdrWriteBuf destBuf, uint cutVer)
		{
			if (cutVer == 0u || COMDT_PREPARE_GUILD_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_PREPARE_GUILD_INFO.CURRVERSION;
			}
			if (COMDT_PREPARE_GUILD_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = this.stBriefInfo.pack(ref destBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (20 < this.stBriefInfo.bMemberNum)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			if (this.astMemberInfo.Length < (int)this.stBriefInfo.bMemberNum)
			{
				return TdrError.ErrorType.TDR_ERR_VAR_ARRAY_CONFLICT;
			}
			for (int i = 0; i < (int)this.stBriefInfo.bMemberNum; i++)
			{
				errorType = this.astMemberInfo[i].pack(ref destBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			return errorType;
		}

		public TdrError.ErrorType unpack(ref byte[] buffer, int size, ref int usedSize, uint cutVer)
		{
			if (buffer == null || buffer.GetLength(0) == 0 || size > buffer.GetLength(0))
			{
				return TdrError.ErrorType.TDR_ERR_INVALID_BUFFER_PARAMETER;
			}
			TdrReadBuf tdrReadBuf = ClassObjPool<TdrReadBuf>.Get();
			tdrReadBuf.set(ref buffer, size);
			TdrError.ErrorType result = this.unpack(ref tdrReadBuf, cutVer);
			usedSize = tdrReadBuf.getUsedSize();
			tdrReadBuf.Release();
			return result;
		}

		public override TdrError.ErrorType unpack(ref TdrReadBuf srcBuf, uint cutVer)
		{
			if (cutVer == 0u || COMDT_PREPARE_GUILD_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_PREPARE_GUILD_INFO.CURRVERSION;
			}
			if (COMDT_PREPARE_GUILD_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = this.stBriefInfo.unpack(ref srcBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (20 < this.stBriefInfo.bMemberNum)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			for (int i = 0; i < (int)this.stBriefInfo.bMemberNum; i++)
			{
				errorType = this.astMemberInfo[i].unpack(ref srcBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_PREPARE_GUILD_INFO.CLASS_ID;
		}

		public override void OnRelease()
		{
			if (this.stBriefInfo != null)
			{
				this.stBriefInfo.Release();
				this.stBriefInfo = null;
			}
			if (this.astMemberInfo != null)
			{
				for (int i = 0; i < this.astMemberInfo.Length; i++)
				{
					if (this.astMemberInfo[i] != null)
					{
						this.astMemberInfo[i].Release();
						this.astMemberInfo[i] = null;
					}
				}
			}
		}

		public override void OnUse()
		{
			this.stBriefInfo = (COMDT_PREPARE_GUILD_BRIEF_INFO)ProtocolObjectPool.Get(COMDT_PREPARE_GUILD_BRIEF_INFO.CLASS_ID);
			if (this.astMemberInfo != null)
			{
				for (int i = 0; i < this.astMemberInfo.Length; i++)
				{
					this.astMemberInfo[i] = (COMDT_GUILD_MEMBER_BRIEF_INFO)ProtocolObjectPool.Get(COMDT_GUILD_MEMBER_BRIEF_INFO.CLASS_ID);
				}
			}
		}
	}
}
