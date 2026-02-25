using System.Security.Claims;
using geoStudio.Application.DTOs;
using geoStudio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace geoStudio.API.Controllers;

/// <summary>Manages business profiles for authenticated users.</summary>
[ApiController]
[Route("api/v1/businesses")]
[Authorize]
[Produces("application/json")]
public sealed class BusinessController(IBusinessService businessService) : ControllerBase
{
    private Guid CurrentUserId => Guid.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedAccessException("User ID claim not found."));

    /// <summary>Create a new business profile.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BusinessResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBusinessRequest request,
        CancellationToken cancellationToken)
    {
        var result = await businessService.CreateAsync(CurrentUserId, request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Get a business profile by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BusinessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await businessService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get all businesses owned by the current user.</summary>
    [HttpGet("my")]
    [ProducesResponseType(typeof(IEnumerable<BusinessResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMy(CancellationToken cancellationToken)
    {
        var results = await businessService.GetByOwnerAsync(CurrentUserId, cancellationToken);
        return Ok(results);
    }

    /// <summary>Update a business profile.</summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(BusinessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateBusinessRequest request,
        CancellationToken cancellationToken)
    {
        var result = await businessService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    /// <summary>Delete a business profile.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await businessService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
