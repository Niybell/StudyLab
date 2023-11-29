using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyLab.Models.FrontendModels.ResponseModels.Base
{
    public class TextResponse
    {
        public TextResponse(string description, int statusCode)
        {
            StatusCode = statusCode;
            Description = description;
        }

        public int StatusCode { get; set; }
        public string Description { get; set; }
    }
}