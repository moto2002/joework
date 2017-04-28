using System;

namespace Assets.Scripts.GameLogic
{
	public class PassiveCreater<CreateType, CreateTypeAttribute> : Singleton<PassiveCreater<CreateType, CreateTypeAttribute>> where CreateTypeAttribute : PassivetAttribute
	{
		private DictionaryView<int, Type> eventTypeSet = new DictionaryView<int, Type>();

		public override void Init()
		{
			ClassEnumerator classEnumerator = new ClassEnumerator(typeof(CreateTypeAttribute), typeof(CreateType), typeof(CreateTypeAttribute).get_Assembly(), true, false, false);
			using (ListView<Type>.Enumerator enumerator = classEnumerator.get_results().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type current = enumerator.get_Current();
					Attribute customAttribute = Attribute.GetCustomAttribute(current, typeof(CreateTypeAttribute));
					this.eventTypeSet.Add((customAttribute as CreateTypeAttribute).attributeType, current);
				}
			}
		}

		public CreateType Create(int _type)
		{
			Type type;
			if (this.eventTypeSet.TryGetValue(_type, ref type))
			{
				return (CreateType)((object)Activator.CreateInstance(type));
			}
			return default(CreateType);
		}
	}
}
