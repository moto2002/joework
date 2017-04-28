using System;
using System.Collections.Generic;
using UnityEngine;

namespace behaviac
{
	public class Context
	{
		private static DictionaryView<int, Context> ms_contexts = new DictionaryView<int, Context>();

		private DictionaryView<string, Agent> m_namedAgents = new DictionaryView<string, Agent>();

		private DictionaryView<string, Variables> m_static_variables = new DictionaryView<string, Variables>();

		private DictionaryView<string, DictionaryView<CStringID, CNamedEvent>> ms_eventInfosGlobal = new DictionaryView<string, DictionaryView<CStringID, CNamedEvent>>();

		private World m_world;

		private Context(int contextId)
		{
			this.m_world = null;
		}

		~Context()
		{
			this.m_world = null;
			this.CleanupStaticVariables();
			this.CleanupInstances();
			this.ms_eventInfosGlobal.Clear();
		}

		public static Context GetContext(int contextId)
		{
			if (Context.ms_contexts.ContainsKey(contextId))
			{
				return Context.ms_contexts.get_Item(contextId);
			}
			Context context = new Context(contextId);
			Context.ms_contexts.set_Item(contextId, context);
			return context;
		}

		public static void Cleanup(int contextId)
		{
			if (Context.ms_contexts != null)
			{
				if (contextId == -1)
				{
					Context.ms_contexts.Clear();
				}
				else if (Context.ms_contexts.ContainsKey(contextId))
				{
					Context.ms_contexts.Remove(contextId);
				}
			}
		}

		public void SetWorld(World pWorld)
		{
			if (this.m_world == null)
			{
				this.m_world = pWorld;
			}
			else
			{
				if (pWorld != null && this.m_world is DefaultWorld)
				{
					pWorld.Agents = this.m_world.Agents;
					this.m_world.Agents.Clear();
				}
				this.m_world = pWorld;
			}
		}

		public World GetWorld(bool bCreate)
		{
			if (object.ReferenceEquals(this.m_world, null) && bCreate && object.ReferenceEquals(this.m_world, null))
			{
				GameObject gameObject = new GameObject("_world_");
				Object.DontDestroyOnLoad(gameObject);
				this.m_world = gameObject.AddComponent<DefaultWorld>();
			}
			return this.m_world;
		}

		public void LogStaticVariables(string agentClassName)
		{
			if (!string.IsNullOrEmpty(agentClassName))
			{
				if (this.m_static_variables.ContainsKey(agentClassName))
				{
					Variables variables = this.m_static_variables.get_Item(agentClassName);
					variables.Log(null, false);
				}
			}
			else
			{
				using (DictionaryView<string, Variables>.Enumerator enumerator = this.m_static_variables.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, Variables> current = enumerator.get_Current();
						current.get_Value().Log(null, false);
					}
				}
			}
		}

