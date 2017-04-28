Shader "TMPro/Bitmap" {
Properties {
 _MainTex ("Font Atlas", 2D) = "white" {}
 _FaceTex ("Font Texture", 2D) = "white" {}
 _FaceColor ("Text Color", Color) = (1,1,1,1)
 _VertexOffsetX ("Vertex OffsetX", Float) = 0
 _VertexOffsetY ("Vertex OffsetY", Float) = 0
 _MaskCoord ("Mask Coords", Vector) = (0,0,100000,100000)
 _MaskSoftnessX ("Mask SoftnessX", Float) = 0
 _MaskSoftnessY ("Mask SoftnessY", Float) = 0
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest [_ZTestMode]
  ZWrite Off
  Cull [_CullMode]
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
Keywords { "MASK_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _FaceColor;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  lowp vec4 faceColor_2;
  highp vec4 vert_3;
  vert_3.zw = _glesVertex.zw;
  vert_3.x = (_glesVertex.x + _VertexOffsetX);
  vert_3.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 pos_4;
  pos_4 = (glstate_matrix_mvp * vert_3);
  highp vec2 tmpvar_5;
  tmpvar_5 = (_ScreenParams.xy * 0.5);
  pos_4.xy = ((floor(
    (((pos_4.xy / pos_4.w) * tmpvar_5) + vec2(0.5, 0.5))
  ) / tmpvar_5) * pos_4.w);
  faceColor_2 = _glesColor;
  if ((_glesColor.w > 0.5)) {
    faceColor_2.w = (_glesColor.w - 0.5);
  };
  faceColor_2.w = (faceColor_2.w * 2.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (faceColor_2 * _FaceColor);
  faceColor_2 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7.x = ((floor(_glesMultiTexCoord1.x) * 4.0) / 4096.0);
  tmpvar_7.y = (fract(_glesMultiTexCoord1.x) * 4.0);
  gl_Position = pos_4;
  xlv_COLOR = tmpvar_6;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_7;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _FaceTex;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture2D (_FaceTex, xlv_TEXCOORD1) * xlv_COLOR);
  c_1.xyz = tmpvar_2.xyz;
  c_1.w = (tmpvar_2.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _FaceColor;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
out lowp vec4 xlv_COLOR;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  lowp vec4 faceColor_2;
  highp vec4 vert_3;
  vert_3.zw = _glesVertex.zw;
  vert_3.x = (_glesVertex.x + _VertexOffsetX);
  vert_3.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 pos_4;
  pos_4 = (glstate_matrix_mvp * vert_3);
  highp vec2 tmpvar_5;
  tmpvar_5 = (_ScreenParams.xy * 0.5);
  pos_4.xy = ((floor(
    (((pos_4.xy / pos_4.w) * tmpvar_5) + vec2(0.5, 0.5))
  ) / tmpvar_5) * pos_4.w);
  faceColor_2 = _glesColor;
  if ((_glesColor.w > 0.5)) {
    faceColor_2.w = (_glesColor.w - 0.5);
  };
  faceColor_2.w = (faceColor_2.w * 2.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (faceColor_2 * _FaceColor);
  faceColor_2 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7.x = ((floor(_glesMultiTexCoord1.x) * 4.0) / 4096.0);
  tmpvar_7.y = (fract(_glesMultiTexCoord1.x) * 4.0);
  gl_Position = pos_4;
  xlv_COLOR = tmpvar_6;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_7;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _FaceTex;
in lowp vec4 xlv_COLOR;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture (_FaceTex, xlv_TEXCOORD1) * xlv_COLOR);
  c_1.xyz = tmpvar_2.xyz;
  c_1.w = (tmpvar_2.w * texture (_MainTex, xlv_TEXCOORD0).w);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_HARD" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  lowp vec4 faceColor_2;
  highp vec4 vert_3;
  vert_3.zw = _glesVertex.zw;
  vert_3.x = (_glesVertex.x + _VertexOffsetX);
  vert_3.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 pos_4;
  pos_4 = (glstate_matrix_mvp * vert_3);
  highp vec2 tmpvar_5;
  tmpvar_5 = (_ScreenParams.xy * 0.5);
  pos_4.xy = ((floor(
    (((pos_4.xy / pos_4.w) * tmpvar_5) + vec2(0.5, 0.5))
  ) / tmpvar_5) * pos_4.w);
  faceColor_2 = _glesColor;
  if ((_glesColor.w > 0.5)) {
    faceColor_2.w = (_glesColor.w - 0.5);
  };
  faceColor_2.w = (faceColor_2.w * 2.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (faceColor_2 * _FaceColor);
  faceColor_2 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7.x = ((floor(_glesMultiTexCoord1.x) * 4.0) / 4096.0);
  tmpvar_7.y = (fract(_glesMultiTexCoord1.x) * 4.0);
  highp vec2 tmpvar_8;
  tmpvar_8.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
  tmpvar_8.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
  highp vec4 tmpvar_9;
  tmpvar_9.xy = (vert_3.xy - _MaskCoord.xy);
  tmpvar_9.zw = (0.5 / (pos_4.ww / tmpvar_8));
  gl_Position = pos_4;
  xlv_COLOR = tmpvar_6;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = tmpvar_9;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _FaceTex;
uniform highp vec4 _MaskCoord;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture2D (_FaceTex, xlv_TEXCOORD1) * xlv_COLOR);
  c_1.xyz = tmpvar_2.xyz;
  c_1.w = (tmpvar_2.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
  highp vec2 tmpvar_3;
  tmpvar_3 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp float tmpvar_4;
  tmpvar_4 = (c_1.w * (tmpvar_3.x * tmpvar_3.y));
  c_1.w = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
out lowp vec4 xlv_COLOR;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  lowp vec4 faceColor_2;
  highp vec4 vert_3;
  vert_3.zw = _glesVertex.zw;
  vert_3.x = (_glesVertex.x + _VertexOffsetX);
  vert_3.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 pos_4;
  pos_4 = (glstate_matrix_mvp * vert_3);
  highp vec2 tmpvar_5;
  tmpvar_5 = (_ScreenParams.xy * 0.5);
  pos_4.xy = ((floor(
    (((pos_4.xy / pos_4.w) * tmpvar_5) + vec2(0.5, 0.5))
  ) / tmpvar_5) * pos_4.w);
  faceColor_2 = _glesColor;
  if ((_glesColor.w > 0.5)) {
    faceColor_2.w = (_glesColor.w - 0.5);
  };
  faceColor_2.w = (faceColor_2.w * 2.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (faceColor_2 * _FaceColor);
  faceColor_2 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7.x = ((floor(_glesMultiTexCoord1.x) * 4.0) / 4096.0);
  tmpvar_7.y = (fract(_glesMultiTexCoord1.x) * 4.0);
  highp vec2 tmpvar_8;
  tmpvar_8.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
  tmpvar_8.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
  highp vec4 tmpvar_9;
  tmpvar_9.xy = (vert_3.xy - _MaskCoord.xy);
  tmpvar_9.zw = (0.5 / (pos_4.ww / tmpvar_8));
  gl_Position = pos_4;
  xlv_COLOR = tmpvar_6;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = tmpvar_9;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _FaceTex;
uniform highp vec4 _MaskCoord;
in lowp vec4 xlv_COLOR;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture (_FaceTex, xlv_TEXCOORD1) * xlv_COLOR);
  c_1.xyz = tmpvar_2.xyz;
  c_1.w = (tmpvar_2.w * texture (_MainTex, xlv_TEXCOORD0).w);
  highp vec2 tmpvar_3;
  tmpvar_3 = (1.0 - clamp ((
    (abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw)
   * xlv_TEXCOORD2.zw), 0.0, 1.0));
  highp float tmpvar_4;
  tmpvar_4 = (c_1.w * (tmpvar_3.x * tmpvar_3.y));
  c_1.w = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  lowp vec4 faceColor_2;
  highp vec4 vert_3;
  vert_3.zw = _glesVertex.zw;
  vert_3.x = (_glesVertex.x + _VertexOffsetX);
  vert_3.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 pos_4;
  pos_4 = (glstate_matrix_mvp * vert_3);
  highp vec2 tmpvar_5;
  tmpvar_5 = (_ScreenParams.xy * 0.5);
  pos_4.xy = ((floor(
    (((pos_4.xy / pos_4.w) * tmpvar_5) + vec2(0.5, 0.5))
  ) / tmpvar_5) * pos_4.w);
  faceColor_2 = _glesColor;
  if ((_glesColor.w > 0.5)) {
    faceColor_2.w = (_glesColor.w - 0.5);
  };
  faceColor_2.w = (faceColor_2.w * 2.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (faceColor_2 * _FaceColor);
  faceColor_2 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7.x = ((floor(_glesMultiTexCoord1.x) * 4.0) / 4096.0);
  tmpvar_7.y = (fract(_glesMultiTexCoord1.x) * 4.0);
  highp vec2 tmpvar_8;
  tmpvar_8.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
  tmpvar_8.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
  highp vec4 tmpvar_9;
  tmpvar_9.xy = (vert_3.xy - _MaskCoord.xy);
  tmpvar_9.zw = (0.5 / (pos_4.ww / tmpvar_8));
  gl_Position = pos_4;
  xlv_COLOR = tmpvar_6;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = tmpvar_9;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _FaceTex;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture2D (_FaceTex, xlv_TEXCOORD1) * xlv_COLOR);
  c_1.xyz = tmpvar_2.xyz;
  c_1.w = (tmpvar_2.w * texture2D (_MainTex, xlv_TEXCOORD0).w);
  highp vec2 tmpvar_3;
  tmpvar_3.x = _MaskSoftnessX;
  tmpvar_3.y = _MaskSoftnessY;
  highp vec2 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_5;
  tmpvar_5 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_4)
   / 
    (1.0 + tmpvar_4)
  ), 0.0, 1.0));
  highp vec2 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * tmpvar_5);
  highp float tmpvar_7;
  tmpvar_7 = (c_1.w * (tmpvar_6.x * tmpvar_6.y));
  c_1.w = tmpvar_7;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _ScreenParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_projection;
