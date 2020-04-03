// Our texture sampler
texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
};

float DesaturationAmount;

// This data comes from the sprite batch vertex shader
struct VertexShaderOutput
{
    float4 Position : SV_Position;
	float4 Color : COLOR0;
	float2 TextureCordinate : TEXCOORD0;
};

float4 Sepia(VertexShaderOutput input) : COLOR0
{
    float4 color = tex2D(TextureSampler, input.TextureCordinate);
     
    float3x3 sepia ={0.393, 0.349, 0.272,
                    0.769, 0.686, 0.534 ,
                    0.189, 0.168, 0.131};
 
    float4 result;
    result.rgb = mul(color.rgb, sepia);
    result.a = 1.0f;
 
    return result;
}

float4 Desaturate(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(TextureSampler, input.TextureCordinate);

	// Desaturate
	float desaturatedColor = dot(color,float3(0.3, 0.59, 0.11));
	color.rgb = lerp(color, desaturatedColor, DesaturationAmount);
	
	return color;
}

float4 FlipVertical(VertexShaderOutput input) : COLOR0
{
    float4 color = tex2D(TextureSampler, float2(input.TextureCordinate.x, 1 - input.TextureCordinate.y));
	return color;
}

float4 FlipHorizontal(VertexShaderOutput input) : COLOR0
{
    float4 color = tex2D(TextureSampler, float2(1 - input.TextureCordinate.x, input.TextureCordinate.y));
	return color;
}

float4 Mask(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(TextureSampler, input.TextureCordinate);
	if (color.a)
		color.rgb = 0;
	return color;
}

technique SepiaToneTechnique
{
    pass Pass1
    {
		//PixelShader = compile ps_3_0 Sepia();
        PixelShader = compile ps_4_0_level_9_1 Sepia();
		//PixelShader = compile ps_5_0 Sepia();
    }
}

technique DesaturateTechnique
{
    pass Pass1
    {
		//PixelShader = compile ps_3_0 Desaturate();
        PixelShader = compile ps_4_0_level_9_1 Desaturate();
		//PixelShader = compile ps_5_0 Sepia();
    }
}

technique FlipVerticalTechnique
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 FlipVertical();
	    PixelShader = compile ps_4_0_level_9_1 FlipVertical();
		//PixelShader = compile ps_5_0 Sepia();
	}
}

technique FlipHorizontalTechnique
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 FlipHorizontal();
	    PixelShader = compile ps_4_0_level_9_1 FlipHorizontal();
		//PixelShader = compile ps_5_0 Sepia();
	}
}

technique MaskTechnique
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 Mask();
		PixelShader = compile ps_4_0_level_9_1 Mask();
		//PixelShader = compile ps_5_0 Sepia();
	}
}