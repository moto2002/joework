Shader "S_Game_Hero/Hero_Show2" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _MaskTex ("Mask (R,G,B)", 2D) = "white" {}
 _SpecColor ("Spec Color", Color) = (0,0,0,0)
 _SpecPower ("Spec Power", Range(1,128)) = 15
 _SpecMultiplier ("Spec Multiplier", Float) = 1
 _RampMap ("Ramp Map", 2D) = "white" {}
 _AmbientColor ("Ambient", Color) = (0.2,0.2,0.2,0)
 _ShadowColor ("Shadow Color", Color) = (0,0,0,0)
 _LightTex ("轮廓光 (RGB)", 2D) = "white" {}
 _NormalTex ("Normal", 2D) = "bump" {}
 _NoiseTex ("Noise(RGB)", 2D) = "white" {}
 _Scroll2X ("Noise speed X", Float) = 1
 _Scroll2Y ("Noise speed Y", Float) = 0
 _NoiseColor ("Noise Color", Color) = (1,1,1,1)
 _MMultiplier ("Layer Multiplier", Float) = 2
}
SubShader { 
 LOD 200
 Tags { "RenderType"="Opaque" "HeroShader"="1" }
 Pass {
  Tags { "RenderType"="Opaque" "HeroShader"="1" }
  Fog { Mode Off }
Program "vp" {
SubProgram "gles " {
Keywords { "_NORMALMAP_OFF" "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _SGameShadowParams;
uniform highp vec4 _MainTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  tmpvar_3.zw = tmpvar_3.xy;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_2;
  highp vec3 tmpvar_9;
  tmpvar_9 = (glstate_matrix_modelview0 * tmpvar_8).xyz;
  tmpvar_4 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_2);
  highp vec3 tmpvar_12;
  tmpvar_12 = (((tmpvar_11.yzx * tmpvar_10.zxy) - (tmpvar_11.zxy * tmpvar_10.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_10.x;
  tmpvar_13[0].y = tmpvar_12.x;
  tmpvar_13[0].z = tmpvar_11.x;
  tmpvar_13[1].x = tmpvar_10.y;
  tmpvar_13[1].y = tmpvar_12.y;
  tmpvar_13[1].z = tmpvar_11.y;
  tmpvar_13[2].x = tmpvar_10.z;
  tmpvar_13[2].y = tmpvar_12.z;
  tmpvar_13[2].z = tmpvar_11.z;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 0.0;
  tmpvar_14.xyz = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((tmpvar_13 * (_World2Object * tmpvar_14).xyz));
  tmpvar_5 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_13 * (_World2Object * tmpvar_16).xyz));
  tmpvar_6 = tmpvar_17;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _RampMap;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
void main ()
{
  mediump float gloss_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec3 tmpvar_4;
  tmpvar_4 = ((tmpvar_3.xyz * (texture2D (_LightTex, tmpvar_2) * 2.0).xyz) + tmpvar_3.xyz);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MaskTex, xlv_TEXCOORD0.xy).x;
  gloss_1 = tmpvar_5;
  mediump vec3 albedo_6;
  albedo_6 = tmpvar_4;
  lowp vec3 color_7;
  mediump vec3 ramp_8;
  highp float nh_9;
  lowp float diff_10;
  mediump float tmpvar_11;
  tmpvar_11 = ((xlv_TEXCOORD3.z * 0.5) + 0.5);
  diff_10 = tmpvar_11;
  mediump float tmpvar_12;
  tmpvar_12 = max (0.0, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD2)).z);
  nh_9 = tmpvar_12;
  lowp vec2 tmpvar_13;
  tmpvar_13.y = 0.5;
  tmpvar_13.x = diff_10;
  lowp vec3 tmpvar_14;
  tmpvar_14 = texture2D (_RampMap, tmpvar_13).xyz;
  ramp_8 = tmpvar_14;
  highp vec3 tmpvar_15;
  tmpvar_15 = ((_SpecColor * (
    ((pow (nh_9, _SpecPower) * gloss_1) * _SpecMultiplier)
   * 2.0)) + ((ramp_8 + _AmbientColor) * albedo_6));
  color_7 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16.xyz = color_7;
  tmpvar_16.w = tmpvar_3.w;
  gl_FragData[0] = tmpvar_16;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_NORMALMAP_OFF" "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _SGameShadowParams;
uniform highp vec4 _MainTex_ST;
out mediump vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out mediump vec3 xlv_TEXCOORD3;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  tmpvar_3.zw = tmpvar_3.xy;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_2;
  highp vec3 tmpvar_9;
  tmpvar_9 = (glstate_matrix_modelview0 * tmpvar_8).xyz;
  tmpvar_4 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_2);
  highp vec3 tmpvar_12;
  tmpvar_12 = (((tmpvar_11.yzx * tmpvar_10.zxy) - (tmpvar_11.zxy * tmpvar_10.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_10.x;
  tmpvar_13[0].y = tmpvar_12.x;
  tmpvar_13[0].z = tmpvar_11.x;
  tmpvar_13[1].x = tmpvar_10.y;
  tmpvar_13[1].y = tmpvar_12.y;
  tmpvar_13[1].z = tmpvar_11.y;
  tmpvar_13[2].x = tmpvar_10.z;
  tmpvar_13[2].y = tmpvar_12.z;
  tmpvar_13[2].z = tmpvar_11.z;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 0.0;
  tmpvar_14.xyz = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((tmpvar_13 * (_World2Object * tmpvar_14).xyz));
  tmpvar_5 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_13 * (_World2Object * tmpvar_16).xyz));
  tmpvar_6 = tmpvar_17;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _RampMap;
in mediump vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in mediump vec3 xlv_TEXCOORD3;
void main ()
{
  mediump float gloss_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec3 tmpvar_4;
  tmpvar_4 = ((tmpvar_3.xyz * (texture (_LightTex, tmpvar_2) * 2.0).xyz) + tmpvar_3.xyz);
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MaskTex, xlv_TEXCOORD0.xy).x;
  gloss_1 = tmpvar_5;
  mediump vec3 albedo_6;
  albedo_6 = tmpvar_4;
  lowp vec3 color_7;
  mediump vec3 ramp_8;
  highp float nh_9;
  lowp float diff_10;
  mediump float tmpvar_11;
  tmpvar_11 = ((xlv_TEXCOORD3.z * 0.5) + 0.5);
  diff_10 = tmpvar_11;
  mediump float tmpvar_12;
  tmpvar_12 = max (0.0, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD2)).z);
  nh_9 = tmpvar_12;
  lowp vec2 tmpvar_13;
  tmpvar_13.y = 0.5;
  tmpvar_13.x = diff_10;
  lowp vec3 tmpvar_14;
  tmpvar_14 = texture (_RampMap, tmpvar_13).xyz;
  ramp_8 = tmpvar_14;
  highp vec3 tmpvar_15;
  tmpvar_15 = ((_SpecColor * (
    ((pow (nh_9, _SpecPower) * gloss_1) * _SpecMultiplier)
   * 2.0)) + ((ramp_8 + _AmbientColor) * albedo_6));
  color_7 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16.xyz = color_7;
  tmpvar_16.w = tmpvar_3.w;
  _glesFragData[0] = tmpvar_16;
}



#endif"
}
SubProgram "gles " {
Keywords { "_NORMALMAP_OFF" "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _SGameShadowParams;
uniform highp mat4 _SGameShadowMatrix;
uniform highp vec4 _MainTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  tmpvar_3.zw = tmpvar_3.xy;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_2;
  highp vec3 tmpvar_9;
  tmpvar_9 = (glstate_matrix_modelview0 * tmpvar_8).xyz;
  tmpvar_4 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_2);
  highp vec3 tmpvar_13;
  tmpvar_13 = (((tmpvar_12.yzx * tmpvar_11.zxy) - (tmpvar_12.zxy * tmpvar_11.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_11.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_12.x;
  tmpvar_14[1].x = tmpvar_11.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_12.y;
  tmpvar_14[2].x = tmpvar_11.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_12.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 0.0;
  tmpvar_15.xyz = (_WorldSpaceCameraPos - tmpvar_10.xyz);
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_14 * (_World2Object * tmpvar_15).xyz));
  tmpvar_5 = tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 0.0;
  tmpvar_17.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_14 * (_World2Object * tmpvar_17).xyz));
  tmpvar_6 = tmpvar_18;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (_SGameShadowMatrix * tmpvar_10);
}



#endif
#ifdef FRAGMENT

