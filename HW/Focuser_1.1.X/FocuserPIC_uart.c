/* FocuserPIC_uart.c" */
#ifdef _FOCUSER_

void r_buffer_clear(void);

//************************************************************************************//
// serial puts(string)
//************************************************************************************//
/*
void serial_puts(const char *c)
{
	const char *p = c;

	while(*p)
	{
		while (U1STAbits.TRMT == 0);
		U1TXREG = *p;
		++p;
	}
}
*/
//************************************************************************************//
// command process
// 수신 데이터 처리
//************************************************************************************//
void cmd_move_proc(void)
{
	r_posi = atol(p_buffer);

	moving = 1;
	stability = 0;
	stability_counter = 0;
	motor_start = 1;
	r_buffer_clear();
}

void cmd_maxstep_proc(void)
{
	max_step = atol(p_buffer);
	r_buffer_clear();
}

void cmd_maxinc_proc(void)
{
	max_increment = atol(p_buffer);
	r_buffer_clear();
}

void cmd_stepsize_proc(void)
{
	step_size = atoi(p_buffer);
	r_buffer_clear();
}

//************************************************************************************//
// receive buffer clear
//************************************************************************************//
void r_buffer_clear(void)
{
//	memset(r_buffer, 0, RBUFSIZE);
	r_counter = 0;
	command = 0;
}

//************************************************************************************//
// receive position buffer clear
//************************************************************************************//
void p_buffer_clear(void)
{
	memset(p_buffer, 0, PBUFSIZE);
	p_length = 0;
}

