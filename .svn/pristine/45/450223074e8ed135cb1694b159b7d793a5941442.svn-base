Shader "S_Game_Scene/Light_VertexLit/TerrainVertexColor2Layers01" {
Properties {
 _Splat0 ("Layer 1", 2D) = "white" {}
 _Splat1 ("Layer 2", 2D) = "black" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
Keywords { "_FOG_OF_WAR_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _Splat0_ST;
uniform highp vec4 _Splat1_ST;
uniform highp vec4 unity_LightmapST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _Splat0_ST.xy) + _Splat0_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Splat1_ST.xy) + _Splat1_ST.zw);
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = _glesColor;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _Splat0;
uniform sampler2D _Splat1;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  mediump vec4 color_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_Splat0, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_Splat1, xlv_TEXCOORD1);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix (tmpvar_2.xyz, tmpvar_3.xyz, xlv_TEXCOORD3.www);
  color_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (color_1.xyz * xlv_TEXCOORD3.xyz);
  color_1.xyz = tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz);
  color_1.xyz = (color_1.xyz * tmpvar_6);
  gl_FragData[0] = color_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_FOG_OF_WAR_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _Splat0_ST;
uniform highp vec4 _Splat1_ST;
uniform highp vec4 unity_LightmapST;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _Splat0_ST.xy) + _Splat0_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Splat1_ST.xy) + _Splat1_ST.zw);
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = _glesColor;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _Splat0;
uniform sampler2D _Splat1;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
void main ()
{
  mediump vec4 color_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_Splat0, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_Splat1, xlv_TEXCOORD1);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix (tmpvar_2.xyz, tmpvar_3.xyz, xlv_TEXCOORD3.www);
  color_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (color_1.xyz * xlv_TEXCOORD3.xyz);
  color_1.xyz = tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz);
  color_1.xyz = (color_1.xyz * tmpvar_6);
  _glesFragData[0] = color_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "_FOG_OF_WAR_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Splat0_ST;
uniform highp vec4 _Splat1_ST;
uniform highp vec4 unity_LightmapST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _Splat0_ST.xy) + _Splat0_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Splat1_ST.xy) + _Splat1_ST.zw);
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = _glesColor;
  xlv_TEXCOORD4 = (_Object2World * _glesVertex);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _Splat0;
uniform sampler2D _Splat1;
uniform sampler2D unity_Lightmap;
uniform sampler2D _FogOfWar;
uniform highp float _SceneSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float fog_1;
  mediump vec4 color_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_Splat0, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_Splat1, xlv_TEXCOORD1);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = mix (tmpvar_3.xyz, tmpvar_4.xyz, xlv_TEXCOORD3.www);
  color_2 = tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_6 = (color_2.xyz * xlv_TEXCOORD3.xyz);
  color_2.xyz = tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz);
  color_2.xyz = (color_2.xyz * tmpvar_7);
  highp vec2 tmpvar_8;
  tmpvar_8.x = ((xlv_TEXCOORD4.x / _SceneSize) + 0.5);
  tmpvar_8.y = ((xlv_TEXCOORD4.z / _SceneSize) + 0.5);
  lowp float tmpvar_9;
  tmpvar_9 = texture2D (_FogOfWar, tmpvar_8).w;
  fog_1 = tmpvar_9;
  mediump float tmpvar_10;
  tmpvar_10 = max (0.3, fog_1);
  fog_1 = tmpvar_10;
  color_2.xyz = (color_2.xyz * tmpvar_10);
  gl_FragData[0] = color_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_FOG_OF_WAR_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Splat0_ST;
uniform highp vec4 _Splat1_ST;
uniform highp vec4 unity_LightmapST;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _Splat0_ST.xy) + _Splat0_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Splat1_ST.xy) + _Splat1_ST.zw);
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = _glesColor;
  xlv_TEXCOORD4 = (_Object2World * _glesVertex);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _Splat0;
uniform sampler2D _Splat1;
uniform sampler2D unity_Lightmap;
uniform sampler2D _FogOfWar;
uniform highp float _SceneSize;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump float fog_1;
  mediump vec4 color_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_Splat0, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_Splat1, xlv_TEXCOORD1);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = mix (tmpvar_3.xyz, tmpvar_4.xyz, xlv_TEXCOORD3.www);
  color_2 = tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_6 = (color_2.xyz * xlv_TEXCOORD3.xyz);
  color_2.xyz = tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz);
  color_2.xyz = (color_2.xyz * tmpvar_7);
  highp vec2 tmpvar_8;
  tmpvar_8.x = ((xlv_TEXCOORD4.x / _SceneSize) + 0.5);
  tmpvar_8.y = ((xlv_TEXCOORD4.z / _SceneSize) + 0.5);
  lowp float tmpvar_9;
  tmpvar_9 = texture (_FogOfWar, tmpvar_8).w;
  fog_1 = tmpvar_9;
  mediump float tmpvar_10;
  tmpvar_10 = max (0.3, fog_1);
  fog_1 = tmpvar_10;
  color_2.xyz = (color_2.xyz * tmpvar_10);
  _glesFragData[0] = color_2;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "_FOG_OF_WAR_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_FOG_OF_WAR_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "_FOG_OF_WAR_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "_FOG_OF_WAR_ON" }
"!!GLES3"
}
}
 }
}
Fallback "Diffuse"
}