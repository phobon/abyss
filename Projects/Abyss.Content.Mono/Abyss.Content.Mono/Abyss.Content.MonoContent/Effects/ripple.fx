// Our texture sampler
texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
};

float wave;
float distortion;
float2 centerCoord;

// This data comes from the sprite batch vertex shader
struct VertexShaderOutput
{
    float4 Position : SV_Position;
	float4 Color : COLOR0;
	float2 TextureCordinate : TEXCOORD0;
};

float4 Ripple(VertexShaderOutput input) : COLOR0
{
    float2 distance = abs(input.TextureCordinate - centerCoord);
	float scalar = length(distance);

	// Invert the scale, so that 1 is the center point.
	scalar = abs(1 - scalar);

	// Calculate how far to distort for this pixel.
	float sinOffset = sin(wave / scalar);
	sinOffset = clamp(sinOffset, 0, 1);

	// Calculate which direction to distort.
	float sinSign = cos(wave / scalar);

	// Reduce the distortion effect.
	sinOffset = sinOffset * distortion / 32;

	// Pick a pixel on the screen for this pixel, based on the calculated offset and direction.
	float4 color = tex2D(TextureSampler, input.TextureCordinate + (sinOffset * sinSign));

	return color;
}

technique RippleTechnique
{
    pass Pass1
    {
		//PixelShader = compile ps_3_0 Ripple();
        PixelShader = compile ps_4_0_level_9_1 Ripple();
		//PixelShader = compile ps_5_0 Ripple();
    }
}