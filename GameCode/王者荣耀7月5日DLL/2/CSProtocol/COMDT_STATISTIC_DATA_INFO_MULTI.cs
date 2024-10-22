using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_STATISTIC_DATA_INFO_MULTI : ProtocolObject
	{
		public byte bGameType;

		public byte bIsPKAI;

		public byte bAcntNum;

		public uint dwWinSum;

		public uint dwLoseSum;

		public uint dwDrawSum;

		public uint dwContinuousWin;

		public uint dwContinuousLose;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 38u;

		public static readonly uint VERSION_dwContinuousWin = 38u;

		public static readonly uint VERSION_dwContinuousLose = 38u;

		public static readonly int CLASS_ID = 510;

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
			if (cutVer == 0u || COMDT_STATISTIC_DATA_INFO_MULTI.CURRVERSION < cutVer)
			{
				cutVer = COMDT_STATISTIC_DATA_INFO_MULTI.CURRVERSION;
			}
			if (COMDT_STATISTIC_DATA_INFO_MULTI.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = destBuf.writeUInt8(this.bGameType);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt8(this.bIsPKAI);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt8(this.bAcntNum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwWinSum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwLoseSum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt32(this.dwDrawSum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (COMDT_STATISTIC_DATA_INFO_MULTI.VERSION_dwContinuousWin <= cutVer)
			{
				errorType = destBuf.writeUInt32(this.dwContinuousWin);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			if (COMDT_STATISTIC_DATA_INFO_MULTI.VERSION_dwContinuousLose <= cutVer)
			{
				errorType = destBuf.writeUInt32(this.dwContinuousLose);
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
			if (cutVer == 0u || COMDT_STATISTIC_DATA_INFO_MULTI.CURRVERSION < cutVer)
			{
				cutVer = COMDT_STATISTIC_DATA_INFO_MULTI.CURRVERSION;
			}
			if (COMDT_STATISTIC_DATA_INFO_MULTI.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt8(ref this.bGameType);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt8(ref this.bIsPKAI);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt8(ref this.bAcntNum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwWinSum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwLoseSum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwDrawSum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (COMDT_STATISTIC_DATA_INFO_MULTI.VERSION_dwContinuousWin <= cutVer)
			{
				errorType = srcBuf.readUInt32(ref this.dwContinuousWin);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.dwContinuousWin = 0u;
			}
			if (COMDT_STATISTIC_DATA_INFO_MULTI.VERSION_dwContinuousLose <= cutVer)
			{
				errorType = srcBuf.readUInt32(ref this.dwContinuousLose);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			else
			{
				this.dwContinuousLose = 0u;
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_STATISTIC_DATA_INFO_MULTI.CLASS_ID;
		}

		public override void OnRelease()
		{
			this.bGameType = 0;
			this.bIsPKAI = 0;
			this.bAcntNum = 0;
			this.dwWinSum = 0u;
			this.dwLoseSum = 0u;
			this.dwDrawSum = 0u;
			this.dwContinuousWin = 0u;
			this.dwContinuousLose = 0u;
		}
	}
}
