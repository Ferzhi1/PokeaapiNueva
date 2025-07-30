using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeApiNueva.Data;
using PokeApiNueva.Models;

namespace PokeApiNueva.Controllers
{
    public class FriendsController : Controller
    {
        private readonly ApiDbContext _context;

        public FriendsController(ApiDbContext context)
        {
            _context = context;
        }

        private async Task<Userpkm?> GetCurrentUserAsync()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return null;
            return await _context.Userpkmns.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IActionResult> Friends()
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Unauthorized();

            var allUsers = await _context.Userpkmns
                .Where(u => u.Id != currentUser.Id)
                .ToListAsync();

            var friendships = await _context.FriendshipsFM
                .Where(f => f.UserAId == currentUser.Id || f.UserBId == currentUser.Id)
                .ToListAsync();

            var friendList = allUsers.Select(user =>
            {
                var friendship = friendships.FirstOrDefault(f =>
                    (f.UserAId == currentUser.Id && f.UserBId == user.Id) ||
                    (f.UserBId == currentUser.Id && f.UserAId == user.Id));

                var status = friendship?.Status ?? FriendshipStatus.None;

                return new FriendViewModel
                {
                    User = user,
                    Statusfriendship = status
                };
            }).ToList();

            return View(friendList);
        }

        [HttpGet]
        public async Task<IActionResult> AddFriend(int Id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Unauthorized();

            var friendUser = await _context.Userpkmns.FirstOrDefaultAsync(u => u.Id == Id);
            if (friendUser == null || currentUser.Id == friendUser.Id) return BadRequest();

            var existingFriendship = await _context.FriendshipsFM.FirstOrDefaultAsync(f =>
                (f.UserAId == currentUser.Id && f.UserBId == friendUser.Id) ||
                (f.UserBId == currentUser.Id && f.UserAId == friendUser.Id));

            if (existingFriendship == null)
            {
                _context.FriendshipsFM.Add(new FriendshipFM
                {
                    UserAId = currentUser.Id,
                    UserBId = friendUser.Id,
                    Status = FriendshipStatus.Pending
                });
            }
            else if (existingFriendship.Status == FriendshipStatus.Rejected)
            {
                existingFriendship.Status = FriendshipStatus.Pending;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Friends");
        }

        [HttpGet]
        public async Task<IActionResult> AcceptFriend(int Id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Unauthorized();

            var friendship = await _context.FriendshipsFM.FirstOrDefaultAsync(f =>
                ((f.UserAId == Id && f.UserBId == currentUser.Id) ||
                 (f.UserBId == Id && f.UserAId == currentUser.Id)) &&
                 f.Status == FriendshipStatus.Pending);

            if (friendship == null || friendship.UserBId != currentUser.Id)
                return BadRequest("You are not authorized to accept this friend request.");

            friendship.Status = FriendshipStatus.Accepted;
            await _context.SaveChangesAsync();

            return RedirectToAction("Friends");
        }

        [HttpGet]
        public async Task<IActionResult> RejectFriend(int Id)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null) return Unauthorized();

            var friendship = await _context.FriendshipsFM.FirstOrDefaultAsync(f =>
                ((f.UserAId == Id && f.UserBId == currentUser.Id) ||
                 (f.UserBId == Id && f.UserAId == currentUser.Id)) &&
                 f.Status == FriendshipStatus.Pending);

            if (friendship == null || friendship.UserBId != currentUser.Id)
                return BadRequest("You are not authorized to reject this friend request.");

            friendship.Status = FriendshipStatus.Rejected;
            await _context.SaveChangesAsync();

            return RedirectToAction("Friends");
        }
    }
}
