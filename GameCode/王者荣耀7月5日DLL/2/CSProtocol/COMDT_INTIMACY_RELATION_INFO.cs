using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_INTIMACY_RELATION_INFO : ProtocolObject
	{
		public byte bIntimacyRelationPrior;

		public COMDT_INTIMACY_RELATION_DATA stIntimacyRelationData;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly int CLASS_ID = 641;

		public COMDT_INTIMACY_RELATION_INFO()
		{
			this.stIntimacyRelationData = (COMDT_INTIMACY_RELATION_DATA)ProtocolObjectPool.Get(COMDT_INTIMACY_RELATION_DATA.CLASS_ID);
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
			if (cutVer == 0u || COMDT_INTIMACY_RELATION_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_INTIMACY_RELATION_INFO.CURRVERSION;
			}
			if (COMDT_INTIMACY_RELATION_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = destBuf.writeUInt8(this.bIntimacyRelationPrior);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = this.stIntimacyRelationData.pack(ref destBuf, cutVer);
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
			if (cutVer == 0u || COMDT_INTIMACY_RELATION_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_INTIMACY_RELATION_INFO.CURRVERSION;
			}
			if (COMDT_INTIMACY_RELATION_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt8(ref this.bIntimacyRelationPrior);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = this.stIntimacyRelationData.unpack(ref srcBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_INTIMACY_RELATION_INFO.CLASS_ID;
		}

		public override void OnRelease()
		{
			this.bIntimacyRelationPrior = 0;
			if (this.stIntimacyRelationData != null)
			{
				this.stIntimacyRelationData.Release();
				this.stIntimacyRelationData = null;
			}
		}

		public override void OnUse()
		{
			this.stIntimacyRelationData = (COMDT_INTIMACY_RELATION_DATA)ProtocolObjectPool.Get(COMDT_INTIMACY_RELATION_DATA.CLASS_ID);
		}
	}
}