//************************************************************************************//
// UART1 RX ISR
//
// "DA"	Description(GET, Servo OFF, Position RESET)				string & terminate				44 41 17473
// "DB"	Description(GET, Servo OFF)								string & terminate				44 42 17474
// "DE"	Description(GET)										string & terminate				44 45 17477
// "DR"	Description(GET, Servo ON, Position RESET)				string & terminate				44 52 17490
//
// "AC"	Absolute(CLEAR)								relative	terminate						41 43 16707
// "AS"	Absolute(SET)								absolute	terminate						41 53 16723
// "AX"	Absolute(CLEAR, Position RESET)				relative	terminate						41 58 16728
// "AY"	Absolute(SET, Position RESET)				absolute	terminate						41 59 16729
// "HA"	Halt(SET)												응답하지않음						48 41 18497
// "IS"	IsMoving(GET)								moving Y?	moving:1 & terminate			49 53 18771
// "IS"	IsMoving(GET)								moving N?	moving:0 & terminate			49 53 18771
// "PO"	Position(GET)								position ?	position(step) & terminate		50 4F 20559
// "TC"	TempComp(CLEAR)								TempComp[0]	terminate						54 43 21571
// "TE"	Temperature(GET)							temp ?		temperature(xx.x) & terminate	54 45 21573
// "TG"	TempComp(GET)								TempComp Y?	temp_comp: 1 & terminate		54 47 21575
// "TG"	TempComp(GET)								TempComp N?	temp_comp: 0 & terminate		54 47 21575
// "TS"	TempComp(SET)								TempComp[1]	terminate						54 53 21587
// "MO"	Move(SET)									MO{xxxx}	terminate						4D 4F 19791
// "MI"	MaxIncrement(SET)							MI{xxxx}	terminate						4D 49 19785
// "MS"	MaxStep(SET)								MS{xxxx}	terminate						4D 53 19795
// "SS"	StepSize(SET)								SS{xx}		terminate						53 53 21331
//
//************************************************************************************//
void __attribute__((__interrupt__, no_auto_psv)) _U1RXInterrupt(void)
{
	unsigned char r_data;
	unsigned int sum;

	INTCON1bits.NSTDIS = 1;		// Interrupt Nesting Disable bit, 1 = Interrupt nesting is disabled, 0 = Interrupt nesting is enabled

	IFS0bits.U1RXIF = 0;				// Clear UART1 RX interrupt flag

	r_data = U1RXREG;

	switch (r_counter)
	{
		case 0:
			if (r_data > 0x40 && r_data < 0x5B)				// A:41, Z:5A, byte 0, 문자
			{
				r_buffer[r_counter] = r_data;
				++r_counter;
			}
			break;

		case 1:
			if (r_data > 0x40 && r_data < 0x5B)				// A:41, Z:5A, byte 1, 문자
			{
				r_buffer[r_counter] = r_data;
				++r_counter;
			}
			else
			{
				r_buffer_clear();
			}
			break;
			
		case 2:
			if (r_data == 0x23)								// #:23, byte 2, terminate
			{
				link = 1;
				link_counter = 0;

				sum = make16(r_buffer[0],r_buffer[1]);
				switch(sum)
				{
					case 16707:							// "AC" AbsCLEAR		41 43 16707
						absolute = 0;					// relative
						r_buffer_clear();
						break;
					case 16723:							// "AS" AbsSET			41 53 16723
						absolute = 1;					// absolute
						r_buffer_clear();
						break;
					case 16728:							// "AX" AbsCLEAR, Reset	41 58 16728
						absolute = 0;					// relative
						c_posi = 0;						// reset
						r_buffer_clear();
						break;
					case 16729:							// "AY" AbsSET, Reset	41 59 16729
						absolute = 1;					// absolute
						c_posi = 0;						// reset
						r_buffer_clear();
						break;
					case 18497:							// "HA" Halt			48 41 18497
						if (moving) halt = 1;
						r_buffer_clear();
						break;
					case 18771:							// "IS" IsMoving		49 53 18771
						if (moving)
						{
							strcat(ss_buffer, true_terminate);	// "1#"
						}
						else
						{
							strcat(ss_buffer, false_terminate);	// "0#"
						}
						r_buffer_clear();
						break;
					case 20559:							// "PO" Position		50 4F 20559
						itoa(c_posi, s_posi);
						strcat(s_posi, terminate);
						strcat(ss_buffer, s_posi);		// "1000#"
						r_buffer_clear();
						break;
					case 21571:							// "TC" TempCompCLEAR	54 43 21571
						temp_comp = 0;
						r_buffer_clear();
						break;
					case 21573:							// "TE" Temperature		54 45 21573
						strcat(ss_buffer, s_temp);		// "25.5#"
						r_buffer_clear();
						break;
					case 21575:							// "TG" TempCompGET		54 47 21575
						if (temp_comp)
						{
							strcat(ss_buffer, true_terminate);	// "1#"
						}
						else 
						{
							strcat(ss_buffer, false_terminate);	// "0#"
						}
						r_buffer_clear();
						break;
					case 21587:								// "TS" TempCompSET		54 53 21587
						temp_comp = 1;
						r_buffer_clear();
						break;
					default:
						r_buffer_clear();
						break;
				}
			}
			else if ((r_data > 0x2F && r_data < 0x3A) || r_data == 0x2D)	// 1, 0:30, 9:39, -:2D, byte 2, 숫자 또는 음수 기호(-))
			{
				link = 1;
				link_counter = 0;

				sum = make16(r_buffer[0],r_buffer[1]);
				switch(sum)
				{
					case 19785:							// "MI" MaxInc		4D 49 19785
						p_buffer_clear();
						p_buffer[p_length] = r_data;
						++p_length;
						++r_counter;
						command = MAXINCREMENT;
						break;
					case 19791:							// "MO" Move		4D 4F 19791
						p_buffer_clear();
						p_buffer[p_length] = r_data;
						++p_length;
						++r_counter;
						command = MOVE;
						motor_on();
						break;
					case 19795:							// "MS" MaxStep		4D 53 19795
						p_buffer_clear();
						p_buffer[p_length] = r_data;
						++p_length;
						++r_counter;
						command = MAXSTEP;
						break;
					case 21331:							// "SS" StepSize	53 53 21331
						p_buffer_clear();
						p_buffer[p_length] = r_data;
						++p_length;
						++r_counter;
						command = STEPSIZE;
						break;
					default:
						r_buffer_clear();
						break;
				}
			}
			else
			{
				r_buffer_clear();
			}
			break;

		case 3:
			if (r_data == 0x23)								// #:23, byte 3, terminate
			{
				if (p_buffer[0] != 0x2d)					// 0x2d : "-" 가 아니면
				{
					if (command == MOVE)
					{
						cmd_move_proc();
					}
					else if (command == MAXSTEP)
					{
						cmd_maxstep_proc();
					}
					else if (command == MAXINCREMENT)
					{
						cmd_maxinc_proc();
					}
					else if (command == STEPSIZE)
					{
						cmd_stepsize_proc();
					}
					r_buffer_clear();
				}
				else
				{
					r_buffer_clear();
				}
			}
			else if (r_data > 0x2F && r_data < 0x3A)		// 2, 0:30, 9:39, byte 3, 숫자
			{
				p_buffer[p_length] = r_data;
				++p_length;
				++r_counter;
			}
			else
			{
				r_buffer_clear();
			}
			break;

		case 4:
			if (r_data == 0x23)								// #:23, byte 4, terminate
			{
				if (command == MOVE)
				{
					cmd_move_proc();
				}
				else if (command == MAXSTEP)
				{
					cmd_maxstep_proc();
				}
				else if (command == MAXINCREMENT)
				{
					cmd_maxinc_proc();
				}
				else if (command == STEPSIZE)
				{
					cmd_stepsize_proc();
				}
			}
			else if (r_data > 0x2F && r_data < 0x3A)		// 3, 0:30, 9:39, byte 4, 숫자
			{
				p_buffer[p_length] = r_data;
				++p_length;
				++r_counter;
			}
			else
			{
				r_buffer_clear();
			}
			break;

		case 5:
			if (r_data == 0x23)								// #:23, byte 5, terminate
			{
				if (command == MOVE)
				{
					cmd_move_proc();
				}
				else if (command == MAXSTEP)
				{
					cmd_maxstep_proc();
				}
				else if (command == MAXINCREMENT)
				{
					cmd_maxinc_proc();
				}
				else if (command == STEPSIZE)
				{
					cmd_stepsize_proc();
				}
			}
			else if (r_data > 0x2F && r_data < 0x3A)		// 4, 0:30, 9:39, byte 5, 숫자
			{
				p_buffer[p_length] = r_data;
				++p_length;
				++r_counter;
			}
			else
			{
				r_buffer_clear();
			}
			break;

		case 6:
			if (r_data == 0x23)								// #:23, byte 6, terminate
			{
				if (command == MOVE)
				{
					cmd_move_proc();
				}
				else if (command == MAXSTEP)
				{
					cmd_maxstep_proc();
				}
				else if (command == MAXINCREMENT)
				{
					cmd_maxinc_proc();
				}
				else if (command == STEPSIZE)
				{
					cmd_stepsize_proc();
				}
			}
			else if (r_data > 0x2F && r_data < 0x3A)		// 5, 0:30, 9:39, byte 6, 숫자
			{
				p_buffer[p_length] = r_data;
				++p_length;
				++r_counter;
			}
			else
			{
				r_buffer_clear();
			}
			break;
		
		case 7:
			if (r_data == 0x23)								// #:23, byte 7, terminate
			{
				if (command == MOVE)
				{
					cmd_move_proc();
				}
				else if (command == MAXSTEP)
				{
					cmd_maxstep_proc();
				}
				else if (command == MAXINCREMENT)
				{
					cmd_maxinc_proc();
				}
				else if (command == STEPSIZE)
				{
					cmd_stepsize_proc();
				}
			}
			else if (r_data > 0x2F && r_data < 0x3A)		// 6, 0:30, 9:39, byte 7, 숫자
			{
				p_buffer[p_length] = r_data;
				++p_length;
				++r_counter;
			}
			else
			{
				r_buffer_clear();
			}
			break;

		case 8:
			if (r_data == 0x23)								// #:23, byte 8, terminate
			{
				if (command == MOVE)
				{
					cmd_move_proc();
				}
				else if (command == MAXSTEP)
				{
					cmd_maxstep_proc();
				}
				else if (command == MAXINCREMENT)
				{
					cmd_maxinc_proc();
				}
				else if (command == STEPSIZE)
				{
					cmd_stepsize_proc();
				}
			}
			else
			{
				r_buffer_clear();
			}
			break;

		default:											// 모른다
			r_buffer_clear();
			break;
	}
	INTCON1bits.NSTDIS = 0;		// Interrupt Nesting Disable bit, 1 = Interrupt nesting is disabled, 0 = Interrupt nesting is enabled
}

//************************************************************************************//
// UART1 TX ISR
//************************************************************************************//
/*
void __attribute__((__interrupt__, no_auto_psv)) _U1TXInterrupt(void)
{
	IFS0bits.U1TXIF = 0;		// Clear UART1 TX interrupt flag
}
*/

//************************************************************************************//
// UART1 Error ISR
//************************************************************************************//
void __attribute__((__interrupt__, no_auto_psv)) _U1ErrInterrupt(void)
{
	unsigned char dummy;
	
	if (U1STAbits.FERR == 1)	// frame err
	{ 
		dummy = U1RXREG;		// dummy read
	}

	if (U1STAbits.OERR == 1)	// overrun err
	{
		U1STAbits.OERR = 0;
	}

	IFS4bits.U1EIF = 0;			// Clear  UART1 Error Interrupt Flag Status bit
}

#endif
