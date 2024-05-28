/* FocuserPIC.h" */
#ifdef _FOCUSER_

//enum asc_table{AbsSET,AbsCLEAR,DescOFFRESET,DescOFF,DescON,DescONRESET,Halt,IsMoving,Move,MaxInc,MaxStep,Position,StepSize,TempCompGET,TempCompCLEAR,TempCompSET,Temperature};

#define OK				1		// auto_status
#define NG				0		// auto_status
#define ALARM			1		// eeprom_status
#define NOALARM		0		// eeprom_status

#define true			1
#define false			0

#define FAULT			1		// eeprom

#define START			1		// auto_mode, test_mode
#define STOP			0		// auto_mode, test_mode

#define ON				1		// OTF_OUT
#define OFF				0		// OTF_OUT

#define MOVE			1		// command
#define MAXSTEP		2		// command
#define MAXINCREMENT	3		// command
#define STEPSIZE		4		// command

// buffer size
#define RBUFSIZE		5
#define PBUFSIZE		7
#define SRBUFSIZE		10

// eeprom address
#define ADDR_REVISION	0
#define ADDR_POSI			1

#define REVISION			1			// 1:2024-01-15
#define SENSOR_LEVEL		2700		// ref=3V, 
#define GLASS_LEVEL		100		// 100, 15, 600-557=43(new), 650-618=32(old)
#define LIMIT				250		// 250

#define make16(hi,lo)	((unsigned char) hi << 8) | (unsigned char) lo
#define hbyte8(w)	((unsigned char) ((((int) (w)) >> 8) /* & 0xFF*/)) 
#define lbyte8(w)	((unsigned char) (w)) 

#define hbyte4(w)	((unsigned char) ((((int) (w)) >> 4))) 
#define lbyte4(w)	((unsigned char) (w & 0x0f))

//volatile BOOL save_renew;
volatile bool eep_status;

// eeprom
volatile unsigned char revision;

// timer
volatile unsigned char tick;
volatile unsigned char tick_div;
volatile unsigned char tick_adc;
volatile unsigned char t1_counter;
volatile unsigned char t2_counter;
volatile unsigned char test_counter;

// command
volatile unsigned char command;

// step
volatile signed long int c_posi;	// current position, 32bit
volatile signed long int r_posi;	// receive position, 32bit
char s_posi[10];						// string position
volatile unsigned long int max_step;		// 32bit
volatile unsigned long int max_increment;	// 32bit
volatile unsigned int step_size;				// 16bit


// motor
volatile bool first_flag;		// motor on delay
volatile bool motor_start;
volatile bool halt;
volatile bool moving;
volatile bool stability;
volatile bool swing;
volatile unsigned char stability_counter;

// temp
volatile signed int c_temp;	// current temp
volatile signed int c_temp_1;	// sample1 temp
volatile signed int c_temp_2;	// sample2 temp
volatile unsigned char c_temp_counter;
char s_temp[10];					// string temp
volatile bool temp_comp;		// temp comp
volatile unsigned char low_byte;
volatile unsigned char high_byte;
volatile unsigned char config_reg;
volatile unsigned char resolution;
volatile bool tErr;
volatile unsigned char t_err_counter;

// LED
volatile unsigned char blink_counter;
volatile bool link;
volatile unsigned char link_counter;


// mode
volatile bool absolute;

// UART
unsigned char r_counter;				// receive counter
volatile signed int p_length;			// receive position data byte length, -32768 ~ 32768
char r_buffer[5];						// receive buffer 
char p_buffer[7];							// receive position data buffer
char ss_buffer[30];						// 송신 예약
unsigned char ss_counter;				// 송신 예약

char str_tmp[10];					// string tmp

const char D0[4] = ".0#";
const char AC[4] = "AC";	// 16707, Absolute(CLEAR)
const char AS[4] = "AS";	// 16723, Absolute(SET)
const char AX[4] = "AX";	// 16728, Absolute(CLEAR), step Reset
const char AY[4] = "AY";	// 16729, Absolute(SET), step Reset
//const char DA[3] = "DA";	// 17473, Description(GET, Servo OFF, Position RESET)
//const char DB[3] = "DB";	// 17474, Description(GET, Servo OFF)
const char DE[3] = "DE";	// 17477, Description(GET)
//const char DR[3] = "DR";	// 17490, Description(GET, Servo ON, Position RESET)
const char HA[3] = "HA";	// Halt(SET)
const char IS[3] = "IS";	// 18771, IsMoving(GET)
const char PO[3] = "PO";	// 20559, Position(GET)
const char TC[3] = "TC";	// 21571, TempComp(CLEAR)
const char TE[3] = "TE";	// 21573, Temperature(GET)
const char TG[3] = "TG";	// 21575, TempComp(GET)
const char TS[3] = "TS";	// 21587, TempComp(SET)
const char MI[3] = "MI";	// 19785, MaxIncrement(SET)
const char MO[3] = "MO";	// 19791, Move(SET)
const char MS[3] = "MS";	// 19795, MaxStep(SET)
const char SS[3] = "SS";	// 21331, StepSize(SET)
const char dot[2] = ".";
const char terminate[2] = "#";
const char false_terminate[3] = "0#";
const char true_terminate[3] = "1#";
const char description[20] = "H1b#";
const char t_table[17] = "00112334556678890";
const char fail_temp[3] = "0#";

char emptyTerminate[3] = " #";

#endif