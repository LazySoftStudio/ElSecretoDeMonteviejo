#if HAS_NEWTONSOFT
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor;

public class UnityObjectConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
        => typeof(UnityEngine.Object).IsAssignableFrom(objectType);

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var obj = value as UnityEngine.Object;

        if (obj == null) { writer.WriteNull(); return; }

        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long fileId))
        {
            writer.WriteStartObject();
            writer.WritePropertyName("guid");
            writer.WriteValue(guid);
            writer.WritePropertyName("fileId");
            writer.WriteValue(fileId);
            writer.WriteEndObject();
            return;
        }

        var gid = GlobalObjectId.GetGlobalObjectIdSlow(obj);
        if (gid.identifierType == 0) { writer.WriteNull(); return; } // unsaved scene / not resolvable

        writer.WriteStartObject();
        writer.WritePropertyName("gid");
        writer.WriteValue(gid.ToString());
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;

        var token = JObject.Load(reader);

        // Scene object
        string gidStr = token.Value<string>("gid");
        if (gidStr != null)
        {
            if (!GlobalObjectId.TryParse(gidStr, out var gid)) return null;
            var obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(gid);
            return obj != null && objectType.IsInstanceOfType(obj) ? obj : null;
        }

        // Asset
        string guid = token.Value<string>("guid");
        long fileId = token.Value<long>("fileId");

        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (string.IsNullOrEmpty(path)) return null;

        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path))
        {
            if (asset == null || !objectType.IsInstanceOfType(asset)) continue;

            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out _, out long id) && id == fileId)
                return asset;
        }
        return null;
    }
}
#endif