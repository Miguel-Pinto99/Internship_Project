using FluentAssertions.Specialized;
using FluentAssertions;
using Moq;
using Project1.Persistance;
using Xunit;
using Project1.Application.WorkPatterns.Queries.GetAllWorkPattern;
using MediatR;

namespace PVSDashboard.Tests.Application.WorkPatterns.Queries.GetAllWorkPattern
{
    public class GetAllWorkPatternHandlerTests
    {
        private readonly Mock<IWorkPatternRepository> _workPatternRepositoryMock;
        private readonly GetAllWorkPatternHandler _handler;

        public GetAllWorkPatternHandlerTests()
        {
            _workPatternRepositoryMock = new Mock<IWorkPatternRepository>(MockBehavior.Strict);
            _handler = new GetAllWorkPatternHandler(_workPatternRepositoryMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<GetAllWorkPatternResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call GetAllWorkPatternAsync on WorkPatternRepository")]
        public async Task HandleShouldCallGetAllWorkPatternAsyncOnWorkPatternRepository_WhenCommandIsSet()
        {
            // Arrange
            var listAllWorkPatterns = new List<Project1.Models.WorkPattern>
                {
                    new Project1.Models.WorkPattern
                    {
                        Id = Guid.NewGuid(),
                        UserId = 1,
                        StartDate = new DateTime(2022, 11, 29, 10, 0, 0),
                        EndDate = new DateTime(2022, 11, 30, 0, 0, 0)
                    }
                };
            _workPatternRepositoryMock
                .Setup(x => x.GetAllWorkPatternsAsync(CancellationToken.None))
                .ReturnsAsync(listAllWorkPatterns);

            var command = new GetAllWorkPatternCommand();
            // Act
            GetAllWorkPatternResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.listWorkPattern.Should().NotBeNull();

            _workPatternRepositoryMock
                .Verify(x => x.GetAllWorkPatternsAsync(CancellationToken.None), Times.Once);

        }
    }
}
