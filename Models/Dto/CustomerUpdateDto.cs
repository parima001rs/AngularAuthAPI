using System.ComponentModel.DataAnnotations;

namespace UserAuth.Models.Dto
{
    public class CustomerUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int AllowedResources { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
