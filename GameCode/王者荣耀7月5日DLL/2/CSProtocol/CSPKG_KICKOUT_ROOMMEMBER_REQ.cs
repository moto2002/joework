using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class CSPKG_KICKOUT_ROOMMEMBER_REQ : ProtocolObject
	{
		public COMDT_KICKOUT_ROOMMEMBER_REQ stKickMemberInfo;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly int CLASS_ID = 1098;

		public CSPKG_KICKOUT_ROOMMEMBER_REQ()
		{
			this.stKickMemberInfo = (COMDT_KICKOUT_ROOMMEMBER_REQ)ProtocolObjectPool.Get(COMDT_KICKOUT_ROOMMEMBER_REQ.CLASS_ID);
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
			if (cutVer == 0u || CSPKG_KICKOUT_ROOMMEMBER_REQ.CURRVERSION < cutVer)
			{
				cutVer = CSPKG_KICKOUT_ROOMMEMBER_REQ.CURRVERSION;
			}
			if (CSPKG_KICKOUT_ROOMMEMBER_REQ.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = this.stKickMemberInfo.pack(ref destBuf, cutVer);
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
			if (cutVer == 0u || CSPKG_KICKOUT_ROOMMEMBER_REQ.CURRVERSION < cutVer)
			{
				cutVer = CSPKG_KICKOUT_ROOMMEMBER_REQ.CURRVERSION;
			}
			if (CSPKG_KICKOUT_ROOMMEMBER_REQ.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = this.stKickMemberInfo.unpack(ref srcBuf, cutVer);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return CSPKG_KICKOUT_ROOMMEMBER_REQ.CLASS_ID;
		}

		public override void OnRelease()
		{
			if (this.stKickMemberInfo != null)
			{
				this.stKickMemberInfo.Release();
				this.stKickMemberInfo = null;
			}
		}

		public override void OnUse()
		{
			this.stKickMemberInfo = (COMDT_KICKOUT_ROOMMEMBER_REQ)ProtocolObjectPool.Get(COMDT_KICKOUT_ROOMMEMBER_REQ.CLASS_ID);
		}
	}
}
