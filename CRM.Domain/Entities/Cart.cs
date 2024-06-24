using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities
{
    public class Cart : Entity
    {
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
