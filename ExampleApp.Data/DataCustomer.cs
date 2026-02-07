using Csla;

namespace ExampleApp.Data
{
    public class DataCustomer : BusinessBase<DataCustomer>
    {
        public static readonly PropertyInfo<string> NameProperty =
            RegisterProperty<string>(c => c.Name);

        public string Name
        {
            get => GetProperty(NameProperty);
            set => SetProperty(NameProperty, value);
        }
    }
}
