using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_INTIMACY_RELATION_DATA : ProtocolObject
	{
		public ushort wIntimacyRelationNum;

		public COMDT_INTIMACY_RELATION[] astIntimacyRelationList;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly int CLASS_ID = 640;

		public COMDT_INTIMACY_RELATION_DATA()
		{
			this.astIntimacyRelationList = new COMDT_INTIMACY_RELATION[2];
			for (int i = 0; i < 2; i++)
			{
				this.astIntimacyRelationList[i] = (COMDT_INTIMACY_RELATION)ProtocolObjectPool.Get(COMDT_INTIMACY_RELATION.CLASS_ID);
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
			if (cutVer == 0u || COMDT_INTIMACY_RELATION_DATA.CURRVERSION < cutVer)
			{
				cutVer = COMDT_INTIMACY_RELATION_DATA.CURRVERSION;
			}
			if (COMDT_INTIMACY_RELATION_DATA.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = destBuf.writeUInt16(this.wIntimacyRelationNum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (2 < this.wIntimacyRelationNum)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			if (this.astIntimacyRelationList.Length < (int)this.wIntimacyRelationNum)
			{
				return TdrError.ErrorType.TDR_ERR_VAR_ARRAY_CONFLICT;
			}
			for (int i = 0; i < (int)this.wIntimacyRelationNum; i++)
			{
				errorType = this.astIntimacyRelationList[i].pack(ref destBuf, cutVer);
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
			if (cutVer == 0u || COMDT_INTIMACY_RELATION_DATA.CURRVERSION < cutVer)
			{
				cutVer = COMDT_INTIMACY_RELATION_DATA.CURRVERSION;
			}
			if (COMDT_INTIMACY_RELATION_DATA.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt16(ref this.wIntimacyRelationNum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (2 < this.wIntimacyRelationNum)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			for (int i = 0; i < (int)this.wIntimacyRelationNum; i++)
			{
				errorType = this.astIntimacyRelationList[i].unpack(ref srcBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_INTIMACY_RELATION_DATA.CLASS_ID;
		}

		public override void OnRelease()
		{
			this.wIntimacyRelationNum = 0;
			if (this.astIntimacyRelationList != null)
			{
				for (int i = 0; i < this.astIntimacyRelationList.Length; i++)
				{
					if (this.astIntimacyRelationList[i] != null)
					{
						this.astIntimacyRelationList[i].Release();
						this.astIntimacyRelationList[i] = null;
					}
				}
			}
		}

		public override void OnUse()
		{
			if (this.astIntimacyRelationList != null)
			{
				for (int i = 0; i < this.astIntimacyRelationList.Length; i++)
				{
					this.astIntimacyRelationList[i] = (COMDT_INTIMACY_RELATION)ProtocolObjectPool.Get(COMDT_INTIMACY_RELATION.CLASS_ID);
				}
			}
		}
	}
}
