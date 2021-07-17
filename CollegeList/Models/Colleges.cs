using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeList.Models
{

    public class Rootobject
    {
        public Metadata metadata { get; set; }
        public Result[] results { get; set; }
    }

    public class Metadata
    {
        public int page { get; set; }
        public int total { get; set; }
        public int per_page { get; set; }
    }

    public class Result
    {
        [JsonProperty("latest.programs.cip_4_digit")]
        public List<LatestProgram> latestprogramscip_4_digit { get; set; }
        [JsonProperty("school.name")]
        public string schoolname { get; set; }
        [JsonProperty("school.city")]
        public string schoolcity { get; set; }
        [JsonProperty("school.state")]
        public string schoolstate { get; set; }
        [JsonProperty("school.school_url")]
        public string schoolschool_url { get; set; }
        public int id { get; set; }
    }

    public class LatestProgram
    {
        public string title { get; set; }
        public Credential credential { get; set; }
    }

    public class Credential
    {
        public string title { get; set; }
    }

}



