Shader "TMPro/Mobile/Distance Field (Surface)" {
Properties {
 _FaceTex ("Fill Texture", 2D) = "white" {}
 _FaceColor ("Fill Color", Color) = (1,1,1,1)
 _FaceDilate ("Face Dilate", Range(-1,1)) = 0
 _OutlineColor ("Outline Color", Color) = (0,0,0,1)
 _OutlineTex ("Outline Texture", 2D) = "white" {}
 _OutlineWidth ("Outline Thickness", Range(0,1)) = 0
 _OutlineSoftness ("Outline Softness", Range(0,1)) = 0
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
}
SubShader { 
 LOD 300
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull [_CullMode]
  Blend SrcAlpha OneMinusSrcAlpha
  AlphaTest Greater 0
  ColorMask RGB
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "GLOW_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FaceDilate;
uniform highp mat4 _EnvMatrix;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _FaceTex_ST;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = _glesVertex.zw;
  highp vec2 tmpvar_8;
  tmpvar_7.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_7.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_9).xyz * unity_Scale.w) - tmpvar_7.xyz)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((glstate_matrix_mvp * tmpvar_7).ww / (tmpvar_11 * (tmpvar_12 * _ScreenParams.xy)));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = _WorldSpaceCameraPos;
  tmpvar_8.y = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_10, 
    normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - tmpvar_7.xyz))
  )));
  tmpvar_8.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_16;
  tmpvar_16.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_16.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_17;
  tmpvar_17[0] = _EnvMatrix[0].xyz;
  tmpvar_17[1] = _EnvMatrix[1].xyz;
  tmpvar_17[2] = _EnvMatrix[2].xyz;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((tmpvar_16 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_19 = tmpvar_1.xyz;
  tmpvar_20 = (((tmpvar_10.yzx * tmpvar_1.zxy) - (tmpvar_10.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_21;
  tmpvar_21[0].x = tmpvar_19.x;
  tmpvar_21[0].y = tmpvar_20.x;
  tmpvar_21[0].z = tmpvar_10.x;
  tmpvar_21[1].x = tmpvar_19.y;
  tmpvar_21[1].y = tmpvar_20.y;
  tmpvar_21[1].z = tmpvar_10.y;
  tmpvar_21[2].x = tmpvar_19.z;
  tmpvar_21[2].y = tmpvar_20.z;
  tmpvar_21[2].z = tmpvar_10.z;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
  mediump vec3 tmpvar_24;
  mediump vec4 normal_25;
  normal_25 = tmpvar_23;
  highp float vC_26;
  mediump vec3 x3_27;
  mediump vec3 x2_28;
  mediump vec3 x1_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHAr, normal_25);
  x1_29.x = tmpvar_30;
  highp float tmpvar_31;
  tmpvar_31 = dot (unity_SHAg, normal_25);
  x1_29.y = tmpvar_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAb, normal_25);
  x1_29.z = tmpvar_32;
  mediump vec4 tmpvar_33;
  tmpvar_33 = (normal_25.xyzz * normal_25.yzzx);
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHBr, tmpvar_33);
  x2_28.x = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHBg, tmpvar_33);
  x2_28.y = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBb, tmpvar_33);
  x2_28.z = tmpvar_36;
  mediump float tmpvar_37;
  tmpvar_37 = ((normal_25.x * normal_25.x) - (normal_25.y * normal_25.y));
  vC_26 = tmpvar_37;
  highp vec3 tmpvar_38;
  tmpvar_38 = (unity_SHC.xyz * vC_26);
  x3_27 = tmpvar_38;
  tmpvar_24 = ((x1_29 + x2_28) + x3_27);
  shlight_3 = tmpvar_24;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform sampler2D _FaceTex;
uniform highp float _FaceUVSpeedX;
uniform highp float _FaceUVSpeedY;
uniform lowp vec4 _FaceColor;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineUVSpeedX;
uniform highp float _OutlineUVSpeedY;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
  highp float c_6;
  lowp float tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_6 = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = (((
    (0.5 - c_6)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_10;
  tmpvar_10 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_5 = _FaceColor;
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * xlv_COLOR0.w);
  highp vec2 tmpvar_11;
  tmpvar_11.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_11.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_FaceTex, tmpvar_11);
  highp vec4 tmpvar_13;
  tmpvar_13 = ((faceColor_5 * xlv_COLOR0) * tmpvar_12);
  highp vec2 tmpvar_14;
  tmpvar_14.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_14.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_OutlineTex, tmpvar_14);
  highp vec4 tmpvar_16;
  tmpvar_16 = (outlineColor_4 * tmpvar_15);
  outlineColor_4 = tmpvar_16;
  mediump float d_17;
  d_17 = tmpvar_8;
  lowp vec4 faceColor_18;
  faceColor_18 = tmpvar_13;
  lowp vec4 outlineColor_19;
  outlineColor_19 = tmpvar_16;
  mediump float outline_20;
  outline_20 = tmpvar_9;
  mediump float softness_21;
  softness_21 = tmpvar_10;
  faceColor_18.xyz = (faceColor_18.xyz * faceColor_18.w);
  outlineColor_19.xyz = (outlineColor_19.xyz * outlineColor_19.w);
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (faceColor_18, outlineColor_19, vec4((clamp (
    (d_17 + (outline_20 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_20)
  ))));
  faceColor_18 = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (faceColor_18 * (1.0 - clamp (
    (((d_17 - (outline_20 * 0.5)) + (softness_21 * 0.5)) / (1.0 + softness_21))
  , 0.0, 1.0)));
  faceColor_18 = tmpvar_23;
  faceColor_5 = faceColor_18;
  faceColor_5.xyz = (faceColor_5.xyz / faceColor_5.w);
  highp vec3 tmpvar_24;
  tmpvar_24 = faceColor_5.xyz;
  tmpvar_2 = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = faceColor_5.w;
  tmpvar_3 = tmpvar_25;
  lowp vec4 c_26;
  c_26.xyz = ((tmpvar_2 * _LightColor0.xyz) * (max (0.0, xlv_TEXCOORD3.z) * 2.0));
  c_26.w = tmpvar_3;
  c_1.w = c_26.w;
  c_1.xyz = (c_26.xyz + (tmpvar_2 * xlv_TEXCOORD4));
  c_1.xyz = c_1.xyz;
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "GLOW_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FaceDilate;
uniform highp mat4 _EnvMatrix;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _FaceTex_ST;
out highp vec4 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = _glesVertex.zw;
  highp vec2 tmpvar_8;
  tmpvar_7.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_7.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_9).xyz * unity_Scale.w) - tmpvar_7.xyz)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((glstate_matrix_mvp * tmpvar_7).ww / (tmpvar_11 * (tmpvar_12 * _ScreenParams.xy)));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = _WorldSpaceCameraPos;
  tmpvar_8.y = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_10, 
    normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - tmpvar_7.xyz))
  )));
  tmpvar_8.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_16;
  tmpvar_16.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_16.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_17;
  tmpvar_17[0] = _EnvMatrix[0].xyz;
  tmpvar_17[1] = _EnvMatrix[1].xyz;
  tmpvar_17[2] = _EnvMatrix[2].xyz;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((tmpvar_16 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_19 = tmpvar_1.xyz;
  tmpvar_20 = (((tmpvar_10.yzx * tmpvar_1.zxy) - (tmpvar_10.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_21;
  tmpvar_21[0].x = tmpvar_19.x;
  tmpvar_21[0].y = tmpvar_20.x;
  tmpvar_21[0].z = tmpvar_10.x;
  tmpvar_21[1].x = tmpvar_19.y;
  tmpvar_21[1].y = tmpvar_20.y;
  tmpvar_21[1].z = tmpvar_10.y;
  tmpvar_21[2].x = tmpvar_19.z;
  tmpvar_21[2].y = tmpvar_20.z;
  tmpvar_21[2].z = tmpvar_10.z;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
  mediump vec3 tmpvar_24;
  mediump vec4 normal_25;
  normal_25 = tmpvar_23;
  highp float vC_26;
  mediump vec3 x3_27;
  mediump vec3 x2_28;
  mediump vec3 x1_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHAr, normal_25);
  x1_29.x = tmpvar_30;
  highp float tmpvar_31;
  tmpvar_31 = dot (unity_SHAg, normal_25);
  x1_29.y = tmpvar_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAb, normal_25);
  x1_29.z = tmpvar_32;
  mediump vec4 tmpvar_33;
  tmpvar_33 = (normal_25.xyzz * normal_25.yzzx);
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHBr, tmpvar_33);
  x2_28.x = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHBg, tmpvar_33);
  x2_28.y = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBb, tmpvar_33);
  x2_28.z = tmpvar_36;
  mediump float tmpvar_37;
  tmpvar_37 = ((normal_25.x * normal_25.x) - (normal_25.y * normal_25.y));
  vC_26 = tmpvar_37;
  highp vec3 tmpvar_38;
  tmpvar_38 = (unity_SHC.xyz * vC_26);
  x3_27 = tmpvar_38;
  tmpvar_24 = ((x1_29 + x2_28) + x3_27);
  shlight_3 = tmpvar_24;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform sampler2D _FaceTex;
