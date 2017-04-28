Shader "TMPro/Mobile/Distance Field Overlay" {
Properties {
 _FaceColor ("Face Color", Color) = (1,1,1,1)
 _FaceDilate ("Face Dilate", Range(-1,1)) = 0
 _OutlineColor ("Outline Color", Color) = (0,0,0,1)
 _OutlineWidth ("Outline Thickness", Range(0,1)) = 0
 _OutlineSoftness ("Outline Softness", Range(0,1)) = 0
 _UnderlayColor ("Border Color", Color) = (0,0,0,0.5)
 _UnderlayOffsetX ("Border OffsetX", Range(-1,1)) = 0
 _UnderlayOffsetY ("Border OffsetY", Range(-1,1)) = 0
 _UnderlayDilate ("Border Dilate", Range(-1,1)) = 0
 _UnderlaySoftness ("Border Softness", Range(0,1)) = 0
 _WeightNormal ("Weight Normal", Float) = 0
 _WeightBold ("Weight Bold", Float) = 0.5
 _ShaderFlags ("Flags", Float) = 0
 _ScaleRatioA ("Scale RatioA", Float) = 1
 _ScaleRatioB ("Scale RatioB", Float) = 1
 _ScaleRatioC ("Scale RatioC", Float) = 1
 _MainTex ("Font Atlas", 2D) = "white" {}
 _TextureWidth ("Texture Width", Float) = 512
 _TextureHeight ("Texture Height", Float) = 512
 _GradientScale ("Gradient Scale", Float) = 5
 _ScaleX ("Scale X", Float) = 1
 _ScaleY ("Scale Y", Float) = 1
 _PerspectiveFilter ("Perspective Correction", Range(0,1)) = 0.875
 _VertexOffsetX ("Vertex OffsetX", Float) = 0
 _VertexOffsetY ("Vertex OffsetY", Float) = 0
 _MaskID ("Mask ID", Float) = 0
 _MaskCoord ("Mask Coords", Vector) = (0,0,100000,100000)
 _MaskSoftnessX ("Mask SoftnessX", Float) = 0
 _MaskSoftnessY ("Mask SoftnessY", Float) = 0
}
SubShader { 
 Tags { "QUEUE"="Overlay" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Overlay" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  ZWrite Off
  Cull [_CullMode]
  Fog { Mode Off }
  Blend One OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  lowp vec4 outlineColor_4;
  lowp vec4 faceColor_5;
  highp float opacity_6;
  highp float scale_7;
  highp vec4 vert_8;
  highp float tmpvar_9;
  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_8.zw = _glesVertex.zw;
  vert_8.x = (_glesVertex.x + _VertexOffsetX);
  vert_8.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_10;
  tmpvar_10 = (glstate_matrix_mvp * vert_8);
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_10.ww / (tmpvar_11 * abs(
    (tmpvar_12 * _ScreenParams.xy)
  )));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_7 = tmpvar_14;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_15;
    tmpvar_15.w = 1.0;
    tmpvar_15.xyz = _WorldSpaceCameraPos;
    scale_7 = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - vert_8.xyz))
    )));
  };
  highp float tmpvar_16;
  tmpvar_16 = (scale_7 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_7)));
  scale_7 = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = (((0.5 - 
    ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5))
  ) * tmpvar_16) - 0.5);
  highp float tmpvar_18;
  tmpvar_18 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_16);
  lowp float tmpvar_19;
  tmpvar_19 = tmpvar_2.w;
  opacity_6 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = tmpvar_2.xyz;
  tmpvar_20.w = opacity_6;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * _FaceColor);
  faceColor_5 = tmpvar_21;
  outlineColor_4.xyz = _OutlineColor.xyz;
  faceColor_5.xyz = (faceColor_5.xyz * faceColor_5.w);
  highp float tmpvar_22;
  tmpvar_22 = (_OutlineColor.w * opacity_6);
  outlineColor_4.w = tmpvar_22;
  outlineColor_4.xyz = (_OutlineColor.xyz * outlineColor_4.w);
  highp vec4 tmpvar_23;
  tmpvar_23 = mix (faceColor_5, outlineColor_4, vec4(sqrt(min (1.0, 
    (tmpvar_18 * 2.0)
  ))));
  outlineColor_4 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.x = tmpvar_16;
  tmpvar_24.y = (tmpvar_17 - tmpvar_18);
  tmpvar_24.z = (tmpvar_17 + tmpvar_18);
  tmpvar_24.w = tmpvar_17;
  highp vec4 tmpvar_25;
  tmpvar_25.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_25.zw = (0.5 / tmpvar_13);
  mediump vec4 tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_26 = tmpvar_24;
  tmpvar_27 = tmpvar_25;
  gl_Position = tmpvar_10;
  xlv_COLOR = faceColor_5;
  xlv_COLOR1 = outlineColor_4;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_26;
  xlv_TEXCOORD2 = tmpvar_27;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_2;
  tmpvar_2 = (tmpvar_1.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_3;
  tmpvar_3 = clamp ((tmpvar_2 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_2 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_3)) * tmpvar_4);
  gl_FragData[0] = tmpvar_5;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  lowp vec4 outlineColor_4;
  lowp vec4 faceColor_5;
  highp float opacity_6;
  highp float scale_7;
  highp vec4 vert_8;
  highp float tmpvar_9;
  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_8.zw = _glesVertex.zw;
  vert_8.x = (_glesVertex.x + _VertexOffsetX);
  vert_8.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_10;
  tmpvar_10 = (glstate_matrix_mvp * vert_8);
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_10.ww / (tmpvar_11 * abs(
    (tmpvar_12 * _ScreenParams.xy)
  )));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_7 = tmpvar_14;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_15;
    tmpvar_15.w = 1.0;
    tmpvar_15.xyz = _WorldSpaceCameraPos;
    scale_7 = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - vert_8.xyz))
    )));
  };
  highp float tmpvar_16;
  tmpvar_16 = (scale_7 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_7)));
  scale_7 = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = (((0.5 - 
    ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5))
  ) * tmpvar_16) - 0.5);
  highp float tmpvar_18;
  tmpvar_18 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_16);
  lowp float tmpvar_19;
  tmpvar_19 = tmpvar_2.w;
  opacity_6 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = tmpvar_2.xyz;
  tmpvar_20.w = opacity_6;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * _FaceColor);
  faceColor_5 = tmpvar_21;
  outlineColor_4.xyz = _OutlineColor.xyz;
  faceColor_5.xyz = (faceColor_5.xyz * faceColor_5.w);
  highp float tmpvar_22;
  tmpvar_22 = (_OutlineColor.w * opacity_6);
  outlineColor_4.w = tmpvar_22;
  outlineColor_4.xyz = (_OutlineColor.xyz * outlineColor_4.w);
  highp vec4 tmpvar_23;
  tmpvar_23 = mix (faceColor_5, outlineColor_4, vec4(sqrt(min (1.0, 
    (tmpvar_18 * 2.0)
  ))));
  outlineColor_4 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.x = tmpvar_16;
  tmpvar_24.y = (tmpvar_17 - tmpvar_18);
  tmpvar_24.z = (tmpvar_17 + tmpvar_18);
  tmpvar_24.w = tmpvar_17;
  highp vec4 tmpvar_25;
  tmpvar_25.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_25.zw = (0.5 / tmpvar_13);
  mediump vec4 tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_26 = tmpvar_24;
  tmpvar_27 = tmpvar_25;
  gl_Position = tmpvar_10;
  xlv_COLOR = faceColor_5;
  xlv_COLOR1 = outlineColor_4;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_26;
  xlv_TEXCOORD2 = tmpvar_27;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_2;
  tmpvar_2 = (tmpvar_1.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_3;
  tmpvar_3 = clamp ((tmpvar_2 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_2 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_3)) * tmpvar_4);
  _glesFragData[0] = tmpvar_5;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  lowp vec4 outlineColor_4;
  lowp vec4 faceColor_5;
  highp float opacity_6;
  highp float scale_7;
  highp vec4 vert_8;
  highp float tmpvar_9;
  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_8.zw = _glesVertex.zw;
  vert_8.x = (_glesVertex.x + _VertexOffsetX);
  vert_8.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_10;
  tmpvar_10 = (glstate_matrix_mvp * vert_8);
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_10.ww / (tmpvar_11 * abs(
    (tmpvar_12 * _ScreenParams.xy)
  )));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_7 = tmpvar_14;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_15;
    tmpvar_15.w = 1.0;
    tmpvar_15.xyz = _WorldSpaceCameraPos;
    scale_7 = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - vert_8.xyz))
    )));
  };
  highp float tmpvar_16;
  tmpvar_16 = (scale_7 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_7)));
  scale_7 = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = (((0.5 - 
    ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5))
  ) * tmpvar_16) - 0.5);
  highp float tmpvar_18;
  tmpvar_18 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_16);
  lowp float tmpvar_19;
  tmpvar_19 = tmpvar_2.w;
  opacity_6 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = tmpvar_2.xyz;
  tmpvar_20.w = opacity_6;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * _FaceColor);
  faceColor_5 = tmpvar_21;
  outlineColor_4.xyz = _OutlineColor.xyz;
  faceColor_5.xyz = (faceColor_5.xyz * faceColor_5.w);
  highp float tmpvar_22;
  tmpvar_22 = (_OutlineColor.w * opacity_6);
  outlineColor_4.w = tmpvar_22;
  outlineColor_4.xyz = (_OutlineColor.xyz * outlineColor_4.w);
  highp vec4 tmpvar_23;
  tmpvar_23 = mix (faceColor_5, outlineColor_4, vec4(sqrt(min (1.0, 
    (tmpvar_18 * 2.0)
  ))));
  outlineColor_4 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.x = tmpvar_16;
  tmpvar_24.y = (tmpvar_17 - tmpvar_18);
  tmpvar_24.z = (tmpvar_17 + tmpvar_18);
  tmpvar_24.w = tmpvar_17;
  highp vec4 tmpvar_25;
  tmpvar_25.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_25.zw = (0.5 / tmpvar_13);
  mediump vec4 tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_26 = tmpvar_24;
  tmpvar_27 = tmpvar_25;
  gl_Position = tmpvar_10;
  xlv_COLOR = faceColor_5;
  xlv_COLOR1 = outlineColor_4;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_26;
  xlv_TEXCOORD2 = tmpvar_27;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_3 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_3 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_4)) * tmpvar_5);
  mediump vec2 tmpvar_7;
  tmpvar_7 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_8;
  tmpvar_8 = clamp (((tmpvar_7 - _MaskCoord.zw) * xlv_TEXCOORD2.zw), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (1.0 - tmpvar_8);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_6 * (tmpvar_9.x * tmpvar_9.y));
  c_1 = tmpvar_10;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  lowp vec4 outlineColor_4;
  lowp vec4 faceColor_5;
  highp float opacity_6;
  highp float scale_7;
  highp vec4 vert_8;
  highp float tmpvar_9;
  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_8.zw = _glesVertex.zw;
  vert_8.x = (_glesVertex.x + _VertexOffsetX);
  vert_8.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_10;
  tmpvar_10 = (glstate_matrix_mvp * vert_8);
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_10.ww / (tmpvar_11 * abs(
    (tmpvar_12 * _ScreenParams.xy)
  )));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_7 = tmpvar_14;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_15;
    tmpvar_15.w = 1.0;
    tmpvar_15.xyz = _WorldSpaceCameraPos;
    scale_7 = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - vert_8.xyz))
    )));
  };
  highp float tmpvar_16;
  tmpvar_16 = (scale_7 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_7)));
  scale_7 = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = (((0.5 - 
    ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5))
  ) * tmpvar_16) - 0.5);
  highp float tmpvar_18;
  tmpvar_18 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_16);
  lowp float tmpvar_19;
  tmpvar_19 = tmpvar_2.w;
  opacity_6 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = tmpvar_2.xyz;
  tmpvar_20.w = opacity_6;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * _FaceColor);
  faceColor_5 = tmpvar_21;
  outlineColor_4.xyz = _OutlineColor.xyz;
  faceColor_5.xyz = (faceColor_5.xyz * faceColor_5.w);
  highp float tmpvar_22;
  tmpvar_22 = (_OutlineColor.w * opacity_6);
  outlineColor_4.w = tmpvar_22;
  outlineColor_4.xyz = (_OutlineColor.xyz * outlineColor_4.w);
  highp vec4 tmpvar_23;
  tmpvar_23 = mix (faceColor_5, outlineColor_4, vec4(sqrt(min (1.0, 
    (tmpvar_18 * 2.0)
  ))));
  outlineColor_4 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.x = tmpvar_16;
  tmpvar_24.y = (tmpvar_17 - tmpvar_18);
  tmpvar_24.z = (tmpvar_17 + tmpvar_18);
  tmpvar_24.w = tmpvar_17;
  highp vec4 tmpvar_25;
  tmpvar_25.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_25.zw = (0.5 / tmpvar_13);
  mediump vec4 tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_26 = tmpvar_24;
  tmpvar_27 = tmpvar_25;
  gl_Position = tmpvar_10;
  xlv_COLOR = faceColor_5;
  xlv_COLOR1 = outlineColor_4;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_26;
  xlv_TEXCOORD2 = tmpvar_27;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
