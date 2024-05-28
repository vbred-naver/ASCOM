/* FocuserPIC_24C02.c" */
#ifdef _FOCUSER_

/*****************************************************************************
 * I2C Application Example Program ( READ /WRITE 24LC32A EEPROM )
 *****************************************************************************
 * FileName:        I2C1_24LC32A_RD_WR_UART2_PRIPLL_8x10MHz.c
 * Processor:       dsPIC33FJ256GP710
 * Compiler:        MPLAB C30
 * Linker:          MPLAB LINK30
 * Company:         Microchip Technology Incorporated
 *
 * Software License Agreement
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Philip Seo	        2007/02/28	         ...	
 *****************************************************************************/

#define I2C_BAUDRATE	400000		//400KHz
#define I2CBRG			((FCY/I2C_BAUDRATE) - (FCY/1111111)) - 1

volatile char eep_read_buffer[20];
volatile char eep_write_buffer[20];

volatile unsigned int i2cReadData;
volatile bool done_flag;

void i2c1_initial(void)
{   
	// open drain
	ODCBbits.ODCB8 = 1;		// SCL1, RB8
	ODCBbits.ODCB9 = 1;		// SDA1, RB9

	I2C1CONbits.A10M = 0;	// 슬레이브 모드일 때 I2C1ADD를 7비트 어드레스 설정
	I2C1BRG = I2CBRG;		// 마스터 보 레이트 설정 : 400KHz

	I2C1ADD = 0;			// 슬레이브 어드레스 설정(마스터 모드 이므로 사용 안함)
	I2C1MSK = 0;			// 마스크 설정 레지스터(사용 안함)

	I2C1CONbits.I2CEN = 1;	// I2C1 모듈 동작 및 SCL1,SDA1핀 I2C 통신 동작 설정
	IEC1bits.MI2C1IE = 1;	// I2C1 모듈 마스터 모드 인터럽트 동작 중지
	IPC4bits.MI2C1IP = 7;	// I2C1 Master Events Interrupt Priority bits
	IFS1bits.MI2C1IF = 0;	// I2C1 모듈 마스터 모드 인터럽트 플래그 클리어

	done_flag = 0;
}

void i2c1_done(void)
{
	while(done_flag == 0);
	done_flag = 0;
}

unsigned char i2c1_read_random(unsigned char address)
{

	eep_read_buffer[0] = 0;							// buffer clear

	// Start
	I2C1CONbits.SEN = 1;							// Start Condition Enable bit
	i2c1_done();
	
	// Control Byte
	I2C1TRN = 0xA0;									// Transmit register. 0b10100000
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Word Address
	I2C1TRN = address;								// Transmit register
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Start
	I2C1CONbits.RSEN = 1;							// Restart Condition Enable bit
	i2c1_done();

	// Control Byte
	I2C1TRN = 0xA1;									// Transmit register. 0b10100001
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Reveive Enable
	I2C1CONbits.RCEN = 1;							// Receive Enable bit
	i2c1_done();

	// Data Receive
	eep_read_buffer[0] = I2C1RCV;

	// No ACK
	I2C1CONbits.ACKDT = 1;							// 1 = Send NACK during Acknowledge
	I2C1CONbits.ACKEN = 1;							// 1 = Initiate Acknowledge sequence on SDAx and SCLx pins and transmit ACKDT data bit
	i2c1_done();

	// STOP 
	I2C1CONbits.PEN = 1;							// Stop Condition Enable bit
	i2c1_done();

	return true;
}

