﻿Shader "Hidden/UI Default ETC1 (Masked)"
{
	// ETC1-version (with alpha split texture) of Masking.shader.
	// In Unity 5.3 this shader is the same as Masking.shader and it's never used.

	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _AlphaTex("Sprite Alpha Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		[PerRendererData] _Masking("Mask", 2D) = "white" {}

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
				Name "Default"
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

			#if UNITY_VERSION >= 201720
				#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#endif
				#pragma multi_compile __ UNITY_UI_ALPHACLIP
				#pragma multi_compile __ Masking_SIMPLE Masking_SLICED Masking_TILED

				#if UNITY_VERSION >= 540
				#   define Masking_ETC1
				#endif
				#include "MaskingTemplate.cginc"
			ENDCG
			}
		}
}

// UNITY_SHADER_NO_UPGRADE
