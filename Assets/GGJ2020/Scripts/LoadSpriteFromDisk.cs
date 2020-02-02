using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadSpriteFromDisk : MonoBehaviour
{
    private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private string path = null;

    private Sprite generatedSprite = null;

    private void OnValidate()
    {
        this.Autofill(ref spriteRenderer, true);
    }

    public static bool IsTransparent(string path)
    {
        var sprite = GetSprite(path);
        if (sprite == null)
        {
            return true;
        }

        for (int x = 0; x < sprite.texture.width; ++x)
        {
            for (int y = 0; y < sprite.texture.height; ++y)
            {
                var color = sprite.texture.GetPixel(x, y);
                if (!Mathf.Approximately(color.a, 0f))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static Sprite GetSprite(string path)
    {
        path = MetaLoadUtil.GetPath(path);
        Sprite sprite = null;
        if (!spriteCache.TryGetValue(path, out sprite))
        {
            if (File.Exists(path))
            {
                var fileData = File.ReadAllBytes(path);
                var texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);

                var squareSize = Mathf.Min(texture.width, texture.height);
                var rect = new Rect(0, 0, texture.width, texture.height);
                var pivot = new Vector2(0.5f, 0.5f);
                sprite = Sprite.Create(texture, rect, pivot, squareSize);

                spriteCache[path] = sprite;
            }
            else
            {
                Debug.LogError($"Sprite at {path} cannot be found!");
            }
        }

        return sprite;
    }

    private void Awake()
    {
        this.generatedSprite = GetSprite(this.path);
        if (generatedSprite != null)
        {
            var texture = generatedSprite.texture;
            var squareSize = Mathf.Min(texture.width, texture.height);
            var scale = new Vector3(squareSize / (float)texture.width, squareSize / (float)texture.height, 1f);
            this.transform.localScale = scale;
        }

        spriteRenderer.sprite = generatedSprite;
    }
}
