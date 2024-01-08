using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private ScriptableRenderContext m_RenderContext = default;
    private Camera m_Camera = null;

    public void Render(ScriptableRenderContext renderContext, Camera camera)
    {
        m_RenderContext = renderContext;
        m_Camera = camera;

        Setup();
        DrawVisibleGeometry();
        Submit();
    }

    private void Setup() { m_RenderContext.SetupCameraProperties(m_Camera); }

    private void DrawVisibleGeometry() { m_RenderContext.DrawSkybox(m_Camera); }

    private void Submit() { m_RenderContext.Submit(); }
}