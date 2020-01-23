// ManagedWrapper.cpp

// This code verifies that DllMain is not called by the Loader
// automatically when linked with /noentry. It also checks some
// functions that the CRT initializes.
#include <windows.h>
#include "_vcclrit.h"

#using <mscorlib.dll>
using namespace System;

namespace Org {

namespace Snmp {

namespace Snmp_pp {

//
// support CRT (C RunTime) initialization and termination
// (use either Auto or Manual, not both)
namespace CRT
{
	volatile bool g_dllUp = true;
	//
	// support CRT initialization and auto termination
	public ref class Auto sealed
	{
	public:
		//
		// call at program start
		// (before using unmanaged code)
		//
		// - termination is handled in a finalizer
		// - call again and retain the returned object in any routine
		//   that has problems with premature CRT termination
		static Object^ Initialize()
		{
			System::Threading::Monitor::Enter( _lock );
			__try
			{
				if ( _initializer == nullptr )
					_initializer = gcnew Initializer();
			}
			__finally
			{
				System::Threading::Monitor::Exit( _lock );
			}
			return _initializer;
		}
	    
	private:
		Auto() {}

		static Auto()
		{
			_lock = gcnew Object();
		}

		ref struct Initializer
		{
			Initializer()
			{
				__crt_dll_initialize();
			}
			~Initializer()
			{
				g_dllUp = false;
				__crt_dll_terminate();
#ifdef	_DEBUG
				System::IO::TextWriter^ out = Console::Out;
				if (out)
					try
					{
						out->WriteLine("CRT terminated!");
						out->Flush();
					}
					catch (ObjectDisposedException^) {}
#endif
			}
		};

		static Object^ _lock;
		static Initializer^ _initializer;
	};
	 
	//
	// support CRT initialization and termination
	public ref class Manual sealed
	{
	public:
		// call at program start
		// (before using unmanaged code)
		static void Initialize()
		{
			try
			{
				__crt_dll_initialize();
			}
			catch( ... )
			{
				throw;
			}
		}
		// call at program end
		// (after using unmanaged code - but this can be problematic)
		static void Terminate()
		{
			try
			{
				g_dllUp = false;
				__crt_dll_terminate();
			}
			catch( ... )
			{
				throw;
			}
		}

	private:
		Manual() {}
	};
};

}	// end of namespace Snmp_pp

}	// end of namespace Snmp

}	// end of namespace Org

BOOL WINAPI DllMain(HINSTANCE hModule, DWORD dwReason, LPVOID lpvReserved)
{
	return TRUE;
}
