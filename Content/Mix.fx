#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler TextureSampler1 : register(s0);

Texture2D Image2;
sampler TextureSampler2
{
    Texture = ( Image2 );
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    MipFilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};

struct VertexToPixel {
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

float4 SpritePixelShader(VertexToPixel PSIn): COLOR0 {
    float4 color1 = tex2D(TextureSampler1, PSIn.TexCoord);
    float4 color2 = tex2D(TextureSampler2, PSIn.TexCoord);

    return color1 * .5 + color2 * .5;
}

technique SpriteBatch {
    pass {
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}