uniform mediump vec4 _ShadowColor;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _RampMap;
uniform sampler2D _SGameShadowTexture;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float gloss_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec3 tmpvar_4;
  tmpvar_4 = ((tmpvar_3.xyz * (texture2D (_LightTex, tmpvar_2) * 2.0).xyz) + tmpvar_3.xyz);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_MaskTex, xlv_TEXCOORD0.xy).x;
  gloss_1 = tmpvar_5;
  lowp float tmpvar_6;
  highp vec3 coord_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = (((xlv_TEXCOORD4.xyz / xlv_TEXCOORD4.w) * 0.5) + 0.5);
  coord_7.xy = tmpvar_8.xy;
  coord_7.z = (tmpvar_8.z - 0.0002);
  highp vec4 c_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_SGameShadowTexture, tmpvar_8.xy);
  c_9 = tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = clamp ((2.0 - exp(
    ((coord_7.z - dot (c_9, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0)
  )), 0.0, 1.0);
  coord_7.xy = (tmpvar_8.xy + vec2(0.000976563, 0.000976563));
  highp vec4 c_12;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_SGameShadowTexture, coord_7.xy);
  c_12 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (tmpvar_11 + clamp ((2.0 - 
    exp(((coord_7.z - dot (c_12, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))
  ), 0.0, 1.0));
  tmpvar_6 = ((tmpvar_14 * 0.25) + 0.5);
  mediump vec3 albedo_15;
  albedo_15 = tmpvar_4;
  lowp vec3 color_16;
  lowp vec3 darkColor_17;
  mediump vec3 ramp_18;
  highp float nh_19;
  lowp float diff_20;
  mediump float tmpvar_21;
  tmpvar_21 = ((xlv_TEXCOORD3.z * 0.5) + 0.5);
  diff_20 = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = max (0.0, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD2)).z);
  nh_19 = tmpvar_22;
  lowp vec2 tmpvar_23;
  tmpvar_23.y = 0.5;
  tmpvar_23.x = diff_20;
  lowp vec3 tmpvar_24;
  tmpvar_24 = texture2D (_RampMap, tmpvar_23).xyz;
  ramp_18 = tmpvar_24;
  mediump vec3 tmpvar_25;
  tmpvar_25 = (albedo_15 * _ShadowColor.xyz);
  darkColor_17 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = ((_SpecColor * (
    (((pow (nh_19, _SpecPower) * gloss_1) * _SpecMultiplier) * tmpvar_6)
   * 2.0)) + ((ramp_18 + _AmbientColor) * albedo_15));
  color_16 = tmpvar_26;
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = mix (darkColor_17, color_16, vec3(tmpvar_6));
  tmpvar_27.w = tmpvar_3.w;
  gl_FragData[0] = tmpvar_27;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_NORMALMAP_OFF" "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _SGameShadowParams;
uniform highp mat4 _SGameShadowMatrix;
uniform highp vec4 _MainTex_ST;
out mediump vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out mediump vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  tmpvar_3.zw = tmpvar_3.xy;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_2;
  highp vec3 tmpvar_9;
  tmpvar_9 = (glstate_matrix_modelview0 * tmpvar_8).xyz;
  tmpvar_4 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_2);
  highp vec3 tmpvar_13;
  tmpvar_13 = (((tmpvar_12.yzx * tmpvar_11.zxy) - (tmpvar_12.zxy * tmpvar_11.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_11.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_12.x;
  tmpvar_14[1].x = tmpvar_11.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_12.y;
  tmpvar_14[2].x = tmpvar_11.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_12.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 0.0;
  tmpvar_15.xyz = (_WorldSpaceCameraPos - tmpvar_10.xyz);
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_14 * (_World2Object * tmpvar_15).xyz));
  tmpvar_5 = tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 0.0;
  tmpvar_17.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_14 * (_World2Object * tmpvar_17).xyz));
  tmpvar_6 = tmpvar_18;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (_SGameShadowMatrix * tmpvar_10);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform mediump vec4 _ShadowColor;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _RampMap;
uniform sampler2D _SGameShadowTexture;
in mediump vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in mediump vec3 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float gloss_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec3 tmpvar_4;
  tmpvar_4 = ((tmpvar_3.xyz * (texture (_LightTex, tmpvar_2) * 2.0).xyz) + tmpvar_3.xyz);
  lowp float tmpvar_5;
  tmpvar_5 = texture (_MaskTex, xlv_TEXCOORD0.xy).x;
  gloss_1 = tmpvar_5;
  lowp float tmpvar_6;
  highp vec3 coord_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = (((xlv_TEXCOORD4.xyz / xlv_TEXCOORD4.w) * 0.5) + 0.5);
  coord_7.xy = tmpvar_8.xy;
  coord_7.z = (tmpvar_8.z - 0.0002);
  highp vec4 c_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_SGameShadowTexture, tmpvar_8.xy);
  c_9 = tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = clamp ((2.0 - exp(
    ((coord_7.z - dot (c_9, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0)
  )), 0.0, 1.0);
  coord_7.xy = (tmpvar_8.xy + vec2(0.000976563, 0.000976563));
  highp vec4 c_12;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_SGameShadowTexture, coord_7.xy);
  c_12 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (tmpvar_11 + clamp ((2.0 - 
    exp(((coord_7.z - dot (c_12, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))
  ), 0.0, 1.0));
  tmpvar_6 = ((tmpvar_14 * 0.25) + 0.5);
  mediump vec3 albedo_15;
  albedo_15 = tmpvar_4;
  lowp vec3 color_16;
  lowp vec3 darkColor_17;
  mediump vec3 ramp_18;
  highp float nh_19;
  lowp float diff_20;
  mediump float tmpvar_21;
  tmpvar_21 = ((xlv_TEXCOORD3.z * 0.5) + 0.5);
  diff_20 = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = max (0.0, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD2)).z);
  nh_19 = tmpvar_22;
  lowp vec2 tmpvar_23;
  tmpvar_23.y = 0.5;
  tmpvar_23.x = diff_20;
  lowp vec3 tmpvar_24;
  tmpvar_24 = texture (_RampMap, tmpvar_23).xyz;
  ramp_18 = tmpvar_24;
  mediump vec3 tmpvar_25;
  tmpvar_25 = (albedo_15 * _ShadowColor.xyz);
  darkColor_17 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = ((_SpecColor * (
    (((pow (nh_19, _SpecPower) * gloss_1) * _SpecMultiplier) * tmpvar_6)
   * 2.0)) + ((ramp_18 + _AmbientColor) * albedo_15));
  color_16 = tmpvar_26;
  lowp vec4 tmpvar_27;
  tmpvar_27.xyz = mix (darkColor_17, color_16, vec3(tmpvar_6));
  tmpvar_27.w = tmpvar_3.w;
  _glesFragData[0] = tmpvar_27;
}



#endif"
}
SubProgram "gles " {
Keywords { "_NOISETEX_ON" "_NORMALMAP_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump float _Scroll2X;
uniform mediump float _Scroll2Y;
uniform highp vec4 _SGameShadowParams;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _NoiseTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8.x = _Scroll2X;
  tmpvar_8.y = _Scroll2Y;
  highp vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw) + fract((tmpvar_8 * _Time.x)));
  tmpvar_3.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 0.0;
  tmpvar_10.xyz = tmpvar_2;
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * tmpvar_10).xyz;
  tmpvar_4 = tmpvar_11;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_2);
  highp vec3 tmpvar_14;
  tmpvar_14 = (((tmpvar_13.yzx * tmpvar_12.zxy) - (tmpvar_13.zxy * tmpvar_12.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_15;
  tmpvar_15[0].x = tmpvar_12.x;
  tmpvar_15[0].y = tmpvar_14.x;
  tmpvar_15[0].z = tmpvar_13.x;
  tmpvar_15[1].x = tmpvar_12.y;
  tmpvar_15[1].y = tmpvar_14.y;
  tmpvar_15[1].z = tmpvar_13.y;
  tmpvar_15[2].x = tmpvar_12.z;
  tmpvar_15[2].y = tmpvar_14.z;
  tmpvar_15[2].z = tmpvar_13.z;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_15 * (_World2Object * tmpvar_16).xyz));
  tmpvar_5 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18.w = 0.0;
  tmpvar_18.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_15 * (_World2Object * tmpvar_18).xyz));
  tmpvar_6 = tmpvar_19;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform mediump float _MMultiplier;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _NoiseColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NoiseTex;
