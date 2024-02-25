using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD

    private static ShaderTagId[] s_LegacyShaderTagIds =
    {
        new("Always"),
        new("ForwardBase"),
        new("PrepassBase"),
        new("Vertex"),
        new("VertexLMRGBM"),
        new("VertexLM")
    };

    private static Material s_ErrorMaterial = null;

    partial void DrawUnSupportedShadersGeometry()
    {
        if (s_ErrorMaterial == null)
            s_ErrorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));

        var drawingSettings = new DrawingSettings();
        drawingSettings.sortingSettings = new SortingSettings(m_Camera);
        drawingSettings.overrideMaterial = s_ErrorMaterial;
        for (int i = 0; i != s_LegacyShaderTagIds.Length; ++i)
            drawingSettings.SetShaderPassName(i, s_LegacyShaderTagIds[i]);

        var filteringSettings = FilteringSettings.defaultValue;

        m_RenderContext.DrawRenderers(m_CullingResults, ref drawingSettings, ref filteringSettings);
    }

    partial void DrawGizmos()
    {
        if (!Handles.ShouldRenderGizmos()) return;
        m_RenderContext.DrawGizmos(m_Camera,GizmoSubset.PreImageEffects);
        m_RenderContext.DrawGizmos(m_Camera,GizmoSubset.PostImageEffects);
    }
#endif
}