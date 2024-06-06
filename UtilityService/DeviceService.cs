using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuth.UtilityService;

namespace UserAuth.UtilityService
{
    public class DeviceService: IDeviceService
    {
        private readonly AppDbContext _context;

        public DeviceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddDevice(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
        }

    }
}

