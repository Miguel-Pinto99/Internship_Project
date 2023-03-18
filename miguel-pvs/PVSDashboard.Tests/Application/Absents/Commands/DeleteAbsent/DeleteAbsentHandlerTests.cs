using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Project1.Application.Absent.Commands.DeleteAbsent;
using Project1.Persistance;
using Xunit;
using Project1.Models;
using Project1.Events.AbsentLogicEvents;

namespace PVSDashboard.Tests.Application.Absents.Commands.DeleteAbsent
{
    public class DeleteAbsentHandlerTests
    {
        private readonly Mock<IAbsentRepository> _absentRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DeleteAbsentHandler _handler;

        public DeleteAbsentHandlerTests()
        {
            _absentRepositoryMock = new Mock<IAbsentRepository>(MockBehavior.Strict);
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _handler = new DeleteAbsentHandler(_absentRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<DeleteAbsentResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call DeleteAbsentAsync on AbsentRepository")]
        public async Task HandleShouldCallDeleteAbsentAsyncOnAbsentRepository_WhenCommandIsSet()
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
                .Setup(x => x.DeleteAbsentAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(absent);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<AbsentLogicEvent>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var command = new DeleteAbsentCommand(new Guid());

            // Act
            DeleteAbsentResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Absent.Should().NotBeNull();

            _mediatorMock
                .Verify(x => x.Publish(It.IsAny<AbsentLogicEvent>(), CancellationToken.None), Times.Once);

            _absentRepositoryMock
                .Verify(x => x.DeleteAbsentAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
        }
    }
}
