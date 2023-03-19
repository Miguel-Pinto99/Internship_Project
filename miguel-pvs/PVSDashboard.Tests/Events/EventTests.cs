using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using Xunit;

namespace PVSDashboard.Tests.Events
{
    public class EventsTests
    {
        private readonly Mock<IMediator> _mediatorMock;

        public EventsTests()
        {
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
        }

        [Fact(DisplayName = "TestUnsEvents")]
        public async Task TestUnsEvents()
        {
            // Arrange
            var listWorkPatternParts = new List<WorkPatternPart>();
            var listAbsents = new List<Absent>();
            var applicationUser = new ApplicationUser();
            var officeLocation = 1;
            var userEachLocation = new UsersEachLocation(1,new List<int>{ 1, 2 });

            // Act
            var eventDeleteTopicApplicationUser = new DeleteTopicApplicationUserEvent(applicationUser);
            var eventPublishCheckIn = new PublishCheckInEvent(applicationUser);
            var eventPublishLocation = new PublishLocationEvent(userEachLocation,1);
            var eventPublishWorkPattern = new PublishWorkPatternEvent(applicationUser);
            var eventRemoveTimer = new RemoveTimerEvent(applicationUser);
            var eventStopTimer = new StopTimerEvent(applicationUser);
            var eventUpdateTimer = new UpdateTimerEvent(applicationUser);

            _mediatorMock.Setup(x => x.Publish(eventDeleteTopicApplicationUser, CancellationToken.None))
                .Returns(Task.CompletedTask);
            _mediatorMock.Setup(x => x.Publish(eventPublishCheckIn, CancellationToken.None))
                .Returns(Task.CompletedTask);
            _mediatorMock.Setup(x => x.Publish(eventPublishLocation, CancellationToken.None))
                .Returns(Task.CompletedTask);
            _mediatorMock.Setup(x => x.Publish(eventPublishWorkPattern, CancellationToken.None))
                .Returns(Task.CompletedTask);
            _mediatorMock.Setup(x => x.Publish(eventRemoveTimer, CancellationToken.None))
                .Returns(Task.CompletedTask);
            _mediatorMock.Setup(x => x.Publish(eventStopTimer, CancellationToken.None))
                .Returns(Task.CompletedTask);
            _mediatorMock.Setup(x => x.Publish(eventUpdateTimer, CancellationToken.None))
                .Returns(Task.CompletedTask);

            // Assert
            eventDeleteTopicApplicationUser.ApplicationUser.Should().BeEquivalentTo(applicationUser);
            eventPublishCheckIn.ApplicationUser.Should().BeEquivalentTo(applicationUser);
            eventPublishLocation.UsersEachLocation.Should().BeEquivalentTo(userEachLocation);
            eventPublishLocation.OfficeLocation.Should().Be(1);
            eventRemoveTimer.ApplicationUser.Should().BeEquivalentTo(applicationUser);
            eventPublishWorkPattern.ApplicationUser.Should().BeEquivalentTo(applicationUser);
            eventStopTimer.ApplicationUser.Should().BeEquivalentTo(applicationUser);
            eventUpdateTimer.ApplicationUser.Should().BeEquivalentTo(applicationUser);
        }
    }
}

