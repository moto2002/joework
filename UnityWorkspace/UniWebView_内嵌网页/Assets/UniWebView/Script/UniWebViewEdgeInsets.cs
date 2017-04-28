//
//	UniWebViewEdgeInsets.cs
//  Created by Wang Wei(@onevcat) on 2013-10-20.
//
using UnityEngine;

[System.Serializable]
/// <summary>
/// This class defined the edge inset of a UniWebView
/// </summary>
public class UniWebViewEdgeInsets {

	public int top, left, bottom, right;

	/// <summary>
	/// Initializes a new instance of the <see cref="UniWebViewEdgeInsets"/> class.
	/// </summary>
	/// <param name="aTop">Top inset by point.</param>
	/// <param name="aLeft">Left inset by point.</param>
	/// <param name="aBottom">Bottominset by point.</param>
	/// <param name="aRight">Rightinset by point.</param>
	public UniWebViewEdgeInsets(int aTop, int aLeft, int aBottom, int aRight) {
		top = aTop;
		left = aLeft;
		bottom = aBottom;
		right = aRight;
	}

	public static bool operator ==(UniWebViewEdgeInsets inset1, UniWebViewEdgeInsets inset2) 
	{
		return inset1.Equals(inset2);
	}

	public static bool operator !=(UniWebViewEdgeInsets inset1, UniWebViewEdgeInsets inset2) 
	{
		return !inset1.Equals(inset2);
	}

	public override int GetHashCode()
	{
		var calculation = top + left + bottom + right;
		return calculation.GetHashCode();
	}

	public override bool Equals (object obj)
	{
		if (obj == null || GetType() != obj.GetType()) {
			return false;
		}
		UniWebViewEdgeInsets anInset = (UniWebViewEdgeInsets)obj;
		return  (top 	== anInset.top) && 
				(left   == anInset.left) && 
				(bottom == anInset.bottom) && 
				(right  == anInset.right);
	}
}