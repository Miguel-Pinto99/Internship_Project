using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Project1.Application.WorkPatterns.Commands.DeleteWorkPattern;
using Project1.Persistance;
using Project1.Events.UnsLogicEvents;

namespace PVSDashboard.Tests.Application.WorkPatterns.Commands.DeleteWorkPattern
{
    public class DeleteWorkPatternHandlerTests
    {
        private readonly Mock<IWorkPatternRepository> _workPatternRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DeleteWorkPatternHandler _handler;

        public DeleteWorkPatternHandlerTests()
        {
            _workPatternRepositoryMock = new Mock<IWorkPatternRepository>(MockBehavior.Strict);
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new DeleteWorkPatternHandler(_workPatternRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<DeleteWorkPatternResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call DeleteWorkPatternAsync on WorkPatternRepository")]
        public async Task HandleShouldCallDeleteWorkPatternAsyncOnWorkPatternRepository_WhenCommandIsSet()
        {
            // Arrange
            var workPattern = new Project1.Models.WorkPattern
            {
                UserId = 1,
                Id = new Guid(),
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
            };

            _workPatternRepositoryMock
                .Setup(x => x.DeleteWorkPatternAsync(new Guid(), CancellationToken.None))
                .ReturnsAsync(workPattern);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<DeleteWorkPatternLogicEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new DeleteWorkPatternCommand(new Guid());

            // Act
            DeleteWorkPatternResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.WorkPattern.Should().NotBeNull();

            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<DeleteWorkPatternLogicEvent>(), CancellationToken.None), Times.Once);

            _workPatternRepositoryMock
                .Verify(x => x.DeleteWorkPatternAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
        }
    }
}
