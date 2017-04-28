using ResData;
using System;
using System.Runtime.CompilerServices;

namespace Assets.Scripts.GameLogic
{
	internal class StarEvaluation : IStarEvaluation
	{
		private static StarSystemFactory ConditionFactory = new StarSystemFactory(typeof(StarConditionAttribute), typeof(IStarCondition));

		public ListView<IStarCondition> Conditions = new ListView<IStarCondition>(3);

		public ResEvaluateStarInfo StarInfo;

		public int Index;

		private string Description;

		public event OnEvaluationChangedDelegate OnChanged
		{
			[MethodImpl(32)]
			add
			{
				this.OnChanged = (OnEvaluationChangedDelegate)Delegate.Combine(this.OnChanged, value);
			}
			[MethodImpl(32)]
			remove
			{
				this.OnChanged = (OnEvaluationChangedDelegate)Delegate.Remove(this.OnChanged, value);
			}
		}

		public virtual string description
		{
			get
			{
				if (this.StarInfo.bHideDetail > 0)
				{
					return this.Description;
				}
				return this.Description + ((this.Conditions.get_Count() <= 0 || this.Conditions.get_Item(0) == null) ? string.Empty : this.Conditions.get_Item(0).description);
			}
		}

		public string rawDescription
		{
			get
			{
				return this.Description;
			}
		}

		public ResEvaluateStarInfo configInfo
		{
			get
			{
				return this.StarInfo;
			}
		}

		public int index
		{
			get
			{
				return this.Index;
			}
		}

		public RES_LOGIC_OPERATION_TYPE logicType
		{
			get
			{
				return this.StarInfo.bLogicType;
			}
		}

		public StarEvaluationStatus status
		{
			get
			{
				if (this.logicType == null)
				{
					bool flag = true;
					for (int i = 0; i < this.Conditions.get_Count(); i++)
					{
						DebugHelper.Assert(this.Conditions.get_Item(i) != null);
						if (this.Conditions.get_Item(i).status == StarEvaluationStatus.Failure)
						{
							return StarEvaluationStatus.Failure;
						}
						if (this.Conditions.get_Item(i).status == StarEvaluationStatus.InProgressing)
						{
							flag = false;
						}
					}
					return (!flag) ? StarEvaluationStatus.InProgressing : StarEvaluationStatus.Success;
				}
				if (this.logicType == 1)
				{
					for (int j = 0; j < this.Conditions.get_Count(); j++)
					{
						DebugHelper.Assert(this.Conditions.get_Item(j) != null);
						if (this.Conditions.get_Item(j).status == StarEvaluationStatus.Success)
						{
							return StarEvaluationStatus.Success;
						}
					}
					return StarEvaluationStatus.Failure;
				}
				DebugHelper.Assert(false, "未识别的逻辑关系");
				return StarEvaluationStatus.Failure;
			}
		}

		public bool isSuccess
		{
			get
			{
				return this.status == StarEvaluationStatus.Success;
			}
		}

		public bool isFailure
		{
			get
			{
				return this.status == StarEvaluationStatus.Failure;
			}
		}

		public bool isInProgressing
		{
			get
			{
				return this.status == StarEvaluationStatus.InProgressing;
			}
		}

		protected void AddCondition(ResDT_ConditionInfo InCondConfig)
		{
			StarCondition starCondition = StarEvaluation.ConditionFactory.Create((int)InCondConfig.dwType) as StarCondition;
			DebugHelper.Assert(starCondition != null);
			if (starCondition != null)
			{
				starCondition.OnStarConditionChanged += new OnStarConditionChangedDelegate(this.OnConditionChanged);
				starCondition.Initialize(InCondConfig);
				this.Conditions.Add(starCondition);
			}
		}

		protected void OnConditionChanged(IStarCondition InCondition)
		{
			DebugHelper.Assert(InCondition != null);
			if (this.OnChanged != null)
			{
				this.OnChanged(this, InCondition);
			}
		}

		public void Initialize(ResEvaluateStarInfo InStarInfo)
		{
			this.StarInfo = InStarInfo;
			this.Description = Utility.UTF8Convert(InStarInfo.szCondDesc);
			for (int i = 0; i < InStarInfo.astConditions.Length; i++)
			{
				ResDT_ConditionInfo resDT_ConditionInfo = InStarInfo.astConditions[i];
				if (resDT_ConditionInfo.dwType == 0u)
				{
					break;
				}
				this.AddCondition(resDT_ConditionInfo);
			}
		}

		public IStarCondition GetConditionAt(int Index)
		{
			IStarCondition arg_2C_0;
			if (Index >= 0 && Index < this.Conditions.get_Count())
			{
				IStarCondition starCondition = this.Conditions.get_Item(Index);
				arg_2C_0 = starCondition;
			}
			else
			{
				arg_2C_0 = null;
			}
			return arg_2C_0;
		}

		public virtual void Start()
		{
			for (int i = 0; i < this.Conditions.get_Count(); i++)
			{
				this.Conditions.get_Item(i).Start();
			}
		}

		public virtual void OnActorDeath(ref GameDeadEventParam prm)
		{
			int num = this.Conditions.get_Count() - 1;
			while (num >= 0 && num < this.Conditions.get_Count())
			{
				IStarCondition starCondition = this.Conditions.get_Item(num);
				if (starCondition != null)
				{
					starCondition.OnActorDeath(ref prm);
				}
				num--;
			}
		}

		public virtual void OnCampScoreUpdated(ref SCampScoreUpdateParam prm)
		{
			int num = this.Conditions.get_Count() - 1;
			while (num >= 0 && num < this.Conditions.get_Count())
			{
				IStarCondition starCondition = this.Conditions.get_Item(num);
				if (starCondition != null)
				{
					starCondition.OnCampScoreUpdated(ref prm);
				}
				num--;
			}
		}

		public void Dispose()
		{
			for (int i = 0; i < this.Conditions.get_Count(); i++)
			{
				if (this.Conditions.get_Item(i) != null)
				{
					this.Conditions.get_Item(i).Dispose();
				}
			}
			this.Conditions.Clear();
		}

		public ListView<IStarCondition>.Enumerator GetEnumerator()
		{
			return this.Conditions.GetEnumerator();
		}
	}
}
