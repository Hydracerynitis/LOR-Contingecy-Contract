using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ParamList
    {
        public List<Params> Ps;
    }
    public class Params
    {
        public string Id;
        public List<ParamDesc> Desc=new List<ParamDesc>();
    }
    public class ParamDesc
    {
        public string Language;
        public string Content;
    }
}
