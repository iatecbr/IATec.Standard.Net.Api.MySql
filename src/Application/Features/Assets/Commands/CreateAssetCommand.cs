using FluentResults;
using MediatR;

namespace Application.Features.Assets.Commands;

public readonly record struct CreateAssetCommand : IRequest<Result>;