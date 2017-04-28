using System;
using UnityEngine.UI;

namespace Core.ShaderAssist
{
    internal class GaussianBlur_UI : IGaussianBlur
    {
        private Effect_UI<Image> effect;
        public GaussianBlur_UI(Image image)
        {
            effect = new Effect_UI<Image>(image, ShaderConfig.GaussianBlur);
        }

        public IGaussianBlur SetRange(float value)
        {
            effect.SetFloat("radius", value);
            return this;
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

        public void Dispose()
        {
            effect.Dispose();
        }
    }
}
