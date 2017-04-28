using ResData;
using System;

namespace Assets.Scripts.Framework
{
	public class GameTaskGroup : GameTask
	{
		private struct TaskNode
		{
			public GameTask task;

			public bool achieve;
		}

		private ListView<GameTaskGroup.TaskNode> _taskList;

		public RES_GAME_TASK_GROUP_START_TYPE StartType
		{
			get
			{
				return base.Config.iParam1;
			}
		}

		public RES_GAME_TASK_GROUP_CLOSE_TYPE CloseType
		{
			get
			{
				return base.Config.iParam2;
			}
		}

		public override int Target
		{
			get
			{
				return (this.CloseType != 1) ? this._taskList.get_Count() : 1;
			}
		}

		public override bool IsGroup
		{
			get
			{
				return true;
			}
		}

		public GameTask ActiveChild
		{
			get
			{
				for (int i = 0; i < this._taskList.get_Count(); i++)
				{
					GameTask task = this._taskList.get_Item(i).task;
					if (task.Active)
					{
						return task;
					}
				}
				return null;
			}
		}

		protected override void OnInitial()
		{
			ListView<ResGameTaskGroup> groupData = base.RootSys.GetGroupData(base.ID);
			DebugHelper.Assert(null != groupData, "GameTaskGroup.groupData must not be null!");
			this._taskList = new ListView<GameTaskGroup.TaskNode>();
			if (groupData != null)
			{
				for (int i = 0; i < groupData.get_Count(); i++)
				{
					ResGameTaskGroup resGameTaskGroup = groupData.get_Item(i);
					GameTaskGroup.TaskNode taskNode = default(GameTaskGroup.TaskNode);
					taskNode.task = base.RootSys.GetTask(resGameTaskGroup.dwChildTask, true);
					taskNode.achieve = (resGameTaskGroup.bIsAchieve == 0);
					this._taskList.Add(taskNode);
					if (taskNode.task != null)
					{
						taskNode.task._AddOwnerGroup(this);
					}
				}
			}
			DebugHelper.Assert(this._taskList.get_Count() > 0, "GameTaskGroup.taskList.Count must > 0!");
		}

		protected override void OnStart()
		{
			if (this._taskList.get_Count() < 1)
			{
				return;
			}
			if (this.StartType == null)
			{
				this._taskList.get_Item(0).task.Start();
			}
			else
			{
				for (int i = 0; i < this._taskList.get_Count(); i++)
				{
					this._taskList.get_Item(i).task.Start();
				}
			}
		}

		internal void _OnChildClosed(GameTask et)
		{
			int num = 0;
			int num2 = 0;
			int num3 = -1;
			bool flag = false;
			for (int i = 0; i < this._taskList.get_Count(); i++)
			{
				GameTaskGroup.TaskNode taskNode = this._taskList.get_Item(i);
				if (taskNode.task.Closed)
				{
					num2++;
					if (taskNode.task.Achieving == taskNode.achieve)
					{
						num++;
					}
				}
				if (taskNode.task == et)
				{
					num3 = i;
					flag = (et.Achieving == taskNode.achieve);
				}
			}
			if (num3 != -1)
			{
				base.Current = num;
				if (!base.Closed)
				{
					if (num2 >= this._taskList.get_Count() || (et.Closed && ((flag && this.CloseType == 1) || (!flag && this.CloseType == null))))
					{
						base.Close();
					}
					else if (this.StartType == null && ++num3 < this._taskList.get_Count())
					{
						this._taskList.get_Item(num3).task.Start();
					}
				}
			}
		}
	}
}
