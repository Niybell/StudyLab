using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyLab.Models.FrontendModels.ResponseModels.AuthController
{
    public class GetAccountDateResponse
    {
        public GetAccountDateResponse(GetAccountDateUser? user, int statusCode)
        {
            User = user;
            StatusCode = statusCode;
        }

        public GetAccountDateUser? User { get; set; }
        public int StatusCode { get; set; }
    }
}