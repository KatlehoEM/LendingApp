using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class RatingController : BaseApiController
    {
        private readonly IRatingRepository _ratingRepo;
        private readonly DataContext _context;

        public RatingController(IRatingRepository ratingRepository, DataContext context)
        {
            _ratingRepo = ratingRepository;
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> CreateBorrowerRating(CreateRatingDto createRatingDto)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var lenderId = user.Id;
            try
            {
                var borrowerRating = await _ratingRepo.CreateBorrowerRatingAsync(lenderId, createRatingDto);
                return CreatedAtAction(nameof(GetBorrowerRating), new { id = borrowerRating.Id }, borrowerRating);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowerRating(int id)
        {
            var borrowerRating = await _ratingRepo.GetBorrowerRatingByIdAsync(id);
            if (borrowerRating == null)
            {
                return NotFound();
            }
            return Ok(borrowerRating);
        }

        [HttpGet("borrower/{borrowerId}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetBorrowerRatings(int borrowerId)
        {
            var borrowerRatings = await _ratingRepo.GetBorrowerRatingsByBorrowerIdAsync(borrowerId);
            return Ok(borrowerRatings);
        }

        [HttpGet("borrower/{borrowerId}/average")]
        public async Task<ActionResult<double>> GetAverageBorrowerRating(int borrowerId)
        {
            var averageRating = await _ratingRepo.GetAverageBorrowerRatingAsync(borrowerId);
            return Ok(averageRating);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> UpdateBorrowerRating(int id, UpdateRatingDto updateRatingDto)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var lenderId = user.Id;
            try
            {
                var updatedBorrowerRating = await _ratingRepo.UpdateBorrowerRatingAsync(id, lenderId, updateRatingDto);
                return Ok(updatedBorrowerRating);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
