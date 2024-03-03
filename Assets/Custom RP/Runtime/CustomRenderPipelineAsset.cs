using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    [SerializeField] private bool m_UseDynamicBatching = true;
    [SerializeField] private bool m_UseGPUInstancing = true;
    [SerializeField] private bool m_UseSRPBatcher = true;

    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(m_UseDynamicBatching, m_UseGPUInstancing, m_UseSRPBatcher);
    }
}