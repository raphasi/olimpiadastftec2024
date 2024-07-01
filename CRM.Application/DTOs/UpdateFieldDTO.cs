using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.Application.DTOs
{
    public class UpdateFieldDTO
    {
        public string FieldName { get; set; }
        public JsonElement FieldValue { get; set; }
    }
}