uniform highp float _FaceUVSpeedX;
uniform highp float _FaceUVSpeedY;
uniform lowp vec4 _FaceColor;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineUVSpeedX;
uniform highp float _OutlineUVSpeedY;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
  highp float c_6;
  lowp float tmpvar_7;
  tmpvar_7 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_6 = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = (((
    (0.5 - c_6)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_10;
  tmpvar_10 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_5 = _FaceColor;
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * xlv_COLOR0.w);
  highp vec2 tmpvar_11;
  tmpvar_11.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_11.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_FaceTex, tmpvar_11);
  highp vec4 tmpvar_13;
  tmpvar_13 = ((faceColor_5 * xlv_COLOR0) * tmpvar_12);
  highp vec2 tmpvar_14;
  tmpvar_14.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_14.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture (_OutlineTex, tmpvar_14);
  highp vec4 tmpvar_16;
  tmpvar_16 = (outlineColor_4 * tmpvar_15);
  outlineColor_4 = tmpvar_16;
  mediump float d_17;
  d_17 = tmpvar_8;
  lowp vec4 faceColor_18;
  faceColor_18 = tmpvar_13;
  lowp vec4 outlineColor_19;
  outlineColor_19 = tmpvar_16;
  mediump float outline_20;
  outline_20 = tmpvar_9;
  mediump float softness_21;
  softness_21 = tmpvar_10;
  faceColor_18.xyz = (faceColor_18.xyz * faceColor_18.w);
  outlineColor_19.xyz = (outlineColor_19.xyz * outlineColor_19.w);
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (faceColor_18, outlineColor_19, vec4((clamp (
    (d_17 + (outline_20 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_20)
  ))));
  faceColor_18 = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (faceColor_18 * (1.0 - clamp (
    (((d_17 - (outline_20 * 0.5)) + (softness_21 * 0.5)) / (1.0 + softness_21))
  , 0.0, 1.0)));
  faceColor_18 = tmpvar_23;
  faceColor_5 = faceColor_18;
  faceColor_5.xyz = (faceColor_5.xyz / faceColor_5.w);
  highp vec3 tmpvar_24;
  tmpvar_24 = faceColor_5.xyz;
  tmpvar_2 = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = faceColor_5.w;
  tmpvar_3 = tmpvar_25;
  lowp vec4 c_26;
  c_26.xyz = ((tmpvar_2 * _LightColor0.xyz) * (max (0.0, xlv_TEXCOORD3.z) * 2.0));
  c_26.w = tmpvar_3;
  c_1.w = c_26.w;
  c_1.xyz = (c_26.xyz + (tmpvar_2 * xlv_TEXCOORD4));
  c_1.xyz = c_1.xyz;
  c_1.w = tmpvar_3;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" "GLOW_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FaceDilate;
uniform highp mat4 _EnvMatrix;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _FaceTex_ST;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = _glesVertex.zw;
  highp vec2 tmpvar_8;
  tmpvar_7.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_7.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_9).xyz * unity_Scale.w) - tmpvar_7.xyz)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((glstate_matrix_mvp * tmpvar_7).ww / (tmpvar_11 * (tmpvar_12 * _ScreenParams.xy)));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = _WorldSpaceCameraPos;
  tmpvar_8.y = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_10, 
    normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - tmpvar_7.xyz))
  )));
  tmpvar_8.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_16;
  tmpvar_16.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_16.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_17;
  tmpvar_17[0] = _EnvMatrix[0].xyz;
  tmpvar_17[1] = _EnvMatrix[1].xyz;
  tmpvar_17[2] = _EnvMatrix[2].xyz;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((tmpvar_16 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
  highp vec3 tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_20 = tmpvar_1.xyz;
  tmpvar_21 = (((tmpvar_10.yzx * tmpvar_1.zxy) - (tmpvar_10.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_22;
  tmpvar_22[0].x = tmpvar_20.x;
  tmpvar_22[0].y = tmpvar_21.x;
  tmpvar_22[0].z = tmpvar_10.x;
  tmpvar_22[1].x = tmpvar_20.y;
  tmpvar_22[1].y = tmpvar_21.y;
  tmpvar_22[1].z = tmpvar_10.y;
  tmpvar_22[2].x = tmpvar_20.z;
  tmpvar_22[2].y = tmpvar_21.z;
  tmpvar_22[2].z = tmpvar_10.z;
  highp vec3 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_19;
  mediump vec3 tmpvar_25;
  mediump vec4 normal_26;
  normal_26 = tmpvar_24;
  highp float vC_27;
  mediump vec3 x3_28;
  mediump vec3 x2_29;
  mediump vec3 x1_30;
  highp float tmpvar_31;
  tmpvar_31 = dot (unity_SHAr, normal_26);
  x1_30.x = tmpvar_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAg, normal_26);
  x1_30.y = tmpvar_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAb, normal_26);
  x1_30.z = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (normal_26.xyzz * normal_26.yzzx);
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHBr, tmpvar_34);
  x2_29.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBg, tmpvar_34);
  x2_29.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBb, tmpvar_34);
  x2_29.z = tmpvar_37;
  mediump float tmpvar_38;
  tmpvar_38 = ((normal_26.x * normal_26.x) - (normal_26.y * normal_26.y));
  vC_27 = tmpvar_38;
  highp vec3 tmpvar_39;
  tmpvar_39 = (unity_SHC.xyz * vC_27);
  x3_28 = tmpvar_39;
  tmpvar_25 = ((x1_30 + x2_29) + x3_28);
  shlight_3 = tmpvar_25;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * tmpvar_7).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_41 * tmpvar_19.x) + (tmpvar_42 * tmpvar_19.y)) + (tmpvar_43 * tmpvar_19.z))
   * 
    inversesqrt(tmpvar_44)
  )) * (1.0/((1.0 + 
    (tmpvar_44 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_46;
  tmpvar_46 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_45.z)
  ) + (unity_LightColor[3].xyz * tmpvar_45.w)));
  tmpvar_6 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform sampler2D _FaceTex;
