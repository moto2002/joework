using Assets.Scripts.Common;
using System;
using tsf4g_tdr_csharp;

namespace CSProtocol
{
	public class COMDT_SELFDEFINE_EQUIP_INFO : ProtocolObject
	{
		public uint dwLastChgHeroId;

		public ushort wHeroNum;

		public COMDT_HERO_EQUIPLIST[] astEquipInfoList;

		public static readonly uint BASEVERSION = 1u;

		public static readonly uint CURRVERSION = 1u;

		public static readonly int CLASS_ID = 119;

		public COMDT_SELFDEFINE_EQUIP_INFO()
		{
			this.astEquipInfoList = new COMDT_HERO_EQUIPLIST[200];
			for (int i = 0; i < 200; i++)
			{
				this.astEquipInfoList[i] = (COMDT_HERO_EQUIPLIST)ProtocolObjectPool.Get(COMDT_HERO_EQUIPLIST.CLASS_ID);
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
			if (cutVer == 0u || COMDT_SELFDEFINE_EQUIP_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_SELFDEFINE_EQUIP_INFO.CURRVERSION;
			}
			if (COMDT_SELFDEFINE_EQUIP_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = destBuf.writeUInt32(this.dwLastChgHeroId);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = destBuf.writeUInt16(this.wHeroNum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (200 < this.wHeroNum)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			if (this.astEquipInfoList.Length < (int)this.wHeroNum)
			{
				return TdrError.ErrorType.TDR_ERR_VAR_ARRAY_CONFLICT;
			}
			for (int i = 0; i < (int)this.wHeroNum; i++)
			{
				errorType = this.astEquipInfoList[i].pack(ref destBuf, cutVer);
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
			if (cutVer == 0u || COMDT_SELFDEFINE_EQUIP_INFO.CURRVERSION < cutVer)
			{
				cutVer = COMDT_SELFDEFINE_EQUIP_INFO.CURRVERSION;
			}
			if (COMDT_SELFDEFINE_EQUIP_INFO.BASEVERSION > cutVer)
			{
				return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
			}
			TdrError.ErrorType errorType = srcBuf.readUInt32(ref this.dwLastChgHeroId);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			errorType = srcBuf.readUInt16(ref this.wHeroNum);
			if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
			{
				return errorType;
			}
			if (200 < this.wHeroNum)
			{
				return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
			}
			for (int i = 0; i < (int)this.wHeroNum; i++)
			{
				errorType = this.astEquipInfoList[i].unpack(ref srcBuf, cutVer);
				if (errorType != TdrError.ErrorType.TDR_NO_ERROR)
				{
					return errorType;
				}
			}
			return errorType;
		}

		public override int GetClassID()
		{
			return COMDT_SELFDEFINE_EQUIP_INFO.CLASS_ID;
		}

		public override void OnRelease()
		{
			this.dwLastChgHeroId = 0u;
			this.wHeroNum = 0;
			if (this.astEquipInfoList != null)
			{
				for (int i = 0; i < this.astEquipInfoList.Length; i++)
				{
					if (this.astEquipInfoList[i] != null)
					{
						this.astEquipInfoList[i].Release();
						this.astEquipInfoList[i] = null;
					}
				}
			}
		}

		public override void OnUse()
		{
			if (this.astEquipInfoList != null)
			{
				for (int i = 0; i < this.astEquipInfoList.Length; i++)
				{
					this.astEquipInfoList[i] = (COMDT_HERO_EQUIPLIST)ProtocolObjectPool.Get(COMDT_HERO_EQUIPLIST.CLASS_ID);
				}
			}
		}
	}
}