in mediump vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_3 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_3 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_4)) * tmpvar_5);
  mediump vec2 tmpvar_7;
  tmpvar_7 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_8;
  tmpvar_8 = clamp (((tmpvar_7 - _MaskCoord.zw) * xlv_TEXCOORD2.zw), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (1.0 - tmpvar_8);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_6 * (tmpvar_9.x * tmpvar_9.y));
  c_1 = tmpvar_10;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  lowp vec4 outlineColor_4;
  lowp vec4 faceColor_5;
  highp float opacity_6;
  highp float scale_7;
  highp vec4 vert_8;
  highp float tmpvar_9;
  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_8.zw = _glesVertex.zw;
  vert_8.x = (_glesVertex.x + _VertexOffsetX);
  vert_8.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_10;
  tmpvar_10 = (glstate_matrix_mvp * vert_8);
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_10.ww / (tmpvar_11 * abs(
    (tmpvar_12 * _ScreenParams.xy)
  )));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_7 = tmpvar_14;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_15;
    tmpvar_15.w = 1.0;
    tmpvar_15.xyz = _WorldSpaceCameraPos;
    scale_7 = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - vert_8.xyz))
    )));
  };
  highp float tmpvar_16;
  tmpvar_16 = (scale_7 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_7)));
  scale_7 = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = (((0.5 - 
    ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5))
  ) * tmpvar_16) - 0.5);
  highp float tmpvar_18;
  tmpvar_18 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_16);
  lowp float tmpvar_19;
  tmpvar_19 = tmpvar_2.w;
  opacity_6 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = tmpvar_2.xyz;
  tmpvar_20.w = opacity_6;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * _FaceColor);
  faceColor_5 = tmpvar_21;
  outlineColor_4.xyz = _OutlineColor.xyz;
  faceColor_5.xyz = (faceColor_5.xyz * faceColor_5.w);
  highp float tmpvar_22;
  tmpvar_22 = (_OutlineColor.w * opacity_6);
  outlineColor_4.w = tmpvar_22;
  outlineColor_4.xyz = (_OutlineColor.xyz * outlineColor_4.w);
  highp vec4 tmpvar_23;
  tmpvar_23 = mix (faceColor_5, outlineColor_4, vec4(sqrt(min (1.0, 
    (tmpvar_18 * 2.0)
  ))));
  outlineColor_4 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.x = tmpvar_16;
  tmpvar_24.y = (tmpvar_17 - tmpvar_18);
  tmpvar_24.z = (tmpvar_17 + tmpvar_18);
  tmpvar_24.w = tmpvar_17;
  highp vec4 tmpvar_25;
  tmpvar_25.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_25.zw = (0.5 / tmpvar_13);
  mediump vec4 tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_26 = tmpvar_24;
  tmpvar_27 = tmpvar_25;
  gl_Position = tmpvar_10;
  xlv_COLOR = faceColor_5;
  xlv_COLOR1 = outlineColor_4;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_26;
  xlv_TEXCOORD2 = tmpvar_27;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  mediump vec2 s_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_4;
  tmpvar_4 = (tmpvar_3.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_4 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_6;
  tmpvar_6 = clamp ((tmpvar_4 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_5)) * tmpvar_6);
  highp vec2 tmpvar_8;
  tmpvar_8.x = _MaskSoftnessX;
  tmpvar_8.y = _MaskSoftnessY;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * xlv_TEXCOORD2.zw);
  s_1 = tmpvar_9;
  mediump vec2 tmpvar_10;
  tmpvar_10 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_11;
  tmpvar_11 = clamp (((
    ((tmpvar_10 - _MaskCoord.zw) * xlv_TEXCOORD2.zw)
   + s_1) / (1.0 + s_1)), 0.0, 1.0);
  mediump vec2 tmpvar_12;
  tmpvar_12 = (1.0 - tmpvar_11);
  mediump vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_12 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * (tmpvar_13.x * tmpvar_13.y));
  c_2 = tmpvar_14;
  gl_FragData[0] = c_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  lowp vec4 outlineColor_4;
  lowp vec4 faceColor_5;
  highp float opacity_6;
  highp float scale_7;
  highp vec4 vert_8;
  highp float tmpvar_9;
  tmpvar_9 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_8.zw = _glesVertex.zw;
  vert_8.x = (_glesVertex.x + _VertexOffsetX);
  vert_8.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_10;
  tmpvar_10 = (glstate_matrix_mvp * vert_8);
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_10.ww / (tmpvar_11 * abs(
    (tmpvar_12 * _ScreenParams.xy)
  )));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_7 = tmpvar_14;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_15;
    tmpvar_15.w = 1.0;
    tmpvar_15.xyz = _WorldSpaceCameraPos;
    scale_7 = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - vert_8.xyz))
    )));
  };
  highp float tmpvar_16;
  tmpvar_16 = (scale_7 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_7)));
  scale_7 = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = (((0.5 - 
    ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5))
  ) * tmpvar_16) - 0.5);
  highp float tmpvar_18;
  tmpvar_18 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_16);
  lowp float tmpvar_19;
  tmpvar_19 = tmpvar_2.w;
  opacity_6 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = tmpvar_2.xyz;
  tmpvar_20.w = opacity_6;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * _FaceColor);
  faceColor_5 = tmpvar_21;
  outlineColor_4.xyz = _OutlineColor.xyz;
  faceColor_5.xyz = (faceColor_5.xyz * faceColor_5.w);
  highp float tmpvar_22;
  tmpvar_22 = (_OutlineColor.w * opacity_6);
  outlineColor_4.w = tmpvar_22;
  outlineColor_4.xyz = (_OutlineColor.xyz * outlineColor_4.w);
  highp vec4 tmpvar_23;
  tmpvar_23 = mix (faceColor_5, outlineColor_4, vec4(sqrt(min (1.0, 
    (tmpvar_18 * 2.0)
  ))));
  outlineColor_4 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.x = tmpvar_16;
  tmpvar_24.y = (tmpvar_17 - tmpvar_18);
  tmpvar_24.z = (tmpvar_17 + tmpvar_18);
  tmpvar_24.w = tmpvar_17;
  highp vec4 tmpvar_25;
  tmpvar_25.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_25.zw = (0.5 / tmpvar_13);
  mediump vec4 tmpvar_26;
  mediump vec4 tmpvar_27;
  tmpvar_26 = tmpvar_24;
  tmpvar_27 = tmpvar_25;
  gl_Position = tmpvar_10;
  xlv_COLOR = faceColor_5;
  xlv_COLOR1 = outlineColor_4;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_26;
  xlv_TEXCOORD2 = tmpvar_27;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
