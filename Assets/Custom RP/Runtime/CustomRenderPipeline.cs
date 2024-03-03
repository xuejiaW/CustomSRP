using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer m_Renderer = new();
    protected override void Render(ScriptableRenderContext context, Camera[] cameras) { }
    
    public CustomRenderPipeline()
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = false;
    }

    protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
        cameras.ForEach(camera => m_Renderer.Render(context, camera));
    }
}