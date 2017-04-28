Shader "TMPro/Distance Field (Surface)" {
Properties {
 _FaceTex ("Fill Texture", 2D) = "white" {}
 _FaceUVSpeedX ("Face UV Speed X", Range(-5,5)) = 0
 _FaceUVSpeedY ("Face UV Speed Y", Range(-5,5)) = 0
 _FaceColor ("Fill Color", Color) = (1,1,1,1)
 _FaceDilate ("Face Dilate", Range(-1,1)) = 0
 _OutlineColor ("Outline Color", Color) = (0,0,0,1)
 _OutlineTex ("Outline Texture", 2D) = "white" {}
 _OutlineUVSpeedX ("Outline UV Speed X", Range(-5,5)) = 0
 _OutlineUVSpeedY ("Outline UV Speed Y", Range(-5,5)) = 0
 _OutlineWidth ("Outline Thickness", Range(0,1)) = 0
 _OutlineSoftness ("Outline Softness", Range(0,1)) = 0
 _Bevel ("Bevel", Range(0,1)) = 0.5
 _BevelOffset ("Bevel Offset", Range(-0.5,0.5)) = 0
 _BevelWidth ("Bevel Width", Range(-0.5,0.5)) = 0
 _BevelClamp ("Bevel Clamp", Range(0,1)) = 0
 _BevelRoundness ("Bevel Roundness", Range(0,1)) = 0
 _BumpMap ("Normalmap", 2D) = "bump" {}
 _BumpOutline ("Bump Outline", Range(0,1)) = 0.5
 _BumpFace ("Bump Face", Range(0,1)) = 0.5
 _ReflectFaceColor ("Face Color", Color) = (0,0,0,1)
 _ReflectOutlineColor ("Outline Color", Color) = (0,0,0,1)
 _Cube ("Reflection Cubemap", CUBE) = "black" { TexGen CubeReflect }
 _EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
 _SpecColor ("Specular Color", Color) = (0,0,0,1)
 _FaceShininess ("Face Shininess", Range(0,1)) = 0
 _OutlineShininess ("Outline Shininess", Range(0,1)) = 0
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
varying highp vec3 xlv_TEXCOORD5;
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
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
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
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (tmpvar_21 * ((
    (_World2Object * tmpvar_23)
  .xyz * unity_Scale.w) - tmpvar_7.xyz));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform highp mat4 _Object2World;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _ReflectFaceColor;
uniform lowp vec4 _ReflectOutlineColor;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_3 = vec3(0.0, 0.0, 0.0);
  tmpvar_4 = tmpvar_2;
  tmpvar_5 = vec3(0.0, 0.0, 0.0);
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture2D (_MainTex, P_13).w;
  tmpvar_17.y = texture2D (_MainTex, P_14).w;
  tmpvar_17.z = texture2D (_MainTex, P_15).w;
  tmpvar_17.w = texture2D (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = normalize(((
    (tmpvar_43.yzx * tmpvar_45.zxy)
   - 
    (tmpvar_43.zxy * tmpvar_45.yzx)
  ) - tmpvar_47));
  highp mat3 tmpvar_49;
  tmpvar_49[0] = _Object2World[0].xyz;
  tmpvar_49[1] = _Object2World[1].xyz;
  tmpvar_49[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_50;
  highp vec3 N_51;
  N_51 = (tmpvar_49 * tmpvar_48);
  tmpvar_50 = (xlv_TEXCOORD2 - (2.0 * (
    dot (N_51, xlv_TEXCOORD2)
   * N_51)));
  lowp vec4 tmpvar_52;
  tmpvar_52 = textureCube (_Cube, tmpvar_50);
  highp float tmpvar_53;
  tmpvar_53 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  lowp vec3 tmpvar_54;
  tmpvar_54 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_53));
  highp vec3 tmpvar_55;
  tmpvar_55 = ((tmpvar_52.xyz * tmpvar_54) * faceColor_9.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = faceColor_9.xyz;
  tmpvar_3 = tmpvar_56;
  highp vec3 tmpvar_57;
  tmpvar_57 = -(tmpvar_48);
  tmpvar_4 = tmpvar_57;
  tmpvar_5 = tmpvar_55;
  highp float tmpvar_58;
  tmpvar_58 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_59;
  tmpvar_59 = faceColor_9.w;
  tmpvar_6 = tmpvar_59;
  tmpvar_2 = tmpvar_4;
  highp vec3 tmpvar_60;
  tmpvar_60 = normalize(xlv_TEXCOORD5);
  mediump vec3 viewDir_61;
  viewDir_61 = tmpvar_60;
  lowp vec4 c_62;
  highp float nh_63;
  lowp float tmpvar_64;
  tmpvar_64 = max (0.0, dot (tmpvar_4, xlv_TEXCOORD3));
  mediump float tmpvar_65;
  tmpvar_65 = max (0.0, dot (tmpvar_4, normalize(
    (xlv_TEXCOORD3 + viewDir_61)
  )));
  nh_63 = tmpvar_65;
  mediump float y_66;
  y_66 = (mix (_FaceShininess, _OutlineShininess, tmpvar_58) * 128.0);
  highp float tmpvar_67;
  tmpvar_67 = pow (nh_63, y_66);
  highp vec3 tmpvar_68;
  tmpvar_68 = (((
    (tmpvar_3 * _LightColor0.xyz)
   * tmpvar_64) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_67)) * 2.0);
  c_62.xyz = tmpvar_68;
  highp float tmpvar_69;
  tmpvar_69 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_67));
  c_62.w = tmpvar_69;
  c_1.w = c_62.w;
  c_1.xyz = (c_62.xyz + (tmpvar_3 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_5);
  c_1.w = tmpvar_6;
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
out highp vec3 xlv_TEXCOORD5;
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
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
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
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (tmpvar_21 * ((
    (_World2Object * tmpvar_23)
  .xyz * unity_Scale.w) - tmpvar_7.xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform highp mat4 _Object2World;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _ReflectFaceColor;
uniform lowp vec4 _ReflectOutlineColor;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_3 = vec3(0.0, 0.0, 0.0);
  tmpvar_4 = tmpvar_2;
  tmpvar_5 = vec3(0.0, 0.0, 0.0);
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture (_MainTex, P_13).w;
  tmpvar_17.y = texture (_MainTex, P_14).w;
  tmpvar_17.z = texture (_MainTex, P_15).w;
  tmpvar_17.w = texture (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = normalize(((
    (tmpvar_43.yzx * tmpvar_45.zxy)
   - 
    (tmpvar_43.zxy * tmpvar_45.yzx)
  ) - tmpvar_47));
  highp mat3 tmpvar_49;
  tmpvar_49[0] = _Object2World[0].xyz;
  tmpvar_49[1] = _Object2World[1].xyz;
  tmpvar_49[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_50;
  highp vec3 N_51;
  N_51 = (tmpvar_49 * tmpvar_48);
  tmpvar_50 = (xlv_TEXCOORD2 - (2.0 * (
    dot (N_51, xlv_TEXCOORD2)
   * N_51)));
  lowp vec4 tmpvar_52;
  tmpvar_52 = texture (_Cube, tmpvar_50);
  highp float tmpvar_53;
  tmpvar_53 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  lowp vec3 tmpvar_54;
  tmpvar_54 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_53));
  highp vec3 tmpvar_55;
  tmpvar_55 = ((tmpvar_52.xyz * tmpvar_54) * faceColor_9.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = faceColor_9.xyz;
  tmpvar_3 = tmpvar_56;
  highp vec3 tmpvar_57;
  tmpvar_57 = -(tmpvar_48);
  tmpvar_4 = tmpvar_57;
  tmpvar_5 = tmpvar_55;
  highp float tmpvar_58;
  tmpvar_58 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_59;
  tmpvar_59 = faceColor_9.w;
  tmpvar_6 = tmpvar_59;
  tmpvar_2 = tmpvar_4;
  highp vec3 tmpvar_60;
  tmpvar_60 = normalize(xlv_TEXCOORD5);
  mediump vec3 viewDir_61;
  viewDir_61 = tmpvar_60;
  lowp vec4 c_62;
  highp float nh_63;
  lowp float tmpvar_64;
  tmpvar_64 = max (0.0, dot (tmpvar_4, xlv_TEXCOORD3));
  mediump float tmpvar_65;
  tmpvar_65 = max (0.0, dot (tmpvar_4, normalize(
    (xlv_TEXCOORD3 + viewDir_61)
  )));
  nh_63 = tmpvar_65;
  mediump float y_66;
  y_66 = (mix (_FaceShininess, _OutlineShininess, tmpvar_58) * 128.0);
  highp float tmpvar_67;
  tmpvar_67 = pow (nh_63, y_66);
  highp vec3 tmpvar_68;
  tmpvar_68 = (((
    (tmpvar_3 * _LightColor0.xyz)
   * tmpvar_64) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_67)) * 2.0);
  c_62.xyz = tmpvar_68;
  highp float tmpvar_69;
  tmpvar_69 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_67));
  c_62.w = tmpvar_69;
  c_1.w = c_62.w;
  c_1.xyz = (c_62.xyz + (tmpvar_3 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_5);
  c_1.w = tmpvar_6;
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
varying highp vec3 xlv_TEXCOORD5;
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
  tmpvar_24.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_25;
  tmpvar_25.w = 1.0;
  tmpvar_25.xyz = tmpvar_19;
  mediump vec3 tmpvar_26;
  mediump vec4 normal_27;
  normal_27 = tmpvar_25;
  highp float vC_28;
  mediump vec3 x3_29;
  mediump vec3 x2_30;
  mediump vec3 x1_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAr, normal_27);
  x1_31.x = tmpvar_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAg, normal_27);
  x1_31.y = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAb, normal_27);
  x1_31.z = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (normal_27.xyzz * normal_27.yzzx);
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBr, tmpvar_35);
  x2_30.x = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBg, tmpvar_35);
  x2_30.y = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBb, tmpvar_35);
  x2_30.z = tmpvar_38;
  mediump float tmpvar_39;
  tmpvar_39 = ((normal_27.x * normal_27.x) - (normal_27.y * normal_27.y));
  vC_28 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (unity_SHC.xyz * vC_28);
  x3_29 = tmpvar_40;
  tmpvar_26 = ((x1_31 + x2_30) + x3_29);
  shlight_3 = tmpvar_26;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_41;
  tmpvar_41 = (_Object2World * tmpvar_7).xyz;
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosX0 - tmpvar_41.x);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosY0 - tmpvar_41.y);
  highp vec4 tmpvar_44;
  tmpvar_44 = (unity_4LightPosZ0 - tmpvar_41.z);
  highp vec4 tmpvar_45;
  tmpvar_45 = (((tmpvar_42 * tmpvar_42) + (tmpvar_43 * tmpvar_43)) + (tmpvar_44 * tmpvar_44));
  highp vec4 tmpvar_46;
  tmpvar_46 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_42 * tmpvar_19.x) + (tmpvar_43 * tmpvar_19.y)) + (tmpvar_44 * tmpvar_19.z))
   * 
    inversesqrt(tmpvar_45)
  )) * (1.0/((1.0 + 
    (tmpvar_45 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_47;
  tmpvar_47 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_46.x) + (unity_LightColor[1].xyz * tmpvar_46.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_46.z)
  ) + (unity_LightColor[3].xyz * tmpvar_46.w)));
  tmpvar_6 = tmpvar_47;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (tmpvar_22 * ((
    (_World2Object * tmpvar_24)
  .xyz * unity_Scale.w) - tmpvar_7.xyz));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform highp mat4 _Object2World;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _ReflectFaceColor;
