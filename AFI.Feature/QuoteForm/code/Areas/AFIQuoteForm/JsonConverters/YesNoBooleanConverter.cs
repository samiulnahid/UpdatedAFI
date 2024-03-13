using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json.Converters;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "MM/dd/yyyy";
        }

        public CustomDateTimeConverter(string format)
        {
            base.DateTimeFormat = format;
        }
    }

    public class CustomTimestampConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {

            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.Integer) return null;
            //if (!reader.Path.Contains("time")) return null;
            long epoch;
            return long.TryParse(reader.Value.ToString(), out epoch)
                ? DateTimeOffset.FromUnixTimeMilliseconds(epoch).DateTime
                : DateTime.Now;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
	public class YesNoBooleanConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken token = JToken.Load(reader);
			var value = token.Value<string>();
			return value.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}

    public class ArrayString : JsonConverter
    {
        private readonly char _delimiter;

        // JsonConverter attribute can be specified without the 
        // delimiter parameter, in which case colon will be used
        public ArrayString() : this(',')
        {
        }

        public ArrayString(char delimiter)
        {
            _delimiter = delimiter;
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                return null;
            }

            var stringValue = new StringBuilder();

            while (reader.Read() && reader.TokenType == JsonToken.String)
            {
                if (stringValue.Length > 0)
                {
                    stringValue.Append(_delimiter);
                }
                stringValue.Append((string)reader.Value);
            }

            return stringValue.ToString();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var stringValue = value?.ToString();

            if (stringValue == null)
            {
                writer.WriteNull();
                return;
            }

            var arrayValue = stringValue.Split(_delimiter);

            writer.WriteStartArray();

            foreach (var item in arrayValue)
            {
                writer.WriteValue(item);
            }

            writer.WriteEndArray();
        }
    }
}