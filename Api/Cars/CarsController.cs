using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleCarCostCalculator.Api.Cars;
using SimpleCarCostCalculatorServer.Dto;

namespace SimpleCarCostCalculator.Api.Cars
{
    [Route("/cars")]
    public class CarsController : Controller
    {
        private CarDb _carDb;
        private readonly IConfiguration _configuration;

        public CarsController(IConfiguration configuration, CarDb carDb)
         {
            _carDb = carDb;
            _configuration = configuration;
        }   

        [HttpGet]
        public IActionResult Get()
        {
            var cars = _carDb.Cars.ToList();
            cars.Reverse();
            return Ok(Json(cars));
        }
 
        [HttpPut]
        public IActionResult Put([FromBody]CarDto carDto)
        {
            
            if (ModelState.IsValid)
            {
                var categorie =
                    _configuration.GetSection("Categories").Get<List<CategoryDto>>()?
                        .Where(c => c.Key == carDto.CategoryKey)
                            .SingleOrDefault();

                if (categorie == null)
                {
                    return NotFound();
                }
                else 
                {
                    Car car = new Car(carDto, categorie);
                    _carDb.Cars.Add(car);
                    _carDb.SaveChanges();

                    return Get();
                }  
            }
            
            return BadRequest(ModelState);
        }
    }
}