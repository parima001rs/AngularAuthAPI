namespace UserAuth.UtilityService
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }
    }
}