uniform sampler2D _RampMap;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
void main ()
{
  mediump float gloss_1;
  lowp vec3 noise_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MaskTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_NoiseTex, xlv_TEXCOORD0.zw);
  mediump vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6.xyz * (tmpvar_4.xyz * _NoiseColor));
  noise_2 = tmpvar_7;
  mediump vec3 tmpvar_8;
  tmpvar_8 = (noise_2 * (tmpvar_5.y * _MMultiplier));
  noise_2 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = (((tmpvar_4.xyz * 
    (texture2D (_LightTex, tmpvar_3) * 2.0)
  .xyz) + tmpvar_4.xyz) + noise_2);
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_5.x;
  gloss_1 = tmpvar_10;
  mediump vec3 albedo_11;
  albedo_11 = tmpvar_9;
  lowp vec3 color_12;
  mediump vec3 ramp_13;
  highp float nh_14;
  lowp float diff_15;
  mediump float tmpvar_16;
  tmpvar_16 = ((xlv_TEXCOORD3.z * 0.5) + 0.5);
  diff_15 = tmpvar_16;
  mediump float tmpvar_17;
  tmpvar_17 = max (0.0, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD2)).z);
  nh_14 = tmpvar_17;
  lowp vec2 tmpvar_18;
  tmpvar_18.y = 0.5;
  tmpvar_18.x = diff_15;
  lowp vec3 tmpvar_19;
  tmpvar_19 = texture2D (_RampMap, tmpvar_18).xyz;
  ramp_13 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = ((_SpecColor * (
    ((pow (nh_14, _SpecPower) * gloss_1) * _SpecMultiplier)
   * 2.0)) + ((ramp_13 + _AmbientColor) * albedo_11));
  color_12 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21.xyz = color_12;
  tmpvar_21.w = tmpvar_4.w;
  gl_FragData[0] = tmpvar_21;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_ON" "_NORMALMAP_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump float _Scroll2X;
uniform mediump float _Scroll2Y;
uniform highp vec4 _SGameShadowParams;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _NoiseTex_ST;
out mediump vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out mediump vec3 xlv_TEXCOORD3;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8.x = _Scroll2X;
  tmpvar_8.y = _Scroll2Y;
  highp vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw) + fract((tmpvar_8 * _Time.x)));
  tmpvar_3.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 0.0;
  tmpvar_10.xyz = tmpvar_2;
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * tmpvar_10).xyz;
  tmpvar_4 = tmpvar_11;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_2);
  highp vec3 tmpvar_14;
  tmpvar_14 = (((tmpvar_13.yzx * tmpvar_12.zxy) - (tmpvar_13.zxy * tmpvar_12.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_15;
  tmpvar_15[0].x = tmpvar_12.x;
  tmpvar_15[0].y = tmpvar_14.x;
  tmpvar_15[0].z = tmpvar_13.x;
  tmpvar_15[1].x = tmpvar_12.y;
  tmpvar_15[1].y = tmpvar_14.y;
  tmpvar_15[1].z = tmpvar_13.y;
  tmpvar_15[2].x = tmpvar_12.z;
  tmpvar_15[2].y = tmpvar_14.z;
  tmpvar_15[2].z = tmpvar_13.z;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_15 * (_World2Object * tmpvar_16).xyz));
  tmpvar_5 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18.w = 0.0;
  tmpvar_18.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_15 * (_World2Object * tmpvar_18).xyz));
  tmpvar_6 = tmpvar_19;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform mediump float _MMultiplier;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _NoiseColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NoiseTex;
uniform sampler2D _RampMap;
in mediump vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in mediump vec3 xlv_TEXCOORD3;
void main ()
{
  mediump float gloss_1;
  lowp vec3 noise_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MaskTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_NoiseTex, xlv_TEXCOORD0.zw);
  mediump vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6.xyz * (tmpvar_4.xyz * _NoiseColor));
  noise_2 = tmpvar_7;
  mediump vec3 tmpvar_8;
  tmpvar_8 = (noise_2 * (tmpvar_5.y * _MMultiplier));
  noise_2 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = (((tmpvar_4.xyz * 
    (texture (_LightTex, tmpvar_3) * 2.0)
  .xyz) + tmpvar_4.xyz) + noise_2);
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_5.x;
  gloss_1 = tmpvar_10;
  mediump vec3 albedo_11;
  albedo_11 = tmpvar_9;
  lowp vec3 color_12;
  mediump vec3 ramp_13;
  highp float nh_14;
  lowp float diff_15;
  mediump float tmpvar_16;
  tmpvar_16 = ((xlv_TEXCOORD3.z * 0.5) + 0.5);
  diff_15 = tmpvar_16;
  mediump float tmpvar_17;
  tmpvar_17 = max (0.0, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD2)).z);
  nh_14 = tmpvar_17;
  lowp vec2 tmpvar_18;
  tmpvar_18.y = 0.5;
  tmpvar_18.x = diff_15;
  lowp vec3 tmpvar_19;
  tmpvar_19 = texture (_RampMap, tmpvar_18).xyz;
  ramp_13 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = ((_SpecColor * (
    ((pow (nh_14, _SpecPower) * gloss_1) * _SpecMultiplier)
   * 2.0)) + ((ramp_13 + _AmbientColor) * albedo_11));
  color_12 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21.xyz = color_12;
  tmpvar_21.w = tmpvar_4.w;
  _glesFragData[0] = tmpvar_21;
}



#endif"
}
SubProgram "gles " {
Keywords { "_NOISETEX_ON" "_NORMALMAP_OFF" "_SGAME_HEROSHOW_SHADOW_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump float _Scroll2X;
uniform mediump float _Scroll2Y;
uniform highp vec4 _SGameShadowParams;
uniform highp mat4 _SGameShadowMatrix;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _NoiseTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8.x = _Scroll2X;
  tmpvar_8.y = _Scroll2Y;
  highp vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw) + fract((tmpvar_8 * _Time.x)));
  tmpvar_3.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 0.0;
  tmpvar_10.xyz = tmpvar_2;
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * tmpvar_10).xyz;
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_2);
  highp vec3 tmpvar_15;
  tmpvar_15 = (((tmpvar_14.yzx * tmpvar_13.zxy) - (tmpvar_14.zxy * tmpvar_13.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_16;
  tmpvar_16[0].x = tmpvar_13.x;
  tmpvar_16[0].y = tmpvar_15.x;
  tmpvar_16[0].z = tmpvar_14.x;
  tmpvar_16[1].x = tmpvar_13.y;
  tmpvar_16[1].y = tmpvar_15.y;
  tmpvar_16[1].z = tmpvar_14.y;
  tmpvar_16[2].x = tmpvar_13.z;
  tmpvar_16[2].y = tmpvar_15.z;
  tmpvar_16[2].z = tmpvar_14.z;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 0.0;
  tmpvar_17.xyz = (_WorldSpaceCameraPos - tmpvar_12.xyz);
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_16 * (_World2Object * tmpvar_17).xyz));
  tmpvar_5 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_20;
  tmpvar_20 = normalize((tmpvar_16 * (_World2Object * tmpvar_19).xyz));
  tmpvar_6 = tmpvar_20;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (_SGameShadowMatrix * tmpvar_12);
}



#endif
#ifdef FRAGMENT

