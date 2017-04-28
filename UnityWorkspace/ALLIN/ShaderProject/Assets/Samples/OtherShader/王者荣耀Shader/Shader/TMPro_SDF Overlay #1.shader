Shader "TMPro/Distance Field Overlay" {
Properties {
 _FaceTex ("Face Texture", 2D) = "white" {}
 _FaceColor ("Face Color", Color) = (1,1,1,1)
 _FaceDilate ("Face Dilate", Range(-1,1)) = 0
 _OutlineColor ("Outline Color", Color) = (0,0,0,1)
 _OutlineTex ("Outline Texture", 2D) = "white" {}
 _OutlineWidth ("Outline Thickness", Range(0,1)) = 0
 _OutlineSoftness ("Outline Softness", Range(0,1)) = 0
 _Bevel ("Bevel", Range(0,1)) = 0.5
 _BevelOffset ("Bevel Offset", Range(-0.5,0.5)) = 0
 _BevelWidth ("Bevel Width", Range(-0.5,0.5)) = 0
 _BevelClamp ("Bevel Clamp", Range(0,1)) = 0
 _BevelRoundness ("Bevel Roundness", Range(0,1)) = 0
 _LightAngle ("Light Angle", Range(0,6.28319)) = 3.1416
 _SpecularColor ("Specular", Color) = (1,1,1,1)
 _SpecularPower ("Specular", Range(0,4)) = 2
 _Reflectivity ("Reflectivity", Range(5,15)) = 10
 _Diffuse ("Diffuse", Range(0,1)) = 0.5
 _Ambient ("Ambient", Range(1,0)) = 0.5
 _BumpMap ("Normal map", 2D) = "bump" {}
 _BumpOutline ("Bump Outline", Range(0,1)) = 0
 _BumpFace ("Bump Face", Range(0,1)) = 0
 _ReflectFaceColor ("Reflection Color", Color) = (0,0,0,1)
 _ReflectOutlineColor ("Reflection Color", Color) = (0,0,0,1)
 _Cube ("Reflection Cubemap", CUBE) = "black" { TexGen CubeReflect }
 _EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
 _UnderlayColor ("Border Color", Color) = (0,0,0,0.5)
 _UnderlayOffsetX ("Border OffsetX", Range(-1,1)) = 0
 _UnderlayOffsetY ("Border OffsetY", Range(-1,1)) = 0
 _UnderlayDilate ("Border Dilate", Range(-1,1)) = 0
 _UnderlaySoftness ("Border Softness", Range(0,1)) = 0
 _GlowColor ("Color", Color) = (0,1,0,0.5)
 _GlowOffset ("Offset", Range(-1,1)) = 0
 _GlowInner ("Inner", Range(0,1)) = 0.05
 _GlowOuter ("Outer", Range(0,1)) = 0.05
 _GlowPower ("Falloff", Range(1,0)) = 0.75
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
 _MaskCoord ("Mask Coords", Vector) = (0,0,0,0)
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
Keywords { "UNDERLAY_OFF" "MASK_OFF" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec2 tmpvar_21;
  tmpvar_21 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 * (tmpvar_21.x * tmpvar_21.y));
  faceColor_3 = tmpvar_22;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec2 tmpvar_21;
  tmpvar_21 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 * (tmpvar_21.x * tmpvar_21.y));
  faceColor_3 = tmpvar_22;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec2 tmpvar_21;
  tmpvar_21.x = _MaskSoftnessX;
  tmpvar_21.y = _MaskSoftnessY;
  highp vec2 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_23;
  tmpvar_23 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_22)
   / 
    (1.0 + tmpvar_22)
  ), 0.0, 1.0));
  highp vec2 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = (faceColor_3 * (tmpvar_24.x * tmpvar_24.y));
  faceColor_3 = tmpvar_25;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec2 tmpvar_21;
  tmpvar_21.x = _MaskSoftnessX;
  tmpvar_21.y = _MaskSoftnessY;
  highp vec2 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_23;
  tmpvar_23 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_22)
   / 
    (1.0 + tmpvar_22)
  ), 0.0, 1.0));
  highp vec2 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = (faceColor_3 * (tmpvar_24.x * tmpvar_24.y));
  faceColor_3 = tmpvar_25;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  highp vec2 tmpvar_27;
  tmpvar_27 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_28;
  tmpvar_28 = (faceColor_3 * (tmpvar_27.x * tmpvar_27.y));
  faceColor_3 = tmpvar_28;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  highp vec2 tmpvar_27;
  tmpvar_27 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_28;
  tmpvar_28 = (faceColor_3 * (tmpvar_27.x * tmpvar_27.y));
  faceColor_3 = tmpvar_28;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  highp vec2 tmpvar_27;
  tmpvar_27.x = _MaskSoftnessX;
  tmpvar_27.y = _MaskSoftnessY;
  highp vec2 tmpvar_28;
  tmpvar_28 = (tmpvar_27 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_29;
  tmpvar_29 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_28)
   / 
    (1.0 + tmpvar_28)
  ), 0.0, 1.0));
  highp vec2 tmpvar_30;
  tmpvar_30 = (tmpvar_29 * tmpvar_29);
  highp vec4 tmpvar_31;
  tmpvar_31 = (faceColor_3 * (tmpvar_30.x * tmpvar_30.y));
  faceColor_3 = tmpvar_31;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  highp vec2 tmpvar_27;
  tmpvar_27.x = _MaskSoftnessX;
  tmpvar_27.y = _MaskSoftnessY;
  highp vec2 tmpvar_28;
  tmpvar_28 = (tmpvar_27 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_29;
  tmpvar_29 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_28)
   / 
    (1.0 + tmpvar_28)
  ), 0.0, 1.0));
  highp vec2 tmpvar_30;
  tmpvar_30 = (tmpvar_29 * tmpvar_29);
  highp vec4 tmpvar_31;
  tmpvar_31 = (faceColor_3 * (tmpvar_30.x * tmpvar_30.y));
  faceColor_3 = tmpvar_31;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3 = tmpvar_22;
  tmpvar_1 = tmpvar_22;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3 = tmpvar_22;
  tmpvar_1 = tmpvar_22;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_23;
  tmpvar_23 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_22 * (tmpvar_23.x * tmpvar_23.y));
  faceColor_3 = tmpvar_24;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_23;
  tmpvar_23 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_22 * (tmpvar_23.x * tmpvar_23.y));
  faceColor_3 = tmpvar_24;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_23;
  tmpvar_23.x = _MaskSoftnessX;
  tmpvar_23.y = _MaskSoftnessY;
  highp vec2 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_25;
  tmpvar_25 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_24)
   / 
    (1.0 + tmpvar_24)
  ), 0.0, 1.0));
  highp vec2 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (tmpvar_22 * (tmpvar_26.x * tmpvar_26.y));
  faceColor_3 = tmpvar_27;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_23;
  tmpvar_23.x = _MaskSoftnessX;
  tmpvar_23.y = _MaskSoftnessY;
  highp vec2 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_25;
  tmpvar_25 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_24)
   / 
    (1.0 + tmpvar_24)
  ), 0.0, 1.0));
  highp vec2 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (tmpvar_22 * (tmpvar_26.x * tmpvar_26.y));
  faceColor_3 = tmpvar_27;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  highp vec2 tmpvar_29;
  tmpvar_29 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_30;
  tmpvar_30 = (faceColor_3 * (tmpvar_29.x * tmpvar_29.y));
  faceColor_3 = tmpvar_30;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  highp vec2 tmpvar_29;
  tmpvar_29 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_30;
  tmpvar_30 = (faceColor_3 * (tmpvar_29.x * tmpvar_29.y));
  faceColor_3 = tmpvar_30;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  highp vec2 tmpvar_29;
  tmpvar_29.x = _MaskSoftnessX;
  tmpvar_29.y = _MaskSoftnessY;
  highp vec2 tmpvar_30;
  tmpvar_30 = (tmpvar_29 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_31;
  tmpvar_31 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_30)
   / 
    (1.0 + tmpvar_30)
  ), 0.0, 1.0));
  highp vec2 tmpvar_32;
  tmpvar_32 = (tmpvar_31 * tmpvar_31);
  highp vec4 tmpvar_33;
  tmpvar_33 = (faceColor_3 * (tmpvar_32.x * tmpvar_32.y));
  faceColor_3 = tmpvar_33;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  highp vec2 tmpvar_29;
  tmpvar_29.x = _MaskSoftnessX;
  tmpvar_29.y = _MaskSoftnessY;
  highp vec2 tmpvar_30;
  tmpvar_30 = (tmpvar_29 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_31;
  tmpvar_31 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_30)
   / 
    (1.0 + tmpvar_30)
  ), 0.0, 1.0));
  highp vec2 tmpvar_32;
  tmpvar_32 = (tmpvar_31 * tmpvar_31);
  highp vec4 tmpvar_33;
  tmpvar_33 = (faceColor_3 * (tmpvar_32.x * tmpvar_32.y));
  faceColor_3 = tmpvar_33;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3 = tmpvar_24;
  tmpvar_1 = tmpvar_24;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3 = tmpvar_24;
  tmpvar_1 = tmpvar_24;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_25;
  tmpvar_25 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_26;
  tmpvar_26 = (tmpvar_24 * (tmpvar_25.x * tmpvar_25.y));
  faceColor_3 = tmpvar_26;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_25;
  tmpvar_25 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_26;
  tmpvar_26 = (tmpvar_24 * (tmpvar_25.x * tmpvar_25.y));
  faceColor_3 = tmpvar_26;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_25;
  tmpvar_25.x = _MaskSoftnessX;
  tmpvar_25.y = _MaskSoftnessY;
  highp vec2 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_27;
  tmpvar_27 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_26)
   / 
    (1.0 + tmpvar_26)
  ), 0.0, 1.0));
  highp vec2 tmpvar_28;
  tmpvar_28 = (tmpvar_27 * tmpvar_27);
  highp vec4 tmpvar_29;
  tmpvar_29 = (tmpvar_24 * (tmpvar_28.x * tmpvar_28.y));
  faceColor_3 = tmpvar_29;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_25;
  tmpvar_25.x = _MaskSoftnessX;
  tmpvar_25.y = _MaskSoftnessY;
  highp vec2 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_27;
  tmpvar_27 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_26)
   / 
    (1.0 + tmpvar_26)
  ), 0.0, 1.0));
  highp vec2 tmpvar_28;
  tmpvar_28 = (tmpvar_27 * tmpvar_27);
  highp vec4 tmpvar_29;
  tmpvar_29 = (tmpvar_24 * (tmpvar_28.x * tmpvar_28.y));
  faceColor_3 = tmpvar_29;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  highp vec2 tmpvar_31;
  tmpvar_31 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_32;
  tmpvar_32 = (faceColor_3 * (tmpvar_31.x * tmpvar_31.y));
  faceColor_3 = tmpvar_32;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  highp vec2 tmpvar_31;
  tmpvar_31 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_32;
  tmpvar_32 = (faceColor_3 * (tmpvar_31.x * tmpvar_31.y));
  faceColor_3 = tmpvar_32;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  highp vec2 tmpvar_31;
  tmpvar_31.x = _MaskSoftnessX;
  tmpvar_31.y = _MaskSoftnessY;
  highp vec2 tmpvar_32;
  tmpvar_32 = (tmpvar_31 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_33;
  tmpvar_33 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_32)
   / 
    (1.0 + tmpvar_32)
  ), 0.0, 1.0));
  highp vec2 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * tmpvar_33);
  highp vec4 tmpvar_35;
  tmpvar_35 = (faceColor_3 * (tmpvar_34.x * tmpvar_34.y));
  faceColor_3 = tmpvar_35;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  highp vec2 tmpvar_31;
  tmpvar_31.x = _MaskSoftnessX;
  tmpvar_31.y = _MaskSoftnessY;
  highp vec2 tmpvar_32;
  tmpvar_32 = (tmpvar_31 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_33;
  tmpvar_33 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_32)
   / 
    (1.0 + tmpvar_32)
  ), 0.0, 1.0));
  highp vec2 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * tmpvar_33);
  highp vec4 tmpvar_35;
  tmpvar_35 = (faceColor_3 * (tmpvar_34.x * tmpvar_34.y));
  faceColor_3 = tmpvar_35;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec2 tmpvar_21;
  tmpvar_21 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 * (tmpvar_21.x * tmpvar_21.y));
  faceColor_3 = tmpvar_22;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec2 tmpvar_21;
  tmpvar_21 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 * (tmpvar_21.x * tmpvar_21.y));
  faceColor_3 = tmpvar_22;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec2 tmpvar_21;
  tmpvar_21.x = _MaskSoftnessX;
  tmpvar_21.y = _MaskSoftnessY;
  highp vec2 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_23;
  tmpvar_23 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_22)
   / 
    (1.0 + tmpvar_22)
  ), 0.0, 1.0));
  highp vec2 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = (faceColor_3 * (tmpvar_24.x * tmpvar_24.y));
  faceColor_3 = tmpvar_25;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec2 tmpvar_21;
  tmpvar_21.x = _MaskSoftnessX;
  tmpvar_21.y = _MaskSoftnessY;
  highp vec2 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_23;
  tmpvar_23 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_22)
   / 
    (1.0 + tmpvar_22)
  ), 0.0, 1.0));
  highp vec2 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = (faceColor_3 * (tmpvar_24.x * tmpvar_24.y));
  faceColor_3 = tmpvar_25;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  highp vec2 tmpvar_27;
  tmpvar_27 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_28;
  tmpvar_28 = (faceColor_3 * (tmpvar_27.x * tmpvar_27.y));
  faceColor_3 = tmpvar_28;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  highp vec2 tmpvar_27;
  tmpvar_27 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_28;
  tmpvar_28 = (faceColor_3 * (tmpvar_27.x * tmpvar_27.y));
  faceColor_3 = tmpvar_28;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  highp vec2 tmpvar_27;
  tmpvar_27.x = _MaskSoftnessX;
  tmpvar_27.y = _MaskSoftnessY;
  highp vec2 tmpvar_28;
  tmpvar_28 = (tmpvar_27 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_29;
  tmpvar_29 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_28)
   / 
    (1.0 + tmpvar_28)
  ), 0.0, 1.0));
  highp vec2 tmpvar_30;
  tmpvar_30 = (tmpvar_29 * tmpvar_29);
  highp vec4 tmpvar_31;
  tmpvar_31 = (faceColor_3 * (tmpvar_30.x * tmpvar_30.y));
  faceColor_3 = tmpvar_31;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
