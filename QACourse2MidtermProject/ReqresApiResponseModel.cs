using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QACourse2MidtermProject
{
    public class ReqresApiResponseModel
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public Data[]? data { get; set; }
        public Support? support { get; set; }
    }

    public class Support
    {
        public string? url { get; set; }
        public string? text { get; set; }
    }

    public class Data
    {
        public int id { get; set; }
        public string? email { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? avatar { get; set; }
    }

    public class ReqresApiSingleResponseModel
    {
        public Data? data { get; set; }
        public Support? support { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int year { get; set; }
        public string? color { get; set; }
        public string? pantone_value { get; set; }
    }

    public class ReqresApiPatchResponseModel
    {
        public string? name { get; set; }
        public string? job { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class ReqresApiCreateResponseModel
    {
        public string? name { get; set; }
        public string? job { get; set; }
        public string? id { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class ReqResApiCreateUserModel
    {
        public string? name { get; set; }
        public string? job { get; set; }
    }

    public class ReqResApiLoginModel
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }

    public class ReqResApiLoginResponseModel
    {
        public string? token { get; set; }
    }



    public class ReqResApiRegisterModel
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }

    public class ReqResApiRegisterResponseModel
    {
        public int? id { get; set; }
        public string? token { get; set; }
    }

}
