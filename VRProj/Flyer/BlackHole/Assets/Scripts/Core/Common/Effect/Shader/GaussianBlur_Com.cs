using UnityEngine;

namespace Core.ShaderAssist
{
    internal class GaussianBlur_Com : IGaussianBlur
    {
        public Effect_Com<Renderer> effect;

        public GaussianBlur_Com(Renderer com)
        {
            effect = new Effect_Com<Renderer>(com, ShaderConfig.GaussianBlur);
        }

        public void Dispose()
        {
            effect.Dispose();
        }

        public IGaussianBlur SetAlpha(float value)
        {
            effect.SetFloat("alpha", value);
            return this;
        }

        public IGaussianBlur SetLight(float value)
        {
            effect.SetFloat("light", value);
            return this;
        }

        public IGaussianBlur SetRange(float value)
        {
            effect.SetFloat("radius", value);
            return this;
        }
    }
}
