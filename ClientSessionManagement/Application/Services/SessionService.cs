using Application.DTOs;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Services;
internal sealed class SessionService(ISessionRepository sessionRepository,
                                    IClientRepository clientRepository,
                                    IProviderRepository providerRepository,
                                    IValidator<AddSessionDto> addSessionValidator,
                                    IValidator<UpdateSessionDto> updateSessionValidator,
                                    IUnitOfWork unitOfWork) : ISessionService
{
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IProviderRepository _providerRepository = providerRepository;
    private readonly IValidator<AddSessionDto> _addSessionValidator = addSessionValidator;
    private readonly IValidator<UpdateSessionDto> _updateSessionValidator = updateSessionValidator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> AddAsync(AddSessionDto addSessionDto, CancellationToken cancellationToken)
    {
        var validationResult = await _addSessionValidator.ValidateAsync(addSessionDto, cancellationToken);

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

    public async Task<Result<List<GetSessionsDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var sessions = _sessionRepository.GetAll();

        var sessionsList = await sessions.ToListAsync(cancellationToken);

        var mappedSessions = MapToGetSessionsDto(sessionsList);

        return Result.Success(mappedSessions);
    }

    //Return sessions by client name
    //Return list, if client has same name
    public async Task<Result<List<GetSessionsDto>>> GetSessionsByClientNameAsync(FilterSession filter, CancellationToken cancellationToken)
    {
        var sessions = _sessionRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(filter.ClientName))
        {
            sessions = sessions.Where(s => s.Client.Name == filter.ClientName);
        }

        var sessionsList = await sessions.ToListAsync(cancellationToken);

        var mappedSessions = MapToGetSessionsDto(sessionsList);

        return Result.Success(mappedSessions);
    }

    public async Task<Result<bool>> UpdateAsync(Guid sessionId, UpdateSessionDto updateSessionDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateSessionValidator.ValidateAsync(updateSessionDto, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            var errors = JsonSerializer.Serialize(validationResult.Errors);
            return Result.Failure<bool>(Error.Validation("Session.Validation", errors));
        }

        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);
       
        if (session == null)
        {
            return Result.Failure<bool>(Error.NotFound("Session.NotFound", $"Session with ID {sessionId} not found."));
        }

        session.SessionTypeId = updateSessionDto.SessionTypeId;
        session.ProviderId = updateSessionDto.ProviderId;
        session.SessionDate = updateSessionDto.SessionDate;
        session.Notes = updateSessionDto.Notes;
        session.UpdatedAt = DateTime.UtcNow;    

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result == 0)
        {
            return Result.Failure<bool>(Error.Failure("Session.Update", "Failed to update client session."));
        }

        return Result.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);
      
        if (session == null)
        {
            return Result.Failure<bool>(Error.NotFound("Session.NotFound", $"Session with ID {sessionId} not found."));
        }

        _sessionRepository.Remove(session);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result == 0)
        {
            return Result.Failure<bool>(Error.Failure("Session.Delete", "Failed to delete client session."));
        }

        return Result.Success(true);
    }
    private static List<GetSessionsDto> MapToGetSessionsDto(IEnumerable<Session> sessions)
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