unsigned char i2c1_read_sequential(unsigned char address, unsigned char count)
{
	unsigned char i;

	// Start
	I2C1CONbits.SEN = 1;							// Start Condition Enable bit.
	i2c1_done();
	
	// Control Byte
	I2C1TRN = 0xA0;									// Transmit register. 0b10100000
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Word Address
	I2C1TRN = address;								// Transmit register
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Start
	I2C1CONbits.RSEN = 1;							// Restart Condition Enable bit
	i2c1_done();

	// Control Byte
	I2C1TRN = 0xA1;									// Transmit register. 0b10100001
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	for (i = 0; i < (count-1); i++)
	{
		// Reveive Enable
		I2C1CONbits.RCEN = 1;						// Receive Enable bit
		i2c1_done();

		// Data Receive n
		eep_read_buffer[i] = I2C1RCV;

		// ACK
		I2C1CONbits.ACKDT = 0;						// 0 = Send ACK during Acknowledge
		I2C1CONbits.ACKEN = 1;						// 1 = Initiate Acknowledge sequence on SDAx and SCLx pins and transmit ACKDT data bit
		i2c1_done();
	}

	// Reveive Enable
	I2C1CONbits.RCEN = 1;							// Receive Enable bit
	i2c1_done();

	// Data Receive
	eep_read_buffer[count-1] = I2C1RCV;
	
	// No ACK 
	I2C1CONbits.ACKDT = 1;							// 1 = Send NACK during Acknowledge
	I2C1CONbits.ACKEN = 1;							// 1 = Initiate Acknowledge sequence on SDAx and SCLx pins and transmit ACKDT data bit
	i2c1_done();
	
	// STOP 
	I2C1CONbits.PEN = 1;							// Stop Condition Enable bit
	i2c1_done();

	return true;
}

unsigned char i2c1_write_byte(unsigned char address, unsigned char data)
{
	
	// Start
	I2C1CONbits.SEN = 1;							// Start Condition Enable bit.
	i2c1_done();

	// Control
	I2C1TRN = 0xA0;									// Transmit register. 0b10100000
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Word Address
	I2C1TRN = address;								// Transmit register
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Write Data
	I2C1TRN = data;
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// STOP
	I2C1CONbits.PEN = 1;							// Stop Condition Enable bit
	i2c1_done();

	__delay_ms(5);									// Byte write within 5 ms

	return true;
}

unsigned char i2c1_write_page(unsigned char address, unsigned char counter)
{
	unsigned char i;
	
	// Start
	I2C1CONbits.SEN = 1;							// Start Condition Enable bit.
	i2c1_done();

	// Control Byte
	I2C1TRN = 0xA0;									// Transmit register. 0b10100000
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Word Address
	I2C1TRN = address;								// Transmit register
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	for (i = 0; i < counter ; i++)
	{
		// Write data n
		I2C1TRN = eep_write_buffer[i];
		i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK
	}

	// STOP
	I2C1CONbits.PEN = 1;							// Stop Condition Enable bit
	i2c1_done();

	__delay_ms(5);									// Page write within 5 ms

	return true;
}

unsigned char i2c1_write_control(unsigned char address, unsigned char data)
{
	// Start
	I2C1CONbits.SEN = 1;							// Start Condition Enable bit.
	i2c1_done();

	// Control Byte
	I2C1TRN = 0x30;									// Transmit register. 0b00110000
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Word Address
	I2C1TRN = address;								// Transmit register
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Write Data
	I2C1TRN = data;
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// STOP
	I2C1CONbits.PEN = 1;							// Stop Condition Enable bit
	i2c1_done();

	return true;
}

unsigned char i2c1_read_control(void)
{
	eep_read_buffer[0] = 0;							// buffer clear
	
	// Start
	I2C1CONbits.SEN = 1;							// Start Condition Enable bit.
	i2c1_done();
	
	// Control Byte
	I2C1TRN = 0x31;									// Transmit register. 0b00110001
	i2c1_done();
	if (I2C1STATbits.ACKSTAT == 1) return false;	// ACK

	// Reveive Enable
	I2C1CONbits.RCEN = 1;							// Receive Enable bit
	i2c1_done();

	// Receive Data
	eep_read_buffer[0] = I2C1RCV;

	// No ACK 
	I2C1CONbits.ACKDT = 1;							// 1 = Send NACK during Acknowledge
	I2C1CONbits.ACKEN = 1;							// 1 = Initiate Acknowledge sequence on SDAx and SCLx pins and transmit ACKDT data bit
	i2c1_done();

	// STOP 
	I2C1CONbits.PEN = 1;							// Stop Condition Enable bit
	i2c1_done();

	return true;
}

// I2C1 ISR
void __attribute__((__interrupt__, no_auto_psv)) _MI2C1Interrupt(void)
{
	done_flag = 1;									// Done Flag
    IFS1bits.MI2C1IF = 0;							// I2C1 Master Events Interrupt Flag Status bit
}

#endif