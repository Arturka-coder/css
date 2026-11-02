using System.Collections.Generic;
using AirlineProject.Models;

namespace AirlineProject.Services
{
    public interface IAirlinesService
    {
        List<Aircraft> GetAllAircrafts();
        int GetTotalPassengerCapacity();
        double GetTotalCargoCapacity();
        void SortByFlightRange();
        List<Aircraft> FindByFuelConsumption(double minFuel, double maxFuel);
    }
}