using System;
using UnityEngine;

namespace Core.ShaderAssist
{
    internal class Gray_Com : IDisposable
    {
        internal Effect_Com<Renderer> effect;

        public Gray_Com(Renderer com)
        {
            effect = new Effect_Com<Renderer>(com, ShaderConfig.Gray);
        }

        public void Dispose()
        {
            effect.Dispose();
        }
    }
}