out lowp vec4 xlv_COLOR;
out lowp vec4 xlv_COLOR1;
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
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
  tmpvar_14 = ((inversesqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
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
  tmpvar_16 = ((mix (_WeightNormal, _WeightBold, tmpvar_9) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_17;
  tmpvar_17 = tmpvar_2.w;
  opacity_6 = tmpvar_17;
  faceColor_5 = _FaceColor;
  faceColor_5.xyz = (faceColor_5.xyz * _glesColor.xyz);
  faceColor_5.w = (faceColor_5.w * opacity_6);
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * opacity_6);
  highp vec2 tmpvar_18;
  tmpvar_18.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_18.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_19;
  tmpvar_19.xy = tmpvar_3;
  tmpvar_19.zw = tmpvar_18;
  highp vec4 tmpvar_20;
  tmpvar_20.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_7)) - tmpvar_16);
  tmpvar_20.y = scale_7;
  tmpvar_20.z = ((0.5 - tmpvar_16) + (0.5 / scale_7));
  tmpvar_20.w = tmpvar_16;
  highp vec4 tmpvar_21;
  tmpvar_21.xy = (vert_8.xy - _MaskCoord.xy);
  tmpvar_21.zw = (0.5 / tmpvar_13);
  highp mat3 tmpvar_22;
  tmpvar_22[0] = _EnvMatrix[0].xyz;
  tmpvar_22[1] = _EnvMatrix[1].xyz;
  tmpvar_22[2] = _EnvMatrix[2].xyz;
  lowp vec4 tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_23 = faceColor_5;
  tmpvar_24 = outlineColor_4;
  gl_Position = tmpvar_10;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_23;
  xlv_COLOR2 = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_19;
  xlv_TEXCOORD1 = tmpvar_20;
  xlv_TEXCOORD2 = tmpvar_21;
  xlv_TEXCOORD3 = (tmpvar_22 * (_WorldSpaceCameraPos - (_Object2World * vert_8).xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  highp vec4 tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_23;
  tmpvar_23 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_22 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_24;
  tmpvar_24 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_22 / (1.0 + tmpvar_23)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_23)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_25;
  tmpvar_25.xyz = _GlowColor.xyz;
  tmpvar_25.w = tmpvar_24;
  tmpvar_21 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (faceColor_3.xyz + ((tmpvar_21.xyz * tmpvar_21.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_26;
  highp vec2 tmpvar_27;
  tmpvar_27.x = _MaskSoftnessX;
  tmpvar_27.y = _MaskSoftnessY;
  highp vec2 tmpvar_28;
  tmpvar_28 = (tmpvar_27 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_29;
  tmpvar_29 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_28)
   / 
    (1.0 + tmpvar_28)
  ), 0.0, 1.0));
  highp vec2 tmpvar_30;
  tmpvar_30 = (tmpvar_29 * tmpvar_29);
  highp vec4 tmpvar_31;
  tmpvar_31 = (faceColor_3 * (tmpvar_30.x * tmpvar_30.y));
  faceColor_3 = tmpvar_31;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3 = tmpvar_22;
  tmpvar_1 = tmpvar_22;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3 = tmpvar_22;
  tmpvar_1 = tmpvar_22;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_23;
  tmpvar_23 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_22 * (tmpvar_23.x * tmpvar_23.y));
  faceColor_3 = tmpvar_24;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_23;
  tmpvar_23 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_22 * (tmpvar_23.x * tmpvar_23.y));
  faceColor_3 = tmpvar_24;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_23;
  tmpvar_23.x = _MaskSoftnessX;
  tmpvar_23.y = _MaskSoftnessY;
  highp vec2 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_25;
  tmpvar_25 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_24)
   / 
    (1.0 + tmpvar_24)
  ), 0.0, 1.0));
  highp vec2 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (tmpvar_22 * (tmpvar_26.x * tmpvar_26.y));
  faceColor_3 = tmpvar_27;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_23;
  tmpvar_23.x = _MaskSoftnessX;
  tmpvar_23.y = _MaskSoftnessY;
  highp vec2 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_25;
  tmpvar_25 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_24)
   / 
    (1.0 + tmpvar_24)
  ), 0.0, 1.0));
  highp vec2 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (tmpvar_22 * (tmpvar_26.x * tmpvar_26.y));
  faceColor_3 = tmpvar_27;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  highp vec2 tmpvar_29;
  tmpvar_29 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_30;
  tmpvar_30 = (faceColor_3 * (tmpvar_29.x * tmpvar_29.y));
  faceColor_3 = tmpvar_30;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  highp vec2 tmpvar_29;
  tmpvar_29 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_30;
  tmpvar_30 = (faceColor_3 * (tmpvar_29.x * tmpvar_29.y));
  faceColor_3 = tmpvar_30;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  highp vec2 tmpvar_29;
  tmpvar_29.x = _MaskSoftnessX;
  tmpvar_29.y = _MaskSoftnessY;
  highp vec2 tmpvar_30;
  tmpvar_30 = (tmpvar_29 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_31;
  tmpvar_31 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_30)
   / 
    (1.0 + tmpvar_30)
  ), 0.0, 1.0));
  highp vec2 tmpvar_32;
  tmpvar_32 = (tmpvar_31 * tmpvar_31);
  highp vec4 tmpvar_33;
  tmpvar_33 = (faceColor_3 * (tmpvar_32.x * tmpvar_32.y));
  faceColor_3 = tmpvar_33;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_7;
  tmpvar_7 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_10;
  tmpvar_10 = (faceColor_3 * tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_12;
  tmpvar_12 = (outlineColor_2 * tmpvar_11);
  outlineColor_2 = tmpvar_12;
  mediump float d_13;
  d_13 = tmpvar_6;
  lowp vec4 faceColor_14;
  faceColor_14 = tmpvar_10;
  lowp vec4 outlineColor_15;
  outlineColor_15 = tmpvar_12;
  mediump float outline_16;
  outline_16 = tmpvar_7;
  mediump float softness_17;
  softness_17 = tmpvar_8;
  faceColor_14.xyz = (faceColor_14.xyz * faceColor_14.w);
  outlineColor_15.xyz = (outlineColor_15.xyz * outlineColor_15.w);
  mediump vec4 tmpvar_18;
  tmpvar_18 = mix (faceColor_14, outlineColor_15, vec4((clamp (
    (d_13 + (outline_16 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_16)
  ))));
  faceColor_14 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = (faceColor_14 * (1.0 - clamp (
    (((d_13 - (outline_16 * 0.5)) + (softness_17 * 0.5)) / (1.0 + softness_17))
  , 0.0, 1.0)));
  faceColor_14 = tmpvar_19;
  faceColor_3 = faceColor_14;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_20.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (faceColor_3 + ((xlv_TEXCOORD5 * tmpvar_21) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_22.w;
  highp vec4 tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = (tmpvar_6 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_25;
  tmpvar_25 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_24 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_26;
  tmpvar_26 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_24 / (1.0 + tmpvar_25)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_25)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = _GlowColor.xyz;
  tmpvar_27.w = tmpvar_26;
  tmpvar_23 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_22.xyz + ((tmpvar_23.xyz * tmpvar_23.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_28;
  highp vec2 tmpvar_29;
  tmpvar_29.x = _MaskSoftnessX;
  tmpvar_29.y = _MaskSoftnessY;
  highp vec2 tmpvar_30;
  tmpvar_30 = (tmpvar_29 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_31;
  tmpvar_31 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_30)
   / 
    (1.0 + tmpvar_30)
  ), 0.0, 1.0));
  highp vec2 tmpvar_32;
  tmpvar_32 = (tmpvar_31 * tmpvar_31);
  highp vec4 tmpvar_33;
  tmpvar_33 = (faceColor_3 * (tmpvar_32.x * tmpvar_32.y));
  faceColor_3 = tmpvar_33;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3 = tmpvar_24;
  tmpvar_1 = tmpvar_24;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3 = tmpvar_24;
  tmpvar_1 = tmpvar_24;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_25;
  tmpvar_25 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_26;
  tmpvar_26 = (tmpvar_24 * (tmpvar_25.x * tmpvar_25.y));
  faceColor_3 = tmpvar_26;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_25;
  tmpvar_25 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_26;
  tmpvar_26 = (tmpvar_24 * (tmpvar_25.x * tmpvar_25.y));
  faceColor_3 = tmpvar_26;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_25;
  tmpvar_25.x = _MaskSoftnessX;
  tmpvar_25.y = _MaskSoftnessY;
  highp vec2 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_27;
  tmpvar_27 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_26)
   / 
    (1.0 + tmpvar_26)
  ), 0.0, 1.0));
  highp vec2 tmpvar_28;
  tmpvar_28 = (tmpvar_27 * tmpvar_27);
  highp vec4 tmpvar_29;
  tmpvar_29 = (tmpvar_24 * (tmpvar_28.x * tmpvar_28.y));
  faceColor_3 = tmpvar_29;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "GLOW_OFF" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    ((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  highp vec2 tmpvar_25;
  tmpvar_25.x = _MaskSoftnessX;
  tmpvar_25.y = _MaskSoftnessY;
  highp vec2 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_27;
  tmpvar_27 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_26)
   / 
    (1.0 + tmpvar_26)
  ), 0.0, 1.0));
  highp vec2 tmpvar_28;
  tmpvar_28 = (tmpvar_27 * tmpvar_27);
  highp vec4 tmpvar_29;
  tmpvar_29 = (tmpvar_24 * (tmpvar_28.x * tmpvar_28.y));
  faceColor_3 = tmpvar_29;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  highp vec2 tmpvar_31;
  tmpvar_31 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_32;
  tmpvar_32 = (faceColor_3 * (tmpvar_31.x * tmpvar_31.y));
  faceColor_3 = tmpvar_32;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  highp vec2 tmpvar_31;
  tmpvar_31 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp vec4 tmpvar_32;
  tmpvar_32 = (faceColor_3 * (tmpvar_31.x * tmpvar_31.y));
  faceColor_3 = tmpvar_32;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying lowp vec4 xlv_COLOR1;
