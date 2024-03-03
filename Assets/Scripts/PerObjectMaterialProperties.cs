using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    private static int s_BaseColorId = Shader.PropertyToID("m_BaseColor");
    private static int s_CutoffId = Shader.PropertyToID("m_Cutoff");
    private static MaterialPropertyBlock s_Block;

    [SerializeField] private float m_Cutoff = 0.5f;
    [SerializeField] private Color m_BaseColor = Color.white;

    private void Awake() { OnValidate(); }

    private void OnValidate()
    {
        s_Block ??= new MaterialPropertyBlock();
        s_Block.SetColor(s_BaseColorId, m_BaseColor);
        s_Block.SetFloat(s_CutoffId, m_Cutoff);
        GetComponent<Renderer>().SetPropertyBlock(s_Block);
    }
}