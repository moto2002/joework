using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Geome
{
    public class Collision2DTools
    {
        #region public api
        public static bool Collision2D(Circle circle1, Circle circle2)
        {
            return CircleCollisionCircle(circle1.Trans.position, circle1.Radius, circle2.Trans.position, circle2.Radius);
        }

        public static bool Collision2D(Rectangle rect, Circle circle)
        {
            return RectangleCollisionCircle(rect.Trans.position, rect.Trans.forward, rect.Length, rect.Width, circle.Trans.position, circle.Radius);
        }

        public static bool Collision2D(Sector sector, Circle circle)
        {
            return SectorCollisionCircle(sector.Trans.position,sector.Trans.forward,sector.Angle,sector.Radius,circle.Trans.position,circle.Radius);
        }

        public static bool Collision2D(Rectangle rect1, Rectangle rect2)
        {
            return Intersect(rect1,rect2);
        }

        #region 矩形和矩形
        private static bool Intersect(Rectangle rect1, Rectangle rect2)
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
            float min_proj_box = Vector2.Dot(vecs_box[0], axis);
            int min_dot_box = 0;
            float max_proj_box = Vector2.Dot(vecs_box[0], axis);
            int max_dot_box = 0;

            for (int i = 1; i < vecs_box.Count; i++)
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
        #endregion

        #region private api
        private static bool CircleCollisionCircle(Vector3 _circleCenter1, float _radius1, Vector3 _circleCenter2, float _radius2)
        {
            _circleCenter1.y = 0;
            _circleCenter2.y = 0;

            float dis = _radius1 + _radius2;
            float c1c2 = (_circleCenter1 - _circleCenter2).sqrMagnitude;
            if (c1c2 <= dis * dis)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool RectangleCollisionCircle(Vector3 _rectCenter, Vector3 _rectDir, float _rectLength, float _rectWidth, Vector3 _circleCenter, float _radius)
        {
            _rectCenter.y = 0;
            _rectDir.y = 0;
            _circleCenter.y = 0;

            Vector3 rectangleCenter = _rectCenter + _rectDir.normalized * _rectWidth * 0.5f;//方盒起点到方盒中心点位置转换

            Quaternion q = Quaternion.LookRotation(_rectDir, Vector3.up);
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetTRS(rectangleCenter, q, Vector3.one);//local to world
            matrix = matrix.inverse;//world to local 

            Vector3 circleCenter = matrix.MultiplyPoint3x4(_circleCenter);

            circleCenter.x = Mathf.Abs(circleCenter.x);
            circleCenter.z = Mathf.Abs(circleCenter.z);

            float halfWidth = _rectLength * 0.5f;
            float halfHeight = _rectWidth * 0.5f;

            float a = circleCenter.x - halfWidth;
            float b = circleCenter.z - halfHeight;

            if (circleCenter.x <= halfWidth && circleCenter.z <= halfHeight)
            {
                return true;
            }
            else if (circleCenter.z > halfHeight && circleCenter.x < halfWidth)
            {
                if (b < _radius)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (circleCenter.z < halfHeight && circleCenter.x > halfWidth)
            {
                if (a < _radius)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (a * a + b * b <= _radius * _radius)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        private static bool SectorCollisionCircle(Vector3 _sectorCenter, Vector3 _sectorDir, float _sectorAngle, float _sectorRadius, Vector3 _circleCenter, float _circleRadius)
        {
            _sectorCenter.y = 0;
            _sectorDir.y = 0;
            _circleCenter.y = 0;

            float doubleDis = Vector3.SqrMagnitude(_circleCenter - _sectorCenter);

            if (doubleDis > (_sectorRadius + _circleRadius) * (_sectorRadius + _circleRadius))
            {
                //两圆不相交
                return false;
            }

            if (doubleDis < (_circleRadius) * (_circleRadius))
            {
                //扇形中心在园内 
                return true;

            }

            Quaternion q = Quaternion.LookRotation(_sectorDir, Vector3.up);

            Vector3 leftLine = Quaternion.Euler(0, _sectorAngle * -0.5f, 0) * _sectorDir;
            Vector3 rightLine = Quaternion.Euler(0, _sectorAngle * 0.5f, 0) * _sectorDir;

            Vector3 rightLineNormal = rightLine;
            rightLineNormal.x = rightLineNormal.z - rightLineNormal.x;
            rightLineNormal.z = rightLineNormal.x - rightLineNormal.z;
            rightLineNormal.x = rightLineNormal.x - rightLineNormal.z;

            Vector3 leftLineNormal = leftLine;
            leftLineNormal.x = leftLineNormal.z - leftLineNormal.x;
            leftLineNormal.z = leftLineNormal.x - leftLineNormal.z;
            leftLineNormal.x = leftLineNormal.x - leftLineNormal.z;

            Vector3 circleLine = _circleCenter - _sectorCenter;
            bool rightSide = Vector3.Dot(rightLineNormal, circleLine) <= 0;
            bool leftSide = Vector3.Dot(leftLineNormal, circleLine) >= 0;

            if (rightSide && leftSide && _sectorAngle < 180f)
            {
                return true;

            }

            if ((rightSide || leftSide) && _sectorAngle >= 180f)
            {
                return true;

            }

            if (rightSide == false)
            {
                //pan duan  you bian shi fou yu yuan xiangjiao 
                Vector3 rightLineNormalize = rightLine.normalized;
                Vector3 lineEnd = _sectorCenter + rightLineNormalize * _sectorRadius;
                if (LineCollisionCircle(_sectorCenter, lineEnd, _circleCenter, _circleRadius))
                {
                    return true;
                }

            }

            if (leftSide == false)
            {
                Vector3 leftLineNormalize = leftLine.normalized;
                Vector3 lineEnd = _sectorCenter + leftLineNormalize * _sectorRadius;
                if (LineCollisionCircle(_sectorCenter, lineEnd, _circleCenter, _circleRadius))
                {
                    return true;
                }

            }
            return false;

        }
        private static bool LineCollisionCircle(Vector3 _lineStart, Vector3 _lineEnd, Vector3 _circleCenter, float _radius)
        {

            float a = _lineEnd.x - _lineStart.x;
            float b = _lineEnd.z - _lineStart.z;
            float c = _circleCenter.x - _lineStart.x;
            float d = _circleCenter.z - _lineStart.z;

            Vector3 line = _lineEnd - _lineStart;
            Vector3 lineStart2Circle = _circleCenter - _lineStart;
            Vector3 lineNormal = new Vector3(-line.z, 0, line.x);

            float sqrRadius = _radius * _radius;
            float dot = Vector3.Dot(lineNormal, lineStart2Circle);
            float sqrDot = dot * dot;

            if (sqrDot <= sqrRadius * line.sqrMagnitude)
            {
                float sqrLineStart2CircleDis = lineStart2Circle.sqrMagnitude;
                if (sqrLineStart2CircleDis < sqrRadius)
                {
                    return true;
                }

                float sqrLineEnd2CircleDis = (_circleCenter - _lineEnd).sqrMagnitude;

                if (sqrLineEnd2CircleDis <= sqrRadius)
                {
                    return true;
                }


                float dot2 = Vector3.Dot(lineStart2Circle, line);
                if (dot2 >= 0 && dot2 <= line.sqrMagnitude)
                {

                    return true;
                }

            }

            return false;
        }
        private static bool LineCollisionCircle2(Vector3 _lineStart, Vector3 _lineEnd, Vector3 _circleCenter, float _radius)
        {
            float a = _lineEnd.x - _lineStart.x;
            float b = _lineEnd.z - _lineStart.z;
            float c = _circleCenter.x - _lineStart.x;
            float d = _circleCenter.z - _lineStart.z;

            float sqrRadius = _radius * _radius;

            if ((d * a - c * b) * (d * a - c * b) <= sqrRadius * (a * a + b * b))
            {
                if ((c * c + d * d) < sqrRadius)
                {
                    return true;
                }
                if ((a - c) * (a - c) + (b - d) * (b - d) <= sqrRadius)
                {
                    return true;
                }
                if (c * a + d * b >= 0 && c * a + d * b <= a * a + b * b)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}

