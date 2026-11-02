namespace AirlineProject.Models
{
    public class PassengerAircraft : Aircraft
    {
        public PassengerAircraft(string model, double flightRange, int passengerCapacity, double cargoCapacity, double fuelConsumption)
            : base(model, flightRange, passengerCapacity, cargoCapacity, fuelConsumption)
        {
        }
    }
}