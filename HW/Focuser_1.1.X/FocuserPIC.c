/************************************************************************************
 * EUC-KR
 * FileName:		FocuserPIC.c
 * Version:			V1.1
 * Processor:		DSPIC33FJ12MC201-I/SS, 12Kbyte, 1Kbyte
 * Hardware :		FT230X, TB6608FNG, AT24C02D, DS1820
 * Compiler:		MPLAB XC16 V2.1
 * Last edit:		2024-03-11	start
 * Last edit:		2024-04-01	#ifdef _FOCUSER_
 * Last edit:		2024-05-08	signed int -> signed long int
 * Last edit:		2024-05-27	NINA 커맨드 폭주 문제로 시리얼통신 송.수신부 수정, DS18B20전용
************************************************************************************/
#define _FOCUSER_

#include <xc.h>
#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>
#include <p33FJ12MC201.h>
#include "FocuserPIC.h"

//  Macros for Configuration Fuse Registers 
#pragma config FNOSC = PRIPLL	// Initial Oscillator Source Selection Bit
#pragma config FCKSM = CSECMD	// Clock Switching Mode Bit
#pragma config OSCIOFNC = OFF	// OSC2 Pin I/O Function Enable Bit
#pragma config POSCMD = HS		// Primary Oscillator Mode Select Bit. HS 모드는 10~40MHz의 수정 주파수로 작업하는 데 사용되는 고이득, 고주파 모드입니다
#pragma config WDTPRE = PR32	// Watchdog Timer Prescaler
#pragma config WDTPOST = PS128	// Watchdog Timer Postscaler, 128ms
#pragma config FWDTEN = OFF		// Watchdog Timer Enable Bit
#pragma config ICS = PGD2		// ICD Communication Channel Select Bit

#define FOSC (80000000ULL)
#define FCY	(FOSC/2)
#define BAUD9600	((FCY/9600)/16) - 1
#define BAUD19200	((FCY/19200)/16) - 1
#define BAUD38400	((FCY/38400)/16) - 1
#define BAUD57600	((FCY/57600)/16) - 1
#define BAUD115200	((FCY/115200)/16) - 1

#include <libpic30.h>

// port definitions
#define	USB_TX_EN		PORTAbits.RA4		// USB TX Enable
#define	TRIS_USB_TX_EN	TRISAbits.TRISA4	// input 

#define	TEMP			PORTBbits.RB0		// Temperature Sensor
#define	TRIS_TEMP		TRISBbits.TRISB0	// input

#define	LED				LATBbits.LATB1		// LED
#define	TRIS_LED		TRISBbits.TRISB1	// output

#define MOTOR_DIR		LATBbits.LATB4		// Motor Direction
#define	TRIS_MOTOR_DIR	TRISBbits.TRISB4	// output

#define MOTOR_RST		LATBbits.LATB7		// Motor Reset
#define	TRIS_MOTOR_RST	TRISBbits.TRISB7	// output

#define MOTOR_EN		LATBbits.LATB12		// Motor Enable
#define	TRIS_MOTOR_EN	TRISBbits.TRISB12	// output

#define MOTOR_CLK		LATBbits.LATB13		// Motor Clock
#define	TRIS_MOTOR_CLK	TRISBbits.TRISB13	// output

#define USB_TXD			LATBbits.LATB15		// USB TXD
#define	TRIS_USB_TXD	TRISBbits.TRISB15	// output
#define USB_RXD			LATBbits.LATB14		// USB RXD
#define	TRIS_USB_RXD	TRISBbits.TRISB14	// input

#define	I2C_CLK			LATBbits.LATB8		// I2C Clock
#define	I2C_DATA		LATBbits.LATB9		// I2C Data
#define	TRIS_I2C_CLK	TRISBbits.TRISB8	// I2C Clock(output)
#define	TRIS_I2C_DATA	TRISBbits.TRISB9	// I2C Data(output)

