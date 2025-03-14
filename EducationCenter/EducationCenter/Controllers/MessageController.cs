using BLL.DTO.Message;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationCenter.Controllers
{

    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageService messageService, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage(CreateMessageDto messageDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Discussion", new { discussionId = messageDto.DiscussionId });
            }

            messageDto.UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _messageService.CreateMessageAsync(messageDto);
            if (result == null)
            {
                TempData["Error"] = "Unsucessful";
            }

            return RedirectToAction("Details", "Discussion", new { discussionId = messageDto.DiscussionId });
        }

        [Authorize(Roles = "Admin,Lecturer")]
        [HttpPost]
        public async Task<IActionResult> Delete(int messageId, int discussionId)
        {
            var userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _messageService.DeleteMessageAsync(messageId, userId);
            if (!result)
            {
                return BadRequest("Unsuccessful");
            }

            return RedirectToAction("Details", "Discussion", new { discussionId });
        }
    }
}
