sampler TextureSampler : register(s0);

float time;
float4 mainColor = float4(0.5, 0.0, 0.8, 1.0);

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(TextureSampler, texCoord);
    
    // نبض متحرك
    float pulse = 0.5 + 0.5 * sin(time * 2);
    color.rgb += mainColor.rgb * pulse * 0.6;

    // شفافية تدريجية
    color.a *= saturate(1.2 - length(texCoord - 0.5) * 2);

    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
