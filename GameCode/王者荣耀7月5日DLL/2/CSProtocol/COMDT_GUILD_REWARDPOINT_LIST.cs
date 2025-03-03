using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_GUILD_REWARDPOINT_LIST : ProtocolObject
	{
		public uint dwCount;

		public COMDT_GUILDPOINT[] astList;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly int CLASS_ID = 357;

		public COMDT_GUILD_REWARDPOINT_LIST()
		{
			this.astList = new COMDT_GUILDPOINT[8];
			for (int i = 0; i < 8; i++)
			{
				this.astList[i] = (COMDT_GUILDPOINT)ProtocolObjectPool.Get(COMDT_GUILDPOINT.CLASS_ID);
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
			if (cutVer == 0u || COMDT_GUILD_REWARDPOINT_LIST.CURRVERSION < cutVer)
			{
				cutVer = COMDT_GUILD_REWARDPOINT_LIST.CURRVERSION;
			}
			if (COMDT_GUILD_REWARDPOINT_LIST.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = destBuf.writeUInt32(this.dwCount);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (8u < this.dwCount)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			if ((long)this.astList.Length < (long)((ulong)this.dwCount))
			{
				return TdrError.ErrorType.TDR_ERR_VAR_ARRAY_CONFLICT;
			}
			int num = 0;
			while ((long)num < (long)((ulong)this.dwCount))
			{
				errorType = this.astList[num].pack(ref destBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
				num++;
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
			if (cutVer == 0u || COMDT_GUILD_REWARDPOINT_LIST.CURRVERSION < cutVer)
			{
				cutVer = COMDT_GUILD_REWARDPOINT_LIST.CURRVERSION;
			}
			if (COMDT_GUILD_REWARDPOINT_LIST.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt32(ref this.dwCount);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (8u < this.dwCount)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			int num = 0;
			while ((long)num < (long)((ulong)this.dwCount))
			{
				errorType = this.astList[num].unpack(ref srcBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
				num++;
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_GUILD_REWARDPOINT_LIST.CLASS_ID;
		}

		public override void OnRelease()
		{
			this.dwCount = 0u;
			if (this.astList != null)
			{
				for (int i = 0; i < this.astList.Length; i++)
				{
					if (this.astList[i] != null)
					{
						this.astList[i].Release();
						this.astList[i] = null;
					}
				}
			}
		}

		public override void OnUse()
		{
			if (this.astList != null)
			{
				for (int i = 0; i < this.astList.Length; i++)
				{
					this.astList[i] = (COMDT_GUILDPOINT)ProtocolObjectPool.Get(COMDT_GUILDPOINT.CLASS_ID);
				}
			}
		}
	}
}