uniform lowp vec4 _ReflectOutlineColor;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_3 = vec3(0.0, 0.0, 0.0);
  tmpvar_4 = tmpvar_2;
  tmpvar_5 = vec3(0.0, 0.0, 0.0);
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture2D (_MainTex, P_13).w;
  tmpvar_17.y = texture2D (_MainTex, P_14).w;
  tmpvar_17.z = texture2D (_MainTex, P_15).w;
  tmpvar_17.w = texture2D (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = normalize(((
    (tmpvar_43.yzx * tmpvar_45.zxy)
   - 
    (tmpvar_43.zxy * tmpvar_45.yzx)
  ) - tmpvar_47));
  highp mat3 tmpvar_49;
  tmpvar_49[0] = _Object2World[0].xyz;
  tmpvar_49[1] = _Object2World[1].xyz;
  tmpvar_49[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_50;
  highp vec3 N_51;
  N_51 = (tmpvar_49 * tmpvar_48);
  tmpvar_50 = (xlv_TEXCOORD2 - (2.0 * (
    dot (N_51, xlv_TEXCOORD2)
   * N_51)));
  lowp vec4 tmpvar_52;
  tmpvar_52 = textureCube (_Cube, tmpvar_50);
  highp float tmpvar_53;
  tmpvar_53 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  lowp vec3 tmpvar_54;
  tmpvar_54 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_53));
  highp vec3 tmpvar_55;
  tmpvar_55 = ((tmpvar_52.xyz * tmpvar_54) * faceColor_9.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = faceColor_9.xyz;
  tmpvar_3 = tmpvar_56;
  highp vec3 tmpvar_57;
  tmpvar_57 = -(tmpvar_48);
  tmpvar_4 = tmpvar_57;
  tmpvar_5 = tmpvar_55;
  highp float tmpvar_58;
  tmpvar_58 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_59;
  tmpvar_59 = faceColor_9.w;
  tmpvar_6 = tmpvar_59;
  tmpvar_2 = tmpvar_4;
  highp vec3 tmpvar_60;
  tmpvar_60 = normalize(xlv_TEXCOORD5);
  mediump vec3 viewDir_61;
  viewDir_61 = tmpvar_60;
  lowp vec4 c_62;
  highp float nh_63;
  lowp float tmpvar_64;
  tmpvar_64 = max (0.0, dot (tmpvar_4, xlv_TEXCOORD3));
  mediump float tmpvar_65;
  tmpvar_65 = max (0.0, dot (tmpvar_4, normalize(
    (xlv_TEXCOORD3 + viewDir_61)
  )));
  nh_63 = tmpvar_65;
  mediump float y_66;
  y_66 = (mix (_FaceShininess, _OutlineShininess, tmpvar_58) * 128.0);
  highp float tmpvar_67;
  tmpvar_67 = pow (nh_63, y_66);
  highp vec3 tmpvar_68;
  tmpvar_68 = (((
    (tmpvar_3 * _LightColor0.xyz)
   * tmpvar_64) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_67)) * 2.0);
  c_62.xyz = tmpvar_68;
  highp float tmpvar_69;
  tmpvar_69 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_67));
  c_62.w = tmpvar_69;
  c_1.w = c_62.w;
  c_1.xyz = (c_62.xyz + (tmpvar_3 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_5);
  c_1.w = tmpvar_6;
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
out highp vec3 xlv_TEXCOORD5;
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
  tmpvar_24.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_25;
  tmpvar_25.w = 1.0;
  tmpvar_25.xyz = tmpvar_19;
  mediump vec3 tmpvar_26;
  mediump vec4 normal_27;
  normal_27 = tmpvar_25;
  highp float vC_28;
  mediump vec3 x3_29;
  mediump vec3 x2_30;
  mediump vec3 x1_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAr, normal_27);
  x1_31.x = tmpvar_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAg, normal_27);
  x1_31.y = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAb, normal_27);
  x1_31.z = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (normal_27.xyzz * normal_27.yzzx);
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBr, tmpvar_35);
  x2_30.x = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBg, tmpvar_35);
  x2_30.y = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBb, tmpvar_35);
  x2_30.z = tmpvar_38;
  mediump float tmpvar_39;
  tmpvar_39 = ((normal_27.x * normal_27.x) - (normal_27.y * normal_27.y));
  vC_28 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (unity_SHC.xyz * vC_28);
  x3_29 = tmpvar_40;
  tmpvar_26 = ((x1_31 + x2_30) + x3_29);
  shlight_3 = tmpvar_26;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_41;
  tmpvar_41 = (_Object2World * tmpvar_7).xyz;
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosX0 - tmpvar_41.x);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosY0 - tmpvar_41.y);
  highp vec4 tmpvar_44;
  tmpvar_44 = (unity_4LightPosZ0 - tmpvar_41.z);
  highp vec4 tmpvar_45;
  tmpvar_45 = (((tmpvar_42 * tmpvar_42) + (tmpvar_43 * tmpvar_43)) + (tmpvar_44 * tmpvar_44));
  highp vec4 tmpvar_46;
  tmpvar_46 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_42 * tmpvar_19.x) + (tmpvar_43 * tmpvar_19.y)) + (tmpvar_44 * tmpvar_19.z))
   * 
    inversesqrt(tmpvar_45)
  )) * (1.0/((1.0 + 
    (tmpvar_45 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_47;
  tmpvar_47 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_46.x) + (unity_LightColor[1].xyz * tmpvar_46.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_46.z)
  ) + (unity_LightColor[3].xyz * tmpvar_46.w)));
  tmpvar_6 = tmpvar_47;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (tmpvar_22 * ((
    (_World2Object * tmpvar_24)
  .xyz * unity_Scale.w) - tmpvar_7.xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform highp mat4 _Object2World;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _ReflectFaceColor;
uniform lowp vec4 _ReflectOutlineColor;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_3 = vec3(0.0, 0.0, 0.0);
  tmpvar_4 = tmpvar_2;
  tmpvar_5 = vec3(0.0, 0.0, 0.0);
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture (_MainTex, P_13).w;
  tmpvar_17.y = texture (_MainTex, P_14).w;
  tmpvar_17.z = texture (_MainTex, P_15).w;
  tmpvar_17.w = texture (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = normalize(((
    (tmpvar_43.yzx * tmpvar_45.zxy)
   - 
    (tmpvar_43.zxy * tmpvar_45.yzx)
  ) - tmpvar_47));
  highp mat3 tmpvar_49;
  tmpvar_49[0] = _Object2World[0].xyz;
  tmpvar_49[1] = _Object2World[1].xyz;
  tmpvar_49[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_50;
  highp vec3 N_51;
  N_51 = (tmpvar_49 * tmpvar_48);
  tmpvar_50 = (xlv_TEXCOORD2 - (2.0 * (
    dot (N_51, xlv_TEXCOORD2)
   * N_51)));
  lowp vec4 tmpvar_52;
  tmpvar_52 = texture (_Cube, tmpvar_50);
  highp float tmpvar_53;
  tmpvar_53 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  lowp vec3 tmpvar_54;
  tmpvar_54 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_53));
  highp vec3 tmpvar_55;
  tmpvar_55 = ((tmpvar_52.xyz * tmpvar_54) * faceColor_9.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = faceColor_9.xyz;
  tmpvar_3 = tmpvar_56;
  highp vec3 tmpvar_57;
  tmpvar_57 = -(tmpvar_48);
  tmpvar_4 = tmpvar_57;
  tmpvar_5 = tmpvar_55;
  highp float tmpvar_58;
  tmpvar_58 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_59;
  tmpvar_59 = faceColor_9.w;
  tmpvar_6 = tmpvar_59;
  tmpvar_2 = tmpvar_4;
  highp vec3 tmpvar_60;
  tmpvar_60 = normalize(xlv_TEXCOORD5);
  mediump vec3 viewDir_61;
  viewDir_61 = tmpvar_60;
  lowp vec4 c_62;
  highp float nh_63;
  lowp float tmpvar_64;
  tmpvar_64 = max (0.0, dot (tmpvar_4, xlv_TEXCOORD3));
  mediump float tmpvar_65;
  tmpvar_65 = max (0.0, dot (tmpvar_4, normalize(
    (xlv_TEXCOORD3 + viewDir_61)
  )));
  nh_63 = tmpvar_65;
  mediump float y_66;
  y_66 = (mix (_FaceShininess, _OutlineShininess, tmpvar_58) * 128.0);
  highp float tmpvar_67;
  tmpvar_67 = pow (nh_63, y_66);
  highp vec3 tmpvar_68;
  tmpvar_68 = (((
    (tmpvar_3 * _LightColor0.xyz)
   * tmpvar_64) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_67)) * 2.0);
  c_62.xyz = tmpvar_68;
  highp float tmpvar_69;
  tmpvar_69 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_67));
  c_62.w = tmpvar_69;
  c_1.w = c_62.w;
  c_1.xyz = (c_62.xyz + (tmpvar_3 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_5);
  c_1.w = tmpvar_6;
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
varying highp vec3 xlv_TEXCOORD5;
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
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
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
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (tmpvar_21 * ((
    (_World2Object * tmpvar_23)
  .xyz * unity_Scale.w) - tmpvar_7.xyz));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform highp mat4 _Object2World;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _ReflectFaceColor;
uniform lowp vec4 _ReflectOutlineColor;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_3 = vec3(0.0, 0.0, 0.0);
  tmpvar_4 = tmpvar_2;
  tmpvar_5 = vec3(0.0, 0.0, 0.0);
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture2D (_MainTex, P_14).w;
  tmpvar_18.y = texture2D (_MainTex, P_15).w;
  tmpvar_18.z = texture2D (_MainTex, P_16).w;
  tmpvar_18.w = texture2D (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = normalize(((
    (tmpvar_44.yzx * tmpvar_46.zxy)
   - 
    (tmpvar_44.zxy * tmpvar_46.yzx)
  ) - tmpvar_48));
  highp mat3 tmpvar_50;
  tmpvar_50[0] = _Object2World[0].xyz;
  tmpvar_50[1] = _Object2World[1].xyz;
  tmpvar_50[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_51;
  highp vec3 N_52;
  N_52 = (tmpvar_50 * tmpvar_49);
  tmpvar_51 = (xlv_TEXCOORD2 - (2.0 * (
    dot (N_52, xlv_TEXCOORD2)
   * N_52)));
  lowp vec4 tmpvar_53;
  tmpvar_53 = textureCube (_Cube, tmpvar_51);
  highp float tmpvar_54;
  tmpvar_54 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  lowp vec3 tmpvar_55;
  tmpvar_55 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_54));
  highp vec4 tmpvar_56;
  highp float tmpvar_57;
  tmpvar_57 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_58;
  tmpvar_58 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_57 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_59;
  tmpvar_59 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_57 / (1.0 + tmpvar_58)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_58)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_60;
  tmpvar_60.xyz = _GlowColor.xyz;
  tmpvar_60.w = tmpvar_59;
  tmpvar_56 = tmpvar_60;
  glowColor_7.xyz = tmpvar_56.xyz;
  glowColor_7.w = (tmpvar_56.w * xlv_COLOR0.w);
  highp vec3 tmpvar_61;
  tmpvar_61 = (((tmpvar_53.xyz * tmpvar_55) * faceColor_10.w) + (tmpvar_56.xyz * glowColor_7.w));
  highp vec4 overlying_62;
  overlying_62.w = glowColor_7.w;
  highp vec4 underlying_63;
  underlying_63.w = faceColor_10.w;
  overlying_62.xyz = (tmpvar_56.xyz * glowColor_7.w);
  underlying_63.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_64;
  tmpvar_64 = (overlying_62.xyz + ((1.0 - glowColor_7.w) * underlying_63.xyz));
  highp float tmpvar_65;
  tmpvar_65 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_66;
  tmpvar_66.xyz = tmpvar_64;
  tmpvar_66.w = tmpvar_65;
  faceColor_10.w = tmpvar_66.w;
  faceColor_10.xyz = (tmpvar_64 / tmpvar_65);
  highp vec3 tmpvar_67;
  tmpvar_67 = faceColor_10.xyz;
  tmpvar_3 = tmpvar_67;
  highp vec3 tmpvar_68;
  tmpvar_68 = -(tmpvar_49);
  tmpvar_4 = tmpvar_68;
  tmpvar_5 = tmpvar_61;
  highp float tmpvar_69;
  tmpvar_69 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_70;
  tmpvar_70 = faceColor_10.w;
  tmpvar_6 = tmpvar_70;
  tmpvar_2 = tmpvar_4;
  highp vec3 tmpvar_71;
  tmpvar_71 = normalize(xlv_TEXCOORD5);
  mediump vec3 viewDir_72;
  viewDir_72 = tmpvar_71;
  lowp vec4 c_73;
  highp float nh_74;
  lowp float tmpvar_75;
  tmpvar_75 = max (0.0, dot (tmpvar_4, xlv_TEXCOORD3));
  mediump float tmpvar_76;
  tmpvar_76 = max (0.0, dot (tmpvar_4, normalize(
    (xlv_TEXCOORD3 + viewDir_72)
  )));
  nh_74 = tmpvar_76;
  mediump float y_77;
  y_77 = (mix (_FaceShininess, _OutlineShininess, tmpvar_69) * 128.0);
  highp float tmpvar_78;
  tmpvar_78 = pow (nh_74, y_77);
  highp vec3 tmpvar_79;
  tmpvar_79 = (((
    (tmpvar_3 * _LightColor0.xyz)
   * tmpvar_75) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_78)) * 2.0);
  c_73.xyz = tmpvar_79;
  highp float tmpvar_80;
  tmpvar_80 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_78));
  c_73.w = tmpvar_80;
  c_1.w = c_73.w;
  c_1.xyz = (c_73.xyz + (tmpvar_3 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_5);
  c_1.w = tmpvar_6;
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
out highp vec3 xlv_TEXCOORD5;
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
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = (tmpvar_18 * (tmpvar_10 * unity_Scale.w));
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
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (tmpvar_21 * ((
    (_World2Object * tmpvar_23)
  .xyz * unity_Scale.w) - tmpvar_7.xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform highp mat4 _Object2World;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _ReflectFaceColor;
uniform lowp vec4 _ReflectOutlineColor;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_3 = vec3(0.0, 0.0, 0.0);
  tmpvar_4 = tmpvar_2;
  tmpvar_5 = vec3(0.0, 0.0, 0.0);
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture (_MainTex, P_14).w;
  tmpvar_18.y = texture (_MainTex, P_15).w;
  tmpvar_18.z = texture (_MainTex, P_16).w;
  tmpvar_18.w = texture (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = normalize(((
    (tmpvar_44.yzx * tmpvar_46.zxy)
   - 
    (tmpvar_44.zxy * tmpvar_46.yzx)
  ) - tmpvar_48));
  highp mat3 tmpvar_50;
  tmpvar_50[0] = _Object2World[0].xyz;
  tmpvar_50[1] = _Object2World[1].xyz;
  tmpvar_50[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_51;
  highp vec3 N_52;
  N_52 = (tmpvar_50 * tmpvar_49);
  tmpvar_51 = (xlv_TEXCOORD2 - (2.0 * (
    dot (N_52, xlv_TEXCOORD2)
   * N_52)));
  lowp vec4 tmpvar_53;
  tmpvar_53 = texture (_Cube, tmpvar_51);
  highp float tmpvar_54;
  tmpvar_54 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  lowp vec3 tmpvar_55;
  tmpvar_55 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_54));
  highp vec4 tmpvar_56;
  highp float tmpvar_57;
  tmpvar_57 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_58;
  tmpvar_58 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_57 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_59;
  tmpvar_59 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_57 / (1.0 + tmpvar_58)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_58)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_60;
  tmpvar_60.xyz = _GlowColor.xyz;
  tmpvar_60.w = tmpvar_59;
  tmpvar_56 = tmpvar_60;
  glowColor_7.xyz = tmpvar_56.xyz;
  glowColor_7.w = (tmpvar_56.w * xlv_COLOR0.w);
  highp vec3 tmpvar_61;
  tmpvar_61 = (((tmpvar_53.xyz * tmpvar_55) * faceColor_10.w) + (tmpvar_56.xyz * glowColor_7.w));
  highp vec4 overlying_62;
  overlying_62.w = glowColor_7.w;
  highp vec4 underlying_63;
  underlying_63.w = faceColor_10.w;
  overlying_62.xyz = (tmpvar_56.xyz * glowColor_7.w);
  underlying_63.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_64;
  tmpvar_64 = (overlying_62.xyz + ((1.0 - glowColor_7.w) * underlying_63.xyz));
  highp float tmpvar_65;
  tmpvar_65 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_66;
  tmpvar_66.xyz = tmpvar_64;
  tmpvar_66.w = tmpvar_65;
  faceColor_10.w = tmpvar_66.w;
  faceColor_10.xyz = (tmpvar_64 / tmpvar_65);
  highp vec3 tmpvar_67;
  tmpvar_67 = faceColor_10.xyz;
  tmpvar_3 = tmpvar_67;
  highp vec3 tmpvar_68;
  tmpvar_68 = -(tmpvar_49);
  tmpvar_4 = tmpvar_68;
  tmpvar_5 = tmpvar_61;
  highp float tmpvar_69;
  tmpvar_69 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_70;
  tmpvar_70 = faceColor_10.w;
  tmpvar_6 = tmpvar_70;
  tmpvar_2 = tmpvar_4;
  highp vec3 tmpvar_71;
  tmpvar_71 = normalize(xlv_TEXCOORD5);
  mediump vec3 viewDir_72;
  viewDir_72 = tmpvar_71;
  lowp vec4 c_73;
  highp float nh_74;
  lowp float tmpvar_75;
  tmpvar_75 = max (0.0, dot (tmpvar_4, xlv_TEXCOORD3));
  mediump float tmpvar_76;
  tmpvar_76 = max (0.0, dot (tmpvar_4, normalize(
    (xlv_TEXCOORD3 + viewDir_72)
  )));
  nh_74 = tmpvar_76;
  mediump float y_77;
  y_77 = (mix (_FaceShininess, _OutlineShininess, tmpvar_69) * 128.0);
  highp float tmpvar_78;
  tmpvar_78 = pow (nh_74, y_77);
  highp vec3 tmpvar_79;
  tmpvar_79 = (((
    (tmpvar_3 * _LightColor0.xyz)
   * tmpvar_75) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_78)) * 2.0);
  c_73.xyz = tmpvar_79;
  highp float tmpvar_80;
  tmpvar_80 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_78));
  c_73.w = tmpvar_80;
  c_1.w = c_73.w;
  c_1.xyz = (c_73.xyz + (tmpvar_3 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_5);
  c_1.w = tmpvar_6;
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
varying highp vec3 xlv_TEXCOORD5;
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
  tmpvar_24.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_25;
  tmpvar_25.w = 1.0;
  tmpvar_25.xyz = tmpvar_19;
  mediump vec3 tmpvar_26;
  mediump vec4 normal_27;
  normal_27 = tmpvar_25;
  highp float vC_28;
  mediump vec3 x3_29;
  mediump vec3 x2_30;
  mediump vec3 x1_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAr, normal_27);
  x1_31.x = tmpvar_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAg, normal_27);
  x1_31.y = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAb, normal_27);
  x1_31.z = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (normal_27.xyzz * normal_27.yzzx);
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBr, tmpvar_35);
  x2_30.x = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBg, tmpvar_35);
  x2_30.y = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBb, tmpvar_35);
  x2_30.z = tmpvar_38;
  mediump float tmpvar_39;
  tmpvar_39 = ((normal_27.x * normal_27.x) - (normal_27.y * normal_27.y));
  vC_28 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (unity_SHC.xyz * vC_28);
  x3_29 = tmpvar_40;
  tmpvar_26 = ((x1_31 + x2_30) + x3_29);
  shlight_3 = tmpvar_26;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_41;
  tmpvar_41 = (_Object2World * tmpvar_7).xyz;
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosX0 - tmpvar_41.x);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosY0 - tmpvar_41.y);
  highp vec4 tmpvar_44;
  tmpvar_44 = (unity_4LightPosZ0 - tmpvar_41.z);
  highp vec4 tmpvar_45;
  tmpvar_45 = (((tmpvar_42 * tmpvar_42) + (tmpvar_43 * tmpvar_43)) + (tmpvar_44 * tmpvar_44));
  highp vec4 tmpvar_46;
  tmpvar_46 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_42 * tmpvar_19.x) + (tmpvar_43 * tmpvar_19.y)) + (tmpvar_44 * tmpvar_19.z))
   * 
    inversesqrt(tmpvar_45)
  )) * (1.0/((1.0 + 
    (tmpvar_45 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_47;
  tmpvar_47 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_46.x) + (unity_LightColor[1].xyz * tmpvar_46.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_46.z)
  ) + (unity_LightColor[3].xyz * tmpvar_46.w)));
  tmpvar_6 = tmpvar_47;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (tmpvar_22 * ((
    (_World2Object * tmpvar_24)
  .xyz * unity_Scale.w) - tmpvar_7.xyz));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform highp mat4 _Object2World;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _ReflectFaceColor;
