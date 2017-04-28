using System;
using System.Runtime.InteropServices;
using System.Text;

public static class TssSdk
{
	public enum EUINTYPE
	{
		UIN_TYPE_INT = 1,
		UIN_TYPE_STR
	}

	public enum EAPPIDTYPE
	{
		APP_ID_TYPE_INT = 1,
		APP_ID_TYPE_STR
	}

	public enum EENTRYID
	{
		ENTRY_ID_QQ = 1,
		ENTRY_ID_WX,
		ENTRT_ID_FACEBOOK,
		ENTRY_ID_TWITTER,
		ENTRY_ID_LINE,
		ENTRY_ID_WHATSAPP,
		ENTRY_ID_OTHERS = 99
	}

	public enum EGAMESTATUS
	{
		GAME_STATUS_FRONTEND = 1,
		GAME_STATUS_BACKEND
	}

	public enum AntiEncryptResult
	{
		ANTI_ENCRYPT_OK,
		ANTI_NOT_NEED_ENCRYPT
	}

	public enum AntiDecryptResult
	{
		ANTI_DECRYPT_OK,
		ANTI_DECRYPT_FAIL
	}

	[StructLayout(LayoutKind.Sequential)]
	public class AntiDataInfo
	{
		public ushort anti_data_len;

		public IntPtr anti_data;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class InitInfo
	{
		public uint size_;

		public uint game_id_;

		public IntPtr send_data_to_svr;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class GameStatusInfo
	{
		public uint size_;

		public uint game_status_;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class UserInfoEx
	{
		public int size;

		public uint entrance_id;

		public uint uin_type;

		public byte uin_00;

		public byte uin_01;

		public byte uin_02;

		public byte uin_03;

		public byte uin_04;

		public byte uin_05;

		public byte uin_06;

		public byte uin_07;

		public byte uin_08;

		public byte uin_09;

		public byte uin_10;

		public byte uin_11;

		public byte uin_12;

		public byte uin_13;

		public byte uin_14;

		public byte uin_15;

		public byte uin_16;

		public byte uin_17;

		public byte uin_18;

		public byte uin_19;

		public byte uin_20;

		public byte uin_21;

		public byte uin_22;

		public byte uin_23;

		public byte uin_24;

		public byte uin_25;

		public byte uin_26;

		public byte uin_27;

		public byte uin_28;

		public byte uin_29;

		public byte uin_30;

		public byte uin_31;

		public byte uin_32;

		public byte uin_33;

		public byte uin_34;

		public byte uin_35;

		public byte uin_36;

		public byte uin_37;

		public byte uin_38;

		public byte uin_39;

		public byte uin_40;

		public byte uin_41;

		public byte uin_42;

		public byte uin_43;

		public byte uin_44;

		public byte uin_45;

		public byte uin_46;

		public byte uin_47;

		public byte uin_48;

		public byte uin_49;

		public byte uin_50;

		public byte uin_51;

		public byte uin_52;

		public byte uin_53;

		public byte uin_54;

		public byte uin_55;

		public byte uin_56;

		public byte uin_57;

		public byte uin_58;

		public byte uin_59;

		public byte uin_60;

		public byte uin_61;

		public byte uin_62;

		public byte uin_63;

		public uint appid_type;

		public byte appid_00;

		public byte appid_01;

		public byte appid_02;

		public byte appid_03;

		public byte appid_04;

		public byte appid_05;

		public byte appid_06;

		public byte appid_07;

		public byte appid_08;

		public byte appid_09;

		public byte appid_10;

		public byte appid_11;

		public byte appid_12;

		public byte appid_13;

		public byte appid_14;

		public byte appid_15;

		public byte appid_16;

		public byte appid_17;

		public byte appid_18;

		public byte appid_19;

		public byte appid_20;

		public byte appid_21;

		public byte appid_22;

		public byte appid_23;

		public byte appid_24;

		public byte appid_25;

		public byte appid_26;

		public byte appid_27;

		public byte appid_28;

		public byte appid_29;

		public byte appid_30;

		public byte appid_31;

		public byte appid_32;

		public byte appid_33;

		public byte appid_34;

		public byte appid_35;

		public byte appid_36;

		public byte appid_37;

		public byte appid_38;

		public byte appid_39;

		public byte appid_40;

		public byte appid_41;

		public byte appid_42;

		public byte appid_43;

		public byte appid_44;

		public byte appid_45;

		public byte appid_46;

		public byte appid_47;

		public byte appid_48;

		public byte appid_49;

		public byte appid_50;

		public byte appid_51;

		public byte appid_52;

		public byte appid_53;

		public byte appid_54;

		public byte appid_55;

		public byte appid_56;

		public byte appid_57;

		public byte appid_58;

		public byte appid_59;

		public byte appid_60;

		public byte appid_61;

		public byte appid_62;

		public byte appid_63;

		public uint world_id;

		public byte role_id_00;

		public byte role_id_01;

		public byte role_id_02;

		public byte role_id_03;

		public byte role_id_04;

		public byte role_id_05;

		public byte role_id_06;

		public byte role_id_07;

		public byte role_id_08;

		public byte role_id_09;

		public byte role_id_10;

		public byte role_id_11;

		public byte role_id_12;

		public byte role_id_13;

		public byte role_id_14;

		public byte role_id_15;

		public byte role_id_16;

		public byte role_id_17;

		public byte role_id_18;

		public byte role_id_19;

		public byte role_id_20;

		public byte role_id_21;

		public byte role_id_22;

		public byte role_id_23;

		public byte role_id_24;

		public byte role_id_25;

		public byte role_id_26;

		public byte role_id_27;

		public byte role_id_28;

		public byte role_id_29;

		public byte role_id_30;

		public byte role_id_31;

		public byte role_id_32;

		public byte role_id_33;

		public byte role_id_34;

		public byte role_id_35;

		public byte role_id_36;

		public byte role_id_37;

		public byte role_id_38;

		public byte role_id_39;

		public byte role_id_40;

		public byte role_id_41;

		public byte role_id_42;

		public byte role_id_43;

		public byte role_id_44;

		public byte role_id_45;

		public byte role_id_46;

		public byte role_id_47;

		public byte role_id_48;

		public byte role_id_49;

		public byte role_id_50;

		public byte role_id_51;

		public byte role_id_52;

		public byte role_id_53;

		public byte role_id_54;

		public byte role_id_55;

		public byte role_id_56;

		public byte role_id_57;

		public byte role_id_58;

		public byte role_id_59;

		public byte role_id_60;

		public byte role_id_61;

		public byte role_id_62;

		public byte role_id_63;
	}

	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public class EncryptPkgInfo
	{
		[FieldOffset(0)]
		public int cmd_id_;

		[FieldOffset(4)]
		public IntPtr game_pkg_;

		[FieldOffset(8)]
		public uint game_pkg_len_;

		[FieldOffset(12)]
		public IntPtr encrpty_data_;

		[FieldOffset(16)]
		public uint encrypt_data_len_;
	}

	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public class DecryptPkgInfo
	{
		[FieldOffset(0)]
		public IntPtr encrypt_data_;

		[FieldOffset(4)]
		public uint encrypt_data_len;

		[FieldOffset(8)]
		public IntPtr game_pkg_;

		[FieldOffset(12)]
		public uint game_pkg_len_;
	}

	public static bool Is64bit()
	{
		return IntPtr.get_Size() == 8;
	}

	public static bool Is32bit()
	{
		return IntPtr.get_Size() == 4;
	}

	public static void TssSdkInit(uint gameId)
	{
		TssSdk.InitInfo initInfo = new TssSdk.InitInfo();
		initInfo.size_ = (uint)Marshal.SizeOf(initInfo);
		initInfo.game_id_ = gameId;
		initInfo.send_data_to_svr = (IntPtr)0;
		TssSdk.tss_sdk_init(initInfo);
		TssSdk.tss_enable_get_report_data();
		TssSdk.tss_log_str(TssSdkVersion.GetSdkVersion());
		TssSdk.tss_log_str(TssSdtVersion.GetSdtVersion());
		BugtraceAgent.EnableExceptionHandler();
	}

	public static void TssSdkSetGameStatus(TssSdk.EGAMESTATUS gameStatus)
	{
		TssSdk.GameStatusInfo gameStatusInfo = new TssSdk.GameStatusInfo();
		gameStatusInfo.size_ = (uint)Marshal.SizeOf(gameStatusInfo);
		gameStatusInfo.game_status_ = (uint)gameStatus;
		TssSdk.tss_sdk_setgamestatus(gameStatusInfo);
	}

	public static void TssSdkSetUserInfo(TssSdk.EENTRYID entryId, string uin, string appId)
	{
		TssSdk.TssSdkSetUserInfoEx(entryId, uin, appId, 0u, "0");
	}

	public static byte[] String2Bytes(string str)
	{
		return Encoding.get_ASCII().GetBytes(str);
	}

	public static void TssSdkSetUserInfoEx(TssSdk.EENTRYID entryId, string uin, string appId, uint worldId, string roleId)
	{
		TssSdk.UserInfoEx userInfoEx = new TssSdk.UserInfoEx();
		userInfoEx.size = Marshal.SizeOf(userInfoEx);
		userInfoEx.entrance_id = (uint)entryId;
		userInfoEx.uin_type = 2u;
		byte[] array = new byte[64];
		byte[] array2 = TssSdk.String2Bytes(uin);
		int i = 0;
		while (i < array2.Length && i < 64)
		{
			array[i] = array2[i];
			i++;
		}
		while (i < 64)
		{
			array[i] = 0;
			i++;
		}
		userInfoEx.uin_00 = array[0];
		userInfoEx.uin_01 = array[1];
		userInfoEx.uin_02 = array[2];
		userInfoEx.uin_03 = array[3];
		userInfoEx.uin_04 = array[4];
		userInfoEx.uin_05 = array[5];
		userInfoEx.uin_06 = array[6];
		userInfoEx.uin_07 = array[7];
		userInfoEx.uin_08 = array[8];
		userInfoEx.uin_09 = array[9];
		userInfoEx.uin_10 = array[10];
		userInfoEx.uin_11 = array[11];
		userInfoEx.uin_12 = array[12];
		userInfoEx.uin_13 = array[13];
		userInfoEx.uin_14 = array[14];
		userInfoEx.uin_15 = array[15];
		userInfoEx.uin_16 = array[16];
		userInfoEx.uin_17 = array[17];
		userInfoEx.uin_18 = array[18];
		userInfoEx.uin_19 = array[19];
		userInfoEx.uin_20 = array[20];
		userInfoEx.uin_21 = array[21];
		userInfoEx.uin_22 = array[22];
		userInfoEx.uin_23 = array[23];
		userInfoEx.uin_24 = array[24];
		userInfoEx.uin_25 = array[25];
		userInfoEx.uin_26 = array[26];
		userInfoEx.uin_27 = array[27];
		userInfoEx.uin_28 = array[28];
		userInfoEx.uin_29 = array[29];
		userInfoEx.uin_30 = array[30];
		userInfoEx.uin_31 = array[31];
		userInfoEx.uin_32 = array[32];
		userInfoEx.uin_33 = array[33];
		userInfoEx.uin_34 = array[34];
		userInfoEx.uin_35 = array[35];
		userInfoEx.uin_36 = array[36];
		userInfoEx.uin_37 = array[37];
		userInfoEx.uin_38 = array[38];
		userInfoEx.uin_39 = array[39];
		userInfoEx.uin_40 = array[40];
		userInfoEx.uin_41 = array[41];
		userInfoEx.uin_42 = array[42];
		userInfoEx.uin_43 = array[43];
		userInfoEx.uin_44 = array[44];
		userInfoEx.uin_45 = array[45];
		userInfoEx.uin_46 = array[46];
		userInfoEx.uin_47 = array[47];
		userInfoEx.uin_48 = array[48];
		userInfoEx.uin_49 = array[49];
		userInfoEx.uin_50 = array[50];
		userInfoEx.uin_51 = array[51];
		userInfoEx.uin_52 = array[52];
		userInfoEx.uin_53 = array[53];
		userInfoEx.uin_54 = array[54];
		userInfoEx.uin_55 = array[55];
		userInfoEx.uin_56 = array[56];
		userInfoEx.uin_57 = array[57];
		userInfoEx.uin_58 = array[58];
		userInfoEx.uin_59 = array[59];
		userInfoEx.uin_60 = array[60];
		userInfoEx.uin_61 = array[61];
		userInfoEx.uin_62 = array[62];
		userInfoEx.uin_63 = array[63];
		userInfoEx.appid_type = 2u;
		byte[] array3 = TssSdk.String2Bytes(appId);
		i = 0;
		while (i < array3.Length && i < 64)
		{
			array[i] = array3[i];
			i++;
		}
		while (i < 64)
		{
			array[i] = 0;
			i++;
		}
		userInfoEx.appid_00 = array[0];
		userInfoEx.appid_01 = array[1];
		userInfoEx.appid_02 = array[2];
		userInfoEx.appid_03 = array[3];
		userInfoEx.appid_04 = array[4];
		userInfoEx.appid_05 = array[5];
		userInfoEx.appid_06 = array[6];
		userInfoEx.appid_07 = array[7];
		userInfoEx.appid_08 = array[8];
		userInfoEx.appid_09 = array[9];
		userInfoEx.appid_10 = array[10];
		userInfoEx.appid_11 = array[11];
		userInfoEx.appid_12 = array[12];
		userInfoEx.appid_13 = array[13];
		userInfoEx.appid_14 = array[14];
		userInfoEx.appid_15 = array[15];
		userInfoEx.appid_16 = array[16];
		userInfoEx.appid_17 = array[17];
		userInfoEx.appid_18 = array[18];
		userInfoEx.appid_19 = array[19];
		userInfoEx.appid_20 = array[20];
		userInfoEx.appid_21 = array[21];
		userInfoEx.appid_22 = array[22];
		userInfoEx.appid_23 = array[23];
		userInfoEx.appid_24 = array[24];
		userInfoEx.appid_25 = array[25];
		userInfoEx.appid_26 = array[26];
		userInfoEx.appid_27 = array[27];
		userInfoEx.appid_28 = array[28];
		userInfoEx.appid_29 = array[29];
		userInfoEx.appid_30 = array[30];
		userInfoEx.appid_31 = array[31];
		userInfoEx.appid_32 = array[32];
		userInfoEx.appid_33 = array[33];
		userInfoEx.appid_34 = array[34];
		userInfoEx.appid_35 = array[35];
		userInfoEx.appid_36 = array[36];
		userInfoEx.appid_37 = array[37];
		userInfoEx.appid_38 = array[38];
		userInfoEx.appid_39 = array[39];
		userInfoEx.appid_40 = array[40];
		userInfoEx.appid_41 = array[41];
		userInfoEx.appid_42 = array[42];
		userInfoEx.appid_43 = array[43];
		userInfoEx.appid_44 = array[44];
		userInfoEx.appid_45 = array[45];
		userInfoEx.appid_46 = array[46];
		userInfoEx.appid_47 = array[47];
		userInfoEx.appid_48 = array[48];
		userInfoEx.appid_49 = array[49];
		userInfoEx.appid_50 = array[50];
		userInfoEx.appid_51 = array[51];
		userInfoEx.appid_52 = array[52];
		userInfoEx.appid_53 = array[53];
		userInfoEx.appid_54 = array[54];
		userInfoEx.appid_55 = array[55];
		userInfoEx.appid_56 = array[56];
		userInfoEx.appid_57 = array[57];
		userInfoEx.appid_58 = array[58];
		userInfoEx.appid_59 = array[59];
		userInfoEx.appid_60 = array[60];
		userInfoEx.appid_61 = array[61];
		userInfoEx.appid_62 = array[62];
		userInfoEx.appid_63 = array[63];
		userInfoEx.world_id = worldId;
		byte[] array4 = TssSdk.String2Bytes(roleId);
		for (i = 0; i < array4.Length; i++)
		{
			array[i] = array4[i];
		}
		while (i < 64)
		{
			array[i] = 0;
			i++;
		}
		userInfoEx.role_id_00 = array[0];
		userInfoEx.role_id_01 = array[1];
		userInfoEx.role_id_02 = array[2];
		userInfoEx.role_id_03 = array[3];
		userInfoEx.role_id_04 = array[4];
		userInfoEx.role_id_05 = array[5];
		userInfoEx.role_id_06 = array[6];
		userInfoEx.role_id_07 = array[7];
		userInfoEx.role_id_08 = array[8];
		userInfoEx.role_id_09 = array[9];
		userInfoEx.role_id_10 = array[10];
		userInfoEx.role_id_11 = array[11];
		userInfoEx.role_id_12 = array[12];
		userInfoEx.role_id_13 = array[13];
		userInfoEx.role_id_14 = array[14];
		userInfoEx.role_id_15 = array[15];
		userInfoEx.role_id_16 = array[16];
		userInfoEx.role_id_17 = array[17];
		userInfoEx.role_id_18 = array[18];
		userInfoEx.role_id_19 = array[19];
		userInfoEx.role_id_20 = array[20];
		userInfoEx.role_id_21 = array[21];
		userInfoEx.role_id_22 = array[22];
		userInfoEx.role_id_23 = array[23];
		userInfoEx.role_id_24 = array[24];
		userInfoEx.role_id_25 = array[25];
		userInfoEx.role_id_26 = array[26];
		userInfoEx.role_id_27 = array[27];
		userInfoEx.role_id_28 = array[28];
		userInfoEx.role_id_29 = array[29];
		userInfoEx.role_id_30 = array[30];
		userInfoEx.role_id_31 = array[31];
		userInfoEx.role_id_32 = array[32];
		userInfoEx.role_id_33 = array[33];
		userInfoEx.role_id_34 = array[34];
		userInfoEx.role_id_35 = array[35];
		userInfoEx.role_id_36 = array[36];
		userInfoEx.role_id_37 = array[37];
		userInfoEx.role_id_38 = array[38];
		userInfoEx.role_id_39 = array[39];
		userInfoEx.role_id_40 = array[40];
		userInfoEx.role_id_41 = array[41];
		userInfoEx.role_id_42 = array[42];
		userInfoEx.role_id_43 = array[43];
		userInfoEx.role_id_44 = array[44];
		userInfoEx.role_id_45 = array[45];
		userInfoEx.role_id_46 = array[46];
		userInfoEx.role_id_47 = array[47];
		userInfoEx.role_id_48 = array[48];
		userInfoEx.role_id_49 = array[49];
		userInfoEx.role_id_50 = array[50];
		userInfoEx.role_id_51 = array[51];
		userInfoEx.role_id_52 = array[52];
		userInfoEx.role_id_53 = array[53];
		userInfoEx.role_id_54 = array[54];
		userInfoEx.role_id_55 = array[55];
		userInfoEx.role_id_56 = array[56];
		userInfoEx.role_id_57 = array[57];
		userInfoEx.role_id_58 = array[58];
		userInfoEx.role_id_59 = array[59];
		userInfoEx.role_id_60 = array[60];
		userInfoEx.role_id_61 = array[61];
		userInfoEx.role_id_62 = array[62];
		userInfoEx.role_id_63 = array[63];
		TssSdk.tss_sdk_setuserinfo_ex(userInfoEx);
	}

	[DllImport("tersafe")]
	private static extern void tss_sdk_init(TssSdk.InitInfo info);

	[DllImport("tersafe")]
	private static extern void tss_log_str(string sdk_version);

	[DllImport("tersafe")]
	private static extern void tss_sdk_setuserinfo_ex(TssSdk.UserInfoEx info);

	[DllImport("tersafe")]
	private static extern void tss_sdk_setgamestatus(TssSdk.GameStatusInfo info);

	[DllImport("tersafe")]
	private static extern void tss_sdk_rcv_anti_data(IntPtr info);

	public static void TssSdkRcvAntiData(byte[] data, ushort length)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(2 + IntPtr.get_Size());
		if (intPtr != IntPtr.Zero)
		{
			Marshal.WriteInt16(intPtr, 0, (short)length);
			IntPtr intPtr2 = Marshal.AllocHGlobal(data.Length);
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr2, data.Length);
				Marshal.WriteIntPtr(intPtr, 2, intPtr2);
				TssSdk.tss_sdk_rcv_anti_data(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			Marshal.FreeHGlobal(intPtr);
		}
	}

	[DllImport("tersafe")]
	private static extern TssSdk.AntiEncryptResult tss_sdk_encryptpacket(TssSdk.EncryptPkgInfo info);

	public static TssSdk.AntiEncryptResult TssSdkEncrypt(int cmd_id, byte[] src, uint src_len, ref byte[] tar, ref uint tar_len)
	{
		TssSdk.AntiEncryptResult result = TssSdk.AntiEncryptResult.ANTI_NOT_NEED_ENCRYPT;
		GCHandle gCHandle = GCHandle.Alloc(src, 3);
		GCHandle gCHandle2 = GCHandle.Alloc(tar, 3);
		if (gCHandle.get_IsAllocated() && gCHandle2.get_IsAllocated())
		{
			TssSdk.EncryptPkgInfo encryptPkgInfo = new TssSdk.EncryptPkgInfo();
			encryptPkgInfo.cmd_id_ = cmd_id;
			encryptPkgInfo.game_pkg_ = gCHandle.AddrOfPinnedObject();
			encryptPkgInfo.game_pkg_len_ = src_len;
			encryptPkgInfo.encrpty_data_ = gCHandle2.AddrOfPinnedObject();
			encryptPkgInfo.encrypt_data_len_ = tar_len;
			result = TssSdk.tss_sdk_encryptpacket(encryptPkgInfo);
			tar_len = encryptPkgInfo.encrypt_data_len_;
		}
		if (gCHandle.get_IsAllocated())
		{
			gCHandle.Free();
		}
		if (gCHandle2.get_IsAllocated())
		{
			gCHandle2.Free();
		}
		return result;
	}

	[DllImport("tersafe")]
	private static extern TssSdk.AntiDecryptResult tss_sdk_decryptpacket(TssSdk.DecryptPkgInfo info);

	public static TssSdk.AntiDecryptResult TssSdkDecrypt(byte[] src, uint src_len, ref byte[] tar, ref uint tar_len)
	{
		TssSdk.AntiDecryptResult result = TssSdk.AntiDecryptResult.ANTI_DECRYPT_FAIL;
		GCHandle gCHandle = GCHandle.Alloc(src, 3);
		GCHandle gCHandle2 = GCHandle.Alloc(tar, 3);
		if (gCHandle.get_IsAllocated() && gCHandle2.get_IsAllocated())
		{
			TssSdk.DecryptPkgInfo decryptPkgInfo = new TssSdk.DecryptPkgInfo();
			decryptPkgInfo.encrypt_data_ = gCHandle.AddrOfPinnedObject();
			decryptPkgInfo.encrypt_data_len = src_len;
			decryptPkgInfo.game_pkg_ = gCHandle2.AddrOfPinnedObject();
			decryptPkgInfo.game_pkg_len_ = tar_len;
			result = TssSdk.tss_sdk_decryptpacket(decryptPkgInfo);
			tar_len = decryptPkgInfo.game_pkg_len_;
		}
		if (gCHandle.get_IsAllocated())
		{
			gCHandle.Free();
		}
		if (gCHandle2.get_IsAllocated())
		{
			gCHandle2.Free();
		}
		return result;
	}

	[DllImport("tersafe")]
	private static extern void tss_enable_get_report_data();

	[DllImport("tersafe")]
	public static extern IntPtr tss_get_report_data();

	[DllImport("tersafe")]
	public static extern void tss_del_report_data(IntPtr info);
}
