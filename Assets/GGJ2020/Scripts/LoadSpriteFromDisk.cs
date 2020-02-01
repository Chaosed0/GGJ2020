using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadSpriteFromDisk : MonoBehaviour
{
    private const string EditorTestPath = "TestAssets/TestingDirectory/";

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private string path = null;

    private Sprite generatedSprite = null;

    private void OnValidate()
    {
        this.Autofill(ref spriteRenderer, true);
    }

    private void Awake()
    {
        var path = this.path;
        if (Application.isEditor)
        {
            path = Path.Combine(EditorTestPath, this.path);
        }

        if (File.Exists(path))
        {
            var fileData = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            var squareSize = Mathf.Min(texture.width, texture.height);
            var rect = new Rect(0, 0, texture.width, texture.height);
            var pivot = new Vector2(0.5f, 0.5f);
            generatedSprite = Sprite.Create(texture, rect, pivot, squareSize);

            var scale = new Vector3(squareSize / (float)texture.width, squareSize / (float)texture.height, 1f);
            this.transform.localScale = scale;
        }

        spriteRenderer.sprite = generatedSprite;
    }
}
