using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TranslationManagement.DAL.Enums;

namespace TranslationManagement.DAL.Models
{
    public class Translator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HourlyRate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TranslatorStatus Status { get; set; }
        public string CreditCardNumber { get; set; }
    }
}
