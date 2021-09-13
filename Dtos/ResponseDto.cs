using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FletcherProj.Dtos
{
    public class ResponseDto<T>
    {
        public string Message { get; set; }
        public Dictionary<string, string> Errs { get; set; }
        public T Data { get; set; }
    }
}
