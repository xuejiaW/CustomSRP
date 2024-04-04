using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    private ScriptableRenderContext m_RenderContext = default;
    private Camera m_Camera = null;

    private const string k_BufferName = "Render Camera";
    private CommandBuffer m_Buffer = new() {name = k_BufferName};
    private CullingResults m_CullingResults = default;

    private static ShaderTagId s_UnlitShaderTagId = new("SRPDefaultUnlit");
    private static ShaderTagId s_LitShaderTagId = new("CustomLit");

    private Lighting m_Lighting = new Lighting();

    partial void PrepareForSceneWindow();
    partial void DrawUnSupportedShadersGeometry();
    partial void DrawGizmos();
    partial void PrepareBuffer();

    public void Render(ScriptableRenderContext renderContext, Camera camera, bool useDynamicBatching,
                       bool useGPUInstancing)
    {
        m_RenderContext = renderContext;
        m_Camera = camera;

        PrepareBuffer();
        PrepareForSceneWindow();

        if (!Cull()) return;

        Setup();
        m_Lighting.Setup(renderContext);
        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
        DrawUnSupportedShadersGeometry();
        DrawGizmos();
        Submit();
    }

    private void Setup()
    {
        m_RenderContext.SetupCameraProperties(m_Camera);
        CameraClearFlags flags = m_Camera.clearFlags;
        m_Buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth, flags == CameraClearFlags.Color,
                                   flags == CameraClearFlags.Color ? m_Camera.backgroundColor.linear : Color.clear);

        m_Buffer.BeginSample(k_BufferName);
        ExecuteCommandBuffer();
    }

    private void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        var sortingSettings = new SortingSettings(m_Camera) {criteria = SortingCriteria.CommonOpaque};
        var drawingSettings = new DrawingSettings(s_UnlitShaderTagId, sortingSettings)
        {
            enableDynamicBatching = useDynamicBatching,
            enableInstancing = useGPUInstancing
        };
        drawingSettings.SetShaderPassName(1, s_LitShaderTagId);

        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        m_RenderContext.DrawRenderers(m_CullingResults, ref drawingSettings, ref filteringSettings);

        m_RenderContext.DrawSkybox(m_Camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        m_RenderContext.DrawRenderers(m_CullingResults, ref drawingSettings, ref filteringSettings);
    }

    private void Submit()
    {
        m_Buffer.EndSample(k_BufferName);
        ExecuteCommandBuffer();

        m_RenderContext.Submit();
    }

    private void ExecuteCommandBuffer()
    {
        m_RenderContext.ExecuteCommandBuffer(m_Buffer);
        m_Buffer.Clear();
    }

    private bool Cull()
    {
        if (!m_Camera.TryGetCullingParameters(out ScriptableCullingParameters cullingParameters)) return false;

        m_CullingResults = m_RenderContext.Cull(ref cullingParameters);
        return true;
    }
}