uniform lowp vec4 _ReflectOutlineColor;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_3 = vec3(0.0, 0.0, 0.0);
  tmpvar_4 = tmpvar_2;
  tmpvar_5 = vec3(0.0, 0.0, 0.0);
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture2D (_MainTex, P_14).w;
  tmpvar_18.y = texture2D (_MainTex, P_15).w;
  tmpvar_18.z = texture2D (_MainTex, P_16).w;
  tmpvar_18.w = texture2D (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = normalize(((
    (tmpvar_44.yzx * tmpvar_46.zxy)
   - 
    (tmpvar_44.zxy * tmpvar_46.yzx)
  ) - tmpvar_48));
  highp mat3 tmpvar_50;
  tmpvar_50[0] = _Object2World[0].xyz;
  tmpvar_50[1] = _Object2World[1].xyz;
  tmpvar_50[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_51;
  highp vec3 N_52;
  N_52 = (tmpvar_50 * tmpvar_49);
  tmpvar_51 = (xlv_TEXCOORD2 - (2.0 * (
    dot (N_52, xlv_TEXCOORD2)
   * N_52)));
  lowp vec4 tmpvar_53;
  tmpvar_53 = textureCube (_Cube, tmpvar_51);
  highp float tmpvar_54;
  tmpvar_54 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  lowp vec3 tmpvar_55;
  tmpvar_55 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_54));
  highp vec4 tmpvar_56;
  highp float tmpvar_57;
  tmpvar_57 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_58;
  tmpvar_58 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_57 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_59;
  tmpvar_59 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_57 / (1.0 + tmpvar_58)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_58)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_60;
  tmpvar_60.xyz = _GlowColor.xyz;
  tmpvar_60.w = tmpvar_59;
  tmpvar_56 = tmpvar_60;
  glowColor_7.xyz = tmpvar_56.xyz;
  glowColor_7.w = (tmpvar_56.w * xlv_COLOR0.w);
  highp vec3 tmpvar_61;
  tmpvar_61 = (((tmpvar_53.xyz * tmpvar_55) * faceColor_10.w) + (tmpvar_56.xyz * glowColor_7.w));
  highp vec4 overlying_62;
  overlying_62.w = glowColor_7.w;
  highp vec4 underlying_63;
  underlying_63.w = faceColor_10.w;
  overlying_62.xyz = (tmpvar_56.xyz * glowColor_7.w);
  underlying_63.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_64;
  tmpvar_64 = (overlying_62.xyz + ((1.0 - glowColor_7.w) * underlying_63.xyz));
  highp float tmpvar_65;
  tmpvar_65 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_66;
  tmpvar_66.xyz = tmpvar_64;
  tmpvar_66.w = tmpvar_65;
  faceColor_10.w = tmpvar_66.w;
  faceColor_10.xyz = (tmpvar_64 / tmpvar_65);
  highp vec3 tmpvar_67;
  tmpvar_67 = faceColor_10.xyz;
  tmpvar_3 = tmpvar_67;
  highp vec3 tmpvar_68;
  tmpvar_68 = -(tmpvar_49);
  tmpvar_4 = tmpvar_68;
  tmpvar_5 = tmpvar_61;
  highp float tmpvar_69;
  tmpvar_69 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_70;
  tmpvar_70 = faceColor_10.w;
  tmpvar_6 = tmpvar_70;
  tmpvar_2 = tmpvar_4;
  highp vec3 tmpvar_71;
  tmpvar_71 = normalize(xlv_TEXCOORD5);
  mediump vec3 viewDir_72;
  viewDir_72 = tmpvar_71;
  lowp vec4 c_73;
  highp float nh_74;
  lowp float tmpvar_75;
  tmpvar_75 = max (0.0, dot (tmpvar_4, xlv_TEXCOORD3));
  mediump float tmpvar_76;
  tmpvar_76 = max (0.0, dot (tmpvar_4, normalize(
    (xlv_TEXCOORD3 + viewDir_72)
  )));
  nh_74 = tmpvar_76;
  mediump float y_77;
  y_77 = (mix (_FaceShininess, _OutlineShininess, tmpvar_69) * 128.0);
  highp float tmpvar_78;
  tmpvar_78 = pow (nh_74, y_77);
  highp vec3 tmpvar_79;
  tmpvar_79 = (((
    (tmpvar_3 * _LightColor0.xyz)
   * tmpvar_75) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_78)) * 2.0);
  c_73.xyz = tmpvar_79;
  highp float tmpvar_80;
  tmpvar_80 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_78));
  c_73.w = tmpvar_80;
  c_1.w = c_73.w;
  c_1.xyz = (c_73.xyz + (tmpvar_3 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_5);
  c_1.w = tmpvar_6;
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
out highp vec3 xlv_TEXCOORD5;
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
  tmpvar_24.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_25;
  tmpvar_25.w = 1.0;
  tmpvar_25.xyz = tmpvar_19;
  mediump vec3 tmpvar_26;
  mediump vec4 normal_27;
  normal_27 = tmpvar_25;
  highp float vC_28;
  mediump vec3 x3_29;
  mediump vec3 x2_30;
  mediump vec3 x1_31;
  highp float tmpvar_32;
  tmpvar_32 = dot (unity_SHAr, normal_27);
  x1_31.x = tmpvar_32;
  highp float tmpvar_33;
  tmpvar_33 = dot (unity_SHAg, normal_27);
  x1_31.y = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAb, normal_27);
  x1_31.z = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (normal_27.xyzz * normal_27.yzzx);
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHBr, tmpvar_35);
  x2_30.x = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHBg, tmpvar_35);
  x2_30.y = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBb, tmpvar_35);
  x2_30.z = tmpvar_38;
  mediump float tmpvar_39;
  tmpvar_39 = ((normal_27.x * normal_27.x) - (normal_27.y * normal_27.y));
  vC_28 = tmpvar_39;
  highp vec3 tmpvar_40;
  tmpvar_40 = (unity_SHC.xyz * vC_28);
  x3_29 = tmpvar_40;
  tmpvar_26 = ((x1_31 + x2_30) + x3_29);
  shlight_3 = tmpvar_26;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_41;
  tmpvar_41 = (_Object2World * tmpvar_7).xyz;
  highp vec4 tmpvar_42;
  tmpvar_42 = (unity_4LightPosX0 - tmpvar_41.x);
  highp vec4 tmpvar_43;
  tmpvar_43 = (unity_4LightPosY0 - tmpvar_41.y);
  highp vec4 tmpvar_44;
  tmpvar_44 = (unity_4LightPosZ0 - tmpvar_41.z);
  highp vec4 tmpvar_45;
  tmpvar_45 = (((tmpvar_42 * tmpvar_42) + (tmpvar_43 * tmpvar_43)) + (tmpvar_44 * tmpvar_44));
  highp vec4 tmpvar_46;
  tmpvar_46 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_42 * tmpvar_19.x) + (tmpvar_43 * tmpvar_19.y)) + (tmpvar_44 * tmpvar_19.z))
   * 
    inversesqrt(tmpvar_45)
  )) * (1.0/((1.0 + 
    (tmpvar_45 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_47;
  tmpvar_47 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_46.x) + (unity_LightColor[1].xyz * tmpvar_46.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_46.z)
  ) + (unity_LightColor[3].xyz * tmpvar_46.w)));
  tmpvar_6 = tmpvar_47;
  gl_Position = (glstate_matrix_mvp * tmpvar_7);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = (tmpvar_17 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_7).xyz));
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (tmpvar_22 * ((
    (_World2Object * tmpvar_24)
  .xyz * unity_Scale.w) - tmpvar_7.xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform highp mat4 _Object2World;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _ReflectFaceColor;
uniform lowp vec4 _ReflectOutlineColor;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_3 = vec3(0.0, 0.0, 0.0);
  tmpvar_4 = tmpvar_2;
  tmpvar_5 = vec3(0.0, 0.0, 0.0);
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture (_MainTex, P_14).w;
  tmpvar_18.y = texture (_MainTex, P_15).w;
  tmpvar_18.z = texture (_MainTex, P_16).w;
  tmpvar_18.w = texture (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = normalize(((
    (tmpvar_44.yzx * tmpvar_46.zxy)
   - 
    (tmpvar_44.zxy * tmpvar_46.yzx)
  ) - tmpvar_48));
  highp mat3 tmpvar_50;
  tmpvar_50[0] = _Object2World[0].xyz;
  tmpvar_50[1] = _Object2World[1].xyz;
  tmpvar_50[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_51;
  highp vec3 N_52;
  N_52 = (tmpvar_50 * tmpvar_49);
  tmpvar_51 = (xlv_TEXCOORD2 - (2.0 * (
    dot (N_52, xlv_TEXCOORD2)
   * N_52)));
  lowp vec4 tmpvar_53;
  tmpvar_53 = texture (_Cube, tmpvar_51);
  highp float tmpvar_54;
  tmpvar_54 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  lowp vec3 tmpvar_55;
  tmpvar_55 = mix (_ReflectFaceColor.xyz, _ReflectOutlineColor.xyz, vec3(tmpvar_54));
  highp vec4 tmpvar_56;
  highp float tmpvar_57;
  tmpvar_57 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_58;
  tmpvar_58 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_57 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_59;
  tmpvar_59 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_57 / (1.0 + tmpvar_58)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_58)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_60;
  tmpvar_60.xyz = _GlowColor.xyz;
  tmpvar_60.w = tmpvar_59;
  tmpvar_56 = tmpvar_60;
  glowColor_7.xyz = tmpvar_56.xyz;
  glowColor_7.w = (tmpvar_56.w * xlv_COLOR0.w);
  highp vec3 tmpvar_61;
  tmpvar_61 = (((tmpvar_53.xyz * tmpvar_55) * faceColor_10.w) + (tmpvar_56.xyz * glowColor_7.w));
  highp vec4 overlying_62;
  overlying_62.w = glowColor_7.w;
  highp vec4 underlying_63;
  underlying_63.w = faceColor_10.w;
  overlying_62.xyz = (tmpvar_56.xyz * glowColor_7.w);
  underlying_63.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_64;
  tmpvar_64 = (overlying_62.xyz + ((1.0 - glowColor_7.w) * underlying_63.xyz));
  highp float tmpvar_65;
  tmpvar_65 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_66;
  tmpvar_66.xyz = tmpvar_64;
  tmpvar_66.w = tmpvar_65;
  faceColor_10.w = tmpvar_66.w;
  faceColor_10.xyz = (tmpvar_64 / tmpvar_65);
  highp vec3 tmpvar_67;
  tmpvar_67 = faceColor_10.xyz;
  tmpvar_3 = tmpvar_67;
  highp vec3 tmpvar_68;
  tmpvar_68 = -(tmpvar_49);
  tmpvar_4 = tmpvar_68;
  tmpvar_5 = tmpvar_61;
  highp float tmpvar_69;
  tmpvar_69 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_70;
  tmpvar_70 = faceColor_10.w;
  tmpvar_6 = tmpvar_70;
  tmpvar_2 = tmpvar_4;
  highp vec3 tmpvar_71;
  tmpvar_71 = normalize(xlv_TEXCOORD5);
  mediump vec3 viewDir_72;
  viewDir_72 = tmpvar_71;
  lowp vec4 c_73;
  highp float nh_74;
  lowp float tmpvar_75;
  tmpvar_75 = max (0.0, dot (tmpvar_4, xlv_TEXCOORD3));
  mediump float tmpvar_76;
  tmpvar_76 = max (0.0, dot (tmpvar_4, normalize(
    (xlv_TEXCOORD3 + viewDir_72)
  )));
  nh_74 = tmpvar_76;
  mediump float y_77;
  y_77 = (mix (_FaceShininess, _OutlineShininess, tmpvar_69) * 128.0);
  highp float tmpvar_78;
  tmpvar_78 = pow (nh_74, y_77);
  highp vec3 tmpvar_79;
  tmpvar_79 = (((
    (tmpvar_3 * _LightColor0.xyz)
   * tmpvar_75) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_78)) * 2.0);
  c_73.xyz = tmpvar_79;
  highp float tmpvar_80;
  tmpvar_80 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_78));
  c_73.w = tmpvar_80;
  c_1.w = c_73.w;
  c_1.xyz = (c_73.xyz + (tmpvar_3 * xlv_TEXCOORD4));
  c_1.xyz = (c_1.xyz + tmpvar_5);
  c_1.w = tmpvar_6;
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
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull [_CullMode]
  Fog {
   Color (0,0,0,0)
  }
  Blend SrcAlpha One
  AlphaTest Greater 0
  ColorMask RGB
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" "GLOW_OFF" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xyz;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture2D (_MainTex, P_13).w;
  tmpvar_17.y = texture2D (_MainTex, P_14).w;
  tmpvar_17.z = texture2D (_MainTex, P_15).w;
  tmpvar_17.w = texture2D (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_52;
  tmpvar_52 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_52;
  highp float tmpvar_53;
  tmpvar_53 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp float atten_54;
  atten_54 = texture2D (_LightTexture0, vec2(tmpvar_53)).w;
  lowp vec4 c_55;
  highp float nh_56;
  lowp float tmpvar_57;
  tmpvar_57 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_58;
  tmpvar_58 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_56 = tmpvar_58;
  mediump float y_59;
  y_59 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_60;
  tmpvar_60 = pow (nh_56, y_59);
  highp vec3 tmpvar_61;
  tmpvar_61 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_57) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_60)) * (atten_54 * 2.0));
  c_55.xyz = tmpvar_61;
  highp float tmpvar_62;
  tmpvar_62 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_60) * atten_54));
  c_55.w = tmpvar_62;
  c_1.xyz = c_55.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "POINT" "GLOW_OFF" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xyz;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture (_MainTex, P_13).w;
  tmpvar_17.y = texture (_MainTex, P_14).w;
  tmpvar_17.z = texture (_MainTex, P_15).w;
  tmpvar_17.w = texture (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_52;
  tmpvar_52 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_52;
  highp float tmpvar_53;
  tmpvar_53 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp float atten_54;
  atten_54 = texture (_LightTexture0, vec2(tmpvar_53)).w;
  lowp vec4 c_55;
  highp float nh_56;
  lowp float tmpvar_57;
  tmpvar_57 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_58;
  tmpvar_58 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_56 = tmpvar_58;
  mediump float y_59;
  y_59 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_60;
  tmpvar_60 = pow (nh_56, y_59);
  highp vec3 tmpvar_61;
  tmpvar_61 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_57) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_60)) * (atten_54 * 2.0));
  c_55.xyz = tmpvar_61;
  highp float tmpvar_62;
  tmpvar_62 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_60) * atten_54));
  c_55.w = tmpvar_62;
  c_1.xyz = c_55.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "GLOW_OFF" }
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture2D (_MainTex, P_13).w;
  tmpvar_17.y = texture2D (_MainTex, P_14).w;
  tmpvar_17.z = texture2D (_MainTex, P_15).w;
  tmpvar_17.w = texture2D (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  lightDir_2 = xlv_TEXCOORD3;
  lowp vec4 c_52;
  highp float nh_53;
  lowp float tmpvar_54;
  tmpvar_54 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_55;
  tmpvar_55 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_53 = tmpvar_55;
  mediump float y_56;
  y_56 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_57;
  tmpvar_57 = pow (nh_53, y_56);
  highp vec3 tmpvar_58;
  tmpvar_58 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_54) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_57)) * 2.0);
  c_52.xyz = tmpvar_58;
  highp float tmpvar_59;
  tmpvar_59 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_57));
  c_52.w = tmpvar_59;
  c_1.xyz = c_52.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "GLOW_OFF" }
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture (_MainTex, P_13).w;
  tmpvar_17.y = texture (_MainTex, P_14).w;
  tmpvar_17.z = texture (_MainTex, P_15).w;
  tmpvar_17.w = texture (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  lightDir_2 = xlv_TEXCOORD3;
  lowp vec4 c_52;
  highp float nh_53;
  lowp float tmpvar_54;
  tmpvar_54 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_55;
  tmpvar_55 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_53 = tmpvar_55;
  mediump float y_56;
  y_56 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_57;
  tmpvar_57 = pow (nh_53, y_56);
  highp vec3 tmpvar_58;
  tmpvar_58 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_54) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_57)) * 2.0);
  c_52.xyz = tmpvar_58;
  highp float tmpvar_59;
  tmpvar_59 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_57));
  c_52.w = tmpvar_59;
  c_1.xyz = c_52.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "GLOW_OFF" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
