using ExampleApp.Data;

namespace ExampleApp.Web
{
    /// <summary>
    /// This class demonstrates the property and method parameter analyzers.
    /// Since ExampleApp.Web references the analyzer project, all three violations will be flagged:
    /// - CSLA002: Property 'Customer' exposes CSLA-based type 'DataCustomer'
    /// - CSLA003: Method 'ProcessOrder' has parameter 'customer' of CSLA-based type 'DataCustomer'
    /// - CSLA003: Method 'UpdateOrder' has parameter 'customer' of CSLA-based type 'DataCustomer'
    /// - CSLA003: Method 'UpdateOrder' has parameter 'previousOrder' of CSLA-based type 'DataOrder'
    /// </summary>
    public class WebOrderPresenter
    {
        // CSLA002 Warning: This property exposes a CSLA-based type
        // In a web/UI layer, you should use DTOs or ViewModels instead
        public DataCustomer Customer { get; set; }

        // CSLA003 Warning: This method parameter is a CSLA-based type
        // Business objects should stay in the data layer
        public void ProcessOrder(DataCustomer customer)
        {
            Customer = customer;
        }

        // CSLA003 Warning: Both parameters are CSLA-based types
        // This will generate two separate warnings
        public void UpdateOrder(DataCustomer customer, DataOrder previousOrder)
        {
            Customer = customer;
        }

        // No warning: private method parameters are ignored by the analyzer
        private void InternalProcess(DataCustomer customer)
        {
            Customer = customer;
        }

        // No warning: this property is private
        private DataOrder CurrentOrder { get; set; }
    }
}
