using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CAchieveInfo
	{
		public ListView<CAchieveItem> m_achieveList = new ListView<CAchieveItem>();

		public ListView<CAchieveItem> m_AchiveSeriesList = new ListView<CAchieveItem>();

		public int[] m_achieveDoneArr = new int[60];

		public static CAchieveInfo GetAchieveInfo()
		{
			return null;
		}

		public void InitAchieveInfo(COMDT_ACHIEVEMENT_INFO svrAchieveInfo)
		{
			this.m_achieveList.Clear();
			int i;
			for (i = 0; i < 60; i++)
			{
				this.m_achieveDoneArr[i] = 0;
			}
			i = 0;
			while ((long)i < (long)((ulong)svrAchieveInfo.dwDoneTypeNum))
			{
				int dwDoneType = (int)svrAchieveInfo.astDoneData[i].dwDoneType;
				this.m_achieveDoneArr[dwDoneType] = svrAchieveInfo.astDoneData[i].iDoneCnt;
				i++;
			}
			i = 0;
			while ((long)i < (long)((ulong)svrAchieveInfo.dwAchievementNum))
			{
				CAchieveItem cAchieveItem = new CAchieveItem();
				cAchieveItem.InitStateData(svrAchieveInfo.astAchievementData[i]);
				cAchieveItem.SetDoneData(ref this.m_achieveDoneArr);
				this.m_achieveList.Add(cAchieveItem);
				i++;
			}
		}

		public ListView<CAchieveItem> GetNeedShowAchieveItemsByType(int achieveType)
		{
			ListView<CAchieveItem> listView = new ListView<CAchieveItem>();
			for (int i = 0; i < this.m_achieveList.get_Count(); i++)
			{
				if (this.m_achieveList.get_Item(i) != null && this.m_achieveList.get_Item(i).m_cfgInfo != null && (ulong)this.m_achieveList.get_Item(i).m_cfgInfo.dwType == (ulong)((long)achieveType) && this.m_achieveList.get_Item(i).IsNeedShow())
				{
					listView.Add(this.m_achieveList.get_Item(i));
				}
			}
			return listView;
		}

		public CAchieveItem GetAchieveItemById(uint achievementId)
		{
			for (int i = 0; i < this.m_achieveList.get_Count(); i++)
			{
				if (this.m_achieveList.get_Item(i).m_cfgId == achievementId)
				{
					return this.m_achieveList.get_Item(i);
				}
			}
			return null;
		}

		public int GetTotalAchievePoint(int achieveType)
		{
			int num = 0;
			for (int i = 0; i < this.m_achieveList.get_Count(); i++)
			{
				if (this.m_achieveList.get_Item(i) != null && this.m_achieveList.get_Item(i).m_cfgInfo != null && ((ulong)this.m_achieveList.get_Item(i).m_cfgInfo.dwType == (ulong)((long)achieveType) || achieveType == 0))
				{
					num += (int)this.m_achieveList.get_Item(i).m_cfgInfo.dwPoint;
				}
			}
			return num;
		}

		public int GetFinishAchievePoint(int achieveType)
		{
			int num = 0;
			for (int i = 0; i < this.m_achieveList.get_Count(); i++)
			{
				if (this.m_achieveList.get_Item(i) != null && this.m_achieveList.get_Item(i).IsFinish() && this.m_achieveList.get_Item(i).m_cfgInfo != null && ((ulong)this.m_achieveList.get_Item(i).m_cfgInfo.dwType == (ulong)((long)achieveType) || achieveType == 0))
				{
					num += (int)this.m_achieveList.get_Item(i).m_cfgInfo.dwPoint;
				}
			}
			return num;
		}

		public int GetGotRewardAchievePoint(int achieveType)
		{
			int num = 0;
			for (int i = 0; i < this.m_achieveList.get_Count(); i++)
			{
				if (this.m_achieveList.get_Item(i) != null && this.m_achieveList.get_Item(i).IsGotReward() && this.m_achieveList.get_Item(i).m_cfgInfo != null && ((ulong)this.m_achieveList.get_Item(i).m_cfgInfo.dwType == (ulong)((long)achieveType) || achieveType == 0))
				{
					num += (int)this.m_achieveList.get_Item(i).m_cfgInfo.dwPoint;
				}
			}
			return num;
		}

		public bool IsHaveFinishButNotGetRewardAchievement(int achieveType)
		{
			for (int i = 0; i < this.m_achieveList.get_Count(); i++)
			{
				if (this.m_achieveList.get_Item(i) != null && this.m_achieveList.get_Item(i).m_cfgInfo != null && ((ulong)this.m_achieveList.get_Item(i).m_cfgInfo.dwType == (ulong)((long)achieveType) || achieveType == 0) && this.m_achieveList.get_Item(i).GetAchieveState() == 1)
				{
					return true;
				}
			}
			return false;
		}

		public void OnAchieveStateChange(COMDT_ACHIEVEMENT_DATA achieveData)
		{
			int count = this.m_achieveList.get_Count();
			for (int i = 0; i < count; i++)
			{
				if (this.m_achieveList.get_Item(i).m_cfgId == achieveData.dwID)
				{
					this.m_achieveList.get_Item(i).SetAchieveState(achieveData.bState);
					return;
				}
			}
		}

		public void OnAchieveDoneDataChange(COMDT_ACHIEVEMENT_DONE_DATA donwData)
		{
			int count = this.m_achieveList.get_Count();
			for (int i = 0; i < count; i++)
			{
				if (this.m_achieveList.get_Item(i).m_doneType == donwData.dwDoneType)
				{
					this.m_achieveList.get_Item(i).SetAchieveDoneCnt(donwData.iDoneCnt);
				}
			}
		}
	}
}