uniform sampler2D _LightTextureB0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture2D (_MainTex, P_13).w;
  tmpvar_17.y = texture2D (_MainTex, P_14).w;
  tmpvar_17.z = texture2D (_MainTex, P_15).w;
  tmpvar_17.w = texture2D (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_52;
  tmpvar_52 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_52;
  highp vec2 P_53;
  P_53 = ((xlv_TEXCOORD5.xy / xlv_TEXCOORD5.w) + 0.5);
  highp float tmpvar_54;
  tmpvar_54 = dot (xlv_TEXCOORD5.xyz, xlv_TEXCOORD5.xyz);
  lowp float atten_55;
  atten_55 = ((float(
    (xlv_TEXCOORD5.z > 0.0)
  ) * texture2D (_LightTexture0, P_53).w) * texture2D (_LightTextureB0, vec2(tmpvar_54)).w);
  lowp vec4 c_56;
  highp float nh_57;
  lowp float tmpvar_58;
  tmpvar_58 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_59;
  tmpvar_59 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_57 = tmpvar_59;
  mediump float y_60;
  y_60 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_61;
  tmpvar_61 = pow (nh_57, y_60);
  highp vec3 tmpvar_62;
  tmpvar_62 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_58) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_61)) * (atten_55 * 2.0));
  c_56.xyz = tmpvar_62;
  highp float tmpvar_63;
  tmpvar_63 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_61) * atten_55));
  c_56.w = tmpvar_63;
  c_1.xyz = c_56.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "SPOT" "GLOW_OFF" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
