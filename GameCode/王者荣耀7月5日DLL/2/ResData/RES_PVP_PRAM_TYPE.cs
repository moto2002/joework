using System;

namespace ResData
{
	public enum RES_PVP_PRAM_TYPE
	{
		RES_PVP_PARAM_TYPE_NULL,
		RES_NORMALMATCH_MEMBER2_ADJUST_MMR,
		RES_NORMALMATCH_MEMBER3_ADJUST_MMR,
		RES_NORMALMATCH_MEMBER4_ADJUST_MMR,
		RES_NORMALMATCH_MEMBER5_ADJUST_MMR,
		RES_NORMALMATCH_INIT_MMR,
		RES_RANKMATCH_INIT_MMR,
		RES_PVP_PARAM_TYPE_LADDER_K_MIN = 9,
		RES_PVP_PARAM_TYPE_LADDER_K_MID,
		RES_PVP_PARAM_TYPE_LADDER_K_MAX,
		RES_PVP_PARAM_TYPE_LADDER_K_MAX_GAMENUM,
		RES_PVP_PARAM_TYPE_LADDER_K_1PT_GAMENUM,
		RES_PVP_PARAM_TYPE_LADDER_O_MAX,
		RES_PVP_PARAM_TYPE_LADDER_O_MAX_GRADE,
		RES_PVP_PARAM_TYPE_LADDER_O_MID,
		RES_PVP_PARAM_TYPE_LADDER_O_MID_GRADE,
		RES_PVP_PARAM_TYPE_LADDER_O_MIN,
		RES_PVP_PARAM_TYPE_LADDER_O_MIN_GRADE,
		RES_PVP_PARAM_TYPE_PVP_MATCH_1V1_K,
		RES_PVP_PARAM_TYPE_PVP_MATCH_2V2_K,
		RES_PVP_PARAM_TYPE_PVP_MATCH_3V3_K,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_WIN_11,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_WIN_22,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_WIN_33,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_FAIL_11,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_FAIL_22,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_FAIL_33,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_WIN_11,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_WIN_22,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_WIN_33,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_FAIL_11,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_FAIL_22,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_FAIL_33,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI11_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI22_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI33_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI11_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI22_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI33_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_WIN_11,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_WIN_22,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_WIN_33,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_FAIL_11,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_FAIL_22,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_FAIL_33,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_WIN_11,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_WIN_22,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_WIN_33,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_FAIL_11,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_FAIL_22,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_FAIL_33,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI11_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI22_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI33_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI11_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI22_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI33_MAX_MIN,
		RES_PVP_PARAM_TYPE_BTPERFORM_LOW,
		RES_PVP_PARAM_TYPE_BTPERFORM_1V1_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_2V2_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_3V3_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_PARAM_1V1,
		RES_PVP_PARAM_TYPE_BTPERFORM_PARAM_2v2,
		RES_PVP_PARAM_TYPE_BTPERFORM_PARAM_3v3,
		RES_PVP_PARAM_TYPE_PROFICIENCY_LOW,
		RES_PVP_PARAM_TYPE_PROFICIENCY_1V1_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_2V2_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_3V3_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_1V1_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PROFICIENCY_2V2_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PROFICIENCY_3V3_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PVPEXP_BASE_1V1,
		RES_PVP_PARAM_TYPE_PVPEXP_BASE_2V2,
		RES_PVP_PARAM_TYPE_PVPEXP_BASE_3V3,
		RES_PVP_PARAM_TYPE_PVPEXP_WIN_1V1,
		RES_PVP_PARAM_TYPE_PVPEXP_WIN_2V2,
		RES_PVP_PARAM_TYPE_PVPEXP_WIN_3V3,
		RES_PVP_PARAM_TYPE_MMR_NORMAL_MIN,
		RES_PVP_PARAM_TYPE_MMR_NORMAL_MAX,
		RES_PVP_PARAM_TYPE_MMR_LADDER_MIN,
		RES_PVP_PARAM_TYPE_MMR_LADDER_MAX,
		RES_ANTICHEAT_PARAM_SKILL_DISTANCE,
		RES_ANTICHEAT_PARAM_SKILL_CD,
		RES_ANTICHEAT_PARAM_MOVE_SPEED,
		RES_ANTICHEAT_PARAM_EXTREMEPOWER_TIME_MIN,
		RES_ANTICHEAT_PARAM_EXTREMEPOWER_TIME_MAX,
		RES_ANTICHEAT_PARAM_EXTREMEPOWER_PARAM_MIN,
		RES_ANTICHEAT_PARAM_EXTREMEPOWER_PARAM_MID,
		RES_ANTICHEAT_PARAM_EXTREMEPOWER_PARAM_MAX,
		RES_DEFAULTMATCH_INIT_RADIUS,
		RES_DEFAULTMATCH_CONDCHG_FREQ,
		RES_DEFAULTMATCH_CONDCHG_VAL,
		RES_DEFAULTMATCH_CONDCHG_MAXVAL,
		RES_RANKMATCH_INIT_RADIUS,
		RES_RANKMATCH_CONDCHG_FREQ,
		RES_RANKMATCH_CONDCHG_VAL,
		RES_RANKMATCH_CONDCHG_MAXVAL,
		RES_RANK_CHGMMR_ONETIME,
		RES_RANK_ADDITIONAL_NEEDLV,
		RES_RANK_ADDITIONAL_NEEDWINCNT,
		RES_ANTICHEAT_PARAM_AFFECT_GAMERESULT,
		RES_MAX_SCORE_OF_RANK,
		RES_ANTICHEAT_PARAM_MAX_SOUL_EXP,
		RES_RANK_TOPGRADE_MMR,
		RES_ANTICHEAT_PARAM_MAX_SOLDIER_CNT,
		RES_ANTICHEAT_PARAM_MAX_TOWER_ATTACK_DISTANCE,
		RES_ANTICHEAT_PARAM_EXTREMEPOWER_ONCE_PARAM,
		RES_ANTICHEAT_PARAM_POWER_TTH,
		RES_PVP_PARAM_TYPE_PVP_MATCH_4V4_K,
		RES_PVP_PARAM_TYPE_PVP_MATCH_5V5_K,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_WIN_44,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_WIN_55,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_FAIL_44,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI_FAIL_55,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_WIN_44,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_WIN_55,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_FAIL_44,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI_FAIL_55,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI44_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHOUTAI55_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI44_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_WITHAI55_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_WIN_44,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_WIN_55,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_FAIL_44,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI_FAIL_55,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_WIN_44,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_WIN_55,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_FAIL_44,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI_FAIL_55,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI44_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHOUTAI55_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI44_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_WITHAI55_MAX_MIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_4V4_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_5V5_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_4V4_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PROFICIENCY_5V5_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_BTPERFORM_4V4_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_5V5_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_PARAM_4V4,
		RES_PVP_PARAM_TYPE_BTPERFORM_PARAM_5v5,
		RES_PVP_PARAM_TYPE_WARMBATTLE_LOWLEVEL,
		RES_PVP_PARAM_TYPE_WARMBATTLE_HIGHLEVEL,
		RES_PVP_PARAM_TYPE_1V1_WARMBATTLE_CNT,
		RES_PVP_PARAM_TYPE_3V3_WARMBATTLE_CNT,
		RES_PVP_PARAM_TYPE_5V5_WARMBATTLE_CNT,
		RES_PVP_PARAM_TYPE_WARMBATTLE_PERDAYCNT_OFLOWLEVEL = 150,
		RES_PVP_PARAM_TYPE_WARMBATTLE_SINGLECNT_OFLOWLEVEL,
		RES_PVP_PARAM_TYPE_WARMBATTLE_PERDAYCNT_OFHIGHLEVEL,
		RES_PVP_PARAM_TYPE_WARMBATTLE_SINGLECNT_OFHIGHLEVEL,
		RES_PVP_PARAM_TYPE_WARMBATTLE_KILLNUM_OFLOWLEVEL,
		RES_PVP_PARAM_TYPE_WARMBATTLE_DEADNUM_OFLOWLEVEL,
		RES_PVP_PARAM_TYPE_WARMBATTLE_CONLOSECNT_OFLOWLEVEL,
		RES_PVP_PARAM_TYPE_WARMBATTLE_CONLOSECNT_OFHIGHLEVEL,
		RES_PVP_PARAM_TYPE_WARMBATTLE_TEAMUPDATE_CNT,
		RES_PVP_PARAM_TYPE_SINGLEWARMBATTLE_MIN_MATCHTIME,
		RES_PVP_PARAM_TYPE_SINGLEWARMBATTLE_MAX_MATCHTIME,
		RES_PVP_PARAM_TYPE_RANK_PROTECT_GRADE,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_1V1_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_2V2_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_3V3_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_4V4_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_5V5_PARAM_WIN,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_1V1_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_2V2_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_3V3_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_4V4_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PROFICIENCY_ENTERTAINMENT_5V5_PARAM_FAIL,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_11,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_22,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_33,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_44,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_55,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_11,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_22,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_33,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_44,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_55,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI11_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI22_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI33_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI44_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHOUTAI55_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_WIN_11,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_WIN_22,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_WIN_33,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_WIN_44,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_WIN_55,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_FAIL_11,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_FAIL_22,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_FAIL_33,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_FAIL_44,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI_FAIL_55,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI11_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI22_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI33_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI44_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPCOIN_PARAM_ENTERTAINMENT_WITHAI55_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_11,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_22,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_33,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_44,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_WIN_55,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_11,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_22,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_33,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_44,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI_FAIL_55,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI11_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI22_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI33_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI44_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHOUTAI55_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_WIN_11,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_WIN_22,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_WIN_33,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_WIN_44,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_WIN_55,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_FAIL_11,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_FAIL_22,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_FAIL_33,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_FAIL_44,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI_FAIL_55,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI11_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI22_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI33_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI44_MAX_MIN,
		RES_PVP_PARAM_TYPE_PVPEXP_PARAM_ENTERTAINMENT_WITHAI55_MAX_MIN,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_1V1_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_2V2_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_3V3_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_4V4_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_5V5_HIGH,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_PARAM_1V1,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_PARAM_2v2,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_PARAM_3v3,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_PARAM_4v4,
		RES_PVP_PARAM_TYPE_BTPERFORM_ENTERTAINMENT_PARAM_5v5,
		RES_PVP_PARAM_TYPE_CHGCREDIT_MAX_VALUE,
		RES_PVP_PARAM_TYPE_NORMAL_MMR_GRADE_INC_PARAM = 248,
		RES_PVP_PARAM_TYPE_RANK_MAX_GRADEDIFF_VALUE,
		RES_PVP_PARAM_TYPE_MAX_NORMALMMR_OF_ACNT,
		RES_PVP_PARAM_TYPE_SETTLE_CREDIT_WEEK_REWARD_DAY_CNT,
		RES_PVP_PARAM_TYPE_NEW_MMR_PARAM1,
		RES_PVP_PARAM_TYPE_NEW_MMR_PARAM2,
		RES_PVP_PARAM_TYPE_NEW_MMR_PARAM3,
		RES_PVP_PARAM_TYPE_NEW_MMR_1V1_MIN,
		RES_PVP_PARAM_TYPE_NEW_MMR_1V1_MAX,
		RES_PVP_PARAM_TYPE_NEW_MMR_3V3_MIN,
		RES_PVP_PARAM_TYPE_NEW_MMR_3V3_MAX,
		RES_PVP_PARAM_TYPE_NEW_MMR_5V5_MIN,
		RES_PVP_PARAM_TYPE_NEW_MMR_5V5_MAX,
		RES_PVP_PARAM_TYPE_NEW_MMR_CHAOS_MIN,
		RES_PVP_PARAM_TYPE_NEW_MMR_CHAOS_MAX,
		RES_PVP_PARAM_TYPE_NEW_MMR_ENTERTAINMENT_MIN,
		RES_PVP_PARAM_TYPE_NEW_MMR_ENTERTAINMENT_MAX,
		RES_PVP_PARAM_TYPE_NEW_MMR_TYPE_RATIO_TTH,
		RES_PVP_PARAM_TYPE_NEW_MMR_MAX_RATIO_TTH,
		RES_NORMALMATCH1V1_INIT_RADIUS = 269,
		RES_NORMALMATCH1V1_CONDCHG_VAL,
		RES_NORMALMATCH1V1_CONDCHG_MAXVAL,
		RES_NORMALMATCH1V1_CONDCHG_FACTOR,
		RES_NORMALMATCH3V3_INIT_RADIUS,
		RES_NORMALMATCH3V3_CONDCHG_VAL,
		RES_NORMALMATCH3V3_CONDCHG_MAXVAL,
		RES_NORMALMATCH3V3_CONDCHG_FACTOR,
		RES_NORMALMATCH5V5_INIT_RADIUS,
		RES_NORMALMATCH5V5_CONDCHG_VAL,
		RES_NORMALMATCH5V5_CONDCHG_MAXVAL,
		RES_NORMALMATCH5V5_CONDCHG_FACTOR,
		RES_CHAOSMATCH_INIT_RADIUS,
		RES_CHAOSMATCH_CONDCHG_VAL,
		RES_CHAOSMATCH_CONDCHG_MAXVAL,
		RES_CHAOSMATCH_CONDCHG_FACTOR,
		RES_ENTERTAINMENTMATCH_INIT_RADIUS,
		RES_ENTERTAINMENTMATCH_CONDCHG_VAL,
		RES_ENTERTAINMENTMATCH_CONDCHG_MAXVAL,
		RES_ENTERTAINMENTMATCH_CONDCHG_FACTOR,
		RES_RANKMATCH_CONDCHG_FACTOR,
		RES_DEFAULTMATCH_CONDCHG_FACTOR,
		RES_CHGROOMPOS_TIMEOUTSEC,
		RES_WARMRANKMATCH_INIT_RADIUS,
		RES_GUILDMATCH_INIT_RADIUS,
		RES_GUILDMATCH_CONDCHG_VAL,
		RES_GUILDMATCH_CONDCHG_MAXVAL,
		RES_GUILDMATCH_CONDCHG_FACTOR,
		RES_PVP_OBTIP_MASK,
		RES_PVP_PARAM_TYPE_MAX
	}
}
