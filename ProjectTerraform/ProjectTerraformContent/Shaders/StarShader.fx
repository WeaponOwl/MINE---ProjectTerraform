float4x4 Projection;

struct VertexData
{
	float4 Pos    : POSITION0;
	float4 Color    : COLOR0;
	float2 TexCoords : TEXCOORD0;
};

static const float PI = 3.14159265f;

VertexData VertexShaderFunction(VertexData input)
{
	VertexData output;

    output.Pos = mul(input.Pos,Projection);
	output.Color = input.Color;
	output.TexCoords = input.TexCoords;

	return output;
}

float4 PixelShaderFunction(VertexData input) : COLOR0
{
	float4 uv;

	uv.rg = input.TexCoords;

	float3 p;

	p.x = uv.r*2-1;
	p.y = uv.g*2-1;
	float l = length(p.xy);

	if(l>1)discard;

	float a=75-(l*75);
	float d = 0;

	if(a<=19){d=119*a/19/255;}
	else if(a>=22){d=1;}
	else 
	{
		a-=18;
		d=(0.0284*pow(a,5) - 1.0127*pow(a,4) + 13.901*pow(a,3) - 91.188*pow(a,2) + 284.64*a - 87.273)/255;
		if(d>1)d=1;
	}

	return input.Color*d;
}

float4 PixelShaderFunction2(VertexData input) : COLOR0
{
	float4 uv;

	uv.rg = input.TexCoords;

	float3 p;

	p.x = uv.r*2-1;
	p.y = uv.g*2-1;
	float l = length(p.xy);

	if(l>1)discard;
	return input.Color;
}

technique Technique1
{
    pass Pass1
    {
		VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique Technique2
{
    pass Pass1
    {
		VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction2();
    }
}
