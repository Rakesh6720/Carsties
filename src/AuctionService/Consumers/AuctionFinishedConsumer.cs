using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly AuctionDbContext _dbContext;

        public AuctionFinishedConsumer(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            Console.WriteLine($"Auction finished: {context.Message.AuctionId} with status {(context.Message.ItemSold ? "sold" : "not sold")} to {context.Message.Winner} for amount {context.Message.Amount}");
            
            var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

            if (context.Message.ItemSold)
            {                
                auction.Winner = context.Message.Winner;                
                auction.SoldAmount = context.Message.Amount;
            }
            
            auction.Status = auction.SoldAmount > auction.ReservePrice ? Status.Finished : Status.ReserveNotMet;

            await _dbContext.SaveChangesAsync();
        }
    }
}