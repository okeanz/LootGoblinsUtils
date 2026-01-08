using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Logger = Jotunn.Logger;

namespace LootGoblinsUtils.Submods.TgDeath;

public static class TgMultipartSender
{
    public static IEnumerator Send(byte[] jpgBytes, string deathInfo)
    {
        var url = "http://localhost:3000/tgSend";

        var form = new WWWForm();

        // 1) image=@file.jpg
        form.AddBinaryData(
            fieldName: "image",
            contents: jpgBytes,
            fileName: "death.jpg",
            mimeType: "image/jpeg"
        );

        // 2) data={json}
        // ВАЖНО: это именно поле формы, не файл
        form.AddField("data", deathInfo);

        using var req = UnityWebRequest.Post(url, form);

        // Content-Type НЕ трогаем — Unity сама поставит multipart/form-data; boundary=...
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Logger.LogError(
                $"Upload failed: {req.responseCode}\n{req.error}\n{req.downloadHandler?.text}"
            );
        }
        else
        {
            Logger.LogInfo("Upload OK");
        }
    }
}