uniform highp float _FaceUVSpeedX;
uniform highp float _FaceUVSpeedY;
uniform lowp vec4 _FaceColor;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineUVSpeedX;
uniform highp float _OutlineUVSpeedY;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
  highp float c_6;
  lowp float tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_6 = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = (((
    (0.5 - c_6)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_10;
  tmpvar_10 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_5 = _FaceColor;
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * xlv_COLOR0.w);
  highp vec2 tmpvar_11;
  tmpvar_11.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_11.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_FaceTex, tmpvar_11);
  highp vec4 tmpvar_13;
  tmpvar_13 = ((faceColor_5 * xlv_COLOR0) * tmpvar_12);
  highp vec2 tmpvar_14;
  tmpvar_14.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_14.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_OutlineTex, tmpvar_14);
  highp vec4 tmpvar_16;
  tmpvar_16 = (outlineColor_4 * tmpvar_15);
  outlineColor_4 = tmpvar_16;
  mediump float d_17;
  d_17 = tmpvar_8;
  lowp vec4 faceColor_18;
  faceColor_18 = tmpvar_13;
  lowp vec4 outlineColor_19;
  outlineColor_19 = tmpvar_16;
  mediump float outline_20;
  outline_20 = tmpvar_9;
  mediump float softness_21;
  softness_21 = tmpvar_10;
  faceColor_18.xyz = (faceColor_18.xyz * faceColor_18.w);
  outlineColor_19.xyz = (outlineColor_19.xyz * outlineColor_19.w);
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (faceColor_18, outlineColor_19, vec4((clamp (
    (d_17 + (outline_20 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_20)
  ))));
  faceColor_18 = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (faceColor_18 * (1.0 - clamp (
    (((d_17 - (outline_20 * 0.5)) + (softness_21 * 0.5)) / (1.0 + softness_21))
  , 0.0, 1.0)));
  faceColor_18 = tmpvar_23;
  faceColor_5 = faceColor_18;
  faceColor_5.xyz = (faceColor_5.xyz / faceColor_5.w);
  highp vec3 tmpvar_24;
  tmpvar_24 = faceColor_5.xyz;
  tmpvar_2 = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = faceColor_5.w;
  tmpvar_3 = tmpvar_25;
  lowp vec4 c_26;
  c_26.xyz = ((tmpvar_2 * _LightColor0.xyz) * (max (0.0, xlv_TEXCOORD3.z) * 2.0));
  c_26.w = tmpvar_3;
  c_1.w = c_26.w;
  c_1.xyz = (c_26.xyz + (tmpvar_2 * xlv_TEXCOORD4));
  c_1.xyz = c_1.xyz;
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" "GLOW_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FaceDilate;
uniform highp mat4 _EnvMatrix;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _FaceTex_ST;
out highp vec4 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = _glesVertex.zw;
  highp vec2 tmpvar_8;
  tmpvar_7.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_7.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_9).xyz * unity_Scale.w) - tmpvar_7.xyz)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((glstate_matrix_mvp * tmpvar_7).ww / (tmpvar_11 * (tmpvar_12 * _ScreenParams.xy)));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = _WorldSpaceCameraPos;
  tmpvar_8.y = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_10, 
    normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - tmpvar_7.xyz))
  )));
  tmpvar_8.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_16;
  tmpvar_16.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_16.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_17;
  tmpvar_17[0] = _EnvMatrix[0].xyz;
  tmpvar_17[1] = _EnvMatrix[1].xyz;
  tmpvar_17[2] = _EnvMatrix[2].xyz;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((tmpvar_16 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
  highp vec3 tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_20 = tmpvar_1.xyz;
  tmpvar_21 = (((tmpvar_10.yzx * tmpvar_1.zxy) - (tmpvar_10.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_22;
  tmpvar_22[0].x = tmpvar_20.x;
  tmpvar_22[0].y = tmpvar_21.x;
  tmpvar_22[0].z = tmpvar_10.x;
  tmpvar_22[1].x = tmpvar_20.y;
  tmpvar_22[1].y = tmpvar_21.y;
  tmpvar_22[1].z = tmpvar_10.y;
  tmpvar_22[2].x = tmpvar_20.z;
  tmpvar_22[2].y = tmpvar_21.z;
  tmpvar_22[2].z = tmpvar_10.z;
  highp vec3 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_19;
  mediump vec3 tmpvar_25;
  mediump vec4 normal_26;
  normal_26 = tmpvar_24;
  highp float vC_27;
  mediump vec3 x3_28;
  mediump vec3 x2_29;
  mediump vec3 x1_30;
  highp float tmpvar_31;
  tmpvar_31 = dot (unity_SHAr, normal_26);
  x1_30.x = tmpvar_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAg, normal_26);
  x1_30.y = tmpvar_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAb, normal_26);
  x1_30.z = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (normal_26.xyzz * normal_26.yzzx);
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHBr, tmpvar_34);
  x2_29.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBg, tmpvar_34);
  x2_29.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBb, tmpvar_34);
  x2_29.z = tmpvar_37;
  mediump float tmpvar_38;
  tmpvar_38 = ((normal_26.x * normal_26.x) - (normal_26.y * normal_26.y));
  vC_27 = tmpvar_38;
  highp vec3 tmpvar_39;
  tmpvar_39 = (unity_SHC.xyz * vC_27);
  x3_28 = tmpvar_39;
  tmpvar_25 = ((x1_30 + x2_29) + x3_28);
  shlight_3 = tmpvar_25;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * tmpvar_7).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_41 * tmpvar_19.x) + (tmpvar_42 * tmpvar_19.y)) + (tmpvar_43 * tmpvar_19.z))
   * 
    inversesqrt(tmpvar_44)
  )) * (1.0/((1.0 + 
    (tmpvar_44 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_46;
  tmpvar_46 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_45.z)
  ) + (unity_LightColor[3].xyz * tmpvar_45.w)));
  tmpvar_6 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform sampler2D _FaceTex;
