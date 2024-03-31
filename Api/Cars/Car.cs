using SimpleCarCostCalculatorServer.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SimpleCarCostCalculator.Api.Cars
{
    /// <summary>
    ///  Main domain model object
    /// </summary>
    public class Car
    {
        /// <summary>
        ///  Constants
        /// </summary>
        static readonly double STORAGE_FEE = 100;
        static readonly double USING_RATE_FEE = 0.1;

        /// <summary>
        ///  Car Info
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; private set; }
        public double BasePrice { get; set; }
        public double AssociationFee { get; set; }
        public double StorageFee { get { return STORAGE_FEE; } }
        public double CalculatedPrice { get; set; }

        /// <summary>
        ///  Category Info
        /// </summary>
        public double SellerFeeRate { get; set; }
        public double[] SellerFeeRange { get; set; } = [];
        public string CategoryName { get; set; }
        public double CategoryBaseUsingFee { get; set; }
        public double CategorySpecialFee { get; set; }

        /// <summary>
        ///  Used for EF purposes
        /// </summary>
        public Car()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        ///  Principal constructor
        /// </summary>
        public Car(CarDto carDto, CategoryDto categoryDto)
        {
            Id = Guid.NewGuid();
            BasePrice = carDto.BasePrice;
            CategoryName = categoryDto.Name;
            SellerFeeRate = categoryDto.SellerFeeRate;
            SellerFeeRange = categoryDto.SellerFeeRange;

            Calculate();
        }

        /// <summary>
        ///  To set calculation object fields
        /// </summary>
        public void Calculate()
        {
            CategoryBaseUsingFee = Math.Round(BasePrice * USING_RATE_FEE);

            if (CategoryBaseUsingFee.CompareTo(SellerFeeRange?.Min()) < 0)
                CategoryBaseUsingFee = SellerFeeRange.Min();
            if (CategoryBaseUsingFee.CompareTo(SellerFeeRange?.Max()) > 0)
                CategoryBaseUsingFee = SellerFeeRange.Max();

            CategorySpecialFee = Math.Round(BasePrice * SellerFeeRate, 2);

            switch (BasePrice)
            {
                case double BasePrice when BasePrice <= 500 && BasePrice >= 1:
                    AssociationFee = 5;
                    break;
                case double BasePrice when BasePrice <= 1000 && BasePrice > 500:
                    AssociationFee = 10;
                    break;
                case double BasePrice when BasePrice <= 3000 && BasePrice > 1000:
                    AssociationFee = 15;
                    break;
                case double BasePrice when BasePrice > 3000:
                    AssociationFee = 20;
                    break;
            }

            CalculatedPrice = BasePrice + CategoryBaseUsingFee + CategorySpecialFee + AssociationFee + STORAGE_FEE;
        }
    }
}
