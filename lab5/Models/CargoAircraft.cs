namespace AirlineProject.Models
{
    public class CargoAircraft : Aircraft
    {
        public CargoAircraft(string model, double flightRange, int passengerCapacity, double cargoCapacity, double fuelConsumption)
            : base(model, flightRange, passengerCapacity, cargoCapacity, fuelConsumption)
        {
        }
    }
}