uniform highp float _FaceUVSpeedX;
uniform highp float _FaceUVSpeedY;
uniform lowp vec4 _FaceColor;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineUVSpeedX;
uniform highp float _OutlineUVSpeedY;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp float tmpvar_3;
  highp vec4 outlineColor_4;
  highp vec4 faceColor_5;
  highp float c_6;
  lowp float tmpvar_7;
  tmpvar_7 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_6 = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = (((
    (0.5 - c_6)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_9;
  tmpvar_9 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_10;
  tmpvar_10 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_5 = _FaceColor;
  outlineColor_4 = _OutlineColor;
  outlineColor_4.w = (outlineColor_4.w * xlv_COLOR0.w);
  highp vec2 tmpvar_11;
  tmpvar_11.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_11.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_FaceTex, tmpvar_11);
  highp vec4 tmpvar_13;
  tmpvar_13 = ((faceColor_5 * xlv_COLOR0) * tmpvar_12);
  highp vec2 tmpvar_14;
  tmpvar_14.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_14.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture (_OutlineTex, tmpvar_14);
  highp vec4 tmpvar_16;
  tmpvar_16 = (outlineColor_4 * tmpvar_15);
  outlineColor_4 = tmpvar_16;
  mediump float d_17;
  d_17 = tmpvar_8;
  lowp vec4 faceColor_18;
  faceColor_18 = tmpvar_13;
  lowp vec4 outlineColor_19;
  outlineColor_19 = tmpvar_16;
  mediump float outline_20;
  outline_20 = tmpvar_9;
  mediump float softness_21;
  softness_21 = tmpvar_10;
  faceColor_18.xyz = (faceColor_18.xyz * faceColor_18.w);
  outlineColor_19.xyz = (outlineColor_19.xyz * outlineColor_19.w);
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (faceColor_18, outlineColor_19, vec4((clamp (
    (d_17 + (outline_20 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_20)
  ))));
  faceColor_18 = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (faceColor_18 * (1.0 - clamp (
    (((d_17 - (outline_20 * 0.5)) + (softness_21 * 0.5)) / (1.0 + softness_21))
  , 0.0, 1.0)));
  faceColor_18 = tmpvar_23;
  faceColor_5 = faceColor_18;
  faceColor_5.xyz = (faceColor_5.xyz / faceColor_5.w);
  highp vec3 tmpvar_24;
  tmpvar_24 = faceColor_5.xyz;
  tmpvar_2 = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = faceColor_5.w;
  tmpvar_3 = tmpvar_25;
  lowp vec4 c_26;
  c_26.xyz = ((tmpvar_2 * _LightColor0.xyz) * (max (0.0, xlv_TEXCOORD3.z) * 2.0));
  c_26.w = tmpvar_3;
  c_1.w = c_26.w;
  c_1.xyz = (c_26.xyz + (tmpvar_2 * xlv_TEXCOORD4));
  c_1.xyz = c_1.xyz;
  c_1.w = tmpvar_3;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "GLOW_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FaceDilate;
uniform highp mat4 _EnvMatrix;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _FaceTex_ST;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = _glesVertex.zw;
  highp vec2 tmpvar_8;
  tmpvar_7.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_7.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_9).xyz * unity_Scale.w) - tmpvar_7.xyz)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((glstate_matrix_mvp * tmpvar_7).ww / (tmpvar_11 * (tmpvar_12 * _ScreenParams.xy)));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = _WorldSpaceCameraPos;
  tmpvar_8.y = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_10, 
    normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - tmpvar_7.xyz))
  )));
  tmpvar_8.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_16;
  tmpvar_16.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_16.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_17;
  tmpvar_17[0] = _EnvMatrix[0].xyz;
  tmpvar_17[1] = _EnvMatrix[1].xyz;
  tmpvar_17[2] = _EnvMatrix[2].xyz;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((tmpvar_16 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_19 = tmpvar_1.xyz;
  tmpvar_20 = (((tmpvar_10.yzx * tmpvar_1.zxy) - (tmpvar_10.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_21;
  tmpvar_21[0].x = tmpvar_19.x;
  tmpvar_21[0].y = tmpvar_20.x;
  tmpvar_21[0].z = tmpvar_10.x;
  tmpvar_21[1].x = tmpvar_19.y;
  tmpvar_21[1].y = tmpvar_20.y;
  tmpvar_21[1].z = tmpvar_10.y;
  tmpvar_21[2].x = tmpvar_19.z;
  tmpvar_21[2].y = tmpvar_20.z;
  tmpvar_21[2].z = tmpvar_10.z;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
  mediump vec3 tmpvar_24;
  mediump vec4 normal_25;
  normal_25 = tmpvar_23;
  highp float vC_26;
  mediump vec3 x3_27;
  mediump vec3 x2_28;
  mediump vec3 x1_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHAr, normal_25);
  x1_29.x = tmpvar_30;
  highp float tmpvar_31;
  tmpvar_31 = dot (unity_SHAg, normal_25);
  x1_29.y = tmpvar_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAb, normal_25);
  x1_29.z = tmpvar_32;
  mediump vec4 tmpvar_33;
  tmpvar_33 = (normal_25.xyzz * normal_25.yzzx);
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHBr, tmpvar_33);
  x2_28.x = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHBg, tmpvar_33);
  x2_28.y = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBb, tmpvar_33);
  x2_28.z = tmpvar_36;
  mediump float tmpvar_37;
  tmpvar_37 = ((normal_25.x * normal_25.x) - (normal_25.y * normal_25.y));
  vC_26 = tmpvar_37;
  highp vec3 tmpvar_38;
  tmpvar_38 = (unity_SHC.xyz * vC_26);
  x3_27 = tmpvar_38;
  tmpvar_24 = ((x1_29 + x2_28) + x3_27);
  shlight_3 = tmpvar_24;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform sampler2D _FaceTex;
