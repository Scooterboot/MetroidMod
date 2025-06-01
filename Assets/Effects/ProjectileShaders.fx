sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float uIntensity;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 LaserShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    
	float frameX = (coords.x * uImageSize0.x - uSourceRect.x) / uSourceRect.z;
    //float frameY = (coords.y * uImageSize0.y) / 24;
    //float wave = pow(max(sin(frameY * 8 + uWorldPosition.y / 20), 0), 3);
	float frameX2 = abs(frameX - 0.5);
	float wave = pow(max(sin(frameX2 * -10 + uTime * 12), 0), 2);
    
	float core = pow(max(sin(frameX * 3.14f), 0), 3) * uOpacity;
	color.rgb *= (core * uColor + wave * uSecondaryColor) + sampleColor.rgb;
	color.a *= sampleColor.a;
    
    
	return color;
}

float3 div(float3 n, float3 d) // Divides n by d, returning 1 if d is 0 unless n is 0
{
    float3 result = float3(1, 1, 1);
    if (d.r != 0)
        result.r = n.r / d.r;
    if (d.g != 0)
        result.g = n.g / d.g;
    if (d.b != 0)
        result.b = n.b / d.b;
    
    if (n.r == 0)
        result.r = 0;
    if (n.g == 0)
        result.g = 0;
    if (n.b == 0)
        result.b = 0;
    return result;
}

float4 DodgeAndBurnShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{   //This shader starts with applying a color burn effect then applies color dodge over that
    //This gives the effect of the primary color tinting the whole sprite with the secondary color as a mid tone
    //Pass in uSaturation and uOpacity to edit the effective opacity of the dodge and burn respectively
    //Tweak values as desired, recommed high uOpacity for stronger secondary color on the dark shades
    
    float4 color = tex2D(uImage0, coords);
    float3 primaryColor = uColor;
    float3 secondaryColor = uSecondaryColor;
    
    float mainColorStr = uSaturation;
    float secondColorStr = uOpacity;
    
    float3 colorBurn = div(1 - color.rgb, secondaryColor);
    colorBurn = clamp(colorBurn, 0, 1);
    
    color.rgb = (1 - colorBurn) * secondColorStr + color.rgb * (1 - secondColorStr);
    color *= color.a;
    
    float3 colorDodge = div(color.rgb, 1 - primaryColor);
    colorDodge = clamp(colorDodge, 0, 1);
      
    color.rgb = colorDodge * mainColorStr + color.rgb * (1 - mainColorStr);
    color *= color.a;
    color.a *= sampleColor.a;
    return color;
}

float4 PaletteShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    //This one overrides the darker shades with the secondary color, and applies a gradient to lighter shades for a cleaner palette override
    float4 color = tex2D(uImage0, coords);
    float3 primaryColor = uColor;
    float3 secondaryColor = uSecondaryColor;
    float lum = (color.r + color.g + color.b) / 3;
    if (lum > 0)
    {
        if (lum < 0.5)
        {
            if (lum < 0.25 && color.a > 0.5)
            {
                color.rgb = lum * 4 * secondaryColor * color.a;
            }
            else
            {
                color.rgb = secondaryColor * color.a;
            }
        }
        else if (lum < 0.95)
        {
            float lum2 = (lum * 2) - 0.8;
            color.rgb = primaryColor * lum2 + secondaryColor * (1 - lum2);
            color *= color.a;
        }
    }
    color *= color.a;
    color.a *= sampleColor.a;
    return color;
}

technique Technique1
{
	pass LaserShaderPass
	{
		PixelShader = compile ps_2_0 LaserShaderFunction();
    }
    pass DualTintShaderPass
    {
        PixelShader = compile ps_2_0 DodgeAndBurnShaderFunction();
    }
    pass PaletteShaderPass
    {
        PixelShader = compile ps_2_0 PaletteShaderFunction();
    }
}
