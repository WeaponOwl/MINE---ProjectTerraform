texture Texture;
sampler2D TextureSampler = sampler_state {
    Texture = (Texture);
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

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
	
	output.Position = input.Position;
	output.TexCoord = input.TexCoord;
	output.Color = input.Color;

    return output;
}

float ratio = 0.2;
float offcetx = 0;
float offcety = 0;

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float x = input.TexCoord.x;
	float y = input.TexCoord.y;

	float distance = (ratio / sqrt((x - 0.5) * (x - 0.5) + (y - 0.5) * (y - 0.5)) + 100+offcetx)/5;
	float angle = (atan2(y - 0.5, x - 0.5) / 6.2832 + 100+offcety);

	distance -= (int)distance;
	angle -= (int)angle;

    //distance = int(ratio * texHeight / sqrt((x - w / 2.0) * (x - w / 2.0) + (y - h / 2.0) * (y - h / 2.0))) % texHeight;
    //angle = (unsigned int)(0.5 * texWidth * atan2(y - h / 2.0, x - w / 2.0) / 3.1416);
	//texture[(unsigned int)(distanceTable[x][y] + shiftX)  % texWidth][(unsigned int)(angleTable[x][y] + shiftY) % texHeight];

	return tex2D(TextureSampler, float2(distance,angle))*input.Color;
	//return tex2D(TextureSampler, tex)*input.Color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}