uniform highp float _FaceUVSpeedX;
uniform highp float _FaceUVSpeedY;
uniform lowp vec4 _FaceColor;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineUVSpeedX;
uniform highp float _OutlineUVSpeedY;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp float tmpvar_4;
  highp vec4 glowColor_5;
  highp vec4 outlineColor_6;
  highp vec4 faceColor_7;
  highp float c_8;
  lowp float tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_8 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (((
    (0.5 - c_8)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_11;
  tmpvar_11 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_12;
  tmpvar_12 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_7 = _FaceColor;
  outlineColor_6 = _OutlineColor;
  outlineColor_6.w = (outlineColor_6.w * xlv_COLOR0.w);
  highp vec2 tmpvar_13;
  tmpvar_13.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_13.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_FaceTex, tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15 = ((faceColor_7 * xlv_COLOR0) * tmpvar_14);
  highp vec2 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_16.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_OutlineTex, tmpvar_16);
  highp vec4 tmpvar_18;
  tmpvar_18 = (outlineColor_6 * tmpvar_17);
  outlineColor_6 = tmpvar_18;
  mediump float d_19;
  d_19 = tmpvar_10;
  lowp vec4 faceColor_20;
  faceColor_20 = tmpvar_15;
  lowp vec4 outlineColor_21;
  outlineColor_21 = tmpvar_18;
  mediump float outline_22;
  outline_22 = tmpvar_11;
  mediump float softness_23;
  softness_23 = tmpvar_12;
  faceColor_20.xyz = (faceColor_20.xyz * faceColor_20.w);
  outlineColor_21.xyz = (outlineColor_21.xyz * outlineColor_21.w);
  mediump vec4 tmpvar_24;
  tmpvar_24 = mix (faceColor_20, outlineColor_21, vec4((clamp (
    (d_19 + (outline_22 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_22)
  ))));
  faceColor_20 = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (faceColor_20 * (1.0 - clamp (
    (((d_19 - (outline_22 * 0.5)) + (softness_23 * 0.5)) / (1.0 + softness_23))
  , 0.0, 1.0)));
  faceColor_20 = tmpvar_25;
  faceColor_7 = faceColor_20;
  faceColor_7.xyz = (faceColor_7.xyz / faceColor_7.w);
  highp vec4 tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = (tmpvar_10 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_28;
  tmpvar_28 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_27 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_27 / (1.0 + tmpvar_28)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_28)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_30;
  tmpvar_30.xyz = _GlowColor.xyz;
  tmpvar_30.w = tmpvar_29;
  tmpvar_26 = tmpvar_30;
  glowColor_5.xyz = tmpvar_26.xyz;
  glowColor_5.w = (tmpvar_26.w * xlv_COLOR0.w);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_26.xyz * glowColor_5.w);
  highp vec4 overlying_32;
  overlying_32.w = glowColor_5.w;
  highp vec4 underlying_33;
  underlying_33.w = faceColor_7.w;
  overlying_32.xyz = (tmpvar_26.xyz * glowColor_5.w);
  underlying_33.xyz = (faceColor_7.xyz * faceColor_7.w);
  highp vec3 tmpvar_34;
  tmpvar_34 = (overlying_32.xyz + ((1.0 - glowColor_5.w) * underlying_33.xyz));
  highp float tmpvar_35;
  tmpvar_35 = (faceColor_7.w + ((1.0 - faceColor_7.w) * glowColor_5.w));
  highp vec4 tmpvar_36;
  tmpvar_36.xyz = tmpvar_34;
  tmpvar_36.w = tmpvar_35;
  faceColor_7.w = tmpvar_36.w;
  faceColor_7.xyz = (tmpvar_34 / tmpvar_35);
  highp vec3 tmpvar_37;
  tmpvar_37 = faceColor_7.xyz;
  tmpvar_2 = tmpvar_37;
  tmpvar_3 = tmpvar_31;
  highp float tmpvar_38;
  tmpvar_38 = faceColor_7.w;
  tmpvar_4 = tmpvar_38;
  lowp vec4 c_39;
  c_39.xyz = ((tmpvar_2 * _LightColor0.xyz) * (max (0.0, xlv_TEXCOORD3.z) * 2.0));
  c_39.w = tmpvar_4;
  c_1.w = c_39.w;
  c_1.xyz = (c_39.xyz + (tmpvar_2 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  c_1.w = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "GLOW_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FaceDilate;
uniform highp mat4 _EnvMatrix;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _FaceTex_ST;
out highp vec4 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = _glesVertex.zw;
  highp vec2 tmpvar_8;
  tmpvar_7.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_7.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_9).xyz * unity_Scale.w) - tmpvar_7.xyz)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((glstate_matrix_mvp * tmpvar_7).ww / (tmpvar_11 * (tmpvar_12 * _ScreenParams.xy)));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = _WorldSpaceCameraPos;
  tmpvar_8.y = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_10, 
    normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - tmpvar_7.xyz))
  )));
  tmpvar_8.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_16;
  tmpvar_16.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_16.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_17;
  tmpvar_17[0] = _EnvMatrix[0].xyz;
  tmpvar_17[1] = _EnvMatrix[1].xyz;
  tmpvar_17[2] = _EnvMatrix[2].xyz;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((tmpvar_16 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_19 = tmpvar_1.xyz;
  tmpvar_20 = (((tmpvar_10.yzx * tmpvar_1.zxy) - (tmpvar_10.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_21;
  tmpvar_21[0].x = tmpvar_19.x;
  tmpvar_21[0].y = tmpvar_20.x;
  tmpvar_21[0].z = tmpvar_10.x;
  tmpvar_21[1].x = tmpvar_19.y;
  tmpvar_21[1].y = tmpvar_20.y;
  tmpvar_21[1].z = tmpvar_10.y;
  tmpvar_21[2].x = tmpvar_19.z;
  tmpvar_21[2].y = tmpvar_20.z;
  tmpvar_21[2].z = tmpvar_10.z;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
  mediump vec3 tmpvar_24;
  mediump vec4 normal_25;
  normal_25 = tmpvar_23;
  highp float vC_26;
  mediump vec3 x3_27;
  mediump vec3 x2_28;
  mediump vec3 x1_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (unity_SHAr, normal_25);
  x1_29.x = tmpvar_30;
  highp float tmpvar_31;
  tmpvar_31 = dot (unity_SHAg, normal_25);
  x1_29.y = tmpvar_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAb, normal_25);
  x1_29.z = tmpvar_32;
  mediump vec4 tmpvar_33;
  tmpvar_33 = (normal_25.xyzz * normal_25.yzzx);
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHBr, tmpvar_33);
  x2_28.x = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHBg, tmpvar_33);
  x2_28.y = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBb, tmpvar_33);
  x2_28.z = tmpvar_36;
  mediump float tmpvar_37;
  tmpvar_37 = ((normal_25.x * normal_25.x) - (normal_25.y * normal_25.y));
  vC_26 = tmpvar_37;
  highp vec3 tmpvar_38;
  tmpvar_38 = (unity_SHC.xyz * vC_26);
  x3_27 = tmpvar_38;
  tmpvar_24 = ((x1_29 + x2_28) + x3_27);
  shlight_3 = tmpvar_24;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform sampler2D _FaceTex;
uniform highp float _FaceUVSpeedX;
uniform highp float _FaceUVSpeedY;
uniform lowp vec4 _FaceColor;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineUVSpeedX;
uniform highp float _OutlineUVSpeedY;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp float tmpvar_4;
  highp vec4 glowColor_5;
  highp vec4 outlineColor_6;
  highp vec4 faceColor_7;
  highp float c_8;
  lowp float tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_8 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (((
    (0.5 - c_8)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_11;
  tmpvar_11 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_12;
  tmpvar_12 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_7 = _FaceColor;
  outlineColor_6 = _OutlineColor;
  outlineColor_6.w = (outlineColor_6.w * xlv_COLOR0.w);
  highp vec2 tmpvar_13;
  tmpvar_13.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_13.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_FaceTex, tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15 = ((faceColor_7 * xlv_COLOR0) * tmpvar_14);
  highp vec2 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_16.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_OutlineTex, tmpvar_16);
  highp vec4 tmpvar_18;
  tmpvar_18 = (outlineColor_6 * tmpvar_17);
  outlineColor_6 = tmpvar_18;
  mediump float d_19;
  d_19 = tmpvar_10;
  lowp vec4 faceColor_20;
  faceColor_20 = tmpvar_15;
  lowp vec4 outlineColor_21;
  outlineColor_21 = tmpvar_18;
  mediump float outline_22;
  outline_22 = tmpvar_11;
  mediump float softness_23;
  softness_23 = tmpvar_12;
  faceColor_20.xyz = (faceColor_20.xyz * faceColor_20.w);
  outlineColor_21.xyz = (outlineColor_21.xyz * outlineColor_21.w);
  mediump vec4 tmpvar_24;
  tmpvar_24 = mix (faceColor_20, outlineColor_21, vec4((clamp (
    (d_19 + (outline_22 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_22)
  ))));
  faceColor_20 = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (faceColor_20 * (1.0 - clamp (
    (((d_19 - (outline_22 * 0.5)) + (softness_23 * 0.5)) / (1.0 + softness_23))
  , 0.0, 1.0)));
  faceColor_20 = tmpvar_25;
  faceColor_7 = faceColor_20;
  faceColor_7.xyz = (faceColor_7.xyz / faceColor_7.w);
  highp vec4 tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = (tmpvar_10 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_28;
  tmpvar_28 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_27 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_27 / (1.0 + tmpvar_28)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_28)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_30;
  tmpvar_30.xyz = _GlowColor.xyz;
  tmpvar_30.w = tmpvar_29;
  tmpvar_26 = tmpvar_30;
  glowColor_5.xyz = tmpvar_26.xyz;
  glowColor_5.w = (tmpvar_26.w * xlv_COLOR0.w);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_26.xyz * glowColor_5.w);
  highp vec4 overlying_32;
  overlying_32.w = glowColor_5.w;
  highp vec4 underlying_33;
  underlying_33.w = faceColor_7.w;
  overlying_32.xyz = (tmpvar_26.xyz * glowColor_5.w);
  underlying_33.xyz = (faceColor_7.xyz * faceColor_7.w);
  highp vec3 tmpvar_34;
  tmpvar_34 = (overlying_32.xyz + ((1.0 - glowColor_5.w) * underlying_33.xyz));
  highp float tmpvar_35;
  tmpvar_35 = (faceColor_7.w + ((1.0 - faceColor_7.w) * glowColor_5.w));
  highp vec4 tmpvar_36;
  tmpvar_36.xyz = tmpvar_34;
  tmpvar_36.w = tmpvar_35;
  faceColor_7.w = tmpvar_36.w;
  faceColor_7.xyz = (tmpvar_34 / tmpvar_35);
  highp vec3 tmpvar_37;
  tmpvar_37 = faceColor_7.xyz;
  tmpvar_2 = tmpvar_37;
  tmpvar_3 = tmpvar_31;
  highp float tmpvar_38;
  tmpvar_38 = faceColor_7.w;
  tmpvar_4 = tmpvar_38;
  lowp vec4 c_39;
  c_39.xyz = ((tmpvar_2 * _LightColor0.xyz) * (max (0.0, xlv_TEXCOORD3.z) * 2.0));
  c_39.w = tmpvar_4;
  c_1.w = c_39.w;
  c_1.xyz = (c_39.xyz + (tmpvar_2 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  c_1.w = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" "GLOW_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FaceDilate;
uniform highp mat4 _EnvMatrix;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _FaceTex_ST;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = _glesVertex.zw;
  highp vec2 tmpvar_8;
  tmpvar_7.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_7.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_9).xyz * unity_Scale.w) - tmpvar_7.xyz)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((glstate_matrix_mvp * tmpvar_7).ww / (tmpvar_11 * (tmpvar_12 * _ScreenParams.xy)));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = _WorldSpaceCameraPos;
  tmpvar_8.y = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_10, 
    normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - tmpvar_7.xyz))
  )));
  tmpvar_8.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_16;
  tmpvar_16.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_16.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_17;
  tmpvar_17[0] = _EnvMatrix[0].xyz;
  tmpvar_17[1] = _EnvMatrix[1].xyz;
  tmpvar_17[2] = _EnvMatrix[2].xyz;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((tmpvar_16 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
  highp vec3 tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_20 = tmpvar_1.xyz;
  tmpvar_21 = (((tmpvar_10.yzx * tmpvar_1.zxy) - (tmpvar_10.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_22;
  tmpvar_22[0].x = tmpvar_20.x;
  tmpvar_22[0].y = tmpvar_21.x;
  tmpvar_22[0].z = tmpvar_10.x;
  tmpvar_22[1].x = tmpvar_20.y;
  tmpvar_22[1].y = tmpvar_21.y;
  tmpvar_22[1].z = tmpvar_10.y;
  tmpvar_22[2].x = tmpvar_20.z;
  tmpvar_22[2].y = tmpvar_21.z;
  tmpvar_22[2].z = tmpvar_10.z;
  highp vec3 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_19;
  mediump vec3 tmpvar_25;
  mediump vec4 normal_26;
  normal_26 = tmpvar_24;
  highp float vC_27;
  mediump vec3 x3_28;
  mediump vec3 x2_29;
  mediump vec3 x1_30;
  highp float tmpvar_31;
  tmpvar_31 = dot (unity_SHAr, normal_26);
  x1_30.x = tmpvar_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAg, normal_26);
  x1_30.y = tmpvar_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAb, normal_26);
  x1_30.z = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (normal_26.xyzz * normal_26.yzzx);
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHBr, tmpvar_34);
  x2_29.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBg, tmpvar_34);
  x2_29.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBb, tmpvar_34);
  x2_29.z = tmpvar_37;
  mediump float tmpvar_38;
  tmpvar_38 = ((normal_26.x * normal_26.x) - (normal_26.y * normal_26.y));
  vC_27 = tmpvar_38;
  highp vec3 tmpvar_39;
  tmpvar_39 = (unity_SHC.xyz * vC_27);
  x3_28 = tmpvar_39;
  tmpvar_25 = ((x1_30 + x2_29) + x3_28);
  shlight_3 = tmpvar_25;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * tmpvar_7).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_41 * tmpvar_19.x) + (tmpvar_42 * tmpvar_19.y)) + (tmpvar_43 * tmpvar_19.z))
   * 
    inversesqrt(tmpvar_44)
  )) * (1.0/((1.0 + 
    (tmpvar_44 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_46;
  tmpvar_46 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_45.z)
  ) + (unity_LightColor[3].xyz * tmpvar_45.w)));
  tmpvar_6 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform sampler2D _FaceTex;
