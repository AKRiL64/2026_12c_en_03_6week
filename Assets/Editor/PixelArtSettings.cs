using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

public class PixelArtSettings
{
    [MenuItem("Tools/Pixel Art/Apply All Settings")]
    public static void ApplyAllSettings()
    {
        int count = 0;

        // Настройки всех спрайтов во всех папках
        string[] guids = AssetDatabase.FindAssets("t:Texture2D");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter ti =
                AssetImporter.GetAtPath(path) as TextureImporter;

            if (ti == null ||
                ti.textureType != TextureImporterType.Sprite)
                continue;

            ti.filterMode = FilterMode.Point;
            ti.textureCompression =
                TextureImporterCompression.Uncompressed;
            ti.mipmapEnabled = false;
            ti.spritePixelsPerUnit = 32;
            ti.maxTextureSize = 2048;
            ti.spriteImportMode = SpriteImportMode.Single;
            ti.SaveAndReimport();
            count++;
        }


        AssetDatabase.SaveAssets();

        Debug.Log(
            $"Pixel Art Setup complete! Updated {count} sprites.");
    }
}