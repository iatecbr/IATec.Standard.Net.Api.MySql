using FluentResults;
using MediatR;

namespace Application.Features.Assets.Queries;

public readonly record struct CheckIfExistsAssetQuery : IRequest<Result<bool>>;