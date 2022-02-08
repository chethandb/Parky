using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWebMVC
{
    public static class StaticDetails
    {
        public static string APIBaseURL = "https://localhost:44374/";
        public static string NationalParkAPIPath = APIBaseURL + "api/v1/nationalparks";
        public static string TrailAPIPath = APIBaseURL + "api/v1/trails";
    }
}