in mediump vec4 xlv_TEXCOORD2;
void main ()
{
  mediump vec2 s_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_4;
  tmpvar_4 = (tmpvar_3.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_4 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_6;
  tmpvar_6 = clamp ((tmpvar_4 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_5)) * tmpvar_6);
  highp vec2 tmpvar_8;
  tmpvar_8.x = _MaskSoftnessX;
  tmpvar_8.y = _MaskSoftnessY;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * xlv_TEXCOORD2.zw);
  s_1 = tmpvar_9;
  mediump vec2 tmpvar_10;
  tmpvar_10 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_11;
  tmpvar_11 = clamp (((
    ((tmpvar_10 - _MaskCoord.zw) * xlv_TEXCOORD2.zw)
   + s_1) / (1.0 + s_1)), 0.0, 1.0);
  mediump vec2 tmpvar_12;
  tmpvar_12 = (1.0 - tmpvar_11);
  mediump vec2 tmpvar_13;
  tmpvar_13 = (tmpvar_12 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * (tmpvar_13.x * tmpvar_13.y));
  c_2 = tmpvar_14;
  _glesFragData[0] = c_2;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_2;
  tmpvar_2 = (tmpvar_1.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_3;
  tmpvar_3 = clamp ((tmpvar_2 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_2 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_3)) * tmpvar_4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_7;
  tmpvar_7 = clamp (((tmpvar_6.w * xlv_TEXCOORD5.x) - xlv_TEXCOORD5.y), 0.0, 1.0);
  lowp vec4 tmpvar_8;
  tmpvar_8 = (tmpvar_5 + (xlv_TEXCOORD4 * (tmpvar_7 * 
    (1.0 - tmpvar_5.w)
  )));
  gl_FragData[0] = tmpvar_8;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out lowp vec4 xlv_TEXCOORD4;
out mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD3;
in lowp vec4 xlv_TEXCOORD4;
in mediump vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_2;
  tmpvar_2 = (tmpvar_1.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_3;
  tmpvar_3 = clamp ((tmpvar_2 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_2 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_3)) * tmpvar_4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_7;
  tmpvar_7 = clamp (((tmpvar_6.w * xlv_TEXCOORD5.x) - xlv_TEXCOORD5.y), 0.0, 1.0);
  lowp vec4 tmpvar_8;
  tmpvar_8 = (tmpvar_5 + (xlv_TEXCOORD4 * (tmpvar_7 * 
    (1.0 - tmpvar_5.w)
  )));
  _glesFragData[0] = tmpvar_8;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_3 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_3 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_4)) * tmpvar_5);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_8;
  tmpvar_8 = clamp (((tmpvar_7.w * xlv_TEXCOORD5.x) - xlv_TEXCOORD5.y), 0.0, 1.0);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_6 + (xlv_TEXCOORD4 * (tmpvar_8 * 
    (1.0 - tmpvar_6.w)
  )));
  mediump vec2 tmpvar_10;
  tmpvar_10 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_11;
  tmpvar_11 = clamp (((tmpvar_10 - _MaskCoord.zw) * xlv_TEXCOORD2.zw), 0.0, 1.0);
  mediump vec2 tmpvar_12;
  tmpvar_12 = (1.0 - tmpvar_11);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_9 * (tmpvar_12.x * tmpvar_12.y));
  c_1 = tmpvar_13;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out lowp vec4 xlv_TEXCOORD4;
out mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
in mediump vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
in lowp vec4 xlv_TEXCOORD4;
in mediump vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_3 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_3 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_4)) * tmpvar_5);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_8;
  tmpvar_8 = clamp (((tmpvar_7.w * xlv_TEXCOORD5.x) - xlv_TEXCOORD5.y), 0.0, 1.0);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_6 + (xlv_TEXCOORD4 * (tmpvar_8 * 
    (1.0 - tmpvar_6.w)
  )));
  mediump vec2 tmpvar_10;
  tmpvar_10 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_11;
  tmpvar_11 = clamp (((tmpvar_10 - _MaskCoord.zw) * xlv_TEXCOORD2.zw), 0.0, 1.0);
  mediump vec2 tmpvar_12;
  tmpvar_12 = (1.0 - tmpvar_11);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_9 * (tmpvar_12.x * tmpvar_12.y));
  c_1 = tmpvar_13;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec2 s_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_4;
  tmpvar_4 = (tmpvar_3.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_4 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_6;
  tmpvar_6 = clamp ((tmpvar_4 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_5)) * tmpvar_6);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_9;
  tmpvar_9 = clamp (((tmpvar_8.w * xlv_TEXCOORD5.x) - xlv_TEXCOORD5.y), 0.0, 1.0);
  lowp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_7 + (xlv_TEXCOORD4 * (tmpvar_9 * 
    (1.0 - tmpvar_7.w)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _MaskSoftnessX;
  tmpvar_11.y = _MaskSoftnessY;
  highp vec2 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * xlv_TEXCOORD2.zw);
  s_1 = tmpvar_12;
  mediump vec2 tmpvar_13;
  tmpvar_13 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_14;
  tmpvar_14 = clamp (((
    ((tmpvar_13 - _MaskCoord.zw) * xlv_TEXCOORD2.zw)
   + s_1) / (1.0 + s_1)), 0.0, 1.0);
  mediump vec2 tmpvar_15;
  tmpvar_15 = (1.0 - tmpvar_14);
  mediump vec2 tmpvar_16;
  tmpvar_16 = (tmpvar_15 * tmpvar_15);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_10 * (tmpvar_16.x * tmpvar_16.y));
  c_2 = tmpvar_17;
  gl_FragData[0] = c_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out lowp vec4 xlv_TEXCOORD4;
out mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
in mediump vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
in lowp vec4 xlv_TEXCOORD4;
in mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec2 s_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_4;
  tmpvar_4 = (tmpvar_3.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_4 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_6;
  tmpvar_6 = clamp ((tmpvar_4 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_5)) * tmpvar_6);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_9;
  tmpvar_9 = clamp (((tmpvar_8.w * xlv_TEXCOORD5.x) - xlv_TEXCOORD5.y), 0.0, 1.0);
  lowp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_7 + (xlv_TEXCOORD4 * (tmpvar_9 * 
    (1.0 - tmpvar_7.w)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _MaskSoftnessX;
  tmpvar_11.y = _MaskSoftnessY;
  highp vec2 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * xlv_TEXCOORD2.zw);
  s_1 = tmpvar_12;
  mediump vec2 tmpvar_13;
  tmpvar_13 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_14;
  tmpvar_14 = clamp (((
    ((tmpvar_13 - _MaskCoord.zw) * xlv_TEXCOORD2.zw)
   + s_1) / (1.0 + s_1)), 0.0, 1.0);
  mediump vec2 tmpvar_15;
  tmpvar_15 = (1.0 - tmpvar_14);
  mediump vec2 tmpvar_16;
  tmpvar_16 = (tmpvar_15 * tmpvar_15);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_10 * (tmpvar_16.x * tmpvar_16.y));
  c_2 = tmpvar_17;
  _glesFragData[0] = c_2;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_2;
  tmpvar_2 = (tmpvar_1.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_3;
  tmpvar_3 = clamp ((tmpvar_2 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_2 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_3)) * tmpvar_4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_7;
  tmpvar_7 = (tmpvar_6.w * xlv_TEXCOORD5.x);
  mediump float tmpvar_8;
  tmpvar_8 = clamp ((tmpvar_7 - xlv_TEXCOORD5.y), 0.0, 1.0);
  mediump float tmpvar_9;
  tmpvar_9 = clamp ((tmpvar_7 - xlv_TEXCOORD1.w), 0.0, 1.0);
  lowp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_5 + ((
    (xlv_TEXCOORD4 * (1.0 - tmpvar_8))
   * tmpvar_9) * (1.0 - tmpvar_5.w)));
  gl_FragData[0] = tmpvar_10;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out lowp vec4 xlv_TEXCOORD4;
out mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD3;
in lowp vec4 xlv_TEXCOORD4;
in mediump vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_2;
  tmpvar_2 = (tmpvar_1.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_3;
  tmpvar_3 = clamp ((tmpvar_2 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_2 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_3)) * tmpvar_4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_7;
  tmpvar_7 = (tmpvar_6.w * xlv_TEXCOORD5.x);
  mediump float tmpvar_8;
  tmpvar_8 = clamp ((tmpvar_7 - xlv_TEXCOORD5.y), 0.0, 1.0);
  mediump float tmpvar_9;
  tmpvar_9 = clamp ((tmpvar_7 - xlv_TEXCOORD1.w), 0.0, 1.0);
  lowp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_5 + ((
    (xlv_TEXCOORD4 * (1.0 - tmpvar_8))
   * tmpvar_9) * (1.0 - tmpvar_5.w)));
  _glesFragData[0] = tmpvar_10;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_3 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_3 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_4)) * tmpvar_5);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_8;
  tmpvar_8 = (tmpvar_7.w * xlv_TEXCOORD5.x);
  mediump float tmpvar_9;
  tmpvar_9 = clamp ((tmpvar_8 - xlv_TEXCOORD5.y), 0.0, 1.0);
  mediump float tmpvar_10;
  tmpvar_10 = clamp ((tmpvar_8 - xlv_TEXCOORD1.w), 0.0, 1.0);
  lowp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_6 + ((
    (xlv_TEXCOORD4 * (1.0 - tmpvar_9))
   * tmpvar_10) * (1.0 - tmpvar_6.w)));
  mediump vec2 tmpvar_12;
  tmpvar_12 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_13;
  tmpvar_13 = clamp (((tmpvar_12 - _MaskCoord.zw) * xlv_TEXCOORD2.zw), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = (1.0 - tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_11 * (tmpvar_14.x * tmpvar_14.y));
  c_1 = tmpvar_15;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out lowp vec4 xlv_TEXCOORD4;
out mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
in mediump vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
in lowp vec4 xlv_TEXCOORD4;
in mediump vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_3;
  tmpvar_3 = (tmpvar_2.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_4;
  tmpvar_4 = clamp ((tmpvar_3 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_3 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_4)) * tmpvar_5);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_8;
  tmpvar_8 = (tmpvar_7.w * xlv_TEXCOORD5.x);
  mediump float tmpvar_9;
  tmpvar_9 = clamp ((tmpvar_8 - xlv_TEXCOORD5.y), 0.0, 1.0);
  mediump float tmpvar_10;
  tmpvar_10 = clamp ((tmpvar_8 - xlv_TEXCOORD1.w), 0.0, 1.0);
  lowp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_6 + ((
    (xlv_TEXCOORD4 * (1.0 - tmpvar_9))
   * tmpvar_10) * (1.0 - tmpvar_6.w)));
  mediump vec2 tmpvar_12;
  tmpvar_12 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_13;
  tmpvar_13 = clamp (((tmpvar_12 - _MaskCoord.zw) * xlv_TEXCOORD2.zw), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = (1.0 - tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_11 * (tmpvar_14.x * tmpvar_14.y));
  c_1 = tmpvar_15;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec2 s_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_4;
  tmpvar_4 = (tmpvar_3.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_4 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_6;
  tmpvar_6 = clamp ((tmpvar_4 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_5)) * tmpvar_6);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_9;
  tmpvar_9 = (tmpvar_8.w * xlv_TEXCOORD5.x);
  mediump float tmpvar_10;
  tmpvar_10 = clamp ((tmpvar_9 - xlv_TEXCOORD5.y), 0.0, 1.0);
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((tmpvar_9 - xlv_TEXCOORD1.w), 0.0, 1.0);
  lowp vec4 tmpvar_12;
  tmpvar_12 = (tmpvar_7 + ((
    (xlv_TEXCOORD4 * (1.0 - tmpvar_10))
   * tmpvar_11) * (1.0 - tmpvar_7.w)));
  highp vec2 tmpvar_13;
  tmpvar_13.x = _MaskSoftnessX;
  tmpvar_13.y = _MaskSoftnessY;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_13 * xlv_TEXCOORD2.zw);
  s_1 = tmpvar_14;
  mediump vec2 tmpvar_15;
  tmpvar_15 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_16;
  tmpvar_16 = clamp (((
    ((tmpvar_15 - _MaskCoord.zw) * xlv_TEXCOORD2.zw)
   + s_1) / (1.0 + s_1)), 0.0, 1.0);
  mediump vec2 tmpvar_17;
  tmpvar_17 = (1.0 - tmpvar_16);
  mediump vec2 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_12 * (tmpvar_18.x * tmpvar_18.y));
  c_2 = tmpvar_19;
  gl_FragData[0] = c_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioC;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out highp vec2 xlv_TEXCOORD0;
