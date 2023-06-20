
// Pixel shader input structure
struct PS_INPUT
{
    float4 Position   : POSITION;
    float2 Texture    : TEXCOORD0;
};

// Pixel shader output structure
struct PS_OUTPUT
{
    float4 Color   : COLOR0;
};

// Global variables
sampler2D Tex0;
float fCoeff;
float fAngle;

PS_OUTPUT ps_main( in PS_INPUT In )
{
  PS_OUTPUT Out;
  PS_OUTPUT OutA;
  PS_OUTPUT OutB;
  PS_OUTPUT OutC;
  PS_OUTPUT OutD;
  PS_OUTPUT OutE;
  PS_OUTPUT OutF;
	float fan = fAngle*0.0174532925f;
	OutA.Color = tex2D(Tex0, float2(In.Texture.x+cos(fan)*fCoeff,In.Texture.y+sin(fan)*fCoeff));
	OutB.Color = tex2D(Tex0, float2(In.Texture.x-cos(fan)*fCoeff,In.Texture.y-sin(fan)*fCoeff));
	OutC.Color = tex2D(Tex0, float2(In.Texture.x+cos(fan)*fCoeff*0.66,In.Texture.y+sin(fan)*fCoeff*0.66));
	OutD.Color = tex2D(Tex0, float2(In.Texture.x-cos(fan)*fCoeff*0.66,In.Texture.y-sin(fan)*fCoeff*0.66));
	OutE.Color = tex2D(Tex0, float2(In.Texture.x+cos(fan)*fCoeff*0.33,In.Texture.y+sin(fan)*fCoeff*0.33));
	OutF.Color = tex2D(Tex0, float2(In.Texture.x-cos(fan)*fCoeff*0.33,In.Texture.y-sin(fan)*fCoeff*0.33));
	Out.Color = tex2D(Tex0, In.Texture.xy);
	Out.Color = (Out.Color+OutA.Color+OutB.Color+OutC.Color+OutD.Color+OutE.Color+OutF.Color)/7;
  return Out;
}

// Effect technique
technique Blur
{
    pass Pass1
    {
        // shaders
        PixelShader  = compile ps_2_0 ps_main();
    }  
}