uniform highp float _FaceUVSpeedX;
uniform highp float _FaceUVSpeedY;
uniform lowp vec4 _FaceColor;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineUVSpeedX;
uniform highp float _OutlineUVSpeedY;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp float tmpvar_4;
  highp vec4 glowColor_5;
  highp vec4 outlineColor_6;
  highp vec4 faceColor_7;
  highp float c_8;
  lowp float tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_8 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (((
    (0.5 - c_8)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_11;
  tmpvar_11 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_12;
  tmpvar_12 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_7 = _FaceColor;
  outlineColor_6 = _OutlineColor;
  outlineColor_6.w = (outlineColor_6.w * xlv_COLOR0.w);
  highp vec2 tmpvar_13;
  tmpvar_13.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_13.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_FaceTex, tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15 = ((faceColor_7 * xlv_COLOR0) * tmpvar_14);
  highp vec2 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_16.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_OutlineTex, tmpvar_16);
  highp vec4 tmpvar_18;
  tmpvar_18 = (outlineColor_6 * tmpvar_17);
  outlineColor_6 = tmpvar_18;
  mediump float d_19;
  d_19 = tmpvar_10;
  lowp vec4 faceColor_20;
  faceColor_20 = tmpvar_15;
  lowp vec4 outlineColor_21;
  outlineColor_21 = tmpvar_18;
  mediump float outline_22;
  outline_22 = tmpvar_11;
  mediump float softness_23;
  softness_23 = tmpvar_12;
  faceColor_20.xyz = (faceColor_20.xyz * faceColor_20.w);
  outlineColor_21.xyz = (outlineColor_21.xyz * outlineColor_21.w);
  mediump vec4 tmpvar_24;
  tmpvar_24 = mix (faceColor_20, outlineColor_21, vec4((clamp (
    (d_19 + (outline_22 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_22)
  ))));
  faceColor_20 = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (faceColor_20 * (1.0 - clamp (
    (((d_19 - (outline_22 * 0.5)) + (softness_23 * 0.5)) / (1.0 + softness_23))
  , 0.0, 1.0)));
  faceColor_20 = tmpvar_25;
  faceColor_7 = faceColor_20;
  faceColor_7.xyz = (faceColor_7.xyz / faceColor_7.w);
  highp vec4 tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = (tmpvar_10 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_28;
  tmpvar_28 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_27 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_27 / (1.0 + tmpvar_28)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_28)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_30;
  tmpvar_30.xyz = _GlowColor.xyz;
  tmpvar_30.w = tmpvar_29;
  tmpvar_26 = tmpvar_30;
  glowColor_5.xyz = tmpvar_26.xyz;
  glowColor_5.w = (tmpvar_26.w * xlv_COLOR0.w);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_26.xyz * glowColor_5.w);
  highp vec4 overlying_32;
  overlying_32.w = glowColor_5.w;
  highp vec4 underlying_33;
  underlying_33.w = faceColor_7.w;
  overlying_32.xyz = (tmpvar_26.xyz * glowColor_5.w);
  underlying_33.xyz = (faceColor_7.xyz * faceColor_7.w);
  highp vec3 tmpvar_34;
  tmpvar_34 = (overlying_32.xyz + ((1.0 - glowColor_5.w) * underlying_33.xyz));
  highp float tmpvar_35;
  tmpvar_35 = (faceColor_7.w + ((1.0 - faceColor_7.w) * glowColor_5.w));
  highp vec4 tmpvar_36;
  tmpvar_36.xyz = tmpvar_34;
  tmpvar_36.w = tmpvar_35;
  faceColor_7.w = tmpvar_36.w;
  faceColor_7.xyz = (tmpvar_34 / tmpvar_35);
  highp vec3 tmpvar_37;
  tmpvar_37 = faceColor_7.xyz;
  tmpvar_2 = tmpvar_37;
  tmpvar_3 = tmpvar_31;
  highp float tmpvar_38;
  tmpvar_38 = faceColor_7.w;
  tmpvar_4 = tmpvar_38;
  lowp vec4 c_39;
  c_39.xyz = ((tmpvar_2 * _LightColor0.xyz) * (max (0.0, xlv_TEXCOORD3.z) * 2.0));
  c_39.w = tmpvar_4;
  c_1.w = c_39.w;
  c_1.xyz = (c_39.xyz + (tmpvar_2 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  c_1.w = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" "GLOW_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ScreenParams;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FaceDilate;
uniform highp mat4 _EnvMatrix;
uniform highp float _WeightNormal;
uniform highp float _WeightBold;
uniform highp float _ScaleRatioA;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp float _GradientScale;
uniform highp float _ScaleX;
uniform highp float _ScaleY;
uniform highp float _PerspectiveFilter;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _FaceTex_ST;
out highp vec4 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR0;
out highp vec2 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = _glesVertex.zw;
  highp vec2 tmpvar_8;
  tmpvar_7.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_7.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_9).xyz * unity_Scale.w) - tmpvar_7.xyz)
  )));
  highp vec2 tmpvar_11;
  tmpvar_11.x = _ScaleX;
  tmpvar_11.y = _ScaleY;
  highp mat2 tmpvar_12;
  tmpvar_12[0] = glstate_matrix_projection[0].xy;
  tmpvar_12[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((glstate_matrix_mvp * tmpvar_7).ww / (tmpvar_11 * (tmpvar_12 * _ScreenParams.xy)));
  highp float tmpvar_14;
  tmpvar_14 = (inversesqrt(dot (tmpvar_13, tmpvar_13)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = _WorldSpaceCameraPos;
  tmpvar_8.y = mix ((tmpvar_14 * (1.0 - _PerspectiveFilter)), tmpvar_14, abs(dot (tmpvar_10, 
    normalize((((_World2Object * tmpvar_15).xyz * unity_Scale.w) - tmpvar_7.xyz))
  )));
  tmpvar_8.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_16;
  tmpvar_16.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_16.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_17;
  tmpvar_17[0] = _EnvMatrix[0].xyz;
  tmpvar_17[1] = _EnvMatrix[1].xyz;
  tmpvar_17[2] = _EnvMatrix[2].xyz;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((tmpvar_16 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
  highp vec3 tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_20 = tmpvar_1.xyz;
  tmpvar_21 = (((tmpvar_10.yzx * tmpvar_1.zxy) - (tmpvar_10.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_22;
  tmpvar_22[0].x = tmpvar_20.x;
  tmpvar_22[0].y = tmpvar_21.x;
  tmpvar_22[0].z = tmpvar_10.x;
  tmpvar_22[1].x = tmpvar_20.y;
  tmpvar_22[1].y = tmpvar_21.y;
  tmpvar_22[1].z = tmpvar_10.y;
  tmpvar_22[2].x = tmpvar_20.z;
  tmpvar_22[2].y = tmpvar_21.z;
  tmpvar_22[2].z = tmpvar_10.z;
  highp vec3 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_19;
  mediump vec3 tmpvar_25;
  mediump vec4 normal_26;
  normal_26 = tmpvar_24;
  highp float vC_27;
  mediump vec3 x3_28;
  mediump vec3 x2_29;
  mediump vec3 x1_30;
  highp float tmpvar_31;
  tmpvar_31 = dot (unity_SHAr, normal_26);
  x1_30.x = tmpvar_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAg, normal_26);
  x1_30.y = tmpvar_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAb, normal_26);
  x1_30.z = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (normal_26.xyzz * normal_26.yzzx);
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHBr, tmpvar_34);
  x2_29.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBg, tmpvar_34);
  x2_29.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBb, tmpvar_34);
  x2_29.z = tmpvar_37;
  mediump float tmpvar_38;
  tmpvar_38 = ((normal_26.x * normal_26.x) - (normal_26.y * normal_26.y));
  vC_27 = tmpvar_38;
  highp vec3 tmpvar_39;
  tmpvar_39 = (unity_SHC.xyz * vC_27);
  x3_28 = tmpvar_39;
  tmpvar_25 = ((x1_30 + x2_29) + x3_28);
  shlight_3 = tmpvar_25;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_40;
  tmpvar_40 = (_Object2World * tmpvar_7).xyz;
  highp vec4 tmpvar_41;
  tmpvar_41 = (unity_4LightPosX0 - tmpvar_40.x);
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosY0 - tmpvar_40.y);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosZ0 - tmpvar_40.z);
  highp vec4 tmpvar_44;
  tmpvar_44 = (((tmpvar_41 * tmpvar_41) + (tmpvar_42 * tmpvar_42)) + (tmpvar_43 * tmpvar_43));
  highp vec4 tmpvar_45;
  tmpvar_45 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_41 * tmpvar_19.x) + (tmpvar_42 * tmpvar_19.y)) + (tmpvar_43 * tmpvar_19.z))
   * 
    inversesqrt(tmpvar_44)
  )) * (1.0/((1.0 + 
    (tmpvar_44 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_46;
  tmpvar_46 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_45.x) + (unity_LightColor[1].xyz * tmpvar_45.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_45.z)
  ) + (unity_LightColor[3].xyz * tmpvar_45.w)));
  tmpvar_6 = tmpvar_46;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform sampler2D _FaceTex;
