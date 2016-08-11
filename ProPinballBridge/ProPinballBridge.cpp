#include "stdafx.h"
#include "ProPinballBridge.h"
#include "boost/interprocess/ipc/message_queue.hpp"
#include <stdio.h>

using namespace boost::interprocess;

typedef int32_t s32;
typedef float f32;

enum MESSAGE_TYPE
{
	MESSAGE_TYPE_SLAVE_READY,
	MESSAGE_TYPE_END,
	MESSAGE_TYPE_FEEDBACK
};

enum SOLENOID_ID
{
	SO_PLUNGER = 0,
	SO_TROUGH_EJECT,
	SO_KNOCKER,
	SO_LEFT_SLINGSHOT,
	SO_RIGHT_SLINGSHOT,
	SO_LEFT_JET,
	SO_RIGHT_JET,
	SO_BOTTOM_JET,
	SO_LEFT_DROPS_UP,
	SO_RIGHT_DROPS_UP,
	SO_LOCK_RELEASE_1,
	SO_LOCK_RELEASE_2,
	SO_LOCK_RELEASE_3,
	SO_LOCK_RELEASE_A,
	SO_LOCK_RELEASE_B,
	SO_LOCK_RELEASE_C,
	SO_LOCK_RELEASE_D,
	SO_MIDDLE_EJECT,
	SO_TOP_EJECT_STRONG,
	SO_TOP_EJECT_WEAK,
	SO_MIDDLE_RAMP_DOWN,
	SO_HIGH_DIVERTOR,
	SO_LOW_DIVERTOR,
	SO_SCOOP_RETRACT,
	SO_MAGNO_SAVE,
	SO_MAGNO_LOCK,
	NUM_SOLENOIDS
};

enum FLASHER_ID
{
	FL_LEFT_RETURN_LANE = 0,
	FL_RIGHT_RETURN_LANE,
	FL_TIME_MACHINE,
	FL_LOCK_ALPHA,
	FL_LOCK_BETA,
	FL_LOCK_GAMMA,
	FL_LOCK_DELTA,
	FL_CRYSTAL,
	NUM_FLASHERS
};

enum FLIPPER_ID
{
	FLIP_LOW_LEFT = 0,
	FLIP_LOW_RIGHT,
	FLIP_HIGH_RIGHT,
	NUM_FLIPPERS
};

enum BUTTON_ID
{
	BUTTON_ID_START,
	BUTTON_ID_FIRE,
	BUTTON_ID_MAGNOSAVE,
	NUM_BUTTONS
};

struct FEEDBACK_MESSAGE_DATA
{
	f32 flasher_intensity[NUM_FLASHERS];
	s32 solenoid_on[NUM_SOLENOIDS];
	s32 flipper_solenoid_on[NUM_FLIPPERS];
	s32 button_lit[NUM_BUTTONS];
};

struct SLAVE_MESSAGE
{
	s32 message_type;
	union
	{
		FEEDBACK_MESSAGE_DATA feedback_message_data;
	} message_data;
};

const char* SOLENOID_NAME[NUM_SOLENOIDS] =
{
	"PLUNGER",
	"TROUGH_EJECT",
	"KNOCKER",
	"LEFT_SLINGSHOT",
	"RIGHT_SLINGSHOT",
	"LEFT_JET",
	"RIGHT_JET",
	"BOTTOM_JET",
	"LEFT_DROPS_UP",
	"RIGHT_DROPS_UP",
	"LOCK_RELEASE_1",
	"LOCK_RELEASE_2",
	"LOCK_RELEASE_3",
	"LOCK_RELEASE_A",
	"LOCK_RELEASE_B",
	"LOCK_RELEASE_C",
	"LOCK_RELEASE_D",
	"MIDDLE_EJECT",
	"TOP_EJECT_STRONG",
	"TOP_EJECT_WEAK",
	"MIDDLE_RAMP_DOWN",
	"HIGH_DIVERTOR",
	"LOW_DIVERTOR",
	"SCOOP_RETRACT",
	"MAGNO_SAVE",
	"MAGNO_LOCK"
};

const char* FLASHER_NAME[NUM_FLASHERS] =
{
	"LEFT_RETURN_LANE",
	"RIGHT_RETURN_LANE",
	"TIME_MACHINE",
	"LOCK_ALPHA",
	"LOCK_BETA",
	"LOCK_GAMMA",
	"LOCK_DELTA",
	"CRYSTAL"
};

const char* FLIPPER_NAME[NUM_FLIPPERS] =
{
	"LOW_LEFT",
	"LOW_RIGHT",
	"HIGH_RIGHT"
};

const char* BUTTON_NAME[NUM_BUTTONS] =
{
	"START",
	"FIRE",
	"MAGNOSAVE"
};

const int DEFAULT_MESSAGE_PRIORITY = 0;

void handle_feedback(const FEEDBACK_MESSAGE_DATA* feedback_data)
{

}

