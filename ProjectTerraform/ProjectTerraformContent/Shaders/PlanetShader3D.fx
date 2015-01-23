float4x4 World;
float4x4 View;
float4x4 Proj;
float4x4 WorldInverseTranspose;
 
float4 AmbientColor;// = float4(0, 0, 0.5, 1);
float AmbientIntensity = 0.03;
 
float3 DiffuseLightDirection = float3(0, 0.5, 1);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;
 
texture PlanetTexture;
sampler2D planetTextureSampler = sampler_state {
    Texture = (PlanetTexture);
    MinFilter = Point;
    MagFilter = Point;
    //AddressU = Wrap;
    //AddressV = Wrap;
	AddressU = Clamp;
	AddressV = Clamp;
};

texture PlanetNormal;
sampler2D planetNormalSampler = sampler_state {
    Texture = (PlanetNormal);
    MinFilter = Point;
    MagFilter = Point;
    //AddressU = Wrap;
    //AddressV = Wrap;
	AddressU = Clamp;
	AddressV = Clamp;
};
 
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};
 
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    //float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
    float3 Normal : TEXCOORD1;
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
 
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Proj);

	output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));
 
    output.TextureCoordinate = input.TextureCoordinate;
	output.TextureCoordinate.y = 1 - output.TextureCoordinate.y;
    return output;
}
 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//return float4(1,0,0,1);
	float2 tex = input.TextureCoordinate;
	tex.x+=10;
	tex.y+=10;
	tex.x-=(int)tex.x;
	tex.y-=(int)tex.y;
	tex.y=1-tex.y;

	float3 bump = (tex2D(planetNormalSampler, tex) - (0.5, 0.5, 0.5));
    float3 bumpNormal = input.Normal + bump;
    bumpNormal = normalize(bumpNormal);

	float diffuseIntensity = dot(normalize(DiffuseLightDirection), bumpNormal)/1.1;
    if(diffuseIntensity < 0)
        diffuseIntensity = 0;

    float4 textureColor = tex2D(planetTextureSampler, tex);
	float lightIntensity = sqrt(dot(bumpNormal, DiffuseLightDirection));
 
	return saturate(textureColor * diffuseIntensity+ AmbientColor * AmbientIntensity)*lightIntensity*2.5;
}
 
technique Textured
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}