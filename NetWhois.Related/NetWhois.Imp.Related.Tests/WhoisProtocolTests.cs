using System.Text;
using System.Threading.Tasks;
using System.Linq;
using FakeItEasy;
using NetWhois.Components;
using NUnit.Framework;
using NetWhois.Imp.Protocol;
using NetWhois.Imp.Related.Tests.Fakes;

namespace NetWhois.Imp.Related.Tests
{
	[TestFixture]
	public class WhoisProtocolTests
	{		
		private IAsyncSocketAdapter _socket; 

		[SetUp]
		public void Setup()
		{
			_socket = A.Fake<IAsyncSocketAdapter>();
		}


		private WhoisProtocol CreateProtocol()
		{
			return new WhoisProtocol(_socket);
		}

		[Test]
		public void GetRequestAsync_GetRequestAsync_UsesAsyncSocet()
		{
			A.CallTo(() => _socket.ReceiveAsync(null, 0, 0))
				.WithAnyArguments()
				.Returns(
					(new TaskFactory<int>()).StartNew(() => 0)
				);

			var protocol = CreateProtocol();

			protocol.GetRequestAsync().Wait();

			A.CallTo(
				() => _socket.ReceiveAsync(null, 0, 0)
				).WithAnyArguments()
				.MustHaveHappened();
		}		
        
		[Test]
		public void GetRequestAsync_GetRequestAsync_ReceivesTillTheEndOfLine()
		{
			var dataSld = "somedomain";
			var dataTld = ".com";
			_socket = new CustomAsyncAdapterFake(dataSld, dataTld, "\r", "\n");
			
			var protocol = CreateProtocol();

			var task = protocol.GetRequestAsync();
			task.Wait();
			var result = task.Result;

			Assert.That(result, Is.EqualTo(dataSld + dataTld));
		}

		[Test]
		public void GetRequestAsync_ReturnsEmptyStringWhenServerAbortsConnection()
		{
			A.CallTo(() => _socket.ReceiveAsync(null, 0, 0))
				.WithAnyArguments()
				.Returns((new TaskFactory<int>().StartNew(() => 0)));

			var protocol = CreateProtocol();

			var task = protocol.GetRequestAsync();
			task.Wait();
			var result = task.Result;

			Assert.That(result, Is.EqualTo(string.Empty));
		}
        
        [Test]
        public void SendResponseAsync_UsesAsyncAdapter()
        {
            A.CallTo(() => _socket.SendAsync(null, 0, 0))
                .WithAnyArguments()
                .Returns((new TaskFactory<int>().StartNew(() => "response".Length)));

            var protocol = CreateProtocol();

            var task = protocol.SendResponseAsync("response");
            task.Wait();

            A.CallTo(() => _socket.SendAsync(null, 0, 0))
                .WithAnyArguments()
                .MustHaveHappened();
        }

        [Test]
        public void SendResponseAsync_SendsAllData()
        {
            string data = "response to be sent";
            var length = new int[] {3, 3, data.Length - 6};

	        A.CallTo(() => _socket.SendAsync(null, 0, 0))
		        .WithAnyArguments()
		        .ReturnsNextFromSequence(
			        length.Select(
						x => (new TaskFactory<int>()).StartNew(() => x)
					).ToArray()
		        );

            var protocol = CreateProtocol();
            var task = protocol.SendResponseAsync(data);
            task.Wait();

	        byte[] expectedBuffer = Encoding.ASCII.GetBytes(data);			
            A.CallTo(() => _socket.SendAsync(				
				A<byte[]>.That.IsSameSequenceAs(expectedBuffer), 0, expectedBuffer.Length)
				).MustHaveHappened();
			A.CallTo(() => _socket.SendAsync(
				A<byte[]>.That.IsSameSequenceAs(expectedBuffer), 3, expectedBuffer.Length - 3)
				).MustHaveHappened();
			A.CallTo(() => _socket.SendAsync(
				A<byte[]>.That.IsSameSequenceAs(expectedBuffer), 6, expectedBuffer.Length - 6)
				).MustHaveHappened();
        }


	}
}