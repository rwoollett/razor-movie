using RazorPages.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace RazorPages.Services
{
  public interface ICarService
  {
    List<Car> ReadAll();
    void Create(Car car);
    Car Read(int id);
    void Update(Car modifiedCar);
    void Delete(int id);
  }

  public class CarService : ICarService
  {
    private readonly IMemoryCache _cache;
    public CarService(IMemoryCache cache)
    {
      _cache = cache;
    }
    public List<Car> ReadAll()
    {
      if (_cache.Get("CarList") == null)
      {
        List<Car> cars = new List<Car>{
                new Car{Id = 1, Make="Audi",Model="R8",Year=2014,Doors=2,Colour="Red",Price=79995},
                new Car{Id = 2, Make="Aston Martin",Model="Rapide",Year=2010,Doors=2,Colour="Black",Price=54995},
                new Car{Id = 3, Make="Porsche",Model=" 911 991",Year=2016,Doors=2,Colour="White",Price=155000},
                new Car{Id = 4, Make="Mercedes-Benz",Model="GLE 63S",Year=2017,Doors=5,Colour="Blue",Price=83995},
                new Car{Id = 5, Make="BMW",Model="X6 M",Year=2016,Doors=5,Colour="Silver",Price=62995},
            };
        _cache.Set("CarList", cars);
      }
      return _cache.Get<List<Car>>("CarList");
    }
    public void Create(Car car)
    {
      var cars = ReadAll();
      car.Id = cars.Max(c => c.Id) + 1;
      cars.Add(car);
      _cache.Set("CarList", cars);
    }
    public Car Read(int id)
    {
      return ReadAll().Single(c => c.Id == id);
    }
    public void Update(Car modifiedCar)
    {
      var cars = ReadAll();
      var car = cars.Single(c => c.Id == modifiedCar.Id);
      car.Make = modifiedCar.Make;
      car.Model = modifiedCar.Model;
      car.Doors = modifiedCar.Doors;
      car.Colour = modifiedCar.Colour;
      car.Year = modifiedCar.Year;
      _cache.Set("CarList", cars);
    }
    public void Delete(int id)
    {
      var cars = ReadAll();
      var deletedCar = cars.Single(c => c.Id == id);
      cars.Remove(deletedCar);
      _cache.Set("CarList", cars);
    }
  }

}