uniform sampler2D _LightTextureB0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture (_MainTex, P_13).w;
  tmpvar_17.y = texture (_MainTex, P_14).w;
  tmpvar_17.z = texture (_MainTex, P_15).w;
  tmpvar_17.w = texture (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_52;
  tmpvar_52 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_52;
  highp vec2 P_53;
  P_53 = ((xlv_TEXCOORD5.xy / xlv_TEXCOORD5.w) + 0.5);
  highp float tmpvar_54;
  tmpvar_54 = dot (xlv_TEXCOORD5.xyz, xlv_TEXCOORD5.xyz);
  lowp float atten_55;
  atten_55 = ((float(
    (xlv_TEXCOORD5.z > 0.0)
  ) * texture (_LightTexture0, P_53).w) * texture (_LightTextureB0, vec2(tmpvar_54)).w);
  lowp vec4 c_56;
  highp float nh_57;
  lowp float tmpvar_58;
  tmpvar_58 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_59;
  tmpvar_59 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_57 = tmpvar_59;
  mediump float y_60;
  y_60 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_61;
  tmpvar_61 = pow (nh_57, y_60);
  highp vec3 tmpvar_62;
  tmpvar_62 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_58) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_61)) * (atten_55 * 2.0));
  c_56.xyz = tmpvar_62;
  highp float tmpvar_63;
  tmpvar_63 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_61) * atten_55));
  c_56.w = tmpvar_63;
  c_1.xyz = c_56.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "GLOW_OFF" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xyz;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _LightTextureB0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture2D (_MainTex, P_13).w;
  tmpvar_17.y = texture2D (_MainTex, P_14).w;
  tmpvar_17.z = texture2D (_MainTex, P_15).w;
  tmpvar_17.w = texture2D (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_52;
  tmpvar_52 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_52;
  highp float tmpvar_53;
  tmpvar_53 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp float atten_54;
  atten_54 = (texture2D (_LightTextureB0, vec2(tmpvar_53)).w * textureCube (_LightTexture0, xlv_TEXCOORD5).w);
  lowp vec4 c_55;
  highp float nh_56;
  lowp float tmpvar_57;
  tmpvar_57 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_58;
  tmpvar_58 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_56 = tmpvar_58;
  mediump float y_59;
  y_59 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_60;
  tmpvar_60 = pow (nh_56, y_59);
  highp vec3 tmpvar_61;
  tmpvar_61 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_57) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_60)) * (atten_54 * 2.0));
  c_55.xyz = tmpvar_61;
  highp float tmpvar_62;
  tmpvar_62 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_60) * atten_54));
  c_55.w = tmpvar_62;
  c_1.xyz = c_55.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" "GLOW_OFF" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xyz;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _LightTextureB0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture (_MainTex, P_13).w;
  tmpvar_17.y = texture (_MainTex, P_14).w;
  tmpvar_17.z = texture (_MainTex, P_15).w;
  tmpvar_17.w = texture (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_52;
  tmpvar_52 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_52;
  highp float tmpvar_53;
  tmpvar_53 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp float atten_54;
  atten_54 = (texture (_LightTextureB0, vec2(tmpvar_53)).w * texture (_LightTexture0, xlv_TEXCOORD5).w);
  lowp vec4 c_55;
  highp float nh_56;
  lowp float tmpvar_57;
  tmpvar_57 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_58;
  tmpvar_58 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_56 = tmpvar_58;
  mediump float y_59;
  y_59 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_60;
  tmpvar_60 = pow (nh_56, y_59);
  highp vec3 tmpvar_61;
  tmpvar_61 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_57) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_60)) * (atten_54 * 2.0));
  c_55.xyz = tmpvar_61;
  highp float tmpvar_62;
  tmpvar_62 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_60) * atten_54));
  c_55.w = tmpvar_62;
  c_1.xyz = c_55.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "GLOW_OFF" }
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xy;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture2D (_MainTex, P_13).w;
  tmpvar_17.y = texture2D (_MainTex, P_14).w;
  tmpvar_17.z = texture2D (_MainTex, P_15).w;
  tmpvar_17.w = texture2D (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  lightDir_2 = xlv_TEXCOORD3;
  lowp float atten_52;
  atten_52 = texture2D (_LightTexture0, xlv_TEXCOORD5).w;
  lowp vec4 c_53;
  highp float nh_54;
  lowp float tmpvar_55;
  tmpvar_55 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_56;
  tmpvar_56 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_54 = tmpvar_56;
  mediump float y_57;
  y_57 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_58;
  tmpvar_58 = pow (nh_54, y_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_55) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_58)) * (atten_52 * 2.0));
  c_53.xyz = tmpvar_59;
  highp float tmpvar_60;
  tmpvar_60 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_58) * atten_52));
  c_53.w = tmpvar_60;
  c_1.xyz = c_53.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" "GLOW_OFF" }
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xy;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec3 bump_7;
  highp vec4 outlineColor_8;
  highp vec4 faceColor_9;
  highp float c_10;
  highp vec4 smp4x_11;
  highp vec3 tmpvar_12;
  tmpvar_12.z = 0.0;
  tmpvar_12.x = (1.0/(_TextureWidth));
  tmpvar_12.y = (1.0/(_TextureHeight));
  highp vec2 P_13;
  P_13 = (xlv_TEXCOORD0.xy - tmpvar_12.xz);
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy + tmpvar_12.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy - tmpvar_12.zy);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy + tmpvar_12.zy);
  lowp vec4 tmpvar_17;
  tmpvar_17.x = texture (_MainTex, P_13).w;
  tmpvar_17.y = texture (_MainTex, P_14).w;
  tmpvar_17.z = texture (_MainTex, P_15).w;
  tmpvar_17.w = texture (_MainTex, P_16).w;
  smp4x_11 = tmpvar_17;
  lowp float tmpvar_18;
  tmpvar_18 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_10 = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = (((
    (0.5 - c_10)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_20;
  tmpvar_20 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_9 = _FaceColor;
  outlineColor_8 = _OutlineColor;
  outlineColor_8.w = (outlineColor_8.w * xlv_COLOR0.w);
  highp vec2 tmpvar_22;
  tmpvar_22.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_22.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture (_FaceTex, tmpvar_22);
  highp vec4 tmpvar_24;
  tmpvar_24 = ((faceColor_9 * xlv_COLOR0) * tmpvar_23);
  highp vec2 tmpvar_25;
  tmpvar_25.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_25.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture (_OutlineTex, tmpvar_25);
  highp vec4 tmpvar_27;
  tmpvar_27 = (outlineColor_8 * tmpvar_26);
  outlineColor_8 = tmpvar_27;
  mediump float d_28;
  d_28 = tmpvar_19;
  lowp vec4 faceColor_29;
  faceColor_29 = tmpvar_24;
  lowp vec4 outlineColor_30;
  outlineColor_30 = tmpvar_27;
  mediump float outline_31;
  outline_31 = tmpvar_20;
  mediump float softness_32;
  softness_32 = tmpvar_21;
  faceColor_29.xyz = (faceColor_29.xyz * faceColor_29.w);
  outlineColor_30.xyz = (outlineColor_30.xyz * outlineColor_30.w);
  mediump vec4 tmpvar_33;
  tmpvar_33 = mix (faceColor_29, outlineColor_30, vec4((clamp (
    (d_28 + (outline_31 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_31)
  ))));
  faceColor_29 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34 = (faceColor_29 * (1.0 - clamp (
    (((d_28 - (outline_31 * 0.5)) + (softness_32 * 0.5)) / (1.0 + softness_32))
  , 0.0, 1.0)));
  faceColor_29 = tmpvar_34;
  faceColor_9 = faceColor_29;
  faceColor_9.xyz = (faceColor_9.xyz / faceColor_9.w);
  highp vec4 h_35;
  h_35 = smp4x_11;
  highp float tmpvar_36;
  tmpvar_36 = (_ShaderFlags / 2.0);
  highp float tmpvar_37;
  tmpvar_37 = (fract(abs(tmpvar_36)) * 2.0);
  highp float tmpvar_38;
  if ((tmpvar_36 >= 0.0)) {
    tmpvar_38 = tmpvar_37;
  } else {
    tmpvar_38 = -(tmpvar_37);
  };
  highp float tmpvar_39;
  tmpvar_39 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_40;
  tmpvar_40 = clamp (((
    ((smp4x_11 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_39) + 0.5), 0.0, 1.0);
  h_35 = tmpvar_40;
  if (bool(float((tmpvar_38 >= 1.0)))) {
    h_35 = (1.0 - abs((
      (tmpvar_40 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_41;
  tmpvar_41 = (min (mix (h_35, 
    sin(((h_35 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_39)
   * _GradientScale) * -2.0));
  h_35 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = vec2(1.0, 0.0);
  tmpvar_42.z = (tmpvar_41.y - tmpvar_41.x);
  highp vec3 tmpvar_43;
  tmpvar_43 = normalize(tmpvar_42);
  highp vec3 tmpvar_44;
  tmpvar_44.xy = vec2(0.0, -1.0);
  tmpvar_44.z = (tmpvar_41.w - tmpvar_41.z);
  highp vec3 tmpvar_45;
  tmpvar_45 = normalize(tmpvar_44);
  lowp vec3 tmpvar_46;
  tmpvar_46 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_7 = tmpvar_46;
  highp vec3 tmpvar_47;
  tmpvar_47 = mix (vec3(0.0, 0.0, 1.0), (bump_7 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0)
  )), faceColor_9.www);
  bump_7 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = faceColor_9.xyz;
  tmpvar_4 = tmpvar_48;
  highp vec3 tmpvar_49;
  tmpvar_49 = -(normalize((
    ((tmpvar_43.yzx * tmpvar_45.zxy) - (tmpvar_43.zxy * tmpvar_45.yzx))
   - tmpvar_47)));
  tmpvar_5 = tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = clamp ((tmpvar_19 + (tmpvar_20 * 0.5)), 0.0, 1.0);
  highp float tmpvar_51;
  tmpvar_51 = faceColor_9.w;
  tmpvar_6 = tmpvar_51;
  tmpvar_3 = tmpvar_5;
  lightDir_2 = xlv_TEXCOORD3;
  lowp float atten_52;
  atten_52 = texture (_LightTexture0, xlv_TEXCOORD5).w;
  lowp vec4 c_53;
  highp float nh_54;
  lowp float tmpvar_55;
  tmpvar_55 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_56;
  tmpvar_56 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_54 = tmpvar_56;
  mediump float y_57;
  y_57 = (mix (_FaceShininess, _OutlineShininess, tmpvar_50) * 128.0);
  highp float tmpvar_58;
  tmpvar_58 = pow (nh_54, y_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_55) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_58)) * (atten_52 * 2.0));
  c_53.xyz = tmpvar_59;
  highp float tmpvar_60;
  tmpvar_60 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_58) * atten_52));
  c_53.w = tmpvar_60;
  c_1.xyz = c_53.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "POINT" "GLOW_ON" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xyz;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture2D (_MainTex, P_14).w;
  tmpvar_18.y = texture2D (_MainTex, P_15).w;
  tmpvar_18.z = texture2D (_MainTex, P_16).w;
  tmpvar_18.w = texture2D (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_63;
  tmpvar_63 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_63;
  highp float tmpvar_64;
  tmpvar_64 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp float atten_65;
  atten_65 = texture2D (_LightTexture0, vec2(tmpvar_64)).w;
  lowp vec4 c_66;
  highp float nh_67;
  lowp float tmpvar_68;
  tmpvar_68 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_69;
  tmpvar_69 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_67 = tmpvar_69;
  mediump float y_70;
  y_70 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_71;
  tmpvar_71 = pow (nh_67, y_70);
  highp vec3 tmpvar_72;
  tmpvar_72 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_68) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_71)) * (atten_65 * 2.0));
  c_66.xyz = tmpvar_72;
  highp float tmpvar_73;
  tmpvar_73 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_71) * atten_65));
  c_66.w = tmpvar_73;
  c_1.xyz = c_66.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "POINT" "GLOW_ON" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xyz;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture (_MainTex, P_14).w;
  tmpvar_18.y = texture (_MainTex, P_15).w;
  tmpvar_18.z = texture (_MainTex, P_16).w;
  tmpvar_18.w = texture (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_63;
  tmpvar_63 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_63;
  highp float tmpvar_64;
  tmpvar_64 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp float atten_65;
  atten_65 = texture (_LightTexture0, vec2(tmpvar_64)).w;
  lowp vec4 c_66;
  highp float nh_67;
  lowp float tmpvar_68;
  tmpvar_68 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_69;
  tmpvar_69 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_67 = tmpvar_69;
  mediump float y_70;
  y_70 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_71;
  tmpvar_71 = pow (nh_67, y_70);
  highp vec3 tmpvar_72;
  tmpvar_72 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_68) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_71)) * (atten_65 * 2.0));
  c_66.xyz = tmpvar_72;
  highp float tmpvar_73;
  tmpvar_73 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_71) * atten_65));
  c_66.w = tmpvar_73;
  c_1.xyz = c_66.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "GLOW_ON" }
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture2D (_MainTex, P_14).w;
  tmpvar_18.y = texture2D (_MainTex, P_15).w;
  tmpvar_18.z = texture2D (_MainTex, P_16).w;
  tmpvar_18.w = texture2D (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  lightDir_2 = xlv_TEXCOORD3;
  lowp vec4 c_63;
  highp float nh_64;
  lowp float tmpvar_65;
  tmpvar_65 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_66;
  tmpvar_66 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_64 = tmpvar_66;
  mediump float y_67;
  y_67 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_68;
  tmpvar_68 = pow (nh_64, y_67);
  highp vec3 tmpvar_69;
  tmpvar_69 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_65) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_68)) * 2.0);
  c_63.xyz = tmpvar_69;
  highp float tmpvar_70;
  tmpvar_70 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_68));
  c_63.w = tmpvar_70;
  c_1.xyz = c_63.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "GLOW_ON" }
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture (_MainTex, P_14).w;
  tmpvar_18.y = texture (_MainTex, P_15).w;
  tmpvar_18.z = texture (_MainTex, P_16).w;
  tmpvar_18.w = texture (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  lightDir_2 = xlv_TEXCOORD3;
  lowp vec4 c_63;
  highp float nh_64;
  lowp float tmpvar_65;
  tmpvar_65 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_66;
  tmpvar_66 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_64 = tmpvar_66;
  mediump float y_67;
  y_67 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_68;
  tmpvar_68 = pow (nh_64, y_67);
  highp vec3 tmpvar_69;
  tmpvar_69 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_65) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_68)) * 2.0);
  c_63.xyz = tmpvar_69;
  highp float tmpvar_70;
  tmpvar_70 = (tmpvar_6 + ((_LightColor0.w * _SpecColor.w) * tmpvar_68));
  c_63.w = tmpvar_70;
  c_1.xyz = c_63.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "SPOT" "GLOW_ON" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
