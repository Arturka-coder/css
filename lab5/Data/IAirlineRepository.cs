using System.Collections.Generic;
using AirlineProject.Models;

namespace AirlineProject.Data
{
    public interface IAirlineRepository
    {
        List<Aircraft> LoadAircrafts();
    }
}