uniform mediump float _MMultiplier;
uniform mediump vec4 _ShadowColor;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _NoiseColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NoiseTex;
uniform sampler2D _RampMap;
uniform sampler2D _SGameShadowTexture;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float gloss_1;
  lowp vec3 noise_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MaskTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_NoiseTex, xlv_TEXCOORD0.zw);
  mediump vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6.xyz * (tmpvar_4.xyz * _NoiseColor));
  noise_2 = tmpvar_7;
  mediump vec3 tmpvar_8;
  tmpvar_8 = (noise_2 * (tmpvar_5.y * _MMultiplier));
  noise_2 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = (((tmpvar_4.xyz * 
    (texture2D (_LightTex, tmpvar_3) * 2.0)
  .xyz) + tmpvar_4.xyz) + noise_2);
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_5.x;
  gloss_1 = tmpvar_10;
  lowp float tmpvar_11;
  highp vec3 coord_12;
  highp vec3 tmpvar_13;
  tmpvar_13 = (((xlv_TEXCOORD4.xyz / xlv_TEXCOORD4.w) * 0.5) + 0.5);
  coord_12.xy = tmpvar_13.xy;
  coord_12.z = (tmpvar_13.z - 0.0002);
  highp vec4 c_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_SGameShadowTexture, tmpvar_13.xy);
  c_14 = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = clamp ((2.0 - exp(
    ((coord_12.z - dot (c_14, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0)
  )), 0.0, 1.0);
  coord_12.xy = (tmpvar_13.xy + vec2(0.000976563, 0.000976563));
  highp vec4 c_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_SGameShadowTexture, coord_12.xy);
  c_17 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (tmpvar_16 + clamp ((2.0 - 
    exp(((coord_12.z - dot (c_17, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))
  ), 0.0, 1.0));
  tmpvar_11 = ((tmpvar_19 * 0.25) + 0.5);
  mediump vec3 albedo_20;
  albedo_20 = tmpvar_9;
  lowp vec3 color_21;
  lowp vec3 darkColor_22;
  mediump vec3 ramp_23;
  highp float nh_24;
  lowp float diff_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((xlv_TEXCOORD3.z * 0.5) + 0.5);
  diff_25 = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = max (0.0, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD2)).z);
  nh_24 = tmpvar_27;
  lowp vec2 tmpvar_28;
  tmpvar_28.y = 0.5;
  tmpvar_28.x = diff_25;
  lowp vec3 tmpvar_29;
  tmpvar_29 = texture2D (_RampMap, tmpvar_28).xyz;
  ramp_23 = tmpvar_29;
  mediump vec3 tmpvar_30;
  tmpvar_30 = (albedo_20 * _ShadowColor.xyz);
  darkColor_22 = tmpvar_30;
  highp vec3 tmpvar_31;
  tmpvar_31 = ((_SpecColor * (
    (((pow (nh_24, _SpecPower) * gloss_1) * _SpecMultiplier) * tmpvar_11)
   * 2.0)) + ((ramp_23 + _AmbientColor) * albedo_20));
  color_21 = tmpvar_31;
  lowp vec4 tmpvar_32;
  tmpvar_32.xyz = mix (darkColor_22, color_21, vec3(tmpvar_11));
  tmpvar_32.w = tmpvar_4.w;
  gl_FragData[0] = tmpvar_32;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_ON" "_NORMALMAP_OFF" "_SGAME_HEROSHOW_SHADOW_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump float _Scroll2X;
uniform mediump float _Scroll2Y;
uniform highp vec4 _SGameShadowParams;
uniform highp mat4 _SGameShadowMatrix;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _NoiseTex_ST;
out mediump vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out mediump vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8.x = _Scroll2X;
  tmpvar_8.y = _Scroll2Y;
  highp vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw) + fract((tmpvar_8 * _Time.x)));
  tmpvar_3.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 0.0;
  tmpvar_10.xyz = tmpvar_2;
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * tmpvar_10).xyz;
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_2);
  highp vec3 tmpvar_15;
  tmpvar_15 = (((tmpvar_14.yzx * tmpvar_13.zxy) - (tmpvar_14.zxy * tmpvar_13.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_16;
  tmpvar_16[0].x = tmpvar_13.x;
  tmpvar_16[0].y = tmpvar_15.x;
  tmpvar_16[0].z = tmpvar_14.x;
  tmpvar_16[1].x = tmpvar_13.y;
  tmpvar_16[1].y = tmpvar_15.y;
  tmpvar_16[1].z = tmpvar_14.y;
  tmpvar_16[2].x = tmpvar_13.z;
  tmpvar_16[2].y = tmpvar_15.z;
  tmpvar_16[2].z = tmpvar_14.z;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 0.0;
  tmpvar_17.xyz = (_WorldSpaceCameraPos - tmpvar_12.xyz);
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_16 * (_World2Object * tmpvar_17).xyz));
  tmpvar_5 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_20;
  tmpvar_20 = normalize((tmpvar_16 * (_World2Object * tmpvar_19).xyz));
  tmpvar_6 = tmpvar_20;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (_SGameShadowMatrix * tmpvar_12);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform mediump float _MMultiplier;
uniform mediump vec4 _ShadowColor;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _NoiseColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NoiseTex;
uniform sampler2D _RampMap;
uniform sampler2D _SGameShadowTexture;
in mediump vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in mediump vec3 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float gloss_1;
  lowp vec3 noise_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MaskTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_NoiseTex, xlv_TEXCOORD0.zw);
  mediump vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6.xyz * (tmpvar_4.xyz * _NoiseColor));
  noise_2 = tmpvar_7;
  mediump vec3 tmpvar_8;
  tmpvar_8 = (noise_2 * (tmpvar_5.y * _MMultiplier));
  noise_2 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = (((tmpvar_4.xyz * 
    (texture (_LightTex, tmpvar_3) * 2.0)
  .xyz) + tmpvar_4.xyz) + noise_2);
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_5.x;
  gloss_1 = tmpvar_10;
  lowp float tmpvar_11;
  highp vec3 coord_12;
  highp vec3 tmpvar_13;
  tmpvar_13 = (((xlv_TEXCOORD4.xyz / xlv_TEXCOORD4.w) * 0.5) + 0.5);
  coord_12.xy = tmpvar_13.xy;
  coord_12.z = (tmpvar_13.z - 0.0002);
  highp vec4 c_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture (_SGameShadowTexture, tmpvar_13.xy);
  c_14 = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = clamp ((2.0 - exp(
    ((coord_12.z - dot (c_14, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0)
  )), 0.0, 1.0);
  coord_12.xy = (tmpvar_13.xy + vec2(0.000976563, 0.000976563));
  highp vec4 c_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture (_SGameShadowTexture, coord_12.xy);
  c_17 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (tmpvar_16 + clamp ((2.0 - 
    exp(((coord_12.z - dot (c_17, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))
  ), 0.0, 1.0));
  tmpvar_11 = ((tmpvar_19 * 0.25) + 0.5);
  mediump vec3 albedo_20;
  albedo_20 = tmpvar_9;
  lowp vec3 color_21;
  lowp vec3 darkColor_22;
  mediump vec3 ramp_23;
  highp float nh_24;
  lowp float diff_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((xlv_TEXCOORD3.z * 0.5) + 0.5);
  diff_25 = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = max (0.0, normalize((xlv_TEXCOORD3 + xlv_TEXCOORD2)).z);
  nh_24 = tmpvar_27;
  lowp vec2 tmpvar_28;
  tmpvar_28.y = 0.5;
  tmpvar_28.x = diff_25;
  lowp vec3 tmpvar_29;
  tmpvar_29 = texture (_RampMap, tmpvar_28).xyz;
  ramp_23 = tmpvar_29;
  mediump vec3 tmpvar_30;
  tmpvar_30 = (albedo_20 * _ShadowColor.xyz);
  darkColor_22 = tmpvar_30;
  highp vec3 tmpvar_31;
  tmpvar_31 = ((_SpecColor * (
    (((pow (nh_24, _SpecPower) * gloss_1) * _SpecMultiplier) * tmpvar_11)
   * 2.0)) + ((ramp_23 + _AmbientColor) * albedo_20));
  color_21 = tmpvar_31;
  lowp vec4 tmpvar_32;
  tmpvar_32.xyz = mix (darkColor_22, color_21, vec3(tmpvar_11));
  tmpvar_32.w = tmpvar_4.w;
  _glesFragData[0] = tmpvar_32;
}



#endif"
}
SubProgram "gles " {
Keywords { "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" "_NORMALMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _SGameShadowParams;
uniform highp vec4 _MainTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  tmpvar_3.zw = tmpvar_3.xy;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_2;
  highp vec3 tmpvar_9;
  tmpvar_9 = (glstate_matrix_modelview0 * tmpvar_8).xyz;
  tmpvar_4 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_2);
  highp vec3 tmpvar_12;
  tmpvar_12 = (((tmpvar_11.yzx * tmpvar_10.zxy) - (tmpvar_11.zxy * tmpvar_10.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_10.x;
  tmpvar_13[0].y = tmpvar_12.x;
  tmpvar_13[0].z = tmpvar_11.x;
  tmpvar_13[1].x = tmpvar_10.y;
  tmpvar_13[1].y = tmpvar_12.y;
  tmpvar_13[1].z = tmpvar_11.y;
  tmpvar_13[2].x = tmpvar_10.z;
  tmpvar_13[2].y = tmpvar_12.z;
  tmpvar_13[2].z = tmpvar_11.z;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 0.0;
  tmpvar_14.xyz = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((tmpvar_13 * (_World2Object * tmpvar_14).xyz));
  tmpvar_5 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_13 * (_World2Object * tmpvar_16).xyz));
  tmpvar_6 = tmpvar_17;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NormalTex;
uniform sampler2D _RampMap;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
void main ()
{
  mediump float gloss_1;
  mediump vec3 normalTS_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec3 tmpvar_4;
  tmpvar_4 = normalize(((texture2D (_NormalTex, xlv_TEXCOORD0.xy).xyz * 2.0) - 1.0));
  normalTS_2 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((tmpvar_5.xyz * (texture2D (_LightTex, tmpvar_3) * 2.0).xyz) + tmpvar_5.xyz);
  lowp float tmpvar_7;
  tmpvar_7 = texture2D (_MaskTex, xlv_TEXCOORD0.xy).x;
  gloss_1 = tmpvar_7;
  mediump vec3 albedo_8;
  albedo_8 = tmpvar_6;
  lowp vec3 color_9;
  mediump vec3 ramp_10;
  highp float nh_11;
  lowp float diff_12;
  mediump float tmpvar_13;
  tmpvar_13 = ((dot (normalTS_2, xlv_TEXCOORD3) * 0.5) + 0.5);
  diff_12 = tmpvar_13;
  mediump float tmpvar_14;
  tmpvar_14 = max (0.0, dot (normalTS_2, normalize(
    (xlv_TEXCOORD3 + xlv_TEXCOORD2)
  )));
  nh_11 = tmpvar_14;
  lowp vec2 tmpvar_15;
  tmpvar_15.y = 0.5;
  tmpvar_15.x = diff_12;
  lowp vec3 tmpvar_16;
  tmpvar_16 = texture2D (_RampMap, tmpvar_15).xyz;
  ramp_10 = tmpvar_16;
  highp vec3 tmpvar_17;
  tmpvar_17 = ((_SpecColor * (
    ((pow (nh_11, _SpecPower) * gloss_1) * _SpecMultiplier)
   * 2.0)) + ((ramp_10 + _AmbientColor) * albedo_8));
  color_9 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18.xyz = color_9;
  tmpvar_18.w = tmpvar_5.w;
  gl_FragData[0] = tmpvar_18;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" "_NORMALMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _SGameShadowParams;
uniform highp vec4 _MainTex_ST;
out mediump vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out mediump vec3 xlv_TEXCOORD3;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  tmpvar_3.zw = tmpvar_3.xy;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_2;
  highp vec3 tmpvar_9;
  tmpvar_9 = (glstate_matrix_modelview0 * tmpvar_8).xyz;
  tmpvar_4 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_2);
  highp vec3 tmpvar_12;
  tmpvar_12 = (((tmpvar_11.yzx * tmpvar_10.zxy) - (tmpvar_11.zxy * tmpvar_10.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_10.x;
  tmpvar_13[0].y = tmpvar_12.x;
  tmpvar_13[0].z = tmpvar_11.x;
  tmpvar_13[1].x = tmpvar_10.y;
  tmpvar_13[1].y = tmpvar_12.y;
  tmpvar_13[1].z = tmpvar_11.y;
  tmpvar_13[2].x = tmpvar_10.z;
  tmpvar_13[2].y = tmpvar_12.z;
  tmpvar_13[2].z = tmpvar_11.z;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 0.0;
  tmpvar_14.xyz = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((tmpvar_13 * (_World2Object * tmpvar_14).xyz));
  tmpvar_5 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_13 * (_World2Object * tmpvar_16).xyz));
  tmpvar_6 = tmpvar_17;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NormalTex;
uniform sampler2D _RampMap;
in mediump vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in mediump vec3 xlv_TEXCOORD3;
void main ()
{
  mediump float gloss_1;
  mediump vec3 normalTS_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec3 tmpvar_4;
  tmpvar_4 = normalize(((texture (_NormalTex, xlv_TEXCOORD0.xy).xyz * 2.0) - 1.0));
  normalTS_2 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((tmpvar_5.xyz * (texture (_LightTex, tmpvar_3) * 2.0).xyz) + tmpvar_5.xyz);
  lowp float tmpvar_7;
  tmpvar_7 = texture (_MaskTex, xlv_TEXCOORD0.xy).x;
  gloss_1 = tmpvar_7;
  mediump vec3 albedo_8;
  albedo_8 = tmpvar_6;
  lowp vec3 color_9;
  mediump vec3 ramp_10;
  highp float nh_11;
  lowp float diff_12;
  mediump float tmpvar_13;
  tmpvar_13 = ((dot (normalTS_2, xlv_TEXCOORD3) * 0.5) + 0.5);
  diff_12 = tmpvar_13;
  mediump float tmpvar_14;
  tmpvar_14 = max (0.0, dot (normalTS_2, normalize(
    (xlv_TEXCOORD3 + xlv_TEXCOORD2)
  )));
  nh_11 = tmpvar_14;
  lowp vec2 tmpvar_15;
  tmpvar_15.y = 0.5;
  tmpvar_15.x = diff_12;
  lowp vec3 tmpvar_16;
  tmpvar_16 = texture (_RampMap, tmpvar_15).xyz;
  ramp_10 = tmpvar_16;
  highp vec3 tmpvar_17;
  tmpvar_17 = ((_SpecColor * (
    ((pow (nh_11, _SpecPower) * gloss_1) * _SpecMultiplier)
   * 2.0)) + ((ramp_10 + _AmbientColor) * albedo_8));
  color_9 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18.xyz = color_9;
  tmpvar_18.w = tmpvar_5.w;
  _glesFragData[0] = tmpvar_18;
}



#endif"
}
SubProgram "gles " {
Keywords { "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_ON" "_NORMALMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _SGameShadowParams;
uniform highp mat4 _SGameShadowMatrix;
uniform highp vec4 _MainTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  tmpvar_3.zw = tmpvar_3.xy;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_2;
  highp vec3 tmpvar_9;
  tmpvar_9 = (glstate_matrix_modelview0 * tmpvar_8).xyz;
  tmpvar_4 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_2);
  highp vec3 tmpvar_13;
  tmpvar_13 = (((tmpvar_12.yzx * tmpvar_11.zxy) - (tmpvar_12.zxy * tmpvar_11.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_11.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_12.x;
  tmpvar_14[1].x = tmpvar_11.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_12.y;
  tmpvar_14[2].x = tmpvar_11.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_12.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 0.0;
  tmpvar_15.xyz = (_WorldSpaceCameraPos - tmpvar_10.xyz);
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_14 * (_World2Object * tmpvar_15).xyz));
  tmpvar_5 = tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 0.0;
  tmpvar_17.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_14 * (_World2Object * tmpvar_17).xyz));
  tmpvar_6 = tmpvar_18;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (_SGameShadowMatrix * tmpvar_10);
}



#endif
#ifdef FRAGMENT

uniform mediump vec4 _ShadowColor;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NormalTex;
uniform sampler2D _RampMap;
uniform sampler2D _SGameShadowTexture;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float gloss_1;
  mediump vec3 normalTS_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec3 tmpvar_4;
  tmpvar_4 = normalize(((texture2D (_NormalTex, xlv_TEXCOORD0.xy).xyz * 2.0) - 1.0));
  normalTS_2 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((tmpvar_5.xyz * (texture2D (_LightTex, tmpvar_3) * 2.0).xyz) + tmpvar_5.xyz);
  lowp float tmpvar_7;
  tmpvar_7 = texture2D (_MaskTex, xlv_TEXCOORD0.xy).x;
  gloss_1 = tmpvar_7;
  lowp float tmpvar_8;
  highp vec3 coord_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = (((xlv_TEXCOORD4.xyz / xlv_TEXCOORD4.w) * 0.5) + 0.5);
  coord_9.xy = tmpvar_10.xy;
  coord_9.z = (tmpvar_10.z - 0.0002);
  highp vec4 c_11;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_SGameShadowTexture, tmpvar_10.xy);
  c_11 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = clamp ((2.0 - exp(
    ((coord_9.z - dot (c_11, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0)
  )), 0.0, 1.0);
  coord_9.xy = (tmpvar_10.xy + vec2(0.000976563, 0.000976563));
  highp vec4 c_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_SGameShadowTexture, coord_9.xy);
  c_14 = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = (tmpvar_13 + clamp ((2.0 - 
    exp(((coord_9.z - dot (c_14, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))
  ), 0.0, 1.0));
  tmpvar_8 = ((tmpvar_16 * 0.25) + 0.5);
  mediump vec3 albedo_17;
  albedo_17 = tmpvar_6;
  lowp vec3 color_18;
  lowp vec3 darkColor_19;
  mediump vec3 ramp_20;
  highp float nh_21;
  lowp float diff_22;
  mediump float tmpvar_23;
  tmpvar_23 = ((dot (normalTS_2, xlv_TEXCOORD3) * 0.5) + 0.5);
  diff_22 = tmpvar_23;
  mediump float tmpvar_24;
  tmpvar_24 = max (0.0, dot (normalTS_2, normalize(
    (xlv_TEXCOORD3 + xlv_TEXCOORD2)
  )));
  nh_21 = tmpvar_24;
  lowp vec2 tmpvar_25;
  tmpvar_25.y = 0.5;
  tmpvar_25.x = diff_22;
  lowp vec3 tmpvar_26;
  tmpvar_26 = texture2D (_RampMap, tmpvar_25).xyz;
  ramp_20 = tmpvar_26;
  mediump vec3 tmpvar_27;
  tmpvar_27 = (albedo_17 * _ShadowColor.xyz);
  darkColor_19 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = ((_SpecColor * (
    (((pow (nh_21, _SpecPower) * gloss_1) * _SpecMultiplier) * tmpvar_8)
   * 2.0)) + ((ramp_20 + _AmbientColor) * albedo_17));
  color_18 = tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = mix (darkColor_19, color_18, vec3(tmpvar_8));
  tmpvar_29.w = tmpvar_5.w;
  gl_FragData[0] = tmpvar_29;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_ON" "_NORMALMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _SGameShadowParams;
uniform highp mat4 _SGameShadowMatrix;
uniform highp vec4 _MainTex_ST;
out mediump vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out mediump vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  tmpvar_3.zw = tmpvar_3.xy;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_2;
  highp vec3 tmpvar_9;
  tmpvar_9 = (glstate_matrix_modelview0 * tmpvar_8).xyz;
  tmpvar_4 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_2);
  highp vec3 tmpvar_13;
  tmpvar_13 = (((tmpvar_12.yzx * tmpvar_11.zxy) - (tmpvar_12.zxy * tmpvar_11.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_11.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_12.x;
  tmpvar_14[1].x = tmpvar_11.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_12.y;
  tmpvar_14[2].x = tmpvar_11.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_12.z;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 0.0;
  tmpvar_15.xyz = (_WorldSpaceCameraPos - tmpvar_10.xyz);
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_14 * (_World2Object * tmpvar_15).xyz));
  tmpvar_5 = tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 0.0;
  tmpvar_17.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_14 * (_World2Object * tmpvar_17).xyz));
  tmpvar_6 = tmpvar_18;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (_SGameShadowMatrix * tmpvar_10);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform mediump vec4 _ShadowColor;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NormalTex;
uniform sampler2D _RampMap;
uniform sampler2D _SGameShadowTexture;
in mediump vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in mediump vec3 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float gloss_1;
  mediump vec3 normalTS_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec3 tmpvar_4;
  tmpvar_4 = normalize(((texture (_NormalTex, xlv_TEXCOORD0.xy).xyz * 2.0) - 1.0));
  normalTS_2 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((tmpvar_5.xyz * (texture (_LightTex, tmpvar_3) * 2.0).xyz) + tmpvar_5.xyz);
  lowp float tmpvar_7;
  tmpvar_7 = texture (_MaskTex, xlv_TEXCOORD0.xy).x;
  gloss_1 = tmpvar_7;
  lowp float tmpvar_8;
  highp vec3 coord_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = (((xlv_TEXCOORD4.xyz / xlv_TEXCOORD4.w) * 0.5) + 0.5);
  coord_9.xy = tmpvar_10.xy;
  coord_9.z = (tmpvar_10.z - 0.0002);
  highp vec4 c_11;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture (_SGameShadowTexture, tmpvar_10.xy);
  c_11 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = clamp ((2.0 - exp(
    ((coord_9.z - dot (c_11, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0)
  )), 0.0, 1.0);
  coord_9.xy = (tmpvar_10.xy + vec2(0.000976563, 0.000976563));
  highp vec4 c_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture (_SGameShadowTexture, coord_9.xy);
  c_14 = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = (tmpvar_13 + clamp ((2.0 - 
    exp(((coord_9.z - dot (c_14, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))
  ), 0.0, 1.0));
  tmpvar_8 = ((tmpvar_16 * 0.25) + 0.5);
  mediump vec3 albedo_17;
  albedo_17 = tmpvar_6;
  lowp vec3 color_18;
  lowp vec3 darkColor_19;
  mediump vec3 ramp_20;
  highp float nh_21;
  lowp float diff_22;
  mediump float tmpvar_23;
  tmpvar_23 = ((dot (normalTS_2, xlv_TEXCOORD3) * 0.5) + 0.5);
  diff_22 = tmpvar_23;
  mediump float tmpvar_24;
  tmpvar_24 = max (0.0, dot (normalTS_2, normalize(
    (xlv_TEXCOORD3 + xlv_TEXCOORD2)
  )));
  nh_21 = tmpvar_24;
  lowp vec2 tmpvar_25;
  tmpvar_25.y = 0.5;
  tmpvar_25.x = diff_22;
  lowp vec3 tmpvar_26;
  tmpvar_26 = texture (_RampMap, tmpvar_25).xyz;
  ramp_20 = tmpvar_26;
  mediump vec3 tmpvar_27;
  tmpvar_27 = (albedo_17 * _ShadowColor.xyz);
  darkColor_19 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = ((_SpecColor * (
    (((pow (nh_21, _SpecPower) * gloss_1) * _SpecMultiplier) * tmpvar_8)
   * 2.0)) + ((ramp_20 + _AmbientColor) * albedo_17));
  color_18 = tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_29.xyz = mix (darkColor_19, color_18, vec3(tmpvar_8));
  tmpvar_29.w = tmpvar_5.w;
  _glesFragData[0] = tmpvar_29;
}



#endif"
}
SubProgram "gles " {
Keywords { "_NOISETEX_ON" "_SGAME_HEROSHOW_SHADOW_OFF" "_NORMALMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump float _Scroll2X;
uniform mediump float _Scroll2Y;
uniform highp vec4 _SGameShadowParams;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _NoiseTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8.x = _Scroll2X;
  tmpvar_8.y = _Scroll2Y;
  highp vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw) + fract((tmpvar_8 * _Time.x)));
  tmpvar_3.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 0.0;
  tmpvar_10.xyz = tmpvar_2;
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * tmpvar_10).xyz;
  tmpvar_4 = tmpvar_11;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_2);
  highp vec3 tmpvar_14;
  tmpvar_14 = (((tmpvar_13.yzx * tmpvar_12.zxy) - (tmpvar_13.zxy * tmpvar_12.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_15;
  tmpvar_15[0].x = tmpvar_12.x;
  tmpvar_15[0].y = tmpvar_14.x;
  tmpvar_15[0].z = tmpvar_13.x;
  tmpvar_15[1].x = tmpvar_12.y;
  tmpvar_15[1].y = tmpvar_14.y;
  tmpvar_15[1].z = tmpvar_13.y;
  tmpvar_15[2].x = tmpvar_12.z;
  tmpvar_15[2].y = tmpvar_14.z;
  tmpvar_15[2].z = tmpvar_13.z;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_15 * (_World2Object * tmpvar_16).xyz));
  tmpvar_5 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18.w = 0.0;
  tmpvar_18.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_15 * (_World2Object * tmpvar_18).xyz));
  tmpvar_6 = tmpvar_19;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform mediump float _MMultiplier;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _NoiseColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NormalTex;
uniform sampler2D _NoiseTex;
uniform sampler2D _RampMap;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
void main ()
{
  mediump float gloss_1;
  lowp vec3 noise_2;
  mediump vec3 normalTS_3;
  mediump vec2 tmpvar_4;
  tmpvar_4 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec3 tmpvar_5;
  tmpvar_5 = normalize(((texture2D (_NormalTex, xlv_TEXCOORD0.xy).xyz * 2.0) - 1.0));
  normalTS_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MaskTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_NoiseTex, xlv_TEXCOORD0.zw);
  mediump vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8.xyz * (tmpvar_6.xyz * _NoiseColor));
  noise_2 = tmpvar_9;
  mediump vec3 tmpvar_10;
  tmpvar_10 = (noise_2 * (tmpvar_7.y * _MMultiplier));
  noise_2 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((tmpvar_6.xyz * 
    (texture2D (_LightTex, tmpvar_4) * 2.0)
  .xyz) + tmpvar_6.xyz) + noise_2);
  lowp float tmpvar_12;
  tmpvar_12 = tmpvar_7.x;
  gloss_1 = tmpvar_12;
  mediump vec3 albedo_13;
  albedo_13 = tmpvar_11;
  lowp vec3 color_14;
  mediump vec3 ramp_15;
  highp float nh_16;
  lowp float diff_17;
  mediump float tmpvar_18;
  tmpvar_18 = ((dot (normalTS_3, xlv_TEXCOORD3) * 0.5) + 0.5);
  diff_17 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = max (0.0, dot (normalTS_3, normalize(
    (xlv_TEXCOORD3 + xlv_TEXCOORD2)
  )));
  nh_16 = tmpvar_19;
  lowp vec2 tmpvar_20;
  tmpvar_20.y = 0.5;
  tmpvar_20.x = diff_17;
  lowp vec3 tmpvar_21;
  tmpvar_21 = texture2D (_RampMap, tmpvar_20).xyz;
  ramp_15 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = ((_SpecColor * (
    ((pow (nh_16, _SpecPower) * gloss_1) * _SpecMultiplier)
   * 2.0)) + ((ramp_15 + _AmbientColor) * albedo_13));
  color_14 = tmpvar_22;
  lowp vec4 tmpvar_23;
  tmpvar_23.xyz = color_14;
  tmpvar_23.w = tmpvar_6.w;
  gl_FragData[0] = tmpvar_23;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_ON" "_SGAME_HEROSHOW_SHADOW_OFF" "_NORMALMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump float _Scroll2X;
