// ProPinballBridge.h

#pragma once
#include "boost/interprocess/ipc/message_queue.hpp"

using namespace System;

namespace ProPinballBridge
{
	public delegate void OnNext(unsigned char* frame);
	public delegate void OnError(const char* message);
	public delegate void OnCompleted();

	public ref class ProPinballFeedback
	{
	public:
		ProPinballFeedback(unsigned int message_size);

		void GetFeedback(OnNext^ onNext, OnError^ onError, OnCompleted^ onCompleted);
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