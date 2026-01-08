using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Submods.TgDeath;

public class TgDeathMono : MonoBehaviour
{
    public static TgDeathMono Instance;

    private bool _inFlight;
    private const int JPGQuality = 100;

    private void Awake()
    {
        Instance = this;
        Logger.LogInfo("TgDeathMono initiated");
    }

    public void CaptureAndSend(string deathInfo)
    {
        if (_inFlight) return;
        StartCoroutine(CaptureAndSend_Coroutine(deathInfo));
    }

    private IEnumerator CaptureAndSend_Coroutine(string deathInfo)
    {
        _inFlight = true;

        var time = 0f;
        while (time < 0.3f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // Важно: дождаться конца кадра, иначе можно получить "не тот" кадр или пустоту на некоторых пайплайнах.
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = null;

        try
        {
            // 1) Снять скрин в Texture2D
            screenshot = ScreenCapture.CaptureScreenshotAsTexture();

            if (screenshot == null)
            {
                Debug.LogWarning("CaptureScreenshotAsTexture returned null.");
                yield break;
            }

            // 2) Закодировать в JPEG (CPU)
            var jpgBytes = screenshot.EncodeToJPG(JPGQuality);

            if (jpgBytes == null || jpgBytes.Length == 0)
            {
                Logger.LogWarning("EncodeToJPG produced empty result.");
                yield break;
            }

            StartCoroutine(TgMultipartSender.Send(jpgBytes, deathInfo));
        }
        catch (Exception e)
        {
            Logger.LogError("Error sending death screenshot");
            Logger.LogError(e);
        }
        finally
        {
            // Критично: уничтожить Texture2D, иначе утечки памяти.
            if (screenshot != null)
                Destroy(screenshot);

            _inFlight = false;
        }
    }
}