uniform lowp vec4 _FaceColor;
uniform highp float _VertexOffsetX;
uniform highp float _VertexOffsetY;
uniform highp vec4 _MaskCoord;
out lowp vec4 xlv_COLOR;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  lowp vec4 faceColor_2;
  highp vec4 vert_3;
  vert_3.zw = _glesVertex.zw;
  vert_3.x = (_glesVertex.x + _VertexOffsetX);
  vert_3.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 pos_4;
  pos_4 = (glstate_matrix_mvp * vert_3);
  highp vec2 tmpvar_5;
  tmpvar_5 = (_ScreenParams.xy * 0.5);
  pos_4.xy = ((floor(
    (((pos_4.xy / pos_4.w) * tmpvar_5) + vec2(0.5, 0.5))
  ) / tmpvar_5) * pos_4.w);
  faceColor_2 = _glesColor;
  if ((_glesColor.w > 0.5)) {
    faceColor_2.w = (_glesColor.w - 0.5);
  };
  faceColor_2.w = (faceColor_2.w * 2.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (faceColor_2 * _FaceColor);
  faceColor_2 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7.x = ((floor(_glesMultiTexCoord1.x) * 4.0) / 4096.0);
  tmpvar_7.y = (fract(_glesMultiTexCoord1.x) * 4.0);
  highp vec2 tmpvar_8;
  tmpvar_8.x = (_ScreenParams.x * glstate_matrix_projection[0].x);
  tmpvar_8.y = (_ScreenParams.y * glstate_matrix_projection[1].y);
  highp vec4 tmpvar_9;
  tmpvar_9.xy = (vert_3.xy - _MaskCoord.xy);
  tmpvar_9.zw = (0.5 / (pos_4.ww / tmpvar_8));
  gl_Position = pos_4;
  xlv_COLOR = tmpvar_6;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = tmpvar_9;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _FaceTex;
uniform highp vec4 _MaskCoord;
uniform highp float _MaskSoftnessX;
uniform highp float _MaskSoftnessY;
in lowp vec4 xlv_COLOR;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture (_FaceTex, xlv_TEXCOORD1) * xlv_COLOR);
  c_1.xyz = tmpvar_2.xyz;
  c_1.w = (tmpvar_2.w * texture (_MainTex, xlv_TEXCOORD0).w);
  highp vec2 tmpvar_3;
  tmpvar_3.x = _MaskSoftnessX;
  tmpvar_3.y = _MaskSoftnessY;
  highp vec2 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * xlv_TEXCOORD2.zw);
  highp vec2 tmpvar_5;
  tmpvar_5 = (1.0 - clamp ((
    (((abs(xlv_TEXCOORD2.xy) - _MaskCoord.zw) * xlv_TEXCOORD2.zw) + tmpvar_4)
   / 
    (1.0 + tmpvar_4)
  ), 0.0, 1.0));
  highp vec2 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * tmpvar_5);
  highp float tmpvar_7;
  tmpvar_7 = (c_1.w * (tmpvar_6.x * tmpvar_6.y));
  c_1.w = tmpvar_7;
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "MASK_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_HARD" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_HARD" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "MASK_SOFT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "MASK_SOFT" }
"!!GLES3"
}
}
 }
}
}