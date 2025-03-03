using System;
using System.Collections.Generic;
using UnityEngine;

namespace AGE
{
	public class TriggerHelper : MonoBehaviour
	{
		private DictionaryObjectView<GameObject, float> collisionSet = new DictionaryObjectView<GameObject, float>();

		private void OnTriggerEnter(Collider _other)
		{
			GameObject gameObject = _other.gameObject;
			if (!this.collisionSet.ContainsKey(gameObject))
			{
				this.collisionSet.Add(gameObject, Time.realtimeSinceStartup);
			}
		}

		private void OnTriggerExit(Collider _other)
		{
			GameObject gameObject = _other.gameObject;
			if (this.collisionSet.ContainsKey(gameObject))
			{
				this.collisionSet.Remove(gameObject);
			}
		}

		public List<GameObject> GetCollisionSet()
		{
			List<GameObject> list = new List<GameObject>();
			using (DictionaryObjectView<GameObject, float>.Enumerator enumerator = this.collisionSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<GameObject, float> current = enumerator.get_Current();
					list.Add(current.get_Key());
				}
			}
			return list;
		}
	}
}
