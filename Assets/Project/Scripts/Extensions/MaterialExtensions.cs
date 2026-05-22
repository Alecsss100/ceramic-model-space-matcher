using UnityEngine;
using UnityEngine.Rendering;

public static class MaterialExtensions
{
    const string UrpLitShaderName = "Universal Render Pipeline/Lit";

    public static Material CreateMaterial(Color color)
    {
        var material = new Material(Shader.Find(UrpLitShaderName));
        material.color = color;

        if (color.a < 1f)
            SetupTransparent(material);

        return material;
    }

    static void SetupTransparent(Material material)
    {
        material.SetFloat("_Surface", 1f);
        material.SetFloat("_Blend", 0f);
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.renderQueue = (int)RenderQueue.Transparent;
    }
}
