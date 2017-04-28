using System;
using tsf4g_tdr_csharp;

namespace ResData
{
	public class ResSkillFuncCfgInfo : IUnpackable, tsf4g_csharp_interface
	{
		public uint dwSkillFuncType;

		public byte[] szSkillFuncDesc_ByteArray;

		public byte[] szParamInstruction_ByteArray;

		public uint dwOnlyLimitType;

		public uint dwOverlyingMax;

		public string szSkillFuncDesc;

		public string szParamInstruction;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly uint LENGTH_szSkillFuncDesc = 128u;

		public static readonly uint LENGTH_szParamInstruction = 128u;

		public ResSkillFuncCfgInfo()
		{
			this.szSkillFuncDesc_ByteArray = new byte[1];
			this.szParamInstruction_ByteArray = new byte[1];
			this.szSkillFuncDesc = string.Empty;
			this.szParamInstruction = string.Empty;
		}

		private void TransferData()
		{
			this.szSkillFuncDesc = StringHelper.UTF8BytesToString(ref this.szSkillFuncDesc_ByteArray);
			this.szSkillFuncDesc_ByteArray = null;
			this.szParamInstruction = StringHelper.UTF8BytesToString(ref this.szParamInstruction_ByteArray);
			this.szParamInstruction_ByteArray = null;
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
			if (cutVer == 0u || ResSkillFuncCfgInfo.CURRVERSION < cutVer)
			{
				cutVer = ResSkillFuncCfgInfo.CURRVERSION;
			}
			if (ResSkillFuncCfgInfo.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt32(ref this.dwSkillFuncType);
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
			if (num > (uint)this.szSkillFuncDesc_ByteArray.GetLength(0))
			{
				if ((long)num > (long)((ulong)ResSkillFuncCfgInfo.LENGTH_szSkillFuncDesc))
				{
					return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
				}
				this.szSkillFuncDesc_ByteArray = new byte[num];
			}
			if (1u > num)
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
			}
			errorType = srcBuf.readCString(ref this.szSkillFuncDesc_ByteArray, (int)num);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (this.szSkillFuncDesc_ByteArray[(int)(num - 1u)] != 0)
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
			}
			int num2 = TdrTypeUtil.cstrlen(this.szSkillFuncDesc_ByteArray) + 1;
			if ((ulong)num != (ulong)((long)num2))
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
			}
			uint num3 = 0u;
			errorType = srcBuf.readUInt32(ref num3);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (num3 > (uint)srcBuf.getLeftSize())
			{
				return TdrError.ErrorType.TDR_ERR_SHORT_BUF_FOR_READ;
			}
			if (num3 > (uint)this.szParamInstruction_ByteArray.GetLength(0))
			{
				if ((long)num3 > (long)((ulong)ResSkillFuncCfgInfo.LENGTH_szParamInstruction))
				{
					return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
				}
				this.szParamInstruction_ByteArray = new byte[num3];
			}
			if (1u > num3)
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
			}
			errorType = srcBuf.readCString(ref this.szParamInstruction_ByteArray, (int)num3);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (this.szParamInstruction_ByteArray[(int)(num3 - 1u)] != 0)
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
			}
			int num4 = TdrTypeUtil.cstrlen(this.szParamInstruction_ByteArray) + 1;
			if ((ulong)num3 != (ulong)((long)num4))
			{
				return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
			}
			errorType = srcBuf.readUInt32(ref this.dwOnlyLimitType);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwOverlyingMax);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
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
			if (cutVer == 0u || ResSkillFuncCfgInfo.CURRVERSION < cutVer)
			{
				cutVer = ResSkillFuncCfgInfo.CURRVERSION;
			}
			if (ResSkillFuncCfgInfo.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt32(ref this.dwSkillFuncType);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			int num = 128;
			if (this.szSkillFuncDesc_ByteArray.GetLength(0) < num)
			{
				this.szSkillFuncDesc_ByteArray = new byte[ResSkillFuncCfgInfo.LENGTH_szSkillFuncDesc];
			}
			errorType = srcBuf.readCString(ref this.szSkillFuncDesc_ByteArray, num);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			int num2 = 128;
			if (this.szParamInstruction_ByteArray.GetLength(0) < num2)
			{
				this.szParamInstruction_ByteArray = new byte[ResSkillFuncCfgInfo.LENGTH_szParamInstruction];
			}
			errorType = srcBuf.readCString(ref this.szParamInstruction_ByteArray, num2);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwOnlyLimitType);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt32(ref this.dwOverlyingMax);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			this.TransferData();
			return errorType;
		}
	}
}