out mediump vec4 xlv_TEXCOORD1;
out mediump vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out lowp vec4 xlv_TEXCOORD4;
out mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 layerColor_4;
  lowp vec4 outlineColor_5;
  lowp vec4 faceColor_6;
  highp float opacity_7;
  highp float layerScale_8;
  highp float scale_9;
  highp vec4 vert_10;
  highp float tmpvar_11;
  tmpvar_11 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_10.zw = _glesVertex.zw;
  vert_10.x = (_glesVertex.x + _VertexOffsetX);
  vert_10.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_12;
  tmpvar_12 = (glstate_matrix_mvp * vert_10);
  highp vec2 tmpvar_13;
  tmpvar_13.x = _ScaleX;
  tmpvar_13.y = _ScaleY;
  highp mat2 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_projection[0].xy;
  tmpvar_14[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12.ww / (tmpvar_13 * abs(
    (tmpvar_14 * _ScreenParams.xy)
  )));
  highp float tmpvar_16;
  tmpvar_16 = (inversesqrt(dot (tmpvar_15, tmpvar_15)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  scale_9 = tmpvar_16;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_17;
    tmpvar_17.w = 1.0;
    tmpvar_17.xyz = _WorldSpaceCameraPos;
    scale_9 = mix ((tmpvar_16 * (1.0 - _PerspectiveFilter)), tmpvar_16, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_17).xyz * unity_Scale.w) - vert_10.xyz))
    )));
  };
  highp float tmpvar_18;
  tmpvar_18 = ((mix (_WeightNormal, _WeightBold, tmpvar_11) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  layerScale_8 = scale_9;
  highp float tmpvar_19;
  tmpvar_19 = (scale_9 / (1.0 + (
    (_OutlineSoftness * _ScaleRatioA)
   * scale_9)));
  scale_9 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((0.5 - tmpvar_18) * tmpvar_19) - 0.5);
  highp float tmpvar_21;
  tmpvar_21 = (((_OutlineWidth * _ScaleRatioA) * 0.5) * tmpvar_19);
  lowp float tmpvar_22;
  tmpvar_22 = tmpvar_2.w;
  opacity_7 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = tmpvar_2.xyz;
  tmpvar_23.w = opacity_7;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * _FaceColor);
  faceColor_6 = tmpvar_24;
  outlineColor_5.xyz = _OutlineColor.xyz;
  faceColor_6.xyz = (faceColor_6.xyz * faceColor_6.w);
  highp float tmpvar_25;
  tmpvar_25 = (_OutlineColor.w * opacity_7);
  outlineColor_5.w = tmpvar_25;
  outlineColor_5.xyz = (_OutlineColor.xyz * outlineColor_5.w);
  highp vec4 tmpvar_26;
  tmpvar_26 = mix (faceColor_6, outlineColor_5, vec4(sqrt(min (1.0, 
    (tmpvar_21 * 2.0)
  ))));
  outlineColor_5 = tmpvar_26;
  layerColor_4 = _UnderlayColor;
  layerColor_4.w = (layerColor_4.w * opacity_7);
  layerColor_4.xyz = (layerColor_4.xyz * layerColor_4.w);
  highp float tmpvar_27;
  tmpvar_27 = (layerScale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * layerScale_8)));
  layerScale_8 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = (((
    -(_UnderlayOffsetX)
   * _ScaleRatioC) * _GradientScale) / _TextureWidth);
  tmpvar_28.y = (((
    -(_UnderlayOffsetY)
   * _ScaleRatioC) * _GradientScale) / _TextureHeight);
  highp vec4 tmpvar_29;
  tmpvar_29.x = tmpvar_19;
  tmpvar_29.y = (tmpvar_20 - tmpvar_21);
  tmpvar_29.z = (tmpvar_20 + tmpvar_21);
  tmpvar_29.w = tmpvar_20;
  highp vec4 tmpvar_30;
  tmpvar_30.xy = (vert_10.xy - _MaskCoord.xy);
  tmpvar_30.zw = (0.5 / tmpvar_15);
  highp vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_27;
  tmpvar_31.y = (((
    (0.5 - tmpvar_18)
   * tmpvar_27) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_27));
  mediump vec4 tmpvar_32;
  mediump vec4 tmpvar_33;
  lowp vec4 tmpvar_34;
  mediump vec2 tmpvar_35;
  tmpvar_32 = tmpvar_29;
  tmpvar_33 = tmpvar_30;
  tmpvar_34 = layerColor_4;
  tmpvar_35 = tmpvar_31;
  gl_Position = tmpvar_12;
  xlv_COLOR = faceColor_6;
  xlv_COLOR1 = outlineColor_5;
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_32;
  xlv_TEXCOORD2 = tmpvar_33;
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + tmpvar_28);
  xlv_TEXCOORD4 = tmpvar_34;
  xlv_TEXCOORD5 = tmpvar_35;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in highp vec2 xlv_TEXCOORD0;
