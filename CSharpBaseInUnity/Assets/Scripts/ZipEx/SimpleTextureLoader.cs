using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SimpleTextureLoader
{
    Dictionary<string, Texture2D> _textureDic = new Dictionary<string, Texture2D>();

    public Texture2D LoadTexture(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    public Texture2D GetTexture(string filePath)
    {
        if (_textureDic.TryGetValue(filePath, out var texture))
            return texture;
        else
            return LoadTexture(filePath);
    }

    public Sprite GetSprite(string filePath)
    {
        var tex = GetTexture(filePath);
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}
