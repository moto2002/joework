using System;
using tsf4g_tdr_csharp;

namespace ResData
{
	public class ResMultiKill : IUnpackable, tsf4g_csharp_interface
	{
		public byte bKillCount;

		public byte[] szAchievementName_ByteArray;

		public string szAchievementName;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly uint LENGTH_szAchievementName = 64u;

		public ResMultiKill()
		{
			this.szAchievementName_ByteArray = new byte[1];
			this.szAchievementName = string.Empty;
		}

		private void TransferData()
		{
			this.szAchievementName = StringHelper.UTF8BytesToString(ref this.szAchievementName_ByteArray);
			this.szAchievementName_ByteArray = null;
		}

		public TdrError.ErrorType construct()
		{
			return TdrError.ErrorType.TDR_NO_ERROR;
		}

		public TdrError.ErrorType unpack(ref byte[] buffer, int size, ref int usedSize, uint cutVer)
		{
			if (buffer == null || buffer.GetLength(0) == 0 || size > buffer.GetLength(0))
			{
				return TdrError.ErrorType.TDR_ERR_INVALID_BUFFER_PARAMETER;
			}
			TdrReadBuf tdrReadBuf = new TdrReadBuf(ref buffer, size);
			TdrError.ErrorType result = this.unpack(ref tdrReadBuf, cutVer);
			usedSize = tdrReadBuf.getUsedSize();
			return result;
		}

		public TdrError.ErrorType unpack(ref TdrReadBuf srcBuf, uint cutVer)
		{
			if (cutVer == 0u || ResMultiKill.CURRVERSION < cutVer)
			{
				cutVer = ResMultiKill.CURRVERSION;
			}
			if (ResMultiKill.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt8(ref this.bKillCount);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
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
			if (num > (uint)this.szAchievementName_ByteArray.GetLength(0))
			{
				if ((long)num > (long)((ulong)ResMultiKill.LENGTH_szAchievementName))
				{
					return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
				}
				this.szAchievementName_ByteArray = new byte[num];
			}
			if (1u > num)
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
			}
			errorType = srcBuf.readCString(ref this.szAchievementName_ByteArray, (int)num);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (this.szAchievementName_ByteArray[(int)(num - 1u)] != 0)
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
			}
			int num2 = TdrTypeUtil.cstrlen(this.szAchievementName_ByteArray) + 1;
			if ((ulong)num != (ulong)((long)num2))
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
			}
			this.TransferData();
			return errorType;
		}

		public TdrError.ErrorType load(ref byte[] buffer, int size, ref int usedSize, uint cutVer)
		{
			if (buffer == null || buffer.GetLength(0) == 0 || size > buffer.GetLength(0))
			{
				return TdrError.ErrorType.TDR_ERR_INVALID_BUFFER_PARAMETER;
			}
			TdrReadBuf tdrReadBuf = new TdrReadBuf(ref buffer, size);
			TdrError.ErrorType result = this.load(ref tdrReadBuf, cutVer);
			usedSize = tdrReadBuf.getUsedSize();
			return result;
		}

		public TdrError.ErrorType load(ref TdrReadBuf srcBuf, uint cutVer)
		{
			srcBuf.disableEndian();
			if (cutVer == 0u || ResMultiKill.CURRVERSION < cutVer)
			{
				cutVer = ResMultiKill.CURRVERSION;
			}
			if (ResMultiKill.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt8(ref this.bKillCount);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			int num = 64;
			if (this.szAchievementName_ByteArray.GetLength(0) < num)
			{
				this.szAchievementName_ByteArray = new byte[ResMultiKill.LENGTH_szAchievementName];
			}
			errorType = srcBuf.readCString(ref this.szAchievementName_ByteArray, num);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			this.TransferData();
			return errorType;
		}
	}
}
