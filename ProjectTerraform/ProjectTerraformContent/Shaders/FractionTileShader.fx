texture Texture;
sampler2D TextureSampler = sampler_state {
    Texture = (Texture);
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture FractionTexture;
sampler2D FractionTextureSampler = sampler_state {
    Texture = (FractionTexture);
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

float2 TextureSize;
float4x4 Proj;
float4x4 Camera;
float3 FractionTemplate1;
float3 FractionTemplate2;
float Fraction;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Color	: COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Color	: COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
	
	output.Position = mul(input.Position,Camera);
	output.Position = mul(output.Position,Proj);
	output.TexCoord = input.TexCoord;
	output.Color = input.Color;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 ret=tex2D(TextureSampler, input.TexCoord);
	//return ret;
	
	if(ret.x==FractionTemplate1.x&&ret.z==FractionTemplate1.z&&ret.y==FractionTemplate1.y)
		ret = tex2D(FractionTextureSampler, float2(0,Fraction));
	else if(ret.x==FractionTemplate2.x&&ret.z==FractionTemplate2.z&&ret.y==FractionTemplate2.y)
		ret = tex2D(FractionTextureSampler, float2(1,Fraction));
	else if(ret.a==0)discard;

	return ret*input.Color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
