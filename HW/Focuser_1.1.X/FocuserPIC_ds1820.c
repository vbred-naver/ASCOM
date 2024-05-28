/* FocuserPIC_ds1820.c" */
#ifdef _FOCUSER_

//************************************************************************************//
// ds18b20 reset
// 초기화 시퀀스 동안 버스 마스터는 1-Wire 버스를 최소 480μs 동안 로우로 풀링하여 리셋 펄스를 전송(Tx)합니다
//************************************************************************************//
void ds18b20_out_lo(void)
{
	TRIS_TEMP = 0;		// output
	TEMP = 0;
}

//************************************************************************************//
// ds18b20 reset
//************************************************************************************//
void ds18b20_out_hi_imp(void)
{
	TRIS_TEMP = 1;		// input
}

//************************************************************************************//
// temp write timing, 70us
//************************************************************************************//
void ds18b20_write(unsigned char data)
{
	unsigned char i;

	for (i=0;i<8;++i)
	{
		TRIS_TEMP = 0;
		TEMP = 0;
		__delay_us(3);				// 3u
		if(data & 0x01) TEMP = 1;
		data = data >> 1;
		__delay_us(60);				// 60u
		TRIS_TEMP = 1;
		__delay_us(7);				// 7u
	}
}

//************************************************************************************//
// temp read timing, 70us
//************************************************************************************//
unsigned char ds18b20_read(void)
{
	unsigned char i;
	unsigned char data = 0;
	unsigned char hi = 0x80;

	for (i=0;i<8;++i)
	{
		TRIS_TEMP = 0;
		TEMP = 0;
		__delay_us(5);			// 5u
		TRIS_TEMP = 1;
		__delay_us(15);			// 15u
		data = data >> 1;
		if (TEMP) data |= hi;
		__delay_us(50);			// 50u
	}
	return data;
}

//************************************************************************************//
// ds18b20 유무 검사
//************************************************************************************//
void ds18b20_check(void)
{
	__delay_us(100);
	if (TEMP) tErr = 1;
}

//************************************************************************************//
// ds18b20 cycle
//************************************************************************************//
void ds18b20(void)
{
	unsigned char dummy;

	if(moving)
	{
		t1_counter = 0;			// 모터 이동시에는 DS1820 정지, 카운터 리셋
	}
	else						// 모터 정지시에는 DS1820 동작
	{
		switch (t1_counter)
		{
			case 1:								// 리셋, minimum:480us
				tErr = 0;						// 
				ds18b20_out_lo();				// Master issues reset pulse
				break;
			case 2:								// 포트를 입력으로 전환한다, minimum:480us
				ds18b20_out_hi_imp();			// DS18S20 responds with presence pulse
				ds18b20_check();				// 있으면 0, 없으면 1
				break;
			case 3:
				if (!tErr)
				{
					ds18b20_write(0xCC);		// Master issues Skip ROM command
					ds18b20_write(0x44);		// Master issues Convert T command
				}
				break;
			case 11:							// Converts temperature in 750 ms (max)
				if (!tErr)
				{
					ds18b20_out_lo();			// Master issues reset pulse
				}
				break;
			case 12:
				if (!tErr)
				{
					ds18b20_out_hi_imp();
				}
				break;
			case 13:
				if (!tErr)
				{
					ds18b20_write(0xCC);		// Master issues Skip ROM command
					ds18b20_write(0xBE);		// Master issues Read Scratchpad command
				}	
				break;
			case 14:
				if (!tErr)
				{
					low_byte = ds18b20_read();	// temperature LSB, byte 0
					high_byte = ds18b20_read();	// temperature MSB, byte 1
				}
				break;
			case 15:
				if (high_byte == 0xff && low_byte == 0xff) tErr = 1;// error, 센서가 없을때

				if (!tErr)
				{
					dummy = ds18b20_read();		// TH Register or User Byte 1, byte 2
					dummy = ds18b20_read();		// TL Register or User Byte 2, byte 3
				}
				break;
			case 16:
				if (!tErr)
				{
					config_reg = 0xFF;				// 먼저 지우고 읽기, byte 4
					config_reg = ds18b20_read();	// Configuration Register, Resolution(table), 18b20, Reserved(FFh):18s20:1820
//					ds18s20_read();					// Reserved(FFh):18b20:18s20:1820
//					ds18s20_read();					// User Byte 3:18b20, COUNT REMAIN(0Ch):18s20:1820
//					ds18s20_read();					// User Byte 4:18b20, COUNT PER C(10h):18s20:1820
//					ds18s20_read();					// CRC:18b20:18s20:1820
				}

				break;
			case 17:
				if (tErr)	// error, 센서가 없을때
				{
					++t_err_counter;
					if (t_err_counter > 2)
					{
						memcpy(s_temp,fail_temp,3);
						t_err_counter = 0;
					}
				}
				else if(config_reg == 0x7F)						// 	18b20(12bit:7Fh)
				{
					resolution = 12;
					switch (c_temp_counter)
					{
						case 0:
							c_temp_1 = make16(high_byte,low_byte) / 16;
							++c_temp_counter;
							break;
						case 1:
							c_temp_2 = make16(high_byte,low_byte) / 16;
							if (c_temp_1 == c_temp_2)						// 동일 데이터가 2번이면 사용한다
							{
								t_err_counter = 0;

								c_temp = make16(high_byte,low_byte);
								if (c_temp < 0)
								{
									signed int div = c_temp / 16;
									unsigned char p = (~c_temp & 0x000f) + 1;
									if (div > -56)							// ds1820 125 ~ -55
									{
										itoa(div, s_temp);
										strcat(s_temp,dot);
										emptyTerminate[0] = t_table[p];
										strcat(s_temp,emptyTerminate);
									}
								}
								else
								{
									signed int div = c_temp / 16;
									unsigned char p = c_temp & 0x000f;
									if (div < 126)							// ds18b20 125 ~ -55
									{
										itoa(div, s_temp);
										strcat(s_temp,dot);
										emptyTerminate[0] = t_table[p];
										strcat(s_temp,emptyTerminate);
									}
								}
							}
							c_temp_counter = 0;
							break;
						default:
							c_temp_counter = 0;
							break;
					}
				}
				else											// 알수없음
				{
					++t_err_counter;
					if (t_err_counter > 2)
					{
						memcpy(s_temp,fail_temp,3);
						t_err_counter = 0;
					}
				}
				break;
			default:
				break;
		}
	}
}

#endif