in mediump vec4 xlv_TEXCOORD1;
in mediump vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
in lowp vec4 xlv_TEXCOORD4;
in mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec2 s_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  mediump float tmpvar_4;
  tmpvar_4 = (tmpvar_3.w * xlv_TEXCOORD1.x);
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((tmpvar_4 - xlv_TEXCOORD1.z), 0.0, 1.0);
  mediump float tmpvar_6;
  tmpvar_6 = clamp ((tmpvar_4 - xlv_TEXCOORD1.y), 0.0, 1.0);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (mix (xlv_COLOR1, xlv_COLOR, vec4(tmpvar_5)) * tmpvar_6);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture (_MainTex, xlv_TEXCOORD3);
  mediump float tmpvar_9;
  tmpvar_9 = (tmpvar_8.w * xlv_TEXCOORD5.x);
  mediump float tmpvar_10;
  tmpvar_10 = clamp ((tmpvar_9 - xlv_TEXCOORD5.y), 0.0, 1.0);
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((tmpvar_9 - xlv_TEXCOORD1.w), 0.0, 1.0);
  lowp vec4 tmpvar_12;
  tmpvar_12 = (tmpvar_7 + ((
    (xlv_TEXCOORD4 * (1.0 - tmpvar_10))
   * tmpvar_11) * (1.0 - tmpvar_7.w)));
  highp vec2 tmpvar_13;
  tmpvar_13.x = _MaskSoftnessX;
  tmpvar_13.y = _MaskSoftnessY;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_13 * xlv_TEXCOORD2.zw);
  s_1 = tmpvar_14;
  mediump vec2 tmpvar_15;
  tmpvar_15 = abs(xlv_TEXCOORD2.xy);
  highp vec2 tmpvar_16;
  tmpvar_16 = clamp (((
    ((tmpvar_15 - _MaskCoord.zw) * xlv_TEXCOORD2.zw)
   + s_1) / (1.0 + s_1)), 0.0, 1.0);
  mediump vec2 tmpvar_17;
  tmpvar_17 = (1.0 - tmpvar_16);
  mediump vec2 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_12 * (tmpvar_18.x * tmpvar_18.y));
  c_2 = tmpvar_19;
  _glesFragData[0] = c_2;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" }
"!!GLES3"
}
}
 }
}
}