using Microsoft.AspNetCore.Mvc;
using TicketHubApi.Models;
using TicketHubApi.Services;

namespace TicketHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketPurchaseController : ControllerBase
    {
        private readonly QueueService _queueService;
        private readonly ILogger<TicketPurchaseController> _logger;

        public TicketPurchaseController(QueueService queueService, ILogger<TicketPurchaseController> logger)
        {
            _queueService = queueService;
            _logger = logger;
        }

        // POST: api/TicketPurchase
        [HttpPost]
        public async Task<ActionResult> CreateTicketPurchase([FromBody] TicketPurchase ticketPurchase)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                _logger.LogWarning("Invalid ticket purchase: {Errors}", string.Join(", ", errors));
                return BadRequest($"An error was encountered: {string.Join(", ", errors)}");
            }

            try
            {
                // Additional validation if needed
                if (!IsExpirationDateValid(ticketPurchase.Expiration))
                {
                    return BadRequest("An error was encountered: Invalid expiration date format");
                }

                if (!IsSecurityCodeValid(ticketPurchase.SecurityCode))
                {
                    return BadRequest("An error was encountered: Invalid security code");
                }

                // Send the valid purchase to the queue
                await _queueService.SendMessageAsync(ticketPurchase);
                _logger.LogInformation("Ticket purchase for {Email} successfully queued", ticketPurchase.Email);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ticket purchase");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        private bool IsExpirationDateValid(string expiration)
        {
            // Implement validation for MM/YY format
            // This is a simple example - you might want to enhance this
            return !string.IsNullOrEmpty(expiration) &&
                   System.Text.RegularExpressions.Regex.IsMatch(expiration, @"^\d{2}/\d{2}$");
        }

        private bool IsSecurityCodeValid(string securityCode)
        {
            // Implement validation for 3 or 4 digit security code
            // This is a simple example - you might want to enhance this
            return !string.IsNullOrEmpty(securityCode) &&
                   System.Text.RegularExpressions.Regex.IsMatch(securityCode, @"^\d{3,4}$");
        }
    }
}