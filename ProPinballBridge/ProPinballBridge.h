// ProPinballBridge.h

#pragma once
#include "boost/interprocess/ipc/message_queue.hpp"

using namespace System;

namespace ProPinballBridge
{
	public delegate void OnFlasher(int id, const char* name, float intensity);
	public delegate void OnSolenoid(int id, const char* name, int status);
	public delegate void OnFlipper(int id, const char* name, int status);
	public delegate void OnButtonLight(int id, const char* name, int status);
	public delegate void OnError(const char* message);
	public delegate void OnCompleted();

	public ref class ProPinballFeedback
	{
	public:
		ProPinballFeedback(unsigned int message_size);

		void GetFeedback(OnFlasher^ onFlasher, OnSolenoid^ onSolenoid, OnFlipper^ onFlipper, OnButtonLight^ onButtonLight, OnError^ onError, OnCompleted^ onCompleted);
		void Release();

		int Status;
		const char* Error;

	private:
		boost::interprocess::message_queue* master_to_slave_message_queue;
		boost::interprocess::message_queue* slave_to_master_message_queue;
		boost::interprocess::message_queue* open_message_queue(const std::string& message_queue_name);
		unsigned char* general_message_buffer;
		unsigned int general_message_buffer_size;
	};
}