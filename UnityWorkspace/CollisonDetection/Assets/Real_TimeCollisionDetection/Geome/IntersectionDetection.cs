using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntersectionDetection 
{
	#region 矩形和矩形
	public static bool Intersect(Rectangle rect1,Rectangle rect2)
	{
        bool isSeparated = false;
		
        //prepare the normals
        List<Vector2> normal_box1 = rect1.GetNormals();
        List<Vector2> normal_box2 = rect2.GetNormals();

        List<Vector2> vecs_box1 = rect1.PrepareVector();
        List<Vector2> vecs_box2 = rect2.PrepareVector();

        MResult result_P1 = getMinMax(vecs_box1, normal_box1[1]);
        MResult result_P2 = getMinMax(vecs_box2, normal_box1[1]);
        MResult result_Q1 = getMinMax(vecs_box1, normal_box1[0]);
        MResult result_Q2 = getMinMax(vecs_box2, normal_box1[0]);

        MResult result_R1 = getMinMax(vecs_box1, normal_box2[1]);
        MResult result_R2 = getMinMax(vecs_box2, normal_box2[1]);
        MResult result_S1 = getMinMax(vecs_box1, normal_box2[0]);
        MResult result_S2 = getMinMax(vecs_box2, normal_box2[0]);


        bool separate_p = result_P1.m_max_proj < result_P2.m_min_proj || result_P2.m_max_proj < result_P1.m_min_proj;
        bool separate_Q = result_Q1.m_max_proj < result_Q2.m_min_proj || result_Q2.m_max_proj < result_Q1.m_min_proj;
        bool separate_R = result_R1.m_max_proj < result_R2.m_min_proj || result_R2.m_max_proj < result_R1.m_min_proj;
        bool separate_S = result_S1.m_max_proj < result_S2.m_min_proj || result_S2.m_max_proj < result_S1.m_min_proj;

        isSeparated = separate_p || separate_Q || separate_R || separate_S;

        return !isSeparated;
	}
    private static MResult getMinMax(List<Vector2> vecs_box, Vector2 axis)
    {
        float min_proj_box = Vector2.Dot(vecs_box[1], axis);
        int min_dot_box = 1;
        float max_proj_box = Vector2.Dot(vecs_box[1], axis);
        int max_dot_box = 1;

        for (int i = 2; i < vecs_box.Count; i++)
        {
            float curr_proj = Vector2.Dot(vecs_box[i], axis);
            if (min_proj_box > curr_proj)
            {
                min_proj_box = curr_proj;
                min_dot_box = i;
            }

            if (curr_proj > max_proj_box)
            {
                max_proj_box = curr_proj;
                max_dot_box = i;
            }
        }

        return new MResult(min_proj_box, max_proj_box);
    }
    private class MResult
    {
        public float m_min_proj;
        public float m_max_proj;

        public MResult(float min_proj_box, float max_proj_box)
        {
            this.m_min_proj = min_proj_box;
            this.m_max_proj = max_proj_box;
        }

    }
	#endregion

	#region 矩形和圆
	public static bool Intersect(Rectangle rect,Circle circle)
	{
		Vector2 v;
		Vector2 current_box_corner;
		Vector2 center_box = rect.center;

		float max = float.MinValue;
		Vector2 box2circle = new Vector2(circle.center.x - rect.center.x,circle.center.y - rect.center.y);
		Vector2 box2circle_normalised = box2circle.normalized;

		for (int i = 1; i <=4; i++)
		{
			current_box_corner = rect.GetVertex(i);
			v = new Vector2(current_box_corner.x - center_box.x,current_box_corner.y - center_box.y);

			float current_proj = Vector2.Dot(v,box2circle_normalised);

			if(max < current_proj)
				max = current_proj;
		}

		if(box2circle.magnitude - max - circle.radius > 0 && box2circle.magnitude > 0)
			return false;
		else
			return true;
	}
	public static bool Intersect(Circle circle,Rectangle rect)
	{
		return Intersect(rect,circle);
	}
	#endregion

	#region 

	#endregion
}
