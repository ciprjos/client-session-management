using Application.DTOs;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using System.Text.Json;

namespace Application.Services;
internal sealed class SessionService(ISessionRepository sessionRepository, IValidator<AddSessionDto> addSessioValidator, IUnitOfWork unitOfWork) : ISessionService
{
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IValidator<AddSessionDto> _addSessioValidator = addSessioValidator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> AddAsync(AddSessionDto addSessionDto, CancellationToken cancellationToken)
    {
        var validationResult = await _addSessioValidator.ValidateAsync(addSessionDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = JsonSerializer.Serialize(validationResult.Errors);

            return Result.Failure<bool>(Error.Validation("Session.Validation", errors));
        }

        var session = new Session
        {
            Id = Guid.NewGuid(),
            ClientId = addSessionDto.ClientId,
            SessionDate = addSessionDto.SessionDate,
            ProviderId = addSessionDto.ProviderId,
            SessionTypeId = addSessionDto.SessionTypeId,
            Notes = addSessionDto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _sessionRepository.AddAsync(session, cancellationToken);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result == 0)
        {
            return Result.Failure<bool>(Error.Failure("Session.Add", "Failed to add client session."));
        }

        return Result.Success(true);
    }
}
