using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using NetWhois.Imp.Protocol;
using NetWhois.Imp.Response;

namespace NetWhois.Imp.Related.Tests
{
	[TestFixture]
	public class WhosRoutineTess
	{
		private IWhoisProtocol _protocol;
		private IResponseBuilder _responseBuilder;
		private string _request;
		private string _response;

		[SetUp]
		public void Setup()
		{
			_request = "request";
			_response = "response";

			_protocol = A.Fake<IWhoisProtocol>();
			_responseBuilder = A.Fake<IResponseBuilder>();

			A.CallTo(() => _protocol.GetRequestAsync())
				.Returns(new TaskFactory<string>().StartNew(() => _request));
			A.CallTo(() => _responseBuilder.BuildResponseAsync(A<string>._))
				.Returns(new TaskFactory<string>().StartNew(() => _response));
		}

		private WhoisRoutine CreateRoutine()
		{
			return new WhoisRoutine(_responseBuilder);
		}

		[Test]
		public void RunAsync_ReadsINformationFromSocket()
		{
			var routine = CreateRoutine();

			routine.RunAsync(_protocol).Wait();

			A.CallTo(() => _protocol.GetRequestAsync()).MustHaveHappened();
		}

		[Test]
		public void RunAsync_UsesResponseBuilderToBuildResponse()
		{		
			var routine = CreateRoutine();

			routine.RunAsync(_protocol).Wait();

			A.CallTo(() => _responseBuilder.BuildResponseAsync(_request)).MustHaveHappened();
		}

		[Test]
		public void RunAsync_SendsResponse()
		{						
			var routine = CreateRoutine();

			routine.RunAsync(_protocol).Wait();

			A.CallTo(() => _protocol.SendResponseAsync(_response)).MustHaveHappened();
		}
	}
}