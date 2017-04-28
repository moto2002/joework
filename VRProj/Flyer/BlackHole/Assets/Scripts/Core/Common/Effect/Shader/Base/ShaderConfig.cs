using System;

/// <summary>
/// 高斯模糊
/// </summary>
public interface IGaussianBlur : IDisposable
{
    /// <summary>
    /// 模糊强度 (范围： 0 - 10)
    /// </summary>
    IGaussianBlur SetRange(float value);
    /// <summary>
    /// 亮度 (范围： 0 - 1)
    /// </summary>
    IGaussianBlur SetLight(float value);
    /// <summary>
    ///  透明度 (范围： 0 - 1)
    /// </summary>
    IGaussianBlur SetAlpha(float value);
}

namespace Core.ShaderAssist
{
    internal class ShaderConfig
    {
        public const string GaussianBlur = "Custom/GaussianBlur";
        public const string Gray = "Custom/ImageGray";
    }
}