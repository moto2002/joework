using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class SCPKG_OFFLINE_CHAT_NTF : ProtocolObject
	{
		public byte bChatCnt;

		public CSDT_OFFLINE_CHAT_INFO[] astChatInfo;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly int CLASS_ID = 1018;

		public SCPKG_OFFLINE_CHAT_NTF()
		{
			this.astChatInfo = new CSDT_OFFLINE_CHAT_INFO[50];
			for (int i = 0; i < 50; i++)
			{
				this.astChatInfo[i] = (CSDT_OFFLINE_CHAT_INFO)ProtocolObjectPool.Get(CSDT_OFFLINE_CHAT_INFO.CLASS_ID);
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
			if (cutVer == 0u || SCPKG_OFFLINE_CHAT_NTF.CURRVERSION < cutVer)
			{
				cutVer = SCPKG_OFFLINE_CHAT_NTF.CURRVERSION;
			}
			if (SCPKG_OFFLINE_CHAT_NTF.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = destBuf.writeUInt8(this.bChatCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (50 < this.bChatCnt)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			if (this.astChatInfo.Length < (int)this.bChatCnt)
			{
				return TdrError.ErrorType.TDR_ERR_VAR_ARRAY_CONFLICT;
			}
			for (int i = 0; i < (int)this.bChatCnt; i++)
			{
				errorType = this.astChatInfo[i].pack(ref destBuf, cutVer);
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
			if (cutVer == 0u || SCPKG_OFFLINE_CHAT_NTF.CURRVERSION < cutVer)
			{
				cutVer = SCPKG_OFFLINE_CHAT_NTF.CURRVERSION;
			}
			if (SCPKG_OFFLINE_CHAT_NTF.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt8(ref this.bChatCnt);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (50 < this.bChatCnt)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			for (int i = 0; i < (int)this.bChatCnt; i++)
			{
				errorType = this.astChatInfo[i].unpack(ref srcBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return SCPKG_OFFLINE_CHAT_NTF.CLASS_ID;
		}

		public override void OnRelease()
		{
			this.bChatCnt = 0;
			if (this.astChatInfo != null)
			{
				for (int i = 0; i < this.astChatInfo.Length; i++)
				{
					if (this.astChatInfo[i] != null)
					{
						this.astChatInfo[i].Release();
						this.astChatInfo[i] = null;
					}
				}
			}
		}

		public override void OnUse()
		{
			if (this.astChatInfo != null)
			{
				for (int i = 0; i < this.astChatInfo.Length; i++)
				{
					this.astChatInfo[i] = (CSDT_OFFLINE_CHAT_INFO)ProtocolObjectPool.Get(CSDT_OFFLINE_CHAT_INFO.CLASS_ID);
				}
			}
		}
	}
}