uniform highp float _FaceUVSpeedX;
uniform highp float _FaceUVSpeedY;
uniform lowp vec4 _FaceColor;
uniform highp float _OutlineSoftness;
uniform sampler2D _OutlineTex;
uniform highp float _OutlineUVSpeedX;
uniform highp float _OutlineUVSpeedY;
uniform lowp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp float tmpvar_4;
  highp vec4 glowColor_5;
  highp vec4 outlineColor_6;
  highp vec4 faceColor_7;
  highp float c_8;
  lowp float tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_8 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (((
    (0.5 - c_8)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_11;
  tmpvar_11 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_12;
  tmpvar_12 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_7 = _FaceColor;
  outlineColor_6 = _OutlineColor;
  outlineColor_6.w = (outlineColor_6.w * xlv_COLOR0.w);
  highp vec2 tmpvar_13;
  tmpvar_13.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_13.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_FaceTex, tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15 = ((faceColor_7 * xlv_COLOR0) * tmpvar_14);
  highp vec2 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_16.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_OutlineTex, tmpvar_16);
  highp vec4 tmpvar_18;
  tmpvar_18 = (outlineColor_6 * tmpvar_17);
  outlineColor_6 = tmpvar_18;
  mediump float d_19;
  d_19 = tmpvar_10;
  lowp vec4 faceColor_20;
  faceColor_20 = tmpvar_15;
  lowp vec4 outlineColor_21;
  outlineColor_21 = tmpvar_18;
  mediump float outline_22;
  outline_22 = tmpvar_11;
  mediump float softness_23;
  softness_23 = tmpvar_12;
  faceColor_20.xyz = (faceColor_20.xyz * faceColor_20.w);
  outlineColor_21.xyz = (outlineColor_21.xyz * outlineColor_21.w);
  mediump vec4 tmpvar_24;
  tmpvar_24 = mix (faceColor_20, outlineColor_21, vec4((clamp (
    (d_19 + (outline_22 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_22)
  ))));
  faceColor_20 = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = (faceColor_20 * (1.0 - clamp (
    (((d_19 - (outline_22 * 0.5)) + (softness_23 * 0.5)) / (1.0 + softness_23))
  , 0.0, 1.0)));
  faceColor_20 = tmpvar_25;
  faceColor_7 = faceColor_20;
  faceColor_7.xyz = (faceColor_7.xyz / faceColor_7.w);
  highp vec4 tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = (tmpvar_10 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_28;
  tmpvar_28 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_27 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_29;
  tmpvar_29 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_27 / (1.0 + tmpvar_28)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_28)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_30;
  tmpvar_30.xyz = _GlowColor.xyz;
  tmpvar_30.w = tmpvar_29;
  tmpvar_26 = tmpvar_30;
  glowColor_5.xyz = tmpvar_26.xyz;
  glowColor_5.w = (tmpvar_26.w * xlv_COLOR0.w);
  highp vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_26.xyz * glowColor_5.w);
  highp vec4 overlying_32;
  overlying_32.w = glowColor_5.w;
  highp vec4 underlying_33;
  underlying_33.w = faceColor_7.w;
  overlying_32.xyz = (tmpvar_26.xyz * glowColor_5.w);
  underlying_33.xyz = (faceColor_7.xyz * faceColor_7.w);
  highp vec3 tmpvar_34;
  tmpvar_34 = (overlying_32.xyz + ((1.0 - glowColor_5.w) * underlying_33.xyz));
  highp float tmpvar_35;
  tmpvar_35 = (faceColor_7.w + ((1.0 - faceColor_7.w) * glowColor_5.w));
  highp vec4 tmpvar_36;
  tmpvar_36.xyz = tmpvar_34;
  tmpvar_36.w = tmpvar_35;
  faceColor_7.w = tmpvar_36.w;
  faceColor_7.xyz = (tmpvar_34 / tmpvar_35);
  highp vec3 tmpvar_37;
  tmpvar_37 = faceColor_7.xyz;
  tmpvar_2 = tmpvar_37;
  tmpvar_3 = tmpvar_31;
  highp float tmpvar_38;
  tmpvar_38 = faceColor_7.w;
  tmpvar_4 = tmpvar_38;
  lowp vec4 c_39;
  c_39.xyz = ((tmpvar_2 * _LightColor0.xyz) * (max (0.0, xlv_TEXCOORD3.z) * 2.0));
  c_39.w = tmpvar_4;
  c_1.w = c_39.w;
  c_1.xyz = (c_39.xyz + (tmpvar_2 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  c_1.w = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "GLOW_ON" }
"!!GLES3"
}
}
 }
 Pass {
  Name "CASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  Cull Off
  Fog { Mode Off }
  AlphaTest Greater 0
  ColorMask RGB
  Offset 1, 1
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp float _OutlineWidth;
uniform highp float _FaceDilate;
uniform highp float _ScaleRatioA;
varying highp vec2 xlv_TEXCOORD1;
varying highp float xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xyw = tmpvar_2.xyw;
  tmpvar_1.z = (tmpvar_2.z + unity_LightShadowBias.x);
  tmpvar_1.z = mix (tmpvar_1.z, max (tmpvar_1.z, -(tmpvar_2.w)), unity_LightShadowBias.y);
  gl_Position = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD2 = (((1.0 - 
    (_OutlineWidth * _ScaleRatioA)
  ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD1;
varying highp float xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD1).wwww;
  highp float x_2;
  x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
  if ((x_2 < 0.0)) {
    discard;
  };
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}



#endif"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp float _OutlineWidth;
uniform highp float _FaceDilate;
uniform highp float _ScaleRatioA;
out highp vec2 xlv_TEXCOORD1;
out highp float xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xyw = tmpvar_2.xyw;
  tmpvar_1.z = (tmpvar_2.z + unity_LightShadowBias.x);
  tmpvar_1.z = mix (tmpvar_1.z, max (tmpvar_1.z, -(tmpvar_2.w)), unity_LightShadowBias.y);
  gl_Position = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD2 = (((1.0 - 
    (_OutlineWidth * _ScaleRatioA)
  ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD1;
in highp float xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture (_MainTex, xlv_TEXCOORD1).wwww;
  highp float x_2;
  x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
  if ((x_2 < 0.0)) {
    discard;
  };
  _glesFragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}



#endif"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _OutlineWidth;
uniform highp float _FaceDilate;
uniform highp float _ScaleRatioA;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp float xlv_TEXCOORD2;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_Object2World * _glesVertex).xyz - _LightPositionRange.xyz);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD2 = (((1.0 - 
    (_OutlineWidth * _ScaleRatioA)
  ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _LightPositionRange;
uniform sampler2D _MainTex;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp float xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD1).wwww;
  highp float x_2;
  x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
  if ((x_2 < 0.0)) {
    discard;
  };
  highp vec4 tmpvar_3;
  tmpvar_3 = fract((vec4(1.0, 255.0, 65025.0, 1.65814e+07) * min (
    (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)
  , 0.999)));
  highp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 - (tmpvar_3.yzww * 0.00392157));
  gl_FragData[0] = tmpvar_4;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_CUBE" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp float _OutlineWidth;
uniform highp float _FaceDilate;
uniform highp float _ScaleRatioA;
out highp vec3 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp float xlv_TEXCOORD2;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_Object2World * _glesVertex).xyz - _LightPositionRange.xyz);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD2 = (((1.0 - 
    (_OutlineWidth * _ScaleRatioA)
  ) - (_FaceDilate * _ScaleRatioA)) / 2.0);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _LightPositionRange;
uniform sampler2D _MainTex;
in highp vec3 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp float xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture (_MainTex, xlv_TEXCOORD1).wwww;
  highp float x_2;
  x_2 = (tmpvar_1.w - xlv_TEXCOORD2);
  if ((x_2 < 0.0)) {
    discard;
  };
  highp vec4 tmpvar_3;
  tmpvar_3 = fract((vec4(1.0, 255.0, 65025.0, 1.65814e+07) * min (
    (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)
  , 0.999)));
  highp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 - (tmpvar_3.yzww * 0.00392157));
  _glesFragData[0] = tmpvar_4;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_CUBE" }
"!!GLES3"
}
}
 }
}
}