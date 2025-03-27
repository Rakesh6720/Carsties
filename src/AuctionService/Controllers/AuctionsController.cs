using System;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private AuctionDbContext _dbContext;
    private IMapper _mapper;

    public AuctionsController(AuctionDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDTO>>> GetAllAuctions()
    {
        var auctions = await _dbContext.Auctions.Include(a => a.Item).OrderBy(x => x.Item.Make).ToListAsync();
        return Ok(_mapper.Map<List<AuctionDTO>>(auctions));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDTO>> GetAuctionById(Guid id)
    {
        var auction = await _dbContext.Auctions.Include(a => a.Item).FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null) return NotFound();
        return Ok(_mapper.Map<AuctionDTO>(auction));
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDTO>> CreateAuction([FromBody] CreateAuctionDTO createAuctionDto)
    {
        var auction = _mapper.Map<Auction>(createAuctionDto);
        _dbContext.Auctions.Add(auction);

        var result = await _dbContext.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Failed to create auction");

        return CreatedAtAction(nameof(GetAuctionById), new { id = auction.Id }, _mapper.Map<AuctionDTO>(auction));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDTO updateAuctionDto)
    {
        var auction = await _dbContext.Auctions.Include(a => a.Item).FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null) return NotFound();

        // TODO: check seller == username

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;

        var result = await _dbContext.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Failed to update auction");

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id) {
        var auction = await _dbContext.Auctions.FindAsync(id);

        if (auction == null) return NotFound();

        // TODO: check seller == username

        _dbContext.Auctions.Remove(auction);
        
        var result = await _dbContext.SaveChangesAsync() > 0;
        
        if (!result) return BadRequest("Failed to delete auction");
        
        return Ok();
    }
}
