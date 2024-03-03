using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    private static int s_BaseColorId = Shader.PropertyToID("m_BaseColor");
    private static MaterialPropertyBlock s_Block;

    [SerializeField] private Color m_BaseColor = Color.white;

    private void Awake() { OnValidate(); }

    private void OnValidate()
    {
        s_Block ??= new MaterialPropertyBlock();
        s_Block.SetColor(s_BaseColorId, m_BaseColor);
        GetComponent<Renderer>().SetPropertyBlock(s_Block);
    }
}