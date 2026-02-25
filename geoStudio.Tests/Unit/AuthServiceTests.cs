using FluentAssertions;
using geoStudio.Application.DTOs;
using geoStudio.Application.Services;
using geoStudio.Infrastructure.Identity;
using geoStudio.Infrastructure.Persistence;
using geoStudio.Infrastructure.Persistence.Repositories;
using geoStudio.Tests.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace geoStudio.Tests.Unit;

public sealed class AuthServiceTests : IDisposable
{
    private readonly GeoStudioDbContext _context;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _context = TestDbContextFactory.Create();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "test-secret-key-that-is-at-least-32-chars",
                ["Jwt:Issuer"] = "geoStudio",
                ["Jwt:Audience"] = "geoStudioUsers",
                ["Jwt:ExpirationMinutes"] = "15",
                ["Jwt:RefreshTokenExpiryDays"] = "7"
            })
            .Build();

        var userRepo = new UserRepository(_context);
        var jwtProvider = new JwtTokenProvider(config);

        _sut = new AuthService(
            userRepo,
            jwtProvider,
            config,
            NullLogger<AuthService>.Instance);
    }

    [Fact]
    public async Task RegisterAsync_WithValidRequest_ReturnsAuthResponse()
    {
        // Arrange
        var request = new RegisterRequest("test@example.com", "Password1!", "Jane", "Doe");

        // Act
        var result = await _sut.RegisterAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.User.Email.Should().Be("test@example.com");
        result.User.FirstName.Should().Be("Jane");
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RegisterRequest("dup@example.com", "Password1!", "Alice", "Smith");
        await _sut.RegisterAsync(request);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.RegisterAsync(request));
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var registerRequest = new RegisterRequest("login@example.com", "Password1!", "Bob", "Jones");
        await _sut.RegisterAsync(registerRequest);

        var loginRequest = new LoginRequest("login@example.com", "Password1!");

        // Act
        var result = await _sut.LoginAsync(loginRequest);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var registerRequest = new RegisterRequest("user@example.com", "Password1!", "User", "Test");
        await _sut.RegisterAsync(registerRequest);

        var loginRequest = new LoginRequest("user@example.com", "WrongPassword1!");

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.LoginAsync(loginRequest));
    }

    public void Dispose() => _context.Dispose();
}
