using System;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class UIPara : MonoBehaviour
{
    private float accum = 0f;
    private int frames = 0;
    public bool isFpsShown = true;
    public bool isMeshesShown = false;
    public bool isPaticleCountShown = false;
    public bool isTrianglesShown = false;
    public bool isVerticesShown = false;
    private float timeleft;
    public float updateInterval = 0.5f;

    private void HandleFPS(ref float fps)
    {
        fps = this.accum / ((float) this.frames);
    }

    private void HandleMeshFilterInfo(GameObject go, ref int vertices, ref int meshTriangles, ref int meshCount)
    {
        Component[] componentsInChildren = go.GetComponentsInChildren(typeof(MeshFilter));
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            MeshFilter filter = componentsInChildren[i];
            vertices += filter.get_sharedMesh().get_vertexCount();
            meshTriangles += filter.get_sharedMesh().get_triangles().Length / 3;
            meshCount++;
        }
    }

    private void HandleParticleCount(Object[] objs, ref int particleCount)
    {
        particleCount = 0;
        Object[] objArray = objs;
        for (int i = 0; i < objArray.Length; i++)
        {
            GameObject obj2 = objArray[i];
            if (obj2.GetComponent<ParticleEmitter>() != null)
            {
                particleCount += obj2.GetComponent<ParticleEmitter>().get_particleCount();
            }
        }
    }

    private void HandleSkinnedMeshRendererInfo(GameObject go, ref int vertices, ref int skinnedMeshTriangles, ref int meshCount)
    {
        Component[] componentsInChildren = go.GetComponentsInChildren(typeof(SkinnedMeshRenderer));
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            SkinnedMeshRenderer renderer = componentsInChildren[i];
            vertices += renderer.get_sharedMesh().get_vertexCount();
            skinnedMeshTriangles += renderer.get_sharedMesh().get_triangles().Length / 3;
            meshCount++;
        }
    }

    private void HandleVerticesInfo(Object[] objs, ref int vertices, ref int meshTriangles, ref int skinnedMeshTriangles, ref int meshCount)
    {
        vertices = 0;
        skinnedMeshTriangles = 0;
        meshTriangles = 0;
        meshCount = 0;
        Object[] objArray = objs;
        for (int i = 0; i < objArray.Length; i++)
        {
            GameObject go = objArray[i];
            this.HandleMeshFilterInfo(go, ref vertices, ref meshTriangles, ref meshCount);
            this.HandleSkinnedMeshRendererInfo(go, ref vertices, ref skinnedMeshTriangles, ref meshCount);
        }
    }

    private void Start()
    {
        Object.DontDestroyOnLoad(this);
        this.timeleft = this.updateInterval;
    }

    private void Update()
    {
        this.timeleft -= Time.get_deltaTime();
        this.accum += Time.get_timeScale() / Time.get_deltaTime();
        this.frames++;
        try
        {
            float fps = 0f;
            int particleCount = 0;
            int vertices = 0;
            int meshTriangles = 0;
            int skinnedMeshTriangles = 0;
            int meshCount = 0;
            if (this.timeleft <= 0.0)
            {
                Object[] objs = Object.FindObjectsOfType(typeof(GameObject));
                if (this.isFpsShown)
                {
                    this.HandleFPS(ref fps);
                }
                if (this.isPaticleCountShown)
                {
                    this.HandleParticleCount(objs, ref particleCount);
                }
                if ((this.isVerticesShown || this.isTrianglesShown) || this.isMeshesShown)
                {
                    this.HandleVerticesInfo(objs, ref vertices, ref meshTriangles, ref skinnedMeshTriangles, ref meshCount);
                }
                this.UpdateMessage(fps, particleCount, vertices, meshTriangles, skinnedMeshTriangles, meshCount);
                this.timeleft = this.updateInterval;
                this.accum = 0f;
                this.frames = 0;
            }
        }
        catch (Exception exception)
        {
            MonoBehaviour.print(exception.Message);
        }
    }

    private void UpdateMessage(float fps, int particleCount, int vertices, int meshTriangles, int skinnedMeshTriangles, int meshCount)
    {
        StringBuilder builder = new StringBuilder();
        if (this.isFpsShown)
        {
            builder.AppendFormat("FPS: {0}", fps.ToString("f2"));
        }
        builder.Append("\n");
        if (this.isPaticleCountShown)
        {
            builder.AppendFormat("Paticle count: {0}", particleCount);
        }
        builder.Append("\n");
        if (this.isVerticesShown)
        {
            builder.AppendFormat("vertices: {0}", vertices);
        }
        builder.Append("\n");
        if (this.isTrianglesShown)
        {
            builder.AppendFormat("triangles: {0}(m: {1}; s: {2})", meshTriangles + skinnedMeshTriangles, meshTriangles, skinnedMeshTriangles);
        }
        builder.Append("\n");
        if (this.isMeshesShown)
        {
            builder.AppendFormat("meshes: {0}", meshCount);
        }
        builder.Append("\n");
        base.GetComponent<GUIText>().set_text(builder.ToString());
    }
}

