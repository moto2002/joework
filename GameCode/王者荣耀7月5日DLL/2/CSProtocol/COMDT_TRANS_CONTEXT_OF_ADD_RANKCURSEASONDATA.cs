using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA : ProtocolObject
	{
		public COMDT_RANK_CURSEASON_FIGHT_RECORD stCurSeasonRecord;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 181u;

		public static readonly int CLASS_ID = 426;

		public COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA()
		{
			this.stCurSeasonRecord = (COMDT_RANK_CURSEASON_FIGHT_RECORD)ProtocolObjectPool.Get(COMDT_RANK_CURSEASON_FIGHT_RECORD.CLASS_ID);
		}

		public override TdrError.ErrorType construct()
		{
			TdrError.ErrorType errorType = this.stCurSeasonRecord.construct();
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			return errorType;
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
			if (cutVer == 0u || COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA.CURRVERSION < cutVer)
			{
				cutVer = COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA.CURRVERSION;
			}
			if (COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = this.stCurSeasonRecord.pack(ref destBuf, cutVer);
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
			if (cutVer == 0u || COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA.CURRVERSION < cutVer)
			{
				cutVer = COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA.CURRVERSION;
			}
			if (COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = this.stCurSeasonRecord.unpack(ref srcBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_TRANS_CONTEXT_OF_ADD_RANKCURSEASONDATA.CLASS_ID;
		}

		public override void OnRelease()
		{
			if (this.stCurSeasonRecord != null)
			{
				this.stCurSeasonRecord.Release();
				this.stCurSeasonRecord = null;
			}
		}

		public override void OnUse()
		{
			this.stCurSeasonRecord = (COMDT_RANK_CURSEASON_FIGHT_RECORD)ProtocolObjectPool.Get(COMDT_RANK_CURSEASON_FIGHT_RECORD.CLASS_ID);
		}
	}
}
