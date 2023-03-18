using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Project1.Application.Absent.Commands.CreateAbsent;
using Project1.Persistance;
using Xunit;
using Project1.Models;
using Project1.Events.AbsentLogicEvents;

namespace PVSDashboard.Tests.Application.Absent.Commands.CreateAbsent
{
    public class CreateAbsentHandlerTests
    {
        private readonly Mock<IAbsentRepository> _absentRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CreateAbsentHandler _handler;

        public CreateAbsentHandlerTests()
        {
            _absentRepositoryMock = new Mock<IAbsentRepository>(MockBehavior.Strict);
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new CreateAbsentHandler(_absentRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<CreateAbsentResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call CreateAbsentAsync on AbsentRepository")]
        public async Task HandleShouldCallCreateAbsentAsyncOnAbsentRepository_WhenCommandIsSet()
        {
            // Arrange
            var absent = new Project1.Models.Absent
            {
                Id = Guid.NewGuid(),
                UserId = 1,
                StartDate = new DateTime(2022, 11, 29, 10, 0, 0),
                EndDate = new DateTime(2022, 11, 30, 0, 0, 0)
            };

            _absentRepositoryMock
                .Setup(x => x.CreateAbsentAsync(It.IsAny<Project1.Models.Absent>(), CancellationToken.None))
                .ReturnsAsync(absent);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<AbsentLogicEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new CreateAbsentCommand(1, new CreateAbsentCommandBody
            {
                StartDate = new DateTime(2022, 11, 29, 10, 0, 0),
                EndDate = new DateTime(2022, 11, 30, 0, 0, 0)
            });

            // Act
            CreateAbsentResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Absent.Should().NotBeNull();

            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<AbsentLogicEvent>(), CancellationToken.None), Times.Once);

            _absentRepositoryMock
                .Verify(x => x.CreateAbsentAsync(It.IsAny<Project1.Models.Absent>(),CancellationToken.None), Times.Once);
        }
    }
}
