using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimpleTileGenerator : MonoBehaviour
{
    public int TilesX;
    public int TilesY;

    public string SortingLayer;
    public int OrderInLayer = 0;
    public Color SpriteColor = Color.white;

    public Sprite[] Sprites;
    [Header("Corner Sprites")]
    public Sprite[] TopLeftCornerSprites;
    public Sprite[] TopRightCornerSprites;
    public Sprite[] BottomLeftCornerSprites;
    public Sprite[] BottomRightCornerSprites;
    [Header("Wall Sprites")]
    public Sprite[] LeftWallSprites;
    public Sprite[] RightWallSprites;
    public Sprite[] TopWallSprites;
    public Sprite[] DownWallSprites;


    public Transform[] Generated = null;
    private GameObject _lastParent;

    public void Generate(bool generateNew)
    {
        if (Sprites != null && Sprites.Length > 0)
        {
            if (generateNew || Generated == null || Generated.Length < 1)
            {
                int size = TilesX * TilesY;
                Generated = new Transform[size];
            }

            GameObject parent = new GameObject("TileGenerator");
            parent.transform.SetParent(this.transform);
            int count = 0;
            for (int i = 0; i < TilesX; i++)
            {
                for (int j = 0; j < TilesY; j++)
                {
                    SpriteRenderer newTile = null;
                    if (!generateNew && Generated != null && Generated.Length > count)
                    {
                        if (Generated[count] != null)
                        {
                            newTile = Generated[count].GetComponent<SpriteRenderer>();
                            newTile.sprite = GetRandomSprite(i, j);
                            newTile.transform.SetParent(parent.transform);
                        }
                    }
                    else
                    {
                        newTile = CreateSprite(i, j);
                        newTile.transform.position += Vector3.right * newTile.bounds.size.x * i + Vector3.up * newTile.bounds.size.y * j;
                        newTile.transform.SetParent(parent.transform);
                        newTile.gameObject.name = "Sprite " + i + "-" + j;
                        Generated[count] = newTile.transform;
                    }

                    count++;
                }
            }

            if (_lastParent)
                DestroyImmediate(_lastParent);
            _lastParent = parent;
        }
    }

    SpriteRenderer CreateSprite(int x, int y)
    {
        Sprite sprite = GetRandomSprite(x, y);
        GameObject obj = new GameObject();
        SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingLayerName = SortingLayer;
        spriteRenderer.sortingOrder = OrderInLayer;
        spriteRenderer.color = SpriteColor;
        return spriteRenderer;
    }
    Sprite GetRandomSprite(int x, int y)
    {
        Sprite[] spriteList;
        if (x == 0 && y == 0 && BottomLeftCornerSprites != null && BottomLeftCornerSprites.Length > 0)
        {
            spriteList = BottomLeftCornerSprites;
        }
        else if (x == 0 && y == TilesY - 1 && TopLeftCornerSprites != null && TopLeftCornerSprites.Length > 0)
        {
            spriteList = TopLeftCornerSprites;
        }
        else if (x == 0 && LeftWallSprites != null && LeftWallSprites.Length > 0)
        {
            spriteList = LeftWallSprites;
        }
        else if (x == TilesX - 1 && y == 0 && BottomRightCornerSprites != null && BottomRightCornerSprites.Length > 0)
        {
            spriteList = BottomRightCornerSprites;
        }
        else if (x == TilesX - 1 && y == TilesY - 1 && TopRightCornerSprites != null && TopRightCornerSprites.Length > 0)
        {
            spriteList = TopRightCornerSprites;
        }
        else if (x == TilesX - 1 && RightWallSprites != null && RightWallSprites.Length > 0)
        {
            spriteList = RightWallSprites;
        }
        else if (y == 0 && DownWallSprites != null && DownWallSprites.Length > 0)
        {
            spriteList = DownWallSprites;
        }
        else if (y == TilesY - 1 && TopWallSprites != null && TopWallSprites.Length > 0)
        {
            spriteList = TopWallSprites;
        }
        else
        {
            spriteList = Sprites;
        }

        int index = UnityEngine.Random.Range(0, spriteList.Length);
        Sprite sprite = spriteList[index];
        return sprite;
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(SimpleTileGenerator))]
public class SimpleTileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SimpleTileGenerator generator = target as SimpleTileGenerator;
        if (GUILayout.Button("Generate"))
        {
            generator.Generate(false);
        }
        if (GUILayout.Button("Generate New"))
        {
            generator.Generate(true);
        }


    }
}
#endif

