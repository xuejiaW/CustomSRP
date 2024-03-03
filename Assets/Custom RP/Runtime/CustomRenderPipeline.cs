using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer m_Renderer = new();
    private bool m_UseDynamicBatching;
    private bool m_UseGPUInstancing;

    protected override void Render(ScriptableRenderContext context, Camera[] cameras) { }

    public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
    {
        m_UseDynamicBatching = useDynamicBatching;
        m_UseGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
    }

    protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
        cameras.ForEach(camera => m_Renderer.Render(context, camera, m_UseDynamicBatching, m_UseGPUInstancing));
    }
}