﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Stone.Domain.Entities;
using Stone.Domain.Interface.Repositories;
using Stone.Infrastructure.Repositories;
using Stone.Service;

namespace Stone.API.Controllers.EmployeeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePaycheckController : ControllerBase
    {

        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly IDistributedCache _distributedCache;

        public EmployeePaycheckController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> Show(int employeeId)
        {
            var cacheKey = employeeId.ToString();
            string existingKey = String.Empty;
            try
            {
                existingKey = await _distributedCache.GetStringAsync(cacheKey);
            }
            catch
            {
                existingKey = String.Empty;
            }

            Paymentslip slip;

            if (!string.IsNullOrEmpty(existingKey))
            {
                return Ok(existingKey);
            }
            else
            {
                // busca o usuario
                var employee = employeeService.GetEmployeeById(employeeId);

                // cria servico pro usuario
                var service = new PaycheckService(employee);

                // gera contracheque
                slip = service.GetPaySlip();

                // adiciona obj serializado ao redis
                try
                {
                    await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(slip));
                }
                catch
                {
                    Console.WriteLine("nao faz nada");
                }
                
            }
            return Ok(slip);
        }
    }
}