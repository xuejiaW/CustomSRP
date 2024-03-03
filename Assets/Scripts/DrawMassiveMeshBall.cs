using UnityEngine;
using Random = UnityEngine.Random;

public class DrawMassiveMeshBall : MonoBehaviour
{
    private static int s_BaseColorId = Shader.PropertyToID("m_BaseColor");

    [SerializeField] private Mesh m_Mesh = default;

    [SerializeField] private Material m_Material = default;

    private Matrix4x4[] m_Matrices = new Matrix4x4[1023];
    private Vector4[] m_BaseColors = new Vector4[1023];
    private MaterialPropertyBlock m_Block;

    private void Awake()
    {
        for (int i = 0; i != m_Matrices.Length; ++i)
        {
            m_Matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10f,
                                          Quaternion.Euler(Random.value * 360f, Random.value * 360f,
                                                           Random.value * 360f),
                                          Vector3.one * Random.Range(0.5f, 1.0f));
            m_BaseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.value);
        }
    }

    private void Update()
    {
        if (m_Block == null)
        {
            m_Block = new MaterialPropertyBlock();
            m_Block.SetVectorArray(s_BaseColorId, m_BaseColors);
        }

        Graphics.DrawMeshInstanced(m_Mesh, 0, m_Material, m_Matrices, 1023, m_Block);
    }
}