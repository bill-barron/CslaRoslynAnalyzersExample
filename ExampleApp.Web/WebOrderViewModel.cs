using ExampleApp.Data;

namespace ExampleApp.Web
{
    /// <summary>
    /// This class demonstrates the correct approach - using simple DTOs/ViewModels
    /// instead of exposing CSLA business objects in the UI layer.
    /// This class will NOT trigger any analyzer warnings.
    /// </summary>
    public class WebOrderViewModel
    {
        // Good: Using primitive types and DTOs
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }

        // Good: Method accepts DTOs instead of CSLA objects
        public void UpdateFromDto(OrderDto dto)
        {
            OrderNumber = dto.OrderNumber;
            CustomerName = dto.CustomerName;
            TotalAmount = dto.TotalAmount;
        }
    }

    // Simple DTO for transferring data between layers
    public class OrderDto
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
