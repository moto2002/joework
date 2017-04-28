using System;
using tsf4g_tdr_csharp;

namespace ResData
{
	public class ResMentorPrivilegeIcon : IUnpackable, tsf4g_csharp_interface
	{
		public byte[] szPrivilegeIcon_ByteArray;

		public string szPrivilegeIcon;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly uint LENGTH_szPrivilegeIcon = 1u;

		public ResMentorPrivilegeIcon()
		{
			this.szPrivilegeIcon_ByteArray = new byte[1];
			this.szPrivilegeIcon = string.Empty;
		}

		private void TransferData()
		{
			this.szPrivilegeIcon = StringHelper.UTF8BytesToString(ref this.szPrivilegeIcon_ByteArray);
			this.szPrivilegeIcon_ByteArray = null;
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
			if (cutVer == 0u || ResMentorPrivilegeIcon.CURRVERSION < cutVer)
			{
				cutVer = ResMentorPrivilegeIcon.CURRVERSION;
			}
			if (ResMentorPrivilegeIcon.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			uint num = 0u;
			TdrError.ErrorType errorType = srcBuf.readUInt32(ref num);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (num > (uint)srcBuf.getLeftSize())
			{
				return TdrError.ErrorType.TDR_ERR_SHORT_BUF_FOR_READ;
			}
			if (1u > num)
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
			}
			errorType = srcBuf.readCString(ref this.szPrivilegeIcon_ByteArray, (int)num);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			int num2 = TdrTypeUtil.cstrlen(this.szPrivilegeIcon_ByteArray) + 1;
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
			if (cutVer == 0u || ResMentorPrivilegeIcon.CURRVERSION < cutVer)
			{
				cutVer = ResMentorPrivilegeIcon.CURRVERSION;
			}
			if (ResMentorPrivilegeIcon.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			int num = 1;
			if (this.szPrivilegeIcon_ByteArray.GetLength(0) < num)
			{
				this.szPrivilegeIcon_ByteArray = new byte[ResMentorPrivilegeIcon.LENGTH_szPrivilegeIcon];
			}
			TdrError.ErrorType errorType = srcBuf.readCString(ref this.szPrivilegeIcon_ByteArray, num);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			this.TransferData();
			return errorType;
		}
	}
}
