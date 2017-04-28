using Apollo;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameSystem;
using ResData;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Utility
{
	public enum NameResult
	{
		Vaild,
		Null,
		OutOfLength,
		InVaildChar
	}

	public enum enDTFormate
	{
		FULL,
		DATE,
		TIME
	}

	public const long UTC_OFFSET_LOCAL = 28800L;

	public const long UTCTICK_PER_SECONDS = 10000000L;

	public static readonly int MIN_EN_NAME_LEN = 1;

	public static readonly int MAX_EN_NAME_LEN = 12;

	public static uint s_daySecond = 86400u;

	private static ulong[] _DW = new ulong[]
	{
		10000000000uL,
		100000000uL,
		1000000uL,
		10000uL,
		100uL
	};

	private static readonly int CHINESE_CHAR_START = Convert.ToInt32("4e00", 16);

	private static readonly int CHINESE_CHAR_END = Convert.ToInt32("9fff", 16);

	private static readonly char[] s_ban_chars = new char[]
	{
		Convert.ToChar(10),
		Convert.ToChar(13)
	};

	public static int TimeToFrame(float time)
	{
		return (int)Math.Ceiling((double)(time / Time.fixedDeltaTime));
	}

	public static float FrameToTime(int frame)
	{
		return (float)frame * Time.fixedDeltaTime;
	}

	public static Vector3 GetGameObjectSize(GameObject obj)
	{
		Vector3 result = Vector3.zero;
		if (obj.GetComponent<Renderer>() != null)
		{
			result = obj.GetComponent<Renderer>().bounds.size;
		}
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Renderer renderer = componentsInChildren[i];
			Vector3 size = renderer.bounds.size;
			result.x = Math.Max(result.x, size.x);
			result.y = Math.Max(result.y, size.y);
			result.z = Math.Max(result.z, size.z);
		}
		return result;
	}

	public static GameObject FindChild(GameObject p, string path)
	{
		if (p != null)
		{
			Transform transform = p.transform.FindChild(path);
			return (!(transform != null)) ? null : transform.gameObject;
		}
		return null;
	}

	public static GameObject FindChildSafe(GameObject p, string path)
	{
		if (p)
		{
			Transform transform = p.transform.FindChild(path);
			if (transform)
			{
				return transform.gameObject;
			}
		}
		return null;
	}

	public static T GetComponetInChild<T>(GameObject p, string path) where T : MonoBehaviour
	{
		if (p == null || p.transform == null)
		{
			return (T)((object)null);
		}
		Transform transform = p.transform.FindChild(path);
		if (transform == null)
		{
			return (T)((object)null);
		}
		return transform.GetComponent<T>();
	}

	public static GameObject FindChildByName(Component component, string childpath)
	{
		return Utility.FindChildByName(component.gameObject, childpath);
	}

	public static GameObject FindChildByName(GameObject root, string childpath)
	{
		GameObject result = null;
		string[] array = childpath.Split(new char[]
		{
			'/'
		});
		GameObject gameObject = root;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			bool flag = false;
			IEnumerator enumerator = gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.get_Current();
					if (transform.gameObject.name == text)
					{
						gameObject = transform.gameObject;
						flag = true;
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (!flag)
			{
				break;
			}
		}
		if (gameObject != root)
		{
			result = gameObject;
		}
		return result;
	}

	public static void SetChildrenActive(GameObject root, bool active)
	{
		IEnumerator enumerator = root.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.get_Current();
				if (transform != root.transform)
				{
					transform.gameObject.CustomSetActive(active);
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static void StringToByteArray(string str, ref byte[] buffer)
	{
		byte[] bytes = Encoding.get_Default().GetBytes(str);
		Array.Copy(bytes, buffer, Math.Min(bytes.Length, buffer.Length));
		buffer[buffer.Length - 1] = 0;
	}

	public static string UTF8Convert(string str)
	{
		return str;
	}

	public static string UTF8Convert(byte[] p)
	{
		return StringHelper.UTF8BytesToString(ref p);
	}

	public static byte[] BytesConvert(string s)
	{
		return Encoding.get_UTF8().GetBytes(s.TrimEnd(new char[1]));
	}

	public static byte[] SByteArrToByte(sbyte[] p)
	{
		byte[] array = new byte[p.Length];
		for (int i = 0; i < p.Length; i++)
		{
			array[i] = (byte)p[i];
		}
		return array;
	}

	public static string UTF8Convert(sbyte[] p, int len)
	{
		byte[] p2 = Utility.SByteArrToByte(p);
		return Utility.UTF8Convert(p2);
	}

	public static DateTime ToUtcTime2Local(long secondsFromUtcStart)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		return dateTime.AddTicks((secondsFromUtcStart + 28800L) * 10000000L);
	}

	public static uint ToUtcSeconds(DateTime dateTime)
	{
		DateTime dateTime2 = new DateTime(1970, 1, 1);
		if (dateTime.CompareTo(dateTime2) < 0)
		{
			return 0u;
		}
		if ((dateTime - dateTime2).get_TotalSeconds() > 28800.0)
		{
			return (uint)(dateTime - dateTime2).get_TotalSeconds() - 28800u;
		}
		return 0u;
	}

	public static string GetUtcToLocalTimeStringFormat(ulong secondsFromUtcStart, string format)
	{
		return Utility.ToUtcTime2Local((long)secondsFromUtcStart).ToString(format);
	}

	public static uint GetLocalTimeFormattedStrToUtc(string localTimeFormattedStr)
	{
		DateTime dateTime = new DateTime(Convert.ToInt32(localTimeFormattedStr.Substring(0, 4)), Convert.ToInt32(localTimeFormattedStr.Substring(4, 2)), Convert.ToInt32(localTimeFormattedStr.Substring(6, 2)), Convert.ToInt32(localTimeFormattedStr.Substring(8, 2)), Convert.ToInt32(localTimeFormattedStr.Substring(10, 2)), Convert.ToInt32(localTimeFormattedStr.Substring(12, 2)));
		return Utility.ToUtcSeconds(dateTime);
	}

	public static uint GetGlobalRefreshTimeSeconds()
	{
		uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(10u).dwConfValue;
		int num = Utility.Hours2Second((int)(dwConfValue / 100u)) + Utility.Minutes2Seconds((int)(dwConfValue % 100u));
		DateTime dateTime = Utility.ToUtcTime2Local((long)CRoleInfo.GetCurrentUTCTime());
		DateTime dateTime2 = new DateTime(dateTime.get_Year(), dateTime.get_Month(), dateTime.get_Day(), 0, 0, 0, 1);
		DateTime dateTime3 = dateTime2.AddSeconds((double)num);
		return Utility.ToUtcSeconds(dateTime3);
	}

	public static uint GetLatestGlobalRefreshTimeSeconds()
	{
		int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
		uint globalRefreshTimeSeconds = Utility.GetGlobalRefreshTimeSeconds();
		if ((long)currentUTCTime >= (long)((ulong)globalRefreshTimeSeconds))
		{
			return globalRefreshTimeSeconds;
		}
		return globalRefreshTimeSeconds - 86400u;
	}

	public static uint GetTodayStartTimeSeconds()
	{
		DateTime dateTime = Utility.ToUtcTime2Local((long)CRoleInfo.GetCurrentUTCTime());
		DateTime dateTime2 = new DateTime(dateTime.get_Year(), dateTime.get_Month(), dateTime.get_Day(), 0, 0, 0, 1);
		return Utility.ToUtcSeconds(dateTime2);
	}

	public static uint GetNewDayDeltaSec(int nowSec)
	{
		DateTime dateTime = Utility.ToUtcTime2Local((long)nowSec);
		DateTime dateTime2 = new DateTime(dateTime.get_Year(), dateTime.get_Month(), dateTime.get_Day(), 0, 0, 0, 1);
		DateTime dateTime3 = dateTime2.AddSeconds(86400.0);
		return (uint)(dateTime3 - dateTime).get_TotalSeconds();
	}

	public static bool IsSameDay(long secondsFromUtcStart1, long secondsFromUtcStart2)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		DateTime dateTime2 = dateTime.AddTicks((secondsFromUtcStart1 + 28800L) * 10000000L);
		DateTime dateTime3 = dateTime.AddTicks((secondsFromUtcStart2 + 28800L) * 10000000L);
		return DateTime.Equals(dateTime2.get_Date(), dateTime3.get_Date());
	}

	public static bool IsSameWeek(DateTime dtmS, DateTime dtmE)
	{
		double totalDays = (dtmE - dtmS).get_TotalDays();
		int num = Convert.ToInt32(dtmE.get_DayOfWeek());
		if (num == 0)
		{
			num = 7;
		}
		return totalDays < 7.0 && totalDays < (double)num;
	}

	public static bool IsSameWeek(long secondsFromUtcStart1, long secondsFromUtcStart2)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		DateTime dtmS = dateTime.AddTicks((secondsFromUtcStart1 + 28800L) * 10000000L);
		DateTime dtmE = dateTime.AddTicks((secondsFromUtcStart2 + 28800L) * 10000000L);
		return Utility.IsSameWeek(dtmS, dtmE);
	}

	public static long GetZeroBaseSecond(long utcSec)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		DateTime dateTime2 = dateTime.AddTicks((utcSec + 28800L) * 10000000L);
		DateTime dateTime3 = new DateTime(dateTime2.get_Year(), dateTime2.get_Month(), dateTime2.get_Day(), 0, 0, 0);
		return (long)(dateTime3 - dateTime).get_TotalSeconds() - 28800L;
	}

	public static int Hours2Second(int hour)
	{
		return hour * 60 * 60;
	}

	public static int Minutes2Seconds(int min)
	{
		return min * 60;
	}

	public static string GetTimeBeforString(long beforSecondsFromUtc, long nowSecondsFromUtc)
	{
		string result = string.Empty;
		TimeSpan timeSpan = new TimeSpan((beforSecondsFromUtc + 28800L) * 10000000L);
		TimeSpan timeSpan2 = new TimeSpan((nowSecondsFromUtc + 28800L) * 10000000L);
		int num = (int)timeSpan2.get_TotalDays() - (int)timeSpan.get_TotalDays();
		if (num >= 1)
		{
			result = Singleton<CTextManager>.GetInstance().GetText("Time_DayBefore", new string[]
			{
				num.ToString()
			});
		}
		else
		{
			result = Singleton<CTextManager>.GetInstance().GetText("Time_Today");
		}
		return result;
	}

	public static string GetRecentOnlineTimeString(int recentOnlineTime)
	{
		string result = string.Empty;
		if (recentOnlineTime == 0)
		{
			return result;
		}
		int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
		if (currentUTCTime > recentOnlineTime)
		{
			TimeSpan timeSpan = new TimeSpan(0, 0, currentUTCTime - recentOnlineTime);
			if (timeSpan.get_Days() == 0)
			{
				if (timeSpan.get_Hours() == 0)
				{
					if (timeSpan.get_Minutes() == 0)
					{
						result = Singleton<CTextManager>.GetInstance().GetText("Guild_Tips_lastTime_default");
					}
					else
					{
						result = Singleton<CTextManager>.GetInstance().GetText("Guild_Tips_lastTime_min", new string[]
						{
							timeSpan.get_Minutes().ToString()
						});
					}
				}
				else
				{
					result = Singleton<CTextManager>.GetInstance().GetText("Guild_Tips_lastTime_hour_min", new string[]
					{
						timeSpan.get_Hours().ToString(),
						timeSpan.get_Minutes().ToString()
					});
				}
			}
			else
			{
				result = Singleton<CTextManager>.GetInstance().GetText("Guild_Tips_lastTime_day", new string[]
				{
					timeSpan.get_Days().ToString()
				});
			}
		}
		return result;
	}

	public static string ProtErrCodeToStr(int ProtId, int ErrCode)
	{
		string result = string.Empty;
		switch (ProtId)
		{
		case 1902:
		case 1904:
		case 1906:
		case 1908:
		case 1910:
			switch (ErrCode)
			{
			case 1:
				result = Singleton<CTextManager>.GetInstance().GetText("SecurePwd_State_Error");
				break;
			case 2:
				result = Singleton<CTextManager>.GetInstance().GetText("SecurePwd_Invalid_Operation");
				break;
			case 3:
				result = Singleton<CTextManager>.GetInstance().GetText("SecurePwd_Invalid_Pwd");
				break;
			case 4:
				result = Singleton<CTextManager>.GetInstance().GetText("SecurePwd_Invalid_Force_Close_Req");
				break;
			}
			return result;
		case 1903:
		case 1905:
		case 1907:
		case 1909:
			IL_38:
			switch (ProtId)
			{
			case 5403:
			case 5405:
			case 5407:
				switch (ErrCode)
				{
				case 200:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_DataErr");
					break;
				case 201:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_CannotCrossPlat");
					break;
				case 202:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_HasMentor");
					break;
				case 203:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_RefusedRequest");
					break;
				case 204:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_BeyondRequestLimit");
					break;
				case 205:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_RequestDuplicated");
					break;
				case 206:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_ApprenticeCountTooMuch");
					break;
				case 207:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_AlreadyMentor");
					break;
				case 208:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_HasMentorOrNotGraduated");
					break;
				case 209:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_NoRequest");
					break;
				case 210:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_NotMentor");
					break;
				case 211:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_CannotDismissInAday");
					break;
				case 212:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_Level2LowAsMentor");
					break;
				case 213:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_Level2HighAsApprentice");
					break;
				case 214:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_Grade2LowAsMentor");
					break;
				case 215:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_Transaction");
					break;
				case 216:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_NotStudent");
					break;
				case 217:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_MasterApplySelf");
					break;
				case 218:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_MasterBantime");
					break;
				case 219:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_OTHER_LVL_LOW");
					break;
				case 220:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_OTHER_LVL_HIGH");
					break;
				case 221:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_OTHER_GRADE_LOW");
					break;
				case 222:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_REQ_NOTVALID");
					break;
				case 223:
					result = Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_Other_IsStudent");
					break;
				default:
					result = string.Concat(new object[]
					{
						"ProtocolId ",
						ProtId,
						" ErrId ",
						ErrCode,
						" unhandled"
					});
					break;
				}
				return result;
			case 5404:
			case 5406:
				IL_58:
				switch (ProtId)
				{
				case 1022:
				case 1024:
				case 1025:
					goto IL_448;
				case 1023:
					IL_74:
					switch (ProtId)
					{
					case 2011:
						goto IL_6CC;
					case 2012:
					case 2013:
						IL_90:
						switch (ProtId)
						{
						case 2901:
							if (ErrCode == 3)
							{
								result = Singleton<CTextManager>.GetInstance().GetText("Arena_ARENASETBATTLELIST_ERR_FAILED");
							}
							return result;
						case 2902:
						case 2903:
							IL_AC:
							switch (ProtId)
							{
							case 1176:
								switch (ErrCode)
								{
								case 1:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_Sys");
									break;
								case 2:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_ID");
									break;
								case 3:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_Out_Date");
									break;
								case 4:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_Dup");
									break;
								case 5:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_Pay");
									break;
								case 6:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_Money");
									break;
								case 7:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_Exchange_Coupons_Err");
									break;
								case 8:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_Exchange_Coupons_Invalid");
									break;
								case 9:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Recommend_Failed");
									break;
								}
								return result;
							case 1177:
								IL_C4:
								switch (ProtId)
								{
								case 1831:
								case 1833:
									switch (ErrCode)
									{
									case 1:
										result = Singleton<CTextManager>.GetInstance().GetText("CS_PRESENTHEROSKIN_SYS");
										break;
									case 2:
										result = Singleton<CTextManager>.GetInstance().GetText("CS_PRESENTHEROSKIN_LOCK");
										break;
									case 3:
										result = Singleton<CTextManager>.GetInstance().GetText("CS_PRESENTHEROSKIN_NOALLOW");
										break;
									case 4:
										result = Singleton<CTextManager>.GetInstance().GetText("CS_PRESENTHEROSKIN_UNFRIEND");
										break;
									case 5:
										result = Singleton<CTextManager>.GetInstance().GetText("CS_PRESENTHEROSKIN_COINLIMIT");
										break;
									case 6:
										result = Singleton<CTextManager>.GetInstance().GetText("CS_PRESENTHEROSKIN_MAILFAIL");
										break;
									case 10:
										result = Singleton<CTextManager>.GetInstance().GetText("Gift_GiftMax");
										break;
									case 11:
										result = Singleton<CTextManager>.GetInstance().GetText("Gift_GiftNoXin");
										break;
									case 12:
										result = Singleton<CTextManager>.GetInstance().GetText("CS_PRESENTHEROSKIN_INVALID_PLAT");
										break;
									case 13:
										result = Singleton<CTextManager>.GetInstance().GetText("Gift_GiftNotToSelf");
										break;
									case 14:
										result = Singleton<CTextManager>.GetInstance().GetText("SecurePwd_Invalid_Pwd");
										break;
									}
									return result;
								case 1832:
									IL_DC:
									switch (ProtId)
									{
									case 4802:
										switch (ErrCode)
										{
										case 1:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Money_Not_Enough");
											break;
										case 2:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Money_Type_Invalid");
											break;
										case 3:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Lottery_Cnt_Invalid");
											break;
										case 4:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Pay_Failed");
											break;
										case 6:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Pay_Time_Out");
											break;
										case 8:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Other_Err");
											break;
										}
										return result;
									case 4803:
										IL_F4:
										if (ProtId == 1051)
										{
											switch (ErrCode)
											{
											case 1:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_SINGLEGAME_ERR_FAIL");
												break;
											case 2:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_SINGLEGAMEOFARENA_ERR_SELFLOCK");
												break;
											case 3:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_SINGLEGAMEOFARENA_ERR_TARGETLOCK");
												break;
											case 4:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_SINGLEGAMEOFARENA_ERR_TARGETCHG");
												break;
											case 5:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_SINGLEGAMEOFARENA_ERR_NOTFINDTARGET");
												break;
											case 6:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_SINGLEGAMEOFARENA_ERR_OTHERS");
												break;
											case 7:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_ERR_LIMIT_CNT");
												break;
											case 8:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_ERR_LIMIT_CD");
												break;
											case 9:
												result = Singleton<CTextManager>.GetInstance().GetText("Arena_ERR_REWARD");
												break;
											case 11:
												result = Singleton<CTextManager>.GetInstance().GetText("ERR_Freehero_Expire");
												Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.SINGLEGAME_ERR_FREEHERO);
												break;
											}
											return result;
										}
										if (ProtId == 1055)
										{
											switch (ErrCode)
											{
											case 1:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Sweep_Star");
												break;
											case 2:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Sweep_VIP");
												break;
											case 3:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Sweep_AP");
												break;
											case 4:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Sweep_Ticket");
												break;
											case 5:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Sweep_DianQuan");
												break;
											case 7:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Sweep_Other");
												break;
											}
											return result;
										}
										if (ProtId == 1059)
										{
											if (ErrCode == 10)
											{
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Quit_Single_Game");
											}
											return result;
										}
										if (ProtId == 1079)
										{
											goto IL_448;
										}
										if (ProtId == 1194)
										{
											if (ErrCode == 172)
											{
												result = Singleton<CTextManager>.GetInstance().GetText("CS_ERR_SIGNATURE_ILLEGAL");
											}
											return result;
										}
										if (ProtId == 1408)
										{
											switch (ErrCode)
											{
											case 1:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_Not_Open");
												break;
											case 2:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_Not_Close");
												break;
											case 3:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_Buy_Limit");
												break;
											case 4:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_Product_Not_Found");
												break;
											case 5:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_GOODSALREADYBUY");
												break;
											case 6:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_Product_BuyFail");
												break;
											case 7:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_Error_Invalid");
												break;
											case 8:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_ERROR_COINLIMIT");
												break;
											case 9:
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Mystery_Shop_ERROR_STATEERR");
												break;
											}
											return result;
										}
										if (ProtId == 1417)
										{
											if (ErrCode == 1)
											{
												result = Singleton<CTextManager>.GetInstance().GetText("Err_Player_Info_Honor_Use_Not_Have");
											}
											return result;
										}
										if (ProtId == 1431)
										{
											switch (ErrCode)
											{
											case 165:
												result = Singleton<CTextManager>.GetInstance().GetText("CS_ERR_SEND_GUILD_MAIL_AUTH");
												break;
											case 166:
												result = Singleton<CTextManager>.GetInstance().GetText("CS_ERR_SEND_GUILD_MAIL_LIMIT");
												break;
											case 167:
												result = Singleton<CTextManager>.GetInstance().GetText("CS_ERR_SEND_GUILD_MAIL_SUBJECT");
												break;
											case 168:
												result = Singleton<CTextManager>.GetInstance().GetText("CS_ERR_SEND_GUILD_MAIL_CONTENT");
												break;
											}
											return result;
										}
										if (ProtId == 1818)
										{
											switch (ErrCode)
											{
											case 1:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_AlreadyHave");
												break;
											case 2:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_WrongMethod");
												break;
											case 3:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_Invalid");
												break;
											case 4:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_WrongTime");
												break;
											case 5:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_WrongSale");
												break;
											case 6:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_Money");
												break;
											case 7:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_Full");
												break;
											case 8:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_InitFail");
												break;
											case 9:
												result = Singleton<CTextManager>.GetInstance().GetText("BuyHero_Error_Other");
												break;
											}
											return result;
										}
										if (ProtId == 2023 || ProtId == 2030)
										{
											goto IL_6CC;
										}
										if (ProtId == 2607)
										{
											if (ErrCode == 163)
											{
												result = Singleton<CTextManager>.GetInstance().GetText("CS_ERR_FRIEND_INVALID_PLAT");
											}
											return result;
										}
										if (ProtId != 5328)
										{
											return result;
										}
										result = Singleton<CTextManager>.GetInstance().GetText("Err_JudgeFail");
										return result;
									case 4804:
										switch (ErrCode)
										{
										case 1:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Money_Not_Enough");
											break;
										case 2:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Extern_Period_Index_Invalid");
											break;
										case 3:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Extern_No_Reach");
											break;
										case 4:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Extern_Drawed");
											break;
										case 5:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Extern_Reward_Id");
											break;
										case 6:
											result = Singleton<CTextManager>.GetInstance().GetText("Err_Roulette_Other_Err");
											break;
										}
										return result;
									}
									goto IL_F4;
								}
								goto IL_DC;
							case 1178:
								switch (ErrCode)
								{
								case 1:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Roulette_Server");
									break;
								case 2:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Roulette_ID");
									break;
								case 3:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Roulette_Out_Date");
									break;
								case 4:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Roulette_Money");
									break;
								case 5:
									result = Singleton<CTextManager>.GetInstance().GetText("Err_Recommend_Roulette_No_Data");
									break;
								}
								return result;
							}
							goto IL_C4;
						case 2904:
							if (ErrCode != 1)
							{
								if (ErrCode == 2)
								{
									result = Singleton<CTextManager>.GetInstance().GetText("Arena_ARENAADDMEM_ERR_BATTLELISTISNULL");
								}
							}
							else
							{
								result = Singleton<CTextManager>.GetInstance().GetText("Arena_ARENAADDMEM_ERR_ALREADYIN");
							}
							return result;
						}
						goto IL_AC;
					case 2014:
						goto IL_448;
					}
					goto IL_90;
					IL_6CC:
					switch (ErrCode)
					{
					case 1:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_No_Privilege");
						break;
					case 2:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Invalid_Param");
						break;
					case 6:
						result = Singleton<CTextManager>.GetInstance().GetText("COM_MATCH_ERRCODE_OTHERS");
						break;
					case 7:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Team_Destory");
						break;
					case 8:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Game_Already_Start");
						break;
					case 9:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Team_Member_Full");
						break;
					case 10:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Game_Ready_Error");
						break;
					case 11:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Leave_Team_Failed");
						break;
					case 12:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Join_Litmi_Rank_Failed");
						break;
					case 14:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Version_Higher");
						break;
					case 15:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_Version_Lower");
						break;
					case 16:
						result = Singleton<CTextManager>.GetInstance().GetText("Err_ENTERTAINMENT_Lock");
						break;
					case 24:
						result = Singleton<CTextManager>.GetInstance().GetText("COM_MATCHTEAMEERR_OTHERS");
						break;
					case 26:
						result = Singleton<CTextManager>.GetInstance().GetText("COM_MATCHTEAMEERR_PLAT_CHANNEL_CLOSE");
						break;
					case 27:
						result = Singleton<CTextManager>.GetInstance().GetText("COM_MATCHTEAMEERR_OBING");
						break;
					}
					return result;
				}
				goto IL_74;
				IL_448:
				switch (ErrCode)
				{
				case 1:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Resource_Limit");
					break;
				case 2:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Game_Abort");
					break;
				case 3:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Room_Timeout");
					break;
				case 4:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Account_Leave");
					break;
				case 5:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Room_Not_Found");
					break;
				case 6:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Game_Already_Start");
					break;
				case 7:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Room_Member_Full");
					break;
				case 8:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Oper_Failed");
					break;
				case 9:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_No_Privilege");
					break;
				case 10:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Invalid_Param");
					break;
				case 15:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_NM_Cancel");
					break;
				case 16:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_NM_Teamcancel");
					break;
				case 17:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_NM_Teamexit");
					break;
				case 18:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_NM_Othercancel");
					break;
				case 19:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_NM_Otherexit");
					break;
				case 21:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Version_Different");
					break;
				case 22:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Version_Higher");
					break;
				case 23:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Version_Lower");
					break;
				case 24:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_ENTERTAINMENT_Lock");
					break;
				case 26:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Room_Inbantime_Lock");
					break;
				case 27:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Room_Limit_Lock");
					break;
				case 28:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Room_Ban_Pick_Limit");
					break;
				case 29:
					result = Singleton<CTextManager>.GetInstance().GetText("CS_ROOMERR_PLAT_CHANNEL_CLOSE");
					break;
				case 30:
					result = Singleton<CTextManager>.GetInstance().GetText("Err_Room_Ban_Pick_Hero_Limit");
					break;
				}
				return result;
			}
			goto IL_58;
		}
		goto IL_38;
	}

	public static string GetBubbleText(uint tag)
	{
		string result = "BubbleText with tag [" + tag + "] was not found!";
		ResTextData dataByKey = GameDataMgr.textBubbleDatabin.GetDataByKey(tag);
		if (dataByKey != null)
		{
			result = Utility.UTF8Convert(dataByKey.szContent);
		}
		return result;
	}

	public static DateTime ULongToDateTime(ulong dtVal)
	{
		ulong[] array = new ulong[6];
		for (int i = 0; i < Utility._DW.Length; i++)
		{
			array[i] = dtVal / Utility._DW[i];
			dtVal -= array[i] * Utility._DW[i];
		}
		array[Utility._DW.Length] = dtVal;
		return new DateTime((int)array[0], (int)array[1], (int)array[2], (int)array[3], (int)array[4], (int)array[5]);
	}

	public static DateTime SecondsToDateTime(int y, int m, int d, int secondsInDay)
	{
		int num = secondsInDay / 3600;
		secondsInDay %= 3600;
		return new DateTime(y, m, d, num, secondsInDay / 60, secondsInDay % 60);
	}

	public static DateTime StringToDateTime(string dtStr, DateTime defaultIfNone)
	{
		ulong dtVal;
		if (ulong.TryParse(dtStr, ref dtVal))
		{
			return Utility.ULongToDateTime(dtVal);
		}
		return defaultIfNone;
	}

	public static string DateTimeFormatString(DateTime dt, Utility.enDTFormate fm)
	{
		if (fm == Utility.enDTFormate.DATE)
		{
			return string.Format("{0:D4}-{1:D2}-{2:D2}", dt.get_Year(), dt.get_Month(), dt.get_Day());
		}
		if (fm == Utility.enDTFormate.TIME)
		{
			return string.Format("{0:D2}:{1:D2}:{2:D2}", dt.get_Hour(), dt.get_Minute(), dt.get_Second());
		}
		return string.Format("{0:D4}-{1:D2}-{2:D2}", dt.get_Year(), dt.get_Month(), dt.get_Day()) + " " + string.Format("{0:D2}:{1:D2}:{2:D2}", dt.get_Hour(), dt.get_Minute(), dt.get_Second());
	}

	public static string SecondsToTimeText(uint secs)
	{
		uint num = secs / 3600u;
		secs -= num * 3600u;
		uint num2 = secs / 60u;
		secs -= num2 * 60u;
		return string.Format("{0:D2}:{1:D2}:{2:D2}", num, num2, secs);
	}

	public static string GetTimeSpanFormatString(int timeSpanSeconds)
	{
		TimeSpan timeSpan = new TimeSpan((long)timeSpanSeconds * 10000000L);
		if (timeSpan.get_Days() > 0)
		{
			return timeSpan.get_Days().ToString() + Singleton<CTextManager>.GetInstance().GetText("Common_Day") + timeSpan.get_Hours().ToString() + Singleton<CTextManager>.GetInstance().GetText("Common_Hour");
		}
		string text = (timeSpan.get_Hours() >= 10) ? timeSpan.get_Hours().ToString() : ("0" + timeSpan.get_Hours());
		string text2 = (timeSpan.get_Minutes() >= 10) ? timeSpan.get_Minutes().ToString() : ("0" + timeSpan.get_Minutes());
		string text3 = (timeSpan.get_Seconds() >= 10) ? timeSpan.get_Seconds().ToString() : ("0" + timeSpan.get_Seconds());
		return string.Format("{0}:{1}:{2}", text, text2, text3);
	}

	public static bool IsOverOneDay(int timeSpanSeconds)
	{
		TimeSpan timeSpan = new TimeSpan((long)timeSpanSeconds * 10000000L);
		return timeSpan.get_Days() > 0;
	}

	public static bool IsBanChar(char key)
	{
		for (int i = 0; i < Utility.s_ban_chars.Length; i++)
		{
			if (Utility.s_ban_chars[i] == key)
			{
				return true;
			}
		}
		return false;
	}

	public static bool HasBanCharInString(string str)
	{
		if (!string.IsNullOrEmpty(str))
		{
			for (int i = 0; i < str.get_Length(); i++)
			{
				if (Utility.IsBanChar(str.get_Chars(i)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsChineseChar(char key)
	{
		int num = Convert.ToInt32(key);
		return num >= Utility.CHINESE_CHAR_START && num <= Utility.CHINESE_CHAR_END;
	}

	public static bool IsSpecialChar(char key)
	{
		return !Utility.IsChineseChar(key) && !char.IsLetter(key) && !char.IsNumber(key);
	}

	public static bool IsValidText(string text)
	{
		for (int i = 0; i < text.get_Length(); i++)
		{
			if (Utility.IsSpecialChar(text.get_Chars(i)))
			{
				return false;
			}
		}
		return true;
	}

	public static int GetByteCount(string inputStr)
	{
		int num = 0;
		for (int i = 0; i < inputStr.get_Length(); i++)
		{
			if (Utility.IsQuanjiaoChar(inputStr.Substring(i, 1)))
			{
				num += 2;
			}
			else
			{
				num++;
			}
		}
		return num;
	}

	public static bool IsQuanjiaoChar(string inputStr)
	{
		return Encoding.get_Default().GetByteCount(inputStr) > 1;
	}

	public static bool IsNullOrEmpty(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return true;
		}
		for (int i = 0; i < str.get_Length(); i++)
		{
			char c = str.get_Chars(i);
			if (c != ' ')
			{
				return false;
			}
		}
		return true;
	}

	public static Utility.NameResult CheckRoleName(string inputName)
	{
		int byteCount = Utility.GetByteCount(inputName);
		if (Utility.IsNullOrEmpty(inputName))
		{
			return Utility.NameResult.Null;
		}
		if (Utility.HasBanCharInString(inputName))
		{
			return Utility.NameResult.InVaildChar;
		}
		if (byteCount > Utility.MAX_EN_NAME_LEN || byteCount < Utility.MIN_EN_NAME_LEN)
		{
			return Utility.NameResult.OutOfLength;
		}
		return Utility.NameResult.Vaild;
	}

	public static Type GetType(string typeName)
	{
		if (string.IsNullOrEmpty(typeName))
		{
			return null;
		}
		Type type = Type.GetType(typeName);
		if (type != null)
		{
			return type;
		}
		Assembly[] assemblies = AppDomain.get_CurrentDomain().GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Assembly assembly = assemblies[i];
			type = assembly.GetType(typeName);
			if (type != null)
			{
				return type;
			}
		}
		return null;
	}

	public static byte[] ObjectToBytes(object obj)
	{
		byte[] buffer;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			buffer = memoryStream.GetBuffer();
		}
		return buffer;
	}

	public static object BytesToObject(byte[] Bytes)
	{
		object result;
		using (MemoryStream memoryStream = new MemoryStream(Bytes))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			result = binaryFormatter.Deserialize(memoryStream);
		}
		return result;
	}

	public static byte[] ReadFile(string path)
	{
		FileStream fileStream = null;
		if (!CFileManager.IsFileExist(path))
		{
			return null;
		}
		try
		{
			fileStream = new FileStream(path, 3, 1, 0);
			int num = (int)fileStream.get_Length();
			byte[] array = new byte[num];
			fileStream.Read(array, 0, num);
			fileStream.Close();
			fileStream.Dispose();
			return array;
		}
		catch (Exception var_3_4B)
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
		}
		return null;
	}

	public static bool WriteFile(string path, byte[] bytes)
	{
		FileStream fileStream = null;
		bool result;
		try
		{
			if (!CFileManager.IsFileExist(path))
			{
				fileStream = new FileStream(path, 2, 2, 3);
			}
			else
			{
				fileStream = new FileStream(path, 5, 2, 3);
			}
			fileStream.Write(bytes, 0, bytes.Length);
			fileStream.Flush();
			fileStream.Close();
			fileStream.Dispose();
			result = true;
		}
		catch (Exception var_1_4F)
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
			result = false;
		}
		return result;
	}

	public static string CreateMD5Hash(string input)
	{
		MD5 mD = MD5.Create();
		byte[] bytes = Encoding.get_ASCII().GetBytes(input);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static IApolloSnsService GetIApolloSnsService()
	{
		return IApollo.get_Instance().GetService(1) as IApolloSnsService;
	}

	public static float Divide(uint a, uint b)
	{
		if (b == 0u)
		{
			return 0f;
		}
		return a / b;
	}

	public static void VibrateHelper()
	{
		if (GameSettings.EnableVibrate)
		{
			Handheld.Vibrate();
		}
	}

	public static int GetCpuCurrentClock()
	{
		return Utility.GetCpuClock("/sys/devices/system/cpu/cpu0/cpufreq/scaling_cur_freq");
	}

	public static int GetCpuMaxClock()
	{
		return Utility.GetCpuClock("/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_max_freq");
	}

	public static int GetCpuMinClock()
	{
		return Utility.GetCpuClock("/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_min_freq");
	}

	private static int GetCpuClock(string cpuFile)
	{
		string fileContent = Utility.getFileContent(cpuFile);
		int num = 0;
		if (!int.TryParse(fileContent, ref num))
		{
			num = 0;
		}
		return Mathf.FloorToInt((float)(num / 1000));
	}

	private static string getFileContent(string path)
	{
		string result;
		try
		{
			string text = File.ReadAllText(path);
			result = text;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.get_Message());
			result = null;
		}
		return result;
	}

	public static bool IsSelf(ulong uid, int logicWorldId)
	{
		return uid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID && Utility.IsSameLogicWorldWithSelf(logicWorldId);
	}

	public static bool IsValidPlayer(ulong uid, int logicWorldId)
	{
		return uid > 0uL && logicWorldId > 0;
	}

	public static bool IsSameLogicWorldWithSelf(int logicWorldId)
	{
		return MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID == logicWorldId;
	}

	public static bool IsSamePlatformWithSelf(uint logicWorldId)
	{
		return Utility.IsSamePlatform((uint)MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID, logicWorldId);
	}

	public static bool IsSamePlatform(uint logicWorldId1, uint logicWorldId2)
	{
		return logicWorldId1 / 1000u == logicWorldId2 / 1000u;
	}

	public static bool IsCanShowPrompt()
	{
		return !Singleton<BattleLogic>.GetInstance().isRuning && !Singleton<WatchController>.GetInstance().IsWatching && !Singleton<SettlementSystem>.GetInstance().IsExistSettleForm();
	}

	public static T DeepCopyByReflection<T>(T obj)
	{
		Type type = obj.GetType();
		if (obj is string || type.get_IsValueType())
		{
			return obj;
		}
		if (type.get_IsArray())
		{
			Type type2 = Type.GetType(type.get_FullName().Replace("[]", string.Empty));
			Array array = obj as Array;
			Array array2 = Array.CreateInstance(type2, array.get_Length());
			for (int i = 0; i < array.get_Length(); i++)
			{
				array2.SetValue(Utility.DeepCopyByReflection<object>(array.GetValue(i)), i);
			}
			return (T)((object)Convert.ChangeType(array2, obj.GetType()));
		}
		object obj2 = Activator.CreateInstance(obj.GetType());
		PropertyInfo[] properties = obj.GetType().GetProperties(60);
		PropertyInfo[] array3 = properties;
		for (int j = 0; j < array3.Length; j++)
		{
			PropertyInfo propertyInfo = array3[j];
			object value = propertyInfo.GetValue(obj, null);
			if (value != null)
			{
				propertyInfo.SetValue(obj2, Utility.DeepCopyByReflection<object>(value), null);
			}
		}
		FieldInfo[] fields = obj.GetType().GetFields(60);
		FieldInfo[] array4 = fields;
		for (int k = 0; k < array4.Length; k++)
		{
			FieldInfo fieldInfo = array4[k];
			try
			{
				fieldInfo.SetValue(obj2, Utility.DeepCopyByReflection<object>(fieldInfo.GetValue(obj)));
			}
			catch
			{
			}
		}
		return (T)((object)obj2);
	}

	public static T DeepCopyBySerialization<T>(T obj)
	{
		object obj2;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			memoryStream.Seek(0L, 0);
			obj2 = binaryFormatter.Deserialize(memoryStream);
			memoryStream.Close();
		}
		return (T)((object)obj2);
	}
}