varying lowp vec4 xlv_COLOR2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  highp vec2 tmpvar_31;
  tmpvar_31.x = _MaskSoftnessX;
  tmpvar_31.y = _MaskSoftnessY;
  highp vec2 tmpvar_32;
  tmpvar_32 = (tmpvar_31 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_33;
  tmpvar_33 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_32)
   / 
    (1.0 + tmpvar_32)
  ), 0.0, 1.0));
  highp vec2 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * tmpvar_33);
  highp vec4 tmpvar_35;
  tmpvar_35 = (faceColor_3 * (tmpvar_34.x * tmpvar_34.y));
  faceColor_3 = tmpvar_35;
  tmpvar_1 = faceColor_3;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "GLOW_ON" }
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
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _FaceDilate;
uniform highp float _OutlineSoftness;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp mat4 _EnvMatrix;
uniform lowp vec4 _UnderlayColor;
uniform highp float _UnderlayOffsetX;
uniform highp float _UnderlayOffsetY;
uniform highp float _UnderlayDilate;
uniform highp float _UnderlaySoftness;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
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
out lowp vec4 xlv_COLOR2;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
out lowp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec2 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0.xy;
  highp vec4 underlayColor_4;
  highp vec4 outlineColor_5;
  highp vec4 faceColor_6;
  highp float opacity_7;
  highp float scale_8;
  highp vec4 vert_9;
  highp float tmpvar_10;
  tmpvar_10 = float((0.0 >= _glesMultiTexCoord1.y));
  vert_9.zw = _glesVertex.zw;
  vert_9.x = (_glesVertex.x + _VertexOffsetX);
  vert_9.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_11;
  tmpvar_11 = (glstate_matrix_mvp * vert_9);
  highp vec2 tmpvar_12;
  tmpvar_12.x = _ScaleX;
  tmpvar_12.y = _ScaleY;
  highp mat2 tmpvar_13;
  tmpvar_13[0] = glstate_matrix_projection[0].xy;
  tmpvar_13[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11.ww / (tmpvar_12 * abs(
    (tmpvar_13 * _ScreenParams.xy)
  )));
  highp float tmpvar_15;
  tmpvar_15 = ((inversesqrt(
    dot (tmpvar_14, tmpvar_14)
  ) * abs(_glesMultiTexCoord1.y)) * (_GradientScale * 1.5));
  scale_8 = tmpvar_15;
  if ((glstate_matrix_projection[3].w == 0.0)) {
    highp vec4 tmpvar_16;
    tmpvar_16.w = 1.0;
    tmpvar_16.xyz = _WorldSpaceCameraPos;
    scale_8 = mix ((tmpvar_15 * (1.0 - _PerspectiveFilter)), tmpvar_15, abs(dot (tmpvar_1, 
      normalize((((_World2Object * tmpvar_16).xyz * unity_Scale.w) - vert_9.xyz))
    )));
  };
  highp float tmpvar_17;
  tmpvar_17 = ((mix (_WeightNormal, _WeightBold, tmpvar_10) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  lowp float tmpvar_18;
  tmpvar_18 = tmpvar_2.w;
  opacity_7 = tmpvar_18;
  faceColor_6 = _FaceColor;
  faceColor_6.xyz = (faceColor_6.xyz * _glesColor.xyz);
  faceColor_6.w = (faceColor_6.w * opacity_7);
  outlineColor_5 = _OutlineColor;
  outlineColor_5.w = (outlineColor_5.w * opacity_7);
  underlayColor_4 = _UnderlayColor;
  underlayColor_4.w = (underlayColor_4.w * opacity_7);
  underlayColor_4.xyz = (underlayColor_4.xyz * underlayColor_4.w);
  highp float tmpvar_19;
  tmpvar_19 = (scale_8 / (1.0 + (
    (_UnderlaySoftness * _ScaleRatioC)
   * scale_8)));
  highp vec2 tmpvar_20;
  tmpvar_20.x = ((-(
    (_UnderlayOffsetX * _ScaleRatioC)
  ) * _GradientScale) / _TextureWidth);
  tmpvar_20.y = ((-(
    (_UnderlayOffsetY * _ScaleRatioC)
  ) * _GradientScale) / _TextureHeight);
  highp vec2 tmpvar_21;
  tmpvar_21.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_21.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp vec4 tmpvar_22;
  tmpvar_22.xy = tmpvar_3;
  tmpvar_22.zw = tmpvar_21;
  highp vec4 tmpvar_23;
  tmpvar_23.x = (((
    min (((1.0 - (_OutlineWidth * _ScaleRatioA)) - (_OutlineSoftness * _ScaleRatioA)), ((1.0 - (_GlowOffset * _ScaleRatioB)) - (_GlowOuter * _ScaleRatioB)))
   / 2.0) - (0.5 / scale_8)) - tmpvar_17);
  tmpvar_23.y = scale_8;
  tmpvar_23.z = ((0.5 - tmpvar_17) + (0.5 / scale_8));
  tmpvar_23.w = tmpvar_17;
  highp vec4 tmpvar_24;
  tmpvar_24.xy = (vert_9.xy - _MaskCoord.xy);
  tmpvar_24.zw = (0.5 / tmpvar_14);
  highp mat3 tmpvar_25;
  tmpvar_25[0] = _EnvMatrix[0].xyz;
  tmpvar_25[1] = _EnvMatrix[1].xyz;
  tmpvar_25[2] = _EnvMatrix[2].xyz;
  highp vec4 tmpvar_26;
  tmpvar_26.xy = (_glesMultiTexCoord0.xy + tmpvar_20);
  tmpvar_26.z = tmpvar_19;
  tmpvar_26.w = (((
    (0.5 - tmpvar_17)
   * tmpvar_19) - 0.5) - ((
    (_UnderlayDilate * _ScaleRatioC)
   * 0.5) * tmpvar_19));
  lowp vec4 tmpvar_27;
  lowp vec4 tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_27 = faceColor_6;
  tmpvar_28 = outlineColor_5;
  tmpvar_29 = underlayColor_4;
  gl_Position = tmpvar_11;
  xlv_COLOR = tmpvar_2;
  xlv_COLOR1 = tmpvar_27;
  xlv_COLOR2 = tmpvar_28;
  xlv_TEXCOORD0 = tmpvar_22;
  xlv_TEXCOORD1 = tmpvar_23;
  xlv_TEXCOORD2 = tmpvar_24;
  xlv_TEXCOORD3 = (tmpvar_25 * (_WorldSpaceCameraPos - (_Object2World * vert_9).xyz));
  xlv_TEXCOORD4 = tmpvar_26;
  xlv_TEXCOORD5 = tmpvar_29;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _FaceTex;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in lowp vec4 xlv_COLOR1;
in lowp vec4 xlv_COLOR2;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD4;
in lowp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 outlineColor_2;
  mediump vec4 faceColor_3;
  highp float c_4;
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_4 = tmpvar_5;
  highp float x_6;
  x_6 = (c_4 - xlv_TEXCOORD1.x);
  if ((x_6 < 0.0)) {
    discard;
  };
  highp float tmpvar_7;
  tmpvar_7 = ((xlv_TEXCOORD1.z - c_4) * xlv_TEXCOORD1.y);
  highp float tmpvar_8;
  tmpvar_8 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_3 = xlv_COLOR1;
  outlineColor_2 = xlv_COLOR2;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_FaceTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_11;
  tmpvar_11 = (faceColor_3 * tmpvar_10);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_OutlineTex, xlv_TEXCOORD0.zw);
  mediump vec4 tmpvar_13;
  tmpvar_13 = (outlineColor_2 * tmpvar_12);
  outlineColor_2 = tmpvar_13;
  mediump float d_14;
  d_14 = tmpvar_7;
  lowp vec4 faceColor_15;
  faceColor_15 = tmpvar_11;
  lowp vec4 outlineColor_16;
  outlineColor_16 = tmpvar_13;
  mediump float outline_17;
  outline_17 = tmpvar_8;
  mediump float softness_18;
  softness_18 = tmpvar_9;
  faceColor_15.xyz = (faceColor_15.xyz * faceColor_15.w);
  outlineColor_16.xyz = (outlineColor_16.xyz * outlineColor_16.w);
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (faceColor_15, outlineColor_16, vec4((clamp (
    (d_14 + (outline_17 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_17)
  ))));
  faceColor_15 = tmpvar_19;
  mediump vec4 tmpvar_20;
  tmpvar_20 = (faceColor_15 * (1.0 - clamp (
    (((d_14 - (outline_17 * 0.5)) + (softness_18 * 0.5)) / (1.0 + softness_18))
  , 0.0, 1.0)));
  faceColor_15 = tmpvar_20;
  faceColor_3 = faceColor_15;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture (_MainTex, xlv_TEXCOORD4.xy);
  highp float tmpvar_22;
  tmpvar_22 = clamp (((tmpvar_21.w * xlv_TEXCOORD4.z) - xlv_TEXCOORD4.w), 0.0, 1.0);
  highp float tmpvar_23;
  tmpvar_23 = clamp ((1.0 - tmpvar_7), 0.0, 1.0);
  mediump vec4 tmpvar_24;
  tmpvar_24 = (faceColor_3 + ((
    (xlv_TEXCOORD5 * (1.0 - tmpvar_22))
   * tmpvar_23) * (1.0 - faceColor_3.w)));
  faceColor_3.w = tmpvar_24.w;
  highp vec4 tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_7 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_27;
  tmpvar_27 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_26 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_28;
  tmpvar_28 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_26 / (1.0 + tmpvar_27)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_27)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = _GlowColor.xyz;
  tmpvar_29.w = tmpvar_28;
  tmpvar_25 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_24.xyz + ((tmpvar_25.xyz * tmpvar_25.w) * xlv_COLOR.w));
  faceColor_3.xyz = tmpvar_30;
  highp vec2 tmpvar_31;
  tmpvar_31.x = _MaskSoftnessX;
  tmpvar_31.y = _MaskSoftnessY;
  highp vec2 tmpvar_32;
  tmpvar_32 = (tmpvar_31 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_33;
  tmpvar_33 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_32)
   / 
    (1.0 + tmpvar_32)
  ), 0.0, 1.0));
  highp vec2 tmpvar_34;
  tmpvar_34 = (tmpvar_33 * tmpvar_33);
  highp vec4 tmpvar_35;
  tmpvar_35 = (faceColor_3 * (tmpvar_34.x * tmpvar_34.y));
  faceColor_3 = tmpvar_35;
  tmpvar_1 = faceColor_3;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "BEVEL_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_OFF" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_HARD" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "UNDERLAY_OFF" "MASK_SOFT" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_ON" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_ON" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_ON" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" "UNDERLAY_INNER" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" "UNDERLAY_INNER" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" "UNDERLAY_INNER" "GLOW_ON" }
"!!GLES3"
}
}
 }
}
Fallback "TMPro/Mobile/Distance Field"
}