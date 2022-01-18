using System.Drawing;

namespace UserInterface.Models
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public decimal Price { get; set; }
    }
}