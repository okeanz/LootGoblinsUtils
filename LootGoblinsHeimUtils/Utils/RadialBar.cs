using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialBar
{
    private readonly List<Image> _segments;

    private readonly int _segmentCount;

    private static Sprite _segmentSprite =
        SpriteFactory.GenerateRingSprite(256, 0.75f, Color.white, new Color32(100, 100, 100, 255));


    public RadialBar(List<Image> segments)
    {
        _segments = segments;
        _segmentCount = segments.Count;
    }

    public static RadialBar Create(Canvas canvas, int segmentCount, Vector2 position)
    {
        var root = new GameObject("RadialBar", typeof(RectTransform));
        root.transform.SetParent(canvas.transform);

        var rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = new Vector2(0.5f, 0.5f);
        rootRect.anchorMax = new Vector2(0.5f, 0.5f);
        rootRect.sizeDelta = new Vector2(100, 100);
        rootRect.anchoredPosition = position;

        var segments = new List<Image>(segmentCount);
        var angleStep = 360.0f / segmentCount;

        MakeText(root.transform, "\u2756", new Color(0, 171, 255, 255));

        for (int i = 0; i < segmentCount; i++)
        {
            var (rect, image) = MakeCircle($"Segment_{i}", root.transform, (1f / segmentCount) * 0.90f);

            rect.localEulerAngles = new Vector3(0f, 0f, -angleStep * i);
            
            segments.Add(image);
        }

        return new RadialBar(segments);
    }

    private static Text MakeText(Transform root, string text, Color color)
    {
        var textGO = new GameObject("Label", typeof(RectTransform), typeof(Text));
        textGO.transform.SetParent(root, false);

        var textTmp = textGO.GetComponent<Text>();
        textTmp.text = text;
        textTmp.color = color;
        textTmp.fontSize = 64;
        textTmp.alignment = TextAnchor.MiddleCenter;
        textTmp.font = Resources.Load<Font>("LiberationSans");

        return textTmp;
    }

    private static (RectTransform, Image) MakeCircle(string name, Transform root, float fillAmount)
    {
        var segGo = new GameObject(name, typeof(RectTransform), typeof(Image));
        segGo.transform.SetParent(root.transform);

        var fillRect = segGo.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0, 0);
        fillRect.anchorMax = new Vector2(1, 1);
        fillRect.anchoredPosition = new Vector2(0, 0);
        fillRect.sizeDelta = new Vector2(0, 0);

        var fillImage = segGo.GetComponent<Image>();
        fillImage.sprite = _segmentSprite;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Radial360;
        fillImage.fillOrigin = (int)Image.Origin360.Top;
        fillImage.fillAmount = fillAmount;

        return (fillRect, fillImage);
    }
}


public static class SpriteFactory
{
    public static Sprite GenerateRingSprite(int size, float innerRadius01, Color32 innerColor, Color32 borderColor)
    {
        if (size <= 0) size = 256;

        // Нормализуем внутренний радиус
        innerRadius01 = Mathf.Clamp(innerRadius01, 0.0f, 0.99f);

        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        float cx = (size - 1) * 0.5f;
        float cy = (size - 1) * 0.5f;

        float outerRadius = Mathf.Min(cx, cy); // внешний радиус по краю текстуры
        float innerRadius = outerRadius * innerRadius01; // внутренний радиус — то, что задаёшь

        float outerRadiusSq = outerRadius * outerRadius;
        float innerRadiusSq = innerRadius * innerRadius;

        Color32 transparent = new Color32(0, 0, 0, 0);
        Color32 white = new Color32(255, 255, 255, 255);
        Color32 black = new Color32(100, 100, 100, 255);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - cx;
                float dy = y - cy;
                float distSq = dx * dx + dy * dy;
                var dist = Mathf.Sqrt(distSq);

                if (dist >= innerRadius && dist <= innerRadius + 2)
                {
                    tex.SetPixel(x, y, black);
                    continue;
                }

                if (dist <= outerRadius && dist >= outerRadius - 2)
                {
                    tex.SetPixel(x, y, black);
                    continue;
                }

                // Пиксель лежит в кольце, если он между внутренним и внешним радиусом
                if (distSq >= innerRadiusSq && distSq <= outerRadiusSq)
                {
                    tex.SetPixel(x, y, white);
                }
                else
                {
                    tex.SetPixel(x, y, transparent);
                }
            }
        }

        tex.Apply();

        // PixelsPerUnit можно поставить как угодно, для UI не критично.
        const float pixelsPerUnit = 100f;

        SmoothAlphaGaussian(tex, 8, 1.5f);

        return Sprite.Create(
            tex,
            new Rect(0, 0, size, size),
            new Vector2(0.5f, 0.5f),
            pixelsPerUnit,
            0,
            SpriteMeshType.FullRect
        );
    }

    private static float[,] GenerateGaussianKernel(int radius, float sigma)
    {
        int size = radius * 2 + 1;
        float[,] kernel = new float[size, size];

        float sum = 0f;
        float invSigma2 = 1f / (2f * sigma * sigma);

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                float v = Mathf.Exp(-(x * x + y * y) * invSigma2);
                kernel[y + radius, x + radius] = v;
                sum += v;
            }
        }

        // нормализация ядра
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
            kernel[y, x] /= sum;

        return kernel;
    }

    public static void SmoothAlphaGaussian(Texture2D tex, int radius = 2, float sigma = 1.5f)
    {
        int w = tex.width;
        int h = tex.height;

        Color[] src = tex.GetPixels();
        Color[] dst = new Color[src.Length];

        var kernel = GenerateGaussianKernel(radius, sigma);
        int size = radius * 2 + 1;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float a = 0f;

                for (int ky = -radius; ky <= radius; ky++)
                {
                    int sy = Mathf.Clamp(y + ky, 0, h - 1);

                    for (int kx = -radius; kx <= radius; kx++)
                    {
                        int sx = Mathf.Clamp(x + kx, 0, w - 1);
                        float wght = kernel[ky + radius, kx + radius];
                        a += src[sy * w + sx].a * wght;
                    }
                }

                Color c = src[y * w + x];
                c.a = a;
                dst[y * w + x] = c;
            }
        }

        tex.SetPixels(dst);
        tex.Apply();
    }
}