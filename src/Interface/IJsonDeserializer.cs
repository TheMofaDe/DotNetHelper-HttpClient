using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetHelper_HttpClient.Interface
{
    public interface IJsonDeserializer
    {
        T Deserialize<T>(string json);
    }
}
