﻿using NetWhois.Components;

namespace NetWhois.Imp.Protocol
{
	public interface IProtocolFactory
	{
		IWhoisProtocol CreateWhois(ISocketAsyncAdapter boundSocket);
	}
}