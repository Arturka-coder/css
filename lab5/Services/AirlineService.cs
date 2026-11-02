using System.Collections.Generic;
using System.Linq;
using AirlineProject.Models;

namespace AirlineProject.Services
{
    public class AirlineService : IAirlinesService
    {
        private readonly List<Aircraft> _aircrafts;

        public AirlineService(List<Aircraft> aircrafts)
        {
            _aircrafts = aircrafts;
        }

        public List<Aircraft> GetAllAircrafts() => _aircrafts;

        public int GetTotalPassengerCapacity() => _aircrafts.Sum(a => a.PassengerCapacity);

        public double GetTotalCargoCapacity() => _aircrafts.Sum(a => a.CargoCapacity);

        public void SortByFlightRange() => _aircrafts.Sort();

        public List<Aircraft> FindByFuelConsumption(double minFuel, double maxFuel)
        {
            return _aircrafts.Where(a => a.FuelConsumption >= minFuel && a.FuelConsumption <= maxFuel).ToList();
        }
    }
}