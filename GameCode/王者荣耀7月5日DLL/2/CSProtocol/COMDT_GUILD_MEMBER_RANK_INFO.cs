using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_GUILD_MEMBER_RANK_INFO : ProtocolObject
	{
		public uint dwMaxRankPoint;

		public uint dwTotalRankPoint;

		public uint dwKillCnt;

		public uint dwDeadCnt;

		public uint dwAssistCnt;

		public uint dwWeekRankPoint;

		public byte bSignIn;

		public uint dwConsumeRP;

		public uint dwGameRP;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 82u;

		public static readonly uint VERSION_dwWeekRankPoint = 82u;

		public static readonly uint VERSION_bSignIn = 82u;

		public static readonly uint VERSION_dwConsumeRP = 82u;

		public static readonly uint VERSION_dwGameRP = 82u;

		public static readonly int CLASS_ID = 341;

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
			if (cutVer == 0u || COMDT_GUILD_MEMBER_RANK_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_GUILD_MEMBER_RANK_INFO.CURRVERSION;
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = destBuf.writeUInt32(this.dwMaxRankPoint);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwTotalRankPoint);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwKillCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwDeadCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwAssistCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.VERSION_dwWeekRankPoint <= cutVer)
			{
				errorType = destBuf.writeUInt32(this.dwWeekRankPoint);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.VERSION_bSignIn <= cutVer)
			{
				errorType = destBuf.writeUInt8(this.bSignIn);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.VERSION_dwConsumeRP <= cutVer)
			{
				errorType = destBuf.writeUInt32(this.dwConsumeRP);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.VERSION_dwGameRP <= cutVer)
			{
				errorType = destBuf.writeUInt32(this.dwGameRP);
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
			if (cutVer == 0u || COMDT_GUILD_MEMBER_RANK_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_GUILD_MEMBER_RANK_INFO.CURRVERSION;
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt32(ref this.dwMaxRankPoint);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwTotalRankPoint);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwKillCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwDeadCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwAssistCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.VERSION_dwWeekRankPoint <= cutVer)
			{
				errorType = srcBuf.readUInt32(ref this.dwWeekRankPoint);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.dwWeekRankPoint = 0u;
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.VERSION_bSignIn <= cutVer)
			{
				errorType = srcBuf.readUInt8(ref this.bSignIn);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.bSignIn = 0;
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.VERSION_dwConsumeRP <= cutVer)
			{
				errorType = srcBuf.readUInt32(ref this.dwConsumeRP);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.dwConsumeRP = 0u;
			}
			if (COMDT_GUILD_MEMBER_RANK_INFO.VERSION_dwGameRP <= cutVer)
			{
				errorType = srcBuf.readUInt32(ref this.dwGameRP);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.dwGameRP = 0u;
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_GUILD_MEMBER_RANK_INFO.CLASS_ID;
		}

		public override void OnRelease()
		{
			this.dwMaxRankPoint = 0u;
			this.dwTotalRankPoint = 0u;
			this.dwKillCnt = 0u;
			this.dwDeadCnt = 0u;
			this.dwAssistCnt = 0u;
			this.dwWeekRankPoint = 0u;
			this.bSignIn = 0;
			this.dwConsumeRP = 0u;
			this.dwGameRP = 0u;
		}
	}
}
