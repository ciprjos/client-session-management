using Application.DTOs;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using System.Text.Json;

namespace Application.Services;
internal sealed class SessionService(ISessionRepository sessionRepository,
                                    IClientRepository clientRepository,
                                    IProviderRepository providerRepository,
                                    IValidator<AddSessionDto> addSessionValidator,
                                    IUnitOfWork unitOfWork) : ISessionService
{
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IProviderRepository _providerRepository = providerRepository;
    private readonly IValidator<AddSessionDto> _addSessioValidator = addSessionValidator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> AddAsync(AddSessionDto addSessionDto, CancellationToken cancellationToken)
    {
        var validationResult = await _addSessioValidator.ValidateAsync(addSessionDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = JsonSerializer.Serialize(validationResult.Errors);

            return Result.Failure<bool>(Error.Validation("Session.Validation", errors));
        }

        var client = await _clientRepository.GetByIdAsync(addSessionDto.ClientId, cancellationToken);

        if (client == null)
        {
            return Result.Failure<bool>(Error.NotFound("Session.ClientNotFound", $"Client with ID {addSessionDto.ClientId} not found."));
        }

        var provider = await _providerRepository.GetByIdAsync(addSessionDto.ProviderId, cancellationToken);

        if (provider is null)
        {
            return Result.Failure<bool>(Error.NotFound("Session.ProviderNotFound", $"Provider with ID {addSessionDto.ProviderId} not found."));
        }

        var providerSessionType = provider.ProviderSessionTypes.FirstOrDefault(pst => pst.SessionTypeId == addSessionDto.SessionTypeId);

        if (providerSessionType is null)
        {
            return Result.Failure<bool>(Error.Validation("Session.ProviderSessionTypeInvalid", $"Provider with ID {addSessionDto.ProviderId} does not offer session type with ID {addSessionDto.SessionTypeId}."));
        }

        var sessionDate = addSessionDto.SessionDate.ToUniversalTime();

        if (sessionDate < DateTime.UtcNow.Date)
        {
            return Result.Failure<bool>(Error.Validation("Session.SessionDateInvalid", "Session date cannot be in the past."));
        }

        var clientSession = await _sessionRepository.ClientSessionAsync(addSessionDto.ClientId, cancellationToken);

        if (clientSession != null)
        {
            if (addSessionDto.SessionDate.Date <= clientSession.SessionDate.Date)
            {
                return Result.Failure<bool>(Error.Validation("Session.SessionDateInvalid", "New session date must be after the existing session date."));
            }
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

    public async Task<Result<IEnumerable<GetSessionsDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var sessions = await _sessionRepository.GetAllAsync(cancellationToken);

        var mappedSessions = MapToGetSessionsDto(sessions);

        return Result.Success(mappedSessions);
    }

    private static IEnumerable<GetSessionsDto> MapToGetSessionsDto(IEnumerable<Session> sessions)
    {
        var sessionDtos = new List<GetSessionsDto>();

        foreach (var data in sessions)
        {
           var session = new GetSessionsDto
            {
                SessionId = data.Id,
                ProviderName = data.Provider.Name,
                ClientName = data.Client.Name,
                SessionType = data.SessionType.Name,
                SessionDate = data.SessionDate,
                Notes = data.Notes,
            };
            sessionDtos.Add(session);
        }

        return sessionDtos;
    }
}