uniform sampler2D _LightTextureB0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture2D (_MainTex, P_14).w;
  tmpvar_18.y = texture2D (_MainTex, P_15).w;
  tmpvar_18.z = texture2D (_MainTex, P_16).w;
  tmpvar_18.w = texture2D (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_63;
  tmpvar_63 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_63;
  highp vec2 P_64;
  P_64 = ((xlv_TEXCOORD5.xy / xlv_TEXCOORD5.w) + 0.5);
  highp float tmpvar_65;
  tmpvar_65 = dot (xlv_TEXCOORD5.xyz, xlv_TEXCOORD5.xyz);
  lowp float atten_66;
  atten_66 = ((float(
    (xlv_TEXCOORD5.z > 0.0)
  ) * texture2D (_LightTexture0, P_64).w) * texture2D (_LightTextureB0, vec2(tmpvar_65)).w);
  lowp vec4 c_67;
  highp float nh_68;
  lowp float tmpvar_69;
  tmpvar_69 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_70;
  tmpvar_70 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_68 = tmpvar_70;
  mediump float y_71;
  y_71 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_72;
  tmpvar_72 = pow (nh_68, y_71);
  highp vec3 tmpvar_73;
  tmpvar_73 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_69) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_72)) * (atten_66 * 2.0));
  c_67.xyz = tmpvar_73;
  highp float tmpvar_74;
  tmpvar_74 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_72) * atten_66));
  c_67.w = tmpvar_74;
  c_1.xyz = c_67.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "SPOT" "GLOW_ON" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
