/* FocuserPIC_motor.c" */
#ifdef _FOCUSER_

//************************************************************************************//
// motor Initialize
// TB6608FNG
//************************************************************************************//
void init_motor(void)
{
	MOTOR_CLK = 0;		// Motor Clock(output)
	MOTOR_DIR = 0;		// Motor Direction(output), L:CW
	MOTOR_EN = 0;		// Motor Enable(output), L:Disable
	MOTOR_RST = 0;		// Motor Reset(output), L:Reset
	__delay_us(5);
	MOTOR_RST = 1;		// Motor Reset(output), L:Reset
}

//************************************************************************************//
// Motor On
//************************************************************************************//
void motor_on(void)
{
	MOTOR_EN = 1;		// Motor Enable(output), L:Disable
}

//************************************************************************************//
// Motor Off
//************************************************************************************//
void motor_off(void)
{
	MOTOR_EN = 0;		// Motor Enable(output), L:Disable
}

//************************************************************************************//
// step driver
// TB6608FNG
//************************************************************************************//
void step_driver(void)
{
	if (absolute)	// plus only, MO200#, MO0#
	{
		if(r_posi > c_posi)							// 이동해야 할 값이 현재값보다 클때
		{
			if (max_step > c_posi)					// 절대값은 max_step 값보다 크면 안됨
			{
				MOTOR_DIR = 0;
				if(swing == 0)						// odd, 펄스제어
				{
					swing = 1;
					MOTOR_CLK = 1;
				}
				else
				{
					swing = 0;
					MOTOR_CLK = 0;
					++c_posi;						// 증가
					if (halt)
					{
						halt = 0;
						r_posi = c_posi;			// 목적지 도착
					}
				}
			}
			else
			{
				r_posi = c_posi;					// 완료
			}
		}
		else if(r_posi < c_posi)
		{
			if (c_posi > 0)							// 절대값은 0 보다 작으면 안됨
			{
				MOTOR_DIR = 1;
				if(swing == 0)		// odd
				{
					swing = 1;
					MOTOR_CLK = 1;
				}
				else
				{
					swing = 0;
					MOTOR_CLK = 0;
					--c_posi;
					if (halt)
					{
						halt = 0;
						r_posi = c_posi;
					}
				}
			}
			else
			{
				r_posi = c_posi;
			}
		}
		else
		{
			stability = 1;							// 안정화 시작
			motor_start = 0;						// step_driver 루프 종료
			eeprom_save_step(c_posi);				// 스텝값 저장
		}
	}
	else		// relative, MO200#, MO-200#
	{
		if(r_posi > 0)								// 상대값은 증가 감소가 무한, 위치값이 없음
		{
			MOTOR_DIR = 0;
			if(swing == 0)		// odd
			{
				swing = 1;
				MOTOR_CLK = 1;
			}
			else
			{
				swing = 0;
				MOTOR_CLK = 0;
				--r_posi;
				if (halt)
				{
					halt = 0;
					r_posi = 0;
				}
			}
		}
		else if(r_posi < 0)
		{
			MOTOR_DIR = 1;
			if(swing == 0)		// odd
			{
				swing = 1;
				MOTOR_CLK = 1;
			}
			else
			{
				swing = 0;
				MOTOR_CLK = 0;
				++r_posi;
				if (halt)
				{
					halt = 0;
					r_posi = 0;
				}
			}
		}
		else
		{
			stability = 1;
			motor_start = 0;
		}
	}
}

#endif