char* itoa(signed long int i, char b[]);
unsigned int make32(unsigned char b32, unsigned char b24, unsigned char b16, unsigned char b8);
void init_eeprom(void);

#include "FocuserPIC_24C02.c"
#include "FocuserPIC_eeprom.c"
#include "FocuserPIC_ds1820.c"
#include "FocuserPIC_motor.c"
#include "FocuserPIC_uart.c"


//************************************************************************************//
// 8MHz, 40MIPS
//************************************************************************************//
void oscConfig(void)
{
	PLLFBD = 38;
	CLKDIVbits.PLLPOST = 0;
	CLKDIVbits.PLLPRE = 0;
	OSCTUN=0;

	RCONbits.SWDTEN = 0;
	while(OSCCONbits.LOCK != 1);
}

//************************************************************************************//
// H/W Initialize
//************************************************************************************//
void init_hw(void)
{
	AD1PCFGL = 0b111111;// AN2 Port = Analog mode, 0 = Port pin in Analog mode

	TRIS_USB_TX_EN = 1;	// USB TX Enable(input)
	TRIS_TEMP = 1;		// Temperature Sensor(input)
	TRIS_LED = 0;		// LED(output)
	TRIS_MOTOR_DIR = 0;	// Motor Direction(output)
	TRIS_MOTOR_RST = 0;	// Motor Reset(output)
	TRIS_MOTOR_EN = 0;	// Motor Enable(output)
	TRIS_MOTOR_CLK = 0;	// Motor Clock(output)

	TRIS_USB_TXD = 0;	// output
	TRIS_USB_RXD = 1;	// input
	
	TRIS_I2C_CLK = 0;	// I2C Clock(output)
	TRIS_I2C_DATA = 0;	// I2C Data(output)
	I2C_CLK = 1;		// I2C Clock
	I2C_DATA = 1;		// I2C Data

	// UART1 Receive
	RPINR18 = 0x0E;		// 1110 = Input tied to RP14, RB4
	// UART1 Transmit
	RPOR7bits.RP15R = 0x03;	// RP15, RB15

	LED = 0;			// LED(output)
}

//************************************************************************************//
// memory Initialize
//************************************************************************************//
void init_mem(void)
{
	r_posi = 0;
	first_flag = 0;
	r_counter = 0;
	motor_start = 0;
	absolute = 0;
	c_temp_counter = 0;
	c_temp_1 = 0;
	c_temp_2 = 0;
	link = 0;
	link_counter = 0;
	t_err_counter = 0;
	
	// step
	itoa(c_posi, s_posi);
	strcat(s_posi, terminate);	// "#"
//	sprintf(s_posi,"%d", c_posi);

	// temp
	itoa(0, s_temp);
	strcat(s_temp,D0);			// ".0#"
//	sprintf(s_temp,"0.0#");
//	sprintf(s_posi,"0#");
}

//************************************************************************************//
// Timer1, 100ms
//************************************************************************************//
void init_timer1(void)
{
	/*
	 * 80M(Fosc)/2=40MHz=25ns
	 * 25ns*256(Prescaler)=6.4us
	 * 6.4us*15625=100ms
	 */
	T1CONbits.TON = 0;			// Disable Timer1(Stops 16-bit Timer1)
	T1CONbits.TCS = 0;			// Select internal instruction cycle clock(Fosc/2=40M)
	T1CONbits.TGATE = 0;		// Disable Gated Timer mode(mode=timer)
	T1CONbits.TCKPS = 0x3;		// Select 1:256 Prescaler(1,8,64,256)
	TMR1 = 0x00;				// Clear timer register
	PR1 = 15625;				// Load the period value (100ms/(256*25ns))=6.4us*15625=100ms
	IPC0bits.T1IP = 0x2;		// Set Timer 1 Interrupt Priority Level
	IFS0bits.T1IF = 0;			// Clear Timer 1 Interrupt Flag
	IEC0bits.T1IE = 1;			// Enable Timer1 interrupt

	RCONbits.SWDTEN = 0;		// 0 = WDT is disable
}

