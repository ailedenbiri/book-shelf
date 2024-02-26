#ifndef Masking_INCLUDED
#define Masking_INCLUDED

#include "UnityUI.cginc"

/*  API Reference
    -------------

    #define Masking_COORDS(idx)
        Add it to the declaration of the structure that is passed from the vertex shader
        to the fragment shader.
          idx    The number of interpolator to use. Specify the first free TEXCOORD index.

    #define Masking_CALCULATE_COORDS(OUT, pos)
        Use it in the vertex shader to calculate mask-related data.
          OUT    An instance of the output structure that will be passed to the fragment
                 shader. It should be of the type that contains a Masking_COORDS()
                 declaration.
          pos    A source vertex position that have been passed to the vertex shader.

    #define Masking_GET_MASK(IN)
        Use it in the fragment shader to finally compute the mask value.
          IN     An instance of the vertex shader output structure. It should be of type
                 that contains a Masking_COORDS() declaration.

  The following functions are defined only when one of Masking_SIMPLE, Masking_SLICED
  or Masking_TILED macro is defined. It's better to use the macros listed above when
  possible because they properly handle situation when Soft Mask is disabled.

    inline float Masking_GetMask(float2 maskPosition)
        Returns the mask value for a given pixel.
          maskPosition   A position of the current pixel in mask's local space.
                         To get this position use macro Masking_CALCULATE_COORDS().

    inline float4 Masking_GetMaskTexture(float2 maskPosition)
        Returns the color of the mask texture for a given pixel. maskPosition is the same
        as in Masking_GetMask(). This function returns the original pixel of the mask,
        which may be useful for debugging.
*/

#if defined(Masking_SIMPLE) || defined(Masking_SLICED) || defined(Masking_TILED)
#   define __Masking_ENABLE
#   if defined(Masking_SLICED) || defined(Masking_TILED)
#       define __Masking_USE_BORDER
#   endif
#endif

#ifdef __Masking_ENABLE

# define Masking_COORDS(idx)                  float4 maskPosition : TEXCOORD ## idx;
# define Masking_CALCULATE_COORDS(OUT, pos)   (OUT).maskPosition = mul(_Masking_WorldToMask, pos);
# define Masking_GET_MASK(IN)                 Masking_GetMask((IN).maskPosition.xy)

    sampler2D _Masking;
    float4 _Masking_Rect;
    float4 _Masking_UVRect;
    float4x4 _Masking_WorldToMask;
    float4 _Masking_ChannelWeights;
# ifdef __Masking_USE_BORDER
    float4 _Masking_BorderRect;
    float4 _Masking_UVBorderRect;
# endif
# ifdef Masking_TILED
    float2 _Masking_TileRepeat;
# endif
    bool _Masking_InvertMask;
    bool _Masking_InvertOutsides;

    // On changing logic of the following functions, don't forget to update
    // according functions in Masking.MaterialParameters (C#).

    inline float2 __Masking_Inset(float2 a, float2 a1, float2 a2, float2 u1, float2 u2, float2 repeat) {
        float2 w = (a2 - a1);
        float2 d = (a - a1) / w;
        // use repeat only when both w and repeat are not zeroes
        return lerp(u1, u2, (w * repeat != 0.0f ? frac(d * repeat) : d));
    }

    inline float2 __Masking_Inset(float2 a, float2 a1, float2 a2, float2 u1, float2 u2) {
        float2 w = (a2 - a1);
        return lerp(u1, u2, (w != 0.0f ? (a - a1) / w : 0.0f));
    }

# ifdef __Masking_USE_BORDER
    inline float2 __Masking_XY2UV(
            float2 a,
            float2 a1, float2 a2, float2 a3, float2 a4,
            float2 u1, float2 u2, float2 u3, float2 u4) {
        float2 s1 = step(a2, a);
        float2 s2 = step(a3, a);
        float2 s1i = 1 - s1;
        float2 s2i = 1 - s2;
        float2 s12 = s1 * s2;
        float2 s12i = s1 * s2i;
        float2 s1i2i = s1i * s2i;
        float2 aa1 = a1 * s1i2i + a2 * s12i + a3 * s12;
        float2 aa2 = a2 * s1i2i + a3 * s12i + a4 * s12;
        float2 uu1 = u1 * s1i2i + u2 * s12i + u3 * s12;
        float2 uu2 = u2 * s1i2i + u3 * s12i + u4 * s12;
        return
            __Masking_Inset(a, aa1, aa2, uu1, uu2
#   if Masking_TILED
                , s12i * _Masking_TileRepeat
#   endif
            );
    }

    inline float2 Masking_GetMaskUV(float2 maskPosition) {
        return
            __Masking_XY2UV(
                maskPosition,
                _Masking_Rect.xy, _Masking_BorderRect.xy, _Masking_BorderRect.zw, _Masking_Rect.zw,
                _Masking_UVRect.xy, _Masking_UVBorderRect.xy, _Masking_UVBorderRect.zw, _Masking_UVRect.zw);
    }
# else
    inline float2 Masking_GetMaskUV(float2 maskPosition) {
        return
            __Masking_Inset(
                maskPosition,
                _Masking_Rect.xy, _Masking_Rect.zw, _Masking_UVRect.xy, _Masking_UVRect.zw);
    }
# endif
    inline float4 Masking_GetMaskTexture(float2 maskPosition) {
        return tex2D(_Masking, Masking_GetMaskUV(maskPosition));
    }

    inline float Masking_GetMask(float2 maskPosition) {
        float2 uv = Masking_GetMaskUV(maskPosition);
        float4 sampledMask = tex2D(_Masking, uv);
        float weightedMask = dot(sampledMask * _Masking_ChannelWeights, 1);
        float maskInsideRect = _Masking_InvertMask ? 1 - weightedMask : weightedMask;
        float maskOutsideRect = _Masking_InvertOutsides;
        float isInsideRect = UnityGet2DClipping(maskPosition, _Masking_Rect);
        return lerp(maskOutsideRect, maskInsideRect, isInsideRect);
    }
#else // __Masking_ENABLED

# define Masking_COORDS(idx)
# define Masking_CALCULATE_COORDS(OUT, pos)
# define Masking_GET_MASK(IN)                 (1.0f)

    inline float4 Masking_GetMaskTexture(float2 maskPosition) { return 1.0f; }
    inline float Masking_GetMask(float2 maskPosition) { return 1.0f; }
#endif

#endif

// UNITY_SHADER_NO_UPGRADE
