using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	public abstract class MultiGameInfo : GameInfoBase
	{
		public override void PreBeginPlay()
		{
			base.PreBeginPlay();
			this.PreparePlayer();
			this.ResetSynchrConfig();
			this.LoadAllTeamActors();
		}

		protected virtual void PreparePlayer()
		{
			MultiGameContext multiGameContext = this.GameContext as MultiGameContext;
			DebugHelper.Assert(multiGameContext != null);
			if (multiGameContext == null)
			{
				return;
			}
			if (Singleton<GamePlayerCenter>.get_instance().GetAllPlayers().get_Count() > 0)
			{
			}
			Singleton<GamePlayerCenter>.get_instance().ClearAllPlayers();
			uint num = 0u;
			for (int i = 0; i < 2; i++)
			{
				int num2 = 0;
				while ((long)num2 < (long)((ulong)multiGameContext.MessageRef.astCampInfo[i].dwPlayerNum))
				{
					uint dwObjId = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.dwObjId;
					Player player = Singleton<GamePlayerCenter>.GetInstance().GetPlayer(dwObjId);
					DebugHelper.Assert(player == null, "你tm肯定在逗我");
					if (num == 0u && i + 1 == 1)
					{
						num = dwObjId;
					}
					bool flag = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.bObjType == 2;
					if (player != null)
					{
						goto IL_727;
					}
					string openId = string.Empty;
					uint vipLv = 0u;
					int honorId = 0;
					int honorLevel = 0;
					ulong uid;
					uint logicWrold;
					uint level;
					if (flag)
					{
						if (Convert.ToBoolean(multiGameContext.MessageRef.stDeskInfo.bIsWarmBattle))
						{
							uid = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.stDetail.get_stPlayerOfNpc().ullFakeUid;
							logicWrold = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.stDetail.get_stPlayerOfNpc().dwFakeLogicWorldID;
							level = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.stDetail.get_stPlayerOfNpc().dwFakePvpLevel;
							openId = string.Empty;
						}
						else
						{
							uid = 0uL;
							logicWrold = 0u;
							level = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.dwLevel;
							openId = string.Empty;
						}
					}
					else
					{
						uid = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.stDetail.get_stPlayerOfAcnt().ullUid;
						logicWrold = (uint)multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.stDetail.get_stPlayerOfAcnt().iLogicWorldID;
						level = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.dwLevel;
						openId = Utility.UTF8Convert(multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].szOpenID);
						vipLv = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.stDetail.get_stPlayerOfAcnt().stGameVip.dwCurLevel;
						honorId = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.stDetail.get_stPlayerOfAcnt().iHonorID;
						honorLevel = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.stDetail.get_stPlayerOfAcnt().iHonorLevel;
					}
					GameIntimacyData intimacyData = null;
					if (!flag)
					{
						CSDT_CAMPPLAYERINFO cSDT_CAMPPLAYERINFO = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2];
						if (cSDT_CAMPPLAYERINFO == null)
						{
							goto IL_81D;
						}
						ulong ullUid = cSDT_CAMPPLAYERINFO.stPlayerInfo.stDetail.get_stPlayerOfAcnt().ullUid;
						int iLogicWorldID = cSDT_CAMPPLAYERINFO.stPlayerInfo.stDetail.get_stPlayerOfAcnt().iLogicWorldID;
						byte bIntimacyRelationPrior = cSDT_CAMPPLAYERINFO.stIntimacyRelation.bIntimacyRelationPrior;
						COMDT_INTIMACY_RELATION_DATA stIntimacyRelationData = cSDT_CAMPPLAYERINFO.stIntimacyRelation.stIntimacyRelationData;
						ulong num4;
						ulong num3 = num4 = 0uL;
						uint num6;
						uint num5 = num6 = 0u;
						string text = string.Empty;
						string text2 = string.Empty;
						for (int j = 0; j < (int)stIntimacyRelationData.wIntimacyRelationNum; j++)
						{
							COMDT_INTIMACY_RELATION cOMDT_INTIMACY_RELATION = stIntimacyRelationData.astIntimacyRelationList[j];
							if (cOMDT_INTIMACY_RELATION != null)
							{
								for (int k = 0; k < 2; k++)
								{
									int num7 = 0;
									while ((long)num7 < (long)((ulong)multiGameContext.MessageRef.astCampInfo[k].dwPlayerNum))
									{
										CSDT_CAMPPLAYERINFO cSDT_CAMPPLAYERINFO2 = multiGameContext.MessageRef.astCampInfo[k].astCampPlayerInfo[num7];
										if (multiGameContext.MessageRef.astCampInfo[k].astCampPlayerInfo[num7].stPlayerInfo.bObjType != 2)
										{
											ulong ullUid2 = cSDT_CAMPPLAYERINFO2.stPlayerInfo.stDetail.get_stPlayerOfAcnt().ullUid;
											int iLogicWorldID2 = cSDT_CAMPPLAYERINFO2.stPlayerInfo.stDetail.get_stPlayerOfAcnt().iLogicWorldID;
											if (cSDT_CAMPPLAYERINFO2 != null && (ullUid2 != ullUid || iLogicWorldID2 != iLogicWorldID))
											{
												if (cOMDT_INTIMACY_RELATION.ullUid == ullUid2 && (ulong)cOMDT_INTIMACY_RELATION.dwLogicWorldId == (ulong)((long)iLogicWorldID2))
												{
													string text3 = StringHelper.UTF8BytesToString(ref cSDT_CAMPPLAYERINFO2.stPlayerInfo.szName);
													if (cOMDT_INTIMACY_RELATION.bIntimacyState == bIntimacyRelationPrior)
													{
														num4 = ullUid2;
														num6 = (uint)iLogicWorldID2;
														text = text3;
													}
													else
													{
														num3 = ullUid2;
														num5 = (uint)iLogicWorldID2;
														text2 = text3;
													}
												}
											}
										}
										num7++;
									}
								}
							}
						}
						string text4 = StringHelper.UTF8BytesToString(ref cSDT_CAMPPLAYERINFO.stPlayerInfo.szName);
						if (num4 != 0uL && num6 != 0u)
						{
							string title = string.Empty;
							if (bIntimacyRelationPrior == 1)
							{
								title = Singleton<CTextManager>.get_instance().GetText("IntimacyShowLoadGay", new string[]
								{
									text
								});
							}
							if (bIntimacyRelationPrior == 2)
							{
								title = Singleton<CTextManager>.get_instance().GetText("IntimacyShowLoadLover", new string[]
								{
									text
								});
							}
							intimacyData = new GameIntimacyData(bIntimacyRelationPrior, num4, num6, title);
							string text5 = string.Format("----FR, 局内亲密度, 玩家:{0}, 优先选择的关系:{1} --- finded 关系:{2}, 对方名字:{3}", new object[]
							{
								text4,
								bIntimacyRelationPrior,
								bIntimacyRelationPrior,
								text
							});
						}
						else if (num3 != 0uL && num5 != 0u)
						{
							byte b = 0;
							if (bIntimacyRelationPrior == 1)
							{
								b = 2;
							}
							if (bIntimacyRelationPrior == 2)
							{
								b = 1;
							}
							string title2 = string.Empty;
							if (b == 1)
							{
								title2 = Singleton<CTextManager>.get_instance().GetText("IntimacyShowLoadGay", new string[]
								{
									text2
								});
							}
							if (b == 2)
							{
								title2 = Singleton<CTextManager>.get_instance().GetText("IntimacyShowLoadLover", new string[]
								{
									text2
								});
							}
							intimacyData = new GameIntimacyData(b, num3, num5, title2);
							string text6 = string.Format("----FR, 局内亲密度, 玩家:{0}, 优先选择的关系:{1} --- finded 关系:{2}, 对方名字:{3}", new object[]
							{
								text4,
								bIntimacyRelationPrior,
								b,
								text2
							});
						}
					}
					player = Singleton<GamePlayerCenter>.GetInstance().AddPlayer(dwObjId, i + 1, (int)multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.bPosOfCamp, level, flag, Utility.UTF8Convert(multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.szName), 0, (int)logicWrold, uid, vipLv, openId, multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].dwGradeOfRank, multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].dwClassOfRank, honorId, honorLevel, intimacyData);
					DebugHelper.Assert(player != null, "创建player失败");
					if (player != null)
					{
						player.isGM = (multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].bIsGM > 0);
						goto IL_727;
					}
					goto IL_727;
					IL_81D:
					num2++;
					continue;
					IL_727:
					for (int l = 0; l < multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.astChoiceHero.Length; l++)
					{
						COMDT_CHOICEHERO cOMDT_CHOICEHERO = multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.astChoiceHero[l];
						int dwHeroID = (int)cOMDT_CHOICEHERO.stBaseInfo.stCommonInfo.dwHeroID;
						if (dwHeroID != 0)
						{
							bool flag2 = (cOMDT_CHOICEHERO.stBaseInfo.stCommonInfo.dwMaskBits & 4u) > 0u && (cOMDT_CHOICEHERO.stBaseInfo.stCommonInfo.dwMaskBits & 1u) == 0u;
							if (multiGameContext.MessageRef.astCampInfo[i].astCampPlayerInfo[num2].stPlayerInfo.bObjType == 1)
							{
								if (flag2)
								{
								}
							}
							if (player != null)
							{
								player.AddHero((uint)dwHeroID);
							}
						}
					}
					goto IL_81D;
				}
			}
			if (Singleton<WatchController>.GetInstance().IsWatching)
			{
				Singleton<GamePlayerCenter>.GetInstance().SetHostPlayer(num);
			}
			else if (Singleton<GameReplayModule>.HasInstance() && Singleton<GameReplayModule>.get_instance().isReplay)
			{
				Singleton<GamePlayerCenter>.GetInstance().SetHostPlayer(num);
			}
			else
			{
				Player playerByUid = Singleton<GamePlayerCenter>.GetInstance().GetPlayerByUid(Singleton<CRoleInfoManager>.get_instance().masterUUID);
				DebugHelper.Assert(playerByUid != null, "load multi game but hostPlayer is null");
				Singleton<GamePlayerCenter>.GetInstance().SetHostPlayer(playerByUid.PlayerId);
			}
			multiGameContext.levelContext.m_isWarmBattle = Convert.ToBoolean(multiGameContext.MessageRef.stDeskInfo.bIsWarmBattle);
			multiGameContext.SaveServerData();
		}

		protected virtual void ResetSynchrConfig()
		{
			MultiGameContext multiGameContext = this.GameContext as MultiGameContext;
			DebugHelper.Assert(multiGameContext != null);
			Singleton<FrameSynchr>.GetInstance().SetSynchrConfig(multiGameContext.MessageRef.dwKFrapsFreqMs, (uint)multiGameContext.MessageRef.bKFrapsLater, (uint)multiGameContext.MessageRef.bPreActFrap, multiGameContext.MessageRef.dwRandomSeed);
		}

		public override void OnLoadingProgress(float Progress)
		{
			if (!Singleton<WatchController>.GetInstance().IsWatching)
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1083u);
				cSPkg.stPkgData.get_stMultGameLoadProcessReq().wProcess = Convert.ToUInt16(Progress * 100f);
				Singleton<NetworkModule>.GetInstance().SendGameMsg(ref cSPkg, 0u);
			}
			CUILoadingSystem.OnSelfLoadProcess(Progress);
		}

		public override void StartFight()
		{
			base.StartFight();
		}

		public override void EndGame()
		{
			base.EndGame();
		}
	}
}
