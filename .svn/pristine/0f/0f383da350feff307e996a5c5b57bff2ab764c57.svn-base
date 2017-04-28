using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_GUILD_INFO : ProtocolObject
	{
		public COMDT_GUILD_BRIEF_INFO stBriefInfo;

		public ulong ullBuildTime;

		public uint dwGuildMoney;

		public uint dwActive;

		public uint dwCoinPool;

		public uint dwMaintainTime;

		public uint dwElectionTime;

		public COMDT_GUILD_MEMBER_INFO[] astMemberInfo;

		public byte bBuildingCnt;

		public COMDT_GUILD_BUILDING[] astBuildingInfo;

		public byte bSelfRecommendCnt;

		public COMDT_SELF_RECOMMEND_INFO[] astSelfRecommendInfo;

		public COMDT_GUILD_RANK_INFO stRankInfo;

		public uint dwChangeNameCnt;

		public uint dwStar;

		public uint dwGroupGuildID;

		public byte[] szGroupOpenID;

		public COMDT_GUILD_MATCH_INFO stGuildMatchInfo;

		public byte bOBMatchCnt;

		public COMDT_GUILD_MATCH_OB_INFO[] astGuildMatchOBInfo;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 153u;

		public static readonly uint VERSION_dwChangeNameCnt = 59u;

		public static readonly uint VERSION_dwStar = 82u;

		public static readonly uint VERSION_dwGroupGuildID = 86u;

		public static readonly uint VERSION_szGroupOpenID = 96u;

		public static readonly uint VERSION_stGuildMatchInfo = 145u;

		public static readonly uint VERSION_bOBMatchCnt = 145u;

		public static readonly uint VERSION_astGuildMatchOBInfo = 145u;

		public static readonly uint LENGTH_szGroupOpenID = 128u;

		public static readonly int CLASS_ID = 361;

		public COMDT_GUILD_INFO()
		{
			this.stBriefInfo = (COMDT_GUILD_BRIEF_INFO)ProtocolObjectPool.Get(COMDT_GUILD_BRIEF_INFO.CLASS_ID);
			this.astMemberInfo = new COMDT_GUILD_MEMBER_INFO[150];
			for (int i = 0; i < 150; i++)
			{
				this.astMemberInfo[i] = (COMDT_GUILD_MEMBER_INFO)ProtocolObjectPool.Get(COMDT_GUILD_MEMBER_INFO.CLASS_ID);
			}
			this.astBuildingInfo = new COMDT_GUILD_BUILDING[10];
			for (int j = 0; j < 10; j++)
			{
				this.astBuildingInfo[j] = (COMDT_GUILD_BUILDING)ProtocolObjectPool.Get(COMDT_GUILD_BUILDING.CLASS_ID);
			}
			this.astSelfRecommendInfo = new COMDT_SELF_RECOMMEND_INFO[2];
			for (int k = 0; k < 2; k++)
			{
				this.astSelfRecommendInfo[k] = (COMDT_SELF_RECOMMEND_INFO)ProtocolObjectPool.Get(COMDT_SELF_RECOMMEND_INFO.CLASS_ID);
			}
			this.stRankInfo = (COMDT_GUILD_RANK_INFO)ProtocolObjectPool.Get(COMDT_GUILD_RANK_INFO.CLASS_ID);
			this.szGroupOpenID = new byte[128];
			this.stGuildMatchInfo = (COMDT_GUILD_MATCH_INFO)ProtocolObjectPool.Get(COMDT_GUILD_MATCH_INFO.CLASS_ID);
			this.astGuildMatchOBInfo = new COMDT_GUILD_MATCH_OB_INFO[10];
			for (int l = 0; l < 10; l++)
			{
				this.astGuildMatchOBInfo[l] = (COMDT_GUILD_MATCH_OB_INFO)ProtocolObjectPool.Get(COMDT_GUILD_MATCH_OB_INFO.CLASS_ID);
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
			if (cutVer == 0u || COMDT_GUILD_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_GUILD_INFO.CURRVERSION;
			}
			if (COMDT_GUILD_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = this.stBriefInfo.pack(ref destBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt64(this.ullBuildTime);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwGuildMoney);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwActive);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwCoinPool);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwMaintainTime);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwElectionTime);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (150 < this.stBriefInfo.bMemberNum)
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
			errorType = destBuf.writeUInt8(this.bBuildingCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (10 < this.bBuildingCnt)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			if (this.astBuildingInfo.Length < (int)this.bBuildingCnt)
			{
				return TdrError.ErrorType.TDR_ERR_VAR_ARRAY_CONFLICT;
			}
			for (int j = 0; j < (int)this.bBuildingCnt; j++)
			{
				errorType = this.astBuildingInfo[j].pack(ref destBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			errorType = destBuf.writeUInt8(this.bSelfRecommendCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (2 < this.bSelfRecommendCnt)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			if (this.astSelfRecommendInfo.Length < (int)this.bSelfRecommendCnt)
			{
				return TdrError.ErrorType.TDR_ERR_VAR_ARRAY_CONFLICT;
			}
			for (int k = 0; k < (int)this.bSelfRecommendCnt; k++)
			{
				errorType = this.astSelfRecommendInfo[k].pack(ref destBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			errorType = this.stRankInfo.pack(ref destBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (COMDT_GUILD_INFO.VERSION_dwChangeNameCnt <= cutVer)
			{
				errorType = destBuf.writeUInt32(this.dwChangeNameCnt);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_INFO.VERSION_dwStar <= cutVer)
			{
				errorType = destBuf.writeUInt32(this.dwStar);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_INFO.VERSION_dwGroupGuildID <= cutVer)
			{
				errorType = destBuf.writeUInt32(this.dwGroupGuildID);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_INFO.VERSION_szGroupOpenID <= cutVer)
			{
				int usedSize = destBuf.getUsedSize();
				errorType = destBuf.reserve(4);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
				int usedSize2 = destBuf.getUsedSize();
				int num = TdrTypeUtil.cstrlen(this.szGroupOpenID);
				if (num >= 128)
				{
					return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
				}
				errorType = destBuf.writeCString(this.szGroupOpenID, num);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
				errorType = destBuf.writeUInt8(0);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
				int src = destBuf.getUsedSize() - usedSize2;
				errorType = destBuf.writeUInt32((uint)src, usedSize);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_INFO.VERSION_stGuildMatchInfo <= cutVer)
			{
				errorType = this.stGuildMatchInfo.pack(ref destBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_INFO.VERSION_bOBMatchCnt <= cutVer)
			{
				errorType = destBuf.writeUInt8(this.bOBMatchCnt);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_INFO.VERSION_astGuildMatchOBInfo <= cutVer)
			{
				if (10 < this.bOBMatchCnt)
				{
					return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
				}
				if (this.astGuildMatchOBInfo.Length < (int)this.bOBMatchCnt)
				{
					return TdrError.ErrorType.TDR_ERR_VAR_ARRAY_CONFLICT;
				}
				for (int l = 0; l < (int)this.bOBMatchCnt; l++)
				{
					errorType = this.astGuildMatchOBInfo[l].pack(ref destBuf, cutVer);
					if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
					{
						return errorType;
					}
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
			if (cutVer == 0u || COMDT_GUILD_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_GUILD_INFO.CURRVERSION;
			}
			if (COMDT_GUILD_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = this.stBriefInfo.unpack(ref srcBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt64(ref this.ullBuildTime);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwGuildMoney);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwActive);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwCoinPool);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwMaintainTime);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwElectionTime);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (150 < this.stBriefInfo.bMemberNum)
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
			errorType = srcBuf.readUInt8(ref this.bBuildingCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (10 < this.bBuildingCnt)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			for (int j = 0; j < (int)this.bBuildingCnt; j++)
			{
				errorType = this.astBuildingInfo[j].unpack(ref srcBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			errorType = srcBuf.readUInt8(ref this.bSelfRecommendCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (2 < this.bSelfRecommendCnt)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			for (int k = 0; k < (int)this.bSelfRecommendCnt; k++)
			{
				errorType = this.astSelfRecommendInfo[k].unpack(ref srcBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			errorType = this.stRankInfo.unpack(ref srcBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (COMDT_GUILD_INFO.VERSION_dwChangeNameCnt <= cutVer)
			{
				errorType = srcBuf.readUInt32(ref this.dwChangeNameCnt);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.dwChangeNameCnt = 0u;
			}
			if (COMDT_GUILD_INFO.VERSION_dwStar <= cutVer)
			{
				errorType = srcBuf.readUInt32(ref this.dwStar);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.dwStar = 0u;
			}
			if (COMDT_GUILD_INFO.VERSION_dwGroupGuildID <= cutVer)
			{
				errorType = srcBuf.readUInt32(ref this.dwGroupGuildID);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.dwGroupGuildID = 0u;
			}
			if (COMDT_GUILD_INFO.VERSION_szGroupOpenID <= cutVer)
			{
				uint num = 0u;
				errorType = srcBuf.readUInt32(ref num);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
				if (num > (uint)srcBuf.getLeftSize())
				{
					return TdrError.ErrorType.TDR_ERR_SHORT_BUF_FOR_READ;
				}
				if (num > (uint)this.szGroupOpenID.GetLength(0))
				{
					if ((long)num > (long)((ulong)COMDT_GUILD_INFO.LENGTH_szGroupOpenID))
					{
						return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
					}
					this.szGroupOpenID = new byte[num];
				}
				if (1u > num)
				{
					return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
				}
				errorType = srcBuf.readCString(ref this.szGroupOpenID, (int)num);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
				if (this.szGroupOpenID[(int)(num - 1u)] != 0)
				{
					return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
				}
				int num2 = TdrTypeUtil.cstrlen(this.szGroupOpenID) + 1;
				if ((ulong)num != (ulong)((long)num2))
				{
					return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
				}
			}
			if (COMDT_GUILD_INFO.VERSION_stGuildMatchInfo <= cutVer)
			{
				errorType = this.stGuildMatchInfo.unpack(ref srcBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				errorType = this.stGuildMatchInfo.construct();
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_INFO.VERSION_bOBMatchCnt <= cutVer)
			{
				errorType = srcBuf.readUInt8(ref this.bOBMatchCnt);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.bOBMatchCnt = 0;
			}
			if (COMDT_GUILD_INFO.VERSION_astGuildMatchOBInfo <= cutVer)
			{
				if (10 < this.bOBMatchCnt)
				{
					return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
				}
				for (int l = 0; l < (int)this.bOBMatchCnt; l++)
				{
					errorType = this.astGuildMatchOBInfo[l].unpack(ref srcBuf, cutVer);
					if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
					{
						return errorType;
					}
				}
			}
			else
			{
				if (10 < this.bOBMatchCnt)
				{
					return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
				}
				for (int m = 0; m < (int)this.bOBMatchCnt; m++)
				{
					errorType = this.astGuildMatchOBInfo[m].construct();
					if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
					{
						return errorType;
					}
				}
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_GUILD_INFO.CLASS_ID;
		}

		public override void OnRelease()
		{
			if (this.stBriefInfo != null)
			{
				this.stBriefInfo.Release();
				this.stBriefInfo = null;
			}
			this.ullBuildTime = 0uL;
			this.dwGuildMoney = 0u;
			this.dwActive = 0u;
			this.dwCoinPool = 0u;
			this.dwMaintainTime = 0u;
			this.dwElectionTime = 0u;
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
			this.bBuildingCnt = 0;
			if (this.astBuildingInfo != null)
			{
				for (int j = 0; j < this.astBuildingInfo.Length; j++)
				{
					if (this.astBuildingInfo[j] != null)
					{
						this.astBuildingInfo[j].Release();
						this.astBuildingInfo[j] = null;
					}
				}
			}
			this.bSelfRecommendCnt = 0;
			if (this.astSelfRecommendInfo != null)
			{
				for (int k = 0; k < this.astSelfRecommendInfo.Length; k++)
				{
					if (this.astSelfRecommendInfo[k] != null)
					{
						this.astSelfRecommendInfo[k].Release();
						this.astSelfRecommendInfo[k] = null;
					}
				}
			}
			if (this.stRankInfo != null)
			{
				this.stRankInfo.Release();
				this.stRankInfo = null;
			}
			this.dwChangeNameCnt = 0u;
			this.dwStar = 0u;
			this.dwGroupGuildID = 0u;
			if (this.stGuildMatchInfo != null)
			{
				this.stGuildMatchInfo.Release();
				this.stGuildMatchInfo = null;
			}
			this.bOBMatchCnt = 0;
			if (this.astGuildMatchOBInfo != null)
			{
				for (int l = 0; l < this.astGuildMatchOBInfo.Length; l++)
				{
					if (this.astGuildMatchOBInfo[l] != null)
					{
						this.astGuildMatchOBInfo[l].Release();
						this.astGuildMatchOBInfo[l] = null;
					}
				}
			}
		}

		public override void OnUse()
		{
			this.stBriefInfo = (COMDT_GUILD_BRIEF_INFO)ProtocolObjectPool.Get(COMDT_GUILD_BRIEF_INFO.CLASS_ID);
			if (this.astMemberInfo != null)
			{
				for (int i = 0; i < this.astMemberInfo.Length; i++)
				{
					this.astMemberInfo[i] = (COMDT_GUILD_MEMBER_INFO)ProtocolObjectPool.Get(COMDT_GUILD_MEMBER_INFO.CLASS_ID);
				}
			}
			if (this.astBuildingInfo != null)
			{
				for (int j = 0; j < this.astBuildingInfo.Length; j++)
				{
					this.astBuildingInfo[j] = (COMDT_GUILD_BUILDING)ProtocolObjectPool.Get(COMDT_GUILD_BUILDING.CLASS_ID);
				}
			}
			if (this.astSelfRecommendInfo != null)
			{
				for (int k = 0; k < this.astSelfRecommendInfo.Length; k++)
				{
					this.astSelfRecommendInfo[k] = (COMDT_SELF_RECOMMEND_INFO)ProtocolObjectPool.Get(COMDT_SELF_RECOMMEND_INFO.CLASS_ID);
				}
			}
			this.stRankInfo = (COMDT_GUILD_RANK_INFO)ProtocolObjectPool.Get(COMDT_GUILD_RANK_INFO.CLASS_ID);
			this.stGuildMatchInfo = (COMDT_GUILD_MATCH_INFO)ProtocolObjectPool.Get(COMDT_GUILD_MATCH_INFO.CLASS_ID);
			if (this.astGuildMatchOBInfo != null)
			{
				for (int l = 0; l < this.astGuildMatchOBInfo.Length; l++)
				{
					this.astGuildMatchOBInfo[l] = (COMDT_GUILD_MATCH_OB_INFO)ProtocolObjectPool.Get(COMDT_GUILD_MATCH_OB_INFO.CLASS_ID);
				}
			}
		}
	}
}
