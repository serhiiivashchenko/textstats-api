# Engineering Learning Log

## PR #1 — OpenAI Integration
[View PR #1](https://github.com/serhiiivashchenko/textstats-api/pull/1)

Learned:
- HttpClient and external API integration
- Dependency Injection
- Async/await
- JSON serialization
- HTTP error handling

Key insight:
External API integrations require proper
error handling and separation of concerns.

## PR #2 — Web UI
[View PR #2](https://github.com/serhiiivashchenko/textstats-api/pull/2)

Learned:
- Connecting frontend and backend
- HTTP requests from the browser
- Handling API responses
- Basic UI integration

Key insight:
A working API is only one part of a
complete user-facing application.

## PR #3 — Fix Instructions
[View PR #3](https://github.com/serhiiivashchenko/textstats-api/pull/3)

Focus:
- Improving project instructions
- Maintaining documentation

Key insight:
Clear instructions improve maintainability
and reduce ambiguity.

## PR #4 — Integration Tests
[View PR #4](https://github.com/serhiiivashchenko/textstats-api/pull/4)

Learned:
- WebApplicationFactory
- ASP.NET Core integration testing
- HttpMessageHandler test doubles
- Testing HTTP endpoints
- Test isolation

Key insight:
Integration tests verify how application
components work together.

## PR #5 — MediatR
[View PR #5](https://github.com/serhiiivashchenko/textstats-api/pull/5)

Learned:
- Mediator pattern
- IRequest and IRequestHandler
- CQRS fundamentals
- Dependency Injection
- Separation of concerns

Key insight:
MediatR decouples request handling
from HTTP endpoints.

## PR #6 — Validation & Exception Handling
[View PR #6](https://github.com/serhiiivashchenko/textstats-api/pull/6)

Learned:
- FluentValidation
- MediatR pipeline behaviors
- Global exception handling
- ProblemDetails
- Cross-cutting concerns

Key insight:
Validation and error handling should
be consistent across the application.

## PR #7 — CancellationToken
[View PR #7](https://github.com/serhiiivashchenko/textstats-api/pull/7)

Learned:
- Cooperative cancellation
- TaskCompletionSource
- Task.WaitAsync
- HttpMessageHandler testing
- Mutation testing

Key insight:
CancellationToken does not stop an operation
automatically. It must be propagated.

## PR #8 — IOptions
[View PR #8](https://github.com/serhiiivashchenko/textstats-api/pull/8)

Learned:
- Configuration binding
- Strongly typed configuration
- ValidateOnStart
- Environment variables
- Test configuration isolation

Key insight:
Tests must not depend on a developer's
local environment variables.

## PR #10 — Refit Integration

- Migrated OpenAI HTTP integration to Refit.
- Separated application abstraction from infrastructure implementation.
- Learned how Refit generates HTTP requests from interface definitions.
- Implemented an HTTP contract test without real network calls.
- Investigated HttpRequestMessage/HttpContent disposal.
- Learned to verify serialized JSON contracts independently of DTOs.

### Key Insights

**Application vs Infrastructure**
Application defines what our application can do, without knowing about external providers.
Infrastructure implements these capabilities using external services (e.g., OpenAI).
Provider changes should ideally affect only Infrastructure, not Application.

**Mental Model:**
Application = WHAT we do.
Infrastructure = HOW we do it using external services.