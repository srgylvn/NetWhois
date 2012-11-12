using System;
using System.Net.Sockets;

namespace NetWhois.Components
{
	public static class SocketUtilities
	{
		 public static bool Try(Action operation, Action<Exception> onError)
		 {		
			 try
			 {
				 operation();
				 return true;
			 }
			 catch (SocketException ex)
			 {
				 onError(ex);
				 return false;
			 }
			 catch (ObjectDisposedException ex)
			 {
				 onError(ex);
				 return false;
			 }
			 catch (ArgumentException ex)
			 {
				 onError(ex);
				 return false;
			 }
			 catch (InvalidOperationException ex)
			 {
				 onError(ex);
				 return false;
			 }			
		 }
	}
}