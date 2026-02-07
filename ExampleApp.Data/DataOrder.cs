using Csla;

namespace ExampleApp.Data
{
    public class DataOrder : BusinessBase<DataOrder>
    {
        public static readonly PropertyInfo<string> OrderNumberProperty =
            RegisterProperty<string>(c => c.OrderNumber);

        public string OrderNumber
        {
            get => GetProperty(OrderNumberProperty);
            set => SetProperty(OrderNumberProperty, value);
        }

        // This property exposes a CSLA type, but won't trigger warnings in ExampleApp.Data
        // because the analyzer is not referenced in this project
        public DataCustomer Customer { get; set; }

        // This method has a CSLA parameter, but won't trigger warnings in ExampleApp.Data
        // because the analyzer is not referenced in this project
        public void ProcessOrder(DataCustomer customer)
        {
            Customer = customer;
        }

        // Another method with multiple CSLA parameters
        public void UpdateOrder(DataCustomer customer, DataOrder previousOrder)
        {
            Customer = customer;
        }
    }
}
