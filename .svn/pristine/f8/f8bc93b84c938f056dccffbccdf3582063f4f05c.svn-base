Shader "TMPro/Mobile/Bitmap" {
Properties {
 _MainTex ("Font Atlas", 2D) = "white" {}
 _Color ("Text Color", Color) = (1,1,1,1)
 _DiffusePower ("Diffuse Power", Range(1,4)) = 1
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _Color;
uniform highp float _DiffusePower;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  lowp vec4 tmpvar_2;
  highp vec4 pos_3;
  pos_3 = (glstate_matrix_mvp * _glesVertex);
  highp vec2 tmpvar_4;
  tmpvar_4 = (_ScreenParams.xy * 0.5);
  pos_3.xy = ((floor(
    (((pos_3.xy / pos_3.w) * tmpvar_4) + vec2(0.5, 0.5))
  ) / tmpvar_4) * pos_3.w);
  tmpvar_2 = _glesColor;
  if ((_glesColor.w > 0.5)) {
    tmpvar_2.w = (_glesColor.w - 0.5);
  };
  tmpvar_2.w = (tmpvar_2.w * 2.0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_2 * _Color);
  tmpvar_2.w = tmpvar_5.w;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5.xyz * _DiffusePower);
  tmpvar_2.xyz = tmpvar_6;
  gl_Position = pos_3;
  xlv_COLOR = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.xyz = xlv_COLOR.xyz;
  tmpvar_1.w = (xlv_COLOR.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _Color;
uniform highp float _DiffusePower;
out lowp vec4 xlv_COLOR;
out highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  lowp vec4 tmpvar_2;
  highp vec4 pos_3;
  pos_3 = (glstate_matrix_mvp * _glesVertex);
  highp vec2 tmpvar_4;
  tmpvar_4 = (_ScreenParams.xy * 0.5);
  pos_3.xy = ((floor(
    (((pos_3.xy / pos_3.w) * tmpvar_4) + vec2(0.5, 0.5))
  ) / tmpvar_4) * pos_3.w);
  tmpvar_2 = _glesColor;
  if ((_glesColor.w > 0.5)) {
    tmpvar_2.w = (_glesColor.w - 0.5);
  };
  tmpvar_2.w = (tmpvar_2.w * 2.0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_2 * _Color);
  tmpvar_2.w = tmpvar_5.w;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5.xyz * _DiffusePower);
  tmpvar_2.xyz = tmpvar_6;
  gl_Position = pos_3;
  xlv_COLOR = tmpvar_2;
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in lowp vec4 xlv_COLOR;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.xyz = xlv_COLOR.xyz;
  tmpvar_1.w = (xlv_COLOR.w * texture (_MainTex, xlv_TEXCOORD0).w);
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles3 " {
"!!GLES3"
}
}
 }
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "texcoord", TexCoord0
  }
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor [_Color] combine constant * primary, constant alpha * texture alpha }
 }
}
}