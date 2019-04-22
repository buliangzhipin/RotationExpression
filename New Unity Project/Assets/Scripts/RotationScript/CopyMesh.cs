using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMesh : MonoBehaviour
{
    private MeshFilter mf;
    private Mesh originalMesh;
    private Mesh mesh;
    public MeshFilter mfOthers;
    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        mf = this.GetComponent<MeshFilter>();
        originalMesh = mfOthers.mesh;
        mesh = Instantiate(originalMesh);
        mf.mesh = mesh;
    }

    public void ShowRotationMesh()
    {
        var euler = go.transform.rotation.eulerAngles;
        euler.x = Mathf.Deg2Rad *euler.x;
        euler.y = Mathf.Deg2Rad *euler.y;
        euler.z = Mathf.Deg2Rad *euler.z;
        var matrix = new RotationMatrix(euler);
        mesh = Instantiate(originalMesh);
        var vertices = mesh.vertices;
        for(int i = 0;i<vertices.Length;i++)
        {
            vertices[i] = matrix.Rotate(vertices[i]);
        }
        mesh.vertices = vertices;
        mf.mesh = mesh;
    }

    public struct RotationMatrix
    {
        float cx;
        float cy;
        float cz;        
        float sx;
        float sy;
        float sz;
        Matrix4x4 rx;
        Matrix4x4 ry;
        Matrix4x4 rz;

        public RotationMatrix(Vector3 euler)
        {
            cx = Mathf.Cos(euler.x);
            cy = Mathf.Cos(euler.y);
            cz = Mathf.Cos(euler.z);
            sx = Mathf.Sin(euler.x);
            sy = Mathf.Sin(euler.y);
            sz = Mathf.Sin(euler.z);
            rx = Matrix4x4.identity;
            ry = Matrix4x4.identity;
            rz = Matrix4x4.identity;
            RX();
            RY();
            RZ();
            // var row1 = new Vector4(cz*cy-sz*sx*sy,-sz*cx,cz*sy+sz*cy*sx,0f);
            // var row2 = new Vector4(sz*cy+cz*sx*sy,cz*cx,sz*sy-cz*cy*sx,0f);
            // var row3 = new Vector4(-cx*sy,sx,cx*cy,0f);
            // rotationMatrix.SetRow(0,row1);
            // rotationMatrix.SetRow(1,row2);
            // rotationMatrix.SetRow(2,row3);
        }
        private void RX()
        {
            rx.m11=cx;
            rx.m12=-sx;
            rx.m21=sx;
            rx.m22=cx;
        }
        private void RY()
        {
            ry.m00=cy;
            ry.m02=sy;
            ry.m20=-sy;
            ry.m22=cy;
        }
        private void RZ()
        {
            rz.m00=cz;
            rz.m01=-sz;
            rz.m10=sz;
            rz.m11=cz;
        }
        public Vector3 Rotate(Vector3 original)
        {
            return ry.MultiplyPoint3x4(rx.MultiplyPoint3x4(rz.MultiplyPoint3x4(original)));
        }
    }
}
