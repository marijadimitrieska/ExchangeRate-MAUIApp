using ExchangeRate_MAUIApp.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRate_MAUIApp.Models
{
    public class ExchangeRate
    {
        // Number of exchange rate list
        [JsonConverter(typeof(CommaIntConverter))]
        public int RBr {  get; set; }
        //Exchange rate list validity date
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Datum { get; set; }
        [JsonConverter(typeof(CommaIntConverter))]
        public int Valuta { get; set; }
        // Currency units
        [JsonConverter(typeof (CommaIntConverter))]
        public int Nomin {  get; set; } 
        public string Oznaka { get; set; }
        public string Drzava { get; set; }
        public string DrzavaAng { get; set; }
        public string DrzavaAl {  get; set; }
        public string NazivMak { get; set; }
        public string NazivAng { get; set; }
        public string NazivAl { get; set; }
        // Middle exchange rate in Denars
        [JsonConverter(typeof(CommaDecimalConverter))]
        public decimal Sreden { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Datum_f {  get; set; }
    }
}
