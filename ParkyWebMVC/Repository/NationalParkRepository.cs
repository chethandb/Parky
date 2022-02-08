﻿using ParkyWebMVC.Models;
using ParkyWebMVC.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWebMVC.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NationalParkRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