uniform mediump float _Scroll2Y;
uniform highp vec4 _SGameShadowParams;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _NoiseTex_ST;
out mediump vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out mediump vec3 xlv_TEXCOORD3;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8.x = _Scroll2X;
  tmpvar_8.y = _Scroll2Y;
  highp vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw) + fract((tmpvar_8 * _Time.x)));
  tmpvar_3.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 0.0;
  tmpvar_10.xyz = tmpvar_2;
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * tmpvar_10).xyz;
  tmpvar_4 = tmpvar_11;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_2);
  highp vec3 tmpvar_14;
  tmpvar_14 = (((tmpvar_13.yzx * tmpvar_12.zxy) - (tmpvar_13.zxy * tmpvar_12.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_15;
  tmpvar_15[0].x = tmpvar_12.x;
  tmpvar_15[0].y = tmpvar_14.x;
  tmpvar_15[0].z = tmpvar_13.x;
  tmpvar_15[1].x = tmpvar_12.y;
  tmpvar_15[1].y = tmpvar_14.y;
  tmpvar_15[1].z = tmpvar_13.y;
  tmpvar_15[2].x = tmpvar_12.z;
  tmpvar_15[2].y = tmpvar_14.z;
  tmpvar_15[2].z = tmpvar_13.z;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_15 * (_World2Object * tmpvar_16).xyz));
  tmpvar_5 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18.w = 0.0;
  tmpvar_18.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_15 * (_World2Object * tmpvar_18).xyz));
  tmpvar_6 = tmpvar_19;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform mediump float _MMultiplier;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _NoiseColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NormalTex;
