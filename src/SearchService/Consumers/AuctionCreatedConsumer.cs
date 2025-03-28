using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        public Task Consume(ConsumeContext<AuctionCreated> context)
        {
            // Handle the AuctionCreated event here
            return Task.CompletedTask;
        }
    }
}