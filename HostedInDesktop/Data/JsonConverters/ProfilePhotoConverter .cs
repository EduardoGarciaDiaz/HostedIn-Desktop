using HostedInDesktop.Data.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HostedInDesktop.Data.JsonConverters
{
    public class ProfilePhotoConverter : JsonConverter<ProfilePhoto>
    {
        public override ProfilePhoto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var profilePhoto = new ProfilePhoto();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return profilePhoto;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "type":
                        profilePhoto.type = reader.GetString();
                        break;
                    case "data":
                        if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            profilePhoto.data = Array.Empty<byte>();
                            reader.Skip();
                        }
                        else if (reader.TokenType == JsonTokenType.String)
                        {
                            string base64String = reader.GetString();
                            profilePhoto.data = Convert.FromBase64String(base64String);
                        }
                        else
                        {
                            throw new JsonException($"Unexpected token type for 'data': {reader.TokenType}");
                        }
                        break;
                    default:
                        throw new JsonException($"Unknown property: {propertyName}");
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ProfilePhoto value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("type", value.type);
            writer.WriteString("data", Convert.ToBase64String(value.data));
            writer.WriteEndObject();
        }
    }
}
