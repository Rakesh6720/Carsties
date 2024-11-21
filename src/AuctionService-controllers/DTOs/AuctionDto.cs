using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService_controllers.DTOs
{
    public class AuctionDto
    {
        public Guid Id { get; set; }
        public int ReservePrice { get; set; } = 0;
        public string Seller { get; set; }
        public string Winner {get; set;}
        public int? SoldAmount { get; set; }
        public int? CurrentHighBid { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
        public DateTime AuctionEnd {get; set;} 
        public string Status {get; set;}
    }
}