uniform sampler2D _NoiseTex;
uniform sampler2D _RampMap;
in mediump vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in mediump vec3 xlv_TEXCOORD3;
void main ()
{
  mediump float gloss_1;
  lowp vec3 noise_2;
  mediump vec3 normalTS_3;
  mediump vec2 tmpvar_4;
  tmpvar_4 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec3 tmpvar_5;
  tmpvar_5 = normalize(((texture (_NormalTex, xlv_TEXCOORD0.xy).xyz * 2.0) - 1.0));
  normalTS_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture (_MaskTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture (_NoiseTex, xlv_TEXCOORD0.zw);
  mediump vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8.xyz * (tmpvar_6.xyz * _NoiseColor));
  noise_2 = tmpvar_9;
  mediump vec3 tmpvar_10;
  tmpvar_10 = (noise_2 * (tmpvar_7.y * _MMultiplier));
  noise_2 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((tmpvar_6.xyz * 
    (texture (_LightTex, tmpvar_4) * 2.0)
  .xyz) + tmpvar_6.xyz) + noise_2);
  lowp float tmpvar_12;
  tmpvar_12 = tmpvar_7.x;
  gloss_1 = tmpvar_12;
  mediump vec3 albedo_13;
  albedo_13 = tmpvar_11;
  lowp vec3 color_14;
  mediump vec3 ramp_15;
  highp float nh_16;
  lowp float diff_17;
  mediump float tmpvar_18;
  tmpvar_18 = ((dot (normalTS_3, xlv_TEXCOORD3) * 0.5) + 0.5);
  diff_17 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = max (0.0, dot (normalTS_3, normalize(
    (xlv_TEXCOORD3 + xlv_TEXCOORD2)
  )));
  nh_16 = tmpvar_19;
  lowp vec2 tmpvar_20;
  tmpvar_20.y = 0.5;
  tmpvar_20.x = diff_17;
  lowp vec3 tmpvar_21;
  tmpvar_21 = texture (_RampMap, tmpvar_20).xyz;
  ramp_15 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = ((_SpecColor * (
    ((pow (nh_16, _SpecPower) * gloss_1) * _SpecMultiplier)
   * 2.0)) + ((ramp_15 + _AmbientColor) * albedo_13));
  color_14 = tmpvar_22;
  lowp vec4 tmpvar_23;
  tmpvar_23.xyz = color_14;
  tmpvar_23.w = tmpvar_6.w;
  _glesFragData[0] = tmpvar_23;
}



