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
        public async Task<IActionResult> Create(CreateMessageDto messageDto)
        {


            var result = await _messageService.CreateMessageAsync(messageDto);
            return RedirectToAction("Details", "Discussion", new { discussionId = messageDto.DiscussionId });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string content, int discussionId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _messageService.UpdateMessageAsync(id, userId, new UpdateMessageDto { Content = content });

            if (!result)
            {
                TempData["Error"] = "Failed to update message.";
            }

            return RedirectToAction("Details", "Discussion", new { discussionId = discussionId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int discussionId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _messageService.DeleteMessageAsync(id, userId);

            if (!result)
            {
                TempData["Error"] = "Failed to delete message.";
            }

            return RedirectToAction("Details", "Discussion", new { discussionId = discussionId });
        }
    }
}