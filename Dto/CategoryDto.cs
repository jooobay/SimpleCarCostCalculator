namespace SimpleCarCostCalculatorServer.Dto
{
    /// <summary>
    ///  To ease request/response with front-end
    /// </summary>
    public class CategoryDto
    {
        public int Key { get; set; }
        public string Name { get; set; } = "";
        public double SellerFeeRate { get; set; }
        public double[] SellerFeeRange { get; set; } = [];
    }
}
