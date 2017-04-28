using System.Collections.Generic;
using UnityEngine;

namespace BoTing.GamePublic
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
    public class CardFlip : MonoBehaviour
    {
        private bool pivotLeftCenter = true;
        /// <summary>
        /// 锚点是否中心
        /// </summary>
        public bool PivotLeftCenter
        {
            get { return pivotLeftCenter; }
            set
            {
                pivotLeftCenter = value;
                Reset();
            }
        }
        private bool visible = true;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = /*gameObject.GetComponent<MeshRenderer>().enabled =*/ value;
            }
        }
        /// <summary>
        /// 翻转角度
        /// </summary>
        public float turn = 0.0f;

        private Mesh _mesh;
        private Mesh mesh
        {
            get
            {
                if (_mesh != null)
                {
                    return _mesh;
                }
                MeshFilter mf = GetComponent<MeshFilter>();
                if (mf != null)
                {
                    _mesh = mf.sharedMesh;
                    if (_mesh == null)
                    {
                        _mesh = new Mesh();
                    }
                    return _mesh;
                }
                MeshFilter nm = gameObject.AddComponent<MeshFilter>();
                _mesh = nm.sharedMesh = new Mesh();
                return _mesh;
            }
            set
            {
                _mesh = GetComponent<MeshFilter>().mesh = value;
            }
        }

        private float rho = 0.0f;
        private float theta = 0.0f;
        private float deltaT = 0.0f;
        private float ap1 = -15.0f;
        private float ap2 = -2.5f;
        private float ap3 = -3.5f;
        private bool flipx = true;
        private float fx = 1.0f;
        private Vector3 apex = new Vector3(0.0f, 0.0f, -3.0f);

        [System.NonSerialized]
        private Matrix4x4 tm = new Matrix4x4();
        [System.NonSerialized]
        private Matrix4x4 invtm = new Matrix4x4();

        // new for mt
        private Vector3[] verts;
        private Vector3[] sverts;
        private Mesh cachedMesh;
        private Vector2[] uvs;
        private Vector2[] suvs;
        private int UpdateMesh = 0;
        private ModChannel dirtyChannels;   // =========== TODO: ===========

        private void Reset()
        {
            mesh = Instantiate(Build(PivotLeftCenter));
            mesh.name = "Card";
            cachedMesh = Instantiate(mesh);
            sverts = new Vector3[cachedMesh.vertexCount];
            verts = cachedMesh.vertices;
            uvs = cachedMesh.uv;
            suvs = new Vector2[cachedMesh.uv.Length];
            UpdateMesh = -1;
        }

        private void Awake()
        {
            Reset();
        }

        private void Update()
        {
            if (Visible)
            {
                ModifyObject();
            }
        }

        private void OnBecameVisible()
        {
            Visible = true;
        }

        private void OnBecameInvisible()
        {
            Visible = false;
        }

        #region Assist Methodes

        private void ModifyObject()
        {
            dirtyChannels = ModChannel.None;

            tm = Matrix4x4.identity;
            Quaternion rot = Quaternion.Euler(-Vector3.zero);

            tm.SetTRS(Vector3.zero, rot, Vector3.one);
            invtm = tm.inverse;

            if (Prepare())
            {
                if (UpdateMesh < 1)
                {
                    Modify();
                    UpdateMesh = 1;
                }
                else
                {
                    Modify();
                }
                dirtyChannels |= ModChannel.Verts;
            }

            if (UpdateMesh == 1)
            {
                SetMesh(ref sverts);
                UpdateMesh = 0;
            }
            else
            {
                if (UpdateMesh == 0)
                {
                    SetMesh(ref verts);
                    UpdateMesh = -1;    // Dont need to set verts again until a mod is enabled
                }
            }
        }
        private void SetMesh(ref Vector3[] _verts)
        {
            if (mesh == null)
                return;

            if ((dirtyChannels & ModChannel.Verts) != 0)
                mesh.vertices = sverts; //_verts;	// mesh.vertices = GetVerts(true);

            if ((dirtyChannels & ModChannel.UV) != 0)
                mesh.uv = suvs; //GetUVs(true);
        }
        private void Modify()
        {
            //Vector3[]	verts = mc.GetSourceVerts();
            //Vector3[]	sverts = mc.GetDestVerts();
            if (verts != null)
            {
                for (int i = 0; i < verts.Length; i++)
                    sverts[i] = Map(i, verts[i]);
            }
        }
        private Vector3 Map(int i, Vector3 p)
        {
            p = tm.MultiplyPoint3x4(p);

            // ---- curlTurn ----
            float rhs = Mathf.Sqrt((p.x * p.x) + Mathf.Pow((p.z - apex.z), 2.0f));
            float num2 = rhs * Mathf.Sin(theta);
            float f = Mathf.Asin(p.x / rhs) / Mathf.Sin(theta);
            p.x = num2 * Mathf.Sin(f);
            p.z = (rhs + apex.z) - ((num2 * (1.0f - Mathf.Cos(f))) * Mathf.Sin(theta));
            p.y = (num2 * (1.0f - Mathf.Cos(f))) * Mathf.Cos(theta);
            // ------------------

            p.x *= fx;

            // ---- RotPage -----
            float x = p.x;
            float y = p.y;
            p.x = Mathf.Cos(rho * Mathf.Deg2Rad) * x + Mathf.Sin(rho * Mathf.Deg2Rad) * y;
            p.y = Mathf.Sin(rho * Mathf.Deg2Rad) * -x + Mathf.Cos(rho * Mathf.Deg2Rad) * y;
            // ------------------

            p.x *= fx;
            return invtm.MultiplyPoint3x4(p);
        }

        private bool Prepare()
        {
            if (flipx)
                fx = -1.0f;
            else
                fx = 1.0f;

            theta = 15.0f * Mathf.Deg2Rad;

            if (turn < 0.0f)
                turn = 0.0f;

            if (turn > 100.0f)
                turn = 100.0f;

            deltaT = turn / 100.0f;

            CalcAuto(deltaT);

            return true;
        }
        private void CalcAuto(float t)
        {
            float num = 90.0f * Mathf.Deg2Rad;
            if (t == 0.0f)
            {
                rho = 0.0f;
                theta = num;
                apex.z = ap1;   //-15.0f;
            }
            else
            {
                float num2;
                float num3;
                float num4;
                if (t <= 0.15f)
                {
                    num2 = t / 0.15f;
                    num3 = Mathf.Sin((Mathf.PI * Mathf.Pow(num2, 0.05f)) / 2.0f);
                    num4 = Mathf.Sin((Mathf.PI * Mathf.Pow(num2, 0.5f)) / 2.0f);
                    rho = t * 180.0f;
                    theta = funcLinear(num3, 90.0f * Mathf.Deg2Rad, 8.0f * Mathf.Deg2Rad);
                    apex.z = funcLinear(num4, ap1, ap2);    //-15.0f, -2.5f);
                }
                else
                {
                    if (t <= 0.4f)
                    {
                        num2 = (t - 0.15f) / 0.25f;
                        rho = t * 180f;
                        theta = funcLinear(num2, 8.0f * Mathf.Deg2Rad, 6.0f * Mathf.Deg2Rad);
                        apex.z = funcLinear(num2, ap2, ap3);    //-2.5f, -3.5f);
                    }
                    else
                    {
                        if (t <= 1.0f)
                        {
                            num2 = (t - 0.4f) / 0.6f;
                            rho = t * 180.0f;
                            num3 = Mathf.Sin((Mathf.PI * Mathf.Pow(num2, 10.0f)) / 2.0f);
                            num4 = Mathf.Sin((Mathf.PI * Mathf.Pow(num2, 2.0f)) / 2.0f);
                            theta = funcLinear(num3, 6.0f * Mathf.Deg2Rad, 90.0f * Mathf.Deg2Rad);
                            apex.z = funcLinear(num4, ap3, ap1);    //-3.5f, -15.0f);
                        }
                    }
                }
            }
        }
        private float funcLinear(float ft, float f0, float f1)
        {
            return (f0 + ((f1 - f0) * ft));
        }

        #endregion

        private enum ModChannel
        {
            None = 0,
            Verts = 1,
            UV = 2,
            UV1 = 4,
            UV2 = 8,
            Normals = 16,
            Tris = 32,
            Col = 64,
            Selection = 128,
            All = 32767,
        }

        private void MakeQuad1(List<int> f, int a, int b, int c, int d)
        {
            f.Add(a);
            f.Add(b);
            f.Add(c);
            f.Add(c);
            f.Add(d);
            f.Add(a);
        }

        private Mesh Build(bool leftCenter)
        {
            float Width = 1.0f;
            float Length = 1.0f;
            int WidthSegs = 10;
            int LengthSegs = 10;
            bool genUVs = true;
            float rotate = 90.0f;

            Width = Mathf.Clamp(Width, 0.0f, float.MaxValue);
            Length = Mathf.Clamp(Length, 0.0f, float.MaxValue);

            LengthSegs = Mathf.Clamp(LengthSegs, 1, 200);
            WidthSegs = Mathf.Clamp(WidthSegs, 1, 200);

            Vector3 vb = new Vector3(Width, 0, Length) / 2.0f;
            Vector3 va = -vb;

            if (leftCenter)
            {
                va.y = 0.0f;
                vb.y = 0;
                va.x = 0.0f;
                vb.x = Width;
            }
            else
            {
                va.y = 0f;
                vb.y = 0;
                va.x = Width * -0.5f;
                vb.x = Width * -0.5f;
            }

            float dx = Width / (float)WidthSegs;
            float dz = Length / (float)LengthSegs;

            Vector3 p = va;

            List<Vector3> verts = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> tris = new List<int>();
            List<int> tris1 = new List<int>();

            Vector2 uv = Vector2.zero;

            #region top bottom

            if (Width > 0.0f && Length > 0.0f)
            {
                Matrix4x4 tm1 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, rotate, 0.0f), Vector3.one);

                Vector3 uv1 = Vector3.zero;

                p.y = vb.y;
                for (int iz = 0; iz <= LengthSegs; iz++)
                {
                    p.x = va.x;
                    for (int ix = 0; ix <= WidthSegs; ix++)
                    {
                        verts.Add(p);

                        if (genUVs)
                        {
                            uv.x = (p.x - va.x) / Width;
                            uv.y = (p.z + vb.z) / Length;

                            uv1.x = uv.x - 0.5f;
                            uv1.y = 0.0f;
                            uv1.z = uv.y - 0.5f;

                            uv1 = tm1.MultiplyPoint3x4(uv1);
                            uv.x = 0.5f + uv1.x;
                            uv.y = 0.5f + uv1.z;

                            uvs.Add(uv);
                        }
                        p.x += dx;
                    }
                    p.z += dz;
                }

                for (int iz = 0; iz < LengthSegs; iz++)
                {
                    int kv = iz * (WidthSegs + 1);
                    for (int ix = 0; ix < WidthSegs; ix++)
                    {
                        MakeQuad1(tris, kv, kv + WidthSegs + 1, kv + WidthSegs + 2, kv + 1);
                        kv++;
                    }
                }

                int index = verts.Count;

                p.y = va.y;
                p.z = va.z;

                for (int iy = 0; iy <= LengthSegs; iy++)
                {
                    p.x = va.x;
                    for (int ix = 0; ix <= WidthSegs; ix++)
                    {
                        verts.Add(p);
                        if (genUVs)
                        {
                            uv.x = 1.0f - ((p.x + vb.x) / Width);
                            uv.y = ((p.z + vb.z) / Length);

                            uv1.x = uv.x - 0.5f;
                            uv1.y = 0.0f;
                            uv1.z = uv.y - 0.5f;

                            uv1 = tm1.MultiplyPoint3x4(uv1);
                            uv.x = 0.5f + uv1.x;
                            uv.y = 0.5f + uv1.z;

                            uvs.Add(uv);
                        }
                        p.x += dx;
                    }
                    p.z += dz;
                }

                for (int iy = 0; iy < LengthSegs; iy++)
                {
                    int kv = iy * (WidthSegs + 1) + index;
                    for (int ix = 0; ix < WidthSegs; ix++)
                    {
                        MakeQuad1(tris1, kv, kv + 1, kv + WidthSegs + 2, kv + WidthSegs + 1);
                        kv++;
                    }
                }
            }
            #endregion

            Mesh mesh = new Mesh();
            mesh.subMeshCount = 2;
            mesh.vertices = verts.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.SetTriangles(tris.ToArray(), 0);
            mesh.SetTriangles(tris1.ToArray(), 1);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}