//************************************************************************************//
// Timer2, 4ms
//************************************************************************************//
void init_timer2(void)
{
	T2CONbits.TON = 0;			// Disable Timer2(Stops 16-bit Timer2)
	T2CONbits.TSIDL = 0;		// Continue module operation in Idle mode
	T2CONbits.TCS = 0;			// Select internal instruction cycle clock(Fosc/2=40M)
	T2CONbits.TGATE = 0;		// Disable Gated Timer mode(mode=timer)
	T2CONbits.TCKPS = 0x2;		// Select 1:1 Prescaler(1,8,64,256)
	T2CONbits.T32 = 0;			// Timer2 and Timer3 act as two 16-bit timers
	TMR2 = 0x00;				// Clear timer register
	PR2 = 2500;					// Load the period value (1ms/(64*25ns))=1.6us*625=1ms, 625(1=1K), 1250(2=500) , 1875(3=333),  2500(4=250), 3125(5=200) 3750(6=166), 4375(7=142), 5000(8=125), 5625(9=111), 6250(10=100) :2500
	IPC1bits.T2IP = 3;		// Set Timer 2 Interrupt Priority Level 2
	IFS0bits.T2IF = 0;			// Clear Timer 2 Interrupt Flag
	IEC0bits.T2IE = 1;			// Enable Timer2 interrupt
}

//************************************************************************************//
// UART1 Initialize
//************************************************************************************//
void init_uart1(void)
{
	U1MODEbits.IREN = 0;	// 0 = IrDA encoder and decoder disabled
	U1MODEbits.PDSEL = 0;	// 0 = 8-bit data, no parity
	U1MODEbits.STSEL = 0;	// 0 = One Stop bit
	U1MODEbits.ABAUD = 0;	// 0 = Baud rate measurement disabled or completed
	U1MODEbits.BRGH = 0;	// 0 = BRG generates 16 clocks per bit period (16x baud clock, Standard mode)

	U1BRG = BAUD9600;		// Baud Rate setting for 9600
//	U1BRG = BAUD19200;		// Baud Rate setting for 19200
//	U1BRG = BAUD57600;		// Baud Rate setting for 57600
//	U1BRG = BAUD115200;		// Baud Rate setting for 115200

	// rx
	IPC2bits.U1RXIP = 5;	// Set UART1 Receiver Interrupt Priority
	IFS0bits.U1RXIF = 0;	// Clear UART1 Receiver Interrupt Flag Status
	IEC0bits.U1RXIE = 1;	// Enable UART1 Receiver Interrupt Enable

	// error
	IPC16bits.U1EIP = 4;	// Set UART1 Error Interrupt Priority bits
	IFS4bits.U1EIF = 0;		// Clear  UART1 Error Interrupt Flag Status bit
	IEC4bits.U1EIE = 1;		// Enable UART1 Error Interrupt Enable

	U1MODEbits.UARTEN = 1;	// 1 = UART1 is enabled
	U1STAbits.UTXEN = 1;	// 1 = 전송 활성화, UARTx에 의해 제어되는 UxTX 핀
}

//************************************************************************************//
// integer to string, 가져옴
// stackoverflow.com/questions/9655202/how-to-convert-integer-to-string-in-c
//************************************************************************************//
char* itoa(signed long int i, char b[])
{
	char const digit[] = "0123456789";
	char* p = b;
	if(i<0)
	{
		*p++ = '-';
		i *= -1;
	}
	signed long int shifter = i;
	do {	//Move to where representation ends
		++p;
		shifter = shifter/10;
	} while(shifter);
	*p = '\0';
	do {	//Move back, inserting digits as u go
		*--p = digit[i%10];
		i = i/10;
	} while(i);
	return b;
}