uniform sampler2D _LightTextureB0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture (_MainTex, P_14).w;
  tmpvar_18.y = texture (_MainTex, P_15).w;
  tmpvar_18.z = texture (_MainTex, P_16).w;
  tmpvar_18.w = texture (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_63;
  tmpvar_63 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_63;
  highp vec2 P_64;
  P_64 = ((xlv_TEXCOORD5.xy / xlv_TEXCOORD5.w) + 0.5);
  highp float tmpvar_65;
  tmpvar_65 = dot (xlv_TEXCOORD5.xyz, xlv_TEXCOORD5.xyz);
  lowp float atten_66;
  atten_66 = ((float(
    (xlv_TEXCOORD5.z > 0.0)
  ) * texture (_LightTexture0, P_64).w) * texture (_LightTextureB0, vec2(tmpvar_65)).w);
  lowp vec4 c_67;
  highp float nh_68;
  lowp float tmpvar_69;
  tmpvar_69 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_70;
  tmpvar_70 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_68 = tmpvar_70;
  mediump float y_71;
  y_71 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_72;
  tmpvar_72 = pow (nh_68, y_71);
  highp vec3 tmpvar_73;
  tmpvar_73 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_69) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_72)) * (atten_66 * 2.0));
  c_67.xyz = tmpvar_73;
  highp float tmpvar_74;
  tmpvar_74 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_72) * atten_66));
  c_67.w = tmpvar_74;
  c_1.xyz = c_67.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "GLOW_ON" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xyz;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _LightTextureB0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture2D (_MainTex, P_14).w;
  tmpvar_18.y = texture2D (_MainTex, P_15).w;
  tmpvar_18.z = texture2D (_MainTex, P_16).w;
  tmpvar_18.w = texture2D (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_63;
  tmpvar_63 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_63;
  highp float tmpvar_64;
  tmpvar_64 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp float atten_65;
  atten_65 = (texture2D (_LightTextureB0, vec2(tmpvar_64)).w * textureCube (_LightTexture0, xlv_TEXCOORD5).w);
  lowp vec4 c_66;
  highp float nh_67;
  lowp float tmpvar_68;
  tmpvar_68 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_69;
  tmpvar_69 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_67 = tmpvar_69;
  mediump float y_70;
  y_70 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_71;
  tmpvar_71 = pow (nh_67, y_70);
  highp vec3 tmpvar_72;
  tmpvar_72 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_68) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_71)) * (atten_65 * 2.0));
  c_66.xyz = tmpvar_72;
  highp float tmpvar_73;
  tmpvar_73 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_71) * atten_65));
  c_66.w = tmpvar_73;
  c_1.xyz = c_66.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" "GLOW_ON" }
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
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xyz;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _LightTextureB0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture (_MainTex, P_14).w;
  tmpvar_18.y = texture (_MainTex, P_15).w;
  tmpvar_18.z = texture (_MainTex, P_16).w;
  tmpvar_18.w = texture (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  mediump vec3 tmpvar_63;
  tmpvar_63 = normalize(xlv_TEXCOORD3);
  lightDir_2 = tmpvar_63;
  highp float tmpvar_64;
  tmpvar_64 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp float atten_65;
  atten_65 = (texture (_LightTextureB0, vec2(tmpvar_64)).w * texture (_LightTexture0, xlv_TEXCOORD5).w);
  lowp vec4 c_66;
  highp float nh_67;
  lowp float tmpvar_68;
  tmpvar_68 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_69;
  tmpvar_69 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_67 = tmpvar_69;
  mediump float y_70;
  y_70 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_71;
  tmpvar_71 = pow (nh_67, y_70);
  highp vec3 tmpvar_72;
  tmpvar_72 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_68) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_71)) * (atten_65 * 2.0));
  c_66.xyz = tmpvar_72;
  highp float tmpvar_73;
  tmpvar_73 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_71) * atten_65));
  c_66.w = tmpvar_73;
  c_1.xyz = c_66.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "GLOW_ON" }
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xy;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture2D (_MainTex, P_14).w;
  tmpvar_18.y = texture2D (_MainTex, P_15).w;
  tmpvar_18.z = texture2D (_MainTex, P_16).w;
  tmpvar_18.w = texture2D (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture2D (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  lightDir_2 = xlv_TEXCOORD3;
  lowp float atten_63;
  atten_63 = texture2D (_LightTexture0, xlv_TEXCOORD5).w;
  lowp vec4 c_64;
  highp float nh_65;
  lowp float tmpvar_66;
  tmpvar_66 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_67;
  tmpvar_67 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_65 = tmpvar_67;
  mediump float y_68;
  y_68 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_69;
  tmpvar_69 = pow (nh_65, y_68);
  highp vec3 tmpvar_70;
  tmpvar_70 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_66) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_69)) * (atten_63 * 2.0));
  c_64.xyz = tmpvar_70;
  highp float tmpvar_71;
  tmpvar_71 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_69) * atten_63));
  c_64.w = tmpvar_71;
  c_1.xyz = c_64.xyz;
  c_1.w = tmpvar_6;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" "GLOW_ON" }
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
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 _LightMatrix0;
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
out mediump vec3 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.zw = _glesVertex.zw;
  highp vec2 tmpvar_7;
  tmpvar_6.x = (_glesVertex.x + _VertexOffsetX);
  tmpvar_6.y = (_glesVertex.y + _VertexOffsetY);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_2 * sign(dot (tmpvar_2, 
    (((_World2Object * tmpvar_8).xyz * unity_Scale.w) - tmpvar_6.xyz)
  )));
  highp vec2 tmpvar_10;
  tmpvar_10.x = _ScaleX;
  tmpvar_10.y = _ScaleY;
  highp mat2 tmpvar_11;
  tmpvar_11[0] = glstate_matrix_projection[0].xy;
  tmpvar_11[1] = glstate_matrix_projection[1].xy;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((glstate_matrix_mvp * tmpvar_6).ww / (tmpvar_10 * (tmpvar_11 * _ScreenParams.xy)));
  highp float tmpvar_13;
  tmpvar_13 = (inversesqrt(dot (tmpvar_12, tmpvar_12)) * ((
    abs(_glesMultiTexCoord1.y)
   * _GradientScale) * 1.5));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = _WorldSpaceCameraPos;
  tmpvar_7.y = mix ((tmpvar_13 * (1.0 - _PerspectiveFilter)), tmpvar_13, abs(dot (tmpvar_9, 
    normalize((((_World2Object * tmpvar_14).xyz * unity_Scale.w) - tmpvar_6.xyz))
  )));
  tmpvar_7.x = ((mix (_WeightNormal, _WeightBold, 
    float((0.0 >= _glesMultiTexCoord1.y))
  ) / _GradientScale) + ((_FaceDilate * _ScaleRatioA) * 0.5));
  highp vec2 tmpvar_15;
  tmpvar_15.x = ((floor(_glesMultiTexCoord1.x) * 5.0) / 4096.0);
  tmpvar_15.y = (fract(_glesMultiTexCoord1.x) * 5.0);
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _EnvMatrix[0].xyz;
  tmpvar_16[1] = _EnvMatrix[1].xyz;
  tmpvar_16[2] = _EnvMatrix[2].xyz;
  tmpvar_3.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.zw = ((tmpvar_15 * _FaceTex_ST.xy) + _FaceTex_ST.zw);
  highp vec3 tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_17 = tmpvar_1.xyz;
  tmpvar_18 = (((tmpvar_9.yzx * tmpvar_1.zxy) - (tmpvar_9.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_19;
  tmpvar_19[0].x = tmpvar_17.x;
  tmpvar_19[0].y = tmpvar_18.x;
  tmpvar_19[0].z = tmpvar_9.x;
  tmpvar_19[1].x = tmpvar_17.y;
  tmpvar_19[1].y = tmpvar_18.y;
  tmpvar_19[1].z = tmpvar_9.y;
  tmpvar_19[2].x = tmpvar_17.z;
  tmpvar_19[2].y = tmpvar_18.z;
  tmpvar_19[2].z = tmpvar_9.z;
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_4 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_19 * ((
    (_World2Object * tmpvar_21)
  .xyz * unity_Scale.w) - tmpvar_6.xyz));
  tmpvar_5 = tmpvar_22;
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = (tmpvar_16 * (_WorldSpaceCameraPos - (_Object2World * tmpvar_6).xyz));
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * tmpvar_6)).xy;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
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
uniform highp float _Bevel;
uniform highp float _BevelOffset;
uniform highp float _BevelWidth;
uniform highp float _BevelClamp;
uniform highp float _BevelRoundness;
uniform sampler2D _BumpMap;
uniform highp float _BumpOutline;
uniform highp float _BumpFace;
uniform lowp vec4 _GlowColor;
uniform highp float _GlowOffset;
uniform highp float _GlowOuter;
uniform highp float _GlowInner;
uniform highp float _GlowPower;
uniform highp float _ShaderFlags;
uniform highp float _ScaleRatioA;
uniform highp float _ScaleRatioB;
uniform sampler2D _MainTex;
uniform highp float _TextureWidth;
uniform highp float _TextureHeight;
uniform highp float _GradientScale;
uniform mediump float _FaceShininess;
uniform mediump float _OutlineShininess;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR0;
in highp vec2 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec2 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp float tmpvar_6;
  tmpvar_4 = vec3(0.0, 0.0, 0.0);
  tmpvar_5 = tmpvar_3;
  tmpvar_6 = 0.0;
  highp vec4 glowColor_7;
  highp vec3 bump_8;
  highp vec4 outlineColor_9;
  highp vec4 faceColor_10;
  highp float c_11;
  highp vec4 smp4x_12;
  highp vec3 tmpvar_13;
  tmpvar_13.z = 0.0;
  tmpvar_13.x = (1.0/(_TextureWidth));
  tmpvar_13.y = (1.0/(_TextureHeight));
  highp vec2 P_14;
  P_14 = (xlv_TEXCOORD0.xy - tmpvar_13.xz);
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0.xy + tmpvar_13.xz);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0.xy - tmpvar_13.zy);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0.xy + tmpvar_13.zy);
  lowp vec4 tmpvar_18;
  tmpvar_18.x = texture (_MainTex, P_14).w;
  tmpvar_18.y = texture (_MainTex, P_15).w;
  tmpvar_18.z = texture (_MainTex, P_16).w;
  tmpvar_18.w = texture (_MainTex, P_17).w;
  smp4x_12 = tmpvar_18;
  lowp float tmpvar_19;
  tmpvar_19 = texture (_MainTex, xlv_TEXCOORD0.xy).w;
  c_11 = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = (((
    (0.5 - c_11)
   - xlv_TEXCOORD1.x) * xlv_TEXCOORD1.y) + 0.5);
  highp float tmpvar_21;
  tmpvar_21 = ((_OutlineWidth * _ScaleRatioA) * xlv_TEXCOORD1.y);
  highp float tmpvar_22;
  tmpvar_22 = ((_OutlineSoftness * _ScaleRatioA) * xlv_TEXCOORD1.y);
  faceColor_10 = _FaceColor;
  outlineColor_9 = _OutlineColor;
  outlineColor_9.w = (outlineColor_9.w * xlv_COLOR0.w);
  highp vec2 tmpvar_23;
  tmpvar_23.x = (xlv_TEXCOORD0.z + (_FaceUVSpeedX * _Time.y));
  tmpvar_23.y = (xlv_TEXCOORD0.w + (_FaceUVSpeedY * _Time.y));
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture (_FaceTex, tmpvar_23);
  highp vec4 tmpvar_25;
  tmpvar_25 = ((faceColor_10 * xlv_COLOR0) * tmpvar_24);
  highp vec2 tmpvar_26;
  tmpvar_26.x = (xlv_TEXCOORD0.z + (_OutlineUVSpeedX * _Time.y));
  tmpvar_26.y = (xlv_TEXCOORD0.w + (_OutlineUVSpeedY * _Time.y));
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture (_OutlineTex, tmpvar_26);
  highp vec4 tmpvar_28;
  tmpvar_28 = (outlineColor_9 * tmpvar_27);
  outlineColor_9 = tmpvar_28;
  mediump float d_29;
  d_29 = tmpvar_20;
  lowp vec4 faceColor_30;
  faceColor_30 = tmpvar_25;
  lowp vec4 outlineColor_31;
  outlineColor_31 = tmpvar_28;
  mediump float outline_32;
  outline_32 = tmpvar_21;
  mediump float softness_33;
  softness_33 = tmpvar_22;
  faceColor_30.xyz = (faceColor_30.xyz * faceColor_30.w);
  outlineColor_31.xyz = (outlineColor_31.xyz * outlineColor_31.w);
  mediump vec4 tmpvar_34;
  tmpvar_34 = mix (faceColor_30, outlineColor_31, vec4((clamp (
    (d_29 + (outline_32 * 0.5))
  , 0.0, 1.0) * sqrt(
    min (1.0, outline_32)
  ))));
  faceColor_30 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35 = (faceColor_30 * (1.0 - clamp (
    (((d_29 - (outline_32 * 0.5)) + (softness_33 * 0.5)) / (1.0 + softness_33))
  , 0.0, 1.0)));
  faceColor_30 = tmpvar_35;
  faceColor_10 = faceColor_30;
  faceColor_10.xyz = (faceColor_10.xyz / faceColor_10.w);
  highp vec4 h_36;
  h_36 = smp4x_12;
  highp float tmpvar_37;
  tmpvar_37 = (_ShaderFlags / 2.0);
  highp float tmpvar_38;
  tmpvar_38 = (fract(abs(tmpvar_37)) * 2.0);
  highp float tmpvar_39;
  if ((tmpvar_37 >= 0.0)) {
    tmpvar_39 = tmpvar_38;
  } else {
    tmpvar_39 = -(tmpvar_38);
  };
  highp float tmpvar_40;
  tmpvar_40 = max (0.01, (_OutlineWidth + _BevelWidth));
  highp vec4 tmpvar_41;
  tmpvar_41 = clamp (((
    ((smp4x_12 + (xlv_TEXCOORD1.x + _BevelOffset)) - 0.5)
   / tmpvar_40) + 0.5), 0.0, 1.0);
  h_36 = tmpvar_41;
  if (bool(float((tmpvar_39 >= 1.0)))) {
    h_36 = (1.0 - abs((
      (tmpvar_41 * 2.0)
     - 1.0)));
  };
  highp vec4 tmpvar_42;
  tmpvar_42 = (min (mix (h_36, 
    sin(((h_36 * 3.14159) / 2.0))
  , vec4(_BevelRoundness)), vec4((1.0 - _BevelClamp))) * ((
    (_Bevel * tmpvar_40)
   * _GradientScale) * -2.0));
  h_36 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = vec2(1.0, 0.0);
  tmpvar_43.z = (tmpvar_42.y - tmpvar_42.x);
  highp vec3 tmpvar_44;
  tmpvar_44 = normalize(tmpvar_43);
  highp vec3 tmpvar_45;
  tmpvar_45.xy = vec2(0.0, -1.0);
  tmpvar_45.z = (tmpvar_42.w - tmpvar_42.z);
  highp vec3 tmpvar_46;
  tmpvar_46 = normalize(tmpvar_45);
  lowp vec3 tmpvar_47;
  tmpvar_47 = ((texture (_BumpMap, xlv_TEXCOORD0.zw).xyz * 2.0) - 1.0);
  bump_8 = tmpvar_47;
  highp vec3 tmpvar_48;
  tmpvar_48 = mix (vec3(0.0, 0.0, 1.0), (bump_8 * mix (_BumpFace, _BumpOutline, 
    clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0)
  )), faceColor_10.www);
  bump_8 = tmpvar_48;
  highp vec4 tmpvar_49;
  highp float tmpvar_50;
  tmpvar_50 = (tmpvar_20 - ((
    (_GlowOffset * _ScaleRatioB)
   * 0.5) * xlv_TEXCOORD1.y));
  highp float tmpvar_51;
  tmpvar_51 = ((mix (_GlowInner, 
    (_GlowOuter * _ScaleRatioB)
  , 
    float((tmpvar_50 >= 0.0))
  ) * 0.5) * xlv_TEXCOORD1.y);
  highp float tmpvar_52;
  tmpvar_52 = clamp (((_GlowColor.w * 
    ((1.0 - pow (clamp (
      abs((tmpvar_50 / (1.0 + tmpvar_51)))
    , 0.0, 1.0), _GlowPower)) * sqrt(min (1.0, tmpvar_51)))
  ) * 2.0), 0.0, 1.0);
  lowp vec4 tmpvar_53;
  tmpvar_53.xyz = _GlowColor.xyz;
  tmpvar_53.w = tmpvar_52;
  tmpvar_49 = tmpvar_53;
  glowColor_7.xyz = tmpvar_49.xyz;
  glowColor_7.w = (tmpvar_49.w * xlv_COLOR0.w);
  highp vec4 overlying_54;
  overlying_54.w = glowColor_7.w;
  highp vec4 underlying_55;
  underlying_55.w = faceColor_10.w;
  overlying_54.xyz = (tmpvar_49.xyz * glowColor_7.w);
  underlying_55.xyz = (faceColor_10.xyz * faceColor_10.w);
  highp vec3 tmpvar_56;
  tmpvar_56 = (overlying_54.xyz + ((1.0 - glowColor_7.w) * underlying_55.xyz));
  highp float tmpvar_57;
  tmpvar_57 = (faceColor_10.w + ((1.0 - faceColor_10.w) * glowColor_7.w));
  highp vec4 tmpvar_58;
  tmpvar_58.xyz = tmpvar_56;
  tmpvar_58.w = tmpvar_57;
  faceColor_10.w = tmpvar_58.w;
  faceColor_10.xyz = (tmpvar_56 / tmpvar_57);
  highp vec3 tmpvar_59;
  tmpvar_59 = faceColor_10.xyz;
  tmpvar_4 = tmpvar_59;
  highp vec3 tmpvar_60;
  tmpvar_60 = -(normalize((
    ((tmpvar_44.yzx * tmpvar_46.zxy) - (tmpvar_44.zxy * tmpvar_46.yzx))
   - tmpvar_48)));
  tmpvar_5 = tmpvar_60;
  highp float tmpvar_61;
  tmpvar_61 = clamp ((tmpvar_20 + (tmpvar_21 * 0.5)), 0.0, 1.0);
  highp float tmpvar_62;
  tmpvar_62 = faceColor_10.w;
  tmpvar_6 = tmpvar_62;
  tmpvar_3 = tmpvar_5;
  lightDir_2 = xlv_TEXCOORD3;
  lowp float atten_63;
  atten_63 = texture (_LightTexture0, xlv_TEXCOORD5).w;
  lowp vec4 c_64;
  highp float nh_65;
  lowp float tmpvar_66;
  tmpvar_66 = max (0.0, dot (tmpvar_5, lightDir_2));
  mediump float tmpvar_67;
  tmpvar_67 = max (0.0, dot (tmpvar_5, normalize(
    (lightDir_2 + normalize(xlv_TEXCOORD4))
  )));
  nh_65 = tmpvar_67;
  mediump float y_68;
  y_68 = (mix (_FaceShininess, _OutlineShininess, tmpvar_61) * 128.0);
  highp float tmpvar_69;
  tmpvar_69 = pow (nh_65, y_68);
  highp vec3 tmpvar_70;
  tmpvar_70 = (((
    (tmpvar_4 * _LightColor0.xyz)
   * tmpvar_66) + (
    (_LightColor0.xyz * _SpecColor.xyz)
   * tmpvar_69)) * (atten_63 * 2.0));
  c_64.xyz = tmpvar_70;
  highp float tmpvar_71;
  tmpvar_71 = (tmpvar_6 + ((
    (_LightColor0.w * _SpecColor.w)
   * tmpvar_69) * atten_63));
  c_64.w = tmpvar_71;
  c_1.xyz = c_64.xyz;
  c_1.w = tmpvar_6;
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "POINT" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SPOT" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SPOT" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "GLOW_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" "GLOW_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "POINT" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SPOT" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SPOT" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" "GLOW_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "GLOW_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" "GLOW_ON" }
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