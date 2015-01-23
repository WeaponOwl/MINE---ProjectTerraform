texture Texture;
sampler2D TextureSampler = sampler_state {
    Texture = (Texture);
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture Normal;
sampler2D NormalSampler = sampler_state {
    Texture = (Normal);
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};


struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

float4 AmbientColor;
float AmbientIntensity = 0.03;
float3 DiffuseLightDirection = float3(0, 0.5, -1);
float Offcet;

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
	
	output.Position = input.Position;
	output.TexCoord = input.TexCoord;

    return output;
}

float3 CreateNormal(float2 tex)
{
	float a = tex.x*6.28;
	return float3(sin(a+Offcet),cos(a+Offcet),(tex.y*2-1));
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 normal = CreateNormal(input.TexCoord);

	float3 bump = (tex2D(NormalSampler, input.TexCoord) - (0.5, 0.5, 0.5));
    float3 bumpNormal = normal + bump;
    bumpNormal = normalize(bumpNormal);

	float diffuseIntensity = dot(normalize(DiffuseLightDirection), bumpNormal)/1.1;
    if(diffuseIntensity < 0)
        diffuseIntensity = 0;

    float4 textureColor = tex2D(TextureSampler, input.TexCoord);
	float lightIntensity = sqrt(dot(bumpNormal, DiffuseLightDirection));

	float4 color = saturate(textureColor * diffuseIntensity+ AmbientColor * AmbientIntensity)*lightIntensity*2;
	return lerp(color,textureColor,0.1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
