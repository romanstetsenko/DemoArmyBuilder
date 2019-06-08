using System;
using System.Collections.Generic;
using System.Text;
using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;
using Xenko.Rendering;

namespace DemoArmyBuilder
{
    public static class Class1
    {
        public static MeshDraw ToMeshDraw(this MeshDraw primitive)
        {
            var vertexBufferBinding = new VertexBufferBinding(primitive.VertexBuffers[0].Buffer, new VertexPositionNormalTexture().GetLayout(), primitive.VertexBuffers[0].Count);
            var indexBufferBinding = new IndexBufferBinding(primitive.IndexBuffer.Buffer, primitive.IndexBuffer.Is32Bit, primitive.IndexBuffer.Buffer.ElementCount);
            var data = new MeshDraw
            {
                StartLocation = 0,
                PrimitiveType = PrimitiveType.TriangleList,
                VertexBuffers = new[] { vertexBufferBinding },
                IndexBuffer = indexBufferBinding,
                DrawCount = primitive.IndexBuffer.Buffer.ElementCount,
            };

            return data;
        }
    }
}
