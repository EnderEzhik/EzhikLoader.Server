using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Admin.Services;
using EzhikLoader.Admin.Models.Subscription;

namespace EzhikLoader.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService subscriptionService;

        public SubscriptionController(SubscriptionService _subscriptionService)
        {
            subscriptionService = _subscriptionService;
        }

        // GET: api/subscription
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionResponseDTO>>> GetSubscriptions()
        {
            var subscriptions = await subscriptionService.GetAllSubscriptionsAsync();
            return Ok(subscriptions);
        }

        // GET: api/subscription/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionResponseDTO>> GetSubscription(int id)
        {
            var subscription = await subscriptionService.GetSubscriptionByIdAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // GET: api/subscription/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<SubscriptionResponseDTO>>> GetUserSubscriptions(int userId)
        {
            var subscriptions = await subscriptionService.GetUserSubscriptionsAsync(userId);
            return Ok(subscriptions);
        }

        // GET: api/subscription/user/5/active
        [HttpGet("user/{userId}/active")]
        public async Task<ActionResult<IEnumerable<SubscriptionResponseDTO>>> GetUserActiveSubscriptions(int userId)
        {
            var subscriptions = await subscriptionService.GetUserActiveSubscriptionsAsync(userId);
            return Ok(subscriptions);
        }

        // GET: api/subscription/app/5
        [HttpGet("app/{appId}")]
        public async Task<ActionResult<IEnumerable<SubscriptionResponseDTO>>> GetAppSubscriptions(int appId)
        {
            var subscriptions = await subscriptionService.GetAppSubscriptionsAsync(appId);
            return Ok(subscriptions);
        }

        // GET: api/subscription/app/5/active
        [HttpGet("app/{appId}/active")]
        public async Task<ActionResult<IEnumerable<SubscriptionResponseDTO>>> GetAppActiveSubscriptions(int appId)
        {
            var subscriptions = await subscriptionService.GetAppActiveSubscriptionsAsync(appId);
            return Ok(subscriptions);
        }

        // POST: api/subscription
        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequestDTO createSubscription)
        {
            try
            {
                var subscription = await subscriptionService.CreateSubscriptionAsync(createSubscription);
                return CreatedAtAction(
                    nameof(GetSubscription),
                    new { id = subscription.Id },
                    subscription);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/subscription/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(int id, UpdateSubscriptionRequestDTO subscriptionUpdate)
        {
            var updatedSubscription = await subscriptionService.UpdateSubscriptionAsync(id, subscriptionUpdate);
            if (updatedSubscription == null)
            {
                return NotFound();
            }

            return Ok(updatedSubscription);
        }

        // DELETE: api/subscription/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            bool deleted = await subscriptionService.DeleteSubscriptionAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/subscription/5/cancel
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelSubscription(int id)
        {
            var subscription = await subscriptionService.CancelSubscriptionAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }

        // POST: api/subscription/5/reactivate
        [HttpPost("{id}/reactivate")]
        public async Task<IActionResult> ReactivateSubscription(int id)
        {
            var subscription = await subscriptionService.ReactivateSubscriptionAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }

        // POST: api/subscription/5/download
        [HttpPost("{id}/download")]
        public async Task<IActionResult> UpdateLastDownloaded(int id)
        {
            var subscription = await subscriptionService.UpdateLastDownloadedAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }
    }
}
