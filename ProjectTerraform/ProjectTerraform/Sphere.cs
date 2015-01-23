using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTerraform
{
    public class Sphere
    {
        public VertexPositionNormalTexture[] vertices;
        public VertexBuffer vbuffer;
        public short[] indices;
        public IndexBuffer ibuffer;
        public float radius;
        public int nvertices, nindices;
        public Effect effect;
        public GraphicsDevice graphicsDevice;
        public int vNum;

        public Sphere(float Radius, GraphicsDevice graphics,int num)
        {
            radius = Radius;
            nvertices = num * num * 2;
            nindices = num * num * 3;
            vNum = num;
            vbuffer = new VertexBuffer(graphics, typeof(VertexPositionNormalTexture), nvertices, BufferUsage.WriteOnly);
            ibuffer = new IndexBuffer(graphics, IndexElementSize.SixteenBits, nindices, BufferUsage.WriteOnly);
            createspherevertices();
            createindices();
            vbuffer.SetData<VertexPositionNormalTexture>(vertices);
            ibuffer.SetData<short>(indices);
        }
        void createspherevertices()
        {
            vertices = new VertexPositionNormalTexture[nvertices];
            Vector3 center = new Vector3(0, 0, 0);
            Vector3 rad = new Vector3((float)Math.Abs(radius), 0, 0);
            int i = 0;

            float difx = 360.0f / vNum;
            float dify = 360.0f / vNum;
            for (int x = vNum/2; x < vNum; x++)
            {
                for (int y = 0; y < vNum; y++)
                {
                    //---------------------------------------------------------------------
                    Matrix zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify)); //rotate vertex around z
                    Matrix yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx)); //rotate circle around y
                    Vector3 point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation
                    Vector3 pointnormal = Vector3.Normalize(point);
                    Vector2 tex = new Vector2((float)(Math.Atan2(pointnormal.X, pointnormal.Z) / (Math.PI * 2)),
                                              (float)(Math.Asin(pointnormal.Y) / Math.PI + 0.5));
                    if (tex.X < 0.25 || x == vNum - 1) tex.X += 1;
                    vertices[i] = new VertexPositionNormalTexture(point, pointnormal, tex); i++;
                    //---------------------------------------------------------------------
                    zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify + dify)); //rotate vertex around z
                    yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx)); //rotate circle around y
                    point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation
                    pointnormal = Vector3.Normalize(point);
                    tex = new Vector2((float)(Math.Atan2(pointnormal.X, pointnormal.Z) / (Math.PI * 2)),
                                              (float)(Math.Asin(pointnormal.Y) / Math.PI + 0.5));
                    if (tex.X < 0.25 || x == vNum - 1) tex.X += 1;
                    vertices[i] = new VertexPositionNormalTexture(point, pointnormal, tex); i++;
                    //---------------------------------------------------------------------
                    zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify + dify)); //rotate vertex around z
                    yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx + difx)); //rotate circle around y
                    point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation
                    pointnormal = Vector3.Normalize(point);
                    tex = new Vector2((float)(Math.Atan2(pointnormal.X, pointnormal.Z) / (Math.PI * 2)),
                                              (float)(Math.Asin(pointnormal.Y) / Math.PI + 0.5));
                    if (tex.X < 0.25 || x == vNum - 1) tex.X += 1;
                    vertices[i] = new VertexPositionNormalTexture(point, pointnormal, tex); i++;
                    //---------------------------------------------------------------------
                    zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify)); //rotate vertex around z
                    yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx + difx)); //rotate circle around y
                    point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation
                    pointnormal = Vector3.Normalize(point);
                    tex = new Vector2((float)(Math.Atan2(pointnormal.X, pointnormal.Z) / (Math.PI * 2)),
                                              (float)(Math.Asin(pointnormal.Y) / Math.PI + 0.5));
                    if (tex.X < 0.25 || x == vNum - 1) tex.X += 1;
                    vertices[i] = new VertexPositionNormalTexture(point, pointnormal, tex); i++;
                    //---------------------------------------------------------------------
                }
            }
        }
        void createindices()
        {
            indices = new short[nindices];
            int j=0;
            for (int i = 0; i < vNum * vNum/2; i++)
            {
                indices[j] = (short)(i * 4 + 0); j++;
                indices[j] = (short)(i * 4 + 1); j++;
                indices[j] = (short)(i * 4 + 3); j++;
                indices[j] = (short)(i * 4 + 3); j++;
                indices[j] = (short)(i * 4 + 1); j++;
                indices[j] = (short)(i * 4 + 2); j++;
            }
        }
        public void Draw(Matrix world,Matrix view,Matrix proj)
        {
            effect.Parameters["World"].SetValue(world);
            effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Invert(Matrix.Transpose(world)));
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Proj"].SetValue(proj);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, nvertices, indices, 0, indices.Length / 3);
            }
        }
    }
}
