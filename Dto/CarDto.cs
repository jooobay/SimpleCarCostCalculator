namespace SimpleCarCostCalculatorServer.Dto
{
    /// <summary>
    ///  To ease request/response with front-end
    /// </summary>
    public class CarDto
    {
        public int CategoryKey { get; set; }
        public double BasePrice { get; set; }
    }
}
