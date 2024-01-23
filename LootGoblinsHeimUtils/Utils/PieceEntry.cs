using System;
using System.Globalization;
using UnityEngine;

namespace LootGoblinsUtils.Utils;

public class PieceEntry
{
    public string line;
    public string name;
    public string category;
    public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;
    public string additionalInfo;
    public float scaleX;
    public float scaleY;
    public float scaleZ;
    
    public static PieceEntry FromBlueprint(string line)
    {
        if (line.IndexOf(',') > -1)
            line = line.Replace(',', '.');
        string[] strArray = line.Split(';');
        string name = strArray[0];
        string category = strArray[1];
        float x1 = InvariantFloat(strArray[2]);
        float y1 = InvariantFloat(strArray[3]);
        float z1 = InvariantFloat(strArray[4]);
        float x2 = InvariantFloat(strArray[5]);
        float y2 = InvariantFloat(strArray[6]);
        float z2 = InvariantFloat(strArray[7]);
        float w = InvariantFloat(strArray[8]);
        Vector3 pos = new Vector3(x1, y1, z1);
        Quaternion normalized = new Quaternion(x2, y2, z2, w).normalized;
        string json = strArray[9];
        string additionalInfo;
        if (!string.IsNullOrEmpty(json))
        {
            if (!json.Equals("\"\""))
            {
                try
                {
                    additionalInfo = SimpleJson.SimpleJson.DeserializeObject<string>(json);
                    goto label_7;
                }
                catch
                {
                    additionalInfo = strArray[9];
                    goto label_7;
                }
            }
        }
        additionalInfo = (string) null;
        label_7:
        Vector3 scale = Vector3.one;
        if (strArray.Length > 10)
            scale = new Vector3(InvariantFloat(strArray[10]), InvariantFloat(strArray[11]), InvariantFloat(strArray[12]));
        return new PieceEntry(name, category, pos, normalized, additionalInfo, scale);
    }

    public static PieceEntry FromVBuild(string line)
    {
        if (line.IndexOf(',') > -1)
            line = line.Replace(',', '.');
        string[] strArray = line.Split(' ');
        string name = strArray[0];
        float x1 = InvariantFloat(strArray[1]);
        float y1 = InvariantFloat(strArray[2]);
        float z1 = InvariantFloat(strArray[3]);
        float w = InvariantFloat(strArray[4]);
        float x2 = InvariantFloat(strArray[5]);
        float y2 = InvariantFloat(strArray[6]);
        float z2 = InvariantFloat(strArray[7]);
        string category = "Building";
        Quaternion normalized = new Quaternion(x1, y1, z1, w).normalized;
        Vector3 pos = new Vector3(x2, y2, z2);
        string empty = string.Empty;
        return new PieceEntry(name, category, pos, normalized, empty, Vector3.one);
    }

    public PieceEntry(
        string name,
        string category,
        Vector3 pos,
        Quaternion rot,
        string additionalInfo,
        Vector3 scale)
    {
        this.name = name.Split('(')[0];
        this.category = category;
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
        rotX = rot.x;
        rotY = rot.y;
        rotZ = rot.z;
        rotW = rot.w;
        this.additionalInfo = additionalInfo;
        if (!string.IsNullOrEmpty(this.additionalInfo))
            this.additionalInfo = string.Concat(this.additionalInfo.Split(';'));
        scaleX = scale.x;
        scaleY = scale.y;
        scaleZ = scale.z;
        line = string.Join(";", new string[13]
        {
            this.name,
            this.category,
            InvariantString(posX),
            InvariantString(posY),
            InvariantString(posZ),
            InvariantString(rotX),
            InvariantString(rotY),
            InvariantString(rotZ),
            InvariantString(rotW),
            SimpleJson.SimpleJson.SerializeObject((object) additionalInfo),
            InvariantString(scaleX),
            InvariantString(scaleY),
            InvariantString(scaleZ)
        });
    }

    public Vector3 GetPosition() => new Vector3(posX, posY, posZ);

    public Quaternion GetRotation() => new Quaternion(rotX, rotY, rotZ, rotW);

    public Vector3 GetScale() => new Vector3(scaleX, scaleY, scaleZ);

    public static float InvariantFloat(string s) => string.IsNullOrEmpty(s) ? 0.0f : float.Parse(s, NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo);

    public static string InvariantString(float f) => f.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
}