//************************************************************************************//
// make32, b32=b32:b24, b24=b23:b16, b16=b15:b8, b8=b7:b0
//************************************************************************************//
unsigned int make32(unsigned char b32, unsigned char b24, unsigned char b16, unsigned char b8)
{
	return (((((unsigned int)b32 << 8 | b24) << 8) | b16) << 8) | b8;       
}

//************************************************************************************//
// LED Drive, 100ms
//************************************************************************************//
void led_drive(void)
{
	if (eep_status)
	{
		if (++blink_counter > 5)
		{
			blink_counter = 0;
			LED ^= 1;
		}
	}
	else
	{
		if (link)
		{
			if (++blink_counter > 2)
			{
				blink_counter = 0;
				LED ^= 1;				// 통신이 있으면 계속 점멸
			}
		}
		else
		{
			if (++blink_counter > 50)
			{
				blink_counter = 0;
				LED = 0;				// 통신이 었으면 윙크
			}
			else
			{
				LED = 1;
			}
		}
	}
}

//************************************************************************************//
// timer1 ISR, 100ms, stability, LED
// DS18S20 : 9-bit thermometer resolution, Converts temperature in 750 ms (max)
// DS18B20 : 12-bit resolution(750ms), 9-bit resolution(93.75ms) 
//************************************************************************************//
void __attribute__((__interrupt__, no_auto_psv)) _T1Interrupt(void)
{
	if (stability)						// 자체 안정화 시간 = 초
	{
		if (++stability_counter > 1)
		{
			stability = 0;
			stability_counter = 0;
			moving = 0;					// 움직임 종료
		}
	}

	led_drive();

	if (++t1_counter > 17) t1_counter = 0;	// temp

	if (++link_counter > 50)	// led
	{
		link_counter = 0;
		link = 0;
	}
	
	ds18b20();		// ds18b20 디지털 온도계

	IFS0bits.T1IF = 0;		// Clear Timer1 interrupt flag
}

//************************************************************************************//
// timer2 ISR, motor drive 4ms
//************************************************************************************//
void __attribute__((__interrupt__, no_auto_psv)) _T2Interrupt(void)
{
	unsigned char length;

	// step
	if (motor_start) step_driver();

	// serial, 4ms 에 1바이트씩 보낸다
	INTCON1bits.NSTDIS = 1;		// Interrupt Nesting Disable bit, 1 = Interrupt nesting is disabled, 0 = Interrupt nesting is enabled
	length = strlen(ss_buffer);	// 송신 버퍼의 데이터 수
	if (length > 0)
	{
		while (U1STAbits.TRMT == 0);
		U1TXREG = ss_buffer[0];
		memmove(ss_buffer,ss_buffer+1,sizeof(ss_buffer)-1);		// 메모리 시프트
	}

	INTCON1bits.NSTDIS = 0;		// Interrupt Nesting Disable bit, 1 = Interrupt nesting is disabled, 0 = Interrupt nesting is enabled

	IFS0bits.T2IF = 0;	// Clear Timer2 interrupt flag
}

//************************************************************************************//
// timer3 ISR, 사용안함
//************************************************************************************//
/*
void __attribute__((__interrupt__, no_auto_psv)) _T3Interrupt(void)
{
	IFS0bits.T3IF = 0;		// Clear Timer3 interrupt flag
}
*/

//************************************************************************************//
// Main
//************************************************************************************//
int main(void)
{
	oscConfig();			// Configure Oscillator Clock Source
	init_hw();				// Initialize hardware on the board
	init_eeprom();			// 24c02 Initialize & read
	init_mem();				// Initialize mem
	init_motor();			// Initialize motor
	init_uart1();			// Initialize UART
	init_timer1();			// Initialize timer1
	init_timer2();			// Initialize timer2
	motor_on();
	
	T1CONbits.TON = 1;		// Start Timer1
	T2CONbits.TON = 1;		// Start Timer2
	RCONbits.SWDTEN = 1;	// Software Enable/Disable of WDT bit
	
	while (1)
	{
		ClrWdt();			// Clear Watchdog Timer
	}
}
