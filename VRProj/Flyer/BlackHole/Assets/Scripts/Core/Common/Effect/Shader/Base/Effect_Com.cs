using UnityEngine;

namespace Core.ShaderAssist
{
    /// <summary>
    /// 用于非 UI 组件
    /// </summary>
   internal class Effect_Com<T> where T:Component
    {
        private Renderer render;
        private Material mat;
        public Material Mat;
        public Effect_Com(T t, string shaderName)
        {
            render = t.GetComponent<Renderer>();
            Mat = render.material;
            mat = new Material(Shader.Find(shaderName));
            mat.mainTexture = Mat.mainTexture;
            render.material = mat;
        }

        internal void SetFloat(string key, float value)
        {
            mat.SetFloat(key, value);
            if (render != null)
                render.material = mat;
        }
        internal void SetInt(string key, int value)
        {
            mat.SetInt(key, value);
            if (render != null)
                render.material = mat;
        }

        internal void Dispose()
        {
            if (render != null)
                render.material = Mat;
        }
    }
}
