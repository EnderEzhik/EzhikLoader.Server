﻿using Microsoft.AspNetCore.Mvc;
using EzhikLoader.Server.Services;
using EzhikLoader.Server.Models.DTOs.Admin.Request;
using Microsoft.AspNetCore.Authorization;

namespace EzhikLoader.Server.Controllers.Admin
{
    [ApiController]
    [Route("api/subscriptions")]
    [Authorize(Roles = "admin")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionsController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> GiveSubscription(CreateSubscriptionDTO subscriptionDTO)
        {
            try
            {
                var subscription = await _subscriptionService.GiveSubscription(subscriptionDTO.UserId, subscriptionDTO.AppId);
                return Ok(subscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{subscriptionId}")]
        public async Task<IActionResult> RevokeSubscription(int subscriptionId)
        {
            try
            {
                var subscription = await _subscriptionService.RevokeSubscription(subscriptionId);
                return Ok(subscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ExtendSubscription(ExtendSubscriptionDTO subscriptionDTO)
        {
            try
            {
                await _subscriptionService.ExtendSubscription(subscriptionDTO);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var subs = await _subscriptionService.GetAllSubscriptions();
            return Ok(subs);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserSubscriptions(int userId)
        {
            var subs = await _subscriptionService.GetUserSubscriptionsAsync(userId);
            return Ok(subs);
        }
    }
}
