using Chat.Client.Library.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Entities;
using Xunit;
using Moq;
using FluentAssertions;


// 4 x 12

namespace Chat.Client.Library.Tests
{
    public class ChatClientTests : IDisposable
    {
        private readonly IChatClient _chatClient;

        public ChatClientTests()
        {
            var messages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Author = "Usuario3",
                    Message = "Test",
                    Date = DateTime.Now
                }
            };
            //Creacion y configuracion de Mocks y creacion de IchatClient
            var chat = new Mock<IChatApiClient>(MockBehavior.Strict);
            var user = new Mock<IUserApiClient>(MockBehavior.Strict);

            user.Setup(x => x.LoginAsync("Usuario1", "P2ssw0rd!")).Returns(Task.FromResult(new ChatUser() { Name = "Usuario1" }));
            user.Setup(x => x.LoginAsync("Usuario3", "P2ssw0rd!")).Returns(Task.FromResult(new ChatUser() { Name = "Usuario3" }));
            user.Setup(x => x.CreateUserAsync("Usuario1", "P2ssw0rd!")).Returns(Task.FromResult(new ChatUser() { Name = "Usuario1" }));
            
            chat.Setup(x => x.SendMessageAsync(It.IsAny<ChatMessage>())).Returns(Task.FromResult(true));
            chat.Setup(x => x.GetChatMessagesAsync()).Returns(Task.FromResult<IEnumerable<ChatMessage>>(messages));

            _chatClient = new ChatClient(chat.Object, user.Object);

        }

        [Theory]
        [InlineData(true, "Usuario1", "P2ssw0rd!")]
        [InlineData(false, "Usuario2", "")]
        public async Task LoginAsync_ShouldBeExpectedResult(bool expected, string username, string password)
        {
            //Arrange

            //Act
            var result = await _chatClient.LoginAsync(username, password);

            //Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void LoginAsync_ShouldBeArgumentNullException_IfArgumentNull()
        {
            //Arrange

            //Act
            Func<Task> act = async () => { _ = await _chatClient.LoginAsync(null, null); };

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true, "Usuario1", "P2ssw0rd!")]
        [InlineData(false, "Usuario2", "")]
        public async Task CreateUserAsync_ShouldBeExpectedResult(bool expected, string username, string password)
        {
            //Arrange

            //Act
            var result = await _chatClient.CreateUserAsync(username, password);

            //Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void CreateUserAsync_ShouldBeArgumentNullException_IfArgumentNull()
        {
            //Arrange

            //Act
            Func<Task> act = async () => { _ = await _chatClient.CreateUserAsync(null, null); };

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task SendMessageAsync_ShouldBeTrue_IfLoggedAndConnected()
        {
            //Arrange
            var message = "Message1";
            await _chatClient.LoginAsync("Usuario1", "P2ssw0rd!");
            _chatClient.Connect();

            //Act
            var result = await _chatClient.SendMessageAsync(message);

            //Assert
            Assert.True(result);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task SendMessageAsync_ShouldBeFalse_IfNotLogged()
        {
            //Arrange
            var message = "Message1";

            //Act
            var result = await _chatClient.SendMessageAsync(message);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task NewMessageRecived_ShouldBeRaised_IfConnect()
        {
            var eventRecived = false;
            await _chatClient.LoginAsync("Usuario1", "P2ssw0rd!");
            _chatClient.NewMessageRecived += (sender, _) => eventRecived = true;
            _chatClient.Connect();

            //Act
            await Task.Delay(100); //Le damos tiempo para que se ejecute el evento

            //Assert
            eventRecived.Should().BeTrue();
        }

        [Fact]
        public async Task OverwriteLastLine_ShouldBeRaised_IfOneMessageAndAuthorIsWhoseIsLogged()
        {
            var eventRecived = false;
            await _chatClient.LoginAsync("Usuario3", "P2ssw0rd!");
            _chatClient.OverwriteLastLine += (sender, _) => eventRecived = true;
            _chatClient.Connect();

            //Act
            await Task.Delay(100); //Le damos tiempo para que se ejecute el evento

            //Assert
            eventRecived.Should().BeTrue();
        }

        [Fact]
        public async Task OverwriteLastLine_ShouldNotBeRaised_IfOneMessageAndAuthorIsOther()
        {
            var eventRecived = false;
            await _chatClient.LoginAsync("Usuario1", "P2ssw0rd!");
            _chatClient.OverwriteLastLine += (sender, _) => eventRecived = true;
            _chatClient.Connect();

            //Act
            await Task.Delay(100); //Le damos tiempo para que se ejecute el evento

            //Assert
            eventRecived.Should().BeFalse();
        }

        [Fact]
        public async Task OverwriteLastLine_ShouldNotBeRaised_IfMoreThanOneMessage()
        {
            var eventRecived = false;
            var messages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Author = "Usuario3",
                    Message = "Test",
                    Date = DateTime.Now
                },
                new ChatMessage
                {
                    Author = "Usuario3",
                    Message = "Test",
                    Date = DateTime.Now
                }
            };
            //Modificacion de Mock

            await _chatClient.LoginAsync("Usuario1", "P2ssw0rd!");
            _chatClient.OverwriteLastLine += (sender, _) => eventRecived = true;
            _chatClient.Connect();

            //Act
            await Task.Delay(100); //Le damos tiempo para que se ejecute el evento

            //Assert
            eventRecived.Should().BeFalse();
        }

        public void Dispose()
        {
            _chatClient?.Dispose();
        }
    }
}
