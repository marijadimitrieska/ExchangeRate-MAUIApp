using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRate_MAUIApp.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>

    {
        private readonly string[] stringFormats = new[] { "dd-MMM-yyyy", "yyyy-MM-dd", "dd.MM.yyyy" };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
          if(reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();

                if(DateTime.TryParseExact(str, stringFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                    return dt;

                if (DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    return dt;
            }
          else if(reader.TokenType == JsonTokenType.Number)
            {
                if(reader.TryGetDouble(out double doubleVal))
                {
                    var s = doubleVal.ToString("F0");
                    if(s.Length == 8 && 
                        int.TryParse(s.Substring(0,4), out int year) && 
                        int.TryParse(s.Substring(4, 2), out int month) && 
                        int.TryParse(s.Substring(6,2), out int day))
                    {
                        return new DateTime(year, month, day);
                    }
                }
            }
                return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("dd.MM.yyyy"));
        }
    }
}
