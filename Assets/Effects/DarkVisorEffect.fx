sampler uImage0 : register(s0); // The contents of the screen.
sampler uImage1 : register(s1); // Up to three extra textures you can use for various purposes (for instance as an overlay).
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition; // The position of the camera.
float2 uTargetPosition; // The "target" of the shader, what this actually means tends to vary per shader.
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect; // Doesn't seem to be used, but included for parity.
float2 uZoom;

float4 DarkVisorEffect(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // Sample the texture
    float4 tex = tex2D(uImage0, texCoord);

    // Check if the color is red
    if (tex.r > 0 && tex.g == 0 && tex.b == 0)
    {
        // Output red for red things
        return float4(tex.r, 0.0, 0.0, 1.0);
    }
    else
    {
        // Output grey for everything else
        float greyValue = (tex.r + tex.g + tex.b) / 3.0;
        return float4(greyValue, greyValue, greyValue, tex.a);
    }
}

technique Technique1
{
    pass DarkVisorEffect
    {
        PixelShader = compile ps_2_0 DarkVisorEffect();
    }
}
