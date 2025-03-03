using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC : ProtocolObject
	{
		public ulong ullUid;

		public uint dwLogicWorldID;

		public COMDT_PLAYER_FIGHT_HISTORY stRecordList;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 172u;

		public static readonly int CLASS_ID = 901;

		public CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC()
		{
			this.stRecordList = (COMDT_PLAYER_FIGHT_HISTORY)ProtocolObjectPool.Get(COMDT_PLAYER_FIGHT_HISTORY.CLASS_ID);
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
			if (cutVer == 0u || CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC.CURRVERSION < cutVer)
			{
				cutVer = CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC.CURRVERSION;
			}
			if (CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = destBuf.writeUInt64(this.ullUid);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwLogicWorldID);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = this.stRecordList.pack(ref destBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
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
			if (cutVer == 0u || CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC.CURRVERSION < cutVer)
			{
				cutVer = CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC.CURRVERSION;
			}
			if (CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt64(ref this.ullUid);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwLogicWorldID);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = this.stRecordList.unpack(ref srcBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return CSDT_FIGHTHISTORY_RECORD_DETAIL_SUCC.CLASS_ID;
		}

		public override void OnRelease()
		{
			this.ullUid = 0uL;
			this.dwLogicWorldID = 0u;
			if (this.stRecordList != null)
			{
				this.stRecordList.Release();
				this.stRecordList = null;
			}
		}

		public override void OnUse()
		{
			this.stRecordList = (COMDT_PLAYER_FIGHT_HISTORY)ProtocolObjectPool.Get(COMDT_PLAYER_FIGHT_HISTORY.CLASS_ID);
		}
	}
}
