using System;

namespace Apollo
{
	public class ApolloGroupInfo : ApolloStruct<ApolloGroupInfo>
	{
		public string groupName
		{
			get;
			set;
		}

		public string fingerMemo
		{
			get;
			set;
		}

		public string memberNum
		{
			get;
			set;
		}

		public string maxNum
		{
			get;
			set;
		}

		public string ownerOpenid
		{
			get;
			set;
		}

		public string unionid
		{
			get;
			set;
		}

		public string zoneid
		{
			get;
			set;
		}

		public string adminOpenids
		{
			get;
			set;
		}

		public string groupOpenid
		{
			get;
			set;
		}

		public string groupKey
		{
			get;
			set;
		}

		public override ApolloGroupInfo FromString(string src)
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
					if (array3[0].CompareTo("GroupName") == 0)
					{
						this.groupName = array3[1];
					}
					else if (array3[0].CompareTo("FingerMemo") == 0)
					{
						this.fingerMemo = array3[1];
					}
					else if (array3[0].CompareTo("MemberNum") == 0)
					{
						this.memberNum = array3[1];
					}
					else if (array3[0].CompareTo("MaxNum") == 0)
					{
						this.maxNum = array3[1];
					}
					else if (array3[0].CompareTo("OwnerOpenid") == 0)
					{
						this.ownerOpenid = array3[1];
					}
					else if (array3[0].CompareTo("Unionid") == 0)
					{
						this.unionid = array3[1];
					}
					else if (array3[0].CompareTo("Zoneid") == 0)
					{
						this.zoneid = array3[1];
					}
					else if (array3[0].CompareTo("AdminOpenids") == 0)
					{
						this.adminOpenids = array3[1];
					}
					else if (array3[0].CompareTo("GroupOpenid") == 0)
					{
						this.groupOpenid = array3[1];
					}
					else if (array3[0].CompareTo("GroupKey") == 0)
					{
						this.groupKey = array3[1];
					}
				}
			}
			return this;
		}
	}
}