#endif"
}
SubProgram "gles " {
Keywords { "_NOISETEX_ON" "_SGAME_HEROSHOW_SHADOW_ON" "_NORMALMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump float _Scroll2X;
uniform mediump float _Scroll2Y;
uniform highp vec4 _SGameShadowParams;
uniform highp mat4 _SGameShadowMatrix;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _NoiseTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8.x = _Scroll2X;
  tmpvar_8.y = _Scroll2Y;
  highp vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw) + fract((tmpvar_8 * _Time.x)));
  tmpvar_3.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 0.0;
  tmpvar_10.xyz = tmpvar_2;
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * tmpvar_10).xyz;
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_2);
  highp vec3 tmpvar_15;
  tmpvar_15 = (((tmpvar_14.yzx * tmpvar_13.zxy) - (tmpvar_14.zxy * tmpvar_13.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_16;
  tmpvar_16[0].x = tmpvar_13.x;
  tmpvar_16[0].y = tmpvar_15.x;
  tmpvar_16[0].z = tmpvar_14.x;
  tmpvar_16[1].x = tmpvar_13.y;
  tmpvar_16[1].y = tmpvar_15.y;
  tmpvar_16[1].z = tmpvar_14.y;
  tmpvar_16[2].x = tmpvar_13.z;
  tmpvar_16[2].y = tmpvar_15.z;
  tmpvar_16[2].z = tmpvar_14.z;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 0.0;
  tmpvar_17.xyz = (_WorldSpaceCameraPos - tmpvar_12.xyz);
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_16 * (_World2Object * tmpvar_17).xyz));
  tmpvar_5 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_20;
  tmpvar_20 = normalize((tmpvar_16 * (_World2Object * tmpvar_19).xyz));
  tmpvar_6 = tmpvar_20;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (_SGameShadowMatrix * tmpvar_12);
}



#endif
#ifdef FRAGMENT

