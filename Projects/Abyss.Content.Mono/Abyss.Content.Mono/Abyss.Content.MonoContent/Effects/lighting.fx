texture2D ColorMap;
sampler ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
};

texture2D ShadowMap;
sampler ShadowMapSampler = sampler_state
{
	Texture = <ShadowMap>;
};

struct VertexShaderOutput
{
    float4 Position : SV_Position;
	float4 Color : COLOR0;
	float2 TextureCoordinate : TEXCOORD0;
};

// Ambient light variables.
float AmbientIntensity;		// A number between 0.0 and 1.0. This calculates the level of ambient light in the scene.
float4 AmbientColor;

float4 BlackMaskShader(VertexShaderOutput input) : COLOR0  
{  
    float4 color = tex2D(ColorMapSampler, input.TextureCoordinate);		
	//if (color.a > 0)
	//{
	//	if (color.g < 1 && color.b < 1)
	//	{
	//		color.rgb = 0;
	//	}
	//}

	if (color.a > 0)
		color.rgb = 0;
	
	return color;
}

float4 OutlineShader(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(ColorMapSampler, input.TextureCoordinate);
	if (color.g == 1 && color.b == 1)
	{
		// We have a hit on the outline.
		color.rgb = 0;
	}

	return color;
}

float4 NormalShader(VertexShaderOutput input) : COLOR0  
{  
    float4 color = tex2D(ColorMapSampler, input.TextureCoordinate);		
	//if (color.a > 0)
	//{
	//	if (color.g < 1 && color.b < 1)
	//	{
	//		color.rgb = 1;
	//	}		
	//}

	//if (color.a > 0)
	//	color.rgb = 1;
	
	return color;
}

float4 DeferredLightingShader(VertexShaderOutput input) : COLOR0
{
	// Get all of the values we need from each of the samplers.
	float4 color = tex2D(ColorMapSampler, input.TextureCoordinate);
	float4 shading = tex2D(ShadowMapSampler, input.TextureCoordinate);
	
	// Get a scalar to test whether there is anything to actually show at this point.
	if (color.a > 0.0f)
	{
		float4 finalResult = color * AmbientColor * AmbientIntensity;
		finalResult += (shading * color);	
		return finalResult;
	}

	// If this point is reached, return a transparent pixel. The ColorMap should provide this returned value anyway, but just to be sure.
	return float4(0, 0, 0, 0);
}

technique OutlineTechnique
{
    pass Pass1
    {
		//PixelShader = compile ps_3_0 OutlineShader();
		PixelShader = compile ps_4_0_level_9_1 OutlineShader();		
		//PixelShader = compile ps_5_0 OutlineShader();
    }
}

technique BlackMaskTechnique
{
    pass Pass1
    {
		//PixelShader = compile ps_3_0 BlackMaskShader();
		PixelShader = compile ps_4_0_level_9_1 BlackMaskShader();    
		//PixelShader = compile ps_5_0 OutlineShader();
    }
}

technique NormalTechnique
{
    pass Pass1
    {
		//PixelShader = compile ps_3_0 NormalShader();
        PixelShader = compile ps_4_0_level_9_1 NormalShader();
		//PixelShader = compile ps_5_0 OutlineShader();
    }
}

technique DeferredLightingTechnique
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 DeferredLightingShader();
		PixelShader = compile ps_4_0_level_9_1 DeferredLightingShader();
		//PixelShader = compile ps_5_0 OutlineShader();
	}
}