namespace AirlineProject.Models
{
    public class BusinessJet : Aircraft
    {
        public BusinessJet(string model, double flightRange, int passengerCapacity, double cargoCapacity, double fuelConsumption)
            : base(model, flightRange, passengerCapacity, cargoCapacity, fuelConsumption)
        {
        }
    }
}