uniform mediump float _MMultiplier;
uniform mediump vec4 _ShadowColor;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _NoiseColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NormalTex;
uniform sampler2D _NoiseTex;
uniform sampler2D _RampMap;
uniform sampler2D _SGameShadowTexture;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float gloss_1;
  lowp vec3 noise_2;
  mediump vec3 normalTS_3;
  mediump vec2 tmpvar_4;
  tmpvar_4 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec3 tmpvar_5;
  tmpvar_5 = normalize(((texture2D (_NormalTex, xlv_TEXCOORD0.xy).xyz * 2.0) - 1.0));
  normalTS_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MaskTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_NoiseTex, xlv_TEXCOORD0.zw);
  mediump vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8.xyz * (tmpvar_6.xyz * _NoiseColor));
  noise_2 = tmpvar_9;
  mediump vec3 tmpvar_10;
  tmpvar_10 = (noise_2 * (tmpvar_7.y * _MMultiplier));
  noise_2 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((tmpvar_6.xyz * 
    (texture2D (_LightTex, tmpvar_4) * 2.0)
  .xyz) + tmpvar_6.xyz) + noise_2);
  lowp float tmpvar_12;
  tmpvar_12 = tmpvar_7.x;
  gloss_1 = tmpvar_12;
  lowp float tmpvar_13;
  highp vec3 coord_14;
  highp vec3 tmpvar_15;
  tmpvar_15 = (((xlv_TEXCOORD4.xyz / xlv_TEXCOORD4.w) * 0.5) + 0.5);
  coord_14.xy = tmpvar_15.xy;
  coord_14.z = (tmpvar_15.z - 0.0002);
  highp vec4 c_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_SGameShadowTexture, tmpvar_15.xy);
  c_16 = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = clamp ((2.0 - exp(
    ((coord_14.z - dot (c_16, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0)
  )), 0.0, 1.0);
  coord_14.xy = (tmpvar_15.xy + vec2(0.000976563, 0.000976563));
  highp vec4 c_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_SGameShadowTexture, coord_14.xy);
  c_19 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_18 + clamp ((2.0 - 
    exp(((coord_14.z - dot (c_19, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))
  ), 0.0, 1.0));
  tmpvar_13 = ((tmpvar_21 * 0.25) + 0.5);
  mediump vec3 albedo_22;
  albedo_22 = tmpvar_11;
  lowp vec3 color_23;
  lowp vec3 darkColor_24;
  mediump vec3 ramp_25;
  highp float nh_26;
  lowp float diff_27;
  mediump float tmpvar_28;
  tmpvar_28 = ((dot (normalTS_3, xlv_TEXCOORD3) * 0.5) + 0.5);
  diff_27 = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0, dot (normalTS_3, normalize(
    (xlv_TEXCOORD3 + xlv_TEXCOORD2)
  )));
  nh_26 = tmpvar_29;
  lowp vec2 tmpvar_30;
  tmpvar_30.y = 0.5;
  tmpvar_30.x = diff_27;
  lowp vec3 tmpvar_31;
  tmpvar_31 = texture2D (_RampMap, tmpvar_30).xyz;
  ramp_25 = tmpvar_31;
  mediump vec3 tmpvar_32;
  tmpvar_32 = (albedo_22 * _ShadowColor.xyz);
  darkColor_24 = tmpvar_32;
  highp vec3 tmpvar_33;
  tmpvar_33 = ((_SpecColor * (
    (((pow (nh_26, _SpecPower) * gloss_1) * _SpecMultiplier) * tmpvar_13)
   * 2.0)) + ((ramp_25 + _AmbientColor) * albedo_22));
  color_23 = tmpvar_33;
  lowp vec4 tmpvar_34;
  tmpvar_34.xyz = mix (darkColor_24, color_23, vec3(tmpvar_13));
  tmpvar_34.w = tmpvar_6.w;
  gl_FragData[0] = tmpvar_34;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_ON" "_SGAME_HEROSHOW_SHADOW_ON" "_NORMALMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump float _Scroll2X;
uniform mediump float _Scroll2Y;
uniform highp vec4 _SGameShadowParams;
uniform highp mat4 _SGameShadowMatrix;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _NoiseTex_ST;
out mediump vec4 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out mediump vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.xy = tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_8.x = _Scroll2X;
  tmpvar_8.y = _Scroll2Y;
  highp vec2 tmpvar_9;
  tmpvar_9 = (((_glesMultiTexCoord0.xy * _NoiseTex_ST.xy) + _NoiseTex_ST.zw) + fract((tmpvar_8 * _Time.x)));
  tmpvar_3.zw = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 0.0;
  tmpvar_10.xyz = tmpvar_2;
  highp vec3 tmpvar_11;
  tmpvar_11 = (glstate_matrix_modelview0 * tmpvar_10).xyz;
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_1.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_2);
  highp vec3 tmpvar_15;
  tmpvar_15 = (((tmpvar_14.yzx * tmpvar_13.zxy) - (tmpvar_14.zxy * tmpvar_13.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_16;
  tmpvar_16[0].x = tmpvar_13.x;
  tmpvar_16[0].y = tmpvar_15.x;
  tmpvar_16[0].z = tmpvar_14.x;
  tmpvar_16[1].x = tmpvar_13.y;
  tmpvar_16[1].y = tmpvar_15.y;
  tmpvar_16[1].z = tmpvar_14.y;
  tmpvar_16[2].x = tmpvar_13.z;
  tmpvar_16[2].y = tmpvar_15.z;
  tmpvar_16[2].z = tmpvar_14.z;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 0.0;
  tmpvar_17.xyz = (_WorldSpaceCameraPos - tmpvar_12.xyz);
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_16 * (_World2Object * tmpvar_17).xyz));
  tmpvar_5 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = -(_SGameShadowParams.xyz);
  highp vec3 tmpvar_20;
  tmpvar_20 = normalize((tmpvar_16 * (_World2Object * tmpvar_19).xyz));
  tmpvar_6 = tmpvar_20;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (_SGameShadowMatrix * tmpvar_12);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform mediump float _MMultiplier;
uniform mediump vec4 _ShadowColor;
uniform mediump float _SpecPower;
uniform mediump float _SpecMultiplier;
uniform mediump vec3 _AmbientColor;
uniform mediump vec3 _NoiseColor;
uniform mediump vec3 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _LightTex;
uniform sampler2D _NormalTex;
uniform sampler2D _NoiseTex;
uniform sampler2D _RampMap;
uniform sampler2D _SGameShadowTexture;
in mediump vec4 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in mediump vec3 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float gloss_1;
  lowp vec3 noise_2;
  mediump vec3 normalTS_3;
  mediump vec2 tmpvar_4;
  tmpvar_4 = ((normalize(xlv_TEXCOORD1).xy * 0.5) + 0.5);
  lowp vec3 tmpvar_5;
  tmpvar_5 = normalize(((texture (_NormalTex, xlv_TEXCOORD0.xy).xyz * 2.0) - 1.0));
  normalTS_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture (_MaskTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture (_NoiseTex, xlv_TEXCOORD0.zw);
  mediump vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8.xyz * (tmpvar_6.xyz * _NoiseColor));
  noise_2 = tmpvar_9;
  mediump vec3 tmpvar_10;
  tmpvar_10 = (noise_2 * (tmpvar_7.y * _MMultiplier));
  noise_2 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((tmpvar_6.xyz * 
    (texture (_LightTex, tmpvar_4) * 2.0)
  .xyz) + tmpvar_6.xyz) + noise_2);
  lowp float tmpvar_12;
  tmpvar_12 = tmpvar_7.x;
  gloss_1 = tmpvar_12;
  lowp float tmpvar_13;
  highp vec3 coord_14;
  highp vec3 tmpvar_15;
  tmpvar_15 = (((xlv_TEXCOORD4.xyz / xlv_TEXCOORD4.w) * 0.5) + 0.5);
  coord_14.xy = tmpvar_15.xy;
  coord_14.z = (tmpvar_15.z - 0.0002);
  highp vec4 c_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_SGameShadowTexture, tmpvar_15.xy);
  c_16 = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = clamp ((2.0 - exp(
    ((coord_14.z - dot (c_16, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0)
  )), 0.0, 1.0);
  coord_14.xy = (tmpvar_15.xy + vec2(0.000976563, 0.000976563));
  highp vec4 c_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture (_SGameShadowTexture, coord_14.xy);
  c_19 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_18 + clamp ((2.0 - 
    exp(((coord_14.z - dot (c_19, vec4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))
  ), 0.0, 1.0));
  tmpvar_13 = ((tmpvar_21 * 0.25) + 0.5);
  mediump vec3 albedo_22;
  albedo_22 = tmpvar_11;
  lowp vec3 color_23;
  lowp vec3 darkColor_24;
  mediump vec3 ramp_25;
  highp float nh_26;
  lowp float diff_27;
  mediump float tmpvar_28;
  tmpvar_28 = ((dot (normalTS_3, xlv_TEXCOORD3) * 0.5) + 0.5);
  diff_27 = tmpvar_28;
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0, dot (normalTS_3, normalize(
    (xlv_TEXCOORD3 + xlv_TEXCOORD2)
  )));
  nh_26 = tmpvar_29;
  lowp vec2 tmpvar_30;
  tmpvar_30.y = 0.5;
  tmpvar_30.x = diff_27;
  lowp vec3 tmpvar_31;
  tmpvar_31 = texture (_RampMap, tmpvar_30).xyz;
  ramp_25 = tmpvar_31;
  mediump vec3 tmpvar_32;
  tmpvar_32 = (albedo_22 * _ShadowColor.xyz);
  darkColor_24 = tmpvar_32;
  highp vec3 tmpvar_33;
  tmpvar_33 = ((_SpecColor * (
    (((pow (nh_26, _SpecPower) * gloss_1) * _SpecMultiplier) * tmpvar_13)
   * 2.0)) + ((ramp_25 + _AmbientColor) * albedo_22));
  color_23 = tmpvar_33;
  lowp vec4 tmpvar_34;
  tmpvar_34.xyz = mix (darkColor_24, color_23, vec3(tmpvar_13));
  tmpvar_34.w = tmpvar_6.w;
  _glesFragData[0] = tmpvar_34;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "_NORMALMAP_OFF" "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_NORMALMAP_OFF" "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "_NORMALMAP_OFF" "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_NORMALMAP_OFF" "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "_NOISETEX_ON" "_NORMALMAP_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_ON" "_NORMALMAP_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "_NOISETEX_ON" "_NORMALMAP_OFF" "_SGAME_HEROSHOW_SHADOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_ON" "_NORMALMAP_OFF" "_SGAME_HEROSHOW_SHADOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" "_NORMALMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_OFF" "_NORMALMAP_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_ON" "_NORMALMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_OFF" "_SGAME_HEROSHOW_SHADOW_ON" "_NORMALMAP_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "_NOISETEX_ON" "_SGAME_HEROSHOW_SHADOW_OFF" "_NORMALMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_ON" "_SGAME_HEROSHOW_SHADOW_OFF" "_NORMALMAP_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "_NOISETEX_ON" "_SGAME_HEROSHOW_SHADOW_ON" "_NORMALMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_NOISETEX_ON" "_SGAME_HEROSHOW_SHADOW_ON" "_NORMALMAP_ON" }
"!!GLES3"
}
}
 }
}
}