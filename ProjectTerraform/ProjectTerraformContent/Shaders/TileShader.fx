texture Texture;
sampler2D TextureSampler = sampler_state {
    Texture = (Texture);
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

float2 TextureSize;
float4x4 Proj;
float4x4 Camera;

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
	//output.Color = output.Position;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//return input.Color;
	//return float4(input.Color.z,input.Color.z,input.Color.z,1);

	float4 ret=tex2D(TextureSampler, input.TexCoord);
	if(ret.a==0)discard;

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
