using UnityEngine;
using System.Collections;

public class IsoUtil {

	public static readonly float Y_CORRECT = Mathf.Cos(-Mathf.PI / 6) * Mathf.Sqrt(2);

	//宽高比,0.75为coc地图，格式宽为40，高为30。0.5为正常的45度地图，格式宽为60，高为30
	public const float W_H_RATE = 0.75f ;

	/// <summary>
	/// Iso坐标转屏幕坐标
	/// </summary>
	/// <returns><c>true</c> if iso to screen the specified px py pz; otherwise, <c>false</c>.</returns>
	/// <param name="px">Px.</param>
	/// <param name="py">Py.</param>
	/// <param name="pz">Pz.</param>
	public static Vector2 IsoToScreen(float px,float py ,float pz){
		float screenX = px - pz;
		float screenY = py * Y_CORRECT + (px + pz) * W_H_RATE;
		return new Vector2(screenX*0.01f, -screenY*0.01f);
	}

	/// <summary>
	/// 场景坐标到iso地图像素坐标的转换
	/// </summary>
	/// <returns>The to iso.</returns>
	/// <param name="px">Px.</param>
	/// <param name="py">Py.</param>
	public static Vector2 ScreenToIso(float px,float py )
	{
		px*=100f;
		py*=100f;
		float zpos=(-py-px*W_H_RATE)/(2*W_H_RATE);
		float xpos = px+zpos;
		float ypos = 0;
		return new Vector2(xpos,ypos);
	}

	/// <summary>
	/// 场景坐标转到iso网格坐标
	/// </summary>
	/// <returns>The to iso grid.</returns>
	/// <param name="size">Size.</param>
	/// <param name="px">Px.</param>
	/// <param name="py">Py.</param>
	public static Vector2 ScreenToIsoGrid(float size,float px ,float py)
	{
		px*=100f;
		py*=100f;
		float zpos = ( -py-px*W_H_RATE )/(2*W_H_RATE);
		float xpos = px + zpos;

		int col = Mathf.FloorToInt ( xpos / size );
		int row = Mathf.FloorToInt ( zpos / size );
		return new Vector2(col,row);
	}
}
