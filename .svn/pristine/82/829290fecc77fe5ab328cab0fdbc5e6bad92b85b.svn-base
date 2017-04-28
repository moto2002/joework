using UnityEngine;
using UnityEngine.UI;

namespace Core.ShaderAssist
{
    /// <summary>
    /// 用于 UI 组件
    /// </summary>
    internal class Effect_UI<T> where T : Graphic
    {
        private Graphic render;
        protected Material mat;
        public Effect_UI(T t, string shaderName)
        {
            render = t;
            mat = new Material(Shader.Find(shaderName));
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

        public void Dispose()
        {
            render.material = null;
        }
    }
}
