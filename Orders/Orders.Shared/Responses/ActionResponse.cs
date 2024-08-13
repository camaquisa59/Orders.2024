using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Responses
{
    public class ActionResponse<T>
    {
        public bool WasSuceess { get; set; }
        public string? Message { get; set; }
        public T? Resultado { get; set; }
    }
}