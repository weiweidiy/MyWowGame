
using UnityEditor;
using UnityEditor.U2D.PSD;
using UnityEngine;

/// <summary>
/// 贴图文件导入格式 自动修改
/// </summary>
public class CustomizeImport : AssetPostprocessor
{
    //纹理导入器, pre-process
    void OnPreprocessTexture()
    {
        //获得importer实例
        TextureImporter texImporter = assetImporter as TextureImporter;
        if (texImporter != null)
        {
            _PostprocessTexture(texImporter);
            return;
        }
    }
    
    private void _PostprocessTexture(TextureImporter texImporter)
    {
        //设置Read/Write Enabled开关,不勾选
        texImporter.isReadable = false;
        texImporter.mipmapEnabled = false;
        
        //设置压缩格式,其它平台可根据规划在这里添加
        TextureImporterPlatformSettings psAndroid = texImporter.GetPlatformTextureSettings("Android");
        TextureImporterPlatformSettings psIPhone = texImporter.GetPlatformTextureSettings("iPhone");
        TextureImporterPlatformSettings psWebGL = texImporter.GetPlatformTextureSettings("WebGL");
        psAndroid.overridden = true;
        psIPhone.overridden = true;
        psWebGL.overridden = true;
        
        if (texImporter.DoesSourceTextureHaveAlpha())
        {
            psAndroid.format = TextureImporterFormat.ASTC_6x6;
            psIPhone.format = TextureImporterFormat.ASTC_6x6;
            psWebGL.format = TextureImporterFormat.ASTC_6x6;
        }
        else
        {
            psAndroid.format = TextureImporterFormat.ASTC_4x4;
            psIPhone.format = TextureImporterFormat.ASTC_4x4;
            psWebGL.format = TextureImporterFormat.ASTC_6x6;
        }
        
        texImporter.SetPlatformTextureSettings(psAndroid);
        texImporter.SetPlatformTextureSettings(psIPhone);
        texImporter.SetPlatformTextureSettings(psWebGL);
        
        Debug.Log($"[AutoChanged !] [{assetImporter.assetPath}] [By CustomizeImport]");
    }
}
