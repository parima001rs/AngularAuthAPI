using System.ComponentModel.DataAnnotations;

namespace UserAuth.Models.Dto
{
    public class DeviceUpdateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
