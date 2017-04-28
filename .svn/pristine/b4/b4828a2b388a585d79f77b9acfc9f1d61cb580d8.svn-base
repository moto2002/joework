using System;

namespace Apollo
{
	public class ApolloGroupResult : ApolloStruct<ApolloGroupResult>
	{
		public ApolloGroupInfo groupInfo;

		public ApolloResult result
		{
			get;
			set;
		}

		public int errorCode
		{
			get;
			set;
		}

		public string desc
		{
			get;
			set;
		}

		public override ApolloGroupResult FromString(string src)
		{
			string[] array = src.Split(new char[]
			{
				'&'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string[] array3 = text.Split(new char[]
				{
					'='
				});
				if (array3.Length > 1)
				{
					if (array3[0].CompareTo("Result") == 0)
					{
						this.result = (ApolloResult)int.Parse(array3[1]);
					}
					else if (array3[0].CompareTo("ErrorCode") == 0)
					{
						this.errorCode = int.Parse(array3[1]);
					}
					else if (array3[0].CompareTo("szDescription") == 0)
					{
						this.desc = array3[1];
					}
					else if (array3[0].CompareTo("GroupInfo") == 0)
					{
						string src2 = ApolloStringParser.ReplaceApolloString(array3[1]);
						this.groupInfo = new ApolloGroupInfo();
						this.groupInfo.FromString(src2);
					}
				}
			}
			return this;
		}
	}
}
