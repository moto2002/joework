Shader "S_Game_Scene/Light_VertexLit/Diffuse" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 LOD 200
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
Keywords { "_FOG_OF_WAR_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 color_1;
  lowp vec4 tmpvar_2;
  tmpvar_2.w = 1.0;
  tmpvar_2.xyz = texture2D (_MainTex, xlv_TEXCOORD0).xyz;
  color_1 = tmpvar_2;
  lowp vec3 tmpvar_3;
  tmpvar_3 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz);
  color_1.xyz = (color_1.xyz * tmpvar_3);
  gl_FragData[0] = color_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_FOG_OF_WAR_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 color_1;
  lowp vec4 tmpvar_2;
  tmpvar_2.w = 1.0;
  tmpvar_2.xyz = texture (_MainTex, xlv_TEXCOORD0).xyz;
  color_1 = tmpvar_2;
  lowp vec3 tmpvar_3;
  tmpvar_3 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz);
  color_1.xyz = (color_1.xyz * tmpvar_3);
  _glesFragData[0] = color_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "_FOG_OF_WAR_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (_Object2World * _glesVertex);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _FogOfWar;
uniform highp float _SceneSize;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  mediump float fog_1;
  mediump vec4 color_2;
  lowp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = texture2D (_MainTex, xlv_TEXCOORD0).xyz;
  color_2 = tmpvar_3;
  highp vec2 tmpvar_4;
  tmpvar_4.x = ((xlv_TEXCOORD2.x / _SceneSize) + 0.5);
  tmpvar_4.y = ((xlv_TEXCOORD2.z / _SceneSize) + 0.5);
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_FogOfWar, tmpvar_4).w;
  fog_1 = tmpvar_5;
  mediump float tmpvar_6;
  tmpvar_6 = max (0.3, fog_1);
  fog_1 = tmpvar_6;
  color_2.xyz = (color_2.xyz * tmpvar_6);
  lowp vec3 tmpvar_7;
  tmpvar_7 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz);
  color_2.xyz = (color_2.xyz * tmpvar_7);
  gl_FragData[0] = color_2;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "_FOG_OF_WAR_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 unity_LightmapST;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = (_Object2World * _glesVertex);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _FogOfWar;
uniform highp float _SceneSize;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  mediump float fog_1;
  mediump vec4 color_2;
  lowp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = texture (_MainTex, xlv_TEXCOORD0).xyz;
  color_2 = tmpvar_3;
  highp vec2 tmpvar_4;
  tmpvar_4.x = ((xlv_TEXCOORD2.x / _SceneSize) + 0.5);
  tmpvar_4.y = ((xlv_TEXCOORD2.z / _SceneSize) + 0.5);
  lowp float tmpvar_5;
  tmpvar_5 = texture (_FogOfWar, tmpvar_4).w;
  fog_1 = tmpvar_5;
  mediump float tmpvar_6;
  tmpvar_6 = max (0.3, fog_1);
  fog_1 = tmpvar_6;
  color_2.xyz = (color_2.xyz * tmpvar_6);
  lowp vec3 tmpvar_7;
  tmpvar_7 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz);
  color_2.xyz = (color_2.xyz * tmpvar_7);
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
}