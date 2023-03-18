using FluentAssertions.Specialized;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Project1.Application.Absent.Queries.GetAbsentByIdById;
using Project1.Persistance;
using Project1.Application.Absent.Queries.GetAbsent;

namespace PVSDashboard.Tests.Application.Absentss.Queries.GetById
{
    public class GetByIdHandlerTests
    {
        private readonly Mock<IAbsentRepository> _absentRepositoryMock;
        private readonly GetAbsentByIdHandler _handler;

        public GetByIdHandlerTests()
        {
            _absentRepositoryMock = new Mock<IAbsentRepository>(MockBehavior.Strict);
            _handler = new GetAbsentByIdHandler(_absentRepositoryMock.Object);
        }

        [Fact(DisplayName = "Handle should throw ArgumentNullException when command is not set")]
        public async Task HandleShouldThrowArgumentNullException_WhenCommandIsNotSet()
        {
            // Arrange

            Func<Task<GetAbsentByIdResponse>> act = async () => await _handler.Handle(null, CancellationToken.None);
            ExceptionAssertions<ArgumentNullException> exception = await act.Should().ThrowAsync<ArgumentNullException>();

            exception.WithMessage("Value cannot be null. (Parameter 'command')");
            // Act

            // Assert
        }

        [Fact(DisplayName = "Handle should call GetByIdAsync on Repository")]
        public async Task HandleShouldCallGetByIdAsyncOnRepository_WhenCommandIsSet()
        {
            // Arrange
            var listAbsentsById = new List<Project1.Models.Absent>
                {
                    new Project1.Models.Absent
                    {
                        Id = Guid.NewGuid(),
                        UserId = 1,
                        StartDate = new DateTime(2022, 11, 29, 10, 0, 0),
                        EndDate = new DateTime(2022, 11, 30, 0, 0, 0)
                    }
                };

            _absentRepositoryMock
                .Setup(x => x.GetAbsentByIdAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(listAbsentsById);

            var command = new GetAbsentByIdCommand(1);

            // Act
            GetAbsentByIdResponse response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.ListAbsent.Should().NotBeNull();

            _absentRepositoryMock
                .Verify(x => x.GetAbsentByIdAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);

        }
    }
}
