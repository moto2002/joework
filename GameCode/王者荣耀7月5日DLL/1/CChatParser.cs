using Assets.Scripts.GameSystem;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class CChatParser
{
	public class LabelType
	{
		public string info;

		public CChatParser.InfoType type;

		public LabelType(string text, CChatParser.InfoType tp)
		{
			this.info = text;
			this.type = tp;
		}
	}

	public enum InfoType
	{
		Text,
		Face
	}

	private const string SPEAKER_STR = "        ";

	private const string SPEAKER_REP_STR = "{0}{1}";

	public static int chat_entry_lineHeight = 39;

	public static int chat_list_max_width = 355;

	public static int chat_entry_channel_img_width = 47;

	public static int start_x = 53;

	public static int chatFaceWidth = 26;

	public static int chatFaceHeight = 34;

	public static int lineHeight = 34;

	public int maxWidth;

	public int viewFontSize = 18;

	public static float element_width = 547.4f;

	public static float element_height = 86f;

	public static float element_half_height = 43f;

	private int mPositionX;

	private int mPositionY;

	private int mOriginalPositionX = 5;

	private int mWidth;

	private ListView<CChatParser.LabelType> mList = new ListView<CChatParser.LabelType>();

	private CChatEntity curEntNode;

	public int chatCell_InitHeight = 80;

	public int chatCell_patchHeight = 10;

	public int chatCell_DeltaHeight = 30;

	public bool bProc_ChatEntry;

	public static int[] ascii_width = new int[]
	{
		7,
		6,
		7,
		15,
		12,
		20,
		15,
		4,
		9,
		9,
		10,
		17,
		6,
		6,
		6,
		6,
		12,
		12,
		12,
		12,
		12,
		12,
		12,
		12,
		12,
		12,
		6,
		6,
		17,
		17,
		17,
		11,
		20,
		15,
		13,
		15,
		16,
		12,
		11,
		18,
		16,
		7,
		10,
		14,
		10,
		20,
		17,
		18,
		13,
		18,
		14,
		13,
		11,
		16,
		14,
		20,
		14,
		13,
		14,
		8,
		6,
		8,
		20,
		10,
		10,
		14,
		14,
		11,
		14,
		13,
		7,
		14,
		13,
		6,
		7,
		13,
		6,
		19,
		13,
		13,
		14,
		14,
		9,
		10,
		8,
		13,
		11,
		18,
		11,
		12,
		11,
		10,
		10,
		10,
		17
	};

	public void Parse(string value, int startX, CChatEntity entNode)
	{
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		this.curEntNode = entNode;
		this.mList.Clear();
		this.mWidth = 0;
		this.mPositionY = -CChatParser.chatFaceHeight;
		this.mPositionX = (this.mOriginalPositionX = 8);
		this.ParseText(value);
		this.ShowText();
	}

	private void ParseText(string value)
	{
		if (this.curEntNode != null && (this.curEntNode.type == EChaterType.Speaker || this.curEntNode.type == EChaterType.LoudSpeaker))
		{
			value = string.Format("{0}{1}", "        ", value);
		}
		string text = value;
		string text2 = "(%\\d+)";
		MatchCollection matchCollection = null;
		if (!string.IsNullOrEmpty(value))
		{
			try
			{
				matchCollection = Regex.Matches(value, text2);
			}
			catch (Exception)
			{
			}
		}
		if (matchCollection != null && matchCollection.get_Count() > 0)
		{
			int count = matchCollection.get_Count();
			for (int i = 0; i < count; i++)
			{
				Match match = matchCollection.get_Item(i);
				string value2 = match.get_Value();
				if (!string.IsNullOrEmpty(value2))
				{
					int num = text.IndexOf(value2);
					int num2 = num + value2.get_Length();
					if (num > -1)
					{
						string text3 = text.Substring(0, num);
						if (!string.IsNullOrEmpty(text3))
						{
							this.mList.Add(new CChatParser.LabelType(text3, CChatParser.InfoType.Text));
						}
						if (!string.IsNullOrEmpty(value2))
						{
							this.mList.Add(new CChatParser.LabelType(value2, CChatParser.InfoType.Face));
						}
						text = text.Substring(num2);
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.mList.Add(new CChatParser.LabelType(text, CChatParser.InfoType.Text));
			}
		}
		else
		{
			this.mList.Add(new CChatParser.LabelType(text, CChatParser.InfoType.Text));
		}
	}

	private void ShowText()
	{
		int count = this.mList.get_Count();
		for (int i = 0; i < count; i++)
		{
			CChatParser.LabelType labelType = this.mList.get_Item(i);
			CChatParser.InfoType type = labelType.type;
			if (type != CChatParser.InfoType.Text)
			{
				if (type == CChatParser.InfoType.Face)
				{
					this.CreateTextFace(labelType.info);
				}
			}
			else
			{
				this.CreateText(labelType.info);
			}
		}
		if (this.curEntNode != null)
		{
			this.curEntNode.final_width += 8f;
		}
	}

	private void CreateText(string value)
	{
		string text = string.Empty;
		string value2 = string.Empty;
		int num = value.IndexOf("\\n");
		if (num != -1)
		{
			text = value.Substring(0, num);
			value2 = value.Substring(num + 2);
			if (!string.IsNullOrEmpty(text))
			{
				this.CreateText(text);
			}
			if (!this.bProc_ChatEntry)
			{
				return;
			}
			this.mPositionX = this.mOriginalPositionX;
			this.mPositionY -= CChatParser.lineHeight;
			if (!string.IsNullOrEmpty(text))
			{
				this.CreateText(value2);
			}
		}
		else
		{
			string empty = string.Empty;
			float num2 = 0f;
			bool flag = this.WrapText(value, (float)this.mPositionX, out empty, out num2);
			if (flag)
			{
				int num3 = empty.IndexOf("\\n");
				text = empty.Substring(0, num3);
				value2 = empty.Substring(num3 + 2);
				if (!string.IsNullOrEmpty(text))
				{
					this.CreateText(text);
				}
				if (this.bProc_ChatEntry)
				{
					return;
				}
				this.mPositionX = this.mOriginalPositionX;
				this.mPositionY -= CChatParser.lineHeight;
				if (!string.IsNullOrEmpty(text))
				{
					this.CreateText(value2);
				}
			}
			else
			{
				num2 += 2.5f;
				if (this.curEntNode == null)
				{
					return;
				}
				this.curEntNode.TextObjList.Add(new CTextImageNode(value, true, num2, 0f, (float)this.mPositionX, (float)this.mPositionY));
				if ((float)this.mPositionY < this.curEntNode.final_height)
				{
					this.curEntNode.final_height = (float)this.mPositionY;
				}
				if (this.curEntNode.final_height < (float)(-(float)CChatParser.lineHeight * 2))
				{
					this.curEntNode.numLine = 3u;
				}
				else if (this.curEntNode.final_height < (float)(-(float)CChatParser.lineHeight))
				{
					this.curEntNode.numLine = 2u;
				}
				else
				{
					this.curEntNode.numLine = 1u;
				}
				this.mPositionX += (int)num2;
				if ((float)this.mPositionX > this.curEntNode.final_width)
				{
					this.curEntNode.final_width = (float)this.mPositionX;
				}
			}
		}
	}

	private void CreateTextFace(string value)
	{
		if (this.curEntNode == null)
		{
			return;
		}
		bool flag = false;
		if (CChatParser.chatFaceWidth + this.mPositionX > this.maxWidth)
		{
			if (this.bProc_ChatEntry)
			{
				return;
			}
			this.mPositionX = this.mOriginalPositionX;
			this.mPositionY -= CChatParser.chatFaceHeight;
			this.curEntNode.numLine = 2u;
			flag = true;
		}
		this.curEntNode.TextObjList.Add(new CTextImageNode(value.Substring(1), false, (float)CChatParser.chatFaceWidth, 0f, (float)this.mPositionX, (float)(-(float)CChatParser.chatFaceHeight)));
		this.mPositionX += CChatParser.chatFaceWidth;
		this.mWidth = Mathf.Max(this.mPositionX, this.mWidth);
		if (!flag)
		{
			this.curEntNode.final_width += (float)CChatParser.chatFaceWidth;
		}
	}

	private bool WrapText(string text, float curPosX, out string finalText, out float length)
	{
		float num = curPosX;
		float num2 = 0f;
		int num3 = -1;
		for (int i = 0; i < text.get_Length(); i++)
		{
			char ch = text.get_Chars(i);
			float num4 = (float)CChatParser.GetCharacterWidth(ch, this.viewFontSize);
			num += num4;
			num2 += num4;
			if (num > (float)this.maxWidth)
			{
				num3 = i;
				break;
			}
		}
		length = num2;
		if (num3 != -1)
		{
			text = text.Insert(num3, "\\n");
		}
		finalText = text;
		return num3 != -1;
	}

	public static int GetCharacterWidth(char ch, int fontSize)
	{
		if (ch >= ' ' && ch <= '~')
		{
			return CChatParser.ascii_width[(int)(ch - ' ')];
		}
		return fontSize;
	}
}