ProPinballBridge::ProPinballFeedback::ProPinballFeedback(unsigned int message_size)
{
	const char* MASTER_TO_SLAVE_QUEUE_NAME = "feedback_master_to_slave";
	master_to_slave_message_queue = open_message_queue(MASTER_TO_SLAVE_QUEUE_NAME);

	const char* SLAVE_TO_MASTER_QUEUE_NAME = "feedback_slave_to_master";
	slave_to_master_message_queue = open_message_queue(SLAVE_TO_MASTER_QUEUE_NAME);

	general_message_buffer_size = message_size; // Get from command line arg m
	general_message_buffer = new unsigned char[general_message_buffer_size];

	SLAVE_MESSAGE* message = (SLAVE_MESSAGE*)general_message_buffer;

	if (slave_to_master_message_queue)
	{
		try
		{
			message->message_type = MESSAGE_TYPE_SLAVE_READY;
			slave_to_master_message_queue->send(message, general_message_buffer_size, DEFAULT_MESSAGE_PRIORITY);
		}
		catch (interprocess_exception &exception)
		{
			Status = 1;
			Error = exception.what();
			printf("Error sending slave ready message: %s\n", exception.what());
		}
	}
}

void ProPinballBridge::ProPinballFeedback::Release()
{
	delete this;
}

void ProPinballBridge::ProPinballFeedback::GetFeedback(OnFlasher^ onFlasher, OnSolenoid^ onSolenoid, OnFlipper^ onFlipper, OnButtonLight^ onButtonLight, OnError^ onError, OnCompleted^ onCompleted)
{
	bool done = false;

	while (!done)
	{
		if (master_to_slave_message_queue)
		{
			SLAVE_MESSAGE* message = (SLAVE_MESSAGE*)general_message_buffer;
			bool received_message = false;

			do
			{
				try
				{
					unsigned int priority;
					message_queue::size_type received_size;

					received_message = master_to_slave_message_queue->try_receive(general_message_buffer,
						general_message_buffer_size,
						received_size,
						priority);
					if (received_message)
					{
						if (received_size == general_message_buffer_size)
						{
							if (message->message_type == MESSAGE_TYPE_END)
							{
								onCompleted();
								done = true;
							}
							else if (message->message_type == MESSAGE_TYPE_FEEDBACK)
							{
								handle_feedback(&(message->message_data.feedback_message_data));
								static FEEDBACK_MESSAGE_DATA previous_feedback_data;
								const FEEDBACK_MESSAGE_DATA* feedback_data = &(message->message_data.feedback_message_data);

								for (int flasher_index = 0; flasher_index < NUM_FLASHERS; flasher_index++)
								{
									if (feedback_data->flasher_intensity[flasher_index] != previous_feedback_data.flasher_intensity[flasher_index])
									{
										onFlasher(flasher_index, FLASHER_NAME[flasher_index], feedback_data->flasher_intensity[flasher_index]);
									}
								}

								for (int solenoid_index = 0; solenoid_index < NUM_SOLENOIDS; solenoid_index++)
								{
									if (feedback_data->solenoid_on[solenoid_index] != previous_feedback_data.solenoid_on[solenoid_index])
									{
										onSolenoid(solenoid_index, SOLENOID_NAME[solenoid_index], feedback_data->solenoid_on[solenoid_index] ? 1 : 0);
									}
								}

								for (int flipper_index = 0; flipper_index < NUM_FLIPPERS; flipper_index++)
								{
									if (feedback_data->flipper_solenoid_on[flipper_index] != previous_feedback_data.flipper_solenoid_on[flipper_index])
									{
										onFlipper(flipper_index, FLIPPER_NAME[flipper_index], feedback_data->flipper_solenoid_on[flipper_index] ? 1 : 0);
									}
								}

								for (int button_index = 0; button_index < NUM_BUTTONS; button_index++)
								{
									if (feedback_data->button_lit[button_index] != previous_feedback_data.button_lit[button_index])
									{
										onButtonLight(button_index, BUTTON_NAME[button_index], feedback_data->button_lit[button_index] ? 1 : 0);
									}
								}
								previous_feedback_data = *feedback_data;

							}
							else
							{
								printf("Received message %d\n", message->message_type);
							}
						}
						else
						{
							printf("Received message size %d, but expecting size %d\n", (int)received_size, general_message_buffer_size);
							onError("Received DMD data has wrong size.");
							received_message = false;
							done = true;
						}
					}
				}
				catch (interprocess_exception &exception)
				{
					onError("Error receiving control message.");
					printf("Control message error: %s\n", exception.what());
					done = true;
					received_message = false;
				}
			} while (received_message);
		}
	}
}

boost::interprocess::message_queue* ProPinballBridge::ProPinballFeedback::open_message_queue(const std::string& message_queue_name)
{
	boost::interprocess::message_queue* message_queue = nullptr;

	try
	{
		message_queue = new boost::interprocess::message_queue(open_only, message_queue_name.c_str());
	}
	catch (interprocess_exception &exception)
	{
		printf("+++ Failed to open message queue '%s', error: '%s'\n", message_queue_name.c_str(), exception.what());
		Status = 1;
		Error = exception.what();
	}
	return message_queue;
}
