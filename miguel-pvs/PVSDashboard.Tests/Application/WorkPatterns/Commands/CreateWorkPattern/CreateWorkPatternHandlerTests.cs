using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Project1.Application.WorkPatterns.Commands.CreateWorkPattern;
using Project1.Persistance;
using Project1.Events.UnsLogicEvents;
using Project1.Infrastructure;

namespace PVSDashboard.Tests.Application.WorkPatterns.Commands.CreateWorkPattern
{
    public class CreateWorkPatternHandlerTests
    {
        private readonly Mock<IWorkPatternRepository> _workPatternRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnsService> _unsServiceMock;
        private readonly CreateWorkPatternHandler _handler;

        public CreateWorkPatternHandlerTests()
        {
            _workPatternRepositoryMock = new Mock<IWorkPatternRepository>(MockBehavior.Strict);
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _unsServiceMock = new Mock<IUnsService>(MockBehavior.Strict);
            _handler = new CreateWorkPatternHandler(_workPatternRepositoryMock.Object, _mediatorMock.Object,_unsServiceMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<CreateWorkPatternResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
        }

        [Fact(DisplayName = "Handle should call CreateWorkPatternAsync on WorkPatternRepository")]
        public async Task HandleShouldCallCreateWorkPatternAsyncOnWorkPatternRepository_WhenCommandIsSet()
        {
            // Arrange
            var workPattern = new Project1.Models.WorkPattern
            {
                UserId = 1,
                Id= new Guid(),
                StartDate = new DateTime(2022,1,1,0,0,0),
                EndDate =  new DateTime(2023,1,1,0,0,0),
                
            };

            _workPatternRepositoryMock
                .Setup(x => x.CreateWorkPatternAsync(It.IsAny<Project1.Models.WorkPattern>(), CancellationToken.None))
                .ReturnsAsync(workPattern);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<CreateWorkPatternLogicEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new CreateWorkPatternCommand(1, new CreateWorkPatternCommandBody
            {
                StartDate = new DateTime(2022, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2023, 1, 1, 0, 0, 0),
                Parts = new List<Project1.Models.WorkPatternPart>()
            });

            // Act
            CreateWorkPatternResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.WorkPattern.Should().NotBeNull();

            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<CreateWorkPatternLogicEvent>(), CancellationToken.None), Times.Once);
        }
    }
}
