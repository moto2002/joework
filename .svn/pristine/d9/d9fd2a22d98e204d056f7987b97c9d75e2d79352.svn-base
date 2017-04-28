using System;
using System.Reflection;

namespace Assets.Scripts.GameLogic
{
	[SkillFuncHandlerClass]
	public class SkillFuncDelegator : Singleton<SkillFuncDelegator>
	{
		private DealSkillFunc[] SkillFuncHandlers = new DealSkillFunc[86];

		public override void Init()
		{
			ClassEnumerator classEnumerator = new ClassEnumerator(typeof(SkillFuncHandlerClassAttribute), null, typeof(SkillFuncDelegator).get_Assembly(), true, false, false);
			ListView<Type>.Enumerator enumerator = classEnumerator.get_results().GetEnumerator();
			while (enumerator.MoveNext())
			{
				Type current = enumerator.get_Current();
				MethodInfo[] methods = current.GetMethods();
				int num = 0;
				while (methods != null && num < methods.Length)
				{
					MethodInfo methodInfo = methods[num];
					if (methodInfo.get_IsStatic())
					{
						object[] customAttributes = methodInfo.GetCustomAttributes(typeof(SkillFuncHandlerAttribute), true);
						for (int i = 0; i < customAttributes.Length; i++)
						{
							SkillFuncHandlerAttribute skillFuncHandlerAttribute = customAttributes[i] as SkillFuncHandlerAttribute;
							if (skillFuncHandlerAttribute != null)
							{
								this.RegisterHandler(skillFuncHandlerAttribute.get_ID(), (DealSkillFunc)Delegate.CreateDelegate(typeof(DealSkillFunc), methodInfo));
								if (skillFuncHandlerAttribute.get_AdditionalIdList() != null)
								{
									int num2 = skillFuncHandlerAttribute.get_AdditionalIdList().Length;
									for (int j = 0; j < num2; j++)
									{
										this.RegisterHandler(skillFuncHandlerAttribute.get_AdditionalIdList()[j], (DealSkillFunc)Delegate.CreateDelegate(typeof(DealSkillFunc), methodInfo));
									}
								}
							}
						}
					}
					num++;
				}
			}
		}

		public void RegisterHandler(int inSkillFuncType, DealSkillFunc handler)
		{
			if (this.SkillFuncHandlers[inSkillFuncType] != null)
			{
				DebugHelper.Assert(false, "重复注册技能效果处理函数，请检查");
				return;
			}
			this.SkillFuncHandlers[inSkillFuncType] = handler;
		}

		public bool DoSkillFunc(int inSkillFuncType, ref SSkillFuncContext inContext)
		{
			DealSkillFunc dealSkillFunc = this.SkillFuncHandlers[inSkillFuncType];
			return dealSkillFunc != null && dealSkillFunc(ref inContext);
		}
	}
}