		public static void LogCurrentStates()
		{
			if (Context.ms_contexts != null)
			{
				using (DictionaryView<int, Context>.Enumerator enumerator = Context.ms_contexts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, Context> current = enumerator.get_Current();
						if (!object.ReferenceEquals(current.get_Value().m_world, null))
						{
							current.get_Value().m_world.LogCurrentStates();
						}
					}
				}
			}
		}

		private void CleanupStaticVariables()
		{
			using (DictionaryView<string, Variables>.Enumerator enumerator = this.m_static_variables.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, Variables> current = enumerator.get_Current();
					current.get_Value().Clear();
				}
			}
			this.m_static_variables.Clear();
		}

		public void ResetChangedVariables()
		{
			using (DictionaryView<string, Variables>.Enumerator enumerator = this.m_static_variables.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, Variables> current = enumerator.get_Current();
					current.get_Value().Reset();
				}
			}
		}

		private void CleanupInstances()
		{
			this.m_namedAgents.Clear();
		}

		public void SetStaticVariable<VariableType>(CMemberBase pMember, string variableName, VariableType value, string staticClassName, uint variableId)
		{
			if (!this.m_static_variables.ContainsKey(staticClassName))
			{
				this.m_static_variables.set_Item(staticClassName, new Variables());
			}
			Variables variables = this.m_static_variables.get_Item(staticClassName);
			variables.Set(null, pMember, variableName, value, variableId);
		}

		private static bool GetClassNameString(string variableName, ref string className)
		{
			int num = variableName.LastIndexOf(':');
			if (num > 0)
			{
				className = variableName.Substring(0, num - 1);
				return true;
			}
			className = variableName;
			return true;
		}

		public CNamedEvent FindEventStatic(string eventName, string className)
		{
			if (this.ms_eventInfosGlobal.ContainsKey(className))
			{
				DictionaryView<CStringID, CNamedEvent> dictionaryView = this.ms_eventInfosGlobal.get_Item(className);
				CStringID cStringID = new CStringID(eventName);
				if (dictionaryView.ContainsKey(cStringID))
				{
					return dictionaryView.get_Item(cStringID);
				}
			}
			return null;
		}

		public void InsertEventGlobal(string className, CNamedEvent pEvent)
		{
			if (this.FindEventStatic(className, pEvent.GetName()) == null)
			{
				if (!this.ms_eventInfosGlobal.ContainsKey(className))
				{
					this.ms_eventInfosGlobal.Add(className, new DictionaryView<CStringID, CNamedEvent>());
				}
				DictionaryView<CStringID, CNamedEvent> dictionaryView = this.ms_eventInfosGlobal.get_Item(className);
				CNamedEvent cNamedEvent = (CNamedEvent)pEvent.clone();
				CStringID cStringID = new CStringID(cNamedEvent.GetName());
				dictionaryView.Add(cStringID, cNamedEvent);
			}
		}

		public CNamedEvent FindNamedEventTemplate(ListView<CMethodBase> methods, string eventName)
		{
			CStringID b = new CStringID(eventName);
			int i = methods.get_Count() - 1;
			while (i >= 0)
			{
				CMethodBase cMethodBase = methods.get_Item(i);
				string name = cMethodBase.GetName();
				CStringID a = new CStringID(name);
				if (a == b && cMethodBase.IsNamedEvent())
				{
					CNamedEvent cNamedEvent = (CNamedEvent)cMethodBase;
					if (cNamedEvent.IsStatic())
					{
						this.InsertEventGlobal(cNamedEvent.GetClassNameString(), cNamedEvent);
						return cNamedEvent;
					}
					return cNamedEvent;
				}
				else
				{
					i--;
				}
			}
			return null;
		}

		public bool BindInstance(Agent pAgentInstance, string agentInstanceName)
		{
			if (string.IsNullOrEmpty(agentInstanceName))
			{
				agentInstanceName = pAgentInstance.GetType().get_FullName();
			}
			if (Agent.IsNameRegistered(agentInstanceName))
			{
				string registeredClassName = Agent.GetRegisteredClassName(agentInstanceName);
				if (Agent.IsDerived(pAgentInstance, registeredClassName))
				{
					this.m_namedAgents.set_Item(agentInstanceName, pAgentInstance);
					return true;
				}
			}
			return false;
		}

		public bool BindInstance(Agent pAgentInstance)
		{
			return this.BindInstance(pAgentInstance, null);
		}

		public bool UnbindInstance(string agentInstanceName)
		{
			if (Agent.IsNameRegistered(agentInstanceName))
			{
				if (this.m_namedAgents.ContainsKey(agentInstanceName))
				{
					this.m_namedAgents.Remove(agentInstanceName);
					return true;
				}
			}
			return false;
		}

		public bool UnbindInstance<T>()
		{
			string fullName = typeof(T).get_FullName();
			return this.UnbindInstance(fullName);
		}

		public Agent GetInstance(string agentInstanceName)
		{
			bool flag = !string.IsNullOrEmpty(agentInstanceName);
			if (!flag)
			{
				return null;
			}
			string text = null;
			Context.GetClassNameString(agentInstanceName, ref text);
			if (this.m_namedAgents.ContainsKey(text))
			{
				return this.m_namedAgents.get_Item(text);
			}
			return null;
		}
	}
}
